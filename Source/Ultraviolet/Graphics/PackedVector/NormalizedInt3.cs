using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Represents a 96-bit packed vector consisting of 3 normalized 32-bit values.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(UInt32) * 3)]
    public partial struct NormalizedInt3
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedInt3"/> structure from the specified vector.
        /// </summary>
        /// <param name="vector">The vector from which to create the packed instance.</param>
        public NormalizedInt3(Vector3 vector)
        {
            this.X = PackedVectorUtils.PackNormalizedSigned(PackingMask, vector.X);
            this.Y = PackedVectorUtils.PackNormalizedSigned(PackingMask, vector.Y);
            this.Z = PackedVectorUtils.PackNormalizedSigned(PackingMask, vector.Z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedInt3"/> structure from the specified vector components.
        /// </summary>
        /// <param name="x">The x-component from  which to create the packed instance.</param>
        /// <param name="y">The y-component from  which to create the packed instance.</param>
        /// <param name="z">The z-component from  which to create the packed instance.</param>
        public NormalizedInt3(Single x, Single y, Single z)
        {
            this.X = PackedVectorUtils.PackNormalizedSigned(PackingMask, x);
            this.Y = PackedVectorUtils.PackNormalizedSigned(PackingMask, y);
            this.Z = PackedVectorUtils.PackNormalizedSigned(PackingMask, z);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{X:X}{Y:X}{Z:X}";

        /// <summary>
        /// Converts the <see cref="NormalizedInt3"/> instance to a <see cref="Vector3"/> instance.
        /// </summary>
        /// <returns>The <see cref="Vector3"/> instance which was created.</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(
                PackedVectorUtils.UnpackNormalizedSigned(PackingMask, X),
                PackedVectorUtils.UnpackNormalizedSigned(PackingMask, Y),
                PackedVectorUtils.UnpackNormalizedSigned(PackingMask, Z));
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

        // Packing mask for this vector type.
        private const UInt32 PackingMask = 0xFFFFFFFF;
    }
}
