using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of resources associated with a <see cref="Model"/> object.
    /// </summary>
    /// <typeparam name="TItem">The type of item contained by the collection.</typeparam>
    public abstract class ModelResourceCollection<TItem> : IEnumerable<TItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelResourceCollection{TItem}"/> class.
        /// </summary>
        internal ModelResourceCollection(IEnumerable<TItem> items, Func<TItem, Int32, Int32> indexSelector = null) 
        {
            if (indexSelector == null)
                indexSelector = (item, ix) => ix;

            var count = items?.Count() ?? 0;
            storage = new TItem[count];
            if (count > 0)
            {
                var index = 0;
                foreach (var item in items)
                {
                    var trueIndex = indexSelector(item, index);
                    storage[trueIndex] = item;
                    index++;
                }
            }
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator"/> for the collection.
        /// </summary>
        /// <returns>An <see cref="ArrayEnumerator{T}"/> which will enumerate through the collection.</returns>
        ArrayEnumerator<TItem> GetEnumerator() => new ArrayEnumerator<TItem>(storage);

        /// <inheritdoc/>
        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets the item at the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the item to retrieve.</param>
        /// <returns>The item at the specified index within the collection.</returns>
        public TItem this[Int32 index] => storage[index];

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public Int32 Count => storage.Length;

        // The collection's backing storage.
        private readonly TItem[] storage;
    }
}
