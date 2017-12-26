using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation
{
    partial class VisualStateGroupCollection
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TValue}.Enumerator"/> that iterates through the collection.</returns>
        public Dictionary<String, VisualStateGroup>.Enumerator GetEnumerator()
        {
            return groups.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<String, VisualStateGroup>> IEnumerable<KeyValuePair<String, VisualStateGroup>>.GetEnumerator()
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
