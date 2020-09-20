using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core.Collections;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a collection of curve keys.
    /// </summary>
    /// <typeparam name="TValue">The type of value which comprises the curve.</typeparam>
    /// <typeparam name="TKey">The type of keyframe which defines the shape of the curve.</typeparam>
    public sealed class CurveKeyCollection<TValue, TKey> : IEnumerable<CurveKeyRecord<TValue, TKey>>
        where TKey : CurveKey<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurveKeyCollection{TValue, TKey}"/> class from the specified collection of keys.
        /// </summary>
        /// <param name="keys">A collection of <typeparamref name="TKey"/> objects with which to populate the collection.</param>
        public CurveKeyCollection(IEnumerable<TKey> keys)
        {
            this.storage = (keys == null) ? new CurveKeyRecord<TValue, TKey>[0] :
                keys.Select(x => new CurveKeyRecord<TValue, TKey>(x, null)).ToArray();

            this.IsArrayValue = typeof(TValue).IsArray;
            if (this.IsArrayValue)
            {
                if (this.storage.Length > 0)
                {
                    this.ElementCount = (this.storage[0].Key.Value as Array)?.Length ?? 0;
                    if (this.storage.Any(x => ((x.Key.Value as Array)?.Length ?? 0) != this.ElementCount))
                        throw new ArgumentException(UltravioletStrings.CurveKeyArrayLengthMismatch);
                }
                else
                {
                    this.ElementCount = 0;
                }
            }
            else
            {
                this.ElementCount = 1;
            }
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public ArrayEnumerator<CurveKeyRecord<TValue, TKey>> GetEnumerator() => new ArrayEnumerator<CurveKeyRecord<TValue, TKey>>(storage);

        /// <inheritdoc/>
        IEnumerator<CurveKeyRecord<TValue, TKey>> IEnumerable<CurveKeyRecord<TValue, TKey>>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Overrides the sampler associated with the specified keyframe.
        /// </summary>
        /// <param name="index">The index of the keyframe to override.</param>
        /// <param name="sampler">The override sampler to set for the specified keyframe.</param>
        public void OverrideKeySampler(Int32 index, ICurveSampler<TValue, TKey> sampler)
        {
            ref var record = ref storage[index];
            record = new CurveKeyRecord<TValue, TKey>(record.Key, sampler);
        }

        /// <summary>
        /// Gets or sets the item at the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the item to retrieve.</param>
        /// <returns>The item at the specified index within the collection.</returns>
        public CurveKeyRecord<TValue, TKey> this[Int32 index] { get => storage[index]; }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public Int32 Count => storage.Length;

        /// <summary>
        /// Gets the number of elements in each value in this curve. If <typeparamref name="TValue"/> is not an array,
        /// this value will always be 1. Otherwise, this value will be the length of the arrays contained by the curve. 
        /// All of the arrays in the curve must have the same element count.
        /// </summary>
        public Int32 ElementCount { get; }

        /// <summary>
        /// Gets a value indicating whether the curve contains an array value.
        /// </summary>
        public Boolean IsArrayValue { get; }

        /// <summary>
        /// Gets a value indicating whether the collection is empty.
        /// </summary>
        public Boolean IsEmpty => storage.Length == 0;

        /// <summary>
        /// Gets a value indicating whether the curve represented by this collection is constant.
        /// </summary>
        public Boolean IsConstant => storage.Length == 0 || storage.Length == 1;

        // The collection's backing storage.
        private readonly CurveKeyRecord<TValue, TKey>[] storage;
    }
}
