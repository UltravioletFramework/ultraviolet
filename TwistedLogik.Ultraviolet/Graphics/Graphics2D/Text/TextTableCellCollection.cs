using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a collection of <see cref="TextTableCell{ViewModelType}"/> objects.
    /// </summary>
    /// <typeparam name="ViewModelType">The type of view model which is bound to this table.</typeparam>
    public sealed class TextTableCellCollection<ViewModelType> : UltravioletCollection<TextTableCell<ViewModelType>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextTableCellCollection{ViewModelType}"/>.
        /// </summary>
        /// <param name="row">The row that owns the collection.</param>
        internal TextTableCellCollection(TextTableRow<ViewModelType> row)
        {
            Contract.Require(row, "row");

            this.row = row;
        }

        /// <summary>
        /// Adds a cell to the collection.
        /// </summary>
        /// <returns>The cell that was added to the collection.</returns>
        public TextTableCell<ViewModelType> Add()
        {
            return Add(null, null, TextFlags.Standard, null, null);
        }

        /// <summary>
        /// Adds a cell to the collection.
        /// </summary>
        /// <param name="format">The cell's format string.</param>
        /// <param name="text">The cell's text.</param>
        /// <param name="textFlags">The cell's text flags.</param>
        /// <returns>The cell that was added to the collection.</returns>
        public TextTableCell<ViewModelType> Add(String format, String text, TextFlags textFlags)
        {
            return Add(format, text, textFlags, null, null);
        }

        /// <summary>
        /// Adds a cell to the collection.
        /// </summary>
        /// <param name="format">The cell's format string.</param>
        /// <param name="text">The cell's text.</param>
        /// <param name="textFlags">The cell's text flags.</param>
        /// <param name="width">The cell's width in pixels, or <c>null</c> to automatically size the cell.</param>
        /// <param name="height">The cell's height in pixels, or <c>null</c> to automatically size the cell.</param>
        /// <returns>The cell that was added to the collection.</returns>
        public TextTableCell<ViewModelType> Add(String format, String text, TextFlags textFlags, Int32? width, Int32? height)
        {
            var cell = new TextTableCell<ViewModelType>(row)
            {
                Format = format,
                Text = text,
                TextFlags = textFlags,
                Width = width,
                Height = height,
            };
            AddInternal(cell);
            row.Table.MarkDirty();
            return cell;
        }

        /// <summary>
        /// Gets the row that owns the collection.
        /// </summary>
        public TextTableRow<ViewModelType> Row
        {
            get { return row; }
        }

        // State values.
        private readonly TextTableRow<ViewModelType> row;
    }
}
