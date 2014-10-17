using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a collection containing a table's rows.
    /// </summary>
    public sealed class TextTableRowCollection<ViewModelType> : UltravioletCollection<TextTableRow<ViewModelType>>
    {
        /// <summary>
        /// Initializes a new instance of the TextTableRowCollection class.
        /// </summary>
        /// <param name="table">The table that owns the row collection.</param>
        internal TextTableRowCollection(TextTable<ViewModelType> table)
        {
            Contract.Require(table, "table");

            this.table = table;
        }

        /// <summary>
        /// Adds a row to the collection.
        /// </summary>
        /// <returns>The row that was added to the collection.</returns>
        public TextTableRow<ViewModelType> Add()
        {
            var row = new TextTableRow<ViewModelType>(table);
            AddInternal(row);
            table.MarkDirty();
            return row;
        }

        /// <summary>
        /// Gets the table that owns the row collection.
        /// </summary>
        public TextTable<ViewModelType> Table
        {
            get { return table; }
        }

        // Property values.
        private readonly TextTable<ViewModelType> table;
    }
}
