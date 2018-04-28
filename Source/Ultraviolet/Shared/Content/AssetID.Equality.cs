using System;

namespace Ultraviolet.Content
{
    partial struct AssetID
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + manifestName?.GetHashCode() ?? 0;
                hash = hash * 23 + manifestGroup?.GetHashCode() ?? 0;
                hash = hash * 23 + assetName?.GetHashCode() ?? 0;
                hash = hash * 23 + assetPath?.GetHashCode() ?? 0;
                hash = hash * 23 + assetIndex.GetHashCode();
                return hash;
            }
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(AssetID v1, AssetID v2)
        {
            return v1.Equals(v2);
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(AssetID v1, AssetID v2)
        {
            return !v1.Equals(v2);
        }
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            return (other is AssetID x) ? Equals(x) : false;
        }
        
        /// <inheritdoc/>
        public Boolean Equals(AssetID other)
        {
            return
                this.manifestName == other.manifestName &&
                this.manifestGroup == other.manifestGroup &&
                this.assetName == other.assetName &&
                this.assetPath == other.assetPath &&
                this.assetIndex == other.assetIndex;
        }
    }
}
