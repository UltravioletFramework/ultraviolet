using System;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Contains extension methods for the <see cref="ModifierKeys"/> enumeration.
    /// </summary>
    public static class ModifierKeysExtensions
    {
        /// <summary>
        /// Gets a value indicating whether one <see cref="ModifierKeys"/> value
        /// is equivalent to the specified value. This is like an equality comparison,
        /// but it ignores the <see cref="ModifierKeys.Repeat"/> value.
        /// </summary>
        /// <param name="this">The first value to compare.</param>
        /// <param name="keys">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equivalent; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsEquivalentTo(this ModifierKeys @this, ModifierKeys keys)
        {
            return (@this & ~ModifierKeys.Repeat) == (keys & ~ModifierKeys.Repeat);
        }
    }
}
