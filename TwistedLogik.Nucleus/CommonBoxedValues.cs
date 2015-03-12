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
        }

        /// <summary>
        /// Contains boxed Int64 values.
        /// </summary>
        public static class Int64
        {
            /// <summary>
            /// The cached box for the value zero (0).
            /// </summary>
            public static readonly Object Zero = (Object)0u;

            /// <summary>
            /// The cached box for the value one (1).
            /// </summary>
            public static readonly Object One = (Object)1u;
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
        }
    }
}
