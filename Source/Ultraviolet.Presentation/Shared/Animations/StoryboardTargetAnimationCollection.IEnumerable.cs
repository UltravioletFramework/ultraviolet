using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Animations
{
    partial class StoryboardTargetAnimationCollection
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TValue}.Enumerator"/> that iterates through the collection.</returns>
        public Dictionary<StoryboardTargetAnimationKey, AnimationBase>.Enumerator GetEnumerator()
        {
            return animations.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<StoryboardTargetAnimationKey, AnimationBase>> IEnumerable<KeyValuePair<StoryboardTargetAnimationKey, AnimationBase>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
