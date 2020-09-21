namespace HexCS.Core
{
    /// <summary>
    /// A promptable event. On each call to Invoke, a promptable event
    /// does not actually invoke any functions, it instead logs that some changes have occured
    /// and awaits prompting. When promted, it calls invoke if changes have occured since last prompt
    /// 
    /// It should be noted that multiple invoke calls between prompts will ignore old invoke calls
    /// This is designed for only-latest-data-is-relevent events
    /// </summary>
    public class PromptableEvent : Event
    {
        private Event _this;
        private bool _isInvokedSincePrompted = false;

        #region API
        /// <summary>
        /// Constrcut a promptable event
        /// </summary>
        public PromptableEvent() : base()
        {
            _this = this as Event;
        }

        /// <summary>
        /// Marks event as invoked. Invoke will not occur until promted
        /// </summary>
        public new void Invoke() => _isInvokedSincePrompted = true;

        /// <summary>
        /// Prompts the event, meaning that if an invoke has occured since
        /// last prompt, the event will be invoked
        /// </summary>
        public void Prompt()
        {
            if (_isInvokedSincePrompted)
            {
                _this?.Invoke();
            }

            _isInvokedSincePrompted = false;
        }
        #endregion
    }

    /// <summary>
    /// A promptable event. On each call to Invoke, a promptable event
    /// does not actually invoke any functions, it instead logs that some changes have occured
    /// and awaits prompting. When promted, it calls invoke if changes have occured since last prompt
    /// 
    /// It should be noted that multiple invoke calls between prompts will ignore old invoke calls.
    /// This is designed for only-latest-data-is-relevent events
    /// </summary>
    public class PromptableEvent<T> : Event<T>
    {
        private Event<T> _this;
        private bool _isInvokedSincePrompted = false;
        private T _lastArgument = default;

        #region API
        /// <summary>
        /// Constrcut a promptable event
        /// </summary>
        public PromptableEvent() : base()
        {
            _this = this as Event<T>;
        }

        /// <summary>
        /// Marks event as invoked. Invoke will not occur until promted.
        /// Caches args.
        /// </summary>
        public new void Invoke(T arg)
        {
            _lastArgument = arg;
            _isInvokedSincePrompted = true;
        }

        /// <summary>
        /// Prompts the event, meaning that if an invoke has occured since
        /// last prompt, the event will be invoked with the last cached arguments
        /// </summary>
        public void Prompt()
        {
            if (_isInvokedSincePrompted)
            {
                _this?.Invoke(_lastArgument);
            }

            _isInvokedSincePrompted = false;
        }
        #endregion
    }
}