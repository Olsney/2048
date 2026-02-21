using System;

namespace Data
{
    public class WorldData : IWorldData
    {
        public event Action Changed;
        
        public int Score { get; private set; }

        public void AddScore(int score)
        {
            Score += score;
            
            Changed?.Invoke();
        }

        public void Reset()
        {
            if (Score == 0)
                return;

            Score = 0;
            Changed?.Invoke();
        }
    }
}
