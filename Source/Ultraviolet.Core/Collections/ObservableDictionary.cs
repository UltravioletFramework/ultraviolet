using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents the method that is called when an observable dictionary performs an operation
    /// that is not related to a specific item.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary that raised the event.</param>
    public delegate void ObservableDictionaryEventHandler<TKey, TValue>(ObservableDictionary<TKey, TValue> dictionary);

    /// <summary>
    /// Represents a method that is called when an observable dictionary performs an operation
    /// relating to a specific item.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary that raised the event.</param>
    /// <param name="key">The key that is the target of the operation.</param>
    /// <param name="value">The value that is the target of the operation.</param>
    public delegate void ObservableDictionaryItemEventHandler<TKey, TValue>(ObservableDictionary<TKey, TValue> dictionary, TKey key, TValue value);

    /// <summary>
    /// Represents a dictionary which raises events when items are added or removed.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged<TKey, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
        /// </summary>
        public ObservableDictionary()
        {
            this.dictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The dictionary's initial capacity.</param>
        public ObservableDictionary(Int32 capacity)
        {
            this.dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class that uses the specified equality comparer.
        /// </summary>
        /// <param name="comparer">The <see cref="IEqualityComparer{TKey}"/> to use when comparing keys.</param>
        public ObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class with the specified initial capacity
        /// that uses the specified equality comparer.
        /// </summary>
        /// <param name="capacity">The dictionary's initial capacity.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{TKey}"/> to use when comparing keys.</param>
        public ObservableDictionary(Int32 capacity, IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class that contains the same items as the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary from which to copy items.</param>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class that contains the same items as the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary from which to copy items.</param>
        /// <param name="comparer">The equality comparer to use when comparing keys.</param>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// Removes all items from the dictionary.
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
            OnCollectionReset();
        }

        /// <summary>
        /// Adds an item to the dictionary.
        /// </summary>
        /// <param name="key">The key of the item to add to the dictionary.</param>
        /// <param name="value">The value of the item to add to the dictionary.</param>
        public void Add(TKey key, TValue value)
        {
            TValue existing;
            if (dictionary.TryGetValue(key, out existing))
            {
                OnCollectionItemRemoved(key, existing);
            }

            dictionary.Add(key, value);
            OnCollectionItemAdded(key, value);
        }

        /// <summary>
        /// Removes the object with the specified key from the dictionary, if such an object exists.
        /// </summary>
        /// <param name="key">The key that represents the object to remove from the dictionary.</param>
        /// <returns><see langword="true"/> if the object was removed from the dictionary; otherwise, <see langword="false"/>.</returns>
        public Boolean Remove(TKey key)
        {
            TValue existing;
            if (dictionary.TryGetValue(key, out existing) && dictionary.Remove(key))
            {
                OnCollectionItemRemoved(key, existing);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the dictionary contains an item with the specified key.
        /// </summary>
        /// <param name="key">The key to evaluate.</param>
        /// <returns><see langword="true"/> if the dictionary contains an item with the specified key; otherwise, <see langword="false"/>.</returns>
        public Boolean ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets a value indicating whether the dictionary contains an item with the specified value.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><see langword="true"/> if the dictoinary contains an item with the specified value; otherwise, <see langword="false"/>.</returns>
        public Boolean ContainsValue(TValue value)
        {
            return dictionary.ContainsValue(value);
        }

        /// <summary>
        /// Attempts to retrieve the item with the specified key from the dictionary.  
        /// </summary>
        /// <param name="key">The key of the item to retrieve.</param>
        /// <param name="value">The value of the item that was retrieved, if an item was successfully retrieved.</param>
        /// <returns><see langword="true"/> if an item with the specified key was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Copies the entire collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array into which to copy the collection's elements.</param>
        /// <param name="arrayIndex">The index within the array at which to begin copying elements.</param>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, Int32 arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The item to add to the collection.</param>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Add(item);

            OnCollectionItemAdded(item.Key, item.Value);
        }

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">The item to remove from the collection.</param>
        /// <returns><see langword="true"/> if the item was removed from the collection; otherwise, <see langword="false"/>.</returns>
        Boolean ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(item))
            {
                OnCollectionItemRemoved(item.Key, item.Value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><see langword="true"/> if the collection contains the specified item; otherwise, <see langword="false"/>.</returns>
        Boolean ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Contains(item);
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return dictionary.GetEnumerator();
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
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the dictionary item with the specified key.
        /// </summary>
        /// <param name="key">The key that represents the item to get or set.</param>
        /// <returns>The dictionary item with the specified key.</returns>
        public TValue this[TKey key]
        {
            get { return dictionary[key]; }
            set 
            {
                TValue existing;
                if (dictionary.TryGetValue(key, out existing))
                {
                    OnCollectionItemRemoved(key, existing);
                }

                dictionary[key] = value;
                OnCollectionItemAdded(key, value);
            }
        }

        /// <summary>
        /// Gets a collection containing the dictionary's set of keys.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return dictionary.Keys; }
        }

        /// <summary>
        /// Gets a collection containing the dictionary's set of values.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return dictionary.Values; }
        }

        /// <summary>
        /// Gets the number of items in the dictionary.
        /// </summary>
        public Int32 Count
        {
            get { return dictionary.Count; }
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
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        Boolean ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).IsReadOnly; }
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
        public event CollectionResetEventHandler<TKey, TValue> CollectionReset;

        /// <inheritdoc/>
        public event CollectionItemAddedEventHandler<TKey, TValue> CollectionItemAdded;

        /// <inheritdoc/>
        public event CollectionItemRemovedEventHandler<TKey, TValue> CollectionItemRemoved;

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
        /// <param name="key">The key of the item that was added to the list.</param>
        /// <param name="value">The item that was added to the list.</param>
        protected virtual void OnCollectionItemAdded(TKey key, TValue value)
        {
            CollectionItemAdded?.Invoke(this, key, value);

            if (suppressUntypedNotifications)
                return;

            untypedCollectionItemAdded?.Invoke(this, null, value);
        }

        /// <summary>
        /// Raises the <see cref="CollectionItemRemoved"/> event.
        /// </summary>
        /// <param name="key">The key of the item that was removed from the list.</param>
        /// <param name="value">The item that was added to the list.</param>
        protected virtual void OnCollectionItemRemoved(TKey key, TValue value)
        {
            CollectionItemRemoved?.Invoke(this, key, value);

            if (suppressUntypedNotifications)
                return;

            untypedCollectionItemRemoved?.Invoke(this, null, value);
        }

        // The wrapped dictionary which contains our items.
        private readonly Dictionary<TKey, TValue> dictionary;

        // Explicitly implemented events belonging to INotifyCollectionChanged.
        private readonly Object untypedEventSyncObject = new Object();
        private CollectionResetEventHandler untypedCollectionReset;
        private CollectionItemAddedEventHandler untypedCollectionItemAdded;
        private CollectionItemRemovedEventHandler untypedCollectionItemRemoved;
        private Boolean suppressUntypedNotifications;
    }
}
