using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    public partial class VisualCollection : IList<Visual>
    {
        /// <inheritdoc/>
        public List<Visual>.Enumerator GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<Visual> IEnumerable<Visual>.GetEnumerator()
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
