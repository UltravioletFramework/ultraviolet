using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.VisualStudio.Uvss.Tagging
{
    /// <summary>
    /// Represents a method which is invoked when <see cref="SyntaxErrorVisitor"/> marks
    /// a span of text as a syntax error.
    /// </summary>
    /// <param name="start">The index of the first character in the span.</param>
    /// <param name="width">The number of characters in the span.</param>
    /// <param name="message">The error message for the tag.</param>
    public delegate void SytnaxErrorTaggerAction(Int32 start, Int32 width, String message);

    /// <summary>
    /// Represents a syntax tree visitor which provides syntax error tag spans.
    /// </summary>
    public class SyntaxErrorVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxErrorVisitor"/> class.
        /// </summary>
        /// <param name="tagger">The action which is called when a span is tagged.</param>
        public SyntaxErrorVisitor(SytnaxErrorTaggerAction tagger)
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

            // TODO

            if (node is SyntaxToken)
                Visit(node.GetTrailingTrivia());
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

        // State values.
        private readonly SytnaxErrorTaggerAction tagger;
    }
}
