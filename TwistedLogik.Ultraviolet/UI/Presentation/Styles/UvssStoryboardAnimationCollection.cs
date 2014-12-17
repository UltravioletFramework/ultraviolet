using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a collection of <see cref="UvssStoryboardAnimation"/> objects that belong to 
    /// an instance of <see cref="UvssStoryboardTarget"/>.
    /// </summary>
    public sealed partial class UvssStoryboardAnimationCollection : IEnumerable<UvssStoryboardAnimation>
    {
        /// <summary>
        /// Adds a storyboard animation to the collection.
        /// </summary>
        /// <param name="animation">The storyboard animation to add to the collection.</param>
        internal void Add(UvssStoryboardAnimation animation)
        {
            Contract.Require(animation, "animation");

            this.animations.Add(animation);
        }

        /// <summary>
        /// Gets the number of animations in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return animations.Count; }
        }

        // State values.
        private readonly List<UvssStoryboardAnimation> animations = 
            new List<UvssStoryboardAnimation>();
    }
}
