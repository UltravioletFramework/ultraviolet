using System;
using System.Linq;
using System.Reflection;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents the types of storyboard loop specifiers which are understood by the UVSS parser.
    /// </summary>
    public static class KnownLoopBehaviors
    {
        /// <summary>
        /// Initializes the <see cref="KnownLoopBehaviors"/> class.
        /// </summary>
        static KnownLoopBehaviors()
        {
            knownBehaviors = typeof(KnownLoopBehaviors).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.FieldType == typeof(String)).Select(x => (String)x.GetValue(null)).ToArray();
        }

        /// <summary>
        /// Gets a value indicating whether the specified string matches
        /// one of the known loop types.
        /// </summary>
        /// <param name="value">The string to evaluate.</param>
        /// <returns>true if the specified string matches one of the 
        /// known loop types; otherwise, false.</returns>
        public static Boolean IsKnownLoopBehavior(String value)
        {
            return knownBehaviors.Contains(value);
        }

        /// <summary>
        /// The storyboard does not loop.
        /// </summary>
        public const String None = "none";

        /// <summary>
        /// The storyboard loops normally.
        /// </summary>
        public const String Loop = "loop";

        /// <summary>
        /// The storyboard loops, reversing direction each time.
        /// </summary>
        public const String Reverse = "reverse";

        // Array of all known easing functions.
        private static readonly String[] knownBehaviors;
    }
}
