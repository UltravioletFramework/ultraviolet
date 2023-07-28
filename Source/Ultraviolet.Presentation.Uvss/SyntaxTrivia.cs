using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss
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
        public SyntaxTrivia(SyntaxKind kind)
            : base(kind)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxTrivia"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        public SyntaxTrivia(BinaryReader reader, Int32 version)
            : base(reader, version)
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
