using System;
using Project.Score;
using UnityEngine;

namespace Project.Bot
{
    public class PlayerZone : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;    

        public Action<PlayerZone> PlayerStayed { get; set; }
        

        public void Highlight(Color color)
        {
            color.a = 0.4f;
            _spriteRenderer.color = color;
        }
        
    }
}