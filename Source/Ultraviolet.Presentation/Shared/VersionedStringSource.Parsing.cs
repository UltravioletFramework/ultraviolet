using System;
using System.Globalization;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    partial struct VersionedStringSource
    {
        /// <inheritdoc/>
        public override String ToString()
        {
            return ToString(null);
        }

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
    }
}