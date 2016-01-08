using System;
using System.Collections.Generic;
using System.IO;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents UVSS trivia, such as a comment or white space.
    /// </summary>
    public partial class SyntaxTrivia : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxTrivia"/> structure.
        /// </summary>
        /// <param name="kind">The trivia's <see cref="SyntaxKind"/> value.</param>
        /// <param name="text">The trivia's text.</param>
        public SyntaxTrivia(SyntaxKind kind, String text)
            : base(kind, text.Length)
        {
            this.Text = text;
        }

        /// <inheritdoc/>
        public override String ToString() => Text;

        /// <inheritdoc/>
        public override String ToFullString() => Text;

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public override Int32 GetLeadingTriviaWidth() => 0;

        /// <inheritdoc/>
        public override Int32 GetTrailingTriviaWidth() => 0;

        /// <inheritdoc/>
        public override SyntaxNode GetLeadingTrivia() => null;

        /// <inheritdoc/>
        public override SyntaxNode GetTrailingTrivia() => null;

        /// <summary>
        /// Gets the trivia's text.
        /// </summary>
        public String Text { get; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSyntaxTrivia(this);
        }

        /// <inheritdoc/>
        internal override void WriteToOrFlatten(TextWriter writer, Stack<SyntaxNode> stack)
        {
            writer.Write(Text);
        }

        /// <inheritdoc/>
        protected override void UpdateIsMissing()
        {

        }
    }
}
