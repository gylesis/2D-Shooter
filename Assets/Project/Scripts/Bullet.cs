using System;
using Project.Score;
using UnityEngine;

namespace Project
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        private Vector2 _moveVector;
        private Side _senderSide;

        public Action<BulletDeathCollisionContext> CollisionDeath { get; set; }
        public Action<Bullet> Destroyed { get; set; }
        
        public void Setup(BulletContext context)
        {
            _moveVector = context.Force;
            _senderSide = context.SenderSide;
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _moveVector;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            var isObstacle = other.collider.CompareTag("Obstacle");

            if (isObstacle) 
                Reflect(other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.TryGetComponent<MyTag>(out var tag))
            {
                if(tag.Side == _senderSide) return;
                
                var bulletDeathCollisionContext = new BulletDeathCollisionContext();
                bulletDeathCollisionContext.SenderSide = _senderSide;
                bulletDeathCollisionContext.Bullet = this;
                bulletDeathCollisionContext.HitObject = other.gameObject;
                
                CollisionDeath?.Invoke(bulletDeathCollisionContext);
            }
        }

        private void Reflect(Collision2D other)
        {
            if(other.contacts.Length < 1) return;
            Vector2 normal = other.contacts[0].normal;

            Vector2 reflect = Vector2.Reflect(_moveVector,normal);

            _moveVector = reflect ;
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
        }
    }

    public struct BulletDeathCollisionContext
    {
        public GameObject HitObject;
        public Bullet Bullet;
        public Side SenderSide;
    }

    public struct BulletContext
    {
        public Vector2 Force;
        public Side SenderSide;
    }
}