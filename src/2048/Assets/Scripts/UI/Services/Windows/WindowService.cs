using System;
using UI.Factory;

namespace UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowType windowType)
        {
            switch (windowType)
            {
                case WindowType.Unknown:
                    break;
                case WindowType.VictoryWindow:
                    _uiFactory.CreateVictoryWindow();
                    break;
                case WindowType.DefeatWindow:
                    _uiFactory.CreateDefeatWindow();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(windowType), windowType, null);
            }
        }
    }
}
