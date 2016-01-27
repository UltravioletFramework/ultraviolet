using System;
using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class UIElementClassCollection
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>A <see cref="HashSet{T}.Enumerator"/> that iterates through the collection.</returns>
        public HashSet<String>.Enumerator GetEnumerator()
        {
            return classes.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<String> IEnumerable<String>.GetEnumerator()
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
