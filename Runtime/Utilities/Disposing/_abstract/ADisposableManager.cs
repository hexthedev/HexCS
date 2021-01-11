using System;
using System.Collections.Generic;

namespace HexCS.Core
{
    /// <summary>
    /// Abstract class that allows disposables to be registered for automatic 
    /// disposable when main class is disposed
    /// </summary>
    public abstract class ADisposableManager : IDisposableResource
    {
        /// <summary>
        /// The message that will be displayed with ObjectDisposedException if CheckDisposedError
        /// called when disposed
        /// </summary>
        protected abstract string AccessAfterDisposalMessage { get; }

        private List<IDisposable> _disposables = new List<IDisposable>();

        #region API
        /// <inheritdoc/>
        public bool IsDisposed { get; private set; } = false;

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }

            SetDisposablesToNull();

            IsDisposed = true;
        }
        #endregion

        /// <summary>
        /// If the class has been disposed throws exception
        /// </summary>
        protected void ThrowErrorIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(AccessAfterDisposalMessage);
            }
        }

        /// <summary>
        /// Register a disposable that will be disposed on call to Dispose()
        /// </summary>
        /// <param name="disposable"></param>
        protected void RegisterInteralDisposable(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        /// <summary>
        /// Dispose then deregister a disposable
        /// </summary>
        /// <param name="disposable"></param>
        protected void DisposeAndUnregisterDisposable(IDisposable disposable)
        {
            disposable.Dispose();
            _disposables.Remove(disposable);
        }

        /// <summary>
        /// Called during dispose to set dispoables to null
        /// </summary>
        protected abstract void SetDisposablesToNull();
    }
}
