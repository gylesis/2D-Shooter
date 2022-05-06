using System.Collections;
using UnityEngine;

namespace Project
{
    public class VisualisationService : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Color _startColor;

        [SerializeField] private bool _isInvulnerable;

        private void Awake()
        {
            _startColor = _spriteRenderer.color;
        }

        public void OnInvulnerable()
        {
            _isInvulnerable = true;
            StartCoroutine(InvulnerableIndicatorCoroutine());
        }

        public void OnVulnerable()
        {
            _isInvulnerable = false;
        }

        public void OnDamaged()
        {
           // StartCoroutine(DamageIndicatorCoroutine());
        }

        private IEnumerator InvulnerableIndicatorCoroutine()
        {
            while (_isInvulnerable)
            {
                while (_spriteRenderer.color != Color.grey)
                {
                    if (_isInvulnerable == false)
                    {
                        ResetColor();
                        yield break;
                    }

                    _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.grey, 0.5f);
                    yield return null;
                }

                while (_spriteRenderer.color != _startColor)
                {
                    if (_isInvulnerable == false)
                    {
                        ResetColor();
                        yield break;
                    }

                    _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, _startColor, 0.5f);
                    yield return null;
                }
            }
        }

        private IEnumerator DamageIndicatorCoroutine()
        {
            while (_spriteRenderer.color != Color.red)
            {
                _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.red, 0.3f);
                yield return null;
            }

            while (_spriteRenderer.color != _startColor)
            {
                _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, _startColor, 0.3f);
                yield return null;
            }
        }

        private void ResetColor()
        {
            _spriteRenderer.color = _startColor;
        }
    }
}