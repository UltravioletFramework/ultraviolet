using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
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
