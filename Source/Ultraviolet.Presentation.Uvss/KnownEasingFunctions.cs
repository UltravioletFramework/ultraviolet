using System;
using System.Linq;
using System.Reflection;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents the easing function names which are recognized by the UVSS parser.
    /// </summary>
    public static class KnownEasingFunctions
    {
        /// <summary>
        /// Initializes the <see cref="KnownEasingFunctions"/> class.
        /// </summary>
        static KnownEasingFunctions()
        {
            knownEasingFunctions = typeof(KnownEasingFunctions).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.FieldType == typeof(String)).Select(x => (String)x.GetValue(null)).ToArray();
        }

        /// <summary>
        /// Gets a value indicating whether the specified string matches
        /// one of the known easing functions.
        /// </summary>
        /// <param name="value">The string to evaluate.</param>
        /// <returns>true if the specified string matches one of the known 
        /// easing functions; otherwise, false.</returns>
        public static Boolean IsKnownEasingFunction(String value)
        {
            return knownEasingFunctions.Contains(value);
        }

        /// <summary>
        /// Eases in linearly.
        /// </summary>
        public const String EaseInLinear = "ease-in-linear";

        /// <summary>
        /// Eases out linearly.
        /// </summary>
        public const String EaseOutLinear = "ease-out-linear";

        /// <summary>
        /// Eases in cubically.
        /// </summary>
        public const String EaseInCubic = "ease-in-cubic";

        /// <summary>
        /// Eases out cubically.
        /// </summary>
        public const String EaseOutCubic = "ease-out-cubic";

        /// <summary>
        /// Eases in quadratically.
        /// </summary>
        public const String EaseInQuadratic = "ease-in-quadratic";

        /// <summary>
        /// Eases out quadratically.
        /// </summary>
        public const String EaseOutQuadratic = "ease-out-quadratic";

        /// <summary>
        /// Eases in and out quadratically.
        /// </summary>
        public const String EaseInOutQuadratic = "ease-in-out-quadratic";

        /// <summary>
        /// Eases in quartically.
        /// </summary>
        public const String EaseInQuartic = "ease-in-quartic";

        /// <summary>
        /// Eases out quartically.
        /// </summary>
        public const String EaseOutQuartic = "ease-out-quartic";

        /// <summary>
        /// Eases in and out quartically.
        /// </summary>
        public const String EaseInOutQuartic = "ease-in-out-quartic";

        /// <summary>
        /// Eases in quintically.
        /// </summary>
        public const String EaseInQuintic = "ease-in-quintic";

        /// <summary>
        /// Eases out quintically.
        /// </summary>
        public const String EaseOutQuintic = "ease-out-quintic";

        /// <summary>
        /// Eases in and out quintically.
        /// </summary>
        public const String EaseInOutQuintic = "ease-in-out-quintic";

        /// <summary>
        /// Eases in sinusoidally.
        /// </summary>
        public const String EaseInSin = "ease-in-sin";

        /// <summary>
        /// Eases out sinusoidally.
        /// </summary>
        public const String EaseOutSin = "ease-out-sin";

        /// <summary>
        /// Eases in and out sinusoidally.
        /// </summary>
        public const String EaseInOutSin = "ease-in-out-sin";

        /// <summary>
        /// Eases in exponentially.
        /// </summary>
        public const String EaseInExponential = "ease-in-exponential";

        /// <summary>
        /// Eases out exponentially.
        /// </summary>
        public const String EaseOutExponential = "ease-out-exponential";

        /// <summary>
        /// Eases in and out exponentially.
        /// </summary>
        public const String EaseInOutExponential = "ease-in-out-exponential";

        /// <summary>
        /// Eases in circularly.
        /// </summary>
        public const String EaseInCircular = "ease-in-circular";

        /// <summary>
        /// Eases out circularly.
        /// </summary>
        public const String EaseOutCircular = "ease-out-circular";

        /// <summary>
        /// Eases in and out circularly.
        /// </summary>
        public const String EaseInOutCircular = "ease-in-out-circular";

        /// <summary>
        /// Eases in after backing out.
        /// </summary>
        public const String EaseInBack = "ease-in-back";

        /// <summary>
        /// Eases out after backing out.
        /// </summary>
        public const String EaseOutBack = "ease-out-back";

        /// <summary>
        /// Eases in and out after backing out.
        /// </summary>
        public const String EaseInOutBack = "ease-in-out-back";

        /// <summary>
        /// Eases in with an elastic motion.
        /// </summary>
        public const String EaseInElastic = "ease-in-elastic";

        /// <summary>
        /// Eases out with an elastic motion.
        /// </summary>
        public const String EaseOutElastic = "ease-out-elastic";

        /// <summary>
        /// Eases in and out with an elastic motion.
        /// </summary>
        public const String EaseInOutElastic = "ease-in-out-elastic";

        /// <summary>
        /// Eases in with a bouncing motion.
        /// </summary>
        public const String EaseInBounce = "ease-in-bounce";

        /// <summary>
        /// Eases out with a bouncing motion.
        /// </summary>
        public const String EaseOutBounce = "ease-out-bounce";

        /// <summary>
        /// Eases in and out with a bouncing motion.
        /// </summary>
        public const String EaseInOutBounce = "ease-in-out-bounce";

        // Array of all known loop types.
        private static readonly String[] knownEasingFunctions;
    }    
}
