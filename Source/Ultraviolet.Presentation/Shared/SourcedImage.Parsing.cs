using System;
using System.Globalization;
using Ultraviolet.Core.Data;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Presentation
{
    partial struct SourcedImage
    {        
        /// <summary>
        /// Converts the string representation of a <see cref="SourcedImage"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out SourcedImage v)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v);
        }

        /// <summary>
        /// Converts the string representation of a <see cref="SourcedImage"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static SourcedImage Parse(String s)
        {
            var v = default(SourcedImage);
            if (!TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v))
                throw new FormatException();

            return v;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="SourcedImage"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out SourcedImage v)
        {
            var source = AssetSource.Global;

            if (s.EndsWith(" local"))
            {
                source = AssetSource.Local;
                s = s.Substring(0, s.Length - " local".Length);
            }
            else if (s.EndsWith(" global"))
            {
                source = AssetSource.Global;
                s = s.Substring(0, s.Length - " global".Length);
            }

            var asset = (TextureImage)ObjectResolver.FromString(s.Trim(), typeof(TextureImage), provider);
            v = new SourcedImage(asset, source);

            return true;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="SourcedImage"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static SourcedImage Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            var v = default(SourcedImage);
            if (!TryParse(s, style, provider, out v))
                throw new FormatException();

            return v;
        }        
    }
}