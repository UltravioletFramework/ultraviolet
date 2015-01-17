using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    partial class Grid
    {
        /// <summary>
        /// Represents the metadata for one of the grid's cells.
        /// </summary>
        private class CellMetadata
        {
            /// <summary>
            /// Gets the cache of elements in this cell.
            /// </summary>
            public List<UIElement> Elements
            {
                get { return elements; }
            }

            /// <summary>
            /// Gets or sets the cell's x-coordinate relative to its containing grid.
            /// </summary>
            public Double GridRelativeX
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the cell's y-coordinate relative to its containing grid.
            /// </summary>
            public Double GridRelativeY
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the cell's width in pixels.
            /// </summary>
            public Double Width
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the cell's height in pixels.
            /// </summary>
            public Double Height
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the cell requires a scissor rectangle.
            /// </summary>
            public Boolean RequiresScissorRectangle
            {
                get;
                set;
            }

            // Property values.
            private readonly List<UIElement> elements = new List<UIElement>();
        }
    }
}
