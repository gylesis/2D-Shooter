using Project.PlayerLogic;
using UnityEngine;

namespace Project.Bot
{
    public class ChaseState : IState
    {
        public BotState State => BotState.Chase;

        private readonly Transform _transform;
        private readonly BotMovementService _botMovementService;
        private readonly PlayerDestinationService _playerDestinationService;
        private readonly UnitController _unitController;
        private readonly WeaponService _weaponService;

        private bool _exit;
        
        public ChaseState(Transform transform, BotMovementService botMovementService,
            PlayerDestinationService playerDestinationService, UnitController unitController,
            WeaponService weaponService)
        {
            _weaponService = weaponService;
            _unitController = unitController;
            _playerDestinationService = playerDestinationService;
            _botMovementService = botMovementService;
            _transform = transform;
        }

        public async void Enter()
        {
            _exit = true;
            while (_exit)
            {
                var botMoveContext = new BotMoveContext();
                
                botMoveContext.Origin = _transform.position;
                botMoveContext.Speed = 15f;
                botMoveContext.TargetPos = _playerDestinationService.PlayerZone.transform.position;
                botMoveContext.Unit = _unitController;
                
                await _botMovementService.Move(botMoveContext);
            }
        }

        public void Update()
        {
            /*Vector3 direction = (_transform.position - _playerDestinationService.Player.transform.position).normalized;

            var angle = -Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg;

            Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            _unitController.Rotate(lookRotation);

            _weaponService.Fire(_transform.right);*/
        }

        public void Exit()
        {
            _exit = false;
        }
    }
}