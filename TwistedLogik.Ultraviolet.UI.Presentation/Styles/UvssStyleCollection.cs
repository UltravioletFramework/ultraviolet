using System;
using System.Collections.Generic;
using System.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a list of styles in an Ultraviolet Style Sheet (UVSS) document.
    /// </summary>
    public sealed partial class UvssStyleCollection : IEnumerable<UvssStyle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStyleCollection"/> class.
        /// </summary>
        /// <param name="styles">A collection containing the styles to add to the list.</param>
        internal UvssStyleCollection(IEnumerable<UvssStyle> styles)
        {
            this.styles = styles.ToList();
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Join("; ", styles.Select(x => x.ToString()));
        }

        // State values.
        private readonly List<UvssStyle> styles;
    }
}
