using System.Collections.Generic;
using Project.Bot;
using Project.PlayerLogic;
using Project.Score;
using UnityEngine;

namespace Project
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerSpawnService _playerSpawnService;
        [SerializeField] private Camera _camera;
        [SerializeField] private ScoreBoard _scoreBoard;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private PathGrid _grid;
        [SerializeField] private BotSpawnService _botSpawnService;
        [SerializeField] private PlayerDestinationService _playerDestinationService;

        [SerializeField] private StartPanel _startPanel;

        private float _timer;
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();

        private bool _gameIsOn;

        private void Awake()
        {
            var gridCellsConditionChecker = new GridCellsConditionChecker(_camera);

            _grid.Init(gridCellsConditionChecker);

            _startPanel.GameStart += OnGameStart;
        }

        private void OnGameStart()
        {
            _gameIsOn = true;

            _playerDestinationService.Init();
            var pathfindingService = new BotMovementService(_grid);

            List<IScoreUpdateListener> scoreUpdateListeners = new List<IScoreUpdateListener>();

            scoreUpdateListeners.Add(_scoreBoard);

            var scoreUpdater = new ScoreUpdater(scoreUpdateListeners.ToArray());
            var bulletsCollisionHandler = new BulletsCollisionHandler(_camera, scoreUpdater);
            var bulletSpawnService = new BulletSpawnService(_bulletPrefab, bulletsCollisionHandler);

            _updatables.Add(bulletsCollisionHandler);
            _botSpawnService.Init(pathfindingService, _playerDestinationService, bulletSpawnService);
            _playerSpawnService.Init(_camera, bulletSpawnService, _playerDestinationService);
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
    }
}