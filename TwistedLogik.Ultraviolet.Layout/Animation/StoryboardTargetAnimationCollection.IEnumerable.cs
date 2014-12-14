using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    partial class StoryboardTargetAnimationCollection
    {
        /// <inheritdoc/>
        public Dictionary<DependencyPropertyName, AnimationBase>.Enumerator GetEnumerator()
        {
            return animations.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<DependencyPropertyName, AnimationBase>> IEnumerable<KeyValuePair<DependencyPropertyName, AnimationBase>>.GetEnumerator()
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
