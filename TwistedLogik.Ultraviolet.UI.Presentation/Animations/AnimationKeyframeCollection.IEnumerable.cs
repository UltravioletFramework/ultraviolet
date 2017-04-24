using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Animations
{
    partial class AnimationKeyframeCollection<T>
    {
        /// <inheritdoc/>
        List<AnimationKeyframe<T>>.Enumerator GetEnumerator()
        {
            return keyframes.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<AnimationKeyframe<T>> IEnumerable<AnimationKeyframe<T>>.GetEnumerator()
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
