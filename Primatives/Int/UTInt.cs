namespace HexCS.Core
{
    /// <summary>
    /// Utilities for ints
    /// </summary>
    public static class UTInt
    {
        /// <summary>
        /// Perform a that works negatively
        /// </summary>
        /// <param name="target">thing to mod</param>
        /// <param name="modby">number to mod by</param>
        /// <returns>mod of number</returns>
        public static int CircularMod(this int target, int modby)
        {
            if (target < 0)
            {
                int mod = (target * -1) % modby;
                return modby - mod;
            }

            return target % modby;
        }
    }
}
