using System.Linq;
using Project.Bot;
using UnityEngine;

namespace Project.PlayerLogic
{
    public class PlayerSpawnService : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private SpawnPoint[] _points;

        public void Init(Camera camera,BulletSpawnService bulletSpawnService, params IPlayerSpawnedListener[] playerSpawnedListeners)
        {
            Transform availablePoint = GetAvailablePoint();

            Player player = Instantiate(_player, availablePoint.position, Quaternion.identity);

            player.Init(bulletSpawnService, camera);

            foreach (IPlayerSpawnedListener listener in playerSpawnedListeners)
                listener.PlayerSpawned(player);
        }

        public Transform GetAvailablePoint()
        {
            var spawnPoints = _points.Where(x => x.IsBusy == false).ToList();

            return spawnPoints[Random.Range(0, _points.Length - 1)].transform;
        }
    }
}