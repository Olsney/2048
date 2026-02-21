using Gameplay.Cubes;
using UnityEngine;

namespace Services.Merge
{
    public interface IMergeService
    {
        void Merge(Cube first, Cube second, Vector3 mergeEffectPosition);
    }
}
