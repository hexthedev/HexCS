using System;
using System.Collections;
using System.Collections.Generic;

namespace HexCS.Core
{
    /// <summary>
    /// Utility methods used for dealing with two dimensional arrays
    /// </summary>
    public static class UTArray2D
    {
        public static int FlatLength<T>(this T[,] target) 
            => target.GetLength(0) * target.GetLength(1);

        public static T AtFlatIndex<T>(this T[,] target, int index)
        {
            int xLength = target.GetLength(0);
            return target[index % xLength, index / xLength];
        }

        public static void SetFlatIndex<T>(this T[,] target, int index, T newValue)
        {
            int xLength = target.GetLength(0);
            target[index % xLength, index / xLength] = newValue;
        }

        /// <summary>
        /// Constructs a 2D array where each element is populated by the output
        /// of a single call to the construction function
        /// </summary>
        /// <typeparam name="T">array type</typeparam>
        /// <param name="size">size of the array</param>
        /// <param name="constuctionFunction">Function that constructs 1 T</param>
        /// <returns>Constructed array</returns>
        public static T[,] ConstructArray<T>(DiscreteVector2 size, Func<T> constuctionFunction)
        {
            T[,] array = new T[size.X, size.Y];

            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    array[x, y] = constuctionFunction();
                }
            }

            return array;
        }

        // T-MODIFY: This does not need a 2D array to be calculated. It's a simple calculation over a grid.
        // This funciton should be removed and it's implementation should apply to some kind of grid object instead. 
        // A 2D array should not be treated as a grid. Grids have extra properties. 
        /// <summary>
        /// Returns an array of neighbour indices that are neighbouring the target index
        /// A neighbour is n steps away from the index. In a 5x5 grid, the 1 step neighbours of the center index
        /// would be all indices that are not on the edge of the grid. 2 step neighbours would be ALL indcies excepts the original index
        /// Note: Neighbours can overlap and appear twice, neighbours are circular (the neighbour of coord (0,0) will be (width-1, height-1)).
        /// If this is not desired, set nonNegativeOnly to true
        /// </summary>
        /// <typeparam name="T">Aray type</typeparam>
        /// <param name="target">Target array</param>
        /// <param name="index">Index to get neighbours of</param>
        /// <param name="steps">steps away from index to collect neighbours</param>
        /// <param name="nonNegativeOnly">This mode only returns neighbours that are non negative x or y steps away. This is useful if you are
        /// iterating over a grid and trying to avoid duplicate neighbours</param>
        /// <returns>Array of gird coordinates that </returns>
        public static DiscreteVector2[] GetNeighbours<T>(this T[,] target, DiscreteVector2 index, int steps, bool nonNegativeOnly = false)
        {
            // get the size of the 2D array
            // T-IMPLEMENT
            DiscreteVector2 size = new DiscreteVector2(target.GetLength(0), target.GetLength(1));

            List<DiscreteVector2> neighbours = new List<DiscreteVector2>();

            int loopStart = nonNegativeOnly ? 0 : -steps;

            for (int x = loopStart; x <= steps; x++)
            {
                for (int y = loopStart; y <= steps; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    neighbours.Add(new DiscreteVector2((index.X + x).CircularMod(size.X), (index.Y + y).CircularMod(size.Y)));
                }
            }

            return neighbours.ToArray();
        }

        public static DiscreteVector2 RandomIndex<T>(this T[,] target)
        {
            int x = UTRandom.Int(0, target.GetLength(0));
            int y = UTRandom.Int(0, target.GetLength(1));
            return new DiscreteVector2(x, y);
        }
        
        public static T RandomElement<T>(this T[,] target)
        {
            DiscreteVector2 coord = target.RandomIndex();
            return target[coord.X, coord.Y];
        }
        
        public static IEnumerable<DiscreteVector2> EnumerateIndices<T>(this T[,] target)
            => new IndexEnumerator2D(target.GetLength(0), target.GetLength(1));

        public static T At<T>(this T[,] target, DiscreteVector2 index)
            => target[index.X, index.Y];

        public static void Set<T>(this T[,] target, DiscreteVector2 index, T value)
            => target[index.X, index.Y] = value;
        
        public static bool IsValidIndex<T>(this T[,] target, DiscreteVector2 index)
            => index.X != -1 && index.Y != -1 && index.X < target.GetLength(0) && index.Y < target.GetLength(1);

        public static DiscreteVector2 Size<T>(this T[,] target) 
            => (target.GetLength(0), target.GetLength(1));

        /// <summary>
        /// Copies data from src to dst only if there is space in dst. i.e. if dst is smaller than src not all
        /// data will be copied 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <typeparam name="T"></typeparam>
        public static void CopyWhatFits<T>(this T[,] src, T[,] dst)
        {
            int blockWidth = src.GetLength(0) <= dst.GetLength(0) 
                ? src.GetLength(0) : dst.GetLength(0);
            
            int blockHeight = src.GetLength(1) <= dst.GetLength(1) 
                ? src.GetLength(1) : dst.GetLength(1);
            
            for (int x = 0; x < blockWidth; x++)
            {
                for (int y = 0; y < blockHeight; y++)
                    dst[x, y] = src[x, y];
            }
        }
    }
    
    public class IndexEnumerator2D : IEnumerator<DiscreteVector2>, IEnumerable<DiscreteVector2>, IEnumerable
    {
        private int d0;
        private int d1;
        
        private int cur0 = -1;
        private int cur1;

        object IEnumerator.Current => Current;
        public DiscreteVector2 Current
        {
            get => new DiscreteVector2(cur0, cur1);
        }

        public IndexEnumerator2D(int dim0Length, int dim1Length)
        {
            d0 = dim0Length;
            d1 = dim1Length;
        }
            
        public bool MoveNext()
        {
            cur0++;

            if (cur0 == d0)
            {
                cur0 = 0;
                cur1++;
            }

            return cur0 < d0 && cur1 < d1;
        }

        public void Reset()
        {
            cur0 = -1;
            cur1 = 0;
        }

        public void Dispose() { }

        public IEnumerator<DiscreteVector2> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}