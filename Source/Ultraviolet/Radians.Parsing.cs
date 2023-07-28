using System;
using System.Globalization;

namespace Ultraviolet
{
    partial struct Radians
    {
        /// <summary>
        /// Converts the string representation of a <see cref="Radians"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out Radians v)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v);
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Radians"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static Radians Parse(String s)
        {
            var v = default(Radians);
            if (!TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v))
                throw new FormatException();
            
            return v;
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Radians"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Radians v)
        {
            v = Radians.Zero;

            // Determine whether the string is being specified in terms of pi or tau.
            var trimmed = s.Trim().ToLower();
            var suffix = EvaluateSuffix(trimmed, out var suffixFactor);
            var suffixLength = (suffix == null) ? 0 : suffix.Length;

            // Parse the fractional part of the string.
            if (!TryParseFraction(trimmed.Substring(0, trimmed.Length - suffixLength), style, provider, out Single numericValue))
            {
                return false;
            }

            // Convert the numeric value to radians.
            v = new Radians(numericValue * suffixFactor);
            return true;
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Radians"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static Radians Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            var v = default(Radians);
            if (!TryParse(s, style, provider, out v))
                throw new FormatException();
            
            return v;
        }
    }
}
