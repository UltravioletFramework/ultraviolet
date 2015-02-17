using System;
using System.Collections;
using System.Collections.Generic;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Collections
{
    /// <summary>
    /// Represents the method that is called when an observable list performs an operation
    /// that is not related to a specific item.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the list.</typeparam>
    /// <param name="list">The list that raised the event.</param>
    public delegate void ObservableListEvent<T>(ObservableList<T> list);

    /// <summary>
    /// Represents a method that is called when an observable list performs an operation
    /// relating to a specific item.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the list.</typeparam>
    /// <param name="list">The list that raised the event.</param>
    /// <param name="item">The item that is the target of the operation.</param>
    public delegate void ObservableListItemEvent<T>(ObservableList<T> list, T item);

    /// <summary>
    /// Represents a list which raises events when items are added or removed.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the list.</typeparam>
    public class ObservableList<T> : IList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        public ObservableList()
        {
            this.list                 = new List<T>();
            this.notifyPropertyChange = typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the list.</param>
        public ObservableList(Int32 capacity)
        {
            this.list                 = new List<T>(capacity);
            this.notifyPropertyChange = typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class that contains the elements 
        /// contained by the specified collection.
        /// </summary>
        /// <param name="collection">The collection that contains the elements to copy to this collection.</param>
        public ObservableList(IEnumerable<T> collection)
        {
            this.list                 = new List<T>(collection);
            this.notifyPropertyChange = typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T));
        }

        /// <summary>
        /// Retrieves the index of the specified item within the list, if the list contains the item.
        /// </summary>
        /// <param name="item">The item to search for within the list.</param>
        /// <returns>The index of the item within the list, or -1 if the list does not contain the item.</returns>
        public Int32 IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        /// <summary>
        /// Reverses the list.
        /// </summary>
        public void Reverse()
        {
            list.Reverse();
            OnChanged();
        }

        /// <summary>
        /// Reverses the specified range of elements within the list.
        /// </summary>
        /// <param name="index">The index of the first element in the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        public void Reverse(Int32 index, Int32 count)
        {
            list.Reverse(index, count);
            OnChanged();
        }

        /// <summary>
        /// Sorts the list using the default comparer.
        /// </summary>
        public void Sort()
        {
            list.Sort();
            OnChanged();
        }

        /// <summary>
        /// Sorts the list using the specified comparison.
        /// </summary>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use to sort the list.</param>
        public void Sort(Comparison<T> comparison)
        {
            list.Sort(comparison);
            OnChanged();
        }

        /// <summary>
        /// Sorts the list using the specified comparer.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use to sort the list.</param>
        public void Sort(IComparer<T> comparer)
        {
            list.Sort(comparer);
            OnChanged();
        }
        
        /// <summary>
        /// Inserts an item into the list at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert the item.</param>
        /// <param name="item">The item to insert into the list.</param>
        public void Insert(Int32 index, T item)
        {
            list.Insert(index, item);
            OnItemAdded(item);
            OnChanged();

            HookPropertyChanged(item);
        }

        /// <summary>
        /// Adds an item to the end of the list.
        /// </summary>
        /// <param name="item">The item to add to the end of the list.</param>
        public void Add(T item)
        {
            list.Add(item);
            OnItemAdded(item);
            OnChanged();

            HookPropertyChanged(item);
        }

        /// <summary>
        /// Removes the specified item from the list, if it exists in the list.
        /// </summary>
        /// <param name="item">The item to remove from the list.</param>
        /// <returns><c>true</c> if the specified item was removed from the list; otherwise, <c>false</c>.</returns>
        public Boolean Remove(T item)
        {
            if (list.Remove(item))
            {
                UnhookPropertyChanged(item);

                OnItemRemoved(item);
                OnChanged();

                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the item at the specified index from the list.
        /// </summary>
        /// <param name="index">The index of the item to remove from the list.</param>
        public void RemoveAt(Int32 index)
        {
            var item = list[index];
            list.RemoveAt(index);

            UnhookPropertyChanged(item);

            OnItemRemoved(item);
            OnChanged();
        }

        /// <summary>
        /// Removes all items from the list.
        /// </summary>
        public void Clear()
        {
            if (notifyPropertyChange)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ((INotifyPropertyChanged)list[i]).PropertyChanged -= ItemPropertyChanged;
                }
            }
            list.Clear();
            OnCleared();
            OnChanged();
        }

        /// <summary>
        /// Gets a value indicating whether the list contains the specified item.
        /// </summary>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><c>true</c> if the list contains the specified item; otherwise, <c>false</c>.</returns>
        public Boolean Contains(T item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Copies the entire list to the specified array.
        /// </summary>
        /// <param name="array">The array into which to copy the list's elements.</param>
        public void CopyTo(T[] array)
        {
            list.CopyTo(array);
        }

        /// <summary>
        /// Copies the entire list to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array into which to copy the list's elements.</param>
        /// <param name="arrayIndex">The index within the array at which to begin copying elements.</param>
        public void CopyTo(T[] array, Int32 arrayIndex)
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
        public void CopyTo(Int32 index, T[] array, Int32 arrayIndex, Int32 count)
        {
            list.CopyTo(index, array, arrayIndex, count);
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public List<T>.Enumerator GetEnumerator()
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
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the item at the specified index within the list.
        /// </summary>
        /// <param name="index">The index of the item to get or set.</param>
        /// <returns>The item at the specified index within the list.</returns>
        public T this[Int32 index]
        {
            get { return list[index]; }
            set 
            {
                var existing = list[index];

                UnhookPropertyChanged(existing);
                OnItemRemoved(existing);

                list[index] = value;

                OnItemAdded(value);
                OnChanged();

                HookPropertyChanged(value);
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
        /// Gets a value indicating whether the list is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Occurs whenever an operation is performed which modifies the contents of the list or their order.
        /// </summary>
        public ObservableListEvent<T> Changed;

        /// <summary>
        /// Occurs when the list is cleared.
        /// </summary>
        public ObservableListEvent<T> Cleared;

        /// <summary>
        /// Occurs when an item is added to the list.
        /// </summary>
        public ObservableListItemEvent<T> ItemAdded;

        /// <summary>
        /// Occurs when an item is removed from the list.
        /// </summary>
        public ObservableListItemEvent<T> ItemRemoved;

        /// <summary>
        /// Raises the <see cref="Changed"/> event.
        /// </summary>
        protected virtual void OnChanged()
        {
            var temp = Changed;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Cleared"/> event.
        /// </summary>
        protected virtual void OnCleared()
        {
            var temp = Cleared;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemAdded"/> event.
        /// </summary>
        /// <param name="item">The item that was added to the list.</param>
        protected virtual void OnItemAdded(T item)
        {
            var temp = ItemAdded;
            if (temp != null)
            {
                temp(this, item);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemRemoved"/> event.
        /// </summary>
        /// <param name="item">The item that was added to the list.</param>
        protected virtual void OnItemRemoved(T item)
        {
            var temp = ItemRemoved;
            if (temp != null)
            {
                temp(this, item);
            }
        }

        /// <summary>
        /// Handles the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the list's items.
        /// </summary>
        /// <param name="instance">The object instance that changed.</param>
        /// <param name="propertyName">The name of the property that was changed. If all of the object's properties have
        /// changed, this value can be either <see cref="String.Empty"/> or <c>null</c>.</param>
        private void ItemPropertyChanged(Object instance, String propertyName)
        {
            // TODO
        }

        /// <summary>
        /// Hooks into the specified item's <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="item">The item for which to add a hook.</param>
        private void HookPropertyChanged(T item)
        {
            if (notifyPropertyChange && item != null)
            {
                ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
            }
        }

        /// <summary>
        /// Unhooks from the specified item's <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="item">The item for which to remove a hook.</param>
        private void UnhookPropertyChanged(T item)
        {
            if (notifyPropertyChange && item != null)
            {
                ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
            }
        }

        // The wrapped list which contains our items.
        private readonly List<T> list;
        private readonly Boolean notifyPropertyChange;
    }
}
