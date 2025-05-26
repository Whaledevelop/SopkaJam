namespace Sopka
{
    public class GameModel : IGameModel
    {
        public GameEvents GameEvents { get; } = new GameEvents();

        public PlayerModel PlayerModel { get; } = new PlayerModel();
    }
}