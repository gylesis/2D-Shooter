using System;
using Project.Score;
using UnityEngine;

namespace Project.PlayerLogic
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _power = 15f;
        public float Cooldown => _cooldown;

        private BulletSpawnService _bulletSpawnService;

        public void Init(BulletSpawnService bulletSpawnService)
        {
            _bulletSpawnService = bulletSpawnService;
        }

        public void Fire(Vector2 direction, Side side, Action bulletHit = null)
        {
            Bullet bullet = _bulletSpawnService.Spawn(_pivot.position);

            var bulletContext = new BulletContext();

            bulletContext.Force = direction * _power;
            bulletContext.SenderSide = side;

            bullet.Setup(bulletContext);

            if (bulletHit != null)
            {
                bullet.CollisionDeath += BulletCollisionDeath;
                bullet.Destroyed += Destroyed;
                
                void BulletCollisionDeath(BulletDeathCollisionContext context) => bulletHit.Invoke();
                void Destroyed(Bullet bussllet)
                {
                    bussllet.CollisionDeath -= BulletCollisionDeath;
                    bussllet.Destroyed -= Destroyed;
                }
            }
        }
    }
}