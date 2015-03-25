using System;

namespace TwistedLogik.Nucleus
{
    /// <summary>
    /// Contains common primitive values in the form of boxed objects, which can be used to prevent boxing allocations.
    /// </summary>
    public static class CommonBoxedValues
    {
        /// <summary>
        /// Contains boxed Boolean values.
        /// </summary>
        public static class Boolean
        {
            /// <summary>
            /// Gets the cached box for the specified Boolean value.
            /// </summary>
            /// <param name="value">The Boolean value for which to retrieve a cached box.</param>
            /// <returns>The cached box that was retrieved.</returns>
            public static Object FromValue(System.Boolean value)
            {
                return value ? True : False;
            }

            /// <summary>
            /// The cached box for the value true.
            /// </summary>
            public static readonly Object True = (Object)true;

            /// <summary>
            /// The cached box for the value false.
            /// </summary>
            public static readonly Object False = (Object)false;
        }

        /// <summary>
        /// Contains boxed Int32 values.
        /// </summary>
        public static class Int32
        {
            /// <summary>
            /// The cached box for the value zero (0).
            /// </summary>
            public static readonly Object Zero = (Object)0;

            /// <summary>
            /// The cached box for the value one (1).
            /// </summary>
            public static readonly Object One = (Object)1;

            /// <summary>
            /// The cached box for the value negative one (-1).
            /// </summary>
            public static readonly Object NegativeOne = (Object)(-1);

            /// <summary>
            /// The cached box for Int32.MinValue.
            /// </summary>
            public static readonly Object MinValue = (Object)System.Int32.MinValue;

            /// <summary>
            /// The cached box for Int32.MaxValue.
            /// </summary>
            public static readonly Object MaxValue = (Object)System.Int32.MaxValue;
        }

        /// <summary>
        /// Contains boxed Int64 values.
        /// </summary>
        public static class Int64
        {
            /// <summary>
            /// The cached box for the value zero (0).
            /// </summary>
            public static readonly Object Zero = (Object)0L;

            /// <summary>
            /// The cached box for the value one (1).
            /// </summary>
            public static readonly Object One = (Object)1L;

            /// <summary>
            /// The cached box for the value negative one (-1).
            /// </summary>
            public static readonly Object NegativeOne = (Object)(-1L);

            /// <summary>
            /// The cached box for Int64.MinValue.
            /// </summary>
            public static readonly Object MinValue = (Object)System.Int64.MinValue;

            /// <summary>
            /// The cached box for Int64.MaxValue.
            /// </summary>
            public static readonly Object MaxValue = (Object)System.Int64.MaxValue;
        }

        /// <summary>
        /// Contains boxed Single values.
        /// </summary>
        public static class Single
        {
            /// <summary>
            /// The cached box for the value zero (0).
            /// </summary>
            public static readonly Object Zero = (Object)0f;

            /// <summary>
            /// The cached box for the value one (1).
            /// </summary>
            public static readonly Object One = (Object)1f;

            /// <summary>
            /// The cached box for the value negative one (-1).
            /// </summary>
            public static readonly Object NegativeOne = (Object)(-1f);

            /// <summary>
            /// The cached box for not a number (NaN).
            /// </summary>
            public static readonly Object NaN = (Object)System.Single.NaN;

            /// <summary>
            /// The cached box for positive infinity.
            /// </summary>
            public static readonly Object PositiveInfinity = (Object)System.Single.PositiveInfinity;

            /// <summary>
            /// The cached box for negative infinity.
            /// </summary>
            public static readonly Object NegativeInfinity = (Object)System.Single.NegativeInfinity;

            /// <summary>
            /// The cached box for Single.MinValue.
            /// </summary>
            public static readonly Object MinValue = (Object)System.Single.MinValue;

            /// <summary>
            /// The cached box for Single.MaxValue.
            /// </summary>
            public static readonly Object MaxValue = (Object)System.Single.MaxValue;
        }

        /// <summary>
        /// Contains boxed Double values.
        /// </summary>
        public static class Double
        {
            /// <summary>
            /// The cached box for the value zero (0).
            /// </summary>
            public static readonly Object Zero = (Object)0.0;

            /// <summary>
            /// The cached box for the value one (1).
            /// </summary>
            public static readonly Object One = (Object)1.0;

            /// <summary>
            /// The cached box for the value negative one (-1).
            /// </summary>
            public static readonly Object NegativeOne = (Object)(-1.0);

            /// <summary>
            /// The cached box for not a number (NaN).
            /// </summary>
            public static readonly Object NaN = (Object)System.Double.NaN;

            /// <summary>
            /// The cached box for positive infinity.
            /// </summary>
            public static readonly Object PositiveInfinity = (Object)System.Double.PositiveInfinity;

            /// <summary>
            /// The cached box for negative infinity.
            /// </summary>
            public static readonly Object NegativeInfinity = (Object)System.Double.NegativeInfinity;

            /// <summary>
            /// The cached box for Double.MinValue.
            /// </summary>
            public static readonly Object MinValue = (Object)System.Double.MinValue;

            /// <summary>
            /// The cached box for Double.MaxValue.
            /// </summary>
            public static readonly Object MaxValue = (Object)System.Double.MaxValue;
        }
    }
}
