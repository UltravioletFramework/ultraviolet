using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssSelectorCollection
    {
        /// <inheritdoc/>
        public List<UvssSelector>.Enumerator GetEnumerator()
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
