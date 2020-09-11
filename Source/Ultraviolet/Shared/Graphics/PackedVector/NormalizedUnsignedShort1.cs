using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Represents a 16-bit packed vector consisting of 1 normalized, unsigned 16-bit value.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(UInt16))]
    public partial struct NormalizedUnsignedShort1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedUnsignedShort1"/> structure from the specified vector components.
        /// </summary>
        /// <param name="x">The x-component from  which to create the packed instance.</param>
        public NormalizedUnsignedShort1(Single x)
        {
            this.X = (UInt16)PackedVectorUtils.PackNormalizedUnsigned(PackingMask, x);
        }

        /// <inheritdoc/>
        public override String ToString() => X.ToString("X");

        /// <summary>
        /// Converts the <see cref="NormalizedShort1"/> instance to a <see cref="Single"/> instance.
        /// </summary>
        /// <returns>The <see cref="Single"/> instance which was created.</returns>
        public Single ToSingle()
        {
            return PackedVectorUtils.UnpackNormalizedUnsigned(PackingMask, X);
        }

        /// <summary>
        /// The vector's X component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(0)]
        public UInt16 X;

        // Packing mask for this vector type.
        private const UInt32 PackingMask = 0xFFFF;
    }
}
