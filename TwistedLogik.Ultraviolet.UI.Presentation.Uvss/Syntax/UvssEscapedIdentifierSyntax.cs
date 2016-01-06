using System;
using System.Collections.Generic;
using System.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS identifier which has been escaped using square brackets.
    /// </summary>
    public sealed class UvssEscapedIdentifierSyntax : UvssIdentifierBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEscapedIdentifierSyntax"/> class.
        /// </summary>
        internal UvssEscapedIdentifierSyntax(
            SyntaxToken openBracketToken,
            SyntaxToken identifierToken,
            SyntaxToken closeBracketToken)
            : base(SyntaxKind.EscapedIdentifier)
        {
            this.OpenBracketToken = openBracketToken;
            ChangeParent(openBracketToken);

            this.IdentifierToken = identifierToken;
            ChangeParent(identifierToken);

            this.CloseBracketToken = closeBracketToken;
            ChangeParent(closeBracketToken);

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenBracketToken;
                case 1: return IdentifierToken;
                case 2: return CloseBracketToken;
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
        /// Gets the open bracket which introduces the escaped keyword.
        /// </summary>
        public SyntaxToken OpenBracketToken { get; internal set; }
        
        /// <summary>
        /// Gets the identifier which this node represents.
        /// </summary>
        public SyntaxToken IdentifierToken { get; internal set; }

        /// <summary>
        /// Gets the close bracket which terminates the escaped keyword.
        /// </summary>
        public SyntaxToken CloseBracketToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitEscapedIdentifier(this);
        }
    }
}
