using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Represents a 16-bit packed vector consisting of 2 unsigned 8-bit values.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(Byte) * 2)]
    public partial struct Byte2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Byte2"/> structure from the specified vector.
        /// </summary>
        /// <param name="vector">The vector from which to create the packed instance.</param>
        public Byte2(Vector2 vector)
        {
            this.X = (Byte)PackedVectorUtils.PackUnsigned(PackingMask, vector.X);
            this.Y = (Byte)PackedVectorUtils.PackUnsigned(PackingMask, vector.Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Byte2"/> structure from the specified vector components.
        /// </summary>
        /// <param name="x">The x-component from  which to create the packed instance.</param>
        /// <param name="y">The y-component from  which to create the packed instance.</param>
        public Byte2(Single x, Single y)
        {
            this.X = (Byte)PackedVectorUtils.PackUnsigned(PackingMask, x);
            this.Y = (Byte)PackedVectorUtils.PackUnsigned(PackingMask, y);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{X:X}{Y:X}";

        /// <summary>
        /// Converts the <see cref="Byte2"/> instance to a <see cref="Vector2"/> instance.
        /// </summary>
        /// <returns>The <see cref="Vector2"/> instance which was created.</returns>
        public Vector2 ToVector2()
        {
            return new Vector2(
                PackedVectorUtils.UnpackUnsigned(PackingMask, X),
                PackedVectorUtils.UnpackUnsigned(PackingMask, Y));
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

        // Packing mask for this vector type.
        private const UInt32 PackingMask = 0xFF;
    }
}
