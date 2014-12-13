using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents a storyboard target's collection of animations.
    /// </summary>
    public sealed partial class StoryboardTargetAnimationCollection : IEnumerable<AnimationBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardTargetAnimationCollection"/> class.
        /// </summary>
        /// <param name="target">The storyboard target that owns the collection.</param>
        public StoryboardTargetAnimationCollection(StoryboardTarget target)
        {
            Contract.Require(target, "target");

            this.target = target;
        }

        /// <summary>
        /// Gets the storyboard target that owns the collection.
        /// </summary>
        public StoryboardTarget Target
        {
            get { return target; }
        }

        /// <summary>
        /// Gets the number of animations in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return animations.Count; }
        }

        // Property values.
        private readonly StoryboardTarget target;

        // State values.
        private readonly List<AnimationBase> animations = 
            new List<AnimationBase>();
    }
}
