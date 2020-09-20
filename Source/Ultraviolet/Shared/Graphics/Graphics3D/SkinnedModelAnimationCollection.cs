using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of animations associated with a particular <see cref="SkinnedModel"/> instance.
    /// </summary>
    public class SkinnedModelAnimationCollection : IEnumerable<SkinnedAnimation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedModelAnimationCollection"/> class.
        /// </summary>
        /// <param name="animations">The animations to add to the collection.</param>
        public SkinnedModelAnimationCollection(IEnumerable<SkinnedAnimation> animations)
        {
            if (animations != null)
            {
                this.animations = animations.ToArray();
                this.animationsByName = this.animations.Where(x => !String.IsNullOrEmpty(x.Name)).ToDictionary(x => x.Name);
            }
            else
            {
                this.animations = new SkinnedAnimation[0];
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

        /// <summary>
        /// Gets the animation at the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the animation to retrieve.</param>
        /// <returns>The animation at the specified index within the collection.</returns>
        public SkinnedAnimation this[Int32 index] => animations[index];

        /// <summary>
        /// Returns an <see cref="IEnumerator"/> for the collection.
        /// </summary>
        /// <returns>An <see cref="ArrayEnumerator{T}"/> which will enumerate through the collection.</returns>
        ArrayEnumerator<SkinnedAnimation> GetEnumerator() => new ArrayEnumerator<SkinnedAnimation>(animations);

        /// <inheritdoc/>
        IEnumerator<SkinnedAnimation> IEnumerable<SkinnedAnimation>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets the number of animations in the collection.
        /// </summary>
        public Int32 Count => animations.Length;

        // Animation collections.
        private readonly SkinnedAnimation[] animations;
        private readonly Dictionary<String, SkinnedAnimation> animationsByName;
    }
}
