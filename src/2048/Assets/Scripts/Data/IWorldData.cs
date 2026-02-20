using System;

namespace Data
{
    public interface IWorldData
    {
        int Score { get; }
        void AddScore(int score);
        event Action Changed;
    }
}