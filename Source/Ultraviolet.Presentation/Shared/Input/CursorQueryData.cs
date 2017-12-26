using System;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents the data for a cursor query event.
    /// </summary>
    public class CursorQueryRoutedEventData : RoutedEventData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CursorQueryRoutedEventData"/> class.
        /// </summary>
        protected internal CursorQueryRoutedEventData()
        { }

        /// <summary>
        /// Retrieves an instance of the <see cref="CursorQueryRoutedEventData"/> class from the global
        /// pool and initializes it for use with a routed event handler.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        /// <returns>The <see cref="CursorQueryRoutedEventData"/> instance that was retrieved.</returns>
        public static new CursorQueryRoutedEventData Retrieve(Object source, Boolean handled = false, Boolean autorelease = true)
        {
            var data = default(CursorQueryRoutedEventData);

            lock (pool)
                data = pool.Retrieve();

            data.OnRetrieved(pool, source, handled, autorelease);
            return data;
        }

        /// <summary>
        /// Gets or sets the cursor to display.
        /// </summary>
        public Cursor Cursor { get; set; }

        /// <inheritdoc/>
        protected override void OnRetrieved(IPool origin, Object source, Boolean handled, Boolean autorelease)
        {
            Cursor = null;

            base.OnRetrieved(origin, source, handled, autorelease);
        }

        /// <inheritdoc/>
        protected override void OnReleased()
        {
            Cursor = null;

            base.OnReleased();
        }

        // The global pool of cursor query data objects.
        private static readonly Pool<CursorQueryRoutedEventData> pool =
            new ExpandingPool<CursorQueryRoutedEventData>(1, 4, () => new CursorQueryRoutedEventData());
    }
}
