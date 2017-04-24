using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Media
{
    public partial class VisualCollection : IList<Visual>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>A <see cref="List{T}.Enumerator"/> that iterates through the collection.</returns>
        public List<Visual>.Enumerator GetEnumerator()
        {
            if (storage == null)
                return emptyStorage.GetEnumerator();

            return storage.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<Visual> IEnumerable<Visual>.GetEnumerator()
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
