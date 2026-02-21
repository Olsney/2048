using Gameplay.Cubes;
using UI;

namespace UI.Elements
{
    public sealed class CubeValuePresenter : Presenter<ICubeValueView>
    {
        private readonly Cube _cube;

        public CubeValuePresenter(ICubeValueView view, Cube cube) : base(view)
        {
            _cube = cube;
        }

        protected override void OnInitialize()
        {
            _cube.ValueUpdated += OnValueUpdated;
        }

        protected override void OnDispose()
        {
            _cube.ValueUpdated -= OnValueUpdated;
        }

        private void OnValueUpdated() =>
            View.SetValue(_cube.Value);
    }
}
