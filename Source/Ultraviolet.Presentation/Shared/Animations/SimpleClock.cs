using System;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents a clock which tracks the timing state of a simple animations.
    /// </summary>
    public sealed class SimpleClock : Clock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleClock"/> class.
        /// </summary>
        /// <param name="loopBehavior">The clock's loop behavior.</param>
        /// <param name="duration">The clock's duration.</param>
        public SimpleClock(LoopBehavior loopBehavior, TimeSpan duration)
        {
            this.loopBehavior = loopBehavior;
            this.duration = duration;
        }
        
        /// <inheritdoc/>
        public override LoopBehavior LoopBehavior
        {
            get { return loopBehavior; }
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get { return duration; }
        }

        /// <summary>
        /// Sets the clock's loop behavior.
        /// </summary>
        /// <param name="loopBehavior">The clock's loop behavior.</param>
        internal void SetLoopBehavior(LoopBehavior loopBehavior)
        {
            this.loopBehavior = loopBehavior;
        }

        /// <summary>
        /// Sets the clock's duration.
        /// </summary>
        /// <param name="duration">The clock's duration.</param>
        internal void SetDuration(TimeSpan duration)
        {
            this.duration = duration;
        }
        
        /// <summary>
        /// Gets the <see cref="UpfPool{TPooledType}.PooledObject"/> that represents this clock, if the clock
        /// was retrieved from an internal pool.
        /// </summary>
        internal UpfPool<SimpleClock>.PooledObject PooledObject
        {
            get;
            set;
        }

        /// <inheritdoc/>
        protected override Boolean IsValid
        {
            get { return true; }
        }

        // Property values.
        private LoopBehavior loopBehavior;
        private TimeSpan duration;
    }
}
