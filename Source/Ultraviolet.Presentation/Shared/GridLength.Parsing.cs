using System;
using System.Globalization;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    partial struct GridLength
    {
        /// <summary>
        /// Converts the string representation of a <see cref="GridLength"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out GridLength v)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v);
        }

        /// <summary>
        /// Converts the string representation of a <see cref="GridLength"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static GridLength Parse(String s)
        {
            var v = default(GridLength);
            if (!TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v))
                throw new FormatException();

            return v;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="GridLength"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out GridLength v)
        {
            Contract.Require(s, nameof(s));

            v = default(GridLength);

            if (String.Equals("Auto", s, StringComparison.InvariantCultureIgnoreCase))
            {
                v = GridLength.Auto;
                return true;
            }

            var value = 1.0;
            var isStar = s.EndsWith("*");
            if (isStar)
            {
                var valuePart = s.Substring(0, s.Length - 1);
                if (valuePart.Length > 0)
                {
                    if (!Double.TryParse(valuePart, out value))
                        return false;
                }
                v = new GridLength(value, GridUnitType.Star);
                return true;
            }

            if (!Double.TryParse(s, out value))
                return false;

            v = new GridLength(value);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="GridLength"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static GridLength Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            var v = default(GridLength);
            if (!TryParse(s, style, provider, out v))
                throw new FormatException();

            return v;
        }
    }
}