using System;
using Microsoft.VisualStudio.Text;
using Ultraviolet.Core;

namespace Ultraviolet.VisualStudio.Uvss.Parsing
{
    /// <summary>
    /// Represents a wrapper around a text buffer which tracks various parameters
    /// required by the UVSS document parser.
    /// </summary>
    public sealed class UvssTextBuffer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssTextBuffer"/> class.
        /// </summary>
        /// <param name="buffer">The text buffer which is wrapped by this object./</param>
        private UvssTextBuffer(ITextBuffer buffer)
        {
            Contract.Require(buffer, nameof(buffer));

            this.multiLineCommentTracker = new MultiLineCommentTracker(buffer);
            this.braceTracker = new BraceTracker(buffer, multiLineCommentTracker);

			this.Parser = new UvssTextParser(buffer);

			this.Buffer = buffer;
            this.Buffer.Changed += Buffer_Changed;
        }

        /// <summary>
        /// Gets the <see cref="UvssTextBuffer"/> instance associated with the specified text buffer.
        /// </summary>
        /// <param name="buffer">The text buffer for which to retrieve a <see cref="UvssTextBuffer"/> instance.</param>
        /// <returns>The <see cref="UvssTextBuffer"/> instance for the specified text buffer.</returns>
        public static UvssTextBuffer ForBuffer(ITextBuffer buffer)
        {
            lock (buffer)
            {
                return buffer.Properties.GetOrCreateSingletonProperty(typeof(UvssTextBuffer), () =>
                    new UvssTextBuffer(buffer));
            }
        }
		
        /// <summary>
        /// Gets the span of the comment at the specified position in the source text.
        /// </summary>
        /// <param name="position">The position in the source text to evaluate.</param>
        /// <returns>The span of the comment at the specified position in the source text,
        /// or null if there is no comment at the specified position.</returns>
        public SnapshotSpan? GetCommentSpanAtPosition(Int32 position)
        {
            return multiLineCommentTracker.GetCommentSpanAtPosition(position);
        }

        /// <summary>
        /// Gets a <see cref="SnapshotSpan"/> that represents the outermost block of text
        /// that contains the specified position in the source text.
        /// </summary>
        /// <param name="position">The position in the source text to evaluate.</param>
        /// <returns>A <see cref="SnapshotSpan"/> that represents the outermost block at 
        /// the specified position in the source text.</returns>
        public SnapshotSpan GetOutermostBlockSpan(Int32 position)
        {
            return braceTracker.GetOutermostBlockSpan(position);
        }

        /// <summary>
        /// Gets a <see cref="SnapshotSpan"/> that represents the outermost block of text
        /// that contains the specified span in the source text.
        /// </summary>
        /// <param name="span">The span in the source text to evaluate.</param>
        /// <returns>A <see cref="SnapshotSpan"/> that represents the outermost block at 
        /// the specified span in the source text.</returns>
        public SnapshotSpan GetOutermostBlockSpan(Span span)
        {
            return braceTracker.GetOutermostBlockSpan(span);
        }

		/// <summary>
		/// Gets the parser for this text buffer.
		/// </summary>
		public UvssTextParser Parser { get; }

        /// <summary>
        /// Gets thhe text buffer which is wrapped by this object.
        /// </summary>
        public ITextBuffer Buffer { get; }

        /// <summary>
        /// Occurs when a span of text which contains a comment is invalidated.
        /// </summary>
        public event SpanInvalidatedDelegate CommentSpanInvalidated
        {
            add { multiLineCommentTracker.SpanInvalidated += value; }
            remove { multiLineCommentTracker.SpanInvalidated -= value; }
        }

        /// <summary>
        /// Called when the wrapped text buffer's content is changed.
        /// </summary>
        private void Buffer_Changed(Object sender, TextContentChangedEventArgs e)
        {
            multiLineCommentTracker.OnBufferChanged(sender, e);
            braceTracker.OnBufferChanged(sender, e);
        }
		
        // Symbol trackers.
        private readonly MultiLineCommentTracker multiLineCommentTracker;
        private readonly BraceTracker braceTracker;
    }
}
