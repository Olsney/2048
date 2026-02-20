using UnityEngine;

namespace Infrastructure
{
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        private void Start() => 
            DontDestroyOnLoad(this);
    }
}