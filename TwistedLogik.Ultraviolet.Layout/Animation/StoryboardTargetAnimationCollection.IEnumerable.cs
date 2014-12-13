using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    partial class StoryboardTargetAnimationCollection
    {
        /// <inheritdoc/>
        List<AnimationBase>.Enumerator GetEnumerator()
        {
            return animations.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<AnimationBase> IEnumerable<AnimationBase>.GetEnumerator()
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
