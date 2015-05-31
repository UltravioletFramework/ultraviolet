using System;
using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssStyleArgumentsCollection
    {
        /// <inheritdoc/>
        public List<String>.Enumerator GetEnumerator()
        {
            return arguments.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<String> IEnumerable<String>.GetEnumerator()
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
