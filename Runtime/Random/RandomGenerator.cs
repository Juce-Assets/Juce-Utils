namespace Juce.Utils.Random
{
    public class RandomGenerator
    {
        private readonly System.Random random = new System.Random();

        public int GetInt()
        {
            return random.Next(int.MinValue, int.MaxValue);
        }

        public int GetInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public float GetFloat()
        {
            double range = (double)float.MaxValue - (double)float.MinValue;
            double sample = random.NextDouble();
            double scaled = (sample * range) + float.MinValue;

            return (float)scaled;
        }

        public float GetFloat(float min, float max)
        {
            float range = min - max;
            double sample = random.NextDouble();
            double scaled = (sample * range) + float.MinValue;

            return (float)scaled;
        }
    }
}