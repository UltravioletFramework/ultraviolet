using System;

namespace Ultraviolet.Presentation
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
        /// <param name="ordinalByType">An ordinal representing the distance between the current type and the type on which the handler is defined.</param>
        /// <param name="ordinalWithinType">An ordinal representing the relative position of the handler within the list of handlers for its type.</param>
        /// <param name="handledEventsToo">A value indicating whether the handler should receive events which have already been handled by other handlers.</param>
        public RoutedEventHandlerMetadata(Delegate handler, Int16 ordinalByType, Int16 ordinalWithinType, Boolean handledEventsToo)
        {
            this.handler           = handler;
            this.ordinalByType     = ordinalByType;
            this.ordinalWithinType = ordinalWithinType;
            this.handledEventsToo  = handledEventsToo;
        }

        /// <summary>
        /// The event handler for the routed event.
        /// </summary>
        public Delegate Handler
        {
            get { return handler; }
        }
        
        /// <summary>
        /// Gets an ordinal representing the distance between the current type and the type on which
        /// the handler is defined. Used for sorting.
        /// </summary>
        public Int16 OrdinalByType
        {
            get { return ordinalByType; }
        }

        /// <summary>
        /// Gets an ordinal representing the relative position of the handler within
        /// the list of handlers for its type.
        /// </summary>
        public Int16 OrdinalWithinType
        {
            get { return ordinalWithinType; }
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
        private readonly Int16 ordinalByType;
        private readonly Int16 ordinalWithinType;
        private readonly Boolean handledEventsToo;
    }
}
