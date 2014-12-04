using System;
using TwistedLogik.Ultraviolet.Layout.Elements;

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
        /// Gets a value indicating whether the rule matches the specified UI element.
        /// </summary>
        /// <param name="element">The UI element to evaluate.</param>
        /// <param name="selector">The selector that matches the element, if any.</param>
        /// <returns><c>true</c> if the rule matches the specified UI element; otherwise, <c>false</c>.</returns>
        public Boolean MatchesElement(UIElement element, out UvssSelector selector)
        {
            var priority = 0;

            selector = null;
            foreach (var potentialMatch in selectors)
            {
                if (potentialMatch.MatchesElement(element))
                {
                    if (potentialMatch.Priority >= priority)
                    {
                        selector = potentialMatch;
                        priority = selector.Priority;
                    }
                }
            }
            return selector != null;
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
