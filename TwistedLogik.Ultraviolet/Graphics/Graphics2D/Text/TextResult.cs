using System;
using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the result of a text processing operation.
    /// </summary>
    /// <typeparam name="T">The type of token contained by the result collection.</typeparam>
    public abstract partial class TextResult<T> : IEnumerable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextResult{T}"/> class.
        /// </summary>
        internal TextResult() { }

        /// <summary>
        /// Clears the result.
        /// </summary>
        public virtual void Clear()
        {
            storage.Clear();
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
        /// Adds an item to the result.
        /// </summary>
        /// <param name="item">The item to add to the result.</param>
        internal void Add(T item)
        {
            storage.Add(item);
        }

        /// <summary>
        /// Removes the item at the specified index from the result.
        /// </summary>
        /// <param name="index">The index of the item to remove from the result.</param>
        internal void RemoveAt(Int32 index)
        {
            storage.RemoveAt(index);
        }
        
        /// <summary>
        /// Removes the specified range of items from the result.
        /// </summary>
        /// <param name="index">The index of the first element to remove from the result.</param>
        /// <param name="count">The number of items to remove from the result.</param>
        internal void RemoveRange(Int32 index, Int32 count)
        {
            storage.RemoveRange(index, count);
        }

        /// <summary>
        /// Inserts a range of elements into the result at the specified index.
        /// </summary>
        /// <param name="index">The index at which to begin inserting elements.</param>
        /// <param name="collection">The collection of elements to insert into the result.</param>
        internal void InsertRange(Int32 index, IEnumerable<T> collection)
        {
            storage.InsertRange(index, collection);
        }
        
        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="ix">The item of the token to retrieve.</param>
        /// <returns>The item at the specified index.</returns>
        public T this[Int32 ix]
        {
            get { return storage[ix]; }
            internal set { storage[ix] = value; }
        }

        /// <summary>
        /// Gets the number of items in the result.
        /// </summary>
        public Int32 Count
        {
            get { return storage.Count; }
        }

        // The backing storage for this result.
        private readonly List<T> storage = new List<T>();
    }
}
