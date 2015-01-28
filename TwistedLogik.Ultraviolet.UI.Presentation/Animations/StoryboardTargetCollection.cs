using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents a storyboard's collection of storyboard targets.
    /// </summary>
    public sealed partial class StoryboardTargetCollection : IEnumerable<StoryboardTarget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardTargetCollection"/> class.
        /// </summary>
        /// <param name="storyboard">The storyboard that owns the collection.</param>
        internal StoryboardTargetCollection(Storyboard storyboard)
        {
            Contract.Require(storyboard, "storyboard");

            this.storyboard = storyboard;
        }

        /// <summary>
        /// Adds a storyboard target to the collection.
        /// </summary>
        /// <param name="target">The storyboard target to add to the collection.</param>
        /// <returns><c>true</c> if the target was added to the collection; otherwise, <c>false</c>.</returns>
        public Boolean Add(StoryboardTarget target)
        {
            Contract.Require(target, "target");

            if (target.Storyboard == Storyboard)
                return false;

            if (target.Storyboard != null)
                target.Storyboard.Targets.Remove(target);

            targets.Add(target);
            target.Storyboard = Storyboard;
            Storyboard.RecalculateDuration();

            return true;
        }

        /// <summary>
        /// Removes a storyboard target from the collection.
        /// </summary>
        /// <param name="target">The storyboard target to remove from the collection.</param>
        /// <returns><c>true</c> if the target was removed from the collection; otherwise, <c>false</c>.</returns>
        public Boolean Remove(StoryboardTarget target)
        {
            Contract.Require(target, "Target");

            if (target.Storyboard != Storyboard)
                return false;

            if (targets.Remove(target))
            {
                target.Storyboard = null;
                Storyboard.RecalculateDuration();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified storyboard target.
        /// </summary>
        /// <param name="target">The storyboard target to evaluate.</param>
        /// <returns><c>true</c> if the collection contains the specified target; otherwise, <c>false</c>.</returns>
        public Boolean Contains(StoryboardTarget target)
        {
            Contract.Require(target, "target");

            return targets.Contains(target);
        }

        /// <summary>
        /// Gets the storyboard that owns this collection.
        /// </summary>
        public Storyboard Storyboard
        {
            get { return storyboard; }
        }

        /// <summary>
        /// Gets the number of targets in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return targets.Count; }
        }

        // Property values.
        private readonly Storyboard storyboard;

        // State values.
        private readonly List<StoryboardTarget> targets = 
            new List<StoryboardTarget>();
    }
}
