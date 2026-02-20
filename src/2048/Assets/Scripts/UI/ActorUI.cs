using Data;
using UI.Elements;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private ScoreView _scoreView;

        private IWorldData _worldData;

        [Inject]
        public void Construct(IWorldData worldData)
        {
            _worldData = worldData;
            
            _worldData.Changed += OnWorldDataChanged;
            OnWorldDataChanged();
        }

        private void OnWorldDataChanged() => 
            _scoreView.SetValue(_worldData.Score);

        private void Start() => 
            OnWorldDataChanged();


        private void OnDestroy() => 
            _worldData.Changed -= OnWorldDataChanged;
    }
}