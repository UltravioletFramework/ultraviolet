using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    partial class Grid
    {
        /// <summary>
        /// Represents the metadata for one of a grid's "virtual cells," which encapsulates
        /// the full row and column span of a single child element.
        /// </summary>
        private class VirtualCellMetadata
        {
            /// <summary>
            /// Gets or sets the child element that the cell represents.
            /// </summary>
            public UIElement Element
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the index of the first row in the virtual cell.
            /// </summary>
            public Int32 RowIndex
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the number of rows in the virtual cell.
            /// </summary>
            public Int32 RowSpan
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the index of the first column in the virtual cell.
            /// </summary>
            public Int32 ColumnIndex
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the number of columns in the virtual cell.
            /// </summary>
            public Int32 ColumnSpan
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the virtual cell contains any auto-sized rows.
            /// </summary>
            public Boolean ContainsAutoRows
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the virtual cell contains any star-sized rows.
            /// </summary>
            public Boolean ContainsStarRows
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the virtual cell contains any auto-sized columns.
            /// </summary>
            public Boolean ContainsAutoColumns
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the virtual cell contains any star-sized columns.
            /// </summary>
            public Boolean ContainsStarColumns
            {
                get;
                set;
            }
        }
    }
}
