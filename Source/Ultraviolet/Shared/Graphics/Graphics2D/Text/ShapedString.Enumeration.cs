using System.Collections;
using System.Collections.Generic;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial class ShapedString : IEnumerable, IEnumerable<ShapedChar>
    {
        /// <summary>
        /// Retrieves an object that can iterate through the individual characters in this string.
        /// </summary>
        /// <returns>An enumerator object.</returns>
        public ArrayEnumerator<ShapedChar> GetEnumerator() => new ArrayEnumerator<ShapedChar>(buffer);

        /// <inheritdoc/>
        IEnumerator<ShapedChar> IEnumerable<ShapedChar>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
