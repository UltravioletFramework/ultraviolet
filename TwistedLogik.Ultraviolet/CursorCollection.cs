using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a collection of named <see cref="Cursor"/> objects.
    /// </summary>
    public sealed class CursorCollection : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CursorCollection"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        internal CursorCollection(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Gets the <see cref="Cursor"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the <see cref="Cursor"/> to retrieve.</param>
        /// <returns>The <see cref="Cursor"/> with the specified name, or <c>null</c> if no such <see cref="Cursor"/> exists.</returns>
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
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
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
