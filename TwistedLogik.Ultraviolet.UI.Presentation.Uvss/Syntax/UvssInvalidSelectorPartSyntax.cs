using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents an invalid UVSS selector part.
    /// </summary>
    public sealed class UvssInvalidSelectorPartSyntax : UvssSelectorPartBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssInvalidSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssInvalidSelectorPartSyntax()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssInvalidSelectorPartSyntax"/> class.
        /// </summary>
        internal UvssInvalidSelectorPartSyntax(
            SyntaxList<SyntaxToken> components)
            : base(SyntaxKind.InvalidSelectorPart)
        {
            this.Components = components;
            ChangeParent(components.Node);

            SlotCount = 1;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return Components.Node;
                default:
                    throw new InvalidOperationException();
            }
        }
        
        /// <summary>
        /// Gets the selector part's list of components.
        /// </summary>
        public SyntaxList<SyntaxToken> Components { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitInvalidSelectorPart(this);
        }
    }
}
