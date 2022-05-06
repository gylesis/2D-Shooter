using UnityEngine;

namespace Project
{
    public class BulletSpawnService
    {
        private readonly Bullet _bulletPrefab;
       
        private readonly BulletsCollisionHandler _bulletsCollisionHandler;

        public BulletSpawnService(Bullet bulletPrefab, BulletsCollisionHandler bulletsCollisionHandler)
        {
            _bulletsCollisionHandler = bulletsCollisionHandler;
            _bulletPrefab = bulletPrefab;
        }

        public Bullet Spawn(Vector2 origin)
        {
            Bullet bullet = Object.Instantiate(_bulletPrefab, origin, Quaternion.identity);

            _bulletsCollisionHandler.Register(bullet);

            return bullet;
        }
       
    }

    public interface IUpdatable
    {
        void Update();
    }
}