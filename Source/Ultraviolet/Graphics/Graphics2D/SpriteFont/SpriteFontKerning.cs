using System;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics2D
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
            if (Char.IsSurrogate(c1) || Char.IsSurrogate(c2))
                return 0;

            if (c1 < asciiLookup.Length && !asciiLookup[c1])
                return DefaultAdjustment;

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
            if (Char.IsSurrogate(pair.FirstCharacter) || Char.IsSurrogate(pair.SecondCharacter))
                return 0;

            var c1 = pair.FirstCharacter;
            if (c1 < asciiLookup.Length && !asciiLookup[c1])
                return DefaultAdjustment;

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

            if (c1 < asciiLookup.Length)
                asciiLookup[c1] = true;
        }

        /// <summary>
        /// Sets the kerning offset for the specified pair of characters.
        /// </summary>
        /// <param name="pair">The character pair.</param>
        /// <param name="value">The kerning offset to set for the specified pair of characters.</param>
        public void Set(SpriteFontKerningPair pair, Int32 value)
        {
            kerning[pair] = value;

            var c1 = pair.FirstCharacter;
            if (c1 < asciiLookup.Length)
                asciiLookup[c1] = true;
        }

        /// <summary>
        /// Removes the specified character pair from the kerning data.
        /// </summary>
        /// <param name="c1">The first character in the pair.</param>
        /// <param name="c2">The second character in the pair.</param>
        /// <returns><see langword="true"/> if the character pair was removed; otherwise, <see langword="false"/>.</returns>
        public Boolean Remove(Char c1, Char c2)
        {
            return kerning.Remove(new SpriteFontKerningPair(c1, c2));
        }

        /// <summary>
        /// Removes the specified character pair from the kerning data.
        /// </summary>
        /// <param name="pair">The character pair.</param>
        /// <returns><see langword="true"/> if the character pair was removed; otherwise, <see langword="false"/>.</returns>
        public Boolean Remove(SpriteFontKerningPair pair)
        {
            return kerning.Remove(pair);
        }

        /// <summary>
        /// Gets a value indicating whether the kerning data contains the specified character pair.
        /// </summary>
        /// <param name="c1">The first character in the pair.</param>
        /// <param name="c2">The second character in the pair.</param>
        /// <returns><see langword="true"/> if the kerning data contains the specified character pair; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(Char c1, Char c2)
        {
            if (c1 < asciiLookup.Length && !asciiLookup[c1])
                return false;

            return kerning.ContainsKey(new SpriteFontKerningPair(c1, c2));
        }

        /// <summary>
        /// Gets a value indicating whether the kerning data contains the specified character pair.
        /// </summary>
        /// <param name="pair">The character pair.</param>
        /// <returns><see langword="true"/> if the kerning data contains the specified character pair; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(SpriteFontKerningPair pair)
        {
            var c1 = pair.FirstCharacter;
            if (c1 < asciiLookup.Length && !asciiLookup[c1])
                return false;

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
        private readonly Boolean[] asciiLookup = new Boolean[SpriteFontGlyphIndex.ExtendedAsciiCount];
        private readonly Dictionary<SpriteFontKerningPair, Int32> kerning =
            new Dictionary<SpriteFontKerningPair, Int32>();
    }
}
