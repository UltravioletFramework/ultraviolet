using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation
{
    partial class TouchesCollection : IEnumerable<Int64>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public HashSet<Int64>.Enumerator GetEnumerator()
        {
            return touches.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return touches.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<Int64> IEnumerable<Int64>.GetEnumerator()
        {
            return touches.GetEnumerator();
        }
    }
}
