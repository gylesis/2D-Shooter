using System.Collections.Generic;
using Project.Score;
using UnityEngine;

namespace Project
{
    public class BulletsCollisionHandler : IUpdatable
    {
        private readonly List<Bullet> _bullets = new List<Bullet>();
        private readonly ScoreUpdater _scoreUpdater;
        private readonly Camera _camera;

        public BulletsCollisionHandler(Camera camera, ScoreUpdater scoreUpdater)
        {
            _camera = camera;
            _scoreUpdater = scoreUpdater;
        }

        public void Register(Bullet bullet)
        {
            bullet.CollisionDeath += OnCollisionDeathHandle;
            _bullets.Add(bullet);
        }

        public void Update()
        {
            for (var i = _bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _bullets[i];
                var isDead = CheckForDeath(bullet);

                if (isDead)
                    _bullets.Remove(bullet);
            }
        }

        private void OnCollisionDeathHandle(BulletDeathCollisionContext context)
        {
            context.Bullet.CollisionDeath -= OnCollisionDeathHandle;

            if (context.SenderSide == Side.Player)
                _scoreUpdater.AddPlayerPoint();

            if (context.SenderSide == Side.Bot)
                _scoreUpdater.AddBotPoint();

            _bullets.Remove(context.Bullet);
            GameObject hitObject = context.HitObject;

            hitObject.GetComponent<VisualisationService>().OnDamaged();
            hitObject.GetComponent<InvulnerabilityService>().MakeInvulnerable();

            Object.Destroy(context.Bullet.gameObject);
        }

        private bool CheckForDeath(Bullet bullet)
        {
            Vector3 screenPos = _camera.WorldToScreenPoint(bullet.transform.position);

            if (screenPos.x > Screen.width || screenPos.x < 0 || screenPos.y > Screen.height || screenPos.y < 0)
            {
                Object.Destroy(bullet.gameObject);
                return true;
            }

            return false;
        }
    }
}