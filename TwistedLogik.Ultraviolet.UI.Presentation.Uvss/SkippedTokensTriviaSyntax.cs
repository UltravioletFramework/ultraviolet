using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss
{
    /// <summary>
    /// Represents a token that could not be parsed.
    /// </summary>
    public sealed class SkippedTokensTriviaSyntax : StructuredTriviaSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkippedTokensTriviaSyntax"/> class.
        /// </summary>
        /// <param name="tokens">The node that represents the skipped tokens.</param>
        public SkippedTokensTriviaSyntax(
            SyntaxNode tokens)
            : base(SyntaxKind.SkippedTokensTrivia)
        {
            this.Tokens = tokens;
            ChangeParent(tokens);

            this.SlotCount = 1;
            UpdateIsMissing();
        }

        /// <summary>
        /// Gets the node that represents the skipped tokens.
        /// </summary>
        public SyntaxNode Tokens { get; internal set; }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            if (index == 0)
                return Tokens;

            throw new InvalidOperationException();
        }        
    }
}
