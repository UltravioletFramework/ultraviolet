using System;
using System.Globalization;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    partial class StretchableImage3
    {        
        /// <summary>
        /// Converts the string representation of a stretchable image into an instance of the <see cref="StretchableImage3"/> class.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out StretchableImage3 image)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out image);
        }

        /// <summary>
        /// Converts the string representation of a stretchable image to an equivalent instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <returns>An instance of the <see cref="StretchableImage3"/> class that is equivalent to the specified string.</returns>
        public static StretchableImage3 Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a stretchable image into an instance of the <see cref="StretchableImage3"/> class.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="image">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out StretchableImage3 image)
        {
            Contract.Require(s, nameof(s));

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);

            var texture = AssetID.Invalid;
            var x = 0;
            var y = 0;
            var width = 0;
            var height = 0;
            var left = 0;
            var right = 0;
            var tileCenter = false;
            var tileEdges = false;
            var vertical = false;

            image = null;

            switch (components.Length)
            {
                case 7:
                case 8:
                case 9:
                case 10:
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
                    if (!Int32.TryParse(components[5], out left))
                        return false;
                    if (!Int32.TryParse(components[6], out right))
                        return false;
                    if (components.Length > 7)
                    {
                        if (!ParseTilingParameter3(components[7], ref tileCenter, ref tileEdges, ref vertical))
                            return false;
                    }
                    if (components.Length > 8)
                    {
                        if (!ParseTilingParameter3(components[8], ref tileCenter, ref tileEdges, ref vertical))
                            return false;
                    }
                    if (components.Length > 9)
                    {
                        if (!ParseTilingParameter3(components[9], ref tileCenter, ref tileEdges, ref vertical))
                            return false;
                    }
                    break;

                default:
                    return false;
            }

            image = Create(texture, x, y, width, height, left, right);
            image.TileCenter = tileCenter;
            image.TileEdges = tileEdges;
            image.Vertical = vertical;
            return true;
        }

        /// <summary>
        /// Converts the string representation of a stretchable image to an equivalent instance of the <see cref="StretchableImage3"/> class.
        /// </summary>
        /// <param name="s">A string containing the image to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>An instance of the <see cref="StretchableImage3"/> class that is equivalent to the specified string.</returns>
        public static StretchableImage3 Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            StretchableImage3 value;
            if (!TryParse(s, style, provider, out value))
            {
                throw new FormatException();
            }
            return value;
        }
    }
}
