using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a collection of named cursors.
    /// </summary>
    public sealed class CursorCollection : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the CursorCollection class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        internal CursorCollection(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Gets the cursor with the specified name.
        /// </summary>
        /// <param name="name">The name of the cursor to retrieve.</param>
        /// <returns>The cursor with the specified name, or null if no such cursor exists.</returns>
        public Cursor this[String name]
        {
            get
            {
                Cursor cursor;
                cursors.TryGetValue(name, out cursor);
                return cursor;
            }
            internal set
            {
                Contract.RequireNotEmpty(name, "name");
                Contract.Require(value, "value");

                cursors[name] = value;
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            foreach (var cursor in cursors)
                cursor.Value.Dispose();

            cursors.Clear();

            base.Dispose(disposing);
        }

        // The cursor registry.
        private readonly Dictionary<String, Cursor> cursors = 
            new Dictionary<String, Cursor>();
    }
}
