using System;
using Project.Score;
using UnityEngine;

namespace Project.PlayerLogic
{
    public class WeaponService : MonoBehaviour
    {
        [SerializeField] private Weapon[] _weapons;
        [SerializeField] private Side _side;

        private Weapon _currentWeapon;
        private DateTime _lastShootTime;

        public Action BulletHit;
        
        public void Init(BulletSpawnService bulletSpawnService)
        {
            foreach (Weapon weapon in _weapons)
                weapon.Init(bulletSpawnService);

            _currentWeapon = _weapons[0];
        }

        public void Fire(Vector2 direction)
        {
            TimeSpan dateTime = DateTime.Now - _lastShootTime;

            if (dateTime.TotalSeconds > _currentWeapon.Cooldown)
            {
                _currentWeapon.Fire(direction, _side, BulletHit);
                _lastShootTime = DateTime.Now;
            }
        }
    }
}