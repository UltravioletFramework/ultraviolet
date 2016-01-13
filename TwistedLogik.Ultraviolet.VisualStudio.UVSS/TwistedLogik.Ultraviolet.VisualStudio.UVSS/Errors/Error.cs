using System;
using Microsoft.VisualStudio.Text;

namespace TwistedLogik.Ultraviolet.VisualStudio.Uvss.Errors
{
    /// <summary>
    /// Represents an error in a UVSS document.
    /// </summary>
    public sealed class Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        /// <param name="span">The snapshot span that contains the error.</param>
        /// <param name="message">The error message.</param>
        /// <param name="file">The path to the file that contains the error.</param>
        public Error(SnapshotSpan span, String message, String file)
        {
            this.Span = span;
            this.Message = message;
            this.File = file;
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
                Span.TranslateTo(targetSnapshot, SpanTrackingMode.EdgeExclusive),
                Message,
                File);
        }

        /// <summary>
        /// Gets the snapshot span that contains the error.
        /// </summary>
        public SnapshotSpan Span { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public String Message { get; }

        /// <summary>
        /// Gets the path to the file that contains the error.
        /// </summary>
        public String File { get; }
    }
}
