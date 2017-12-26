using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.UI
{
    partial class UIScreenStack
    {
        /// <inheritdoc/>
        IEnumerator<UIScreen> IEnumerable<UIScreen>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public LinkedList<UIScreen>.Enumerator GetEnumerator() => screens.GetEnumerator();
    }
}
