using Data;
using Services.GameRestart;
using UI.Elements;

namespace UI
{
    public sealed class ScoreHudPresenter : Presenter<IScoreHudView>
    {
        private readonly IWorldData _worldData;
        private readonly IGameRestartService _gameRestartService;

        public ScoreHudPresenter(IScoreHudView view, IWorldData worldData, IGameRestartService gameRestartService) : base(view)
        {
            _worldData = worldData;
            _gameRestartService = gameRestartService;
        }

        protected override void OnInitialize()
        {
            View.RestartRequested += OnRestartRequested;
            _worldData.Changed += OnWorldDataChanged;
            OnWorldDataChanged();
        }

        protected override void OnDispose()
        {
            View.RestartRequested -= OnRestartRequested;
            _worldData.Changed -= OnWorldDataChanged;
        }

        private void OnWorldDataChanged() =>
            View.SetScore(_worldData.Score);

        private void OnRestartRequested() =>
            _gameRestartService.Restart();
    }
}
