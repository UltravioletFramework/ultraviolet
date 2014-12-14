using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents a keyframe in an animation.
    /// </summary>
    /// <typeparam name="T">The type of value being animated.</typeparam>
    public sealed class AnimationKeyframe<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationKeyframe{T}"/> class.
        /// </summary>
        /// <param name="time">The keyframe time.</param>
        /// <param name="easingFunction">The keyframe's easing function.</param>
        public AnimationKeyframe(TimeSpan time, EasingFunction easingFunction = null)
        {
            this.time           = time;
            this.hasValue       = false;
            this.easingFunction = easingFunction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationKeyframe{T}"/> class.
        /// </summary>
        /// <param name="time">The keyframe time.</param>
        /// <param name="value">The keyframe value.</param>
        /// <param name="easingFunction">The keyframe's easing function.</param>
        public AnimationKeyframe(TimeSpan time, T value, EasingFunction easingFunction = null)
        {
            this.time           = time;
            this.value          = value;
            this.hasValue       = true;
            this.easingFunction = easingFunction;
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

        /// <summary>
        /// Gets the keyframe's easing function.
        /// </summary>
        public EasingFunction EasingFunction
        {
            get { return easingFunction; }
        }

        // Property values.
        private TimeSpan time;
        private T value;
        private Boolean hasValue;
        private EasingFunction easingFunction;
    }
}
