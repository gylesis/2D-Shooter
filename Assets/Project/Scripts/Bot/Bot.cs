using Project.PlayerLogic;
using UnityEngine;

namespace Project.Bot
{
    public class Bot : MonoBehaviour
    {
        [SerializeField] private BotBrain _botBrain;

        [SerializeField] private WeaponService _weaponService;
        
        public void Init(BotMovementService botMovementService, PlayerDestinationService playerDestinationService, BotWaypointsService botWaypointsService, BulletSpawnService bulletSpawnService)
        {
            _weaponService.Init(bulletSpawnService);
            _botBrain.Init(botMovementService ,playerDestinationService, botWaypointsService , _weaponService);
        }

    }

    public struct BotMoveContext
    {
        public float Speed;
        public UnitController Unit;
        public Vector3 Origin;
        public Vector3 TargetPos;
    }
}