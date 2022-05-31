using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="IUltravioletDisplayInfo"/>.
    /// </summary>
    public sealed class DummyUltravioletDisplayInfo : IUltravioletDisplayInfo
    {
        /// <inheritdoc/>
        public IUltravioletDisplay this[Int32 ix]
        {
            get { throw new IndexOutOfRangeException(nameof(ix)); }
        }

        /// <inheritdoc/>
        public IUltravioletDisplay PrimaryDisplay
        {
            get { return null; }
        }

        /// <inheritdoc/>
        public Int32 Count
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>A <see cref="List{T}.Enumerator"/> that iterates through the collection.</returns>
        public List<IUltravioletDisplay>.Enumerator GetEnumerator()
        {
            return displays.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<IUltravioletDisplay> IEnumerable<IUltravioletDisplay>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // State values.
        private readonly List<IUltravioletDisplay> displays = new List<IUltravioletDisplay>();
    }
}
