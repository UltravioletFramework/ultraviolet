using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a collection of <see cref="UvssStoryboardAnimation"/> objects that belong to 
    /// an instance of <see cref="UvssStoryboardTarget"/>.
    /// </summary>
    public sealed partial class UvssStoryboardAnimationCollection : IEnumerable<UvssStoryboardAnimation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardAnimationCollection"/> class.
        /// </summary>
        internal UvssStoryboardAnimationCollection()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardAnimationCollection"/> class
        /// by populating it with the animations in the specified collection.
        /// </summary>
        /// <param name="animations">A collection containing the animations with which to
        /// populate this collection instance.</param>
        internal UvssStoryboardAnimationCollection(IEnumerable<UvssStoryboardAnimation> animations)
        {
            this.animations.AddRange(animations);
        }

        /// <summary>
        /// Adds a storyboard animation to the collection.
        /// </summary>
        /// <param name="animation">The storyboard animation to add to the collection.</param>
        internal void Add(UvssStoryboardAnimation animation)
        {
            Contract.Require(animation, nameof(animation));

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
