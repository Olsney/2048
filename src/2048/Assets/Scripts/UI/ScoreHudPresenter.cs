using Data;
using UI.Elements;

namespace UI
{
    public sealed class ScoreHudPresenter : Presenter<IScoreHudView>
    {
        private readonly IWorldData _worldData;

        public ScoreHudPresenter(IScoreHudView view, IWorldData worldData) : base(view)
        {
            _worldData = worldData;
        }

        protected override void OnInitialize()
        {
            _worldData.Changed += OnWorldDataChanged;
            OnWorldDataChanged();
        }

        protected override void OnDispose()
        {
            _worldData.Changed -= OnWorldDataChanged;
        }

        private void OnWorldDataChanged() =>
            View.SetScore(_worldData.Score);
    }
}
