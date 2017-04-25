using System;
using System.Collections.Generic;
using System.Linq;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a list of selectors in an Ultraviolet Style Sheet (UVSS) document.
    /// </summary>
    public sealed partial class UvssSelectorCollection : IEnumerable<UvssSelector>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorCollection"/> class.
        /// </summary>
        /// <param name="selectors">A collection containing the selectors to add to the list.</param>
        internal UvssSelectorCollection(IEnumerable<UvssSelector> selectors)
        {
            this.selectors = selectors.ToList();
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Join(", ", selectors.Select(x => x.ToString()));
        }

        /// <summary>
        /// Gets the selector at the specified index within the collection.
        /// </summary>
        /// <param name="ix">The index of the selector to retrieve.</param>
        /// <returns>The selector at the specified index within the collection.</returns>
        public UvssSelector this[Int32 ix]
        {
            get { return selectors[ix]; }
        }

        /// <summary>
        /// Gets the number of selectors in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return selectors.Count; }
        }

        // State values.
        private readonly List<UvssSelector> selectors;
    }
}
