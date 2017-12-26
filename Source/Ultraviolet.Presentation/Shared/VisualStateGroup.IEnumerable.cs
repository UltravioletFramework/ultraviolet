using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation
{
    partial class VisualStateGroup
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TValue}.Enumerator"/> that iterates through the collection.</returns>
        public Dictionary<String, VisualState>.Enumerator GetEnumerator()
        {
            return states.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<String, VisualState>> IEnumerable<KeyValuePair<String, VisualState>>.GetEnumerator()
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
