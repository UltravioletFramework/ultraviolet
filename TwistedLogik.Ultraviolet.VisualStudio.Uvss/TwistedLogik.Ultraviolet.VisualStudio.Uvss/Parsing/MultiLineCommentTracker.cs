using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Ultraviolet.Core;

namespace Ultraviolet.VisualStudio.Uvss.Parsing
{
    /// <summary>
    /// Tracks the positions of multi-line comments in a UVSS document.
    /// </summary>
    public sealed partial class MultiLineCommentTracker : SymbolTrackerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineCommentTracker"/> class.
        /// </summary>
        /// <param name="buffer">The text buffer which is being tracked.</param>
        public MultiLineCommentTracker(ITextBuffer buffer)
            : base(buffer)
        {
            InitializeFromBuffer();
        }

        /// <inheritdoc/>
        public override void OnBufferChanged(Object sender, TextContentChangedEventArgs e)
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
                OnSpanInvalidated(new SnapshotSpan(e.After, span));
        }
        
        /// <summary>
        /// Gets the span of the comment at the specified position in the source text.
        /// </summary>
        /// <param name="position">The position in the source text to evaluate.</param>
        /// <returns>The span of the comment at the specified position in the source text,
        /// or null if there is no comment at the specified position.</returns>
        public SnapshotSpan? GetCommentSpanAtPosition(Int32 position)
        {
            var snapshot = Buffer.CurrentSnapshot;

            var open = GetNextSymbol(position, x => x.Type == CommentSymbolType.Open && x.Nesting == 0);
            if (open == null)
                return null;

            var close = GetCloseSymbol(open);
            if (close == null)
                return new SnapshotSpan(snapshot, open.Position, snapshot.Length - open.Position);

            return new SnapshotSpan(snapshot, open.Position, (close.Position + 2) - open.Position);
        }

        /// <summary>
        /// Gets a value indicating whether there is an open comment symbol at the
        /// specified position within the source text.
        /// </summary>
        /// <param name="position">The position in the source text to evaluate.</param>
        /// <returns>true if there is a open comment symbol at the 
        /// specified position; otherwise, false.</returns>
        public Boolean IsOpenCommentSymbolAtPosition(Int32 position)
        {
            CommentSymbol symbol;
            if (!symbols.TryGetValue(position, out symbol))
                return false;

            return symbol.Type == CommentSymbolType.Open && symbol.Nesting == 0;
        }

        /// <summary>
        /// Gets a value indicating whether there is a close comment symbol at the
        /// specified position within the source text.
        /// </summary>
        /// <param name="position">The position in the source text to evaluate.</param>
        /// <returns>true if there is a close comment symbol at the 
        /// specified position; otherwise, false.</returns>
        public Boolean IsCloseCommentSymbolAtPosition(Int32 position)
        {
            CommentSymbol symbol;
            if (!symbols.TryGetValue(position, out symbol))
                return false;

            return symbol.Type == CommentSymbolType.Close && symbol.Nesting == 0;
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

            UpdateNesting();
        }

        /// <summary>
        /// Adds any symbols which occur on the specified line of text.
        /// </summary>
        private void AddSymbolsOnLine(ITextSnapshot snapshot, Span span, String line)
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
        private void RemoveSymbolsOnLine(ITextSnapshot snapshot, Span span, String line)
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
            Contract.Require(open, nameof(open));

            if (open.Type != CommentSymbolType.Open)
                throw new ArgumentException(nameof(open));

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
        /// Gets the next symbol that occurs after the specified position in the source text.
        /// </summary>
        private CommentSymbol GetNextSymbol(Int32 position, Predicate<CommentSymbol> predicate)
        {
            for (int i = 0; i < symbols.Count; i++)
            {
                var symbol = symbols.Values[i];
                if (symbol.Position < position)
                    continue;

                if (predicate(symbol))
                    return symbol;
            }
            return null;
        }
     
        // Known comment symbols.
        private SortedList<Int32, CommentSymbol> symbols = new SortedList<Int32, CommentSymbol>();
        private SortedList<Int32, CommentSymbol> symbolsTemp = new SortedList<Int32, CommentSymbol>();
    }
}
