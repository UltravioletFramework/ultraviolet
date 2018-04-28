using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a pair of glyphs and their associated kerning information.
    /// </summary>
    public partial struct SpriteFontKerningPair : IEquatable<SpriteFontKerningPair>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFontKerningPair"/> structure.
        /// </summary>
        /// <param name="firstCharacter">The first character in the pair.</param>
        /// <param name="secondCharacter">The second character in the pair.</param>
        public SpriteFontKerningPair(Char firstCharacter, Char secondCharacter)
        {
            this.firstCharacter = firstCharacter;
            this.secondCharacter = secondCharacter;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{firstCharacter}{secondCharacter}";

        /// <summary>
        /// Gets the first character in the pair.
        /// </summary>
        public Char FirstCharacter
        {
            get { return firstCharacter; }
        }

        /// <summary>
        /// Gets the second character in the pair.
        /// </summary>
        public Char SecondCharacter
        {
            get { return secondCharacter; }
        }

        // Property values.
        private readonly Char firstCharacter;
        private readonly Char secondCharacter;
    }
}
