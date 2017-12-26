using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a collection of localized string variants.
    /// </summary>
    internal sealed class LocalizedStringVariantCollection : IEnumerable<KeyValuePair<String, LocalizedStringVariant>>
    {
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key for which to retrieve a value.</param>
        /// <param name="value">The value that was retrieved for the specified key.</param>
        /// <returns><see langword="true"/> if the value was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean TryGetValue(String key, out LocalizedStringVariant value)
        {
            return variants.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public Dictionary<String, LocalizedStringVariant>.Enumerator GetEnumerator()
        {
            return variants.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator<KeyValuePair<String, LocalizedStringVariant>> IEnumerable<KeyValuePair<String, LocalizedStringVariant>>.GetEnumerator()
        {
            return GetEnumerator();
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
        /// Gets the string variant with the specified key.
        /// </summary>
        /// <param name="key">The key that identifies the variant to retrieve.</param>
        /// <returns>The string variant with the specified key.</returns>
        public LocalizedStringVariant this[String key]
        {
            get { return variants[key]; }
            set
            {
                variants[key] = value;
                if (defaultVariant == null || value.Group == defaultVariant.Group)
                {
                    defaultVariant = value;
                }
            }
        }

        /// <summary>
        /// Gets the default string variant.
        /// </summary>
        public LocalizedStringVariant DefaultVariant
        {
            get { return defaultVariant; }
        }

        /// <summary>
        /// Gets the number of variants in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return variants.Count; }
        }

        // The underlying variant store.
        private LocalizedStringVariant defaultVariant;
        private readonly Dictionary<String, LocalizedStringVariant> variants = 
            new Dictionary<String, LocalizedStringVariant>();
    }
}
