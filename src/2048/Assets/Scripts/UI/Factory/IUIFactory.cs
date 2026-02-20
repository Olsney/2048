using UnityEngine;

namespace UI.Factory
{
    public interface IUIFactory
    {
        GameObject CreateUIRoot();
        GameObject CreateHud();
        GameObject CreateVictoryWindow();
        GameObject CreateLoseWindow();
    }
}