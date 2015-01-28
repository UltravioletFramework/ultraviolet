using System;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
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

        /// <summary>
        /// Finalizes the object.
        /// </summary>
        ~SimpleClock()
        {
            var pool = poolref.Target as IPool<SimpleClock>;
            if (pool != null)
            {
                HandleReleased();
                pool.Release(this);
                GC.ReRegisterForFinalize(this);
            }
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
        /// Called when the clock is retrieved from the clock pool.
        /// </summary>
        /// <param name="pool">The pool from which the clock was retrieved.</param>
        /// <param name="loopBehavior">A <see cref="LoopBehavior"/> value specifying the clock's loop behavior.</param>
        /// <param name="duration">The clock's duration.</param>
        internal void HandleRetrieved(IPool<SimpleClock> pool, LoopBehavior loopBehavior, TimeSpan duration)
        {
            this.loopBehavior   = loopBehavior;
            this.duration       = duration;
            this.pooled         = true;
            this.poolref.Target = pool;
        }

        /// <summary>
        /// Called when the clock is released back into the clock pool.
        /// </summary>
        internal void HandleReleased()
        {
            this.pooled         = false;
            this.poolref.Target = null;
        }

        /// <summary>
        /// Gets a value indicating whhether the clock was retrieved from the clock pool.
        /// </summary>
        internal Boolean IsPooled
        {
            get { return pooled; }
        }

        /// <inheritdoc/>
        protected override Boolean IsValid
        {
            get { return true; }
        }

        // Property values.
        private LoopBehavior loopBehavior;
        private TimeSpan duration;
        private Boolean pooled;
        private readonly WeakReference poolref = new WeakReference(null);
    }
}
