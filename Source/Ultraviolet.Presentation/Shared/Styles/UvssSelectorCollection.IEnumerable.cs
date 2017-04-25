using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Styles
{
    partial class UvssSelectorCollection
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>A <see cref="List{T}.Enumerator"/> that iterates through the collection.</returns>
        public List<UvssSelector>.Enumerator GetEnumerator()
        {
            return selectors.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssSelector> IEnumerable<UvssSelector>.GetEnumerator()
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
