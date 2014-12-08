using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    partial class UIElementCollection
    {
        /// <inheritdoc/>
        public List<UIElement>.Enumerator GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UIElement> IEnumerable<UIElement>.GetEnumerator()
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
