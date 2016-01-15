using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base type for UVSS selectors.
    /// </summary>
    public abstract class UvssSelectorBaseSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssSelectorBaseSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        internal UvssSelectorBaseSyntax(SyntaxKind kind)
            : base(kind)
        {

        }

        /// <summary>
        /// Gets the selector's list of components.
        /// </summary>
        public abstract SyntaxList<SyntaxNode> Components { get; }

        /// <summary>
        /// Gets a collection containing the selector's parts.
        /// </summary>
        public abstract IEnumerable<UvssSelectorPartSyntax> Parts { get; }

        /// <summary>
        /// Gets a collection containing the selector's combinators.
        /// </summary>
        public abstract IEnumerable<SyntaxToken> Combinators { get; }
    }
}
