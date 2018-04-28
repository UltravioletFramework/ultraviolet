using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Controls
{
    partial class ItemCollection : IList<Object>
    {
        /// <inheritdoc/>
        public void CopyTo(Object[] array, Int32 arrayIndex)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(arrayIndex >= 0, nameof(arrayIndex));

            if (IsBoundToItemsSource)
            {
                foreach (var item in itemsSource)
                    array[arrayIndex++] = item;
            }
            else
            {
                if (arrayIndex + itemsStorage.Count > array.Length)
                    throw new ArgumentException(UltravioletStrings.BufferIsWrongSize);

                for (int i = 0; i < itemsStorage.Count; i++)
                    array[arrayIndex + i] = itemsStorage[i];
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            EnsureNotBoundToItemsSource();

            if (Count == 0)
                return;

            itemsStorage.Clear();
            OnCollectionReset();
        }

        /// <inheritdoc/>
        public void Add(Object item)
        {
            EnsureNotBoundToItemsSource();

            itemsStorage.Add(item);
            OnCollectionItemAdded(itemsStorage.Count - 1, item);
        }

        /// <summary>
        /// Adds the specified collection of items to this collection.
        /// </summary>
        /// <param name="items">The collection of items to add to this collection.</param>
        public void AddRange(IEnumerable<Object> items)
        {
            Contract.Require(items, nameof(items));

            EnsureNotBoundToItemsSource();

            foreach (var item in items)
            {
                itemsStorage.Add(item);
                OnCollectionItemAdded(itemsStorage.Count - 1, item);
            }
        }

        /// <inheritdoc/>
        public void Insert(Int32 index, Object item)
        {
            EnsureNotBoundToItemsSource();

            itemsStorage.Insert(index, item);
            OnCollectionItemAdded(index, item);
        }

        /// <inheritdoc/>
        public void RemoveAt(Int32 index)
        {
            EnsureNotBoundToItemsSource();

            var item = itemsStorage[index];
            itemsStorage.RemoveAt(index);
            OnCollectionItemRemoved(index, item);
        }

        /// <inheritdoc/>
        public Boolean Remove(Object item)
        {
            EnsureNotBoundToItemsSource();

            var index = itemsStorage.IndexOf(item);
            if (index >= 0)
            {
                itemsStorage.RemoveAt(index);
                OnCollectionItemRemoved(index, item);
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public Boolean Contains(Object item)
        {
            return IndexOf(item) >= 0;
        }

        /// <inheritdoc/>
        public Int32 IndexOf(Object item)
        {
            if (IsBoundToItemsSource)
            {
                var list = itemsSource as System.Collections.IList;
                if (list != null)
                    return list.IndexOf(item);

                var index = 0;
                foreach (var itemsSourceItem in itemsSource)
                {
                    if (item == itemsSourceItem)
                    {
                        return index;
                    }
                    index++;
                }

                return -1;
            }
            return itemsStorage.IndexOf(item);
        }

        /// <inheritdoc/>
        public Object this[Int32 index]
        {
            get
            {
                if (IsBoundToItemsSource)
                {
                    var list = itemsSource as System.Collections.IList;
                    if (list != null)
                        return list[index];

                    if (index >= 0)
                    {
                        var currentIndex = 0;
                        foreach (var itemsSourceItem in itemsSource)
                        {
                            if (currentIndex == index)
                            {
                                return itemsSourceItem;
                            }
                            currentIndex++;
                        }
                    }
                    throw new IndexOutOfRangeException("index");
                }
                return itemsStorage[index];
            }
            set
            {
                EnsureNotBoundToItemsSource();

                var existing = itemsStorage[index];
                OnCollectionItemRemoved(index, existing);

                itemsStorage[index] = value;

                OnCollectionItemAdded(index, value);
            }
        }

        /// <inheritdoc/>
        public Int32 Count
        {
            get 
            {
                if (IsBoundToItemsSource)
                {
                    var collection = itemsSource as System.Collections.ICollection;
                    if (collection != null)
                    {
                        return collection.Count;
                    }

                    var count = 0;
                    foreach (var itemsSourceItem in itemsSource)
                        count++;

                    return count;
                }
                return itemsStorage.Count;
            }
        }

        /// <inheritdoc/>
        public Boolean IsReadOnly
        {
            get { return IsBoundToItemsSource; }
        }
    }
}
