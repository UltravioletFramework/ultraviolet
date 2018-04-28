using System;
using System.Globalization;

namespace Ultraviolet
{
    partial struct Color
    {        
        /// <summary>
        /// Converts the string representation of a <see cref="Color"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out Color v)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out v);
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Color"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static Color Parse(String s)
        {
            var v = default(Color);
            if (!TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out v))
                throw new FormatException();
            
            return v;
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Color"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Color v)
        {
            v = default(Color);

            if (String.IsNullOrEmpty(s))
                return false;

            var trimmed = s.Trim();
            if (trimmed.StartsWith("#"))
            {
                switch (trimmed.Length)
                {
                    case 4:
                    case 5:
                    case 7:
                    case 9:
                        return TryParseHex(trimmed, style, provider, ref v);

                    default:
                        return false;
                }
            }
            else
            {
                return TryParseDelimitedOrNamed(trimmed, style, provider, ref v);
            }
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Color"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static Color Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            var v = default(Color);
            if (!TryParse(s, style, provider, out v))
                throw new FormatException();
            
            return v;
        }
        
        /// <summary>
        /// Converts the string representation of a color into an instance of the <see cref="Color"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a color to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="color">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <remarks>Strings parsed by this method must be in one of the following formats:
        /// <list type="bullet">
        /// <item><description>#rgb</description></item>
        /// <item><description>#argb</description></item>
        /// <item><description>#rrggbb</description></item>
        /// <item><description>#aarrggbb</description></item>
        /// </list>
        /// </remarks>
        private static Boolean TryParseHex(String s, NumberStyles style, IFormatProvider provider, ref Color color)
        {
            var length = s.Length;
            var isShortForm = (length == 4 || length == 5);
            var isAlphaAvailable = (length == 5 || length == 9);

            var substrPos = 1;
            var substrLen = isShortForm ? 1 : 2;

            var astr = isShortForm ? "F" : "FF";
            if (isAlphaAvailable)
            {
                astr = s.Substring(substrPos, substrLen);
                substrPos += substrLen;
            }

            var rstr = s.Substring(substrPos, substrLen);
            substrPos += substrLen;

            var gstr = s.Substring(substrPos, substrLen);
            substrPos += substrLen;

            var bstr = s.Substring(substrPos, substrLen);
            substrPos += substrLen;
            
            if (!Int32.TryParse(astr, NumberStyles.AllowHexSpecifier, provider, out Int32 a))
                return false;
            if (!Int32.TryParse(rstr, NumberStyles.AllowHexSpecifier, provider, out Int32 r))
                return false;
            if (!Int32.TryParse(gstr, NumberStyles.AllowHexSpecifier, provider, out Int32 g))
                return false;
            if (!Int32.TryParse(bstr, NumberStyles.AllowHexSpecifier, provider, out Int32 b))
                return false;

            if (isShortForm)
            {
                a = (16 * a) + a;
                r = (16 * r) + r;
                g = (16 * g) + g;
                b = (16 * b) + b;
            }

            color = new Color(r, g, b, a);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a color into an instance of the <see cref="Color"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a color to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="color">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <remarks>Strings parsed by this methods must be comma-delimited lists of three or four color components,
        /// in either R, G, B or A, R, G, B format, or a named color.</remarks>
        private static Boolean TryParseDelimitedOrNamed(String s, NumberStyles style, IFormatProvider provider, ref Color color)
        {
            var components = s.Split(',');
            if (components.Length == 1)
            {
                return NamedColorRegistry.TryGetValue(components[0], out color);
            }
            if (components.Length == 3)
            {
                Int32 r, g, b;
                if (!Int32.TryParse(components[0], style, provider, out r))
                    return false;
                if (!Int32.TryParse(components[1], style, provider, out g))
                    return false;
                if (!Int32.TryParse(components[2], style, provider, out b))
                    return false;

                color = new Color(r, g, b);
                return true;
            }
            if (components.Length == 4)
            {
                Int32 a, r, g, b;
                if (!Int32.TryParse(components[0], style, provider, out a))
                    return false;
                if (!Int32.TryParse(components[1], style, provider, out r))
                    return false;
                if (!Int32.TryParse(components[2], style, provider, out g))
                    return false;
                if (!Int32.TryParse(components[3], style, provider, out b))
                    return false;

                color = new Color(r, g, b, a);
                return true;
            }
            return false;
        }
    }
}
