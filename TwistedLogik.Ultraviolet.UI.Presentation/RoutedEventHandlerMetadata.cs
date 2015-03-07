using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the metadata for routed event handlers.
    /// </summary>
    internal struct RoutedEventHandlerMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventHandlerMetadata"/> structure.
        /// </summary>
        /// <param name="handler">The event handler for the routed event.</param>
        /// <param name="handledEventsToo">A value indicating whether the handler should receive events which have already been handled by other handlers.</param>
        public RoutedEventHandlerMetadata(Delegate handler, Boolean handledEventsToo)
        {
            this.handler          = handler;
            this.handledEventsToo = handledEventsToo;
        }

        /// <summary>
        /// The event handler for the routed event.
        /// </summary>
        public Delegate Handler
        {
            get { return handler; }
        }

        /// <summary>
        /// A value indicating whether the handler should receive events which have already been handled by other handlers.
        /// </summary>
        public Boolean HandledEventsToo
        {
            get { return handledEventsToo; }
        }

        // Property values.
        private readonly Delegate handler;
        private readonly Boolean handledEventsToo;
    }
}
