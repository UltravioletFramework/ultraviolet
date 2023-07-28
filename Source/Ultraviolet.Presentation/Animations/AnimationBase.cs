using System;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents an animation.
    /// </summary>
    public abstract class AnimationBase
    {
        /// <summary>
        /// Gets the storyboard target that owns this animation.
        /// </summary>
        public StoryboardTarget Target
        {
            get { return target; }
            internal set { target = value; }
        }

        /// <summary>
        /// Gets or sets the animation's fill behavior, which specifies
        /// how its value is calculated when the storyboard is outside
        /// of the animation's active period.
        /// </summary>
        public FillBehavior FillBehavior
        {
            get { return fillBehavior; }
            set { fillBehavior = value; }
        }

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
        /// Adds a keyframe to the animation.
        /// </summary>
        /// <param name="keyframe">The keyframe to add to the animation.</param>
        internal abstract void AddKeyframe(AnimationKeyframeBase keyframe);

        /// <summary>
        /// Recalculates the animation's duration.
        /// </summary>
        internal abstract void RecalculateDuration();

        // Property values.
        private StoryboardTarget target;
        private FillBehavior fillBehavior = FillBehavior.HoldEnd;
    }
}
