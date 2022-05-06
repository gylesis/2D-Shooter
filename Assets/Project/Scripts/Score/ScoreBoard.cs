using System;
using TMPro;
using UnityEngine;

namespace Project.Score
{
    public class ScoreBoard : MonoBehaviour, IScoreUpdateListener
    {
        [SerializeField] private TMP_Text _scoreText;

        private int _playerScore = 0;
        private int _botScore = 0;

        private void Awake()
        {
            UpdateScore(new ScoreUpdateContext());
        }

        public void UpdateScore(ScoreUpdateContext context)
        {
            if (context.Side == Side.Player)
            {
                _playerScore += context.Points;
            }
            else if (context.Side == Side.Bot)
            {
                _botScore += context.Points;
            }

            _scoreText.text = $"<color=green>{_playerScore}</color> : <color=red>{_botScore}</color>";
        }
    }
}