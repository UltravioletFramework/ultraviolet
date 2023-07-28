using System;

namespace Ultraviolet.Presentation
{
    partial struct SourcedResource<T>
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Resource?.GetHashCode() ?? 0;
                hash = hash * 23 + Asset.GetHashCode();
                hash = hash * 23 + Source.GetHashCode();
                return hash;
            }
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(SourcedResource<T> v1, SourcedResource<T> v2)
        {
            return v1.Equals(v2);
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(SourcedResource<T> v1, SourcedResource<T> v2)
        {
            return !v1.Equals(v2);
        }
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            return (other is SourcedResource<T> x) ? Equals(x) : false;
        }
        
        /// <inheritdoc/>
        public Boolean Equals(SourcedResource<T> other)
        {
            return
                this.Resource == other.Resource &&
                this.Asset == other.Asset &&
                this.Source == other.Source;
        }
    }
}
