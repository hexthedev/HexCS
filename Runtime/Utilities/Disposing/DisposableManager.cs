using System;

namespace HexCS.Core
{
    /// <summary>
    /// Automatically handles disposing of a group of disposables
    /// when calling dispose
    /// </summary>
    public class DisposableManager : ADisposableManager
    {
        private Action _setDisposablesToNull;

        private string _accessAfterDisposalMessage;

        #region API
        /// <summary>
        /// Give an action to set disposables to null, and a message when a disposable is
        /// accessed after it's been disposed
        /// </summary>
        /// <param name="setDisposablesToNull"></param>
        /// <param name="accessAfterDisposalMessage"></param>
        public DisposableManager(Action setDisposablesToNull, string accessAfterDisposalMessage)
        {
            _setDisposablesToNull = setDisposablesToNull;
            _accessAfterDisposalMessage = accessAfterDisposalMessage;
        }

        /// <inheritdoc/>
        public void RegisterDisposable(IDisposable disposable)
        {
            RegisterInteralDisposable(disposable);
        }

        /// <inheritdoc/>
        public void UnregisterAndDispose(IDisposable disposable)
        {
            DisposeAndUnregisterDisposable(disposable);
        }

        /// <inheritdoc/>
        public new void ThrowErrorIfDisposed()
        {
            base.ThrowErrorIfDisposed();
        }
        #endregion

        /// <inheritdoc/>
        protected override string AccessAfterDisposalMessage => _accessAfterDisposalMessage;

        /// <inheritdoc/>
        protected override void SetDisposablesToNull()
        {
            _setDisposablesToNull?.Invoke();
        }
    }
}
