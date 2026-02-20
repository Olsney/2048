using UnityEngine;

namespace Services.Inputs
{
    public interface IInputService
    {
        bool IsPointerDown();
        bool IsPointerUp();
        Vector2 GetPointerPosition();
    }
}