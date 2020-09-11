using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Represents a 32-bit packed vector consisting of 4 8-bit values.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(Byte) * 4)]
    public partial struct SByte4
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SByte4"/> structure from the specified vector.
        /// </summary>
        /// <param name="vector">The vector from which to create the packed instance.</param>
        public SByte4(Vector4 vector)
        {
            this.X = (Byte)PackedVectorUtils.PackSigned(PackingMask, vector.X);
            this.Y = (Byte)PackedVectorUtils.PackSigned(PackingMask, vector.Y);
            this.Z = (Byte)PackedVectorUtils.PackSigned(PackingMask, vector.Z);
            this.W = (Byte)PackedVectorUtils.PackSigned(PackingMask, vector.W);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Byte4"/> structure from the specified vector components.
        /// </summary>
        /// <param name="x">The x-component from  which to create the packed instance.</param>
        /// <param name="y">The y-component from  which to create the packed instance.</param>
        /// <param name="z">The z-component from  which to create the packed instance.</param>
        /// <param name="w">The w-component from  which to create the packed instance.</param>
        public SByte4(Single x, Single y, Single z, Single w)
        {
            this.X = (Byte)PackedVectorUtils.PackSigned(PackingMask, x);
            this.Y = (Byte)PackedVectorUtils.PackSigned(PackingMask, y);
            this.Z = (Byte)PackedVectorUtils.PackSigned(PackingMask, z);
            this.W = (Byte)PackedVectorUtils.PackSigned(PackingMask, w);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{X:X}{Y:X}{Z:X}{W:X}";

        /// <summary>
        /// Converts the <see cref="SByte4"/> instance to a <see cref="Vector4"/> instance.
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
        /// Gets or sets the vector's X component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(0)]
        public Byte X;

        /// <summary>
        /// Gets or sets the vector's Y component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(1)]
        public Byte Y;

        /// <summary>
        /// Gets or sets the vector's Z component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(2)]
        public Byte Z;

        /// <summary>
        /// Gets or sets the vector's W component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(3)]
        public Byte W;

        // Packing mask for this vector type.
        private const UInt32 PackingMask = 0xFF;
    }
}
