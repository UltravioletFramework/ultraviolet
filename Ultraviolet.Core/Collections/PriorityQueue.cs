using System;

namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents a priority queue.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the queue.</typeparam>
    public sealed class PriorityQueue<T>
    {
        /// <summary>
        /// Represents an item in a priority queue.
        /// </summary>
        private struct PriorityQueueItem : IComparable<PriorityQueueItem>
        {
            /// <summary>
            /// Initializes a new instance of the PriorityQueueItem structure.
            /// </summary>
            /// <param name="priority">The item's priority in the queue.</param>
            /// <param name="value">The item's value.</param>
            public PriorityQueueItem(Single priority, T value)
            {
                this.Priority = priority;
                this.Value = value;
            }

            /// <summary>
            /// Compares the current object with another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>A value that indicates the relative order of the objects being compared.</returns>
            public Int32 CompareTo(PriorityQueueItem other)
            {
                return Priority.CompareTo(other.Priority);
            }

            /// <summary>
            /// The item's priority in the queue.
            /// </summary>
            public readonly Single Priority;

            /// <summary>
            /// The item's value.
            /// </summary>
            public readonly T Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> class.
        /// </summary>
        public PriorityQueue()
            : this(4)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> class.
        /// </summary>
        /// <param name="capacity">The priority queue's initial capacity.</param>
        public PriorityQueue(Int32 capacity)
        {
            Heap = new BinaryHeap<PriorityQueueItem>(capacity);
        }

        /// <summary>
        /// Removes all items from the queue.
        /// </summary>
        public void Clear()
        {
            Heap.Clear();
        }

        /// <summary>
        /// Adds an item to the queue.
        /// </summary>
        /// <param name="priority">The item's priority.</param>
        /// <param name="value">The item's value.</param>
        public void Enqueue(Single priority, T value)
        {
            Heap.Add(new PriorityQueueItem(priority, value));
        }

        /// <summary>
        /// Removes the next item from the queue.
        /// </summary>
        /// <returns>The item that was removed from the queue.</returns>
        public T Dequeue()
        {
            Contract.EnsureNot(Count == 0, CoreStrings.QueueIsEmpty);
            
            var priority = 0f;
            return Dequeue(out priority);
        }

        /// <summary>
        /// Removes the next item from the queue.
        /// </summary>
        /// <param name="priority">The item's priority in the queue.</param>
        /// <returns>The item that was removed from the queue.</returns>
        public T Dequeue(out Single priority)
        {
            Contract.EnsureNot(Count == 0, CoreStrings.QueueIsEmpty);

            var item = Heap.Remove();
            priority = item.Priority;
            return item.Value;
        }

        /// <summary>
        /// Retrieves the first value from the queue without removing it.
        /// </summary>
        /// <returns>The next value on the queue.</returns>
        public T Peek()
        {
            Contract.EnsureNot(Count == 0, CoreStrings.QueueIsEmpty);

            return Heap.Peek().Value;
        }

        /// <summary>
        /// Gets the number of items in the queue.
        /// </summary>
        public Int32 Count
        {
            get { return Heap.Count; }
        }

        /// <summary>
        /// Gets the queue's current capacity.
        /// </summary>
        public Int32 Capacity
        {
            get { return Heap.Capacity; }
        }

        /// <summary>
        /// Gets a value indicating whether the queue is empty.
        /// </summary>
        public Boolean IsEmpty
        {
            get { return Count == 0; }
        }

        // The queue's underlying binary heap.
        private readonly BinaryHeap<PriorityQueueItem> Heap;
    }
}
