using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Uvss.Syntax;

namespace Ultraviolet.Presentation.Uvss.Diagnostics
{
    /// <summary>
    /// Represents a diagnostic warning or error message attached to a syntax node.
    /// </summary>
    public sealed class DiagnosticInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticInfo"/> class.
        /// </summary>
        /// <param name="node">The syntax node with which the diagnostic is associated.</param>
        /// <param name="id">The diagnostic's identifier.</param>
        /// <param name="severity">The diagnostic's severity level.</param>
        /// <param name="location">The diagnostic's location relative to its node.</param>
        /// <param name="message">The diagnostic's message.</param>
        internal DiagnosticInfo(SyntaxNode node, DiagnosticID id, DiagnosticSeverity severity, TextSpan location, String message)
        {
            Contract.Require(node, nameof(node));

            this.start = location.Start;
            this.length = location.Length;

            this.Node = node;
            this.ID = id;
            this.Severity = severity;
            this.Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticInfo"/> class
        /// from the specified binary reader.
        /// </summary>
        internal DiagnosticInfo(SyntaxNode node, BinaryReader reader, Int32 version)
        {
            Contract.Require(node, nameof(node));
            Contract.Require(reader, nameof(reader));

            this.start = reader.ReadInt32();
            this.length = reader.ReadInt32();

            this.Node = node;
            this.ID = (DiagnosticID)reader.ReadByte();
            this.Severity = (DiagnosticSeverity)reader.ReadByte();
            this.Message = reader.ReadString();
        }

        /// <summary>
        /// Serializes the object to the specified stream.
        /// </summary>
        /// <param name="writer">A binary writer on the stream to which to serialize the object.</param>
        /// <param name="version">The file version of the data being produced.</param>
        public void Serialize(BinaryWriter writer, Int32 version)
        {
            Contract.Require(writer, nameof(writer));

            writer.Write(start);
            writer.Write(length);

            writer.Write((Byte)ID);
            writer.Write((Byte)Severity);
            writer.Write(Message);
        }

        /// <summary>
        /// Adds a diagnostic to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="diagnostic">The diagnostic to add to the collection.</param>
        internal static void Report(ref ICollection<DiagnosticInfo> collection, DiagnosticInfo diagnostic)
        {
            Contract.Require(diagnostic, nameof(diagnostic));

            if (collection == null)
                collection = new List<DiagnosticInfo>();

            collection.Add(diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an expected node was missing
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The missing node.</param>
        internal static void ReportMissingNode(ref ICollection<DiagnosticInfo> collection, SyntaxNode node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.MissingToken, DiagnosticSeverity.Error, span,
                $"{GetSyntaxKindFriendlyName(node.Kind)} expected");

            var nodeDiagnosticsArray = node.GetDiagnosticsArray();
            var nodeDiagnostics = (ICollection<DiagnosticInfo>)nodeDiagnosticsArray?.ToList();

            Report(ref nodeDiagnostics, diagnostic);

            node.SetDiagnostics(nodeDiagnostics);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an unexpected token was encountered in the body of a document
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="trivia">The unexpected node.</param>
        internal static void ReportUnexpectedTokenInDocumentContent(ref ICollection<DiagnosticInfo> collection,
            SkippedTokensTriviaSyntax trivia)
        {
            Contract.Require(trivia, nameof(trivia));

            var span = new TextSpan(0, trivia.Width);
            var diagnostic = new DiagnosticInfo(trivia, DiagnosticID.RuleSetOrStoryboardExpected, DiagnosticSeverity.Error, span,
                $"Rule set or storyboard expected");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an unexpected token was encountered in the body of a rule set
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="trivia">The unexpected node.</param>
        internal static void ReportUnexpectedTokenInRuleSetBody(ref ICollection<DiagnosticInfo> collection,
            SkippedTokensTriviaSyntax trivia)
        {
            Contract.Require(trivia, nameof(trivia));

            var span = new TextSpan(0, trivia.Width);
            var diagnostic = new DiagnosticInfo(trivia, DiagnosticID.RuleTransitionOrTriggerExpected, DiagnosticSeverity.Error, span,
                $"Rule, transition, or trigger expected");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an unexpected token was encountered in the body of an animation body
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="trivia">The unexpected node.</param>
        internal static void ReportUnexpectedTokenInEventTriggerArgumentList(ref ICollection<DiagnosticInfo> collection,
            SkippedTokensTriviaSyntax trivia)
        {
            Contract.Require(trivia, nameof(trivia));

            var span = new TextSpan(0, trivia.Width);
            var diagnostic = new DiagnosticInfo(trivia, DiagnosticID.EventTriggerArgumentExpected, DiagnosticSeverity.Error, span,
                $"Event trigger argument expected");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an unexpected token was encountered in the body of a trigger
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="trivia">The unexpected node.</param>
        internal static void ReportUnexpectedTokenInTriggerBody(ref ICollection<DiagnosticInfo> collection,
            SkippedTokensTriviaSyntax trivia)
        {
            Contract.Require(trivia, nameof(trivia));

            var span = new TextSpan(0, trivia.Width);
            var diagnostic = new DiagnosticInfo(trivia, DiagnosticID.TriggerActionExpected, DiagnosticSeverity.Error, span,
                $"Trigger action expected");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an unexpected token was encountered in the body of a storyboard
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="trivia">The unexpected node.</param>
        internal static void ReportUnexpectedTokenInStoryboardBody(ref ICollection<DiagnosticInfo> collection,
            SkippedTokensTriviaSyntax trivia)
        {
            Contract.Require(trivia, nameof(trivia));

            var span = new TextSpan(0, trivia.Width);
            var diagnostic = new DiagnosticInfo(trivia, DiagnosticID.StoryboardTargetExpected, DiagnosticSeverity.Error, span,
                $"Storyboard target declaration expected");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an unexpected token was encountered in the body of a storyboard target
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="trivia">The unexpected node.</param>
        internal static void ReportUnexpectedTokenInStoryboardTargetBody(ref ICollection<DiagnosticInfo> collection,
            SkippedTokensTriviaSyntax trivia)
        {
            Contract.Require(trivia, nameof(trivia));

            var span = new TextSpan(0, trivia.Width);
            var diagnostic = new DiagnosticInfo(trivia, DiagnosticID.AnimationExpected, DiagnosticSeverity.Error, span,
                $"Animation declaration expected");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an unexpected token was encountered in the body of an animation body
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="trivia">The unexpected node.</param>
        internal static void ReportUnexpectedTokenInAnimationBody(ref ICollection<DiagnosticInfo> collection,
            SkippedTokensTriviaSyntax trivia)
        {
            Contract.Require(trivia, nameof(trivia));

            var span = new TextSpan(0, trivia.Width);
            var diagnostic = new DiagnosticInfo(trivia, DiagnosticID.AnimationKeyframeExpected, DiagnosticSeverity.Error, span,
                $"Animation keyframe declaration expected");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an event trigger has an incomplete argument list
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportEventTriggerHasTooFewArguments(ref ICollection<DiagnosticInfo> collection,
            SyntaxNode node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.EventTriggerHasTooFewArguments,
                DiagnosticSeverity.Error, span, "Event trigger argument lists must contain at least one argument");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an event trigger has duplicate arguments in its argument list
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        /// <param name="duplicateTokenText">The text of the duplicated token.</param>
        internal static void ReportEventTriggerHasDuplicateArguments(ref ICollection<DiagnosticInfo> collection,
            SyntaxNode node, String duplicateTokenText)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.EventTriggerHasDuplicateArguments,
                DiagnosticSeverity.Error, span, $"Event trigger has duplicate argument '{duplicateTokenText}'");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that a visual transition has an incomplete argument list
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportTransitionHasTooFewArguments(ref ICollection<DiagnosticInfo> collection,
            SyntaxNode node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.TransitionHasTooFewArguments,
                DiagnosticSeverity.Error, span, "Transition arguments must specify at least a visual state group and a destination state");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that a visual transition has too many arguments in its argument list
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportTransitionHasTooManyArguments(ref ICollection<DiagnosticInfo> collection,
            SyntaxNode node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.TransitionHasTooManyArguments,
                DiagnosticSeverity.Error, span, "Transition has too many arguments");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that a trigger is incomplete
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportIncompleteTrigger(ref ICollection<DiagnosticInfo> collection,
            UvssIncompleteTriggerSyntax node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(node.TriggerKeyword.Width, 0);
            var diagnostic = new DiagnosticInfo(node.TriggerKeyword, DiagnosticID.IncompleteTrigger,
                DiagnosticSeverity.Error, span, "Trigger type expected ('property' or 'event')");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that a property trigger condition is missing its comparison operator
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportPropertyTriggerMissingComparisonOperator(ref ICollection<DiagnosticInfo> collection,
            SyntaxToken node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.MissingToken, DiagnosticSeverity.Error, span,
                $"Comparison operator expected");

            var nodeDiagnosticsArray = node.GetDiagnosticsArray();
            var nodeDiagnostics = (ICollection<DiagnosticInfo>)nodeDiagnosticsArray?.ToList();

            Report(ref nodeDiagnostics, diagnostic);

            node.SetDiagnostics(nodeDiagnostics);
        }

        /// <summary>
        /// Adds a diagnostic indicating that a loop type is unrecognized
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportUnrecognizedLoopType(ref ICollection<DiagnosticInfo> collection,
            UvssIdentifierBaseSyntax node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.UnrecognizedLoopType,
                DiagnosticSeverity.Error, span, $"Unrecognized loop type '{node.Text}' (should be 'none' or 'loop' or 'reverse')");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an easing function is unrecognized
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportUnrecognizedEasingFunction(ref ICollection<DiagnosticInfo> collection,
            UvssIdentifierBaseSyntax node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.UnrecognizedEasingFunction,
                DiagnosticSeverity.Error, span, $"Unrecognized easing function '{node.Text}'");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that a selector is invalid
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportInvalidSelector(ref ICollection<DiagnosticInfo> collection,
            UvssSelectorBaseSyntax node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.InvalidSelector,
                DiagnosticSeverity.Error, span, $"Invalid selector");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that a selector part could not be completely parsed
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportInvalidSelectorPart(ref ICollection<DiagnosticInfo> collection,
            UvssInvalidSelectorPartSyntax node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.InvalidSelectorPart,
                DiagnosticSeverity.Error, span, $"Invalid selector part");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an index must be an integer value
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportIndexMustBeIntegerValue(ref ICollection<DiagnosticInfo> collection,
            SyntaxToken node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.IndexMustBeIntegerValue,
                DiagnosticSeverity.Error, span, $"Index must be integer value");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that a directive is not recognized
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportUnknownDirective(ref ICollection<DiagnosticInfo> collection,
            UvssDirectiveSyntax node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.UnknownDirective,
                DiagnosticSeverity.Error, span, $"Unrecognized directive");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that a directive was encountered at an invalid position
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportDirectiveAtInvalidPosition(ref ICollection<DiagnosticInfo> collection,
            UvssDirectiveSyntax node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.DirectiveAtInvalidPosition,
                DiagnosticSeverity.Error, span, $"Directives are only valid at the start of a document");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that a directive was encountered at an invalid position
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportDirectiveMustBeFirstNonWhiteSpaceOnLine(ref ICollection<DiagnosticInfo> collection,
            UvssDirectiveSyntax node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.DirectiveMustBeFirstNonWhiteSpaceOnLine,
                DiagnosticSeverity.Error, span, $"Directive must appear as the first non-whitespace character on a line");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that a $culture directive references an invalid culture
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportUnknownCulture(ref ICollection<DiagnosticInfo> collection,
            SyntaxNode node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.Width);
            var diagnostic = new DiagnosticInfo(node, DiagnosticID.UnknownCulture,
                DiagnosticSeverity.Error, span, $"Unrecognized culture");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Gets the syntax node associated with the diagnostic information..
        /// </summary>
        public SyntaxNode Node { get; }

        /// <summary>
        /// Gets the diagnostic's identifier.
        /// </summary>
        public DiagnosticID ID { get; }

        /// <summary>
        /// Gets the diagnostic's severity level.
        /// </summary>
        public DiagnosticSeverity Severity { get; }

        /// <summary>
        /// Gets the diagnostic's location in the source text.
        /// </summary>
        public TextSpan Location => new TextSpan(Node.Position + Node.GetLeadingTriviaWidth() + start, length);

        /// <summary>
        /// Gets the diagnostic's associated message.
        /// </summary>
        public String Message { get; }

        /// <summary>
        /// Gets the friendly name that corresponds to the specified <see cref="SyntaxKind"/> value.
        /// </summary>
        private static String GetSyntaxKindFriendlyName(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.EndOfLineTrivia:
                    return "end of line";
                case SyntaxKind.SingleLineCommentTrivia:
                    return "single line comment";
                case SyntaxKind.MultiLineCommentTrivia:
                    return "multi-line comment";
                case SyntaxKind.WhitespaceTrivia:
                    return "white space";
                case SyntaxKind.PlayStoryboardKeyword:
                    return "'play-storyboard' keyword";
                case SyntaxKind.SetHandledKeyword:
                    return "'set-handled' keyword";
                case SyntaxKind.TransitionKeyword:
                    return "'transition' keyword";
                case SyntaxKind.ImportantKeyword:
                    return "'!important' keyword";
                case SyntaxKind.AnimationKeyword:
                    return "'animation' keyword";
                case SyntaxKind.PlaySfxKeyword:
                    return "'play-sfx' keyword";
                case SyntaxKind.PropertyKeyword:
                    return "'property' keyword";
                case SyntaxKind.KeyframeKeyword:
                    return "'keyframe' keyword";
                case SyntaxKind.TriggerKeyword:
                    return "'trigger' keyword";
                case SyntaxKind.HandledKeyword:
                    return "'handled' keyword";
                case SyntaxKind.TargetKeyword:
                    return "'target' keyword";
                case SyntaxKind.EventKeyword:
                    return "'event' keyword";
                case SyntaxKind.SetKeyword:
                    return "'set' keyword";
                case SyntaxKind.AsKeyword:
                    return "'as' keyword";
                case SyntaxKind.IdentifierToken:
                    return "Identifier";
                case SyntaxKind.NumberToken:
                    return "Number";
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
                case SyntaxKind.PropertyValueToken:
                    return "Property value";
                case SyntaxKind.EndOfFileToken:
                    return "End of file";
                case SyntaxKind.List:
                    return "List";
                case SyntaxKind.Block:
                    return "Block";
                case SyntaxKind.RuleSet:
                    return "Rule set";
                case SyntaxKind.Rule:
                    return "Rule";
                case SyntaxKind.Selector:
                case SyntaxKind.SelectorWithNavigationExpression:
                    return "Selector";
                case SyntaxKind.SelectorWithParentheses:
                    return "Parentheses-enclosed selector";
                case SyntaxKind.SelectorPart:
                    return "Selector part";
                case SyntaxKind.PseudoClass:
                    return "Pseudo-class";
                case SyntaxKind.PropertyName:
                    return "Property name";
                case SyntaxKind.PropertyValue:
                    return "Property value";
                case SyntaxKind.PropertyValueWithBraces:
                    return "Brace-enclosed property value";
                case SyntaxKind.EventName:
                    return "Event name";
                case SyntaxKind.IncompleteTrigger:
                    return "Incomplete trigger";
                case SyntaxKind.EventTrigger:
                    return "Event trigger";
                case SyntaxKind.EventTriggerArgumentList:
                    return "Event trigger argument list";
                case SyntaxKind.PropertyTrigger:
                    return "Property trigger";
                case SyntaxKind.PropertyTriggerCondition:
                    return "Property trigger condition";
                case SyntaxKind.PlayStoryboardTriggerAction:
                    return "play-storyboard trigger action";
                case SyntaxKind.PlaySfxTriggerAction:
                    return "play-sfx trigger action";
                case SyntaxKind.SetTriggerAction:
                    return "set trigger action";
                case SyntaxKind.Transition:
                    return "Transition declaration";
                case SyntaxKind.TransitionArgumentList:
                    return "Transition argument list";
                case SyntaxKind.Storyboard:
                    return "Storyboard declaration";
                case SyntaxKind.StoryboardTarget:
                    return "Storyboard target declaration";
                case SyntaxKind.Animation:
                    return "Animation declaration";
                case SyntaxKind.AnimationKeyframe:
                    return "Keyframe declaration";
                case SyntaxKind.NavigationExpression:
                    return "Navigation expression";
                case SyntaxKind.Identifier:
                    return "Identifier";
                case SyntaxKind.EscapedIdentifier:
                    return "Escaped identifier";
                case SyntaxKind.DirectiveToken:
                case SyntaxKind.UnknownDirective:
                case SyntaxKind.CultureDirective:
                    return "Directive";
            }

            return kind.ToString();
        }

        // The syntax node for which the diagnostic was created.
        private readonly Int32 start;
        private readonly Int32 length;
    }
}
