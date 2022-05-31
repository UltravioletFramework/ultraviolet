using System;

namespace Ultraviolet.Presentation
{
    partial struct VersionedStringSource
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                if (sourceString != null)
                    return sourceString.GetHashCode();

                if (sourceStringBuilder != null)
                {
                    var hash = 17;
                    hash = hash * 23 + sourceStringBuilder.GetHashCode();
                    hash = hash * 23 + version.GetHashCode();
                    return hash;
                }
            }
            return 0;
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(VersionedStringSource v1, VersionedStringSource v2)
        {
            return v1.Equals(v2);
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(VersionedStringSource v1, VersionedStringSource v2)
        {
            return !v1.Equals(v2);
        }
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            return (other is VersionedStringSource x) ? Equals(x) : false;
        }
        
        /// <inheritdoc/>
        public Boolean Equals(VersionedStringSource other)
        {
            if (sourceString != null && other.sourceString != null)
                return sourceString == other.sourceString;

            if (sourceStringBuilder != null && other.sourceStringBuilder != null)
                return sourceStringBuilder == other.sourceStringBuilder && version == other.version;

            return !IsValid && !other.IsValid;
        }
    }
}
