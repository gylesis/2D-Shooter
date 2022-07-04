using Fusion;
using UnityEngine;

namespace Project.PlayerLogic
{
    public class Player : NetworkBehaviour, IPlayerLeft
    {
        [SerializeField] private WeaponService _weaponService;

        [SerializeField] private float _speedMultiplier = 5f;
        [SerializeField] private float _rotationMultiplier = 6f;

        [SerializeField] private UnitController _controller;

        public PlayerInfo PlayerInfo { get; set; }
        
        private Camera _camera;
        private float _screenWidthWorld;
        private float _screenHeightWorld;

        public void Init(BulletSpawnService bulletSpawnService, Camera camera)
        {
            _camera = camera;
            _weaponService.Init(bulletSpawnService, Runner);
            
            var rightUp = new Vector2(Screen.width,Screen.height);
            var rightDown = new Vector2(Screen.width,0);
            var leftUp = new Vector2(0,Screen.height);
            var leftDown = new Vector2(0,0);

            Vector3 rightUpWorld = _camera.ScreenToWorldPoint(rightUp);
            Vector3 rightDownWorld = _camera.ScreenToWorldPoint(rightDown);
            Vector3 leftUpWorld = _camera.ScreenToWorldPoint(leftUp);
            Vector3 leftDownWorld = _camera.ScreenToWorldPoint(leftDown);

            _screenWidthWorld = (leftDownWorld - rightDownWorld).magnitude;
            _screenHeightWorld = (leftUpWorld - leftDownWorld).magnitude;
        }

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                _camera = FindObjectOfType<Camera>();
                _weaponService.Init(FindObjectOfType<BulletSpawnService>(), Runner);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                _controller.Move(data.Direction * _speedMultiplier * Runner.DeltaTime);

                if (data.FirePressed)
                {
                    _weaponService.Fire(transform.right);
                }

                Rotation(data);
            }
        }

        private void Update()
        {
            RestrictBounds();
        }
        
        private void Rotation(NetworkInputData data)
        {
            var rightArrowPressed = data.RightArrowPressed;
            var leftArrowPressed = data.LeftArrowPressed;

            var rotationZ = transform.rotation.eulerAngles.z;

            if (rightArrowPressed)
            {
                rotationZ += -_rotationMultiplier;
            }
            else if (leftArrowPressed)
            {
                rotationZ += _rotationMultiplier;
            }

            Quaternion quaternion = Quaternion.Euler(0, 0, rotationZ);

            _controller.Rotate(quaternion);
        }

        private void RestrictBounds()
        {
            Vector3 position = _camera.WorldToScreenPoint(transform.position);

            Vector3 playerPos = transform.position;
            
            if (position.x > Screen.width)
            {
                playerPos.x -= _screenWidthWorld + 1;
            }

            if (position.x < 0)
            {
                playerPos.x += _screenWidthWorld - 1;
            }

            if (position.y > Screen.height)
            {
                playerPos.y -= _screenHeightWorld + 1;
            }

            if (position.y < 0)
            {
                playerPos.y += _screenHeightWorld - 1;
            }

            transform.position = playerPos;
        }

        public void PlayerLeft(PlayerRef player)
        {
            if (Object.HasInputAuthority)
            {
                Runner.Despawn(Object);
            }
        }
    }
}