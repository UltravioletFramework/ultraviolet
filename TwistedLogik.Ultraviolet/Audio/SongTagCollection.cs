using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace TwistedLogik.Ultraviolet.Audio
{
    /// <summary>
    /// Represents a song's collection of tags.
    /// </summary>
    public sealed partial class SongTagCollection : IEnumerable<KeyValuePair<String, SongTag>>
    {
        /// <summary>
        /// Removes all tag from the collection.
        /// </summary>
        public void Clear()
        {
            storage.Clear();
        }

        /// <summary>
        /// Adds a tag to the collection.
        /// </summary>
        /// <param name="key">The tag's identifying key.</param>
        /// <param name="value">The tag's raw value.</param>
        [Preserve]
        public void Add(String key, String value)
        {
            Contract.RequireNotEmpty(key, nameof(key));
            Contract.Require(value, nameof(value));

            storage.Add(key, new SongTag(key, value));
        }

        /// <summary>
        /// Removes the tag with the specified key from the collection,
        /// if such a tag exists.
        /// </summary>
        /// <param name="key">The identifying key of the tag to remove.</param>
        /// <returns><see langword="true"/> if a tag with the specified key
        /// was removed from the collection; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Remove(String key)
        {
            Contract.RequireNotEmpty(key, nameof(key));

            return storage.Remove(key);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains
        /// a tag with the specified key.
        /// </summary>
        /// <param name="key">The identifying key of the tag for which to search.</param>
        /// <returns><see langword="true"/> if the collection contains a tag with the
        /// specified key; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean ContainsKey(String key)
        {
            Contract.RequireNotEmpty(key, nameof(key));

            return storage.ContainsKey(key);
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public Int32 Count => storage.Count;

        /// <summary>
        /// Attempts to retrieve the tag with the specified key.
        /// </summary>
        /// <param name="key">The identifying key of the tag to retrieve.</param>
        /// <returns>The <see cref="SongTag"/> with the specified key, or <see langword="null"/> if
        /// no such tag exists within the collection.</returns>
        public SongTag this[String key]
        {
            get
            {
                SongTag value;
                storage.TryGetValue(key, out value);
                return value;
            }
        }

        // The collection's internal storage.
        private readonly Dictionary<String, SongTag> storage =
            new Dictionary<String, SongTag>();
    }
}
