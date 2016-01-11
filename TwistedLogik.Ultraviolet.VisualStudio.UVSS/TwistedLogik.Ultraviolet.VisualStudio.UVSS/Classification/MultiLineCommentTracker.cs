using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using System.Linq;

namespace TwistedLogik.Ultraviolet.VisualStudio.UVSS.Classification
{
    /// <summary>
    /// Tracks the positions of multi-line comments in a UVSS document.
    /// </summary>
    public sealed partial class MultiLineCommentTracker : ISymbolTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineCommentTracker"/> class.
        /// </summary>
        /// <param name="buffer">The text buffer which is being tracked.</param>
        public MultiLineCommentTracker(ITextBuffer buffer)
        {
            Contract.Require(buffer, nameof(buffer));

            lock (buffer)
            {
                this.Buffer = buffer;
                this.Buffer.Changed += Buffer_Changed;

                InitializeFromBuffer();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified position in the source
        /// text is inside of a comment.
        /// </summary>
        /// <param name="position">The source text position to evaluate.</param>
        /// <returns>true if the position is inside of a comment; otherwise, false.</returns>
        public Boolean IsPositionInsideComment(Int32 position)
        {
            var openSymbolFound = false;
            var openSymbolPos = 0;

            var closeSymbolFound = false;
            var closeSymbolPos = 0;

            for (int i = 0; i < symbols.Count; i++)
            {
                var symbol = symbols.Values[i];
                if (symbol.Nesting > 0)
                    continue;

                if (symbol.Position >= position)
                    break;

                if (symbol.Type == CommentSymbolType.Open)
                {
                    openSymbolFound = true;
                    openSymbolPos = symbol.Position;
                }
                else
                {
                    closeSymbolFound = true;
                    closeSymbolPos = symbol.Position;
                }
            }

            if (!openSymbolFound)
                return false;

            if (!closeSymbolFound)
                return true;

            return openSymbolPos > closeSymbolPos;
        }

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
        private static String DiscardSingleLineComments(String line)
        {
            var ix = line.IndexOf("//");
            if (ix < 0)
                return line;

            return line.Substring(0, ix);
        }

        /// <summary>
        /// Initializes the tracker from the current state of its buffer.
        /// </summary>
        private void InitializeFromBuffer()
        {
            var snapshot = Buffer.CurrentSnapshot;

            for (int i = 0; i < snapshot.LineCount; i++)
            {
                var line = snapshot.GetLineFromLineNumber(i);
                var lineStart = line.Extent.Start;
                var lineText = DiscardSingleLineComments(line.GetText());

                for (int j = 0; j < lineText.Length - 1; j++)
                {
                    var abspos = lineStart + j;

                    if (lineText[j] == '/' && lineText[j + 1] == '*')
                    {
                        symbols.Add(abspos, new CommentSymbol(snapshot, CommentSymbolType.Open, abspos));
                        continue;
                    }

                    if (lineText[j] == '*' && lineText[j + 1] == '/')
                    {
                        symbols.Add(abspos, new CommentSymbol(snapshot, CommentSymbolType.Close, abspos));
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Adds any symbols which occur on the specified line of text.
        /// </summary>
        private void AddSymbolsOnLine(
            ITextSnapshot snapshot, Span span, String line)
        {
            if (String.IsNullOrEmpty(line))
                return;
            
            for (int i = 0; i < line.Length - 1; i++)
            {
                var abspos = span.Start + i;

                if (line[i] == '/' && line[i + 1] == '*')
                {
                    symbols.Add(abspos, new CommentSymbol(snapshot, CommentSymbolType.Open, abspos));
                    i++;
                    continue;
                }

                if (line[i] == '*' && line[i + 1] == '/')
                {
                    symbols.Add(abspos, new CommentSymbol(snapshot, CommentSymbolType.Close, abspos));
                    i++;
                    continue;
                }
            }
        }

        /// <summary>
        /// Removes any symbols which occur on the specified line of text.
        /// </summary>
        private void RemoveSymbolsOnLine(
            ITextSnapshot snapshot, Span span, String line)
        {
            if (String.IsNullOrEmpty(line))
                return;
            
            for (int i = 0; i < line.Length - 1; i++)
            {
                var abspos = span.Start + i;

                if ((line[i] == '/' && line[i + 1] == '*') ||
                    (line[i] == '*' && line[i + 1] == '/'))
                {
                    symbols.Remove(abspos);
                    i++;
                }
            }
        }
        
        /// <summary>
        /// Updates the nesting values of known symbols.
        /// </summary>
        private void UpdateNesting()
        {
            var nesting = 0;

            for (int i = 0; i < symbols.Count; i++)
            {
                var symbol = symbols.Values[i];
                if (symbol.Type == CommentSymbolType.Open)
                {
                    if (nesting < 0)
                        nesting = 0;

                    symbol.Nesting = nesting;
                    nesting++;
                }
                else
                {
                    if (nesting > 1)
                        nesting = 1;

                    nesting--;
                    symbol.Nesting = nesting;
                }
            }
        }

        /// <summary>
        /// Gets a collection of spans representing the multi-line comments in the specified text snapshot.
        /// </summary>
        private IEnumerable<SnapshotSpan> GetCommentSpans(ITextSnapshot snapshot)
        {
            var spans = new List<SnapshotSpan>();

            for (int i = 0; i < symbols.Count; i++)
            {
                var open = symbols.Values[i];
                if (open.Type == CommentSymbolType.Open && open.Nesting == 0)
                {
                    var close = GetCloseSymbol(open);
                    var closeIndex = (close == null) ? symbols.Count : symbols.IndexOfKey(close.Position);

                    var commentStart = open.Position;
                    var commentEnd = (close?.Position + 2) ?? snapshot.Length;
                    var commentLength = commentEnd - commentStart;
                    var commentSpan = new SnapshotSpan(snapshot, commentStart, commentLength);

                    spans.Add(commentSpan);

                    i = closeIndex;
                }
            }

            return spans;
        }

        /// <summary>
        /// Gets the open symbol which corresponds to the specified close symbol.
        /// </summary>
        private CommentSymbol GetOpenSymbol(CommentSymbol close)
        {
            Contract.Require(close, nameof(close));

            if (close.Type != CommentSymbolType.Close)
                throw new ArgumentException(nameof(close));

            if (close.Nesting < 0)
                return null;

            var index = symbols.IndexOfKey(close.Position);

            for (int i = index - 1; i >= 0; i--)
            {
                var symbol = symbols.Values[i];
                if (symbol.Type == CommentSymbolType.Open && symbol.Nesting == 0)
                {
                    return symbol;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the close symbol which corresponds to the specified open symbol.
        /// </summary>
        private CommentSymbol GetCloseSymbol(CommentSymbol open)
        {
            Contract.Require(open, "open");

            if (open.Type != CommentSymbolType.Open)
                throw new ArgumentException("open");

            if (open.Nesting > 0)
                return null;

            var index = symbols.IndexOfKey(open.Position);

            for (int i = index + 1; i < symbols.Count; i++)
            {
                var symbol = symbols.Values[i];
                if (symbol.Type == CommentSymbolType.Close && symbol.Nesting == 0)
                {
                    return symbol;
                }
            }

            return null;
        }

        /// <summary>
        /// Raises the <see cref="SpanInvalidated"/> event.
        /// </summary>
        private void RaiseSpanInvalidated(SnapshotSpan span)
        {
            var temp = SpanInvalidated;
            if (temp != null)
            {
                temp(this, span);
            }
        }

        /// <summary>
        /// Called when the tracked buffer is changed.
        /// </summary>
        private void Buffer_Changed(Object sender, TextContentChangedEventArgs e)
        {
            var invalidatedCommentSpans = new List<Span>();

            // Determine which comment spans from the old snapshot were potentially invalidated.
            var oldSpans = GetCommentSpans(e.Before);
            foreach (var oldSpan in oldSpans)
            {
                var intersections = e.Changes.Any(x => x.OldSpan.IntersectsWith(oldSpan.Span));
                if (intersections)
                {
                    var translated = oldSpan.TranslateTo(e.After, SpanTrackingMode.EdgeExclusive);
                    invalidatedCommentSpans.Add(translated.Span);
                }
            }

            // Remove symbols from lines which were touched.
            foreach (var change in e.Changes)
            {
                var oldLineStart = e.Before.GetLineFromPosition(change.OldPosition).LineNumber;
                var oldLineEnd = e.Before.GetLineFromPosition(change.OldEnd).LineNumber;
                for (int i = oldLineStart; i <= oldLineEnd; i++)
                {
                    var line = e.Before.GetLineFromLineNumber(i);
                    var lineText = DiscardSingleLineComments(line.GetText());
                    RemoveSymbolsOnLine(e.Before, line.Extent.Span, lineText);
                }
            }

            // Translate all of our symbols forward in time.
            foreach (var symbol in symbols)
            {
                symbol.Value.TranslateTo(e.After);
                symbolsTemp.Add(symbol.Value.Position, symbol.Value);
            }

            var temp = symbols;
            symbols = symbolsTemp;
            symbolsTemp = temp;
            symbolsTemp.Clear();

            // Add symbols from lines which were touched.
            foreach (var change in e.Changes)
            {
                var newLineStart = e.After.GetLineFromPosition(change.NewPosition).LineNumber;
                var newLineEnd = e.After.GetLineFromPosition(change.NewEnd).LineNumber;
                for (int i = newLineStart; i <= newLineEnd; i++)
                {
                    var line = e.After.GetLineFromLineNumber(i);
                    var lineText = DiscardSingleLineComments(line.GetText());
                    AddSymbolsOnLine(e.After, line.Extent.Span, lineText);
                }
            }            
            UpdateNesting();

            // Determine which comment spans in the new snapshot are potentially invalid.
            var newSpans = GetCommentSpans(e.After);
            foreach (var newSpan in newSpans)
            {
                var intersections = e.Changes.Any(x => x.NewSpan.IntersectsWith(newSpan.Span));
                if (intersections)
                {
                    invalidatedCommentSpans.Add(newSpan.Span);
                }
            }

            // Invalidate spans.
            foreach (var span in invalidatedCommentSpans)
                RaiseSpanInvalidated(new SnapshotSpan(e.After, span));
        }        

        // Known comment symbols.
        private SortedList<Int32, CommentSymbol> symbols = new SortedList<Int32, CommentSymbol>();
        private SortedList<Int32, CommentSymbol> symbolsTemp = new SortedList<Int32, CommentSymbol>();
    }
}
