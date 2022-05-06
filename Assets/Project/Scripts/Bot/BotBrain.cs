using System;
using Project.PlayerLogic;
using UnityEngine;

namespace Project.Bot
{
    public class BotBrain : MonoBehaviour
    {
        [SerializeField] private UnitController _unitController;
        [SerializeField] private InvulnerabilityService _invulnerabilityService;
        
        private StateMachine _stateMachine;
        private PlayerDestinationService _playerDestinationService;

        private float _timer;
        private WeaponService _weaponService;

        public void Init(BotMovementService botMovementService, PlayerDestinationService playerDestinationService,
            BotWaypointsService botWaypointsService, WeaponService weaponService)
        {
            _weaponService = weaponService;
            _playerDestinationService = playerDestinationService;
            
            var patrolState = new PatrolState(transform, botMovementService, botWaypointsService, _unitController);
            var chaseState = new ChaseState(transform, botMovementService, playerDestinationService, _unitController,
                weaponService);

            _stateMachine = new StateMachine(patrolState, chaseState);

            _stateMachine.ChangeState(BotState.Patrol);

            _invulnerabilityService.Invulnerable += OnInvulnerable;
            
            _weaponService.BulletHit += OnBulletHit;
        }

        private void OnInvulnerable()
        {
            OnBulletHit();
        }

        private void OnBulletHit()
        {
            _stateMachine.ChangeState(BotState.Patrol);
            _stateMachine.LockChangingState(3);
        }

        private void Update()
        {
            if (_stateMachine == null) return;

            _stateMachine.Update();

            _timer += Time.deltaTime;

            if (_timer <= 0.5f) return;
            _timer = 0;

            if ((_playerDestinationService.Player.transform.position - transform.position).sqrMagnitude < 65f)
            {
                if(_invulnerabilityService.IsInvulnerable) return;
                
                _stateMachine.ChangeState(BotState.Chase);
            }
        }

        private void OnDestroy()
        {
            _weaponService.BulletHit -= OnBulletHit;
            _invulnerabilityService.Invulnerable -= OnInvulnerable;
        }
    }
}