using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Styles
{
    partial class UvssStoryboardTargetCollection
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>A <see cref="List{T}.Enumerator"/> that iterates through the collection.</returns>
        public List<UvssStoryboardTarget>.Enumerator GetEnumerator()
        {
            return targets.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssStoryboardTarget> IEnumerable<UvssStoryboardTarget>.GetEnumerator()
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
