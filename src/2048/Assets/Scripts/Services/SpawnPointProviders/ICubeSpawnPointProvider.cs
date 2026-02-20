using Gameplay.Cubes;

namespace Services.SpawnPointProviders
{
    public interface ICubeSpawnPointProvider
    {
        CubeSpawnPoint Instance { get; set; }
    }
}