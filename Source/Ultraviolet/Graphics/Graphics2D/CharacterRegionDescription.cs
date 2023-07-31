using System;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An internal representation of a <see cref="CharacterRegion"/> used during content processing.
    /// </summary>
    internal sealed class CharacterRegionDescription
    {
        /// <summary>
        /// Gets or sets the first character in the region.
        /// </summary>
        public Char Start { get; set; }

        /// <summary>
        /// Gets or sets the last character in the region.
        /// </summary>
        public Char End { get; set; }
    }
}
