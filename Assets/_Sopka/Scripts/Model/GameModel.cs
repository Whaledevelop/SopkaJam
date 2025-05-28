namespace Sopka
{
    public class GameModel : IGameModel
    {
        public GameEvents GameEvents { get; } = new GameEvents();

        public TopDownModel TopDownModel { get; } = new TopDownModel();
        public MapModel MapModel { get; } = new MapModel();
        public DialogModel DialogModel { get; } = new DialogModel();
    }
}