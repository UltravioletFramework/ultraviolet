using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssStoryboardAnimationCollection
    {
        /// <inheritdoc/>
        public List<UvssStoryboardAnimation>.Enumerator GetEnumerator()
        {
            return animations.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssStoryboardAnimation> IEnumerable<UvssStoryboardAnimation>.GetEnumerator()
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
