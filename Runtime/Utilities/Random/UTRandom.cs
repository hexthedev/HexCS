using System;

namespace HexCS.Core
{
    /// <summary>
    /// Utilties for gerating random numbers
    /// </summary>
    public static class UTRandom
    {
        private static Lazy<Random> _random = new Lazy<Random>(() => new Random());

        /// <summary>
        /// Returns a random number between 0 (inclusive) and 1 (exclusive)
        /// </summary>
        /// <returns>random number 0 - 1 exclusive of 1</returns>
        public static double Percentage()
        {
            return _random.Value.NextDouble();
        }

        /// <summary>
        /// Returns a random number between 0 (inclusive) and 1 (exclusive)
        /// </summary>
        /// <returns>random number 0 - 1 exclusive</returns>
        public static float PercentageF()
        {
            return (float)Percentage();
        }

        /// <summary>
        /// Tests float against a random percentage between 0 and 1. If float is greater than
        /// or equal to percentage, return true. Otherwise return false;
        /// </summary>
        /// <param name="test">float to test</param>
        /// <returns>result of random test</returns>
        public static bool TestPercentage(float test)
        {
            return test >= PercentageF();
        }

        /// <summary>
        /// Returns a float between 0 and max (inclusive)
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Float(float max = float.MaxValue)
        {
            return PercentageF() * max;
        }
        
        /// <summary>
        /// Returns an int between 0 and max (exclusive). Max must be > 0
        /// </summary>
        /// <param name="max">max int to random (exclusive)</param>
        /// <returns>random int between 0 and max (exclusive)</returns>
        public static int Int(int max = int.MaxValue)
        {
            return _random.Value.Next(max);
        }

        /// <summary>
        /// Returns an int between 0 and max (exclusive). Max must be > 0
        /// </summary>
        /// <param name="min">max int to random (inclusive)</param>
        /// <param name="max">max int to random (exclusive)</param>
        /// <returns>random int between min (inclusive) and max (exclusive)</returns>
        public static int Int(int min, int max)
        {
            return _random.Value.Next(min, max);
        }

        /// <summary>
        /// Get a Bool with a percentage change of being true
        /// </summary>
        /// <param name="chanceTrue">number between 0 and 1 chance of returning true</param>
        /// <returns>random bool</returns>
        public static bool Bool(double chanceTrue = 0.5)
        {
            return Percentage() <= chanceTrue;
        }

        /// <summary>
        /// Returns a random byte array
        /// </summary>
        public static byte[] Bytes(int length)
        {
            byte[] bytes = new byte[length];
            _random.Value.NextBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Returns a random uppercase or lowercase letter
        /// </summary>
        public static char Letter()
        {
            // 50/50 upper or lower
            if (Bool())
            {
                int min = (int)'A';
                int max = (int)'Z';

                return (char)Int(min, max + 1);
            }
            else
            {
                int min = (int)'a';
                int max = (int)'z';

                return (char)Int(min, max + 1);
            }
        }
    }
}
