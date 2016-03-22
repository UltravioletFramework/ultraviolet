﻿using System;

namespace TwistedLogik.Nucleus.Text
{
    /// <summary>
    /// Contains methods for performing low- or no-allocation conversions 
    /// of the <see cref="StringSegment"/> structure to other types.
    /// </summary>
    public static class StringSegmentConversion
    {
        /// <summary>
        /// Converts the decimal text of the specified <see cref="StringSegment"/> to an
        /// instance of <see cref="Int32"/>, throwing a <see cref="FormatException"/> if the conversion fails.
        /// </summary>
        /// <param name="segment">The string segment to convert.</param>
        /// <returns>The converted value.</returns>
        public static Int32 ParseInt32(StringSegment segment)
        {
            Int32 value;
            if (!TryParseInt32(segment, out value))
                throw new FormatException();

            return value;
        }

        /// <summary>
        /// Converts the decimal text of the specified <see cref="StringSegment"/> to an 
        /// instance of <see cref="Int32"/> if possible.
        /// </summary>
        /// <param name="segment">The string segment to convert.</param>
        /// <param name="result">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParseInt32(StringSegment segment, out Int32 result)
        {
            var spaceCountLeading = CountLeadingSpace(ref segment);
            var spaceCountTrailing = CountTrailingSpace(ref segment);

            var valueStart = spaceCountLeading;
            var valueLength = segment.Length - (spaceCountLeading + spaceCountTrailing);

            var sign = 1;
            if (segment[spaceCountTrailing] == '-')
            {
                sign = -1;
                valueStart++;
                valueLength--;
            }

            var magnitude = (Int64)Math.Pow(10, valueLength - 1);
            var digit = 0;
            var total = 0L;
            for (int i = 0; i < valueLength; i++)
            {
                if (!ConvertDecimalDigit(segment[valueStart + i], out digit))
                {
                    result = 0;
                    return false;
                }

                total += (magnitude * digit);
                magnitude /= 10;
            }

            total *= sign;

            if (total < Int32.MinValue || total > Int32.MaxValue)
                throw new OverflowException();

            result = (Int32)total;
            return true;
        }

        /// <summary>
        /// Converts the decimal text of the specified <see cref="StringSegment"/> to an
        /// instance of <see cref="UInt32"/>, throwing a <see cref="FormatException"/> if the conversion fails.
        /// </summary>
        /// <param name="segment">The string segment to convert.</param>
        /// <returns>The converted value.</returns>
        [CLSCompliant(false)]
        public static UInt32 ParseUInt32(StringSegment segment)
        {
            UInt32 value;
            if (!TryParseUInt32(segment, out value))
                throw new FormatException();

            return value;
        }

        /// <summary>
        /// Converts the decimal text of the specified <see cref="StringSegment"/> to an 
        /// instance of <see cref="UInt32"/> if possible.
        /// </summary>
        /// <param name="segment">The string segment to convert.</param>
        /// <param name="result">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        [CLSCompliant(false)]
        public static Boolean TryParseUInt32(StringSegment segment, out UInt32 result)
        {
            var spaceCountLeading = CountLeadingSpace(ref segment);
            var spaceCountTrailing = CountTrailingSpace(ref segment);

            var valueStart = spaceCountLeading;
            var valueLength = segment.Length - (spaceCountLeading + spaceCountTrailing);

            var magnitude = (Int64)Math.Pow(10, valueLength - 1);
            var digit = 0;
            var total = 0L;
            for (int i = 0; i < valueLength; i++)
            {
                if (!ConvertDecimalDigit(segment[valueStart + i], out digit))
                {
                    result = 0;
                    return false;
                }

                total += (magnitude * digit);
                magnitude /= 10;
            }

            if (total > UInt32.MaxValue)
                throw new OverflowException();

            result = (UInt32)total;
            return true;
        }

        /// <summary>
        /// Converts the hexadecimal text of the specified <see cref="StringSegment"/> to an
        /// instance of <see cref="Int32"/>, throwing a <see cref="FormatException"/> if the conversion fails.
        /// </summary>
        /// <param name="segment">The string segment to convert.</param>
        /// <returns>The converted value.</returns>
        public static Int32 ParseHexadecimalInt32(StringSegment segment)
        {
            Int32 value;
            if (!TryParseHexadecimalInt32(segment, out value))
                throw new FormatException();

            return value;
        }
        
