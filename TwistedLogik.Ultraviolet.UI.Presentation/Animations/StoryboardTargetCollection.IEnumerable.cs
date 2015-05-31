using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    partial class StoryboardTargetCollection
    {
        /// <inheritdoc/>
        public List<StoryboardTarget>.Enumerator GetEnumerator()
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
