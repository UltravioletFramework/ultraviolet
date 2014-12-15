using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    partial class UvssStyleCollection
    {
        /// <inheritdoc/>
        List<UvssStyle>.Enumerator GetEnumerator()
        {
            return styles.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssStyle> IEnumerable<UvssStyle>.GetEnumerator()
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
