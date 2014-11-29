using System;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    /// <summary>
    /// Represents a rule in an Ultraviolet Stylesheet (UVSS) document.
    /// </summary>
    public sealed class UvssRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRule"/> class.
        /// </summary>
        /// <param name="selectors">The rule's selectors.</param>
        /// <param name="styles">The rule's styles.</param>
        internal UvssRule(UvssSelectorList selectors, UvssStyleList styles)
        {
            this.selectors = selectors;
            this.styles    = styles;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Format("{0} {{ {1} }}", Selectors, Styles);
        }

        /// <summary>
        /// Gets the rule's selectors.
        /// </summary>
        public UvssSelectorList Selectors
        {
            get { return selectors; }
        }

        /// <summary>
        /// Gets the rule's styles.
        /// </summary>
        public UvssStyleList Styles
        {
            get { return styles; }
        }

        // State values.
        private readonly UvssSelectorList selectors;
        private readonly UvssStyleList styles;
    }
}
