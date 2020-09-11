using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Represents a 32-bit packed vector consisting of 2 normalized, unsigned 16-bit values.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(UInt16) * 2)]
    public partial struct NormalizedUnsignedShort2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedUnsignedShort2"/> structure from the specified vector.
        /// </summary>
        /// <param name="vector">The vector from which to create the packed instance.</param>
        public NormalizedUnsignedShort2(Vector2 vector)
        {
            this.X = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, vector.X);
            this.Y = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, vector.Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedUnsignedShort3"/> structure from the specified vector components.
        /// </summary>
        /// <param name="x">The x-component from  which to create the packed instance.</param>
        /// <param name="y">The y-component from  which to create the packed instance.</param>
        public NormalizedUnsignedShort2(Single x, Single y)
        {
            this.X = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, x);
            this.Y = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, y);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{X:X}{Y:X}";

        /// <summary>
        /// Converts the <see cref="NormalizedUnsignedShort2"/> instance to a <see cref="Vector2"/> instance.
        /// </summary>
        /// <returns>The <see cref="Vector2"/> instance which was created.</returns>
        public Vector2 ToVector2()
        {
            return new Vector2(
                PackedVectorUtils.UnpackNormalizedUnsigned(PackingMask, X),
                PackedVectorUtils.UnpackNormalizedUnsigned(PackingMask, Y));
        }

        /// <summary>
        /// The vector's X component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(0)]
        public UInt16 X;

        /// <summary>
        /// The vector's Y component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(2)]
        public UInt16 Y;

        // Packing mask for this vector type.
        private const UInt32 PackingMask = 0xFFFF;
    }
}
