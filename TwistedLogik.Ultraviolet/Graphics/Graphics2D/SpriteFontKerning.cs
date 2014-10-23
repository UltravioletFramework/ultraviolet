using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the kerning information for a <see cref="SpriteFont"/>.
    /// </summary>
    public class SpriteFontKerning
    {
        /// <summary>
        /// Gets the kerning offset for the specified pair of characters.
        /// </summary>
        /// <param name="c1">The first character in the pair.</param>
        /// <param name="c2">The second character in the pair.</param>
        /// <returns>The kerning offset for the specified pair of characters.</returns>
        public Int32 Get(Char c1, Char c2)
        {
            int offset;
            if (!kerning.TryGetValue(new SpriteFontKerningPair(c1, c2), out offset))
            {
                return DefaultAdjustment;
            }
            return offset;
        }

        /// <summary>
        /// Gets the kerning offset for the specified pair of characters.
        /// </summary>
        /// <param name="pair">The character pair.</param>
        /// <returns>The kerning offset for the specified pair of characters.</returns>
        public Int32 Get(SpriteFontKerningPair pair)
        {
            int offset;
            if (!kerning.TryGetValue(pair, out offset))
            {
                return DefaultAdjustment;
            }
            return offset;
        }

        /// <summary>
        /// Sets the kerning offset for the specified pair of characters.
        /// </summary>
        /// <param name="c1">The first character in the pair.</param>
        /// <param name="c2">The second character in the pair.</param>
        /// <param name="value">The kerning offset to set for the specified pair of characters.</param>
        public void Set(Char c1, Char c2, Int32 value)
        {
            var pair = new SpriteFontKerningPair(c1, c2);
            kerning[pair] = value;
        }

        /// <summary>
        /// Sets the kerning offset for the specified pair of characters.
        /// </summary>
        /// <param name="pair">The character pair.</param>
        /// <param name="value">The kerning offset to set for the specified pair of characters.</param>
        public void Set(SpriteFontKerningPair pair, Int32 value)
        {
            kerning[pair] = value;
        }

        /// <summary>
        /// Removes the specified character pair from the kerning data.
        /// </summary>
        /// <param name="c1">The first character in the pair.</param>
        /// <param name="c2">The second character in the pair.</param>
        /// <returns><c>true</c> if the character pair was removed; otherwise, <c>false</c>.</returns>
        public Boolean Remove(Char c1, Char c2)
        {
            return kerning.Remove(new SpriteFontKerningPair(c1, c2));
        }

        /// <summary>
        /// Removes the specified character pair from the kerning data.
        /// </summary>
        /// <param name="pair">The character pair.</param>
        /// <returns><c>true</c> if the character pair was removed; otherwise, <c>false</c>.</returns>
        public Boolean Remove(SpriteFontKerningPair pair)
        {
            return kerning.Remove(pair);
        }

        /// <summary>
        /// Gets a value indicating whether the kerning data contains the specified character pair.
        /// </summary>
        /// <param name="c1">The first character in the pair.</param>
        /// <param name="c2">The second character in the pair.</param>
        /// <returns><c>true</c> if the kerning data contains the specified character pair; otherwise, <c>false</c>.</returns>
        public Boolean Contains(Char c1, Char c2)
        {
            return kerning.ContainsKey(new SpriteFontKerningPair(c1, c2));
        }

        /// <summary>
        /// Gets a value indicating whether the kerning data contains the specified character pair.
        /// </summary>
        /// <param name="pair">The character pair.</param>
        /// <returns><c>true</c> if the kerning data contains the specified character pair; otherwise, <c>false</c>.</returns>
        public Boolean Contains(SpriteFontKerningPair pair)
        {
            return kerning.ContainsKey(pair);
        }

        /// <summary>
        /// Gets or sets the default adjustment applied to all character pairs in this font.
        /// </summary>
        public Int32 DefaultAdjustment
        {
            get;
            set;
        }

        // Kerning values.
        private readonly Dictionary<SpriteFontKerningPair, Int32> kerning = 
            new Dictionary<SpriteFontKerningPair, Int32>();
    }
}
