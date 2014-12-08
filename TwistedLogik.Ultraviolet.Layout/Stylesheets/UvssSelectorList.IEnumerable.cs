using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    partial class UvssSelectorList
    {
        /// <inheritdoc/>
        List<UvssSelector>.Enumerator GetEnumerator()
        {
            return selectors.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssSelector> IEnumerable<UvssSelector>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
