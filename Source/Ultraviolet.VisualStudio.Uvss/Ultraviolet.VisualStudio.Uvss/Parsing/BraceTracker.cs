using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Ultraviolet.Core;

namespace Ultraviolet.VisualStudio.Uvss.Parsing
{
    /// <summary>
    /// Tracks the positions of braces in a UVSS document.
    /// </summary>
    public sealed partial class BraceTracker : SymbolTrackerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BraceTracker"/> class.
        /// </summary>
        /// <param name="buffer">The text buffer which is being tracked.</param>
        /// <param name="commentTracker">The buffer's multi-line comment tracker.</param>
        public BraceTracker(ITextBuffer buffer, MultiLineCommentTracker commentTracker)
            : base(buffer)
        {
            Contract.Require(commentTracker, nameof(commentTracker));

            this.commentTracker = commentTracker;

            InitializeFromBuffer();
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
			var snapshot = Buffer.CurrentSnapshot;

			var start = GetStartOfOutermostBlock(position);
			var end = GetEndOfOutermostBlock(position);

			return new SnapshotSpan(snapshot, start, end - start);
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
			var snapshot = Buffer.CurrentSnapshot;

			var start = GetStartOfOutermostBlock(span.Start);
			var end = GetEndOfOutermostBlock(span.End);

			return new SnapshotSpan(snapshot, start, end - start);
		}

		/// <summary>
		/// Gets the starting position of the outermost block that contains
		/// the specified position in the source text.
		/// </summary>
		/// <param name="position">The position in the source text to evaluate.</param>
		/// <returns>The position in the source text at which the outermost block begins.</returns>
		public Int32 GetStartOfOutermostBlock(Int32 position)
        {
            var startPos = 0;
            var startSymbol = default(BraceSymbol);

            for (int i = 0; i < symbols.Count; i++)
            {
                var symbol = symbols.Values[i];
                if (symbol.Position > position)
                    break;
                
                if (commentTracker.IsPositionInsideComment(symbol.Position))
                    continue;

                if (symbol.Type == BraceSymbolType.Open && symbol.Nesting == 0)
                {
                    startPos = symbol.Position;
                    startSymbol = symbol;
                }
            }

            if (startSymbol != null)
            {
                var previousCloseSymbol = GetPreviousCloseSymbol(startSymbol);
                if (previousCloseSymbol != null)
                {
                    startPos = previousCloseSymbol.Position + 1;
                }
                else
                {
                    startPos = 0;
                }
            }

            return startPos;
        }

        /// <summary>
        /// Gets the ending position of the outermost block that contains
        /// the specified position in the source text.
        /// </summary>
        /// <param name="position">The position in the source text to evaluate.</param>
        /// <returns>The position in the source text at which the outermost block ends.</returns>
        public Int32 GetEndOfOutermostBlock(Int32 position)
        {
            var endPos = Buffer.CurrentSnapshot.Length;

            for (int i = 0; i < symbols.Count; i++)
            {
                var symbol = symbols.Values[i];
                if (symbol.Position < position)
                    continue;
                
                if (commentTracker.IsPositionInsideComment(symbol.Position))
                    continue;

				if (symbol.Type == BraceSymbolType.Close && symbol.Nesting == 0)
				{
					endPos = symbol.Position + 1;
					break;
				}
            }

            return endPos;
        }

		/// <summary>
		/// Gets a value indicating whether the document currently has mismatched braces.
		/// </summary>
		public Boolean AreBracesMismatched
		{
			get { return areBracesMismatched; }
		}

