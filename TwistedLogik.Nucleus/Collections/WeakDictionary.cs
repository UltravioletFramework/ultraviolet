using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TwistedLogik.Nucleus.Collections
{
    /// <summary>
    /// Represents a dictionary which maintains weak references to its keys.
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
        public Boolean Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            if (TryGetValue(item.Key, out value) && value == item.Value)
            {
                return Remove(item.Key);
            }
            return false;
        }

        /// <inheritdoc/>
        public Boolean ContainsKey(TKey key) => 
            this.dictionary.ContainsKey(key);

        /// <inheritdoc/>
        public Boolean Contains(KeyValuePair<TKey, TValue> kvp)
        {
            var value = default(TValue);
            return TryGetValue(kvp.Key, out value) && value == kvp.Value;
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
        public ICollection<TKey> Keys
        {
            get
            {
                var keys = new List<TKey>();

                foreach (var item in dictionary.Keys)
                {
                    var weakKey = (WeakKeyReference<TKey>)item;
                    var key = weakKey.Target;
                    if (weakKey.IsAlive)
                    {
                        keys.Add(key);
                    }
                }

                return keys;
            }
        }

        /// <inheritdoc/>
        public ICollection<TValue> Values
        {
            get
            {
                var values = new List<TValue>();

                foreach (var item in dictionary.Values)
                {
                    var weakVal = item;
                    var val = item.Target;
                    if (weakVal.IsAlive)
                    {
                        values.Add(val);
                    }
                }

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
