using System;
using System.Collections.Generic;
using System.Text;

namespace HexCS.Core
{
    /// <summary>
    /// The type of tree search. Normally related to the order that
    /// things in the tree are searched
    /// </summary>
    public enum ETreeSearchType
    {
        /// <summary>
        /// Depth first search
        /// </summary>
        DEPTHFIRST,

        /// <summary>
        /// Bredth first search
        /// </summary>
        BREADTHFIRST
    }
}
