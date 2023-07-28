using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Represents a 64-bit packed vector consisting of 2 unsigned 32-bit values.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(UInt32) * 2)]
    public partial struct UnsignedInt2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsignedInt2"/> structure from the specified vector.
        /// </summary>
        /// <param name="vector">The vector from which to create the packed instance.</param>
        public UnsignedInt2(Vector2 vector)
        {
            this.X = PackedVectorUtils.PackUnsigned(PackingMask, vector.X);
            this.Y = PackedVectorUtils.PackUnsigned(PackingMask, vector.Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsignedInt2"/> structure from the specified vector components.
        /// </summary>
        /// <param name="x">The x-component from  which to create the packed instance.</param>
        /// <param name="y">The y-component from  which to create the packed instance.</param>
        public UnsignedInt2(Single x, Single y)
        {
            this.X = PackedVectorUtils.PackUnsigned(PackingMask, x);
            this.Y = PackedVectorUtils.PackUnsigned(PackingMask, y);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{X:X}{Y:X}";

        /// <summary>
        /// Converts the <see cref="UnsignedInt2"/> instance to a <see cref="Vector2"/> instance.
        /// </summary>
        /// <returns>The <see cref="Vector2"/> instance which was created.</returns>
        public Vector2 ToVector2()
        {
            return new Vector2(
                PackedVectorUtils.UnpackUnsigned(PackingMask, X),
                PackedVectorUtils.UnpackUnsigned(PackingMask, Y));
        }

        /// <summary>
        /// The vector's X component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(0)]
        public UInt32 X;

        /// <summary>
        /// The vector's Y component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(4)]
        public UInt32 Y;

        // Packing mask for this vector type.
        private const UInt32 PackingMask = 0xFFFFFFFF;
    }
}
