using System;

namespace HexCS.Core
{
    /// <summary>
    /// Disposable with extensions for TobiasCS
    /// </summary>
    public interface IDisposableResource : IDisposable
    {
        /// <summary>
        /// Has the object been Disposed
        /// </summary>
        bool IsDisposed { get; }
    }
}
