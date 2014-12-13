using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout.Animation
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
