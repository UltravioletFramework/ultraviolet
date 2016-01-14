using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssRuleCollection
    {
        /// <inheritdoc/>
        public List<UvssRule>.Enumerator GetEnumerator()
        {
            return rules.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssRule> IEnumerable<UvssRule>.GetEnumerator()
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
