namespace Project.Bot
{
    public interface IState
    {
        BotState State { get; }
        void Enter();
        void Update();
        void Exit();
    }
}