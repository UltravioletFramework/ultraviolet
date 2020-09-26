using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of animations associated with a particular <see cref="SkinnedModel"/> instance.
    /// </summary>
    public class SkinnedAnimationCollection : ModelResourceCollection<SkinnedAnimation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedAnimationCollection"/> class.
        /// </summary>
        /// <param name="animations">The animations to add to the collection.</param>
        public SkinnedAnimationCollection(IEnumerable<SkinnedAnimation> animations)
            : base(animations)
        {
            if (animations != null)
            {
                this.animationsByName = animations.Where(x => !String.IsNullOrEmpty(x.Name)).ToDictionary(x => x.Name);
            }
            else
            {
                this.animationsByName = new Dictionary<String, SkinnedAnimation>(0);
            }
        }

        /// <summary>
        /// Attempts to retrieve the animation with the specified name.
        /// </summary>
        /// <param name="name">The name of the animation to retrieve.</param>
        /// <returns>The animation with the specified name, or <see langword="null"/> if no such animation exists.</returns>
        public SkinnedAnimation TryGetAnimationByName(String name)
        {
            Contract.Require(name, nameof(name));

            animationsByName.TryGetValue(name, out var animation);
            return animation;
        }

        // Animation collections.
        private readonly Dictionary<String, SkinnedAnimation> animationsByName;
    }
}
