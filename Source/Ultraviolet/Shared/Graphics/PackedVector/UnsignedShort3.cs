using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Represents a 48-bit packed vector consisting of 3 unsigned 16-bit values.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(UInt16) * 3)]
    public partial struct UnsignedShort3
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsignedShort3"/> structure from the specified vector.
        /// </summary>
        /// <param name="vector">The vector from which to create the packed instance.</param>
        public UnsignedShort3(Vector3 vector)
        {
            this.X = (UInt16)PackedVectorUtils.PackUnsigned(PackingMask, vector.X);
            this.Y = (UInt16)PackedVectorUtils.PackUnsigned(PackingMask, vector.Y);
            this.Z = (UInt16)PackedVectorUtils.PackUnsigned(PackingMask, vector.Z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsignedShort3"/> structure from the specified vector components.
        /// </summary>
        /// <param name="x">The x-component from  which to create the packed instance.</param>
        /// <param name="y">The y-component from  which to create the packed instance.</param>
        /// <param name="z">The z-component from  which to create the packed instance.</param>
        public UnsignedShort3(Single x, Single y, Single z)
        {
            this.X = (UInt16)PackedVectorUtils.PackUnsigned(PackingMask, x);
            this.Y = (UInt16)PackedVectorUtils.PackUnsigned(PackingMask, y);
            this.Z = (UInt16)PackedVectorUtils.PackUnsigned(PackingMask, z);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{X:X}{Y:X}{Z:X}";

        /// <summary>
        /// Converts the <see cref="UnsignedShort3"/> instance to a <see cref="Vector3"/> instance.
        /// </summary>
        /// <returns>The <see cref="Vector3"/> instance which was created.</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(
                PackedVectorUtils.UnpackUnsigned(PackingMask, X),
                PackedVectorUtils.UnpackUnsigned(PackingMask, Y),
                PackedVectorUtils.UnpackUnsigned(PackingMask, Z));
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

        // Packing mask for this vector type.
        private const UInt32 PackingMask = 0xFFFF;
    }
}
