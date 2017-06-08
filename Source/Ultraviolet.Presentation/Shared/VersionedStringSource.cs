using System;
using System.ComponentModel;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a source of string data with an attached version value which is used to track changes to mutable sources.
    /// </summary>
    [TypeConverter(typeof(VersionedStringSourceTypeConverter))]
    public partial struct VersionedStringSource : IEquatable<VersionedStringSource>
    {
        /// <summary>
        /// Initializes the <see cref="VersionedStringSource"/> type.
        /// </summary>
        static VersionedStringSource()
        {
            ObjectResolver.RegisterValueResolver<VersionedStringSource>((value, provider) => new VersionedStringSource(value));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionedStringSource"/> structure that wraps the specified <see cref="String"/> instance.
        /// </summary>
        /// <param name="sourceString">The <see cref="String"/> instance which is wrapped by this buffer.</param>
        public VersionedStringSource(String sourceString)
        {
            this.sourceString = sourceString;
            this.sourceStringBuilder = null;
            this.version = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionedStringSource"/> structure that wraps the specified <see cref="VersionedStringBuilder"/> instance.
        /// </summary>
        /// <param name="sourceStringBuilder">The <see cref="VersionedStringBuilder"/> instance which is wrapped by this buffer.</param>
        public VersionedStringSource(VersionedStringBuilder sourceStringBuilder)
        {
            this.sourceString = null;
            this.sourceStringBuilder = sourceStringBuilder;
            this.version = (sourceStringBuilder == null) ? 0 : sourceStringBuilder.Version;
        }
        
        /// <summary>
        /// Explicitly converts a <see cref="VersionedStringSource"/> instance to its wrapped <see cref="String"/> instance.
        /// </summary>
        /// <param name="buffer">The <see cref="VersionedStringSource"/> to convert.</param>
        /// <returns>The <see cref="String"/> instance which was wrapped by the string buffer.</returns>
        public static explicit operator String(VersionedStringSource buffer)
        {
            if (!buffer.IsSourcedFromString)
                throw new InvalidCastException();

            return buffer.sourceString;
        }

        /// <summary>
        /// Explicitly converts a <see cref="VersionedStringSource"/> instance to its wrapped <see cref="VersionedStringBuilder"/> instance.
        /// </summary>
        /// <param name="buffer">The <see cref="VersionedStringSource"/> to convert.</param>
        /// <returns>The <see cref="VersionedStringBuilder"/> instance which was wrapped by the string buffer.</returns>
        public static explicit operator VersionedStringBuilder(VersionedStringSource buffer)
        {
            if (!buffer.IsSourcedFromStringBuilder)
                throw new InvalidCastException();

            return buffer.sourceStringBuilder;
        }

        /// <summary>
        /// Explicitly converts a <see cref="String"/> instance to a <see cref="VersionedStringSource"/> instance.
        /// </summary>
        /// <param name="str">The <see cref="String"/> instance to convert.</param>
        /// <returns>The <see cref="VersionedStringSource"/> that was created.</returns>
        public static explicit operator VersionedStringSource(String str)
        {
            return new VersionedStringSource(str);
        }

        /// <summary>
        /// Explicitly converts a <see cref="VersionedStringBuilder"/> instance to a <see cref="VersionedStringSource"/> instance.
        /// </summary>
        /// <param name="str">The <see cref="VersionedStringBuilder"/> instance to convert.</param>
        /// <returns>The <see cref="VersionedStringSource"/> that was created.</returns>
        public static explicit operator VersionedStringSource(VersionedStringBuilder str)
        {
            return new VersionedStringSource(str);
        }

        /// <inheritdoc/>
        public override String ToString() => ToString(null);

        /// <summary>
        /// Converts the object to a human-readable string using the specified culture information.
        /// </summary>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A human-readable string that represents the object.</returns>
        public String ToString(IFormatProvider provider)
        {
            if (sourceString != null)
                return sourceString;

            if (sourceStringBuilder != null)
            {
                if (sourceStringBuilder.Version != version)
                    throw new InvalidOperationException(PresentationStrings.VersionedStringSourceIsStale);

                return sourceStringBuilder.ToString();
            }

            return null;
        }

        /// <summary>
        /// Converts a substring of the string buffer to a <see cref="String"/> instance.
        /// </summary>
        /// <param name="startIndex">The index of the first character in the substring to convert.</param>
        /// <param name="length">The number of characters in the substring to convert.</param>
        /// <returns>The string that was created.</returns>
        public String ToString(Int32 startIndex, Int32 length)
        {
            if (sourceString != null)
                return sourceString.Substring(startIndex, length);

            if (sourceStringBuilder != null)
            {
                if (sourceStringBuilder.Version != version)
                    throw new InvalidOperationException(PresentationStrings.VersionedStringSourceIsStale);

                return sourceStringBuilder.ToString();
            }

            if (startIndex != 0)
                throw new ArgumentOutOfRangeException("startIndex");

            if (length > 0)
                throw new ArgumentOutOfRangeException("length");

            return String.Empty;
        }

        /// <summary>
        /// Creates an invalid <see cref="VersionedStringSource"/> instance.
        /// </summary>
        public static VersionedStringSource Invalid
        {
            get { return new VersionedStringSource(); }
        }

        /// <summary>
        /// Gets the character at the specified index within the string.
        /// </summary>
        /// <param name="ix">The index of the character to retrieve.</param>
        /// <returns>The character at the specified index within the string.</returns>
        public Char this[Int32 ix]
        {
            get
            {
                if (IsSourcedFromString)
                    return sourceString[ix];

                if (IsSourcedFromStringBuilder)
                {
                    if (sourceStringBuilder.Version != version)
                        throw new InvalidOperationException(PresentationStrings.VersionedStringSourceIsStale);

                    return sourceStringBuilder[ix];
                }

                throw new ArgumentOutOfRangeException(nameof(ix));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is a valid buffer.
        /// </summary>
        public Boolean IsValid
        {
            get { return sourceString != null || sourceStringBuilder != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this string source references stale data.
        /// </summary>
        public Boolean IsStale
        {
            get { return (sourceStringBuilder != null && sourceStringBuilder.Version != version); }
        }

        /// <summary>
        /// Gets a value indicating whether the buffer is sourced from an instance of <see cref="String"/>.
        /// </summary>
        public Boolean IsSourcedFromString
        {
            get { return sourceString != null; }
        }

        /// <summary>
        /// Gets a value indicating whether the buffer is sources from an instance of <see cref="System.Text.StringBuilder"/>.
        /// </summary>
        public Boolean IsSourcedFromStringBuilder
        {
            get { return sourceStringBuilder != null; }
        }

        /// <summary>
        /// Gets the number of characters in the string.
        /// </summary>
        public Int32 Length
        {
            get
            {
                if (IsSourcedFromString)
                    return sourceString.Length;

                if (IsSourcedFromStringBuilder)
                {
                    if (sourceStringBuilder.Version != version)
                        throw new InvalidOperationException(PresentationStrings.VersionedStringSourceIsStale);

                    return sourceStringBuilder.Length;
                }

                return 0;
            }
        }

        // State values.
        private readonly String sourceString;
        private readonly VersionedStringBuilder sourceStringBuilder;
        private readonly Int64 version;
    }
}
