using System;
using UnityEngine;

namespace Gameplay.Input
{
    public interface IPlayerInputEvents
    {
        event Action<Vector2> TapStarted;
        event Action<Vector2> TapEnded;

        Vector2 PointerPosition();
    }
}
