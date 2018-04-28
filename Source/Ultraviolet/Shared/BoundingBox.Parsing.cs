using System;
using System.Globalization;

namespace Ultraviolet
{
    partial struct BoundingBox
    {
        /// <summary>
        /// Converts the string representation of a <see cref="BoundingBox"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out BoundingBox v)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v);
        }

        /// <summary>
        /// Converts the string representation of a <see cref="BoundingBox"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static BoundingBox Parse(String s)
        {
            if (!TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out var v))
                throw new FormatException();

            return v;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="BoundingBox"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out BoundingBox v)
        {
            v = default(BoundingBox);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 6)
                return false;

            if (!Single.TryParse(components[0], style, provider, out var x1))
                return false;
            if (!Single.TryParse(components[1], style, provider, out var y1))
                return false;
            if (!Single.TryParse(components[2], style, provider, out var z1))
                return false;
            
            if (!Single.TryParse(components[3], style, provider, out var x2))
                return false;
            if (!Single.TryParse(components[4], style, provider, out var y2))
                return false;
            if (!Single.TryParse(components[5], style, provider, out var z2))
                return false;

            v = new BoundingBox(new Vector3(x1, y1, z1), new Vector3(x2, y2, z2));
            return true;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="BoundingBox"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static BoundingBox Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            if (!TryParse(s, style, provider, out var v))
                throw new FormatException();

            return v;
        }
    }
}
