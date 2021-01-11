using System;

namespace HexCS.Core
{
    /// <summary>
    /// Utilities and extensions for floats
    /// </summary>
    public static class UTFloat
    {
        /// <summary>
        /// Clamps the float between two values. min must be less thna or equal to max
        /// </summary>
        /// <param name="target">float to target</param>
        /// <param name="min">minimum value</param>
        /// <param name="max">maximum value</param>
        /// <returns>clamped float</returns>
        public static float Clamp(this float target, float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentException("Min cannot be greater than max");
            }

            if (target < min)
            {
                return min;
            }

            if (target > max)
            {
                return max;
            }

            return target;
        }
    }
}
