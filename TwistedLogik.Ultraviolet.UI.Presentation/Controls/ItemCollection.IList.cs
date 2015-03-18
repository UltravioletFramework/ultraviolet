using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    partial class ItemCollection : IList<Object>
    {
        /// <inheritdoc/>
        public void CopyTo(Object[] array, Int32 arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            EnsureNotBoundToItemsSource();

            itemsStorage.Clear();
            OnCollectionReset();
        }

        /// <inheritdoc/>
        public void Add(Object item)
        {
            EnsureNotBoundToItemsSource();

            itemsStorage.Add(item);
            OnCollectionItemAdded(item);
        }

        /// <inheritdoc/>
        public void Insert(Int32 index, Object item)
        {
            EnsureNotBoundToItemsSource();

            itemsStorage.Insert(index, item);
            OnCollectionItemAdded(item);
        }

        /// <inheritdoc/>
        public void RemoveAt(Int32 index)
        {
            EnsureNotBoundToItemsSource();

            var item = itemsStorage[index];
            itemsStorage.RemoveAt(index);
            OnCollectionItemRemoved(item);
        }

        /// <inheritdoc/>
        public Boolean Remove(Object item)
        {
            EnsureNotBoundToItemsSource();

            if (itemsStorage.Remove(item))
            {
                OnCollectionItemRemoved(item);
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
                OnCollectionItemRemoved(existing);

                itemsStorage[index] = value;

                OnCollectionItemAdded(value);
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
