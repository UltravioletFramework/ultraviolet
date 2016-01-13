using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Syntax;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Diagnostics
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

            this.node = node;
            this.start = location.Start;
            this.length = location.Length;

            this.ID = id;
            this.Severity = severity;
            this.Message = message;
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
        /// Adds a diagnostic indicating that an expected token was missing
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="token">The missing token.</param>
        internal static void ReportMissingToken(ref ICollection<DiagnosticInfo> collection, SyntaxToken token)
        {
            Contract.Require(token, nameof(token));

            var span = new TextSpan(0, token.Width);
            var diagnostic = new DiagnosticInfo(token, DiagnosticID.MissingToken, DiagnosticSeverity.Error, span,
                $"{token.Kind} expected");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an unexpected token was encountered
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="token">The unexpected token.</param>
        internal static void ReportUnexpectedToken(ref ICollection<DiagnosticInfo> collection, SyntaxToken token)
        {
            Contract.Require(token, nameof(token));

            var span = new TextSpan(0, token.Width);
            var diagnostic = new DiagnosticInfo(token, DiagnosticID.UnexpectedToken, DiagnosticSeverity.Error, span,
                $"unexpected {token.Kind}");

            Report(ref collection, diagnostic);
        }

        /// <summary>
        /// Adds a diagnostic indicating that an animation is missing its property name 
        /// to the specified collection of diagnostics.
        /// </summary>
        /// <param name="collection">The collection to which to add the diagnostic.</param>
        /// <param name="node">The syntax node which is associated with the diagnostic.</param>
        internal static void ReportAnimationMissingPropertyName(ref ICollection<DiagnosticInfo> collection,
            UvssAnimationSyntax node)
        {
            Contract.Require(node, nameof(node));

            var span = new TextSpan(0, node.AnimationKeyword.Width);
            var diagnostic = new DiagnosticInfo(node.AnimationKeyword, DiagnosticID.AnimationMissingPropertyName, 
                DiagnosticSeverity.Error, span, "Animation must specify the name of a property to animate");

            Report(ref collection, diagnostic);
        }

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
        public TextSpan Location => new TextSpan(node.Position + node.GetLeadingTriviaWidth() + start, length);
        
        /// <summary>
        /// Gets the diagnostic's associated message.
        /// </summary>
        public String Message { get; }

        // The syntax node for which the diagnostic was created.
        private readonly SyntaxNode node;
        private readonly Int32 start;
        private readonly Int32 length;
    }
}
