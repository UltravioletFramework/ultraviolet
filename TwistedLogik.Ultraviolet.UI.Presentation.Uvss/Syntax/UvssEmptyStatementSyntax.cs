using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents an empty statement.
    /// </summary>
    public sealed class UvssEmptyStatementSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssEmptyStatementSyntax"/> class.
        /// </summary>
        public UvssEmptyStatementSyntax(
            SyntaxToken emptyToken)
            : base(SyntaxKind.EmptyStatement)
        {
            this.EmptyToken = emptyToken;
            ChangeParent(emptyToken);

            this.SlotCount = 1;
            UpdateIsMissing();
        }

        /// <summary>
        /// Gets the empty statement's empty placeholder token.
        /// </summary>
        public SyntaxToken EmptyToken { get; internal set; }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            if (index == 0)
                return EmptyToken;

            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitEmptyStatement(this);
        }
    }
}
