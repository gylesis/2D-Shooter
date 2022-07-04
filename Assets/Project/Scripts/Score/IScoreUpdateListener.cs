namespace Project.Score
{
    public interface IScoreUpdateListener
    {
        void UpdateScore(ScoreUpdateContext context);
    }

    public struct ScoreUpdateContext
    {
        public PlayerInfo Player1;
        public PlayerInfo Player2;

        public int Points1;
        public int Points2;
    }
}