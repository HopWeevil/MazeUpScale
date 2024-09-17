using CodeBase.SO;

namespace CodeBase.Services.Randomizer
{
    public interface IRandomService
    {
        int Next(int minValue, int maxValue);
        float Next(float minValue, float maxValue);
        int NextBetweenZeroAndHundred();
    }
}