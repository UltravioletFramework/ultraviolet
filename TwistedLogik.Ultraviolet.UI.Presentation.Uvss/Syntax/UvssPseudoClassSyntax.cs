using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS pseudo-class.
    /// </summary>
    public sealed class UvssPseudoClassSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPseudoClassSyntax"/> class.
        /// </summary>
        internal UvssPseudoClassSyntax(
            SyntaxToken colonToken,
            UvssIdentifierBaseSyntax classNameIdentifier)
            : base(SyntaxKind.PseudoClass)
        {
            this.ColonToken = colonToken;
            ChangeParent(colonToken);

            this.ClassNameIdentifier = classNameIdentifier;
            ChangeParent(classNameIdentifier);

            SlotCount = 2;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return ColonToken;
                case 1: return ClassNameIdentifier;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the pseudo-class' leading colon.
        /// </summary>
        public SyntaxToken ColonToken { get; internal set; }

        /// <summary>
        /// Gets the pseudo-class' name identifier.
        /// </summary>
        public UvssIdentifierBaseSyntax ClassNameIdentifier { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPseudoClass(this);
        }
    }
}
