namespace Sopka
{
    public interface IGameModel
    {
        public GameEvents GameEvents { get; }

        public TopDownModel TopDownModel { get; }
        
        public MapModel MapModel { get; }
    }
}