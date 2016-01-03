using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS property value enclosed in curly braces.
    /// </summary>
    public sealed class UvssPropertyValueWithBracesSyntax : UvssPropertyValueBaseSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyValueWithBracesSyntax"/> class.
        /// </summary>
        internal UvssPropertyValueWithBracesSyntax(
            SyntaxToken openCurlyBraceToken,
            SyntaxToken contentToken,
            SyntaxToken closeCurlyBraceToken)
            : base(SyntaxKind.PropertyValueWithBraces)
        {
            this.OpenCurlyBrace = openCurlyBraceToken;
            ChangeParent(openCurlyBraceToken);

            this.ContentToken = contentToken;
            ChangeParent(contentToken);

            this.CloseCurlyBrace = closeCurlyBraceToken;
            ChangeParent(closeCurlyBraceToken);

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenCurlyBrace;
                case 1: return ContentToken;
                case 2: return CloseCurlyBrace;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the value's opening curly brace.
        /// </summary>
        public SyntaxToken OpenCurlyBrace { get; internal set; }

        /// <summary>
        /// Gets the value's content token.
        /// </summary>
        public SyntaxToken ContentToken { get; internal set; }

        /// <summary>
        /// Gets the value's closing curly brace.
        /// </summary>
        public SyntaxToken CloseCurlyBrace { get; internal set; }

        /// <inheritdoc/>
        public override String Value
        {
            get { return ContentToken?.Text; }
        }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitPropertyValueWithBraces(this);
        }
    }
}