        /// <summary>
        /// Converts the hexadecimal text of the specified <see cref="StringSegment"/> to an 
        /// instance of <see cref="Int32"/> if possible.
        /// </summary>
        /// <param name="segment">The string segment to convert.</param>
        /// <param name="result">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParseHexadecimalInt32(StringSegment segment, out Int32 result)
        {
            var spaceCountLeading = CountLeadingSpace(ref segment);
            var spaceCountTrailing = CountTrailingSpace(ref segment);
            
            var valueStart = spaceCountLeading;
            var valueLength = segment.Length - (spaceCountLeading + spaceCountTrailing);
            
            var magnitude = (Int64)Math.Pow(16, valueLength - 1);
            var digit = 0;
            var total = 0L;
            for (int i = 0; i < valueLength; i++)
            {
                if (!ConvertHexadecimalDigit(segment[valueStart + i], out digit))
                {
                    result = 0;
                    return false;
                }

                total += (magnitude * digit);
                magnitude /= 16;
            }

            if (total > Int32.MaxValue)
                throw new OverflowException();

            result = (Int32)total;
            return true;
        }

        /// <summary>
        /// Converts the hexadecimal text of the specified <see cref="StringSegment"/> to an
        /// instance of <see cref="UInt32"/>, throwing a <see cref="FormatException"/> if the conversion fails.
        /// </summary>
        /// <param name="segment">The string segment to convert.</param>
        /// <returns>The converted value.</returns>
        [CLSCompliant(false)]
        public static UInt32 ParseHexadecimalUInt32(StringSegment segment)
        {
            UInt32 value;
            if (!TryParseHexadecimalUInt32(segment, out value))
                throw new FormatException();

            return value;
        }

        /// <summary>
        /// Converts the hexadecimal text of the specified <see cref="StringSegment"/> to an 
        /// instance of <see cref="UInt32"/> if possible.
        /// </summary>
        /// <param name="segment">The string segment to convert.</param>
        /// <param name="result">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        [CLSCompliant(false)]
        public static Boolean TryParseHexadecimalUInt32(StringSegment segment, out UInt32 result)
        {
            var spaceCountLeading = CountLeadingSpace(ref segment);
            var spaceCountTrailing = CountTrailingSpace(ref segment);

            var valueStart = spaceCountLeading;
            var valueLength = segment.Length - (spaceCountLeading + spaceCountTrailing);
            
            var magnitude = (Int64)Math.Pow(16, valueLength - 1);
            var digit = 0;
            var total = 0L;
            for (int i = 0; i < valueLength; i++)
            {
                if (!ConvertHexadecimalDigit(segment[valueStart + i], out digit))
                {
                    result = 0;
                    return false;
                }

                total += (magnitude * digit);
                magnitude /= 16;
            }

            if (total > UInt32.MaxValue)
                throw new OverflowException();

            result = (UInt32)total;
            return true;
        }

        /// <summary>
        /// Counts the number of leading spaces in the specified string segment.
        /// </summary>
        private static Int32 CountLeadingSpace(ref StringSegment segment)
        {
            var space = 0;

            for (int i = 0; i < segment.Length; i++)
            {
                if (!Char.IsWhiteSpace(segment[i]))
                    break;

                space++;
            }

            return space;
        }

        /// <summary>
        /// Counts the number of trailing spaces in the specified string segment.
        /// </summary>
        private static Int32 CountTrailingSpace(ref StringSegment segment)
        {
            var space = 0;

            for (int i = segment.Length - 1; i >= 0; i--)
            {
                if (!Char.IsWhiteSpace(segment[i]))
                    break;

                space++;
            }

            return space;
        }

        /// <summary>
        /// Converts a character that represents a decimal digit into an integer value.
        /// </summary>
        private static Boolean ConvertDecimalDigit(Char digit, out Int32 value)
        {
            switch (digit)
            {
                case '0': value = 0; return true;
                case '1': value = 1; return true;
                case '2': value = 2; return true;
                case '3': value = 3; return true;
                case '4': value = 4; return true;
                case '5': value = 5; return true;
                case '6': value = 6; return true;
                case '7': value = 7; return true;
                case '8': value = 8; return true;
                case '9': value = 9; return true;
            }
            value = 0;
            return false;
        }

        /// <summary>
        /// Converts a character that represents a hexadecimal digit into an integer value.
        /// </summary>
        private static Boolean ConvertHexadecimalDigit(Char digit, out Int32 value)
        {
            switch (digit)
            {
                case '0': value = 0; return true;
                case '1': value = 1; return true;
                case '2': value = 2; return true;
                case '3': value = 3; return true;
                case '4': value = 4; return true;
                case '5': value = 5; return true;
                case '6': value = 6; return true;
                case '7': value = 7; return true;
                case '8': value = 8; return true;
                case '9': value = 9; return true;
                case 'A':
                case 'a':
                    value = 10;
                    return true;
                case 'B':
                case 'b':
                    value = 11;
                    return true;
                case 'C':
                case 'c':
                    value = 12;
                    return true;
                case 'D':
                case 'd':
                    value = 13;
                    return true;
                case 'E':
                case 'e':
                    value = 14;
                    return true;
                case 'F':
                case 'f':
                    value = 15;
                    return true;
            }
            value = 0;
            return false;
        }
    }
}
