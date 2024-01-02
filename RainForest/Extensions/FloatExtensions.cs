namespace RainForest.Extensions
{
    public static class FloatExtensions
    {
        public static float Clamp(this float value, float low, float high)
        {
            return value > high ? high : (value < low ? low : value);
        }
    }
}
