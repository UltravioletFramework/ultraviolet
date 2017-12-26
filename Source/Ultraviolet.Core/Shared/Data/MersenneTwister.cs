using System;

namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// Represents a pseudorandom number generator implementing the Mersenne Twister algorithm.
    /// </summary>
    public sealed class MersenneTwister : Random
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MersenneTwister"/> class with a random seed based on the current system time.
        /// </summary>
        public MersenneTwister()
            : this(Environment.TickCount)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MersenneTwister"/> class with the specified random seed.
        /// </summary>
        /// <param name="seed">The psuedorandom number generator's initial seed.</param>
        public MersenneTwister(Int32 seed)
        {
            Reseed(seed);
        }

        /// <summary>
        /// Resets the random number generator to the beginning of the sequence specified by the given random seed.
        /// </summary>
        /// <param name="seed">The seed to which to reset the random number generator.</param>
        public void Reseed(Int32 seed)
        {
            mt[0] = unchecked((uint)seed);
            for (var i = 1; i < mt.Length; i++)
                mt[i] = (uint)(1812433253u * (mt[i - 1] ^ (mt[i - 1] >> 30)) + i);
            index = 0;
        }

        /// <summary>
        /// Returns a nonnegative random number.
        /// </summary>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than MaxValue.</returns>
        public override Int32 Next()
        {
            return Next(0, Int32.MaxValue);
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.</param>
        /// <returns>A 32-bit signed integer greater than or equal to zero, and less than maxValue; that is, the range of return 
        /// values ordinarily includes zero but not maxValue. However, if maxValue equals zero, maxValue is returned.</returns>
        public override Int32 Next(Int32 maxValue)
        {
            return Next(0, maxValue);
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of 
        /// return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        public override Int32 Next(Int32 minValue, Int32 maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException(CoreStrings.MinMustBeLessThanMax.Format("minValue", "maxValue"));
            
            if (minValue == maxValue)
                return minValue;

            var span = maxValue - minValue;
            return minValue + (int)Math.Floor(span * NextDouble());
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
        public override Double NextDouble()
        {
            return Sample();
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        public override void NextBytes(Byte[] buffer)
        {
            Contract.Require(buffer, nameof(buffer));

            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = (byte)Next(0, Byte.MaxValue + 1);
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
        protected override Double Sample()
        {
            return (double)ExtractNumber() / ((long)UInt32.MaxValue + 1);
        }

        /// <summary>
        /// Rebuilds the generator's array of untempered numbers.
        /// </summary>
        private void GenerateNumbers()
        {
            for (int i = 0; i < mt.Length; i++)
            {
                var y = (mt[i] & UpperMask) + (mt[(i + 1) % mt.Length] & LowerMask);
                mt[i] = mt[(i + M) % N] ^ (y >> 1);
                if (y % 2 != 0)
                    mt[i] = mt[i] ^ MatrixA;
            }
        }

        /// <summary>
        /// Extracts a number from the generator sequence.
        /// </summary>
        /// <returns>The extracted number.</returns>
        private uint ExtractNumber()
        {
            if (index == 0)
                GenerateNumbers();

            var y = mt[index];
            y ^= (y >> 1);
            y ^= (y << 7) & TemperingMaskB;
            y ^= (y << 16) & TemperingMaskC;
            y ^= (y >> 18);

            index = (index + 1) % mt.Length;

            return y;
        }

        // Generator state.
        private readonly UInt32[] mt = new UInt32[N];
        private Int32 index;

        // Period parameters.
        private const Int32 N = 624;
        private const Int32 M = 397;
        private const UInt32 MatrixA = 0x9908b0df;
        private const UInt32 UpperMask = 0x80000000;
        private const UInt32 LowerMask = 0x7fffffff;

        // Tempering parameters.
        private const UInt32 TemperingMaskB = 0x9d2c5680;
        private const UInt32 TemperingMaskC = 0xefc60000;
    }
}
