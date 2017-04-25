using System;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Contains useful mathematical functions.
    /// </summary>
    public static class MathUtil
    {
        /// <summary>
        /// Gets a value indicating whether the specified <see cref="Single"/> value is zero to within a reasonable approximation.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><see langword="true"/> if the specified value is zero or approximately zero; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyZero(Single value)
        {
            return Math.Abs(value) < 1E-7;
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="Single"/> value is non-zero to within a reasonable approximation.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><see langword="true"/> if the specified value is non-zero; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyNonZero(Single value)
        {
            return Math.Abs(value) >= 1E-7;
        }

        /// <summary>
        /// Gets a value indicating whether <paramref name="value1"/> is greater than <paramref name="value2"/> to within a reasonable approximation.
        /// </summary>
        /// <param name="value1">The first value to evaluate.</param>
        /// <param name="value2">The second value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is greater than <paramref name="value2"/>; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyGreaterThan(Single value1, Single value2)
        {
            if (value1 == value2)
                return true;

            return Math.Abs(value1 - value2) >= 1E-7 && value1 > value2;
        }

        /// <summary>
        /// Gets a value indicating whether <paramref name="value1"/> is greater than or equal to <paramref name="value2"/> to within a reasonable approximation.
        /// </summary>
        /// <param name="value1">The first value to evaluate.</param>
        /// <param name="value2">The second value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is greater than or equal to <paramref name="value2"/>; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyGreaterThanOrEqual(Single value1, Single value2)
        {
            if (value1 == value2)
                return true;

            return Math.Abs(value1 - value2) < 1E-7 || value1 > value2;
        }

        /// <summary>
        /// Gets a value indicating whether <paramref name="value1"/> is less than <paramref name="value2"/> to within a reasonable approximation.
        /// </summary>
        /// <param name="value1">The first value to evaluate.</param>
        /// <param name="value2">The second value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is less than <paramref name="value2"/>; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyLessThan(Single value1, Single value2)
        {
            if (value1 == value2)
                return false;

            return Math.Abs(value1 - value2) >= 1E-7 && value1 < value2;
        }

        /// <summary>
        /// Gets a value indicating whether <paramref name="value1"/> is less than or equal to <paramref name="value2"/> to within a reasonable approximation.
        /// </summary>
        /// <param name="value1">The first value to evaluate.</param>
        /// <param name="value2">The second value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is less than or equal to <paramref name="value2"/>; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyLessThanOrEqualTo(Single value1, Single value2)
        {
            if (value1 == value2)
                return true;

            return Math.Abs(value1 - value2) < 1E-7 || value1 < value2;
        }

        /// <summary>
        /// Gets a value indicating whether <paramref name="value1"/> is equal to <paramref name="value2"/> to within a reasonable approximation.
        /// </summary>
        /// <param name="value1">The first value to evaluate.</param>
        /// <param name="value2">The second value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is equal to <paramref name="value2"/>; otherwise, <see langword="false"/>.</returns>
        public static Boolean AreApproximatelyEqual(Single value1, Single value2)
        {
            if (value1 == value2)
                return true;

            return Math.Abs(value1 - value2) < 1E-7;
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="Double"/> value is zero to within a reasonable approximation.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><see langword="true"/> if the specified value is zero or approximately zero; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyZero(Double value)
        {
            return Math.Abs(value) < 1E-15;
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="Double"/> value is non-zero to within a reasonable approximation.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><see langword="true"/> if the specified value is non-zero; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyNonZero(Double value)
        {
            return Math.Abs(value) >= 1E-15;
        }

        /// <summary>
        /// Gets a value indicating whether <paramref name="value1"/> is greater than <paramref name="value2"/> to within a reasonable approximation.
        /// </summary>
        /// <param name="value1">The first value to evaluate.</param>
        /// <param name="value2">The second value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is greater than <paramref name="value2"/>; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyGreaterThan(Double value1, Double value2)
        {
            if (value1 == value2)
                return false;

            return Math.Abs(value1 - value2) >= 1E-15 && value1 > value2;
        }

        /// <summary>
        /// Gets a value indicating whether <paramref name="value1"/> is greater than or equal to <paramref name="value2"/> to within a reasonable approximation.
        /// </summary>
        /// <param name="value1">The first value to evaluate.</param>
        /// <param name="value2">The second value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is greater than or equal to <paramref name="value2"/>; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyGreaterThanOrEqual(Double value1, Double value2)
        {
            if (value1 == value2)
                return true;

            return Math.Abs(value1 - value2) < 1E-15 || value1 > value2;
        }

        /// <summary>
        /// Gets a value indicating whether <paramref name="value1"/> is less than <paramref name="value2"/> to within a reasonable approximation.
        /// </summary>
        /// <param name="value1">The first value to evaluate.</param>
        /// <param name="value2">The second value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is less than <paramref name="value2"/>; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyLessThan(Double value1, Double value2)
        {
            if (value1 == value2)
                return true;

            return Math.Abs(value1 - value2) >= 1E-15 && value1 < value2;
        }

        /// <summary>
        /// Gets a value indicating whether <paramref name="value1"/> is less than or equal to <paramref name="value2"/> to within a reasonable approximation.
        /// </summary>
        /// <param name="value1">The first value to evaluate.</param>
        /// <param name="value2">The second value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is less than or equal to <paramref name="value2"/>; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsApproximatelyLessThanOrEqualTo(Double value1, Double value2)
        {
            if (value1 == value2)
                return true;

            return Math.Abs(value1 - value2) < 1E-15 || value1 < value2;
        }

        /// <summary>
        /// Gets a value indicating whether <paramref name="value1"/> is equal to <paramref name="value2"/> to within a reasonable approximation.
        /// </summary>
        /// <param name="value1">The first value to evaluate.</param>
        /// <param name="value2">The second value to evaluate.</param>
        /// <returns><see langword="true"/> if <paramref name="value1"/> is equal to <paramref name="value2"/>; otherwise, <see langword="false"/>.</returns>
        public static Boolean AreApproximatelyEqual(Double value1, Double value2)
        {
            if (value1 == value2)
                return true;
            
            return Math.Abs(value1 - value2) < 1E-15;
        }        

        /// <summary>
        /// Finds the next power of two that is higher than the specified value.
        /// </summary>
        /// <param name="k">The value to evaluate.</param>
        /// <returns>The next power of two that is higher than the specified value.</returns>
        public static Int32 FindNextPowerOfTwo(Int32 k)
        {
            k--;
            for (int i = 1; i < sizeof(int) * 8; i <<= 1)
            {
                k = k | k >> i;
            }
            return k + 1;
        }

        /// <summary>
        /// Clamps a value to the specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum possible value.</param>
        /// <param name="max">The maximum possible value.</param>
        /// <returns>The clamped value.</returns>
        public static Byte Clamp(Byte value, Byte min, Byte max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Clamps a value to the specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum possible value.</param>
        /// <param name="max">The maximum possible value.</param>
        /// <returns>The clamped value.</returns>
        public static Int16 Clamp(Int16 value, Int16 min, Int16 max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Clamps a value to the specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum possible value.</param>
        /// <param name="max">The maximum possible value.</param>
        /// <returns>The clamped value.</returns>
        public static Int32 Clamp(Int32 value, Int32 min, Int32 max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Clamps a value to the specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum possible value.</param>
        /// <param name="max">The maximum possible value.</param>
        /// <returns>The clamped value.</returns>
        public static Int64 Clamp(Int64 value, Int64 min, Int64 max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Clamps a value to the specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum possible value.</param>
        /// <param name="max">The maximum possible value.</param>
        /// <returns>The clamped value.</returns>
        [CLSCompliant(false)]
        public static UInt16 Clamp(UInt16 value, UInt16 min, UInt16 max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Clamps a value to the specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum possible value.</param>
        /// <param name="max">The maximum possible value.</param>
        /// <returns>The clamped value.</returns>
        [CLSCompliant(false)]
        public static UInt32 Clamp(UInt32 value, UInt32 min, UInt32 max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Clamps a value to the specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum possible value.</param>
        /// <param name="max">The maximum possible value.</param>
        /// <returns>The clamped value.</returns>
        [CLSCompliant(false)]
        public static UInt64 Clamp(UInt64 value, UInt64 min, UInt64 max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Clamps a value to the specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum possible value.</param>
        /// <param name="max">The maximum possible value.</param>
        /// <returns>The clamped value.</returns>
        public static Single Clamp(Single value, Single min, Single max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Clamps a value to the specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum possible value.</param>
        /// <param name="max">The maximum possible value.</param>
        /// <returns>The clamped value.</returns>
        public static Double Clamp(Double value, Double min, Double max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        public static Byte Lerp(Byte value1, Byte value2, Single amount)
        {
            return (Byte)(value1 + ((value2 - value1) * amount));
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        public static Int16 Lerp(Int16 value1, Int16 value2, Single amount)
        {
            return (Int16)(value1 + ((value2 - value1) * amount));
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        public static Int32 Lerp(Int32 value1, Int32 value2, Single amount)
        {
            return (Int32)(value1 + ((value2 - value1) * amount));
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        public static Int64 Lerp(Int64 value1, Int64 value2, Single amount)
        {
            return (Int64)(value1 + ((value2 - value1) * amount));
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        [CLSCompliant(false)]
        public static UInt16 Lerp(UInt16 value1, UInt16 value2, Single amount)
        {
            return (UInt16)(value1 + ((value2 - value1) * amount));
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        [CLSCompliant(false)]
        public static UInt32 Lerp(UInt32 value1, UInt32 value2, Single amount)
        {
            return (UInt32)(value1 + ((value2 - value1) * amount));
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        [CLSCompliant(false)]
        public static UInt64 Lerp(UInt64 value1, UInt64 value2, Single amount)
        {
            return (UInt64)(value1 + ((value2 - value1) * amount));
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        public static Single Lerp(Single value1, Single value2, Single amount)
        {
            return value1 + ((value2 - value1) * amount);
        }

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        public static Double Lerp(Double value1, Double value2, Single amount)
        {
            return value1 + ((value2 - value1) * amount);
        }
    }
}
