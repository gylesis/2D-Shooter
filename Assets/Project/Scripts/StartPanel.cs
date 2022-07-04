using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project
{
    public class StartPanel : MonoBehaviour
    {
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        public UnityEvent HostGame { get; set; } = new UnityEvent();

        public UnityEvent ConnectGameEvent { get; set; } = new UnityEvent();
        
        private void Awake()
        {
            _hostButton.onClick.AddListener(StartGame);
            _playButton.onClick.AddListener(ConnectGame);
        }

        private void ConnectGame()
        {
            ConnectGameEvent?.Invoke();
            
            _hostButton.enabled = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void StartGame()
        {
            HostGame.Invoke();

            _hostButton.enabled = false;
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
            _hostButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
            
            HostGame.RemoveAllListeners();
            ConnectGameEvent.RemoveAllListeners();
        }
        
    }
}