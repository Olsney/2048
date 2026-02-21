using Data;
using Services.GameRestart;

namespace UI.Windows
{
    public sealed class DefeatWindowPresenter : Presenter<IDefeatWindowView>
    {
        private readonly IWorldData _worldData;
        private readonly IGameRestartService _gameRestartService;

        public DefeatWindowPresenter(IDefeatWindowView view, IWorldData worldData, IGameRestartService gameRestartService) : base(view)
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
            View.SetMessage($"Your score: \n{_worldData.Score}");

        private void OnRestartRequested()
        {
            _gameRestartService.Restart();
            View.Close();
        }
    }
}
