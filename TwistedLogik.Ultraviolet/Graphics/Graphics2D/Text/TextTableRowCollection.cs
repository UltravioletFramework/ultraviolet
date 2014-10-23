using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a collection of rows in a <see cref="TextTable{ViewModelType}"/>.
    /// </summary>
    /// <typeparam name="ViewModelType">The type of view model which is bound to this table.</typeparam>
    public sealed class TextTableRowCollection<ViewModelType> : UltravioletCollection<TextTableRow<ViewModelType>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextTableRowCollection{ViewModelType}"/> class.
        /// </summary>
        /// <param name="table">The <see cref="TextTable{ViewModelType}"/> that owns the row collection.</param>
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
