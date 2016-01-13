using System;
using Microsoft.VisualStudio.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss;

namespace TwistedLogik.Ultraviolet.VisualStudio.Uvss.Errors
{
    /// <summary>
    /// Represents a method which is invoked when <see cref="ErrorVisitor"/> marks
    /// a span of text as a syntax error.
    /// </summary>
    /// <param name="start">The index of the first character in the span.</param>
    /// <param name="width">The number of characters in the span.</param>
    /// <param name="message">The error message for the tag.</param>
    public delegate void ErrorVisitorAction(Int32 start, Int32 width, String message);

    /// <summary>
    /// Represents a syntax tree visitor which provides syntax error tag spans.
    /// </summary>
    public class ErrorVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorVisitor"/> class.
        /// </summary>
        /// <param name="tagger">The action which is called when a span is tagged.</param>
        public ErrorVisitor(ErrorVisitorAction tagger)
        {
            this.tagger = tagger;
        }

        /// <summary>
        /// Visits the specified syntax node.
        /// </summary>
        /// <param name="node">The syntax node to visit.</param>
        public void Visit(SyntaxNode node)
        {
            if (node == null)
                return;

            if (node is SyntaxToken)
                Visit(node.GetLeadingTrivia());
            
            if (node is SkippedTokensTriviaSyntax)
            {
                Tag(new Span(node.Position, node.FullWidth),
                    $"Invalid token '{node.ToFullString()}'");
            }
            else
            {
                var foundMissing = false;

                var errorPosition = node.Position + node.GetLeadingTriviaWidth();

                for (int i = 0; i < node.SlotCount; i++)
                {
                    var child = node.GetSlot(i);
                    if (child != null)
                    {
                        if (child.IsMissing && !foundMissing)
                        {
                            foundMissing = true;

                            var terminal = child.GetFirstTerminal();
                            Tag(new Span(errorPosition, 1),
                                $"{GetMissingNodeString(terminal)} expected");

                        }
                        else
                        {
                            Visit(child);
                        }

                        errorPosition = child.Position +
                            (child.FullWidth - child.GetTrailingTriviaWidth());
                    }
                }
            }

            if (node is SyntaxToken)
                Visit(node.GetTrailingTrivia());
        }

        /// <summary>
        /// Tags the specified span of text.
        /// </summary>
        /// <param name="span">The span of text to tag.</param>
        /// <param name="message">The error message for the tag.</param>
        private void Tag(Span span, String message)
        {
            tagger(span.Start, span.Length, message);
        }

        /// <summary>
        /// Tags the specified node.
        /// </summary>
        /// <param name="node">The node to tag.</param>
        /// <param name="message">The error message for the tag.</param>
        /// <param name="withLeadingTrivia">A value indicating whether to tag the node's leading trivia.</param>
        /// <param name="withTrailingTrivia">A value indicating whether to tag the node's trailing trivia.</param>
        private void Tag(SyntaxNode node, String message,
            Boolean withLeadingTrivia = false,
            Boolean withTrailingTrivia = false)
        {
            if (node == null)
                return;

            var start = node.Position + (withLeadingTrivia ? 0 : node.GetLeadingTriviaWidth());
            var width = node.FullWidth - (
                (withLeadingTrivia ? 0 : node.GetLeadingTriviaWidth()) +
                (withTrailingTrivia ? 0 : node.GetTrailingTriviaWidth()));
            
            tagger(start, width, message);
        }

        private static String GetMissingNodeString(SyntaxNode node)
        {
            switch (node.Kind)
            {
                case SyntaxKind.PlayStoryboardKeyword:
                    return "storyboard";
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
                case SyntaxKind.CommaToken:
                    return ",";
                case SyntaxKind.ColonToken:
                    return ":";
                case SyntaxKind.SemiColonToken:
                    return ";";
                case SyntaxKind.AtSignToken:
                    return "@";
                case SyntaxKind.HashToken:
                    return "#";
                case SyntaxKind.PeriodToken:
                    return ".";
                case SyntaxKind.ExclamationMarkToken:
                    return "!";
                case SyntaxKind.OpenParenthesesToken:
                    return "(";
                case SyntaxKind.CloseParenthesesToken:
                    return ")";
                case SyntaxKind.OpenCurlyBraceToken:
                    return "{";
                case SyntaxKind.CloseCurlyBraceToken:
                    return "}";
                case SyntaxKind.OpenBracketToken:
                    return "[";
                case SyntaxKind.CloseBracketToken:
                    return "]";
                case SyntaxKind.AsteriskToken:
                    return "*";
                case SyntaxKind.GreaterThanGreaterThanToken:
                    return ">>";
                case SyntaxKind.GreaterThanQuestionMarkToken:
                    return ">?";
                case SyntaxKind.SpaceToken:
                    return " ";
                case SyntaxKind.EqualsToken:
                    return "=";
                case SyntaxKind.NotEqualsToken:
                    return "<>";
                case SyntaxKind.LessThanToken:
                    return "<";
                case SyntaxKind.GreaterThanToken:
                    return ">";
                case SyntaxKind.LessThanEqualsToken:
                    return "<=";
                case SyntaxKind.GreaterThanEqualsToken:
                    return ">=";
                case SyntaxKind.PipeToken:
                    return "|";

                case SyntaxKind.IdentifierToken:
                    return "Identifier";

                case SyntaxKind.NumberToken:
                    return "Number";

                case SyntaxKind.PropertyValueToken:
                    return "Property value";
            }

            return node.Kind.ToString();
        }

        // State values.
        private readonly ErrorVisitorAction tagger;
    }
}
