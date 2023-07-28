using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Audio
{
    /// <summary>
    /// Represents a tag which is part of a song's metadata.
    /// </summary>
    public sealed class SongTag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongTag"/> class.
        /// </summary>
        /// <param name="key">The tag's identifying key.</param>
        /// <param name="value">The tag's raw value.</param>
        public SongTag(String key, String value)
        {
            Contract.RequireNotEmpty(key, nameof(key));
            Contract.Require(value, nameof(value));

            this.Key = key;
            this.Value = value;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return $"{Key}={Value}";
        }

        /// <summary>
        /// Attempts to convert the tag's value to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which to convert the tag's value.</typeparam>
        /// <returns>The converted value of the tag.</returns>
        public T As<T>()
        {
            return (T)ObjectResolver.FromString(Value, typeof(T));
        }

        /// <summary>
        /// Gets the tag's identifying key.
        /// </summary>
        public String Key { get; }

        /// <summary>
        /// Gets the tag's raw value.
        /// </summary>
        public String Value { get; }
    }
}
