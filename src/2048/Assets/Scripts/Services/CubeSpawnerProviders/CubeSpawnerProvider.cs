using Gameplay.Cubes.Spawner;

namespace Services.CubeSpawnerProviders
{
    public class CubeSpawnerProvider : ICubeSpawnerProvider
    {
        public CubeSpawner Instance { get; set; }
    }
}