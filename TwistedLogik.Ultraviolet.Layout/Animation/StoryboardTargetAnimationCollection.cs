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
        /// Adds the specified animation to this collection.
        /// </summary>
        /// <param name="animation">The animation to add to the collection.</param>
        /// <returns><c>true</c> if the animation was added to the collection; otherwise, <c>false</c>.</returns>
        public Boolean Add(AnimationBase animation)
        {
            Contract.Require(animation, "animation");

            if (animation.Target == Target)
                return false;

            if (animation.Target != null)
                animation.Target.Animations.Remove(animation);

            animation.Target = Target;
            animations.Add(animation);
            if (Target.Storyboard != null)
            {
                Target.Storyboard.RecalculateDuration();
            }
            return true;
        }

        /// <summary>
        /// Removes an animation from this collection.
        /// </summary>
        /// <param name="animation">The animation to remove from the collection.</param>
        /// <returns><c>true</c> if the animation was removed from the collection; otherwise, <c>false</c>.</returns>
        public Boolean Remove(AnimationBase animation)
        {
            Contract.Require(animation, "animation");

            if (animation.Target != Target)
                return false;

            if (animations.Remove(animation))
            {
                animation.Target = null;
                if (Target.Storyboard != null)
                {
                    Target.Storyboard.RecalculateDuration();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified animation.
        /// </summary>
        /// <param name="animation">The animation to evaluate.</param>
        /// <returns><c>true</c> if this collection contains the specified animation; otherwise, <c>false</c>.</returns>
        public Boolean Contains(AnimationBase animation)
        {
            Contract.Require(animation, "animation");

            return animations.Contains(animation);
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
