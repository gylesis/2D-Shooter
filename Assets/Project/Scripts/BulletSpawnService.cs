using Fusion;
using UnityEngine;

namespace Project
{
    public class BulletSpawnService : NetworkBehaviour
    {
        private Bullet _bulletPrefab;
       
        private BulletsCollisionHandler _bulletsCollisionHandler;

        public void Init(Bullet bulletPrefab, BulletsCollisionHandler bulletsCollisionHandler)
        {
            _bulletsCollisionHandler = bulletsCollisionHandler;
            _bulletPrefab = bulletPrefab;
        }

        public Bullet Spawn(NetworkRunner runner ,Vector2 origin)
        {
            Bullet bullet = runner.Spawn(_bulletPrefab, origin, Quaternion.identity, runner.LocalPlayer);

            _bulletsCollisionHandler.Register(bullet);

            return bullet;
        }
    }

    public interface IUpdatable
    {
        void Update();
    }
}