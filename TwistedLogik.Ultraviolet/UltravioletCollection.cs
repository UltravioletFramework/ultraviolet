using System;
using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet
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
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="item">The item to remove from the collection.</param>
        /// <returns><c>true</c> if the item was removed from the collection; otherwise, <c>false</c>.</returns>
        protected virtual Boolean RemoveInternal(T item)
        {
            return storage.Remove(item);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><c>true</c> if the collection contains the specified item; otherwise, <c>false</c>.</returns>
        protected virtual Boolean ContainsInternal(T item)
        {
            return storage.Contains(item);
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
