using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Core.Text
{
    partial class LocalizedString
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public Dictionary<String, LocalizedStringVariant>.Enumerator GetEnumerator() =>
            variants.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<String, LocalizedStringVariant>> IEnumerable<KeyValuePair<String, LocalizedStringVariant>>.GetEnumerator() => 
            GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}
