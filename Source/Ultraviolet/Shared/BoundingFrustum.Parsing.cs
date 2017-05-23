using System;

namespace Ultraviolet
{
    partial class BoundingFrustum
    {
        /// <inheritdoc/>
        public override String ToString()
        {
            return ToString(null);
        }
        
        /// <summary>
        /// Converts the object to a human-readable string using the specified culture information.
        /// </summary>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A human-readable string that represents the object.</returns>
        public String ToString(IFormatProvider provider)
        {
            return String.Format(provider, "Near: {{{0}}} Far: {{{1}}} Left: {{{2}}} Right: {{{3}}} Top: {{{4}}} Bottom: {{{5}}}",
                Near, Far, Left, Right, Top, Bottom);
        }
    }
}
