using System;

namespace Ultraviolet.Graphics.PackedVector
{
    /// <summary>
    /// Contains methods for creating and manipulating packed vectors.
    /// </summary>
    internal static class PackedVectorUtils
    {
        /// <summary>
        /// Constrains a value to an integer in the specified range.
        /// </summary>
        /// <param name="value">The value to constrain.</param>
        /// <param name="min">The minimum possible value.</param>
        /// <param name="max">The maximum possible value.</param>
        /// <returns>The constrained value.</returns>
        public static Double ConstrainValue(Single value, Single min, Single max)
        {
            if (Single.IsNaN(value))
                value = 0.0f;

            return
                (value > max) ? max :
                (value < min) ? min : Math.Round(value);
        }

        /// <summary>
        /// Packs a signed value.
        /// </summary>
        /// <param name="mask">The packing bitmask which specifies the range of values.</param>
        /// <param name="value">The value to pack.</param>
        /// <returns>The packed value.</returns>
        public static UInt32 PackSigned(UInt32 mask, Single value)
        {
            var max = (mask >> 1);
            var min = (Single)(-max - 1.0);
            return (UInt32)(Int32)ConstrainValue(value, min, max) & mask;
        }

        /// <summary>
        /// Packs an unsigned value.
        /// </summary>
        /// <param name="mask">The packing bitmask which specifies the range of values.</param>
        /// <param name="value">The value to pack.</param>
        /// <returns>The packed value.</returns>
        public static UInt32 PackUnsigned(UInt32 mask, Single value)
        {
            return (UInt32)ConstrainValue(value, 0.0f, mask);
        }

        /// <summary>
        /// Packs a signed, normalized value.
        /// </summary>
        /// <param name="mask">The packing bitmask which specifies the range of values.</param>
        /// <param name="value">The value to pack.</param>
        /// <returns>The packed value.</returns>
        public static UInt32 PackNormalizedSigned(UInt32 mask, Single value)
        {
            var max = (Single)(mask >> 1);
            return (UInt32)(Int32)ConstrainValue(value * max, -max, max) & mask;
        }

        /// <summary>
        /// Packs an unsigned, normalized value.
        /// </summary>
        /// <param name="mask">The packing bitmask which specifies the range of values.</param>
        /// <param name="value">The value to pack.</param>
        /// <returns>The packed value.</returns>
        public static UInt32 PackNormalizedUnsigned(UInt32 mask, Single value)
        {
            return (UInt32)ConstrainValue(value * mask, 0.0f, mask);
        }

        /// <summary>
        /// Unpacks a signed value.
        /// </summary>
        /// <param name="mask">The packing bitmask which specifies the range of values.</param>
        /// <param name="value">The value to unpack.</param>
        /// <returns>The unpacked value.</returns>
        public static Single UnpackSigned(UInt32 mask, UInt32 value)
        {
            var signBitMask = mask + 1U >> 1;
            var signBitSet = (value & signBitMask) != 0;
            if (signBitSet)
            {
                var isMaxNegativeValue = (value & mask) == signBitMask;
                if (isMaxNegativeValue)
                    return -1f;

                value |= ~mask;
            }
            else
            {
                value &= mask;
            }

            return (Int32)value;
        }

        /// <summary>
        /// Unpacks an unsigned value.
        /// </summary>
        /// <param name="mask">The packing bitmask which specifies the range of values.</param>
        /// <param name="value">The value to unpack.</param>
        /// <returns>The unpacked value.</returns>
        public static Single UnpackUnsigned(UInt32 mask, UInt32 value)
        {
            return value &= mask;
        }

        /// <summary>
        /// Unpacks an unsigned, normalized value.
        /// </summary>
        /// <param name="mask">The packing bitmask which specifies the range of values.</param>
        /// <param name="value">The value to unpack.</param>
        /// <returns>The unpacked value.</returns>
        public static Single UnpackNormalizedSigned(UInt32 mask, UInt32 value)
        {
            var signBitMask = mask + 1U >> 1;
            var signBitSet = (value & signBitMask) != 0;
            if (signBitSet)
            {
                var isMaxNegativeValue = (value & mask) == signBitMask;
                if (isMaxNegativeValue)
                    return -1f;

                value |= ~mask;
            }
            else
            {
                value &= mask;
            }

            var max = (Single)(mask >> 1);
            return value / max;
        }

        /// <summary>
        /// Unpacks an unsigned, normalized value.
        /// </summary>
        /// <param name="mask">The packing bitmask which specifies the range of values.</param>
        /// <param name="value">The value to unpack.</param>
        /// <returns>The unpacked value.</returns>
        public static Single UnpackNormalizedUnsigned(UInt32 mask, UInt32 value)
        {
            return (Single)(value & mask) / mask;
        }
    }
}
