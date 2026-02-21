namespace Services.Randoms
{
    public interface IRandomService
    {
        int GetRandomPowerOfTwoValue();
        float Next(float min, float max);
    }
}
