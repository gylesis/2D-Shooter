using System;
using Fusion;
using Project.PlayerLogic;
using UnityEngine;

namespace Project
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        private Vector2 _moveVector;
        private int _ownerId;

        public Action<BulletDeathCollisionContext> CollisionDeath { get; set; }
        public Action<Bullet> Destroyed { get; set; }
        
        public void Setup(BulletContext context)
        {
            _ownerId = context.OwnerId;
            _moveVector = context.Force;
            transform.right = _moveVector.normalized;
        }

        public override void FixedUpdateNetwork()
        {
            var raycast = Runner.LagCompensation.Raycast(transform.position, transform.right, 10, Object.InputAuthority, out var hit);
           
            Debug.DrawRay(transform.position,transform.right * 5);
            
            if(raycast)
            {
                Debug.Log($"hit.Collider {hit.Collider}");
                Debug.Log($"hit.Hitbox {hit.Hitbox}");
                Debug.Log($"hit.HitType {hit.Type}");
                Debug.Log($"hit.hit.Hitbox.Root {hit.Hitbox.Root}");
                Debug.Log($"hit.hit.Hitbox.Root.Object {hit.Hitbox.Root.Object}");
            }
            
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
            if(other.gameObject.TryGetComponent(out Player player))
            {
                PlayerInfo playerInfo = player.PlayerInfo;

                Debug.Log($"collision id {playerInfo.Id}, owner id {_ownerId}");
                
                if(playerInfo.Id == _ownerId) return;
                
                var bulletDeathCollisionContext = new BulletDeathCollisionContext();
                
                bulletDeathCollisionContext.Bullet = this;
                bulletDeathCollisionContext.HitObject = other.gameObject;
                bulletDeathCollisionContext.PlayerId = _ownerId;
                
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
        public int PlayerId;
    }

    public struct BulletContext
    {
        public int OwnerId;
        public Vector2 Force;
    }
}