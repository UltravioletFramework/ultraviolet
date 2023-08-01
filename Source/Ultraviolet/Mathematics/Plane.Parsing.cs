using System;
using System.Globalization;

namespace Ultraviolet
{
    partial struct Plane
    {        
        /// <summary>
        /// Converts the string representation of a <see cref="Plane"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out Plane v)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v);
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Plane"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static Plane Parse(String s)
        {
            var v = default(Plane);
            if (!TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v))
                throw new FormatException();
            
            return v;
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Plane"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Plane v)
        {
            v = default(Plane);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 4)
                return false;

            if (!Single.TryParse(components[0], style, provider, out Single x))
                return false;
            if (!Single.TryParse(components[1], style, provider, out Single y))
                return false;
            if (!Single.TryParse(components[2], style, provider, out Single z))
                return false;
            if (!Single.TryParse(components[3], style, provider, out Single d))
                return false;

            v = new Plane(x, y, z, d);
            return true;
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Plane"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static Plane Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            var v = default(Plane);
            if (!TryParse(s, style, provider, out v))
                throw new FormatException();
            
            return v;
        }
    }
}
