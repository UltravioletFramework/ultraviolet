using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    partial class StoryboardTargetCollection
    {
        /// <inheritdoc/>
        List<StoryboardTarget>.Enumerator GetEnumerator()
        {
            return targets.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<StoryboardTarget> IEnumerable<StoryboardTarget>.GetEnumerator()
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
