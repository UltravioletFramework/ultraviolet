using System;
using Ultraviolet.Core.Collections;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents the extended event data for a <see cref="UpfExecutedRoutedEventHandler"/> delegate.
    /// </summary>
    public class ExecutedRoutedEventData : RoutedEventData
    {
        /// <summary>
        /// Retrieves an instance of the <see cref="ExecutedRoutedEventData"/> class from the global
        /// pool and initializes it for use with a routed event handler.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        /// <returns>The <see cref="ExecutedRoutedEventData"/> instance that was retrieved.</returns>
        public static new ExecutedRoutedEventData Retrieve(Object source, Boolean handled = false, Boolean autorelease = true)
        {
            return Retrieve(source, null, handled, autorelease);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="ExecutedRoutedEventData"/> class from the global
        /// pool and initializes it for use with a routed event handler.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="value">A <see cref="PrimitiveUnion"/> which represents the command's value parameter.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        /// <returns>The <see cref="ExecutedRoutedEventData"/> instance that was retrieved.</returns>
        public static ExecutedRoutedEventData Retrieve(Object source, PrimitiveUnion? value, Boolean handled = false, Boolean autorelease = true)
        {
            var data = default(ExecutedRoutedEventData);

            lock (pool)
                data = pool.Retrieve();

            data.OnRetrieved(pool, source, handled, autorelease);
            data.CommandValueParameter = value;

            return data;
        }

        /// <summary>
        /// Gets the value parameter associated with this command, if it has one.
        /// </summary>
        public PrimitiveUnion? CommandValueParameter
        {
            get { return commandValueParameter; }
            set { commandValueParameter = value; }
        }

        /// <inheritdoc/>
        protected override void OnRetrieved(IPool origin, Object source, Boolean handled, Boolean autorelease)
        {
            base.OnRetrieved(origin, source, handled, autorelease);
        }

        /// <inheritdoc/>
        protected override void OnReleased()
        {
            this.commandValueParameter = null;
            base.OnReleased();
        }

        // Property values.
        private PrimitiveUnion? commandValueParameter;

        // The global pool of commanding data objects.
        private static readonly Pool<ExecutedRoutedEventData> pool =
            new ExpandingPool<ExecutedRoutedEventData>(2, 4, () => new ExecutedRoutedEventData());
    }
}
