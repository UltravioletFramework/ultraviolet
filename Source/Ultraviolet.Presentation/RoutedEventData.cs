using System;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the extended event data for a routed event.
    /// </summary>
    public class RoutedEventData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventData"/> class.
        /// </summary>
        protected internal RoutedEventData()
        { }

        /// <summary>
        /// Retrieves an instance of the <see cref="RoutedEventData"/> class from the global
        /// pool and initializes it for use with a routed event handler.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        /// <returns>The <see cref="RoutedEventData"/> instance that was retrieved.</returns>
        public static RoutedEventData Retrieve(Object source, Boolean handled = false, Boolean autorelease = true)
        {
            var data = default(RoutedEventData);

            lock (pool)
                data = pool.Retrieve();
            
            data.OnRetrieved(pool, source, handled, autorelease);
            return data;
        }

        /// <summary>
        /// Releases the instance back into the pool which created it.
        /// </summary>
        public void Release()
        {
            if (!Alive)
                throw new InvalidOperationException(PresentationStrings.PooledResourceAlreadyReleased);

            var pool = this.origin;

            OnReleased();

            lock (pool)
                pool.Release(this);
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

        /// <summary>
        /// Allocates the object for use.
        /// </summary>
        /// <param name="origin">The pool which originated the object.</param>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        protected virtual void OnRetrieved(IPool origin, Object source, Boolean handled, Boolean autorelease)
        {
            this.originalSource = source;
            this.source = source;
            this.handled = handled;
            this.autorelease = autorelease;
            this.origin = origin;
        }

        /// <summary>
        /// Releases any references held by the object.
        /// </summary>
        protected virtual void OnReleased()
        {
            this.originalSource = null;
            this.source = null;
            this.handled = false;
            this.autorelease = true;
            this.origin = null;
        }

        /// <summary>
        /// Gets a value indicating whether the object is currently alive.
        /// </summary>
        protected internal Boolean Alive => origin != null;

        /// <summary>
        /// Gets a value indicating whether the object should be automatically released
        /// back into the global pool after it has been used.
        /// </summary>
        protected internal Boolean AutoRelease => autorelease;

        // The global pool of routed event data objects.
        private static readonly Pool<RoutedEventData> pool =
            new ExpandingPool<RoutedEventData>(8, 16, () => new RoutedEventData());

        // Property values.
        private Object originalSource;
        private Object source;
        private Boolean handled;
        private Boolean autorelease;

        // The pool which originated this object.
        private IPool origin;
    }
}
