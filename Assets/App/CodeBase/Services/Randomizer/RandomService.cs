using CodeBase.SO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CodeBase.Services.Randomizer
{
    public class RandomService : IRandomService
    {
        public int Next(int minValue, int maxValue)
        {
            return Random.Range(minValue, maxValue);
        }

        public float Next(float minValue, float maxValue)
        {
            return Random.Range(minValue, maxValue);
        }

        public int NextBetweenZeroAndHundred()
        {
            return Random.Range(0, 101);
        }
    }
}