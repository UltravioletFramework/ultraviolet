using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssStyleCollection
    {
        /// <inheritdoc/>
        public List<UvssStyle>.Enumerator GetEnumerator()
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
