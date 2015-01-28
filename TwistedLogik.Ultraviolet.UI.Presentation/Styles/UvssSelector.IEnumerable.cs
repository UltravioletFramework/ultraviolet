using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssSelector
    {
        /// <inheritdoc/>
        public List<UvssSelectorPart>.Enumerator GetEnumerator()
        {
            return parts.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssSelectorPart> IEnumerable<UvssSelectorPart>.GetEnumerator()
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