        /// <summary>
        /// Called when the contents of the text buffer change.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">A <see cref="TextContentChangedEventArgs"/> that contains the event data.</param>
        public override void OnBufferChanged(Object sender, TextContentChangedEventArgs e)
        {
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

                for (int j = 0; j < lineText.Length; j++)
                {
                    var abspos = lineStart + j;

                    if (lineText[j] == '{')
                    {
                        symbols.Add(abspos, new BraceSymbol(snapshot, BraceSymbolType.Open, abspos));
                        continue;
                    }

                    if (lineText[j] == '}')
                    {
                        symbols.Add(abspos, new BraceSymbol(snapshot, BraceSymbolType.Close, abspos));
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

            for (int i = 0; i < line.Length; i++)
            {
                var abspos = span.Start + i;

                if (line[i] == '{')
                {
                    symbols.Add(abspos, new BraceSymbol(snapshot, BraceSymbolType.Open, abspos));
                    continue;
                }

                if (line[i] == '}')
                {
                    symbols.Add(abspos, new BraceSymbol(snapshot, BraceSymbolType.Close, abspos));
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

            for (int i = 0; i < line.Length; i++)
            {
                var abspos = span.Start + i;

                if (line[i] == '{' || line[i] == '}')
                {
                    symbols.Remove(abspos);
                }
            }
        }

        /// <summary>
        /// Updates the nesting values of known symbols.
        /// </summary>
        private void UpdateNesting()
		{
			var nesting = 0;

			var countOpen = 0;
			var countClosed = 0;

			for (int i = 0; i < symbols.Count; i++)
			{
				var symbol = symbols.Values[i];
				if (commentTracker.IsPositionInsideComment(symbol.Position))
					continue;

				if (symbol.Type == BraceSymbolType.Open)
				{
					countOpen++;

					if (nesting < 0)
						nesting = 0;

					symbol.Nesting = nesting;
					nesting++;
				}
				else
				{
					countClosed++;

					if (nesting < 1)
						nesting = 1;

					nesting--;
					symbol.Nesting = nesting;
				}
			}

			areBracesMismatched = (countOpen != countClosed);
		}
		
        /// <summary>
        /// Gets the previous close symbol which occurs at the same nesting level as
        /// the specified open symbol.
        /// </summary>
        private BraceSymbol GetPreviousCloseSymbol(BraceSymbol open)
        {
            Contract.Require(open, nameof(open));

            var index = symbols.IndexOfKey(open.Position);

            for (int i = index - 1; i >= 0; i--)
            {
                var symbol = symbols.Values[i];

                if (commentTracker.IsPositionInsideComment(symbol.Position))
                    continue;

                if (symbol.Type == BraceSymbolType.Close && symbol.Nesting == open.Nesting)
                    return symbol;
            }

            return null;
        }

        /// <summary>
        /// Gets the open symbol which corresponds to the specified close symbol.
        /// </summary>
        private BraceSymbol GetOpenSymbol(BraceSymbol close)
        {
            Contract.Require(close, nameof(close));

            if (close.Type != BraceSymbolType.Close)
                throw new ArgumentException(nameof(close));

            if (close.Nesting < 0)
                return null;

            var index = symbols.IndexOfKey(close.Position);

            for (int i = index - 1; i >= 0; i--)
            {
                var symbol = symbols.Values[i];

                if (commentTracker.IsPositionInsideComment(symbol.Position))
                    continue;

                if (symbol.Type == BraceSymbolType.Open && symbol.Nesting == close.Nesting)
                    return symbol;
            }

            return null;
        }

        /// <summary>
        /// Gets the close symbol which corresponds to the specified open symbol.
        /// </summary>
        private BraceSymbol GetCloseSymbol(BraceSymbol open)
        {
            Contract.Require(open, nameof(open));

            if (open.Type != BraceSymbolType.Open)
                throw new ArgumentException(nameof(open));

            if (open.Nesting > 0)
                return null;

            var index = symbols.IndexOfKey(open.Position);

            for (int i = index + 1; i < symbols.Count; i++)
            {
                var symbol = symbols.Values[i];

                if (commentTracker.IsPositionInsideComment(symbol.Position))
                    continue;

                if (symbol.Type == BraceSymbolType.Close && symbol.Nesting == open.Nesting)
                    return symbol;
            }

            return null;
        }
                
        // The comment tracker for this buffer.
        private readonly MultiLineCommentTracker commentTracker;

        // Known brace symbols.
        private SortedList<Int32, BraceSymbol> symbols = new SortedList<Int32, BraceSymbol>();
        private SortedList<Int32, BraceSymbol> symbolsTemp = new SortedList<Int32, BraceSymbol>();
		private Boolean areBracesMismatched;
    }
}
