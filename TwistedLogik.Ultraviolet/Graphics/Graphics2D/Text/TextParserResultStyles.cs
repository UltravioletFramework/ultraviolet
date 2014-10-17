using System;
using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a collection of styles created by the formatted text parser.
    /// </summary>
    public sealed partial class TextParserResultStyles : IEnumerable<TextStyle>
    {
        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear()
        {
            storage.Clear();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public List<TextStyle>.Enumerator GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator<TextStyle> IEnumerable<TextStyle>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a style to the collection.
        /// </summary>
        /// <param name="style">The style to add to the collection.</param>
        internal void Add(TextStyle style)
        {
            storage.Add(style);
        }

        /// <summary>
        /// Gets the style at the specified index.
        /// </summary>
        /// <param name="ix">The index of the style to retrieve.</param>
        /// <returns>The style at the specified index.</returns>
        public TextStyle this[Int32 ix]
        {
            get { return storage[ix]; }
            internal set { storage[ix] = value; }
        }

        /// <summary>
        /// Gets the number of styles in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return storage.Count; }
        }

        // The backing storage for this collection.
        private readonly List<TextStyle> storage = new List<TextStyle>();
    }
}
