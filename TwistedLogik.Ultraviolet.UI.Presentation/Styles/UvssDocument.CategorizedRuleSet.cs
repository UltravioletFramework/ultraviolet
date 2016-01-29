using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssDocument
    {
        /// <summary>
        /// Represents a rule set that has been categorized based on its selector
        /// for faster querying.
        /// </summary>
        private struct CategorizedRuleSet : IEquatable<CategorizedRuleSet>
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

            /// <inheritdoc/>
            public override Int32 GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + this.Selector.GetHashCode();
                    hash = hash * 23 + this.RuleSet.GetHashCode();
                    hash = hash * 23 + this.Index.GetHashCode();
                    return hash;
                }
            }

            /// <inheritdoc/>
            public override Boolean Equals(Object obj)
            {
                if (!(obj is CategorizedRuleSet))
                    return false;

                return Equals((CategorizedRuleSet)obj);
            }

            /// <inheritdoc/>
            public Boolean Equals(CategorizedRuleSet other)
            {
                return
                    this.Selector == other.Selector &&
                    this.RuleSet == other.RuleSet &&
                    this.Index == other.Index;
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
            public Int32 Index
            {
                get;
            }
        }
    }
}
