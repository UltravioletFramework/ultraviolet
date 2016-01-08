using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS universal selector part.
    /// </summary>
    public sealed class UvssUniversalSelectorPartSyntax : UvssSelectorPartBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssUniversalSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssUniversalSelectorPartSyntax()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssUniversalSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssUniversalSelectorPartSyntax(
            SyntaxToken asteriskToken,
            UvssPseudoClassSyntax pseudoClass)
            : base(SyntaxKind.SelectorPart)
        {
            this.AsteriskToken = asteriskToken;
            ChangeParent(asteriskToken);

            this.PseudoClass = pseudoClass;
            ChangeParent(pseudoClass);

            SlotCount = 2;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return AsteriskToken;
                case 1: return PseudoClass;
                default:
                    throw new InvalidOperationException();
            }
        }
        
        /// <summary>
        /// Gets the selector part's askterisk token.
        /// </summary>
        public SyntaxToken AsteriskToken { get; internal set; }

        /// <summary>
        /// Gets the selector part's pseudo-class.
        /// </summary>
        public UvssPseudoClassSyntax PseudoClass { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitUniversalSelectorPart(this);
        }
    }
}
