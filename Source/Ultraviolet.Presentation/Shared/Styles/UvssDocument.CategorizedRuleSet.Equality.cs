using System;

namespace Ultraviolet.Presentation.Styles
{
    partial class UvssDocument
    {
        partial struct CategorizedRuleSet
        {
            /// <inheritdoc/>
            public override Int32 GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + this.Selector?.GetHashCode() ?? 0;
                    hash = hash * 23 + this.RuleSet?.GetHashCode() ?? 0;
                    hash = hash * 23 + this.Index.GetHashCode();
                    return hash;
                }
            }

            /// <summary>
            /// Compares two objects to determine whether they are equal.
            /// </summary>
            /// <param name="v1">The first value to compare.</param>
            /// <param name="v2">The second value to compare.</param>
            /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
            public static Boolean operator ==(CategorizedRuleSet v1, CategorizedRuleSet v2)
            {
                return v1.Equals(v2);
            }

            /// <summary>
            /// Compares two objects to determine whether they are unequal.
            /// </summary>
            /// <param name="v1">The first value to compare.</param>
            /// <param name="v2">The second value to compare.</param>
            /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
            public static Boolean operator !=(CategorizedRuleSet v1, CategorizedRuleSet v2)
            {
                return !v1.Equals(v2);
            }

            /// <inheritdoc/>
            public override Boolean Equals(Object other)
            {
                return (other is CategorizedRuleSet x) ? Equals(x) : false;
            }

            /// <inheritdoc/>
            public Boolean Equals(CategorizedRuleSet other)
            {
                return
                    this.Selector == other.Selector &&
                    this.RuleSet == other.RuleSet &&
                    this.Index == other.Index;
            }
        }
    }
}