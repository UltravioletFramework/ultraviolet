using System;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents an animation.
    /// </summary>
    /// <typeparam name="T">The type of value that is being animated.</typeparam>
    public sealed class Animation<T> : AnimationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Animation{T}"/> class.
        /// </summary>
        public Animation()
        {
            this.keyframes = new AnimationKeyframeCollection<T>(this);
        }

        /// <summary>
        /// Interpolates between two values.
        /// </summary>
        /// <param name="value1">The first value to interpolate.</param>
        /// <param name="value2">The second value to interpolate.</param>
        /// <param name="easing">The easing function.</param>
        /// <param name="factor">The interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public T InterpolateValues(T value1, T value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }

        /// <summary>
        /// Gets the animation's first keyframe.
        /// </summary>
        /// <returns>The animation's first keyframe, or <see langword="null"/> if the animation has no keyframes.</returns>
        public AnimationKeyframe<T> GetFirstKeyframe()
        {
            return keyframes.Count == 0 ? null : keyframes[0];
        }

        /// <summary>
        /// Gets the animation's last keyframe.
        /// </summary>
        /// <returns>The animation's last keyframe, or <see langword="null"/> if the animation has no keyframes.</returns>
        public AnimationKeyframe<T> GetLastKeyframe()
        {
            return keyframes.Count == 0 ? null : keyframes[keyframes.Count - 1];
        }

        /// <summary>
        /// Gets the keyframes that surround the specified moment in time.
        /// </summary>
        /// <param name="time">The time to evaluate.</param>
        /// <param name="kf1">The keyframe that precedes the specified time.</param>
        /// <param name="kf2">The keyframe that succeeds the specified time.</param>
        public void GetKeyframes(TimeSpan time, out AnimationKeyframe<T> kf1, out AnimationKeyframe<T> kf2)
        {
            if (keyframes.Count == 0)
            {
                kf1 = null;
                kf2 = null;
                return;
            }

            if (time < keyframes[0].Time)
            {
                kf1 = null;
                kf2 = keyframes[0];
                return;
            }

            if (time > keyframes[keyframes.Count - 1].Time)
            {
                kf1 = keyframes[keyframes.Count - 1];
                kf2 = null;
                return;
            }

            for (int i = 0; i < keyframes.Count - 1; i++)
            {
                if (keyframes[i].Time <= time && keyframes[i + 1].Time >= time)
                {
                    kf1 = keyframes[i];
                    kf2 = keyframes[i + 1];
                    return;
                }
            }

            kf1 = keyframes[keyframes.Count - 1];
            kf2 = null;
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
        internal override void AddKeyframe(AnimationKeyframeBase keyframe)
        {
            Keyframes.Add((AnimationKeyframe<T>)keyframe);
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
