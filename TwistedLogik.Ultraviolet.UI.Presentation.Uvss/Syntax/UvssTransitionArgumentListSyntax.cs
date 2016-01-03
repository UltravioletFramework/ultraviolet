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
            SeparatedSyntaxList<SyntaxNode> arguments,
            SyntaxToken closeParenToken)
            : base(SyntaxKind.TransitionArgumentList)
        {
            this.OpenParenToken = openParenToken;
            ChangeParent(openParenToken);

            this.Arguments = arguments;
            ChangeParent(arguments.Node);

            this.CloseParenToken = closeParenToken;
            ChangeParent(closeParenToken);

            SlotCount = 3;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return OpenParenToken;
                case 1: return Arguments.Node;
                case 2: return CloseParenToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the open parenthesis that introduces the argument list.
        /// </summary>
        public SyntaxToken OpenParenToken { get; internal set; }

        /// <summary>
        /// Gets the list's arguments.
        /// </summary>
        public SeparatedSyntaxList<SyntaxNode> Arguments { get; internal set; }

        /// <summary>
        /// Gets the close parenthesis that terminates the argument list.
        /// </summary>
        public SyntaxToken CloseParenToken { get; internal set; }

        /// <inheritdoc/>
        internal override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitTransitionArgumentList(this);
        }
    }
}
