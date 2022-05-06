using UnityEngine;

namespace Project.Bot
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private CellData _data;

        public CellData Data => _data;

        public void Init(CellData data)
        {
            _data = data;
        }

        public void Highlight()
        {
            _spriteRenderer.color = Color.black;
        }
    }
}