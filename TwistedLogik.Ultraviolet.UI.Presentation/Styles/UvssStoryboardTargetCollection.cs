using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a collection of <see cref="UvssStoryboardTarget"/> objects which belong to an instance of <see cref="UvssStoryboard"/>.
    /// </summary>
    public sealed partial class UvssStoryboardTargetCollection : IEnumerable<UvssStoryboardTarget>
    {
        /// <summary>
        /// Adds a storyboard target to the collection.
        /// </summary>
        /// <param name="target">The storyboard target to add to the collection.</param>
        internal void Add(UvssStoryboardTarget target)
        {
            Contract.Require(target, "target");

            this.targets.Add(target);
        }

        /// <summary>
        /// Gets the number of targets in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return targets.Count; }
        }

        // State values.
        private readonly List<UvssStoryboardTarget> targets = 
            new List<UvssStoryboardTarget>();
    }
}
