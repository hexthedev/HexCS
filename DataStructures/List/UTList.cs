using System;
using System.Collections.Generic;
using System.Linq;

namespace HexCS.Core
{
    /// <summary>
    /// Contains utility methods to facilitate common operations on List datastructures
    /// </summary>
    public static class UTList
    {
        /// <summary>
        /// Returns a copy of a list
        /// </summary>
        /// <param name="to_copy">List ot copy</param>
        /// <typeparam name="T">List type</typeparam>
        /// <returns>Copy of to_copy</returns>
        public static List<T> ShallowCopy<T>(this List<T> to_copy)
        {
            return to_copy.ToList();
        }

        /// <summary>
        /// Returns objet at location using index, but if index is greater than list.count, will find index as if list was circular
        /// </summary>
        /// <param name="list">List to get element from</param>
        /// <param name="index">Index, can be negative</param>
        /// <typeparam name="T">list type</typeparam>
        /// <returns>Object at position</returns>
        public static T GetAtIndexCircular<T>(this List<T> list, int index)
        {
            if (list.Count == 0)
            {
                throw new IndexOutOfRangeException("Cannot get element at index for list of count 0");
            }

            if (list.Count == 1)
            {
                return list[0];
            }

            if (index >= 0)
            {
                return list[index % list.Count];
            }
            else
            {
                // % in C# will act like this: 
                // -5 % 4 = -1. It's a normal mod without sign, then adds the sign of the original
                // so the mod of a negative represents to the steps backwards in the list
                return list[list.Count + index % list.Count];
            }
        }

        /// <summary>
        /// Returns target minus any shared items with test. 
        /// </summary>
        /// <typeparam name="T">List type</typeparam>
        /// <param name="target">The enumerable to return without tests elements</param>
        /// <param name="test">The enumerable whose elements will be removed from target in the reture list</param>
        /// <returns>Enumerable target without shared items in test. If target contains multiple elements, and that element exists in test once, all will be removed</returns>
        public static List<T> Difference<T>(this List<T> target, List<T> test)
        {
            List<T> list = new List<T>();

            foreach (T item in target)
            {
                if (!test.Contains(item))
                {
                    list.Add(item);
                }
            }

            return list;
        }
    }
}