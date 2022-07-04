using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Project.Bot;
using Project.PlayerLogic;
using Project.Score;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class EntryPoint : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private PlayerSpawnService _playerSpawnService;
        [SerializeField] private Camera _camera;
        [SerializeField] private ScoreBoard _scoreBoard;
        [SerializeField] private Bullet _bulletPrefab;

        [SerializeField] private StartPanel _startPanel;

        [SerializeField] private BulletSpawnService _bulletSpawnService;
        [SerializeField] private Player _player;

        private float _timer;
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();

        private NetworkBool _gameIsOn;
        private ScoreUpdater _scoreUpdater;
        private NetworkRunner _runner;

        private Dictionary<PlayerRef, Player> _spawnedPlayers = new Dictionary<PlayerRef, Player>();
        
        private void Awake()
        {
            _startPanel.HostGame.AddListener((() => StartGame(GameMode.Host)));
            _startPanel.ConnectGameEvent.AddListener((() => StartGame(GameMode.Client)));
        }

        async void StartGame(GameMode mode)
        {
            if (_runner == null)
            {
                gameObject.TryGetComponent<NetworkRunner>(out var networkRunner);

                if (networkRunner == null)
                    _runner = gameObject.AddComponent<NetworkRunner>();
                else
                    _runner = networkRunner;
            }

            _runner.ProvideInput = true;

            OnGameStart();
            
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "GameRoom",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneObjectProvider = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

        private void OnGameStart()
        {
            _gameIsOn = true;

            List<IScoreUpdateListener> scoreUpdateListeners = new List<IScoreUpdateListener>();

            scoreUpdateListeners.Add(_scoreBoard);

            _scoreUpdater = new ScoreUpdater(scoreUpdateListeners.ToArray(), _runner);
            var bulletsCollisionHandler = new BulletsCollisionHandler(_camera, _scoreUpdater);

            _bulletSpawnService.Init(_bulletPrefab, bulletsCollisionHandler);
            _updatables.Add(bulletsCollisionHandler);
        }

        private void Update()
        {
            if (_gameIsOn == false) return;

            _timer += Time.time;

            if (_timer > 0.2f)
            {
                _updatables.ForEach(x => x.Update());
                _timer = 0;
            }
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("Connected to server");
            
            OnPlayerSpawn(runner, runner.LocalPlayer);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log("OnPlayerJoined");
            OnPlayerSpawn(runner, player);
        }

        private void OnPlayerSpawn(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                Transform availablePoint = _playerSpawnService.GetAvailablePoint();
                Player playerInstance = runner.Spawn(_player, availablePoint.position, Quaternion.identity, player);
                playerInstance.Init(_bulletSpawnService, _camera);

                _spawnedPlayers.Add(player, playerInstance);

                var playerInfo = new PlayerInfo();

                playerInfo.Id = player.PlayerId;
                playerInfo.Name = $"Player{_spawnedPlayers.Count + 1}";

                playerInstance.PlayerInfo = playerInfo;
                
                _scoreUpdater.RegisterPlayer(playerInfo);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();

            Vector2 moveDirection = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                moveDirection.y += 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveDirection.x -= 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                moveDirection.y -= 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                moveDirection.x += 1;
            }

            moveDirection.Normalize();

            data.Direction = moveDirection;
            data.FirePressed = Input.GetKey(KeyCode.Space);
            data.RightArrowPressed = Input.GetKey(KeyCode.RightArrow);
            data.LeftArrowPressed  = Input.GetKey(KeyCode.LeftArrow);

            input.Set(data);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }


        public void OnDisconnectedFromServer(NetworkRunner runner) { }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token) { }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

        public void OnSceneLoadDone(NetworkRunner runner) { }

        public void OnSceneLoadStart(NetworkRunner runner) { }
    }
}