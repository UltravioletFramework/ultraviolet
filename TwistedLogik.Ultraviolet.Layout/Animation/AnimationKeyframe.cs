using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents a keyframe in an animation.
    /// </summary>
    /// <typeparam name="T">The type of value being animated.</typeparam>
    public abstract class AnimationKeyframe<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationKeyframe{T}"/> class.
        /// </summary>
        /// <param name="time">The keyframe time.</param>
        public AnimationKeyframe(TimeSpan time)
        {
            this.time     = time;
            this.hasValue = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationKeyframe{T}"/> class.
        /// </summary>
        /// <param name="time">The keyframe time.</param>
        /// <param name="value">The keyframe value.</param>
        public AnimationKeyframe(TimeSpan time, T value)
        {
            this.time     = time;
            this.value    = value;
            this.hasValue = true;
        }

        /// <summary>
        /// Gets the keyframe time.
        /// </summary>
        public TimeSpan Time
        {
            get { return time; }
        }

        /// <summary>
        /// Gets the keyframe value.
        /// </summary>
        public T Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets a value indicating whether the keyframe has a value.
        /// </summary>
        public Boolean HasValue
        {
            get { return hasValue; }
        }

        // Property values.
        private TimeSpan time;
        private T value;
        private Boolean hasValue;
    }
}
