namespace Project.Score
{
    public class ScoreUpdater 
    {
        private readonly IScoreUpdateListener[] _scoreUpdateListeners;

        public ScoreUpdater(IScoreUpdateListener[] scoreUpdateListeners)
        {
            _scoreUpdateListeners = scoreUpdateListeners;
        }
        
        public void AddPlayerPoint(int points = 1)
        {
            var scoreUpdateContext = new ScoreUpdateContext();

            scoreUpdateContext.Side = Side.Player;
            scoreUpdateContext.Points = points;
            
            UpdateScore(scoreUpdateContext);
        }
        
        public void AddBotPoint(int points = 1)
        {
            var scoreUpdateContext = new ScoreUpdateContext();

            scoreUpdateContext.Side = Side.Bot;
            scoreUpdateContext.Points = points;
            
            UpdateScore(scoreUpdateContext);
        }

        private void UpdateScore(ScoreUpdateContext context)
        {
            foreach (IScoreUpdateListener scoreUpdateListener in _scoreUpdateListeners)
                scoreUpdateListener.UpdateScore(context);
        }
    }
}