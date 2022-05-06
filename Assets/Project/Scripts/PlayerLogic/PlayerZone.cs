using System;
using Project.Score;
using UnityEngine;

namespace Project.Bot
{
    public class PlayerZone : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;    

        public Action<PlayerZone> PlayerStayed { get; set; }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if(other.TryGetComponent<MyTag>(out var tag))
            {
                if (tag.Side == Side.Player)
                {
                    PlayerStayed.Invoke(this);
                }
            }
        }

        public void Highlight(Color color)
        {
            color.a = 0.4f;
            _spriteRenderer.color = color;
        }
        
    }
}