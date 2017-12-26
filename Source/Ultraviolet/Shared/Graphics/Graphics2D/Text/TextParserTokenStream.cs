using System;
using System.Collections.Generic;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the token stream produced by parsing formatted text.
    /// </summary>
    public sealed partial class TextParserTokenStream
    {
        /// <summary>
        /// Clears the token stream.
        /// </summary>
        public void Clear()
        {
            SourceText = StringSegment.Empty;
            ParserOptions = TextParserOptions.None;
            tokens.Clear();
        }

        /// <summary>
        /// Adds an item to the token stream.
        /// </summary>
        /// <param name="item">The item to add to the token stream.</param>
        internal void Add(TextParserToken item)
        {
            tokens.Add(item);
        }

        /// <summary>
        /// Removes the item at the specified index from the token stream.
        /// </summary>
        /// <param name="index">The index of the item to remove from the token stream.</param>
        internal void RemoveAt(Int32 index)
        {
            tokens.RemoveAt(index);
        }

        /// <summary>
        /// Removes the specified range of items from the token stream.
        /// </summary>
        /// <param name="index">The index of the first element to remove from the token stream.</param>
        /// <param name="count">The number of items to remove from the token stream.</param>
        internal void RemoveRange(Int32 index, Int32 count)
        {
            tokens.RemoveRange(index, count);
        }

        /// <summary>
        /// Inserts a range of elements into the token stream at the specified index.
        /// </summary>
        /// <param name="index">The index at which to begin inserting elements.</param>
        /// <param name="collection">The collection of elements to insert into the token stream.</param>
        internal void InsertRange(Int32 index, IEnumerable<TextParserToken> collection)
        {
            tokens.InsertRange(index, collection);
        }

        /// <summary>
        /// Gets the item at the specified index within the token stream.
        /// </summary>
        /// <param name="ix">The index of the token to retrieve.</param>
        /// <returns>The item at the specified index within the token stream.</returns>
        public TextParserToken this[Int32 ix]
        {
            get { return tokens[ix]; }
            internal set { tokens[ix] = value; }
        }

        /// <summary>
        /// Gets the number of items in the token stream.
        /// </summary>
        public Int32 Count
        {
            get { return tokens.Count; }
        }

        /// <summary>
        /// Gets the source text that was parsed.
        /// </summary>
        public StringSegment SourceText
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the parser options which were used to produce this token stream.
        /// </summary>
        public TextParserOptions ParserOptions
        {
            get;
            internal set;
        }

        // The backing storage for this result.
        private readonly List<TextParserToken> tokens = new List<TextParserToken>();        
    }
}
