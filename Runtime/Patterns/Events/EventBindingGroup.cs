using System.Collections.Generic;

namespace HexCS.Core
{
    /// <summary>
    /// Collection of event bindings that is useful for subscribing, unsubscribing
    /// many bindings at once
    /// </summary>
    public class EventBindingGroup : List<EventBinding>
    {
        /// <summary>
        /// Subscribes all event bindings.
        /// </summary>
        public void SubscribeAll()
        {
            foreach (EventBinding binding in this)
            {
                binding.Subscribe();
            }
        }

        /// <summary>
        /// Unsubscribes all event bindings
        /// </summary>
        public void UnSubscribeAll()
        {
            foreach (EventBinding binding in this)
            {
                binding.UnSubscribe();
            }
        }

        /// <summary>
        /// Forces a fresh subscribtion on all event bindings
        /// </summary>
        public void ForceFreshSubscribeAll()
        {
            foreach (EventBinding binding in this)
            {
                binding.ForceFreshSubscription();
            }
        }

        /// <summary>
        /// Clears the list and unsubsribes all bindings
        /// </summary>
        public void ClearAndUnsubscribeAll()
        {
            UnSubscribeAll();
            Clear();
        }
    }
}
