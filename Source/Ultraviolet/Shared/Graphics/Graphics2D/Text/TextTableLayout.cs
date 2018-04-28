using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the layout information for a table of formatted text.
    /// </summary>
    public sealed class TextTableLayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextTableLayout"/> class.
        /// </summary>
        /// <param name="description">The table layout description.</param>
        internal TextTableLayout(TextTableLayoutDescription description)
        {
            Contract.Require(description, nameof(description));

            this.description = description;
        }

        /// <summary>
        /// Creates a table from the layout information.
        /// </summary>
        /// <typeparam name="ViewModelType">The type of view model which is bound to this table.</typeparam>
        /// <param name="renderer">The <see cref="TextRenderer"/> used to lay out and render the table's text.</param>
        /// <param name="font">The table's default font.</param>
        /// <returns>The <see cref="TextTable{ViewModelType}"/> that was created.</returns>
        public TextTable<ViewModelType> Create<ViewModelType>(TextRenderer renderer, UltravioletFont font)
        {
            Contract.Require(renderer, nameof(renderer));
            Contract.Require(font, nameof(font));
            
            var table = new TextTable<ViewModelType>(renderer, description.Width ?? 0, description.Height ?? 0, font);

            if (description.Rows != null)
            {
                foreach (var rowDesc in description.Rows)
                {
                    var row = table.Rows.Add();
                    if (rowDesc.Cells != null)
                    {
                        foreach (var cellDesc in rowDesc.Cells)
                        {
                            var cell = row.Cells.Add();
                            cell.TextFlags = cellDesc.TextFlags;
                            cell.RawText = cellDesc.Text;
                            cell.Format = cellDesc.Format;
                            cell.Width = cellDesc.Width;
                            cell.Height = cellDesc.Height;

                            if (!String.IsNullOrEmpty(cellDesc.Binding))
                                cell.Bind(cellDesc.Binding);

                            cell.Refresh();
                        }
                    }
                }
            }
            
            return table;
        }

        // State values.
        private readonly TextTableLayoutDescription description;
    }
}
