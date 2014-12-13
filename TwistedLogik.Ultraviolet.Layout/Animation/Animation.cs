using System;

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
        public Animation()
        {
            this.keyframes = new AnimationKeyframeCollection<T>(this);
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

            if (Target != null && Target.Storyboard != null)
                Target.Storyboard.RecalculateDuration();
        }

        // Property values.
        private readonly AnimationKeyframeCollection<T> keyframes;
        private TimeSpan duration;
        private TimeSpan startTime;
        private TimeSpan endTime;
    }
}
