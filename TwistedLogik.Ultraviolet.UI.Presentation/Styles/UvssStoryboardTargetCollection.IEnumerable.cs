using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssStoryboardTargetCollection
    {
        /// <inheritdoc/>
        public List<UvssStoryboardTarget>.Enumerator GetEnumerator()
        {
            return targets.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssStoryboardTarget> IEnumerable<UvssStoryboardTarget>.GetEnumerator()
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
