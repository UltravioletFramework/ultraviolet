using System;
using System.Globalization;

namespace Ultraviolet
{
    partial class BoundingFrustum
    {
        /// <summary>
        /// Converts the string representation of a <see cref="BoundingFrustum"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out BoundingFrustum v)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v);
        }

        /// <summary>
        /// Converts the string representation of a <see cref="BoundingFrustum"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static BoundingFrustum Parse(String s)
        {
            if (!TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out var v))
                throw new FormatException();

            return v;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="BoundingFrustum"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out BoundingFrustum v)
        {
            v = default(BoundingFrustum);

            if (String.IsNullOrEmpty(s))
                return false;

            if (!Matrix.TryParse(s, out var matrix))
                return false;

            v = new BoundingFrustum(matrix);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="BoundingFrustum"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static BoundingFrustum Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            if (!TryParse(s, style, provider, out var v))
                throw new FormatException();

            return v;
        }
    }
}
