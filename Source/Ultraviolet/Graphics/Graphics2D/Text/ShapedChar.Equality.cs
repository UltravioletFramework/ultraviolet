using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial struct ShapedChar : IEquatable<ShapedChar>
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + GlyphIndex.GetHashCode();
                hash = hash * 23 + OffsetX.GetHashCode();
                hash = hash * 23 + OffsetY.GetHashCode();
                hash = hash * 23 + Advance.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(ShapedChar v1, ShapedChar v2) => 
            v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(ShapedChar v1, ShapedChar v2) => 
            !v1.Equals(v2);

        /// <inheritdoc/>
        public override Boolean Equals(Object obj) =>
            (obj is ShapedChar c) ? Equals(c) : false;

        /// <inheritdoc/>
        public Boolean Equals(ShapedChar other)
        {
            return
                this.GlyphIndex == other.GlyphIndex &&
                this.OffsetX == other.OffsetX &&
                this.OffsetY == other.OffsetY &&
                this.Advance == other.Advance;
        }
    }
}
