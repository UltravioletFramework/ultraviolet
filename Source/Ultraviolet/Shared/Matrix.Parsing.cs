using System;
using System.Globalization;

namespace Ultraviolet
{
    partial struct Matrix
    {        
        /// <summary>
        /// Converts the string representation of a <see cref="Matrix"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out Matrix v)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v);
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Matrix"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static Matrix Parse(String s)
        {
            var v = default(Matrix);
            if (!TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v))
                throw new FormatException();
            
            return v;
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Matrix"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Matrix v)
        {
            v = default(Matrix);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 16)
                return false;

            var values = new Single[16];
            for (int i = 0; i < values.Length; i++)
            {
                if (!Single.TryParse(components[i], style, provider, out values[i]))
                    return false;
            }

            v = new Matrix(
                 values[0],  values[1],  values[2],  values[3],
                 values[4],  values[5],  values[6],  values[7],
                 values[8],  values[9], values[10], values[11],
                values[12], values[13], values[14], values[15]
            );

            return true;
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Matrix"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static Matrix Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            var v = default(Matrix);
            if (!TryParse(s, style, provider, out v))
                throw new FormatException();
            
            return v;
        }
    }
}
