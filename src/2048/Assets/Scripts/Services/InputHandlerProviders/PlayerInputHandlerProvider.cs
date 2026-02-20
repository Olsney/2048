using Gameplay.Input;

namespace Services.InputHandlerProviders
{
    public class PlayerInputHandlerProvider : IPlayerInputHandlerProvider
    {
        private IPlayerInputEvents _playerInputEvents;

        public IPlayerInputEvents Get() => 
            _playerInputEvents;

        public void Set(IPlayerInputEvents playerInputEvents) => 
            _playerInputEvents = playerInputEvents;
    }
}
