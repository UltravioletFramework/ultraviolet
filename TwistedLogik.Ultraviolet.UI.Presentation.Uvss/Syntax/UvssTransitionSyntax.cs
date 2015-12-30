using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a visual transition.
    /// </summary>
    public sealed class UvssTransitionSyntax : UvssNodeSyntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTransitionSyntax"/> class.
        /// </summary>
        public UvssTransitionSyntax(
            SyntaxToken transitionKeyword,
            UvssTransitionArgumentListSyntax argumentList,
            SyntaxToken colonToken,
            SyntaxToken storyboardNameToken,
            SyntaxToken qualifierToken,
            SyntaxToken semiColonToken)
            : base(SyntaxKind.Transition)
        {
            this.TransitionKeyword = transitionKeyword;
            ChangeParent(transitionKeyword);

            this.ArgumentList = argumentList;
            ChangeParent(argumentList);

            this.ColonToken = colonToken;
            ChangeParent(colonToken);

            this.StoryboardNameToken = storyboardNameToken;
            ChangeParent(storyboardNameToken);

            this.QualifierToken = qualifierToken;
            ChangeParent(qualifierToken);

            this.SemiColonToken = semiColonToken;
            ChangeParent(semiColonToken);

            SlotCount = 6;
        }

        /// <inheritdoc/>
        public override SyntaxNode GetSlot(Int32 index)
        {
            switch (index)
            {
                case 0: return TransitionKeyword;
                case 1: return ArgumentList;
                case 2: return ColonToken;
                case 3: return StoryboardNameToken;
                case 4: return QualifierToken;
                case 5: return SemiColonToken;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// The transition's "transition" keyword.
        /// </summary>
        public SyntaxToken TransitionKeyword { get; internal set; }

        /// <summary>
        /// The transition's argument list.
        /// </summary>
        public UvssTransitionArgumentListSyntax ArgumentList { get; internal set; }

        /// <summary>
        /// The colon that separates the transition declaration from its value.
        /// </summary>
        public SyntaxToken ColonToken { get; internal set; }

        /// <summary>
        /// The storyboard name that is associated with the transition.
        /// </summary>
        public SyntaxToken StoryboardNameToken { get; internal set; }

        /// <summary>
        /// The transition's qualifier token.
        /// </summary>
        public SyntaxToken QualifierToken { get; internal set; }

        /// <summary>
        /// The semi-colon that terminates the transition.
        /// </summary>
        public SyntaxToken SemiColonToken { get; internal set; }
    }
}
