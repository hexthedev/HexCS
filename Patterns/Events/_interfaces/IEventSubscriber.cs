using System;

namespace HexCS.Core
{
    /// <summary>
    /// Allows Event to be subscribed or unsubscribed
    /// </summary>
    public interface IEventSubscriber
    {
        /// <summary>
        /// Subscribe to the event
        /// </summary>
        /// <param name="callback"></param>
        EventBinding Subscribe(Action callback);

        /// <summary>
        /// Subscribe to the event for one time use. 
        /// The event binding can be used to resubscribe for one time use
        /// </summary>
        /// <param name="callback"></param>
        EventBinding SubscribeSingleUse(Action callback);

        /// <summary>
        /// Unsubscribe to the event
        /// </summary>
        /// <param name="callback"></param>
        void Unsubscribe(Action callback);
    }

    /// <summary>
    /// Allows Event to be subscribed or unsubscribed
    /// </summary>
    public interface IEventSubscriber<T>
    {
        /// <summary>
        /// Subscribe to the event
        /// </summary>
        /// <param name="callback"></param>
        EventBinding Subscribe(Action<T> callback);

        /// <summary>
        /// Subscribe to the event for one time use. 
        /// The event binding can be used to resubscribe for one time use
        /// </summary>
        /// <param name="callback"></param>
        EventBinding SubscribeSingleUse(Action<T> callback);

        /// <summary>
        /// Unsubscribe to the event
        /// </summary>
        /// <param name="callback"></param>
        void Unsubscribe(Action<T> callback);
    }
}
