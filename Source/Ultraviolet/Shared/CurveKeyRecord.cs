using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a curve key and its additional metadata.
    /// </summary>
    /// <typeparam name="TValue">The type of value which comprises the curve.</typeparam>
    /// <typeparam name="TKey">The type of keyframe which defines the shape of the curve.</typeparam>
    public struct CurveKeyRecord<TValue, TKey>
        where TKey : CurveKey<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurveKeyRecord{TValue, TKey}"/> structure.
        /// </summary>
        /// <param name="key">The curve key.</param>
        /// <param name="samplerOverride">The curve key's sampler override.</param>
        public CurveKeyRecord(TKey key, ICurveSampler<TValue, TKey> samplerOverride)
        {
            this.Key = key;
            this.SamplerOverride = samplerOverride;
        }

        /// <summary>
        /// Gets the curve key.
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// Gets the curve key's sampler override.
        /// </summary>
        public ICurveSampler<TValue, TKey> SamplerOverride { get; }
    }
}
