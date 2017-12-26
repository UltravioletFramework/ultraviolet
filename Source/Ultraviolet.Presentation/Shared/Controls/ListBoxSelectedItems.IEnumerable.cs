using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Controls
{
    public sealed partial class ListBoxSelectedItems : IEnumerable<Object>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public List<Object>.Enumerator GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<Object> IEnumerable<Object>.GetEnumerator()
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
