using System;
using Microsoft.VisualStudio.Text;

namespace Ultraviolet.VisualStudio.Uvss.Parsing
{
    /// <summary>
    /// Represents the method that is called when a symbol tracker invalidates a span of text.
    /// </summary>
    /// <param name="obj">The object that raised the event.</param>
    /// <param name="span">The span that was invalidated.</param>
    public delegate void SpanInvalidatedDelegate(Object obj, SnapshotSpan span);

    /// <summary>
    /// Represents an object which tracks the position of symbols in source text.
    /// </summary>
    public interface ISymbolTracker
    {
        /// <summary>
        /// Occurs when the symbol tracker invalidates a span of text.
        /// </summary>
        event SpanInvalidatedDelegate SpanInvalidated;
    }
}
