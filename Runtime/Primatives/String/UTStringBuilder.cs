using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexCS.Core
{
    /// <summary>
    /// String builder utilities
    /// </summary>
    public static class UTStringBuilder
    {

        /// <summary>
        /// Appends to a StringBuilder a series of strings based on the collection and the resolver.
        /// This was created to easily add , separated and \r\n separated lists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sb">The StringBuilder to build to</param>
        /// <param name="collection">The collection to use when building strings</param>
        /// <param name="resolver">The action to perform on each element</param>
        /// <param name="separator">The separator to separate them by</param>
        public static void AppendCharacterSeparatedCollection<T>(
            this StringBuilder sb, 
            IEnumerable<T> collection, 
            Action<StringBuilder, T> resolver,
            string separator)
        {
            int count = collection.Count();
            int index = 0;

            foreach(T element in collection)
            {
                resolver(sb, element);

                if(index < count - 1)
                {
                    sb.Append(separator);
                }

                index++;
            }
        }
    }
}
