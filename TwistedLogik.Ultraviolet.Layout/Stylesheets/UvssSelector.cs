using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    /// <summary>
    /// Represents a selector in an Ultraviolet Stylesheet (UVSS) document.
    /// </summary>
    public sealed class UvssSelector : IEnumerable<UvssSelectorPart>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelector"/> class.
        /// </summary>
        /// <param name="parts">A collection containing the selector's parts.</param>
        internal UvssSelector(IEnumerable<UvssSelectorPart> parts)
        {
            this.parts = parts.ToList();
        }
        
        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Join(" ", parts.Select(x => x.ToString()));
        }

        /// <inheritdoc/>
        public List<UvssSelectorPart>.Enumerator GetEnumerator()
        {
            return parts.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<UvssSelectorPart> IEnumerable<UvssSelectorPart>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // State values.
        private readonly List<UvssSelectorPart> parts;
    }
}
