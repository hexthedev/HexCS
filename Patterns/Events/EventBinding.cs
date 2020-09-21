using System;

namespace HexCS.Core
{
    /// <summary>
    /// Encapsulated the subscribe/unsubscribe actions of a particular function
    /// All event binding actions should occur through this object, because it
    /// tracks whether an action is bound or not
    /// </summary>
    public class EventBinding
    {
        private Action _subscribe;
        private Action _unsubscribe;

        private bool _isSubscribed = false;

        /// <summary>
        /// Create event binding by providing functions to subscribe/unsubscribe events
        /// </summary>
        /// <param name="subscribe"></param>
        /// <param name="unsubscribe"></param>
        public EventBinding(Action subscribe, Action unsubscribe)
        {
            _subscribe = subscribe;
            _unsubscribe = unsubscribe;
        }

        /// <summary>
        /// Subscribe the event binding (if not subscribed)
        /// </summary>
        public void Subscribe()
        {
            if (!_isSubscribed)
            {
                _subscribe?.Invoke();
                _isSubscribed = true;
            }
        }

        /// <summary>
        /// Forces binding to unsubscribe old reference to funciton if exists
        /// then resubscribes
        /// </summary>
        public void ForceFreshSubscription()
        {
            _unsubscribe?.Invoke();
            _subscribe?.Invoke();
            _isSubscribed = true;
        }

        /// <summary>
        /// Unsubscribe the event binding (if currently subscribed)
        /// </summary>
        public void UnSubscribe()
        {
            if (_isSubscribed)
            {
                _unsubscribe?.Invoke();
                _isSubscribed = false;
            }
        }
    }
}
