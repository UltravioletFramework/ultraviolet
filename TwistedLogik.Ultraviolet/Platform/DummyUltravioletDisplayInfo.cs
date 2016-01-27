using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="IUltravioletDisplayInfo"/>.
    /// </summary>
    public sealed class DummyUltravioletDisplayInfo : IUltravioletDisplayInfo
    {
        /// <inheritdoc/>
        public IUltravioletDisplay PrimaryDisplay
        {
            get { return null; }
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
