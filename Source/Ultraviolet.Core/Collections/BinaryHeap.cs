using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents a binary heap.
    /// Based on code made available at http://content.gpwiki.org/index.php/C_sharp:BinaryHeapOfT
    /// </summary>
    /// <typeparam name="T">The type of item contained by the heap.</typeparam>
    public sealed partial class BinaryHeap<T> : ICollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryHeap{T}"/> class.
        /// </summary>
        public BinaryHeap()
            : this(null, 4)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryHeap{T}"/> class with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the binary heap.</param>
        public BinaryHeap(Int32 capacity)
            : this(null, capacity)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryHeap{T}"/> class.
        /// </summary>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to use to compare items in the heap.</param>
        public BinaryHeap(IComparer<T> comparer)
            : this(comparer, 4)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryHeap{T}"/> class with the specified initial capacity.
        /// </summary>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to use to compare items in the heap.</param>
        /// <param name="capacity">The initial capacity of the binary heap.</param>
        public BinaryHeap(IComparer<T> comparer, Int32 capacity)
        {
            Contract.EnsureNot<ArgumentOutOfRangeException>(capacity < 0, nameof(capacity));

            this.capacity = capacity;
            this.count = 0;
            this.data = new T[capacity];
            this.reftype = !typeof(T).IsValueType;
            this.comparer = comparer ?? Comparer<T>.Default;
        }

        /// <summary>
        /// Removes all items from the heap.
        /// </summary>
        public void Clear()
        {
            this.count = 0;
            Array.Clear(this.data, 0, this.data.Length);
        }

        /// <summary>
        /// Updates the specified item's position in the heap.
        /// </summary>
        /// <param name="item">The item to update.</param>
        public void Update(T item)
        {
            if (reftype && item == null)
                throw new ArgumentNullException("item");

            var i = Array.BinarySearch<T>(data, 0, count, item, comparer);
            if (i < 0)
                return;

            SiftDown(0, i);
            SiftUp(i);
        }

        /// <summary>
        /// Adds an item to the heap.
        /// </summary>
        /// <param name="item">The item to add to the heap.</param>
        public void Add(T item)
        {
            if (reftype && item == null)
                throw new ArgumentNullException("item");

            if (count == capacity)
                ExpandCapacity();

            data[count++] = item;
            SiftDown(0, count - 1);
        }

        /// <summary>
        /// Adds all of the items in the specified collection to the heap.
        /// </summary>
        /// <param name="collection">The collection that contains the items to add to the heap.</param>
        public void AddRange(IEnumerable<T> collection)
        {
            Contract.Require(collection, nameof(collection));

            foreach (var item in collection)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Removes and returns the first item in the heap.
        /// </summary>
        /// <returns>The next item in the heap.</returns>
        public T Remove()
        {
            Contract.EnsureNot<InvalidOperationException>(IsEmpty, "Heap is empty.");

            T lastelt = data[count - 1];
            T returnitem = default(T);
            if (--count > 0)
            {
                returnitem = data[0];
                data[0] = lastelt;
                SiftUp(0);
            }
            else
            {
                returnitem = lastelt;
            }
            return returnitem;
        }

        /// <summary>
        /// Retrieves the first value from the heap without removing it.
        /// </summary>
        /// <returns>The first element on the heap.</returns>
        public T Peek()
        {
            Contract.EnsureNot<InvalidOperationException>(IsEmpty, "Heap is empty.");

            return this.data[0];
        }

        /// <summary>
        /// Removes an item from the binary heap.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <returns><see langword="true"/> if the item was removed; otherwise, <see langword="false"/>.</returns>
        public bool Remove(T item)
        {
            EnsureSort();

            var i = Array.BinarySearch<T>(data, 0, count, item);
            if (i < 0)
                return false;

            Array.Copy(data, i + 1, data, i, count - (i + 1));
            data[--count] = default(T);

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the heap contains the specified value.
        /// </summary>
        /// <param name="item">The value for which to search the heap.</param>
        /// <returns><see langword="true"/> if the heap contains the specified value; otherwise, <see langword="false"/>.</returns>
        public bool Contains(T item)
        {
            EnsureSort();
            return Array.BinarySearch<T>(data, 0, count, item, comparer) >= 0;
        }

        /// <summary>
        /// Copies the binary heap into an array at the specified index.
        /// </summary>
        /// <param name="array">A one-dimensional array that is the destination of the copied elements.</param>
        /// <param name="arrayIndex">The zero-based index at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            EnsureSort();
            Array.Copy(data, arrayIndex, array, 0, count);
        }

        /// <summary>
        /// Creates a copy of the binary heap.
        /// </summary>
        /// <returns>A copy of the binary heap.</returns>
        public BinaryHeap<T> Copy()
        {
            var copy = new BinaryHeap<T>(comparer, capacity);
            copy.count = count;
            Array.Copy(data, copy.data, count);
            return copy;
        }

        /// <summary>
        /// Gets an enumerator for the binary heap.
        /// </summary>
        /// <returns>An enumerator for the binary heap.</returns>
        public Enumerator GetEnumerator()
        {
            EnsureSort();
            return new Enumerator(this);
        }

        /// <summary>
        /// Gets an enumerator for the binary heap without sorting the heap.
        /// </summary>
        /// <returns>An enumerator for the binary heap.</returns>
        public Enumerator GetEnumeratorUnsorted()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Gets an enumerator for the binary heap.
        /// </summary>
        /// <returns>An enumerator for the binary heap.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the binary heap.
        /// </summary>
        /// <returns>An enumerator for the binary heap.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the number of items in the heap.
        /// </summary>
        public Int32 Count
        {
            get { return count; }
        }

        /// <summary>
        /// Gets the heap's current capacity.
        /// </summary>
        public Int32 Capacity
        {
            get { return capacity; }
        }

        /// <summary>
        /// Gets a value indicating whether this a read-only collection.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the queue is empty.
        /// </summary>
        public Boolean IsEmpty
        {
            get { return Count == 0; }
        }

        /// <summary>
        /// Gets the parent of the specified item.
        /// </summary>
        /// <param name="ix">The index of the item to evaluate.</param>
        /// <returns>The index of the parent of the specified item.</returns>
        private static Int32 GetParent(Int32 ix)
        {
            return (ix - 1) >> 1;
        }

        /// <summary>
        /// Gets the first child of the specified item.
        /// </summary>
        /// <param name="ix">The index of the item to evaluate.</param>
        /// <returns>The index of the first child of the specified item.</returns>
        private static Int32 GetChild1(Int32 ix)
        {
            return (ix << 1) + 1;
        }

        /// <summary>
        /// Gets the second child of the specified item.
        /// </summary>
        /// <param name="ix">The index of the item to evaluate.</param>
        /// <returns>The index of the second child of the specified item.</returns>
        private static Int32 GetChild2(Int32 ix)
        {
            return (ix << 1) + 2;
        }

        /// <summary>
        /// Expands the heap's capacity.
        /// </summary>
        private void ExpandCapacity()
        {
            var prev = capacity;
            capacity = Math.Max(prev * 2, count);
            if (capacity != prev)
            {
                var temp = new T[capacity];
                Array.Copy(data, temp, count);
                data = temp;
            }
        }

        /// <summary>
        /// Performs up-heap bubbling.
        /// </summary>
        private void SiftUp(Int32 pos)
        {
            sorted = false;

            var endpos = count;
            var startpos = pos;
            var newitem = data[pos];
            var childpos = 2 * pos + 1;
            var rightpos = 0;

            while (childpos < endpos)
            {
                rightpos = childpos + 1;
                if (rightpos < endpos && comparer.Compare(data[childpos], data[rightpos]) >= 0)
                {
                    childpos = rightpos;
                }
                data[pos] = data[childpos];
                pos = childpos;
                childpos = 2 * pos + 1;
            }
            data[pos] = newitem;
            SiftDown(startpos, pos);
        }

        /// <summary>
        /// Performs down-heap bubbling.
        /// </summary>
        private void SiftDown(Int32 startpos, Int32 pos)
        {
            sorted = false;

            var parentpos = 0;
            var parent = default(T);
            var newitem = data[pos];
            while (pos > startpos)
            {
                parentpos = (pos - 1) >> 1;
                parent = data[parentpos];
                if (comparer.Compare(newitem, parent) < 0)
                {
                    data[pos] = parent;
                    pos = parentpos;
                    continue;
                }
                break;
            }
            data[pos] = newitem;
        }

        /// <summary>
        /// Ensures that the data store is sorted.
        /// </summary>
        private void EnsureSort()
        {
            if (sorted)
                return;

            Array.Sort<T>(data, 0, count, comparer);
            sorted = true;
        }

        // Property values.
        private int count;
        private int capacity;

        // The underlying data store.
        private T[] data;
        private bool sorted;
        private bool reftype;
        private IComparer<T> comparer;
    }
}
