using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project
{
    public class StartPanel : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [SerializeField] private CanvasGroup _canvasGroup;  
        
        public Action GameStart { get; set; }

        private void Awake()
        {
            _button.onClick.AddListener((StartGame));
        }

        private void StartGame()
        {
            GameStart.Invoke();

            _button.enabled = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void RestartGame()
        {
            Debug.Log("restart");
            SceneManager.LoadScene(0);
        }
        
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
        
        
        
    }
}