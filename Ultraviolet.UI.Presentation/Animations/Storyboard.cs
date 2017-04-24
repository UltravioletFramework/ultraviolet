using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents a collection of related animations.
    /// </summary>
    public sealed class Storyboard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Storyboard"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="loopBehavior">The storyboard's loop behavior.</param>
        public Storyboard(UltravioletContext uv, LoopBehavior loopBehavior = LoopBehavior.None)
        {
            this.targets = new StoryboardTargetCollection(this);
            this.loopBehavior = loopBehavior;
        }

        /// <summary>
        /// Begins playing the storyboard on the specified element.
        /// </summary>
        /// <param name="element">The element on which to begin playing the storyboard.</param>
        public void Begin(UIElement element)
        {
            Contract.Require(element, nameof(element));

            element.BeginStoryboard(this);
        }

        /// <summary>
        /// Stops the storyboard on the specified element.
        /// </summary>
        /// <param name="element">The element on which to stop the storyboard.</param>
        public void Stop(UIElement element)
        {
            Contract.Require(element, nameof(element));

            element.StopStoryboard(this);
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
                    if (animation.Value.EndTime > duration)
                    {
                        duration = animation.Value.EndTime;
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
