using System;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a rule set in an Ultraviolet Style Sheet (UVSS) document.
    /// </summary>
    public sealed class UvssRuleSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRuleSet"/> class.
        /// </summary>
        /// <param name="selectors">The rule's selectors.</param>
        /// <param name="rules">The rule's styling rules.</param>
        /// <param name="triggers">The rule's triggers.</param>
        internal UvssRuleSet(
            UvssSelectorCollection selectors,
            UvssRuleCollection rules,
            UvssTriggerCollection triggers)
        {
            this.selectors = selectors;
            this.rules = rules;
            this.triggers = triggers;
        }

        /// <inheritdoc/>
        public override String ToString() => String.Format("{0} {{ {1} }}", Selectors, Rules);

        /// <summary>
        /// Gets a value indicating whether this rule set is applied to the view's resource manager.
        /// </summary>
        /// <returns><see langword="true"/> if this rule set is applied to the view's resource manager; otherwise, <see langword="false"/>.</returns>
        public Boolean IsViewResourceRule()
        {
            if (selectors.Count != 1)
                return false;
            
            return selectors[0].IsViewResourceSelector;
        }

        /// <summary>
        /// Gets a value indicating whether the rule set matches the specified UI element.
        /// </summary>
        /// <param name="element">The UI element to evaluate.</param>
        /// <param name="selector">The selector that matches the element, if any.</param>
        /// <returns><see langword="true"/> if the rule set matches the specified UI element; otherwise, <see langword="false"/>.</returns>
        public Boolean MatchesElement(UIElement element, out UvssSelector selector)
        {
            selector = null;
            foreach (var potentialMatch in selectors)
            {
                if (potentialMatch.MatchesElement(element))
                {
                    if (selector == null || potentialMatch.ComparePriority(selector) >= 0)
                    {
                        selector = potentialMatch;
                    }
                }
            }
            return selector != null;
        }

        /// <summary>
        /// Gets the rule set's selectors.
        /// </summary>
        public UvssSelectorCollection Selectors
        {
            get { return selectors; }
        }
        
        /// <summary>
        /// Gets the rule set's styling rules.
        /// </summary>
        public UvssRuleCollection Rules
        {
            get { return rules; }
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
        private readonly UvssRuleCollection rules;
        private readonly UvssTriggerCollection triggers;
    }
}
