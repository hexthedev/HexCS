using System;

namespace HexCS.Core
{
    /// <summary>
    /// An Event object that can be subscribed to and invoked
    /// </summary>
    public class Event : IEvent
    {
        private event Action _event;
        private event Action _singleUseEvent;

        /// <inheritdoc />
        public EventBinding Subscribe(Action callback)
        {
            EventBinding binding = new EventBinding(
                () => _event += callback,
                () => _event -= callback
            );

            binding.Subscribe();
            return binding;
        }

        /// <inheritdoc />
        public EventBinding SubscribeSingleUse(Action callback)
        {
            EventBinding binding = new EventBinding(
                () => _singleUseEvent += callback,
                () => _singleUseEvent -= callback
            );

            binding.Subscribe();
            return binding;
        }

        /// <inheritdoc />
        public void Unsubscribe(Action callback)
        {
            if (callback != null)
            {
                _event -= callback;
            }
        }

        /// <summary>
        /// Invoke the event
        /// </summary>
        public void Invoke()
        {
            if (_event != null)
            {
                _event.Invoke();
            }

            if (_singleUseEvent != null)
            {
                _singleUseEvent.Invoke();

                Delegate[] callbacks = _singleUseEvent.GetInvocationList();

                foreach (Delegate d in callbacks)
                {
                    _singleUseEvent -= d as Action;
                }
            }
        }
    }

    /// <summary>
    /// An Event object that can be subscripbed to and invoked
    /// </summary>
    public class Event<T> : IEvent<T>
    {
        private event Action<T> _event;
        private event Action<T> _singleUseEvent;

        /// <inheritdoc />
        public EventBinding Subscribe(Action<T> callback)
        {
            EventBinding binding = new EventBinding(
                () => _event += callback,
                () => _event -= callback
            );

            binding.Subscribe();
            return binding;
        }

        /// <inheritdoc />
        public EventBinding SubscribeSingleUse(Action<T> callback)
        {
            EventBinding binding = new EventBinding(
                () => _singleUseEvent += callback,
                () => _singleUseEvent -= callback
            );

            binding.Subscribe();
            return binding;
        }

        /// <inheritdoc />
        public void Unsubscribe(Action<T> callback)
        {
            if (callback != null)
            {
                _event -= callback;
            }
        }

        /// <summary>
        /// Invoke the event
        /// </summary>
        public void Invoke(T arg)
        {
            if (_event != null)
            {
                _event.Invoke(arg);
            }

            if (_singleUseEvent != null)
            {
                _singleUseEvent.Invoke(arg);

                Delegate[] callbacks = _singleUseEvent.GetInvocationList();

                foreach (Delegate d in callbacks)
                {
                    _singleUseEvent -= d as Action<T>;
                }
            }
        }
    }
}
