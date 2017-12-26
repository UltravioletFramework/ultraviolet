using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an enumerable collection.
    /// </summary>
    /// <typeparam name="T">The type of item stored in the collection.</typeparam>
    public abstract partial class UltravioletCollection<T> : IEnumerable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletCollection{T}"/> class.
        /// </summary>
        protected UltravioletCollection()
        {
            this.storage = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletCollection{T}"/> class.
        /// </summary>
        /// <param name="capacity">The collection's initial capacity.</param>
        protected UltravioletCollection(Int32 capacity)
        {
            this.storage = new List<T>(capacity);
        }

        /// <summary>
        /// Gets an enumerator for the result.
        /// </summary>
        /// <returns>An enumerator for the result.</returns>
        public List<T>.Enumerator GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the result.
        /// </summary>
        /// <returns>An enumerator for the result.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the result.
        /// </summary>
        /// <returns>An enumerator for the result.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the item at the specified index within the collection.
        /// </summary>
        /// <param name="ix">The index of the item to retrieve.</param>
        /// <returns>The item at the specified index within the collection.</returns>
        public T this[Int32 ix]
        {
            get { return storage[ix]; }
            protected set { storage[ix] = value; }
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return storage.Count; }
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        protected virtual void ClearInternal()
        {
            storage.Clear();
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The item to add to the collection.</param>
        protected virtual void AddInternal(T item)
        {
            storage.Add(item);
        }

        /// <summary>
        /// Adds the items in the specified collection to the end of the collection.
        /// </summary>
        /// <param name="collection">The collection whose items should be added to the end of the collection.</param>
        protected virtual void AddRangeInternal(IEnumerable<T> collection)
        {
            storage.AddRange(collection);
        }

        /// <summary>
        /// Inserts the specified item at the specified index within the collection.
        /// </summary>
        /// <param name="index">The index at which to insert the item.</param>
        /// <param name="item">The item to insert into the collection.</param>
        protected virtual void InsertInternal(Int32 index, T item)
        {
            storage.Insert(index, item);
        }

        /// <summary>
        /// Removes the element at the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the item to remove from the collection.</param>
        protected virtual void RemoveAtInternal(Int32 index)
        {
            storage.RemoveAt(index);
        }

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="item">The item to remove from the collection.</param>
        /// <returns><see langword="true"/> if the item was removed from the collection; otherwise, <see langword="false"/>.</returns>
        protected virtual Boolean RemoveInternal(T item)
        {
            return storage.Remove(item);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><see langword="true"/> if the collection contains the specified item; otherwise, <see langword="false"/>.</returns>
        protected virtual Boolean ContainsInternal(T item)
        {
            return storage.Contains(item);
        }

        /// <summary>
        /// Gets the index of the specified item.
        /// </summary>
        /// <param name="item">The item for which to retrieve an index.</param>
        /// <returns>The index of the specified item within the collection, or -1 if the collection does not contain the item.</returns>
        protected virtual Int32 IndexOfInternal(T item)
        {
            return storage.IndexOf(item);
        }

        /// <summary>
        /// Gets the collection's underlying storage.
        /// </summary>
        protected List<T> Storage
        {
            get { return storage; }
        }

        // The collection's backing storage.
        private readonly List<T> storage;
    }
}
