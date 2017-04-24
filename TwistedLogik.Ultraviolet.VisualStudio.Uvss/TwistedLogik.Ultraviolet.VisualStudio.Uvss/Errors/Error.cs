using System;
using Microsoft.VisualStudio.Text;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Uvss.Diagnostics;

namespace Ultraviolet.VisualStudio.Uvss.Errors
{
    /// <summary>
    /// Represents an error in a UVSS document.
    /// </summary>
    public sealed class Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        /// <param name="file">The path to the file that contains the error.</param>
        /// <param name="span">The snapshot span that contains the error.</param>
        /// <param name="diagnosticInfo">The diagnostic information that this error represents.</param>
        internal Error(String file, SnapshotSpan span, DiagnosticInfo diagnosticInfo)
        {
            Contract.Require(file, nameof(file));
            Contract.Require(diagnosticInfo, nameof(diagnosticInfo));

            this.File = file;
            this.Span = span;
            this.DiagnosticInfo = diagnosticInfo;
        }

        /// <summary>
        /// Creates a copy of this error which has been translated to the specified snapshot.
        /// </summary>
        /// <param name="targetSnapshot">The snapshot to which to translate the error.</param>
        /// <returns>The <see cref="Error"/> instance which was created.</returns>
        public Error TranslateTo(ITextSnapshot targetSnapshot)
        {
            if (Span.Snapshot == targetSnapshot)
                return this;

            return new Error(
                File,
                Span.TranslateTo(targetSnapshot, SpanTrackingMode.EdgeExclusive),
                DiagnosticInfo);
        }

        /// <summary>
        /// Gets the path to the file that contains the error.
        /// </summary>
        public String File { get; }

        /// <summary>
        /// Gets the snapshot span that contains the error.
        /// </summary>
        public SnapshotSpan Span { get; }
        
        /// <summary>
        /// Gets the snapshot span that contains the error, adjusted to be safe
        /// for display as an error tag.
        /// </summary>
        public SnapshotSpan TagSafeSpan
        {
            get
            {
                var tagWidth = Span.Length;
                var tagStart = Span.Span.Start;

                if (DiagnosticInfo.ID == DiagnosticID.MissingToken)
                {
                    var prevToken = DiagnosticInfo.Node.GetPreviousToken(includeMissing: false);
                    if (prevToken.HasTrailingLineBreaks || DiagnosticInfo.Node.HasLeadingLineBreaks)
                    {
                        var positionDelta = prevToken.Position - DiagnosticInfo.Node.Position;

                        tagStart = (tagStart + positionDelta) + (prevToken.FullWidth - prevToken.GetTrailingTriviaWidth());
                        tagWidth = 1;
                    }
                }

                if (tagWidth == 0)
                    tagWidth = 1;

                if (tagStart + tagWidth > Span.Snapshot.Length)
                    tagStart = Span.Snapshot.Length - tagWidth;

                return new SnapshotSpan(Span.Snapshot, tagStart, tagWidth);
            }
        }

        /// <summary>
        /// Gets the diagnostic information that this error represents.
        /// </summary>
        public DiagnosticInfo DiagnosticInfo { get; }
    }
}
