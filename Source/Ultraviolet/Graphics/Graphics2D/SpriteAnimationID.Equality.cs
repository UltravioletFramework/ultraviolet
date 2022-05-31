using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    partial struct SpriteAnimationID
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + spriteAssetID.GetHashCode();
                hash = hash * 23 + animationName?.GetHashCode() ?? 0;
                hash = hash * 23 + animationIndex.GetHashCode();
                return hash;
            }
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(SpriteAnimationID v1, SpriteAnimationID v2)
        {
            return v1.Equals(v2);
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(SpriteAnimationID v1, SpriteAnimationID v2)
        {
            return !v1.Equals(v2);
        }
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            return (other is SpriteAnimationID x) ? Equals(x) : false;
        }
        
        /// <inheritdoc/>
        public Boolean Equals(SpriteAnimationID other)
        {
            return
                this.spriteAssetID == other.spriteAssetID &&
                this.animationName == other.animationName &&
                this.animationIndex == other.animationIndex;
        }
    }
}
