using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a block of nodes enclosed by curly braces.
    /// </summary>
    public sealed class UvssBlockSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssBlockSyntax"/> class.
        /// </summary>
        public UvssBlockSyntax(SyntaxKind kind,
            SyntaxToken openCurlyBraceToken,
            SyntaxList<SyntaxNode> content,
            SyntaxToken closeCurlyBraceToken)
            : base(kind)
        {
            this.OpenCurlyBraceToken = openCurlyBraceToken;
            this.Content = content;
            this.CloseCurlyBraceToken = closeCurlyBraceToken;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenCurlyBraceToken;
                case 1: return Content.Node;
                case 2: return CloseCurlyBraceToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The open curly brace that introduces the block.
        /// </summary>
        public SyntaxToken OpenCurlyBraceToken;

        /// <summary>
        /// The list of nodes that make up the block's content.
        /// </summary>
        public SyntaxList<SyntaxNode> Content;

        /// <summary>
        /// The close curly brace that terminates the block.
        /// </summary>
        public SyntaxToken CloseCurlyBraceToken;
    }
}
