using System;
using Ultraviolet.Core.Collections;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents the extended event data for a <see cref="UpfCanExecuteRoutedEventHandler"/> delegate.
    /// </summary>
    public class CanExecuteRoutedEventData : RoutedEventData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanExecuteRoutedEventData"/> class.
        /// </summary>
        protected internal CanExecuteRoutedEventData() { }

        /// <summary>
        /// Retrieves an instance of the <see cref="RoutedEventData"/> class from the global
        /// pool and initializes it for use with a routed event handler.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        /// <returns>The <see cref="RoutedEventData"/> instance that was retrieved.</returns>
        public static new CanExecuteRoutedEventData Retrieve(Object source, Boolean handled = false, Boolean autorelease = true)
        {
            return Retrieve(source, null, handled, autorelease);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="RoutedEventData"/> class from the global
        /// pool and initializes it for use with a routed event handler.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="valparam">A value parameter to associate with the command.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        /// <returns>The <see cref="RoutedEventData"/> instance that was retrieved.</returns>
        public static CanExecuteRoutedEventData Retrieve(Object source, PrimitiveUnion? valparam, Boolean handled = false, Boolean autorelease = true)
        {
            var data = default(CanExecuteRoutedEventData);

            lock (pool)
                data = pool.Retrieve();

            data.OnRetrieved(pool, source, handled, autorelease);
            data.commandValueParameter = valparam;
            data.canExecute = false;
            data.continueRouting = false;
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

        /// <summary>
        /// Gets or sets a value indicating whether the command can be executed
        /// with the specified parameter.
        /// </summary>
        public Boolean CanExecute
        {
            get { return canExecute; }
            set { canExecute = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the input event that
        /// caused the command to execute should continue its route.
        /// </summary>
        public Boolean ContinueRouting
        {
            get { return continueRouting; }
            set { continueRouting = value; }
        }
        
        /// <inheritdoc/>
        protected override void OnRetrieved(IPool origin, Object source, Boolean handled, Boolean autorelease)
        {
            base.OnRetrieved(origin, source, handled, autorelease);
        }

        /// <summary>
        /// Releases any references held by the object.
        /// </summary>
        protected override void OnReleased()
        {
            this.commandValueParameter = null;
            this.canExecute = false;
            this.continueRouting = false;
            base.OnReleased();
        }

        // The global pool of event args objects.
        private static readonly Pool<CanExecuteRoutedEventData> pool =
            new ExpandingPool<CanExecuteRoutedEventData>(1, 8, () => new CanExecuteRoutedEventData());

        // Property values.
        private PrimitiveUnion? commandValueParameter;
        private Boolean canExecute;
        private Boolean continueRouting;
    }
}
