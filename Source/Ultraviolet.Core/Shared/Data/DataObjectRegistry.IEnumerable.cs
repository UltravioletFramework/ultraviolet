using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Core.Data
{
    partial class DataObjectRegistry<T> : IEnumerable<KeyValuePair<String, T>>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public Dictionary<String, T>.Enumerator GetEnumerator() => 
            objectsByKey.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<String, T>> IEnumerable<KeyValuePair<String, T>>.GetEnumerator() => 
            GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
