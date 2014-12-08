using System;
using System.Collections.Generic;
using System.Linq;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    /// <summary>
    /// Represents a list of styles in an Ultraviolet Stylesheet (UVSS) document.
    /// </summary>
    public sealed partial class UvssStyleList : IEnumerable<UvssStyle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStyleList"/> class.
        /// </summary>
        /// <param name="styles">A collection containing the styles to add to the list.</param>
        internal UvssStyleList(IEnumerable<UvssStyle> styles)
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
