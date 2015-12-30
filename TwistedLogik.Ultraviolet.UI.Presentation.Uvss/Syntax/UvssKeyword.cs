using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS keyword token.
    /// </summary>
    public class UvssKeyword : SyntaxToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssKeyword"/> class.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value that specifies the keyword's kind.</param>
        public UvssKeyword(SyntaxKind kind)
            : this(kind, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssKeyword"/> class.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value that specifies the keyword's kind.</param>
        /// <param name="leadingTrivia">The keyword's leading trivia.</param>
        /// <param name="trailingTrivia">The keyword's trailing trivia.</param>
        public UvssKeyword(SyntaxKind kind, SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
            : base(kind, GetKeywordText(kind), leadingTrivia, trailingTrivia)
        {

        }

        /// <summary>
        /// Gets the text associated with the specified keyword kind.
        /// </summary>
        /// <param name="kind">The kind of keyword for which to retrieve text.</param>
        /// <returns>The text associated with the specified keyword kind.</returns>
        private static String GetKeywordText(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlayStoryboardKeyword:
                    return "play-storyboard";
                case SyntaxKind.SetHandledKeyword:
                    return "set-handled";
                case SyntaxKind.TransitionKeyword:
                    return "transition";
                case SyntaxKind.ImportantKeyword:
                    return "!important";
                case SyntaxKind.AnimationKeyword:
                    return "animation";
                case SyntaxKind.PlaySfxKeyword:
                    return "play-sfx";
                case SyntaxKind.PropertyKeyword:
                    return "property";
                case SyntaxKind.KeyframeKeyword:
                    return "keyframe";
                case SyntaxKind.TriggerKeyword:
                    return "trigger";
                case SyntaxKind.HandledKeyword:
                    return "handled";
                case SyntaxKind.TargetKeyword:
                    return "target";
                case SyntaxKind.EventKeyword:
                    return "event";
                case SyntaxKind.SetKeyword:
                    return "set";
                case SyntaxKind.AsKeyword:
                    return "as";
            }
            throw new InvalidOperationException();
        }
    }
}
