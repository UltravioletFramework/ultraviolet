using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents the argument list for a visual transition.
    /// </summary>
    public sealed class UvssTransitionArgumentListSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTransitionArgumentListSyntax"/> class.
        /// </summary>
        public UvssTransitionArgumentListSyntax(
            SyntaxToken openParenToken,
            SeparatedSyntaxList<SyntaxNode> argumentList,
            SyntaxToken closeParenToken)
            : base(SyntaxKind.TransitionArgumentList)
        {
            this.OpenParenToken = openParenToken;
            this.ArgumentList = argumentList;
            this.CloseParenToken = closeParenToken;

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenParenToken;
                case 1: return ArgumentList.Node;
                case 2: return CloseParenToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The open parenthesis that introduces the argument list.
        /// </summary>
        public SyntaxToken OpenParenToken;

        /// <summary>
        /// The argument list's arguments.
        /// </summary>
        public SeparatedSyntaxList<SyntaxNode> ArgumentList;

        /// <summary>
        /// The close parenthesis that terminates the argument list.
        /// </summary>
        public SyntaxToken CloseParenToken;
    }
}
