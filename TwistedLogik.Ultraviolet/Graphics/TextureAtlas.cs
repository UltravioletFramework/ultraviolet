using System;
using System.Collections;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a collection of images that have been composited onto a single texture.
    /// </summary>
    public sealed partial class TextureAtlas : UltravioletResource, IEnumerable<KeyValuePair<String, Rectangle>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAtlas"/> class.
        /// </summary>
        /// <param name="texture">The atlas' texture.</param>
        /// <param name="cells">The atlas' cells.</param>
        internal TextureAtlas(Texture2D texture, IEnumerable<KeyValuePair<String, Rectangle>> cells)
            : base((texture == null) ? null : texture.Ultraviolet)
        {
            Contract.Require(texture, "texture");
            Contract.Require(cells, "cells");

            this.texture = texture;
            this.cells = new Dictionary<string, Rectangle>();
            foreach (var cell in cells)
            {
                this.cells[cell.Key] = cell.Value;
            }
        }

        /// <summary>
        /// Implicitly converts a <see cref="TextureAtlas"/> to its underlying <see cref="Texture2D"/>.
        /// </summary>
        /// <param name="atlas">The <see cref="TextureAtlas"/> to convert.</param>
        /// <returns>The underlying <see cref="Texture2D"/> represented by <paramref name="atlas"/>.</returns>
        public static implicit operator Texture2D(TextureAtlas atlas)
        {
            return atlas.texture;
        }

        /// <summary>
        /// Gets a value indicating whether the texture atlas contains a cell with the specified name.
        /// </summary>
        /// <param name="cell">The name of the cell to evaluate.</param>
        /// <returns><c>true</c> if the texture atlas contains a cell with the specified name; otherwise, <c>false</c>.</returns>
        public Boolean ContainsCell(String cell)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return cells.ContainsKey(cell);
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
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator<KeyValuePair<String, Rectangle>> IEnumerable<KeyValuePair<String, Rectangle>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public Dictionary<String, Rectangle>.Enumerator GetEnumerator()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return cells.GetEnumerator();
        }

        /// <summary>
        /// Gets the location of the cell with the specified name.
        /// </summary>
        /// <param name="cell">The name of the cell for which to retrieve a location.</param>
        /// <returns>The location of the cell with the specified name.</returns>
        public Rectangle this[String cell]
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed); 
                
                return cells[cell];
            }
        }

        /// <summary>
        /// Gets the texture atlas' width in pixels.
        /// </summary>
        public Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed); 
                
                return texture.Width;
            }
        }

        /// <summary>
        /// Gets the texture atlas' height in pixels.
        /// </summary>
        public Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed); 
                
                return texture.Height;
            }
        }
        
        /// <summary>
        /// Gets the number of cells on the texture atlas.
        /// </summary>
        public Int32 CellCount
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed); 
                
                return cells.Count;
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(texture);
            }
            base.Dispose(disposing);
        }

        // Property values.
        private readonly Texture2D texture;

        // State values.
        private readonly Dictionary<String, Rectangle> cells;
    }
}
