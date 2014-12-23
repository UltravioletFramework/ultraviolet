using System;
using System.Collections;
using System.Collections.Generic;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class LayoutDocumentElementRegistry : IEnumerable<KeyValuePair<String, UIElement>>
    {
        /// <inheritdoc/>
        public Dictionary<String, UIElement>.Enumerator GetEnumerator()
        {
            return elementsByID.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<String, UIElement>> IEnumerable<KeyValuePair<String, UIElement>>.GetEnumerator()
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
