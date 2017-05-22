using System;
using Ultraviolet.Core;

namespace Ultraviolet
{
    partial struct Ray
    {
        /// <summary>
        /// Compares two rays for equality.
        /// </summary>
        /// <param name="ray1">The first <see cref="Ray"/> to compare.</param>
        /// <param name="ray2">The second <see cref="Ray"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified rays are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Ray ray1, Ray ray2)
        {
            return ray1.Equals(ray2);
        }

        /// <summary>
        /// Compares two rays for inequality.
        /// </summary>
        /// <param name="ray1">The first <see cref="Ray"/> to compare.</param>
        /// <param name="ray2">The second <see cref="Ray"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified rays are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(Ray ray1, Ray ray2)
        {
            return !ray1.Equals(ray2);
        }

        /// <inheritdoc/>
        [Preserve]
        public Boolean Equals(Ray other) => Position.Equals(other.Position) && Direction.Equals(other.Direction);

        /// <inheritdoc/>
        [Preserve]
        public override Boolean Equals(Object obj) => (obj is Ray r) ? Equals(r) : false;

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Position.GetHashCode();
                hash = hash * 23 + Direction.GetHashCode();
                return hash;
            }
        }
    }
}
