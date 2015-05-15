using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    partial class StoryboardTargetAnimationCollection
    {
        /// <inheritdoc/>
        public Dictionary<UvmlName, AnimationBase>.Enumerator GetEnumerator()
        {
            return animations.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<UvmlName, AnimationBase>> IEnumerable<KeyValuePair<UvmlName, AnimationBase>>.GetEnumerator()
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
