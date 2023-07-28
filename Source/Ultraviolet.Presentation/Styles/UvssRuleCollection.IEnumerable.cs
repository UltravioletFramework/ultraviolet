using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Styles
{
    partial class UvssRuleCollection
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>A <see cref="List{T}.Enumerator"/> that iterates through the collection.</returns>
        public List<UvssRule>.Enumerator GetEnumerator()
        {
            return rules.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssRule> IEnumerable<UvssRule>.GetEnumerator()
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
