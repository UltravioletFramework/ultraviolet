using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents the extended event data for a <see cref="CanExecuteRoutedEventHandler"/> delegate.
    /// </summary>
    public class CanExecuteEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanExecuteEventArgs"/> class.
        /// </summary>
        protected internal CanExecuteEventArgs() { }

        /// <summary>
        /// Retrieves an instance of the <see cref="RoutedEventData"/> class from the global
        /// pool and initializes it for use with a routed event handler.
        /// </summary>
        /// <param name="canExecute">A value indicating whether the command can be executed with the specified parameter.</param>
        /// <param name="continueRouting">A value indicating whether the input event that caused the command to execute should continue its route.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        /// <returns>The <see cref="RoutedEventData"/> instance that was retrieved.</returns>
        public static CanExecuteEventArgs Retrieve(Boolean canExecute = false, Boolean continueRouting = false, Boolean autorelease = true)
        {
            var data = default(CanExecuteEventArgs);

            lock (pool)
                data = pool.Retrieve();

            data.OnRetrieved(pool, canExecute, continueRouting, autorelease);
            return data;
        }

        /// <summary>
        /// Releases the instance back into the pool which created it.
        /// </summary>
        [Preserve]
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
        /// Gets or sets a value indicating whether the command can be executed
        /// with the specified parameter.
        /// </summary>
        public Boolean CanExecute => canExecute;

        /// <summary>
        /// Gets or sets a value indicating whether the input event that
        /// caused the command to execute should continue its route.
        /// </summary>
        public Boolean ContinueRouting => continueRouting;

        /// <summary>
        /// Allocates the object for use.
        /// </summary>
        /// <param name="origin">The pool which originated the object.</param>
        /// <param name="canExecute">A value indicating whether the command can be executed with the specified parameter.</param>
        /// <param name="continueRouting">A value indicating whether the input event that caused the command to execute should continue its route.</param>
        /// <param name="autorelease">A value indicating whether the data is automatically released
        /// back to the global pool after it has been used by an event handler delegate.</param>
        protected virtual void OnRetrieved(IPool origin, Boolean canExecute, Boolean continueRouting, Boolean autorelease)
        {
            this.canExecute = canExecute;
            this.continueRouting = continueRouting;
            this.autorelease = autorelease;
            this.origin = origin;
        }

        /// <summary>
        /// Releases any references held by the object.
        /// </summary>
        protected virtual void OnReleased()
        {
            this.canExecute = false;
            this.continueRouting = false;
            this.autorelease = true;
            this.origin = null;
        }

        /// <summary>
        /// Gets a value indicating whether the object is currently alive.
        /// </summary>
        [Preserve]
        protected internal Boolean Alive => origin != null;

        /// <summary>
        /// Gets a value indicating whether the object should be automatically released
        /// back into the global pool after it has been used.
        /// </summary>
        [Preserve]
        protected internal Boolean AutoRelease => autorelease;

        // The global pool of event args objects.
        private static readonly Pool<CanExecuteEventArgs> pool =
            new ExpandingPool<CanExecuteEventArgs>(1, 8, () => new CanExecuteEventArgs());

        // Property values.
        private Boolean canExecute;
        private Boolean continueRouting;
        private Boolean autorelease;

        // The pool which originated this object.
        private IPool origin;
    }
}
