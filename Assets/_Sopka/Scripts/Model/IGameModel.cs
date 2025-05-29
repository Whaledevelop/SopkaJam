namespace Sopka
{
    public interface IGameModel
    {
        public GameEvents GameEvents { get; }

        public TopDownModel TopDownModel { get; }
        
        public MapModel MapModel { get; }
        
        public DialogModel DialogModel { get; } 
        
        // С сеттерами. По-хорошему перенести все эти сеттеры в инсталлер
        public ResourcesModel ResourcesModel { get; set; }
    }
}