using System;

namespace Ultraviolet.Presentation.Controls
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
            /// Updates the value of the <see cref="MeasurementPriority"/> property.
            /// </summary>
            public void UpdateMeasurementPriority()
            {
                MeasurementPriority = CalculateMeasurementPriority();
            }

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

            /// <summary>
            /// Gets a value which indicates the order in which this cell is measured
            /// compared to other virtual cells in the grid.
            /// </summary>
            public Int32 MeasurementPriority
            {
                get;
                private set;
            }

            /// <summary>
            /// Calculates the measurement priority for this cell.
            /// </summary>
            /// <returns>The cell's measurement priority based on its current state.</returns>
            private Int32 CalculateMeasurementPriority()
            {
                /*
                 * PRIORITY 0: These can always be measured because they depend only on
                 *             a statically-specified size or the size of the cell's content.
                 * PRIORITY 1: Width depends on statically-specified size or cell content.
                 *             Height depends on a star row.
                 * PRIORITY 2: Width depends on a star column.
                 *             Height depends on statically-specified size or cell content.
                 * PRIORITY 3: Both width and height depend on star rows/columns.
                 * 
                 * Note that we deviate from this a bit as a performance optimization; cells
                 * which are star/px sized are assigned priority 3 to avoid measuring them twice.
                 */

                if (ContainsStarRows && ContainsStarColumns)
                {
                    // NOTE: See note about perf optimization above
                    if (ContainsAutoColumns)
                    {
                        return 1;
                    }
                    return 3;
                }

                if (ContainsStarColumns && !ContainsStarRows)
                    return 2;

                if (ContainsStarRows && !ContainsStarColumns)
                    return 1;

                return 0;
            }
        }
    }
}
