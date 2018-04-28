using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents a list which raises events when items are added or removed.
    /// </summary>
    /// <typeparam name="TValue">The type of item contained by the list.</typeparam>
    public class ObservableList<TValue> : IList<TValue>, INotifyCollectionChanged<Int32, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        public ObservableList()
        {
            this.list = new List<TValue>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the list.</param>
        public ObservableList(Int32 capacity)
        {
            this.list = new List<TValue>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class that contains the elements 
        /// contained by the specified collection.
        /// </summary>
        /// <param name="collection">The collection that contains the elements to copy to this collection.</param>
        public ObservableList(IEnumerable<TValue> collection)
        {
            this.list = new List<TValue>(collection);
        }

        /// <summary>
        /// Retrieves the index of the specified item within the list, if the list contains the item.
        /// </summary>
        /// <param name="item">The item to search for within the list.</param>
        /// <returns>The index of the item within the list, or -1 if the list does not contain the item.</returns>
        public Int32 IndexOf(TValue item)
        {
            return list.IndexOf(item);
        }

        /// <summary>
        /// Reverses the list.
        /// </summary>
        public void Reverse()
        {
            list.Reverse();
            OnCollectionReset();
        }

        /// <summary>
        /// Reverses the specified range of elements within the list.
        /// </summary>
        /// <param name="index">The index of the first element in the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        public void Reverse(Int32 index, Int32 count)
        {
            list.Reverse(index, count);
            OnCollectionReset();
        }

        /// <summary>
        /// Sorts the list using the default comparer.
        /// </summary>
        public void Sort()
        {
            list.Sort();
            OnCollectionReset();
        }

        /// <summary>
        /// Sorts the list using the specified comparison.
        /// </summary>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use to sort the list.</param>
        public void Sort(Comparison<TValue> comparison)
        {
            list.Sort(comparison);
            OnCollectionReset();
        }

        /// <summary>
        /// Sorts the list using the specified comparer.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use to sort the list.</param>
        public void Sort(IComparer<TValue> comparer)
        {
            list.Sort(comparer);
            OnCollectionReset();
        }

        /// <summary>
        /// Removes all items from the list.
        /// </summary>
        public void Clear()
        {
            list.Clear();
            OnCollectionReset();
        }

        /// <summary>
        /// Inserts an item into the list at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert the item.</param>
        /// <param name="item">The item to insert into the list.</param>
        public void Insert(Int32 index, TValue item)
        {
            list.Insert(index, item);
            OnCollectionItemAdded(index, item);
        }

        /// <summary>
        /// Adds an item to the end of the list.
        /// </summary>
        /// <param name="item">The item to add to the end of the list.</param>
        public void Add(TValue item)
        {
            list.Add(item);
            OnCollectionItemAdded(list.Count - 1, item);
        }

        /// <summary>
        /// Removes the item at the specified index from the list.
        /// </summary>
        /// <param name="index">The index of the item to remove from the list.</param>
        public void RemoveAt(Int32 index)
        {
            var item = list[index];
            list.RemoveAt(index);

            OnCollectionItemRemoved(index, item);
        }

        /// <summary>
        /// Removes the specified item from the list, if it exists in the list.
        /// </summary>
        /// <param name="item">The item to remove from the list.</param>
        /// <returns><see langword="true"/> if the specified item was removed from the list; otherwise, <see langword="false"/>.</returns>
        public Boolean Remove(TValue item)
        {
            var index = list.IndexOf(item);
            if (index >= 0)
            {
                list.RemoveAt(index);
                OnCollectionItemRemoved(index, item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the list contains the specified item.
        /// </summary>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><see langword="true"/> if the list contains the specified item; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(TValue item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Copies the entire list to the specified array.
        /// </summary>
        /// <param name="array">The array into which to copy the list's elements.</param>
        public void CopyTo(TValue[] array)
        {
            list.CopyTo(array);
        }

        /// <summary>
        /// Copies the entire list to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array into which to copy the list's elements.</param>
        /// <param name="arrayIndex">The index within the array at which to begin copying elements.</param>
        public void CopyTo(TValue[] array, Int32 arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copies the specified number of elements from this list into the specified array.
        /// </summary>
        /// <param name="index">The index within the list at which to begin copying elements.</param>
        /// <param name="array">The array into which to copy the list's elements.</param>
        /// <param name="arrayIndex">The index within the array at which to begin copying elements.</param>
        /// <param name="count">The number of elements to copy.</param>
        public void CopyTo(Int32 index, TValue[] array, Int32 arrayIndex, Int32 count)
        {
            list.CopyTo(index, array, arrayIndex, count);
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public List<TValue>.Enumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the item at the specified index within the list.
        /// </summary>
        /// <param name="index">The index of the item to get or set.</param>
        /// <returns>The item at the specified index within the list.</returns>
        public TValue this[Int32 index]
        {
            get { return list[index]; }
            set 
            {
                var existing = list[index];
                OnCollectionItemRemoved(index, existing);

                list[index] = value;

                OnCollectionItemAdded(index, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of items that the list can hold before it is resized.
        /// </summary>
        public Int32 Capacity
        {
            get { return list.Capacity; }
            set { list.Capacity = value; }
        }

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        public Int32 Count
        {
            get { return list.Count; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this collection is suppressing the untyped events raised by
        /// the non-generic <see cref="INotifyCollectionChanged"/> interface. Where these events are not necessary,
        /// suppressing them may be useful for performance reasons because it can prevent boxing if the collection
        /// contains value types.
        /// </summary>
        public Boolean SuppressUntypedNotifications
        {
            get { return suppressUntypedNotifications; }
            set { suppressUntypedNotifications = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the list is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc/>
        event CollectionResetEventHandler INotifyCollectionChanged.CollectionReset
        {
            add { lock (untypedEventSyncObject) { untypedCollectionReset += value; } }
            remove { lock (untypedEventSyncObject) { untypedCollectionReset -= value; } }
        }

        /// <inheritdoc/>
        event CollectionItemAddedEventHandler INotifyCollectionChanged.CollectionItemAdded
        {
            add { lock (untypedEventSyncObject) { untypedCollectionItemAdded += value; } }
            remove { lock (untypedEventSyncObject) { untypedCollectionItemAdded -= value; } }
        }

        /// <inheritdoc/>
        event CollectionItemRemovedEventHandler INotifyCollectionChanged.CollectionItemRemoved
        {
            add { lock (untypedEventSyncObject) { untypedCollectionItemRemoved += value; } }
            remove { lock (untypedEventSyncObject) { untypedCollectionItemRemoved -= value; } }
        }

        /// <inheritdoc/>
        public event CollectionResetEventHandler<Int32, TValue> CollectionReset;

        /// <inheritdoc/>
        public event CollectionItemAddedEventHandler<Int32, TValue> CollectionItemAdded;

        /// <inheritdoc/>
        public event CollectionItemRemovedEventHandler<Int32, TValue> CollectionItemRemoved;

        /// <summary>
        /// Raises the <see cref="CollectionReset"/> event.
        /// </summary>
        protected virtual void OnCollectionReset()
        {
            CollectionReset?.Invoke(this);

            if (suppressUntypedNotifications)
                return;

            untypedCollectionReset?.Invoke(this);
        }

        /// <summary>
        /// Raises the <see cref="CollectionItemAdded"/> event.
        /// </summary>
        /// <param name="index">The index at which the item was added to the list.</param>
        /// <param name="value">The item that was added to the list.</param>
        protected virtual void OnCollectionItemAdded(Int32 index, TValue value)
        {
            CollectionItemAdded?.Invoke(this, index, value);

            if (suppressUntypedNotifications)
                return;

            untypedCollectionItemAdded?.Invoke(this, index, value);
        }

        /// <summary>
        /// Raises the <see cref="CollectionItemRemoved"/> event.
        /// </summary>
        /// <param name="index">The index at which the item was removed from the list.</param>
        /// <param name="value">The item that was added to the list.</param>
        protected virtual void OnCollectionItemRemoved(Int32 index, TValue value)
        {
            CollectionItemRemoved?.Invoke(this, index, value);

            if (suppressUntypedNotifications)
                return;

            untypedCollectionItemRemoved?.Invoke(this, index, value);
        }

        // The wrapped list which contains our items.
        private readonly List<TValue> list;

        // Explicitly implemented events belonging to INotifyCollectionChanged.
        private readonly Object untypedEventSyncObject = new Object();
        private CollectionResetEventHandler untypedCollectionReset;
        private CollectionItemAddedEventHandler untypedCollectionItemAdded;
        private CollectionItemRemovedEventHandler untypedCollectionItemRemoved;
        private Boolean suppressUntypedNotifications;
    }
}
