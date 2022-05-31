using System;
using System.Collections.Generic;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents the selected items in a list box.
    /// </summary>
    public sealed partial class ListBoxSelectedItems
    {
        /// <summary>
        /// Gets a value indicating whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><see langword="true"/> if the collection contains the specified item; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(Object item)
        {
            return storage.Contains(item);
        }

        /// <summary>
        /// Gets the index of the specified item within the collection.
        /// </summary>
        /// <param name="item">The item to evaluate.</param>
        /// <returns>The index of the specified item within the collection, or -1 if the item does not exist within the collection.</returns>
        public Int32 IndexOf(Object item)
        {
            return storage.IndexOf(item);
        }

        /// <summary>
        /// Copies the contents of the collection to the specified array.
        /// </summary>
        /// <param name="array">The array to which to copy the collection.</param>
        public void CopyTo(Object[] array)
        {
            storage.CopyTo(array);
        }

        /// <summary>
        /// Copies the contents of the collection to the specified array.
        /// </summary>
        /// <param name="array">The array to which to copy the collection.</param>
        /// <param name="arrayIndex">The index within <paramref name="array"/> at which to begin copying.</param>
        public void CopyTo(Object[] array, Int32 arrayIndex)
        {
            storage.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the object at the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the object to retrieve.</param>
        /// <returns>The object at the specified index within the collection.</returns>
        public Object this[Int32 index]
        {
            get { return storage[index]; }
        }
        
        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return storage.Count; }
        }

        /// <summary>
        /// Adds an object to the collection.
        /// </summary>
        /// <param name="item">The item to add to the collection.</param>
        internal void Add(Object item)
        {
            storage.Add(item);
        }

        /// <summary>
        /// Removes an object from the collection.
        /// </summary>
        /// <param name="item">The item to remove from the collection.</param>
        internal void Remove(Object item)
        {
            storage.Remove(item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        internal void Clear()
        {
            storage.Clear();
        }

        // State values.
        private readonly List<Object> storage = new List<Object>(8);
    }
}
