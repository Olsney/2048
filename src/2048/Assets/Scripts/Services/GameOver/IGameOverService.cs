using Gameplay.Cubes;

namespace Services.GameOver
{
    public interface IGameOverService
    {
        void TryFinish(Cube cube);
    }
}