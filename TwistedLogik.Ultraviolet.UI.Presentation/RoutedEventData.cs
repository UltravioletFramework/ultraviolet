using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the extended event data for a routed event.
    /// </summary>
    public struct RoutedEventData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventData"/> structure.
        /// </summary>
        /// <param name="source">The object that originated the event.</param>
        public RoutedEventData(DependencyObject source)
        {
            Contract.Require(source, "source");

            this.originalSource = source;
            this.source         = source;
            this.handled        = false;
        }

        /// <summary>
        /// Gets a reference to the object that raised the event. This reference cannot be modified,
        /// so it will always refer to the object which originally raised the event, even if 
        /// the <see cref="Source"/> property is modified.
        /// </summary>
        public Object OriginalSource
        {
            get { return originalSource; }
        }

        /// <summary>
        /// Gets a reference to the object that raised the event.
        /// </summary>
        public Object Source
        {
            get { return source; }
            set { source = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the event has been handled.
        /// </summary>
        public Boolean Handled
        {
            get { return handled; }
            set { handled = value; }
        }

        // Property values.
        private Object originalSource;
        private Object source;
        private Boolean handled;
    }
}
