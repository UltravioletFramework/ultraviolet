using System;
using System.Globalization;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    partial struct SpriteAnimationName
    {
        /// <summary>
        /// Converts the string representation of a <see cref="SpriteAnimationName"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, out SpriteAnimationName v)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out v);
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="SpriteAnimationName"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted value.</returns>
        public static SpriteAnimationName Parse(String s)
        {
            var v = default(SpriteAnimationName);
            if (!TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out v))
                throw new FormatException();
            
            return v;
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="SpriteAnimationName"/> to an object instance.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="v">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out SpriteAnimationName v)
        {
            Contract.Require(s, nameof(s));

            return TryParseInternal(s, out v);
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="SpriteAnimationName"/> to an object instance.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>The converted value.</returns>
        public static SpriteAnimationName Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            var v = default(SpriteAnimationName);
            if (!TryParse(s, style, provider, out v))
                throw new FormatException();
            
            return v;
        }

        /// <summary>
        /// Converts the string representation of a sprite animation name to an instance of
        /// the <see cref="SpriteAnimationName"/> structure.
        /// </summary>
        private static Boolean TryParseInternal(String s, out SpriteAnimationName name)
        {
            Int32 index;
            if (Int32.TryParse(s, out index))
            {
                name = new SpriteAnimationName(index);
                return true;
            }

            name = new SpriteAnimationName(s);
            return true;
        }
    }
}
