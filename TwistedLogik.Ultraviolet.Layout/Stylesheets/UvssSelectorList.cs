using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    /// <summary>
    /// Represents a list of selectors in an Ultraviolet Stylesheet (UVSS) document.
    /// </summary>
    public sealed class UvssSelectorList : IEnumerable<UvssSelector>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorList"/> class.
        /// </summary>
        /// <param name="selectors">A collection containing the selectors to add to the list.</param>
        internal UvssSelectorList(IEnumerable<UvssSelector> selectors)
        {
            this.selectors = selectors.ToList();
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Join(", ", selectors.Select(x => x.ToString()));
        }

        /// <inheritdoc/>
        List<UvssSelector>.Enumerator GetEnumerator()
        {
            return selectors.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssSelector> IEnumerable<UvssSelector>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // State values.
        private readonly List<UvssSelector> selectors;
    }
}
