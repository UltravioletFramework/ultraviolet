using System;
using Microsoft.VisualStudio.Text;
using Ultraviolet.Core;

namespace Ultraviolet.VisualStudio.Uvss.Parsing
{
    /// <summary>
    /// Represents the base class for implementations of the <see cref="ISymbolTracker"/> interface.
    /// </summary>
    public abstract class SymbolTrackerBase : ISymbolTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolTrackerBase"/> class.
        /// </summary>
        /// <param name="buffer">The text buffer which is being tracked.</param>
        public SymbolTrackerBase(ITextBuffer buffer)
        {
            Contract.Require(buffer, nameof(buffer));

            this.Buffer = buffer;
        }

        /// <summary>
        /// Called when the contents of the text buffer change.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">A <see cref="TextContentChangedEventArgs"/> that contains the event data.</param>
        public abstract void OnBufferChanged(Object sender, TextContentChangedEventArgs e);

        /// <summary>
        /// Gets the text buffer which is being tracked.
        /// </summary>
        public ITextBuffer Buffer { get; }

        /// <inheritdoc/>
        public event SpanInvalidatedDelegate SpanInvalidated;
        
        /// <summary>
        /// Removes any single-line comments from the end of the specified line of text.
        /// </summary>
        /// <param name="line">The line of text from which to remove comments.</param>
        /// <returns>A string which contains the specified line of text with single-line comments removed.</returns>
        protected static String DiscardSingleLineComments(String line)
        {
            var ix = line.IndexOf("//");
            if (ix < 0)
                return line;

            return line.Substring(0, ix);
        }

        /// <summary>
        /// Raises the <see cref="SpanInvalidated"/> event.
        /// </summary>
        protected virtual void OnSpanInvalidated(SnapshotSpan span)
        {
            var temp = SpanInvalidated;
            if (temp != null)
            {
                temp(this, span);
            }
        }

    }
}
