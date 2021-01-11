using System.Collections.Generic;

namespace HexCS.Core
{
    /// <summary>
    /// Used in a using statement to make it easier to construct arrays.
    /// Create garbage during the creation process. 
    /// </summary>
    public class ArrayBuilder<T> : ADisposableManager
    {
        private List<T> _construction = new List<T>();

        /// <inheritdoc/>
        protected override string AccessAfterDisposalMessage => $"Trying to {nameof(ArrayBuilder<T>)} constructor after disposal";

        /// <inheritdoc/>
        protected override void SetDisposablesToNull()
        {
            _construction = null;
        }

        #region API
        /// <summary>
        /// Append element to the array
        /// </summary>
        /// <param name="elements">The element you want to append</param>
        public void AppendToArray(params T[] elements)
        {
            _construction.AddRange(elements);
        }

        /// <summary>
        /// Returns to constructed array as a new array
        /// </summary>
        /// <returns>The constructed array</returns>
        public T[] ToArray()
        {
            return _construction.ToArray();
        }
        #endregion
    }
}
