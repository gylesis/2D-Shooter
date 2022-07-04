using System;
using Fusion;
using UnityEngine;

namespace Project.PlayerLogic
{
    public class WeaponService : MonoBehaviour
    {
        [SerializeField] private Weapon[] _weapons;

        private Weapon _currentWeapon;
        private DateTime _lastShootTime;

        public Action BulletHit;
        private NetworkRunner _runner;

        public void Init(BulletSpawnService bulletSpawnService,NetworkRunner networkRunner)
        {
            _runner = networkRunner;

            Debug.Log(_runner.LocalPlayer.PlayerId);
            
            foreach (Weapon weapon in _weapons)
                weapon.Init(bulletSpawnService);

            _currentWeapon = _weapons[0];
        }

        public void Fire(Vector2 direction)
        {
            TimeSpan dateTime = DateTime.Now - _lastShootTime;

            if (dateTime.TotalSeconds > _currentWeapon.Cooldown)
            {
                _currentWeapon.Fire(direction, _runner, BulletHit);
                _lastShootTime = DateTime.Now;
            }
        }
    }
}