using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssStoryboardKeyframeCollection
    {
        /// <inheritdoc/>
        public List<UvssStoryboardKeyframe>.Enumerator GetEnumerator()
        {
            return keyframes.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssStoryboardKeyframe> IEnumerable<UvssStoryboardKeyframe>.GetEnumerator()
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
