using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation.
    /// </summary>
    /// <typeparam name="T">The type of value that is being animated.</typeparam>
    public abstract class AnimationBase
    {
        /// <summary>
        /// Gets the animation's total duration.
        /// </summary>
        public abstract TimeSpan Duration
        {
            get;
        }

        /// <summary>
        /// Gets the time at which the animation begins.
        /// </summary>
        public abstract TimeSpan StartTime
        {
            get;
        }

        /// <summary>
        /// Gets the time at which the animation ends.
        /// </summary>
        public abstract TimeSpan EndTime
        {
            get;
        }
        
        /// <summary>
        /// Recalculates the animation's duration.
        /// </summary>
        internal abstract void RecalculateDuration();
    }
}
