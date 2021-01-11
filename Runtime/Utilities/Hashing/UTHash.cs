using System;

namespace HexCS.Core
{
    /// <summary>
    /// Utilities to do with Hashes (objects reduced to unique numbers)
    /// </summary>
    public static class UTHash
    {
        /// <summary>
        /// Uses algorithm found here:
        /// https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        /// 
        /// Gives a hash using all passed in objects
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static int BasicHash(params object[] objects)
        {
            unchecked
            {
                int hash = 17;

                foreach (object o in objects)
                {
                    hash = hash * 37 + o.GetHashCode();
                }

                return hash;
            }
        }

        /// <summary>
        /// Comparison based on hash of two objects
        /// </summary>
        /// <param name="ob1"></param>
        /// <param name="ob2"></param>
        /// <returns></returns>
        public static int HashComparison(object ob1, object ob2)
        {
            int hash1 = ob1.GetHashCode();
            int hash2 = ob2.GetHashCode();

            return hash1 == hash2 ? 0 : Math.Sign(hash1 - hash2);
        }
    }
}
