using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents structured trivia.
    /// </summary>
    public abstract class StructuredTriviaSyntax : SyntaxTrivia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructuredTriviaSyntax"/> class.
        /// </summary>
        /// <param name="kind">The node's <see cref="SyntaxKind"/> value.</param>
        public StructuredTriviaSyntax(SyntaxKind kind)
            : base(kind)
        {

        }

        /// <inheritdoc/>
        public sealed override Boolean HasStructure => true;
    }
}
