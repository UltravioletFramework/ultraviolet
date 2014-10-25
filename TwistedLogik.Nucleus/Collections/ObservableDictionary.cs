using System;
using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Nucleus.Collections
{
    /// <summary>
    /// Represents the method that is called when an observable dictionary performs an operation
    /// that is not related to a specific item.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary that raised the event.</param>
    public delegate void ObservableDictionaryEvent<TKey, TValue>(ObservableDictionary<TKey, TValue> dictionary);

    /// <summary>
    /// Represents a method that is called when an observable dictionary performs an operation
    /// relating to a specific item.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary that raised the event.</param>
    /// <param name="key">The key that is the target of the operation.</param>
    /// <param name="value">The value that is the target of the operation.</param>
    public delegate void ObservableDictionaryItemEvent<TKey, TValue>(ObservableDictionary<TKey, TValue> dictionary, TKey key, TValue value);

    /// <summary>
    /// Represents a dictionary which raises events when items are added or removed.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
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
        /// Adds an item to the dictionary.
        /// </summary>
        /// <param name="key">The key of the item to add to the dictionary.</param>
        /// <param name="value">The value of the item to add to the dictionary.</param>
        public void Add(TKey key, TValue value)
        {
            TValue existing;
            if (dictionary.TryGetValue(key, out existing))
                OnItemRemoved(key, existing);

            dictionary.Add(key, value);
            OnItemAdded(key, value);
            OnChanged();
        }

        /// <summary>
        /// Removes the object with the specified key from the dictionary, if such an object exists.
        /// </summary>
        /// <param name="key">The key that represents the object to remove from the dictionary.</param>
        /// <returns><c>true</c> if the object was removed from the dictionary; otherwise, <c>false</c>.</returns>
        public Boolean Remove(TKey key)
        {
            TValue existing;
            dictionary.TryGetValue(key, out existing);

            if (dictionary.Remove(key))
            {
                OnItemRemoved(key, existing);
                OnChanged();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes all items from the dictionary.
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
            OnCleared();
            OnChanged();
        }

        /// <summary>
        /// Gets a value indicating whether the dictionary contains an item with the specified key.
        /// </summary>
        /// <param name="key">The key to evaluate.</param>
        /// <returns><c>true</c> if the dictionary contains an item with the specified key; otherwise, <c>false</c>.</returns>
        public Boolean ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets a value indicating whether the dictionary contains an item with the specified value.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><c>true</c> if the dictoinary contains an item with the specified value; otherwise, <c>false</c>.</returns>
        public Boolean ContainsValue(TValue value)
        {
            return dictionary.ContainsValue(value);
        }

        /// <summary>
        /// Attempts to retrieve the item with the specified key from the dictionary.  
        /// </summary>
        /// <param name="key">The key of the item to retrieve.</param>
        /// <param name="value">The value of the item that was retrieved, if an item was successfully retrieved.</param>
        /// <returns><c>true</c> if an item with the specified key was retrieved; otherwise, <c>false</c>.</returns>
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
            OnItemAdded(item.Key, item.Value);
            OnChanged();
        }

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">The item to remove from the collection.</param>
        /// <returns><c>true</c> if the item was removed from the collection; otherwise, <c>false</c>.</returns>
        Boolean ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(item))
            {
                OnItemRemoved(item.Key, item.Value);
                OnChanged();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><c>true</c> if the collection contains the specified item; otherwise, <c>false</c>.</returns>
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
                    OnItemRemoved(key, existing);

                dictionary[key] = value;
                OnItemAdded(key, value);
                OnChanged();
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
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        Boolean ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).IsReadOnly; }
        }

        /// <summary>
        /// Occurs whenever an operation is performed which modifies the contents of the dictionary.
        /// </summary>
        public ObservableDictionaryEvent<TKey, TValue> Changed;

        /// <summary>
        /// Occurs when the dictionary is cleared.
        /// </summary>
        public ObservableDictionaryEvent<TKey, TValue> Cleared;

        /// <summary>
        /// Occurs when an item is added to the dictionary.
        /// </summary>
        public ObservableDictionaryItemEvent<TKey, TValue> ItemAdded;

        /// <summary>
        /// Occurs when an item is removed from the dictionary.
        /// </summary>
        public ObservableDictionaryItemEvent<TKey, TValue> ItemRemoved;

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
        /// <param name="key">The key of the item that was added.</param>
        /// <param name="value">The value of the item that was added.</param>
        protected virtual void OnItemAdded(TKey key, TValue value)
        {
            var temp = ItemAdded;
            if (temp != null)
            {
                temp(this, key, value);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemRemoved"/> event.
        /// </summary>
        /// <param name="key">The key of the item that was removed.</param>
        /// <param name="value">The value of the item that was removed.</param>
        protected virtual void OnItemRemoved(TKey key, TValue value)
        {
            var temp = ItemRemoved;
            if (temp != null)
            {
                temp(this, key, value);
            }
        }

        // The wrapped dictionary which contains our items.
        private readonly Dictionary<TKey, TValue> dictionary;
    }
}
