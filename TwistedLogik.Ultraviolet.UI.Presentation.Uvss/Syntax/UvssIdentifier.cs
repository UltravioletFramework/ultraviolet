using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS identifier token.
    /// </summary>
    public class UvssIdentifier : SyntaxToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssIdentifier"/> class.
        /// </summary>
        /// <param name="text">The identifier's text.</param>
        public UvssIdentifier(String text)
            : this(text, null, null)
        {

        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssIdentifier"/> class.
        /// </summary>
        /// <param name="text">The identifier's text.</param>
        /// <param name="leadingTrivia">The keyword's leading trivia.</param>
        /// <param name="trailingTrivia">The keyword's trailing trivia.</param>
        public UvssIdentifier(String text, SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
            : base(SyntaxKind.IdentifierToken, text, leadingTrivia, trailingTrivia)
        {

        }
    }
}
