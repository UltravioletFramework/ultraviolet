using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Represents an 8-bit packed vector consisting of 1 normalized 8-bit value.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(Byte))]
    public partial struct NormalizedSByte1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedSByte1"/> structure from the specified vector components.
        /// </summary>
        /// <param name="x">The x-component from  which to create the packed instance.</param>
        public NormalizedSByte1(Single x)
        {
            this.X = (Byte)PackedVectorUtils.PackNormalizedSigned(PackingMask, x);
        }

        /// <inheritdoc/>
        public override String ToString() => X.ToString("X");

        /// <summary>
        /// Converts the <see cref="NormalizedSByte1"/> instance to a <see cref="Single"/> instance.
        /// </summary>
        /// <returns>The <see cref="Single"/> instance which was created.</returns>
        public Single ToSingle()
        {
            return PackedVectorUtils.UnpackNormalizedSigned(PackingMask, X);
        }

        /// <summary>
        /// The vector's X component.
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(0)]
        public Byte X;

        // Packing mask for this vector type.
        private const UInt32 PackingMask = 0xFF;
    }
}
