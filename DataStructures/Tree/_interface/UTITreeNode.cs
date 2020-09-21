using System;
using System.Collections.Generic;

namespace HexCS.Core
{
    /// <summary>
    /// Utilites for ITreeNodes
    /// </summary>
    public static class UTITreeNode
    {
        /// <summary>
        /// Returns the most similar ITreeNode in the candidate ienumerable
        /// </summary>
        /// <typeparam name="T">tree type</typeparam>
        /// <param name="target">target tree</param>
        /// <param name="candidates">candidate trees</param>
        /// <returns>most similar tree</returns>
        public static ITreeNode<T> MostSimilar<T>(this ITreeNode<T> target, IEnumerable<ITreeNode<T>> candidates) where T : IEquatable<T>
        {
            ITreeNode<T> mostSimilar = null;

            int similarity = -1;

            foreach (ITreeNode<T> candidate in candidates)
            {
                int testSim = target.Similarity(candidate);

                if (testSim > similarity)
                {
                    similarity = testSim;
                    mostSimilar = candidate;
                }
            }

            return mostSimilar;
        }

        /// <summary>
        /// Returns a DEquality delegate that tests the basic .Equal() of two ITreeNodes
        /// </summary>
        /// <typeparam name="T">tree type</typeparam>
        /// <returns>Basic DEqualityDeletgate</returns>
        public static DEquality<ITreeNode<T>> BasicValueEquality<T>() where T : IEquatable<T>
        {
            return (n1, n2) => n1.Value.Equals(n2.Value);
        }
    }
}
