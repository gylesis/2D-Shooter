using Fusion;
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

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void UpdateScore(ScoreUpdateContext context) 
        {
//            Debug.Log("Update");
            
            string player1Name = "Player1";
            string player2Name = "Player2";
            
            if (context.Player1 != null)    
            {
                player1Name = context.Player1.Name;
                player2Name = context.Player2.Name;
            }
           
            _scoreText.text = $"<color=green>{player1Name} : {context.Points1}</color>\n<color=red>{player2Name} : {context.Points2}</color>";
        }
        
    }
    
    
}