using System;
using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    partial class StoryboardTargetAnimationCollection
    {
        /// <inheritdoc/>
        public Dictionary<String, AnimationBase>.Enumerator GetEnumerator()
        {
            return animations.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<String, AnimationBase>> IEnumerable<KeyValuePair<String, AnimationBase>>.GetEnumerator()
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
