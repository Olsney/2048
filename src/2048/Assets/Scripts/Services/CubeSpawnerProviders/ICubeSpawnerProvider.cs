using Gameplay.Cubes.Spawner;

namespace Services.CubeSpawnerProviders
{
    public interface ICubeSpawnerProvider
    {
        CubeSpawner Instance { get; set; }
    }
}