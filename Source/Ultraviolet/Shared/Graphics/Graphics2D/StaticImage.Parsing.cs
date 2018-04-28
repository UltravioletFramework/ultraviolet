using System;
using System.Globalization;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    partial class StaticImage
    {        
        /// <summary>
        /// Converts the string representation of a static image into an instance of the <see cref="StaticImage"/> class.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out StaticImage image)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out image);
        }

        /// <summary>
        /// Converts the string representation of a static image to an equivalent instance of the <see cref="StaticImage"/> class.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <returns>An instance of the <see cref="StretchableImage3"/> class that is equivalent to the specified string.</returns>
        public static StaticImage Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a static image into an instance of the <see cref="StaticImage"/> class.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out StaticImage image)
        {
            Contract.Require(s, nameof(s));

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);

            var texture = AssetID.Invalid;
            var x = 0;
            var y = 0;
            var width = 0;
            var height = 0;

            image = null;

            if (components.Length != 5)
                return false;

            if (!AssetID.TryParse(components[0], out texture))
                return false;

            if (!Int32.TryParse(components[1], out x))
                return false;
            if (!Int32.TryParse(components[2], out y))
                return false;
            if (!Int32.TryParse(components[3], out width))
                return false;
            if (!Int32.TryParse(components[4], out height))
                return false;

            image = Create(texture, x, y, width, height);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a static image to an equivalent instance of the <see cref="StaticImage"/> class.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>An instance of the <see cref="StaticImage"/> class that is equivalent to the specified string.</returns>
        public static StaticImage Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            StaticImage value;
            if (!TryParse(s, style, provider, out value))
            {
                throw new FormatException();
            }
            return value;
        }
    }
}
