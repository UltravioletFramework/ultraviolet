using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a row in a table of formatted text.
    /// </summary>
    public sealed class TextTableRow<ViewModelType>
    {
        /// <summary>
        /// Initializes a new instance of the TextTableRow class.
        /// </summary>
        /// <param name="table">The table that owns the row.</param>
        internal TextTableRow(TextTable<ViewModelType> table)
        {
            Contract.Require(table, "table");

            this.table = table;
            this.cells = new TextTableCellCollection<ViewModelType>(this);
        }

        /// <summary>
        /// Gets the table that owns the row.
        /// </summary>
        public TextTable<ViewModelType> Table
        {
            get { return table; }
        }

        /// <summary>
        /// Gets the row's collection of cells.
        /// </summary>
        public TextTableCellCollection<ViewModelType> Cells
        {
            get { return cells; }
        }

        // Property values.
        private readonly TextTable<ViewModelType> table;
        private readonly TextTableCellCollection<ViewModelType> cells;
    }
}
