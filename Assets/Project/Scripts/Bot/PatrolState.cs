using System.Threading.Tasks;
using Project.PlayerLogic;
using UnityEngine;

namespace Project.Bot
{
    public class PatrolState : IState
    {
        private readonly Transform _transform;
        private readonly BotMovementService _botMovementService;
        private readonly BotWaypointsService _botWaypointsService;
        private readonly UnitController _unitController;
        private Waypoint _waypoint;
        public BotState State => BotState.Patrol;

        private bool _exit;
        private float _timer;

        public PatrolState(Transform transform, BotMovementService botMovementService,
            BotWaypointsService botWaypointsService, UnitController unitController)
        {
            _unitController = unitController;
            _botWaypointsService = botWaypointsService;
            _botMovementService = botMovementService;
            _transform = transform;
        }

        public async void Enter()
        {
            _exit = false;

            while (_exit == false)
            {
                _waypoint = _botWaypointsService.GetPoint(_transform.position);

                var moveContext = new BotMoveContext();

                moveContext.Origin = _transform.position;
                moveContext.Speed = 15f;
                moveContext.TargetPos = _waypoint.transform.position;
                moveContext.Unit = _unitController;

                await _botMovementService.Move(moveContext);

                await Task.Delay(Random.Range(500, 1000));
            }
        }

        public void Update()
        {
            _timer += Time.deltaTime;

            if (_timer < 2f) return;

            _timer = 0;

            var insideUnitCircle = (Vector3) Random.insideUnitCircle + _transform.position;

            Vector3 direction = (_transform.position - insideUnitCircle).normalized;

            var angle = -Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg;

            Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            _unitController.Rotate(lookRotation);
        }

        public void Exit()
        {
            _exit = true;
        }
    }
}