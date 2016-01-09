using System;
using System.Collections.Generic;
using System.IO;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents UVSS trivia, such as a comment or white space.
    /// </summary>
    public abstract class SyntaxTrivia : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxTrivia"/> structure.
        /// </summary>
        /// <param name="kind">The trivia's <see cref="SyntaxKind"/> value.</param>
        /// <param name="fullWidth">The full width of the node, including any leading or trailing trivia,
        /// or -1 if the full width of the node is not yet known.</param>
        public SyntaxTrivia(SyntaxKind kind, Int32 fullWidth = -1)
            : base(kind)
        {

        }

        /// <summary>
        /// Gets a value indicating whether this trivia has structure.
        /// </summary>
        public abstract Boolean HasStructure { get; }

        /// <inheritdoc/>
        public override Int32 GetLeadingTriviaWidth() => 0;

        /// <inheritdoc/>
        public override Int32 GetTrailingTriviaWidth() => 0;

        /// <inheritdoc/>
        public override SyntaxNode GetLeadingTrivia() => null;

        /// <inheritdoc/>
        public override SyntaxNode GetTrailingTrivia() => null;

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitSyntaxTrivia(this);
        }

        /// <inheritdoc/>
        protected override void UpdateIsMissing()
        {

        }
    }
}
