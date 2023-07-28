using System;
using System.Globalization;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Presentation
{
    partial struct SourcedCursor
    {
        /// <summary>
        /// Converts the string representation of a <see cref="SourcedCursor"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out SourcedCursor v)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v);
        }

        /// <summary>
        /// Converts the string representation of a <see cref="SourcedCursor"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static SourcedCursor Parse(String s)
        {
            var v = default(SourcedCursor);
            if (!TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out v))
                throw new FormatException();

            return v;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="SourcedCursor"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out SourcedCursor v)
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

            var asset = (SourcedCursorResource)ObjectResolver.FromString(s.Trim(), typeof(SourcedCursorResource), provider);
            v = new SourcedCursor(asset, source);

            return true;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="SourcedCursor"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static SourcedCursor Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            var v = default(SourcedCursor);
            if (!TryParse(s, style, provider, out v))
                throw new FormatException();

            return v;
        }        
    }
}