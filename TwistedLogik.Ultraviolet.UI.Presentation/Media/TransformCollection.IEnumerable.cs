using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Media
{
    partial class TransformCollection : IEnumerable<Transform>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public List<Transform>.Enumerator GetEnumerator()
        {
            return transforms.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<Transform> IEnumerable<Transform>.GetEnumerator()
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
