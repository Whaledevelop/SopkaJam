using Whaledevelop.Reactive;

namespace Sopka
{
    public class GameEvents
    {
        public readonly ReactiveCommand InitializePlayerCommand = new();
        public readonly ReactiveCommand<bool> SetInputModeCommand = new();
    }
}