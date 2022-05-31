using System;
using Newtonsoft.Json;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Represents a masked 64-bit integer.
    /// </summary>
    /// <remarks>Masking allows an integer to be stored, on average, with fewer than 8 bytes of memory. To do this, the integer value
    /// is treated as a sequence of bytes, and any bytes which have a value of zero are omitted from the output stream. Masking requires
    /// the integer value to be prepended with an additional byte of data, the masking byte, which tracks which integer bytes have non-zero
    /// values; this means that the size of a masked 64-bit integer is 2 bytes in the best case and 9 bytes in the worst case.</remarks>
    [CLSCompliant(false)]
    [JsonConverter(typeof(CoreJsonConverter))]
    public partial struct MaskedUInt64 : IEquatable<MaskedUInt64>
    {
        /// <summary>
        /// Initializes a new instance of the MaskedUInt64 structure.
        /// </summary>
        /// <param name="value">The underlying value.</param>
        public MaskedUInt64(UInt64 value)
        {
            Value = value;
        }
        
        /// <summary>
        /// Implicitly converts the masked integer to a 32-bit unsigned integer.
        /// </summary>
        /// <param name="masked">The masked integer to convert.</param>
        /// <returns>The converted integer.</returns>
        public static implicit operator UInt64(MaskedUInt64 masked)
        {
            return masked.Value;
        }

        /// <summary>
        /// Implicitly converts a 32-bit unsigned integer to a masked integer.
        /// </summary>
        /// <param name="value">The integer to convert.</param>
        /// <returns>The converted masked integer.</returns>
        public static implicit operator MaskedUInt64(UInt64 value)
        {
            return new MaskedUInt64(value);
        }

        /// <inheritdoc/>
        public override String ToString() => Value.ToString();

        /// <summary>
        /// Creates a copy of this integer with the specified byte set to the specified value.
        /// </summary>
        /// <param name="byteIndex">The index of the byte to set.</param>
        /// <param name="byteValue">The value to set in the specified byte.</param>
        /// <returns>A new <see cref="MaskedUInt64"/> with the specified byte set to the specified value.</returns>
        public MaskedUInt64 WithByte(Int32 byteIndex, Byte byteValue)
        {
            var value = Value;
            value |= (ulong)byteValue << (byteIndex * 8);
            return new MaskedUInt64(value);
        }

        /// <summary>
        /// Gets the mask for this value.
        /// </summary>
        /// <returns>The mask for this value.</returns>
        public byte GetMask()
        {
            byte mask = 0;
            for (int i = 0; i < sizeof(long); i++)
                if (GetByte(i) != 0) mask |= (byte)(1 << i);
            return mask;
        }

        /// <summary>
        /// Gets the value of the byte with the specified index.
        /// </summary>
        /// <param name="ix">The index of the byte to retrieve.</param>
        /// <returns>The value of the byte with the specified index.</returns>
        public byte GetByte(Int32 ix)
        {
            var shift = (ix * 8);
            var mask = (ulong)0xFF << shift;
            return (byte)((Value & mask) >> shift);
        }

        /// <summary>
        /// Gets the size of the integer in bytes.
        /// </summary>
        /// <returns>The size of the integer in bytes.</returns>
        internal Int32 GetSizeInBytes()
        {
            var size = 1;
            if ((Value & 0x00000000000000FF) != 0) size++;
            if ((Value & 0x000000000000FF00) != 0) size++;
            if ((Value & 0x0000000000FF0000) != 0) size++;
            if ((Value & 0x00000000FF000000) != 0) size++;
            if ((Value & 0x000000FF00000000) != 0) size++;
            if ((Value & 0x0000FF0000000000) != 0) size++;
            if ((Value & 0x00FF000000000000) != 0) size++;
            if ((Value & 0xFF00000000000000) != 0) size++;
            return size;
        }

        /// <summary>
        /// Gets the underlying integer value.
        /// </summary>
        public readonly UInt64 Value;
    }
}