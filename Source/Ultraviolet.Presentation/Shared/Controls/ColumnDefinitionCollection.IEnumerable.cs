using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Controls
{
    partial class ColumnDefinitionCollection : IEnumerable<ColumnDefinition>
    {
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="List{ColumnDefinition}.Enumerator"/> object that can be used to iterate through the collection.</returns>
        public List<ColumnDefinition>.Enumerator GetEnumerator()
        {
            return storage.Count == 0 ? implicitStorage.GetEnumerator() : storage.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<ColumnDefinition> IEnumerable<ColumnDefinition>.GetEnumerator()
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
