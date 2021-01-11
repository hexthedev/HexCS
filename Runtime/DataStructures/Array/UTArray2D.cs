﻿using System;
using System.Collections.Generic;

namespace HexCS.Core
{
    /// <summary>
    /// Utility methods used for dealing with two dimensional arrays
    /// </summary>
    public static class UTArray2D
    {
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
    }
}