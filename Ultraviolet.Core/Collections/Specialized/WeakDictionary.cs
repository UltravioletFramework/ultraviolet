using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Core.Collections.Specialized
{
    /// <summary>
    /// Represents a dictionary which maintains weak references to both its keys and its values.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public partial class WeakDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakDictionary{TKey, TValue}"/> class.
        /// </summary>
        public WeakDictionary()
            : this(0, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The dictionary's initial capacity.</param>
        public WeakDictionary(Int32 capacity)
            : this(capacity, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">The equality comparer used to compare the dictionary's keys.</param>
        public WeakDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The dictionary's initial capacity.</param>
        /// <param name="comparer">The equality comparer used to compare the dictionary's keys.</param>
        public WeakDictionary(Int32 capacity, IEqualityComparer<TKey> comparer)
        {
            this.comparer = new WeakKeyComparer<TKey>(comparer);
            this.dictionary = new Dictionary<Object, WeakReference<TValue>>(capacity, this.comparer);
        }

        /// <summary>
        /// Gets an enumerator which iterates through the dictionary.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the dictionary.</returns>
        public IEnumerator<KeyValuePair<Object, WeakReference<TValue>>> GetEnumerator() =>
            this.dictionary.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            foreach (var kvp in this.dictionary)
            {
                var weakKey = (WeakKeyReference<TKey>)kvp.Key;
                var weakVal = kvp.Value;
                var key = weakKey.Target;
                var val = weakVal.Target;
                if (weakKey.IsAlive && weakVal.IsAlive)
                {
                    yield return new KeyValuePair<TKey, TValue>(key, val);
                }
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Purges the dictionary of an entries which have been garbage collected.
        /// </summary>
        public void Purge()
        {
            var removed = default(List<Object>);
            foreach (var kvp in this.dictionary)
            {
                var weakKey = (WeakReference<TKey>)kvp.Key;
                var weakVal = kvp.Value;

                if (!weakKey.IsAlive || !weakVal.IsAlive)
                {
                    if (removed == null)
                        removed = new List<Object>();

                    removed.Add(weakKey);
                }
            }

            if (removed != null)
            {
                foreach (var key in removed)
                    this.dictionary.Remove(key);
            }
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, Int32 index)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(index >= 0, nameof(index));

            var offset = index;
            foreach (var item in dictionary)
            {
                var weakKey = (WeakKeyReference<TKey>)item.Key;
                var weakVal = item.Value;
                var key = weakKey.Target;
                var val = weakVal.Target;
                if (weakKey.IsAlive && weakVal.IsAlive)
                {
                    array[offset++] = new KeyValuePair<TKey, TValue>(key, val);
                }
            }
        }

        /// <inheritdoc/>
        public void Clear() =>
            this.dictionary.Clear();

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            Contract.Require(key, nameof(key));

            var weakKey = new WeakKeyReference<TKey>(key, this.comparer);
            var weakVal = WeakReference<TValue>.Create(value);

            this.dictionary.Add(weakKey, weakVal);
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <inheritdoc/>
        public Boolean TryGetValue(TKey key, out TValue value)
        {
            var weakVal = default(WeakReference<TValue>);
            if (this.dictionary.TryGetValue(key, out weakVal))
            {
                value = weakVal.Target;
                return true;
            }
            value = null;
            return false;
        }

        /// <inheritdoc/>
        public Boolean Remove(TKey key) =>
            this.dictionary.Remove(key);

        /// <inheritdoc/>
        public Boolean Remove(KeyValuePair<TKey, TValue> item) =>
            ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(item);

        /// <inheritdoc/>
        public Boolean ContainsKey(TKey key) => 
            this.dictionary.ContainsKey(key);

        /// <inheritdoc/>
        public Boolean Contains(KeyValuePair<TKey, TValue> item) =>
            ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Contains(item);

        /// <summary>
        /// Populates a collection with the dictionary's living keys.
        /// </summary>
        /// <param name="collection">The collection to populate.</param>
        public void GetKeys(ICollection<TKey> collection)
        {
            Contract.Require(collection, nameof(collection));

            foreach (var item in dictionary.Keys)
            {
                var weakKey = (WeakKeyReference<TKey>)item;
                var key = weakKey.Target;
                if (weakKey.IsAlive)
                {
                    collection.Add(key);
                }
            }
        }

        /// <summary>
        /// Populates a collection with the dictionary's living values.
        /// </summary>
        /// <param name="collection">The collection to populate.</param>
        public void GetValues(ICollection<TValue> collection)
        {
            Contract.Require(collection, nameof(collection));

            foreach (var kvp in dictionary)
            {
                var weakKey = (WeakKeyReference<TKey>)kvp.Key;
                var key = weakKey.Target;
                var weakVal = dictionary[weakKey];
                var val = weakVal.Target;
                if (weakKey.IsAlive && weakVal.IsAlive)
                {
                    collection.Add(val);
                }
            }
        }

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get
            {
                Contract.Require(key, nameof(key));

                var weakKey = new WeakKeyReference<TKey>(key, this.comparer);
                return this.dictionary[weakKey].Target;
            }
            set
            {
                Contract.Require(key, nameof(key));

                var weakKey = new WeakKeyReference<TKey>(key, this.comparer);
                this.dictionary[weakKey] = WeakReference<TValue>.Create(value);
            }
        }

        /// <inheritdoc/>
        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                var keys = new List<TKey>();
                GetKeys(keys);
                return keys;
            }
        }

        /// <inheritdoc/>
        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                var values = new List<TValue>();
                GetValues(values);
                return values;
            }
        }

        /// <inheritdoc/>
        public Int32 Count => this.dictionary.Count;

        /// <summary>
        /// Gets the number of living objects in the dictionary.
        /// </summary>
        public Int32 CountAlive
        {
            get
            {
                var count = 0;
                foreach (var kvp in dictionary)
                {
                    var weakKey = (WeakKeyReference<TKey>)kvp.Key;
                    var weakVal = kvp.Value;
                    if (weakKey.IsAlive && weakVal.IsAlive)
                        count++;
                }
                return count;
            }
        }

        /// <inheritdoc/>
        public Boolean IsReadOnly => false;

        // The underlying dictionary object.
        private readonly Dictionary<Object, WeakReference<TValue>> dictionary;
        private readonly WeakKeyComparer<TKey> comparer;
    }
}
