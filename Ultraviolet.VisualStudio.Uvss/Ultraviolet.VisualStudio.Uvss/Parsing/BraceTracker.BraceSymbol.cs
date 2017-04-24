using System;
using Microsoft.VisualStudio.Text;

namespace Ultraviolet.VisualStudio.Uvss.Parsing
{
    partial class BraceTracker
    {        
        /// <summary>
        /// Represents a stored brace symbol (either "{" or "}").
        /// </summary>
        private class BraceSymbol
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BraceSymbol"/> class.
            /// </summary>
            /// <param name="snapshot">The text snapshot from which the symbol was retrieved.</param>
            /// <param name="type">The symbol's type.</param>
            /// <param name="position">The symbol's position in the source text.</param>
            public BraceSymbol(ITextSnapshot snapshot, BraceSymbolType type, Int32 position)
            {
                this.Snapshot = snapshot;
                this.Type = type;
                this.Position = position;
                this.Nesting = 0;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BraceSymbol"/> class.
            /// </summary>
            /// <param name="snapshot">The text snapshot from which the symbol was retrieved.</param>
            /// <param name="type">The symbol's type.</param>
            /// <param name="position">The symbol's position in the source text.</param>
            /// <param name="nesting">The symbol's nesting level.</param>
            public BraceSymbol(ITextSnapshot snapshot, BraceSymbolType type, Int32 position, Int32 nesting)
            {
                this.Snapshot = snapshot;
                this.Type = type;
                this.Position = position;
				this.Nesting = 0;
			}

            /// <summary>
            /// Translates the symbol to the specified snapshot.
            /// </summary>
            /// <param name="snapshot">The snapshot to which to translate the symbol.</param>
            public void TranslateTo(ITextSnapshot snapshot)
            {
                var oldSpan = new SnapshotSpan(Snapshot, Position, 1);
                var newSpan = oldSpan.TranslateTo(snapshot, SpanTrackingMode.EdgeExclusive);

                this.Snapshot = snapshot;
                this.Position = newSpan.Start;
            }

            /// <summary>
            /// Creates a clone of this object.
            /// </summary>
            /// <returns>The clone that was created.</returns>
            public BraceSymbol Clone()
            {
                return new BraceSymbol(Snapshot, Type, Position, Nesting);
            }

            /// <summary>
            /// Creates a clone of this symbol which has been translated to
            /// the specified text snapshot.
            /// </summary>
            /// <param name="snapshot">The snapshot to which to translate the symbol.</param>
            /// <returns>The clone that was created.</returns>
            public BraceSymbol TranslatedClone(ITextSnapshot snapshot)
            {
                var clone = Clone();
                clone.TranslateTo(snapshot);
                return clone;
            }

            /// <summary>
            /// Gets the text snapshot from which the symbol was retrieved.
            /// </summary>
            public ITextSnapshot Snapshot { get; internal set; }

            /// <summary>
            /// Gets the symbol's type.
            /// </summary>
            public BraceSymbolType Type { get; }

            /// <summary>
            /// Gets the symbol's position in the source text.
            /// </summary>
            public Int32 Position { get; set; }

            /// <summary>
            /// Gets the symbol's nesting level.
            /// </summary>
			public Int32 Nesting { get; set; }
        }
    }
}
