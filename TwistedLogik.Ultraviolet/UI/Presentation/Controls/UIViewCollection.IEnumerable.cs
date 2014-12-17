using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    partial class UIViewCollection
    {
        /// <inheritdoc/>
        public LinkedList<UIView>.Enumerator GetEnumerator()
        {
            return views.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UIView> IEnumerable<UIView>.GetEnumerator()
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
