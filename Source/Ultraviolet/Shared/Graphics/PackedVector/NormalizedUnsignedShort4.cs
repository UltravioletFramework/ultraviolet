using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Represents a 64-bit packed vector consisting of 4 normalized, unsigned 16-bit values.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(UInt16) * 4)]
    public partial struct NormalizedUnsignedShort4
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedUnsignedShort4"/> structure from the specified vector.
        /// </summary>
        /// <param name="vector">The vector from which to create the packed instance.</param>
        public NormalizedUnsignedShort4(Vector4 vector)
        {
            this.X = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, vector.X);
            this.Y = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, vector.Y);
            this.Z = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, vector.Z);
            this.W = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, vector.W);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedUnsignedShort4"/> structure from the specified vector components.
        /// </summary>
        /// <param name="x">The x-component from  which to create the packed instance.</param>
        /// <param name="y">The y-component from  which to create the packed instance.</param>
        /// <param name="z">The z-component from  which to create the packed instance.</param>
        /// <param name="w">The w-component from  which to create the packed instance.</param>
        public NormalizedUnsignedShort4(Single x, Single y, Single z, Single w)
        {
            this.X = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, x);
            this.Y = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, y);
            this.Z = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, z);
            this.W = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, w);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{X:X}{Y:X}{Z:X}{W:X}";

        /// <summary>
        /// Converts the <see cref="NormalizedUnsignedShort4"/> instance to a <see cref="Vector4"/> instance.
        /// </summary>
        /// <returns>The <see cref="Vector4"/> instance which was created.</returns>
        public Vector4 ToVector4()
        {
            return new Vector4(
                PackedVectorUtils.UnpackNormalizedUnsigned(PackingMask, X),
                PackedVectorUtils.UnpackNormalizedUnsigned(PackingMask, Y),
                PackedVectorUtils.UnpackNormalizedUnsigned(PackingMask, Z),
                PackedVectorUtils.UnpackNormalizedUnsigned(PackingMask, W));
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

        /// <summary>
        /// The vector's Z component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(4)]
        public UInt16 Z;

        /// <summary>
        /// The vector's W component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(6)]
        public UInt16 W;

        // Packing mask for this vector type.
        private const UInt32 PackingMask = 0xFFFF;
    }
}
