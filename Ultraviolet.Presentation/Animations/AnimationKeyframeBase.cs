using System;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents the base class for keyframes in an animation.
    /// </summary>
    public abstract class AnimationKeyframeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationKeyframeBase"/> class.
        /// </summary>
        /// <param name="time">The keyframe time.</param>
        /// <param name="easingFunction">The keyframe's easing function.</param>
        public AnimationKeyframeBase(TimeSpan time, EasingFunction easingFunction = null)
        {
            this.time           = time;
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
        /// Gets the keyframe's easing function.
        /// </summary>
        public EasingFunction EasingFunction
        {
            get { return easingFunction; }
        }

        // Property values.
        private readonly TimeSpan time;
        private readonly EasingFunction easingFunction;
    }
}
