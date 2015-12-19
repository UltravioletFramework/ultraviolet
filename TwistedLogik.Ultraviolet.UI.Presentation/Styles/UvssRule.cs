using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a rule in an Ultraviolet Style Sheet (UVSS) document.
    /// </summary>
    public sealed class UvssRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRule"/> class.
        /// </summary>
        /// <param name="selectors">The rule's selectors.</param>
        /// <param name="navigationExpression">The rule's associated navigation expression.</param>
        /// <param name="styles">The rule's styles.</param>
        /// <param name="triggers">The rule's triggers.</param>
        internal UvssRule(UvssSelectorCollection selectors, UvssNavigationExpression navigationExpression, UvssStyleCollection styles, UvssTriggerCollection triggers)
        {
            this.selectors            = selectors;
            this.navigationExpression = navigationExpression;
            this.styles               = styles;
            this.triggers             = triggers;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            var fmt = (navigationExpression != null) ? "{0} {{ {1} }} | {2}" : "{0} {{ {1} }}";
            return String.Format(fmt, Selectors, Styles, NavigationExpression);
        }

        /// <summary>
        /// Gets a value indicating whether this rule is applied to the view's resource manager.
        /// </summary>
        /// <returns><c>true</c> if this rule is applied to the view's resource manager; otherwise, <c>false</c>.</returns>
        public Boolean IsViewResourceRule()
        {
            if (selectors.Count != 1)
                return false;
            
            return selectors[0].IsViewResourceSelector;
        }

        /// <summary>
        /// Gets a value indicating whether the rule matches the specified UI element.
        /// </summary>
        /// <param name="element">The UI element to evaluate.</param>
        /// <param name="selector">The selector that matches the element, if any.</param>
        /// <returns><c>true</c> if the rule matches the specified UI element; otherwise, <c>false</c>.</returns>
        public Boolean MatchesElement(UIElement element, out UvssSelector selector)
        {
            selector = null;
            foreach (var potentialMatch in selectors)
            {
                if (potentialMatch.MatchesElement(element))
                {
                    if (selector == null || potentialMatch.IsHigherPriorityThan(selector))
                    {
                        selector = potentialMatch;
                    }
                }
            }
            return selector != null;
        }

        /// <summary>
        /// Gets the rule's selectors.
        /// </summary>
        public UvssSelectorCollection Selectors
        {
            get { return selectors; }
        }

        /// <summary>
        /// Gets the rule's navigation expression.
        /// </summary>
        public UvssNavigationExpression NavigationExpression
        {
            get { return navigationExpression; }
        }

        /// <summary>
        /// Gets the rule's styles.
        /// </summary>
        public UvssStyleCollection Styles
        {
            get { return styles; }
        }

        /// <summary>
        /// Gets the rule's triggers.
        /// </summary>
        public UvssTriggerCollection Triggers
        {
            get { return triggers; }
        }

        // State values.
        private readonly UvssSelectorCollection selectors;
        private readonly UvssNavigationExpression navigationExpression;
        private readonly UvssStyleCollection styles;
        private readonly UvssTriggerCollection triggers;
    }
}
