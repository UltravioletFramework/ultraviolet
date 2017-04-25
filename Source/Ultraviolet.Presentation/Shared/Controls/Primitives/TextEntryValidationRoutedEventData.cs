using System;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the data for a text entry validation event.
    /// </summary>
    public class TextEntryValidationRoutedEventData : RoutedEventData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextEntryValidationRoutedEventData"/> class.
        /// </summary>
        protected internal TextEntryValidationRoutedEventData()
        { }

        /// <summary>
        /// Retrieves an instance of the <see cref="TextEntryValidationRoutedEventData"/> class from the global
        /// pool and initializes it for use with a routed event handler.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        /// <returns>The <see cref="TextEntryValidationRoutedEventData"/> instance that was retrieved.</returns>
        public static new TextEntryValidationRoutedEventData Retrieve(Object source, Boolean handled = false, Boolean autorelease = true)
        {
            var data = default(TextEntryValidationRoutedEventData);

            lock (pool)
                data = pool.Retrieve();

            data.OnRetrieved(pool, source, handled, autorelease);
            return data;
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether the text is valid.
        /// </summary>
        public Boolean Valid { get; set; }

        /// <inheritdoc/>
        protected override void OnRetrieved(IPool origin, Object source, Boolean handled, Boolean autorelease)
        {
            Valid = true;

            base.OnRetrieved(origin, source, handled, autorelease);
        }

        /// <inheritdoc/>
        protected override void OnReleased()
        {
            Valid = true;

            base.OnReleased();
        }

        // The global pool of text entry validation data objects.
        private static readonly Pool<TextEntryValidationRoutedEventData> pool =
            new ExpandingPool<TextEntryValidationRoutedEventData>(1, 4, () => new TextEntryValidationRoutedEventData());        
    }
}
