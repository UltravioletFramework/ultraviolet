using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation.
    /// </summary>
    /// <typeparam name="T">The type of value that is being animated.</typeparam>
    public abstract class Animation<T> : AnimationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Animation{T}"/> class.
        /// </summary>
        /// <param name="target">The storyboard target that owns this animation.</param>
        internal Animation(StoryboardTarget target)
        {
            Contract.Require(target, "target");

            this.target    = target;
            this.keyframes = new AnimationKeyframeCollection<T>(this);
        }

        /// <summary>
        /// Gets the storyboard target that owns this animation.
        /// </summary>
        public StoryboardTarget Target
        {
            get { return target; }
        }

        /// <summary>
        /// Gets the animation's collection of keyframes.
        /// </summary>
        public AnimationKeyframeCollection<T> Keyframes
        {
            get { return keyframes; }
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get { return duration; }
        }

        /// <inheritdoc/>
        public override TimeSpan StartTime
        {
            get { return startTime; }
        }

        /// <inheritdoc/>
        public override TimeSpan EndTime
        {
            get { return endTime; }
        }

        /// <inheritdoc/>
        internal override void RecalculateDuration()
        {
            if (keyframes.Count > 0)
            {
                startTime = keyframes[0].Time;
                endTime   = keyframes[keyframes.Count - 1].Time;
            }
            else
            {
                startTime = TimeSpan.Zero;
                endTime   = TimeSpan.Zero;
            }

            duration = endTime - startTime;

            target.Storyboard.RecalculateDuration();
        }

        // Property values.
        private readonly StoryboardTarget target;
        private readonly AnimationKeyframeCollection<T> keyframes;
        private TimeSpan duration;
        private TimeSpan startTime;
        private TimeSpan endTime;
    }
}
