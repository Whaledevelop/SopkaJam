namespace Sopka
{
    public interface IGameModel
    {
        public TopDownModel TopDownModel { get; }
        
        public MapModel MapModel { get; }
        
        public DialogModel DialogModel { get; } 
        
        public ResourcesModel ResourcesModel { get; }
    }
}