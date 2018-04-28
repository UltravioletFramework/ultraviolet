using System;
using System.Globalization;

namespace Ultraviolet
{
    partial struct Rectangle
    {
        /// <summary>
        /// Converts the string representation of a <see cref="Rectangle"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out Rectangle v)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out v);
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Rectangle"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static Rectangle Parse(String s)
        {
            var v = default(Rectangle);
            if (!TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out v))
                throw new FormatException();
            
            return v;
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Rectangle"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Rectangle v)
        {
            v = default(Rectangle);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 4)
                return false;
            
            if (!Int32.TryParse(components[0], style, provider, out Int32 x))
                return false;
            if (!Int32.TryParse(components[1], style, provider, out Int32 y))
                return false;
            if (!Int32.TryParse(components[2], style, provider, out Int32 width))
                return false;
            if (!Int32.TryParse(components[3], style, provider, out Int32 height))
                return false;

            v = new Rectangle(x, y, width, height);
            return true;
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Rectangle"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static Rectangle Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            var v = default(Rectangle);
            if (!TryParse(s, style, provider, out v))
                throw new FormatException();
            
            return v;
        }
    }
}
