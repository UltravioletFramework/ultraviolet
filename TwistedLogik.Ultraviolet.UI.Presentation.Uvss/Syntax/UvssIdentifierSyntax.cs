using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS identifier.
    /// </summary>
    public sealed class UvssIdentifierSyntax : UvssIdentifierBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEscapedIdentifierSyntax"/> class.
        /// </summary>
        internal UvssIdentifierSyntax()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEscapedIdentifierSyntax"/> class.
        /// </summary>
        internal UvssIdentifierSyntax(
            SyntaxToken identifierToken)
            : base(SyntaxKind.Identifier)
        {
            this.IdentifierToken = identifierToken;
            ChangeParent(identifierToken);
            
            SlotCount = 1;
            UpdateIsMissing();
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return IdentifierToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        public override String Text
        {
            get { return IdentifierToken?.Text; }
        }
        
        /// <summary>
        /// Gets the identifier which this node represents.
        /// </summary>
        public SyntaxToken IdentifierToken { get; internal set; }
        
        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitIdentifier(this);
        }
    }
}
