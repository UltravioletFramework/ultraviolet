using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Represents a 128-bit packed vector consisting of 4 32-bit values.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(UInt32) * 4)]
    public partial struct Int4
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Int4"/> structure from the specified vector.
        /// </summary>
        /// <param name="vector">The vector from which to create the packed instance.</param>
        public Int4(Vector4 vector)
        {
            this.X = PackedVectorUtils.PackSigned(PackingMask, vector.X);
            this.Y = PackedVectorUtils.PackSigned(PackingMask, vector.Y);
            this.Z = PackedVectorUtils.PackSigned(PackingMask, vector.Z);
            this.W = PackedVectorUtils.PackSigned(PackingMask, vector.W);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int4"/> structure from the specified vector components.
        /// </summary>
        /// <param name="x">The x-component from  which to create the packed instance.</param>
        /// <param name="y">The y-component from  which to create the packed instance.</param>
        /// <param name="z">The z-component from  which to create the packed instance.</param>
        /// <param name="w">The w-component from  which to create the packed instance.</param>
        public Int4(Single x, Single y, Single z, Single w)
        {
            this.X = PackedVectorUtils.PackSigned(PackingMask, x);
            this.Y = PackedVectorUtils.PackSigned(PackingMask, y);
            this.Z = PackedVectorUtils.PackSigned(PackingMask, z);
            this.W = PackedVectorUtils.PackSigned(PackingMask, w);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{X:X}{Y:X}{Z:X}{W:X}";

        /// <summary>
        /// Converts the <see cref="Int4"/> instance to a <see cref="Vector4"/> instance.
        /// </summary>
        /// <returns>The <see cref="Vector4"/> instance which was created.</returns>
        public Vector4 ToVector4()
        {
            return new Vector4(
                PackedVectorUtils.UnpackSigned(PackingMask, X),
                PackedVectorUtils.UnpackSigned(PackingMask, Y),
                PackedVectorUtils.UnpackSigned(PackingMask, Z),
                PackedVectorUtils.UnpackSigned(PackingMask, W));
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

        /// <summary>
        /// The vector's Z component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(8)]
        public UInt32 Z;

        /// <summary>
        /// The vector's W component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(12)]
        public UInt32 W;

        // Packing mask for this vector type.
        private const UInt32 PackingMask = 0xFFFFFFFF;
    }
}
