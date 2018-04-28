using System;
using System.Globalization;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    partial struct Thickness
    {
        /// <summary>
        /// Converts the string representation of a <see cref="Thickness"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out Thickness v)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v);
        }

        /// <summary>
        /// Converts the string representation of a <see cref="Thickness"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static Thickness Parse(String s)
        {
            var v = default(Thickness);
            if (!TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v))
                throw new FormatException();

            return v;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="Thickness"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Thickness v)
        {
            Contract.Require(s, nameof(s));

            v = default(Thickness);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 1 && components.Length != 4)
                return false;

            if (components.Length == 1)
            {
                if (!Double.TryParse(components[0], style, provider, out Double value))
                    return false;

                v = new Thickness(value);
            }
            else
            {
                if (!Double.TryParse(components[0], style, provider, out Double top))
                    return false;
                if (!Double.TryParse(components[1], style, provider, out Double left))
                    return false;
                if (!Double.TryParse(components[2], style, provider, out Double right))
                    return false;
                if (!Double.TryParse(components[3], style, provider, out Double bottom))
                    return false;

                v = new Thickness(top, left, right, bottom);
            }

            return true;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="Thickness"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static Thickness Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            var v = default(Thickness);
            if (!TryParse(s, style, provider, out v))
                throw new FormatException();

            return v;
        }
    }
}