using System;
using System.Collections.Generic;
using System.Linq;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a list of styling rules in an Ultraviolet Style Sheet (UVSS) document.
    /// </summary>
    public sealed partial class UvssRuleCollection : IEnumerable<UvssRule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssRuleCollection"/> class.
        /// </summary>
        /// <param name="rules">A collection containing the styling rules to add to the list.</param>
        internal UvssRuleCollection(IEnumerable<UvssRule> rules)
        {
            this.rules = rules.ToList();
        }

        /// <inheritdoc/>
        public override String ToString() =>
            String.Join("; ", rules.Select(x => x.ToString()));

        // State values.
        private readonly List<UvssRule> rules;
    }
}
