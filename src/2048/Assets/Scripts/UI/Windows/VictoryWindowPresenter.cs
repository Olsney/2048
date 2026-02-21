using Data;

namespace UI.Windows
{
    public sealed class VictoryWindowPresenter : Presenter<IVictoryWindowView>
    {
        private readonly IWorldData _worldData;

        public VictoryWindowPresenter(IVictoryWindowView view, IWorldData worldData) : base(view)
        {
            _worldData = worldData;
        }

        protected override void OnInitialize()
        {
            View.CloseRequested += OnCloseRequested;
            _worldData.Changed += OnWorldDataChanged;
            OnWorldDataChanged();
        }

        protected override void OnDispose()
        {
            View.CloseRequested -= OnCloseRequested;
            _worldData.Changed -= OnWorldDataChanged;
        }

        private void OnWorldDataChanged() =>
            View.SetMessage($"You won! Your score is + \n{_worldData.Score}");

        private void OnCloseRequested() =>
            View.Close();
    }
}
