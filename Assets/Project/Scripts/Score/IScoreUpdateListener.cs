namespace Project.Score
{
    public interface IScoreUpdateListener
    {
        void UpdateScore(ScoreUpdateContext context);
    }

    public enum Side
    {
        Player,
        Bot
    }

    public struct ScoreUpdateContext
    {
        public Side Side;
        public int Points;
    }
}