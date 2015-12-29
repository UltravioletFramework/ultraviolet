using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the base class for UVSS-language syntax nodes.
    /// </summary>
    public abstract class UvssNodeSyntax : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssNodeSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        public UvssNodeSyntax(SyntaxKind kind)
            : base(kind)
        {

        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
