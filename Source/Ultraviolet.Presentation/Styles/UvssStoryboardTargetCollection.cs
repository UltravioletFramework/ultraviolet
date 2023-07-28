using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a collection of <see cref="UvssStoryboardTarget"/> objects which belong to an instance of <see cref="UvssStoryboard"/>.
    /// </summary>
    public sealed partial class UvssStoryboardTargetCollection : IEnumerable<UvssStoryboardTarget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardTargetCollection"/> class.
        /// </summary>
        internal UvssStoryboardTargetCollection()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardTargetCollection"/> class
        /// by populating it with the targets in the specified collection.
        /// </summary>
        /// <param name="targets">A collection containing the targets with which to
        /// populate this collection instance.</param>
        internal UvssStoryboardTargetCollection(IEnumerable<UvssStoryboardTarget> targets)
        {
            this.targets.AddRange(targets);
        }

        /// <summary>
        /// Adds a storyboard target to the collection.
        /// </summary>
        /// <param name="target">The storyboard target to add to the collection.</param>
        internal void Add(UvssStoryboardTarget target)
        {
            Contract.Require(target, nameof(target));

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
