using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Represents an 8-bit packed vector consisting of 1 8-bit value.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(Byte))]
    public partial struct SByte1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SByte1"/> structure from the specified vector components.
        /// </summary>
        /// <param name="x">The x-component from  which to create the packed instance.</param>
        public SByte1(Single x)
        {
            this.X = (Byte)PackedVectorUtils.PackSigned(PackingMask, x);
        }

        /// <inheritdoc/>
        public override String ToString() => X.ToString("X");

        /// <summary>
        /// Converts the <see cref="Byte1"/> instance to a <see cref="Single"/> instance.
        /// </summary>
        /// <returns>The <see cref="Single"/> instance which was created.</returns>
        public Single ToSingle()
        {
            return PackedVectorUtils.UnpackSigned(PackingMask, X);
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
