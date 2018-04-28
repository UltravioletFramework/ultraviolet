using System;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents a keyframe in an animation.
    /// </summary>
    /// <typeparam name="T">The type of value being animated.</typeparam>
    public sealed class AnimationKeyframe<T> : AnimationKeyframeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationKeyframe{T}"/> class.
        /// </summary>
        /// <param name="time">The keyframe time.</param>
        /// <param name="easingFunction">The keyframe's easing function.</param>
        public AnimationKeyframe(TimeSpan time, EasingFunction easingFunction = null)
            : base(time, easingFunction)
        {
            this.hasValue = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationKeyframe{T}"/> class.
        /// </summary>
        /// <param name="time">The keyframe time.</param>
        /// <param name="value">The keyframe value.</param>
        /// <param name="easingFunction">The keyframe's easing function.</param>
        public AnimationKeyframe(TimeSpan time, T value, EasingFunction easingFunction = null)
            : base(time, easingFunction)
        {
            this.value = value;
            this.hasValue = true;
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
        private T value;
        private Boolean hasValue;
    }
}
