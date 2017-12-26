using System;

namespace Ultraviolet.Presentation.Styles
{
    partial class UvssDocument
    {
        /// <summary>
        /// Represents a rule set that has been categorized based on its selector
        /// for faster querying.
        /// </summary>
        private partial struct CategorizedRuleSet : IEquatable<CategorizedRuleSet>
        {
            /// <summary>
            /// Initializes a new instance of thhe <see cref="CategorizedRuleSet"/> structure.
            /// </summary>
            /// <param name="selector">The selector which applies the categorized rule set.</param>
            /// <param name="ruleSet">The rule set which has been categorized.</param>
            /// <param name="index">The index of the rule set within the style sheet.</param>
            public CategorizedRuleSet(UvssSelector selector, UvssRuleSet ruleSet, Int32 index)
            {
                this.Selector = selector;
                this.RuleSet = ruleSet;
                this.Index = index;
            }
            
            /// <summary>
            /// Gets the selector which applies the categorized rule set.
            /// </summary>
            public UvssSelector Selector { get; }

            /// <summary>
            /// Gets the rule set which has been categorized.
            /// </summary>
            public UvssRuleSet RuleSet { get; }

            /// <summary>
            /// Gets the index of the rule set within the style sheet.
            /// </summary>
            public Int32 Index { get; }
        }
    }
}
