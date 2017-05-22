using System;
using System.Globalization;
using Ultraviolet.Core;

namespace Ultraviolet
{
    partial struct Ray
    {
        /// <summary>
        /// Converts the string representation of a ray into an instance of the <see cref="Ray"/> structure.
        /// </summary>
        /// <param name="s">A string containing a ray to convert.</param>
        /// <returns>A instance of the <see cref="Ray"/> structure equivalent to the ray contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Ray Parse(String s) =>
            Parse(s, NumberStyles.Float, NumberFormatInfo.CurrentInfo);

        /// <summary>
        /// Converts the string representation of a ray into an instance of the <see cref="Ray"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a ray to convert.</param>
        /// <param name="ray">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Ray ray) =>
            TryParse(s, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out ray);

        /// <summary>
        /// Converts the string representation of a ray into an instance of the <see cref="Ray"/> structure.
        /// </summary>
        /// <param name="s">A string containing a ray to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Ray"/> structure equivalent to the ray contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Ray Parse(String s, NumberStyles style, IFormatProvider provider) =>
            TryParse(s, style, provider, out Ray ray) ? ray : throw new FormatException();

        /// <summary>
        /// Converts the string representation of a ray into an instance of the <see cref="Ray"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a ray to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="ray">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Ray ray)
        {
            ray = default(Ray);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 6)
                return false;

            if (!Single.TryParse(components[0], style, provider, out Single px))
                return false;
            if (!Single.TryParse(components[1], style, provider, out Single py))
                return false;
            if (!Single.TryParse(components[2], style, provider, out Single pz))
                return false;

            if (!Single.TryParse(components[3], style, provider, out Single dx))
                return false;
            if (!Single.TryParse(components[4], style, provider, out Single dy))
                return false;
            if (!Single.TryParse(components[5], style, provider, out Single dz))
                return false;

            ray = new Ray(new Vector3(px, py, pz), new Vector3(dx, dy, dz));
            return true;
        }

        /// <summary>
        /// Converts the object to a human-readable string using the specified culture information.
        /// </summary>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A human-readable string that represents the object.</returns>
        [Preserve]
        public String ToString(IFormatProvider provider) =>
            String.Format(provider, "{0} {1}", Position, Direction);

        /// <inheritdoc/>
        [Preserve]
        public override String ToString() =>
            ToString(null);
    }
}
