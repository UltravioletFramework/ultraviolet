using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Styles
{
    partial class UvssStoryboardKeyframeCollection
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>A <see cref="List{T}.Enumerator"/> that iterates through the collection.</returns>
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
