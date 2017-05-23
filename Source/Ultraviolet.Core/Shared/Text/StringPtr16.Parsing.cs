using System;

namespace Ultraviolet.Core.Text
{
    partial struct StringPtr16
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
            unsafe
            {
                return new String((char*)ptr, 0, length);
            }
        }
    }
}