using Gameplay.Input;

namespace Services.InputHandlerProviders
{
    public interface IPlayerInputHandlerProvider
    {
        IPlayerInputEvents Get();
        void Set(IPlayerInputEvents playerInputEvents);
    }
}
