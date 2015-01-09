using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents a clock which tracks the timing state of a single explicitly animated property value.
    /// </summary>
    public sealed class AnimationClock : Clock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationClock"/> class.
        /// </summary>
        internal AnimationClock() { }

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
        /// <param name="duration">The clock's duration.</param>
        /// <param name="loopBehavior">The clock's loop behavior.</param>
        internal void HandleRetrieved(TimeSpan duration, LoopBehavior loopBehavior)
        {
            this.duration     = duration;
            this.loopBehavior = loopBehavior;
            this.valid        = true;
        }

        /// <summary>
        /// Called when the clock is released back into the clock pool.
        /// </summary>
        internal void HandleReleased()
        {
            this.valid = false;
        }

        /// <inheritdoc/>
        protected override Boolean IsValid
        {
            get { return valid; }
        }

        // Property values.
        private Boolean valid;
        private TimeSpan duration;
        private LoopBehavior loopBehavior;
    }
}
