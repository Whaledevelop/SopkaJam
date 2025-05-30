namespace Sopka
{
    public class GameModel : IGameModel
    {
        public TopDownModel TopDownModel { get; } = new();
        public MapModel MapModel { get; } = new();
        public DialogModel DialogModel { get; } = new();
        
        public ResourcesModel ResourcesModel { get; set; }

        public GameModel(ModelStartSettings settings)
        {
            ResourcesModel = new ResourcesModel(settings.ResourcesModel);
        }
    }
}