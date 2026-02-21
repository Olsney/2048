using Data;
using Services.GameRestart;

namespace UI.Windows
{
    public sealed class VictoryWindowPresenter : Presenter<IVictoryWindowView>
    {
        private readonly IWorldData _worldData;
        private readonly IGameRestartService _gameRestartService;

        public VictoryWindowPresenter(IVictoryWindowView view, IWorldData worldData, IGameRestartService gameRestartService) : base(view)
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
            View.SetMessage($"You won! Your score is + \n{_worldData.Score}");

        private void OnRestartRequested()
        {
            _gameRestartService.Restart();
            View.Close();
        }
    }
}
