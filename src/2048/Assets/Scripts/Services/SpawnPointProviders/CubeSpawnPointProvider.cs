using Gameplay.Cubes;

namespace Services.SpawnPointProviders
{
    public class CubeSpawnPointProvider : ICubeSpawnPointProvider
    {
        public CubeSpawnPoint Instance { get; set; }
    }
}