using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace Project.Score
{
    public class ScoreUpdater 
    {
        private readonly IScoreUpdateListener[] _scoreUpdateListeners;

        private Dictionary<PlayerInfo, int> _playersScore = new Dictionary<PlayerInfo, int>();
        private NetworkRunner _runner;

        public ScoreUpdater(IScoreUpdateListener[] scoreUpdateListeners, NetworkRunner runner)
        {
            _runner = runner;
            _scoreUpdateListeners = scoreUpdateListeners;
        }
        
        public void RegisterPlayer(PlayerInfo playerInfo)
        {
            _playersScore.Add(playerInfo, 0);
        }
        
        public void AddPoints(int playerId, int points = 1)
        {
            var scoreUpdateContext = new ScoreUpdateContext();

            PlayerInfo player1 = _playersScore.Keys.FirstOrDefault(x => x.Id == playerId);
            PlayerInfo player2 = _playersScore.Keys.FirstOrDefault(x => x.Id != playerId);
    
            if(player1 == null || player2 == null)
            {
                Debug.LogError("Player not found");
                return;
            }
            
            _playersScore[player1] += points;
            
            scoreUpdateContext.Player1 = player1;
            scoreUpdateContext.Player2 = player2;
            scoreUpdateContext.Points1 = _playersScore[player1];
            scoreUpdateContext.Points2 = _playersScore[player2];
            
          //  _runner.SendRpc();
            UpdateScore(scoreUpdateContext);
        }
        
        private void UpdateScore(ScoreUpdateContext context)
        {
            foreach (IScoreUpdateListener scoreUpdateListener in _scoreUpdateListeners)
                scoreUpdateListener.UpdateScore(context);
        }
    }
}