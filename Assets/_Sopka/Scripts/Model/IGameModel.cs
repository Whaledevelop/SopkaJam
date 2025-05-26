namespace Sopka
{
    public interface IGameModel
    {
        public GameEvents GameEvents { get; }

        public PlayerModel PlayerModel { get; }
    }
}