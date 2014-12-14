using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents a collection of related animations.
    /// </summary>
    public sealed class Storyboard : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Storyboard"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public Storyboard(UltravioletContext uv)
            : base(uv)
        {
            this.targets = new StoryboardTargetCollection(this);
        }

        /// <summary>
        /// Gets the storyboard's collection of targets.
        /// </summary>
        public StoryboardTargetCollection Targets
        {
            get { return targets; }
        }

        /// <summary>
        /// Gets the storyboard's loop behavior.
        /// </summary>
        public LoopBehavior LoopBehavior
        {
            get { return loopBehavior; }
            set { loopBehavior = value; }
        }

        /// <summary>
        /// Gets the storyboard's total duration.
        /// </summary>
        public TimeSpan Duration
        {
            get { return duration; }
        }

        /// <summary>
        /// Recalculates the storyboard's duration.
        /// </summary>
        internal void RecalculateDuration()
        {
            duration = TimeSpan.Zero;

            foreach (var target in targets)
            {
                foreach (var animation in target.Animations)
                {
                    if (animation.EndTime > duration)
                    {
                        duration = animation.EndTime;
                    }
                }
            }
        }

        // Property values.
        private readonly StoryboardTargetCollection targets;
        private TimeSpan duration;
        private LoopBehavior loopBehavior = LoopBehavior.None;
    }
}
