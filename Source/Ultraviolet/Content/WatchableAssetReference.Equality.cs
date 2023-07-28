using System;

namespace Ultraviolet.Content
{
    partial struct WatchableAssetReference<T>
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode() => reference?.GetHashCode() ?? 0;
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(WatchableAssetReference<T> v1, WatchableAssetReference<T> v2) => ReferenceEquals(v1.reference, v2.reference);
        
        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(WatchableAssetReference<T> v1, WatchableAssetReference<T> v2) => !ReferenceEquals(v1.reference, v2.reference);

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(WatchableAssetReference<T> v1, T v2) => ReferenceEquals(v1.reference, v2);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(WatchableAssetReference<T> v1, T v2) => !ReferenceEquals(v1.reference, v2);

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(T v1, WatchableAssetReference<T> v2) => ReferenceEquals(v1, v2.reference);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(T v1, WatchableAssetReference<T> v2) => !ReferenceEquals(v1, v2.reference);

        /// <inheritdoc/>
        public override Boolean Equals(Object other) => ReferenceEquals(this.reference, other);

        /// <inheritdoc/>
        public Boolean Equals(WatchableAssetReference<T> other) => ReferenceEquals(this.reference, other.reference);

        /// <inheritdoc/>
        public Boolean Equals(T other) => ReferenceEquals(this.reference, other);
    }
}
