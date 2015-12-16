using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    partial class StoryboardTargetAnimationCollection
    {
        /// <inheritdoc/>
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
