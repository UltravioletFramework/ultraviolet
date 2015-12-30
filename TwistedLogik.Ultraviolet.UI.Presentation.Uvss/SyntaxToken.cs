using System;
using System.Collections.Generic;
using System.IO;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents a terminal token.
    /// </summary>
    public class SyntaxToken : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxToken"/> class.
        /// </summary>
        /// <param name="kind">The syntax token's kind.</param>
        /// <param name="text">The syntax token's text.</param>
        /// <param name="leadingTrivia">The syntax token's leading trivia, if it has any.</param>
        /// <param name="trailingTrivia">The syntax token's trailing trivia, if it has any.</param>
        public SyntaxToken(SyntaxKind kind, String text, SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
            : base(kind, text.Length)
        {
            this.Text = text;

            if (leadingTrivia != null)
            {
                ExpandWidth(leadingTrivia);
                this.leadingTrivia = leadingTrivia;
            }

            if (trailingTrivia != null)
            {
                ExpandWidth(trailingTrivia);
                this.trailingTrivia = trailingTrivia;
            }
        }

        /// <inheritdoc/>
        public override String ToString() => Text;
        
        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public override Int32 GetLeadingTriviaWidth()
        {
            return leadingTrivia?.FullWidth ?? 0;
        }

        /// <inheritdoc/>
        public override Int32 GetTrailingTriviaWidth()
        {
            return trailingTrivia?.FullWidth ?? 0;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetLeadingTrivia()
        {
            return leadingTrivia;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetTrailingTrivia()
        {
            return trailingTrivia;
        }

        /// <inheritdoc/>
        public override Boolean IsToken => true;

        /// <summary>
        /// Gets the token's text.
        /// </summary>
        public String Text { get; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSyntaxToken(this);
        }

        /// <inheritdoc/>
        internal override void WriteToOrFlatten(TextWriter writer, Stack<SyntaxNode> stack)
        {
            var leadingTrivia = GetLeadingTrivia();
            if (leadingTrivia != null)
            {
                leadingTrivia.WriteTo(writer);
            }

            writer.Write(Text);

            var trailingTrivia = GetTrailingTrivia();
            if (trailingTrivia != null)
            {
                trailingTrivia.WriteTo(writer);
            }
        }

        // Token trivia.
        private SyntaxNode leadingTrivia;
        private SyntaxNode trailingTrivia;
    }
}
