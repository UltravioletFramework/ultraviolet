using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Contains extension methods for the <see cref="StringBuilder"/> class.
    /// </summary>
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Appends the specified string pointer to the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to which to append the value.</param>
        /// <param name="ptr">A pointer to the unmanaged string to append.</param>
        public static void AppendStringPtr(this StringBuilder sb, StringPtr8 ptr)
        {
            AppendStringPtr(sb, ptr, 0, ptr.Length);
        }

        /// <summary>
        /// Appends a substring of the specified string pointer to the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to which to append the value.</param>
        /// <param name="ptr">A pointer to the unmanaged string that contains the substring to append.</param>
        /// <param name="offset">The offset into the string at which the substring begins.</param>
        /// <param name="length">The length of the substring.</param>
        public static void AppendStringPtr(this StringBuilder sb, StringPtr8 ptr, Int32 offset, Int32 length)
        {
            Contract.Require(sb, nameof(sb));

            if (ptr == StringPtr8.Zero)
                return;

            for (int i = offset; i < offset + length; i++)
            {
                sb.Append(ptr[i]);
            }
        }

        /// <summary>
        /// Appends the specified string pointer to the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to which to append the value.</param>
        /// <param name="ptr">A pointer to the unmanaged string to append.</param>
        public static void AppendStringPtr(this StringBuilder sb, StringPtr16 ptr)
        {
            AppendStringPtr(sb, ptr, 0, ptr.Length);
        }

        /// <summary>
        /// Appends a substring of the specified string pointer to the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to which to append the value.</param>
        /// <param name="ptr">A pointer to the unmanaged string that contains the substring to append.</param>
        /// <param name="offset">The offset into the string at which the substring begins.</param>
        /// <param name="length">The length of the substring.</param>
        public static void AppendStringPtr(this StringBuilder sb, StringPtr16 ptr, Int32 offset, Int32 length)
        {
            Contract.Require(sb, nameof(sb));

            if (ptr == StringPtr16.Zero)
                return;

            for (int i = offset; i < offset + length; i++)
            {
                sb.Append(ptr[i]);
            }
        }

        /// <summary>
        /// Appends a substring of the specified string to the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to which to append the value.</param>
        /// <param name="str">The string that contains the substring to append.</param>
        /// <param name="offset">The offset into the string at which the substring begins.</param>
        /// <param name="length">The length of the substring.</param>
        public static void AppendSubstring(this StringBuilder sb, String str, Int32 offset, Int32 length)
        {
            Contract.Require(sb, nameof(sb));

            var end = offset + length;
            for (int i = offset; i < end; i++)
                sb.Append(str[i]);
        }

        /// <summary>
        /// Appends the value of a string segment to the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to which to append the value.</param>
        /// <param name="value">The <see cref="StringSegment"/> to append to the <see cref="StringBuilder"/>.</param>
        [CLSCompliant(false)]
        public static void AppendSegment(this StringBuilder sb, StringSegment value)
        {
            Contract.Require(sb, nameof(sb));

            if (value.IsEmpty)
                return;

            for (int i = 0; i < value.Length ; i++)
            {
                sb.Append(value[i]);
            }
        }

        /// <summary>
        /// Appends the value of a string segment to the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to which to append the value.</param>
        /// <param name="value">The <see cref="StringSegment"/> to append to the <see cref="StringBuilder"/>.</param>
        public static void AppendSegment(this StringBuilder sb, ref StringSegment value)
        {
            Contract.Require(sb, nameof(sb));

            if (value.IsEmpty)
                return;

            for (int i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
            }
        }

        /// <summary>
        /// Appends an integer to the specified StringBuilder, padding the string to contain at least 2 digits.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to which to append the value.</param>
        /// <param name="value">The value to append to the <see cref="StringBuilder"/>.</param>
        public static void AppendPaddedInt2(this StringBuilder sb, Int32 value)
        {
            Contract.Require(sb, nameof(sb));

            sb.Concat(value, 2);
        }

        /// <summary>
        /// Appends an integer to the specified StringBuilder, separating each group of 3 digits with commas.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to which to append the value.</param>
        /// <param name="value">The value to append to the <see cref="StringBuilder"/>.</param>
        public static void AppendIntWithCommas(this StringBuilder sb, int value)
        {
            Contract.Require(sb, nameof(sb));

            var group = 0;
            var written = 0;
            for (int div = 1000000000; div > 0; div /= 1000)
            {
                group = value / div;
                if (group > 0 || div == 1)
                {
                    if (written > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Concat(group, (written > 0) ? 3u : 1u);
                    written++;
                }
                value -= group * div;
            }
        }

        /// <summary>
        /// Appends a single-precision floating point value to the specified StringBuilder, padding the string to contain
        /// at exactly two digits after the decimal point.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to which to append the value.</param>
        /// <param name="value">The value to append to the <see cref="StringBuilder"/>.</param>
        public static void AppendPaddedSingle2(this StringBuilder sb, Single value)
        {
            Contract.Require(sb, nameof(sb));

            sb.Concat(value, 2);
        }

        /// <summary>
        /// Appends a double-precision floating point value to the specified StringBuilder, padding the string to contain
        /// at exactly two digits after the decimal point.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to which to append the value.</param>
        /// <param name="value">The value to append to the <see cref="StringBuilder"/>.</param>
        public static void AppendPaddedDouble2(this StringBuilder sb, Double value)
        {
            Contract.Require(sb, nameof(sb));

            sb.Concat((float)value, 2);
        }

        /// <summary>
        /// Populates a StringBuilder with a specified substring of this StringBuilder.
        /// </summary>
        /// <param name="source">The source StringBuilder.</param>
        /// <param name="start">The starting index of the substring.</param>
        /// <param name="length">The number of character in the substring.</param>
        /// <param name="output">The <see cref="StringBuilder"/> to populate with the substring.</param>
        public static void Substring(this StringBuilder source, Int32 start, Int32 length, StringBuilder output)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(output, nameof(output));

            output.Length = 0;
            for (int i = start; i < start + length; i++)
            {
                output.Append(source[i]);
            }
        }

        /// <summary>
        /// Splits the <see cref="StringBuilder"/> into subtrings separated by the specified delimiter.
        /// </summary>
        /// <param name="source">The source StringBuilder.</param>
        /// <param name="delimiter">The delimiter with which to split the string.</param>
        /// <param name="output">An array of StringBuilder objects to populate with substrings.</param>
        /// <returns>The number of substrings that were retrieved.</returns>
        public static int Split(this StringBuilder source, Char delimiter, StringBuilder[] output)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(output, nameof(output));

            if (output.Length == 0)
                return 0;

            var substringCount = 0;
            var substringStart = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == delimiter)
                {
                    source.Substring(substringStart, i - substringStart, output[substringCount]);
                    substringCount++;
                    substringStart = i + 1;
                    if (substringCount >= output.Length)
                    {
                        return substringCount;
                    }
                }
            }
            if (substringStart < source.Length - 1)
            {
                source.Substring(substringStart, source.Length - substringStart, output[substringCount]);
                substringCount++;
            }
            return substringCount;
        }
    }
}
