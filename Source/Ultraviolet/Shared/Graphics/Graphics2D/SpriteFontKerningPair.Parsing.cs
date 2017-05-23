using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    partial struct SpriteFontKerningPair
    {
        /// <inheritdoc/>
        public override String ToString()
        {
            return ToString(null);
        }
        
        /// <summary>
        /// Converts the object to a human-readable string using the specified culture information.
        /// </summary>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A human-readable string that represents the object.</returns>
        public String ToString(IFormatProvider provider)
        {
            return String.Format("{0}{1}", firstCharacter, secondCharacter);
        }
    }
}
