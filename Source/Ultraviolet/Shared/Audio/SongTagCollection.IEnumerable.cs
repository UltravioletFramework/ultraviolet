using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Audio
{
    partial class SongTagCollection
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public Dictionary<String, SongTag>.Enumerator GetEnumerator() =>
            storage.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<String, SongTag>> IEnumerable<KeyValuePair<String, SongTag>>.GetEnumerator() => 
            GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
