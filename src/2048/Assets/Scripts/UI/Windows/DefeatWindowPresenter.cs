using Data;

namespace UI.Windows
{
    public sealed class DefeatWindowPresenter : Presenter<IDefeatWindowView>
    {
        private readonly IWorldData _worldData;

        public DefeatWindowPresenter(IDefeatWindowView view, IWorldData worldData) : base(view)
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
            View.SetMessage($"Your score: \n{_worldData.Score}");

        private void OnCloseRequested() =>
            View.Close();
    }
}
