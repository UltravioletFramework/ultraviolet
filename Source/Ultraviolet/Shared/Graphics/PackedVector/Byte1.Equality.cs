using System;

namespace Ultraviolet.Graphics.PackedVector
{
    public partial struct Byte1 : IEquatable<Byte1>
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode() => X.GetHashCode();

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(Byte1 v1, Byte1 v2) => v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(Byte1 v1, Byte1 v2) => !v1.Equals(v2);

        /// <inheritdoc/>
        public override Boolean Equals(Object obj) => (obj is Byte1 pv) && Equals(pv);

        /// <inheritdoc/>
        public Boolean Equals(Byte1 obj) =>
            obj.X == this.X;
    }
}
