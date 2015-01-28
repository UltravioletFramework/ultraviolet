using System;
using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class VisualStateGroup
    {
        /// <inheritdoc/>
        public Dictionary<String, VisualState>.Enumerator GetEnumerator()
        {
            return states.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<String, VisualState>> IEnumerable<KeyValuePair<String, VisualState>>.GetEnumerator()
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
