using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Bot
{
    public class BotSpawnService : MonoBehaviour
    {
        [SerializeField] private Bot _botPrefab;
        [SerializeField] private SpawnPoint[] _spawnPoints;
        [SerializeField] private BotWaypointsService _botWaypointsService;

        public async void Init(BotMovementService botMovementService, PlayerDestinationService playerDestinationService,
            BulletSpawnService bulletSpawnService)
        {
            var spawnPoints = _spawnPoints.Where(x => x.IsBusy == false).ToList();

            SpawnPoint spawnPoint = spawnPoints[Random.Range(0, _spawnPoints.Length - 1)];
            spawnPoint.IsBusy = true;

            Bot bot = Instantiate(_botPrefab, spawnPoint.transform.position, Quaternion.identity);

            //bot.Init(botMovementService, playerDestinationService, _botWaypointsService, bulletSpawnService);

            await Task.Delay(2000);

            spawnPoint.IsBusy = false;
        }
    }
}