using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a pair of glyphs and their associated kerning information.
    /// </summary>
    public struct SpriteFontKerningPair : IEquatable<SpriteFontKerningPair>
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

        /// <summary>
        /// Tests two kerning pairs for equality.
        /// </summary>
        /// <param name="p1">The first <see cref="SpriteFontKerningPair"/>.</param>
        /// <param name="p2">The second <see cref="SpriteFontKerningPair"/>.</param>
        /// <returns><c>true</c> if the kerning pairs are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(SpriteFontKerningPair p1, SpriteFontKerningPair p2)
        {
            return p1.Equals(p2);
        }

        /// <summary>
        /// Tests two kerning pairs for inequality.
        /// </summary>
        /// <param name="p1">The first <see cref="SpriteFontKerningPair"/>.</param>
        /// <param name="p2">The second <see cref="SpriteFontKerningPair"/>.</param>
        /// <returns><c>true</c> if the kerning pairs are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(SpriteFontKerningPair p1, SpriteFontKerningPair p2)
        {
            return !p1.Equals(p2);
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return String.Format("{0}{1}", firstCharacter, secondCharacter);
        }

        /// <summary>
        /// Gets the hash code for this object.
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + firstCharacter.GetHashCode();
                hash = hash * 23 + secondCharacter.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this object is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare to this object.</param>
        /// <returns><c>true</c> if the objects are equal; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is SpriteFontKerningPair))
                return false;
            return Equals((SpriteFontKerningPair)obj);
        }

        /// <summary>
        /// Gets a value indicating whether this kerning pair is equal to another kerning pair.
        /// </summary>
        /// <param name="obj">The kerning pair to compare to this kerning pair.</param>
        /// <returns><c>true</c> if the kerning pairs are equal; otherwise, <c>false</c>.</returns>
        public Boolean Equals(SpriteFontKerningPair obj)
        {
            return FirstCharacter == obj.FirstCharacter && SecondCharacter == obj.SecondCharacter;
        }

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
