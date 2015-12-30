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
            SyntaxToken classNameToken)
            : base(SyntaxKind.PseudoClass)
        {
            this.ColonToken = colonToken;
            ChangeParent(colonToken);

            this.ClassNameToken = classNameToken;
            ChangeParent(classNameToken);

            SlotCount = 2;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return ColonToken;
                case 1: return ClassNameToken;
                default:
                    return null;
            }
        }

        /// <summary>
        /// The pseudo-class' leading colon.
        /// </summary>
        public SyntaxToken ColonToken { get; internal set; }

        /// <summary>
        /// The name of the pseudo-class.
        /// </summary>
        public SyntaxToken ClassNameToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPseudoClass(this);
        }
    }
}
