using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a table of formatted text.
    /// </summary>
    /// <typeparam name="ViewModelType">The type of view model which is bound to this table.</typeparam>
    public sealed class TextTable<ViewModelType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextTable{ViewModelType}"/> class.
        /// </summary>
        /// <param name="renderer">The <see cref="TextRenderer"/> used to lay out and render the table's text.</param>
        /// <param name="width">The table's width in pixels.</param>
        /// <param name="height">The table's height in pixels.</param>
        /// <param name="font">The table's default font.</param>
        public TextTable(TextRenderer renderer, Int32 width, Int32 height, SpriteFont font)
        {
            Contract.Require(renderer, "renderer");
            Contract.EnsureRange(width >= 0, "width");
            Contract.EnsureRange(height >= 0, "height");
            Contract.Require(font, "font");

            this.renderer = renderer;
            this.width = width;
            this.height = height;
            this.font = font;
            this.rows = new TextTableRowCollection<ViewModelType>(this);
        }

        /// <summary>
        /// Resizes the table.
        /// </summary>
        /// <param name="width">The table's new width.</param>
        /// <param name="height">The table's new height.</param>
        public void Resize(Int32 width, Int32 height)
        {
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");

            this.width = width;
            this.height = height;

            this.MarkDirty();
        }

        /// <summary>
        /// Marks the table as dirty, which causes it to recalculate its layout the next time it is drawn.
        /// </summary>
        public void MarkDirty()
        {
            this.dirty = true;
        }

        /// <summary>
        /// Calculates the table's layout.
        /// </summary>
        public void PerformLayout()
        {
            if (!dirty)
                return;

            dirty = false;

            var cx = 0;
            var cy = 0;
            var rowHeight = 0;

            actualWidth = width;
            actualHeight = 0;

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];

                // Lay out the cell text.
                var automaticWidth = CalculateAutomaticWidth(row);
                foreach (var cell in row.Cells)
                {
                    var cellWidth = cell.Width ?? automaticWidth;
                    var cellHeight = cell.Height ?? 0;
                    if (i + 1 == rows.Count)
                    {
                        cellHeight = Math.Max(cellHeight, height - actualHeight);
                    }
                    cell.PerformLayout(renderer, cellWidth, cellHeight);
                    cellWidth = (cellWidth > cell.CalculatedWidth) ? cellWidth : cell.CalculatedWidth;
                    cellHeight = (cellHeight > cell.CalculatedHeight) ? cellHeight : cell.CalculatedHeight;

                    rowHeight = (cellHeight > rowHeight) ? cellHeight : rowHeight;
                }

                // Position the cells relative to the table.
                foreach (var cell in row.Cells)
                {
                    var cellWidth = Math.Max(cell.Width ?? automaticWidth, cell.CalculatedWidth);
                    var cellHeight = Math.Max(cell.Height ?? 0, cell.CalculatedHeight);
                    var cellX = cx;
                    var cellY = cy;
                    if ((cell.TextFlags & TextFlags.AlignMiddle) == TextFlags.AlignMiddle)
                    {
                        cellY = cy + ((rowHeight - cellHeight) / 2);
                    }
                    if ((cell.TextFlags & TextFlags.AlignBottom) == TextFlags.AlignBottom)
                    {
                        cellY = cy + rowHeight - cellHeight;
                    }
                    cell.Position(cellX, cellY);
                    cx = cx + cellWidth;
                }

                // Advance to the next row.
                actualHeight += rowHeight;
                cx = 0;
                cy = cy + rowHeight;
                rowHeight = 0;
            }
        }
        
        /// <summary>
        /// Draws the table.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which to draw the table.</param>
        /// <param name="position">The position in screen coordinates at which to draw the table.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(spriteBatch, position, Color.White);
        }

        /// <summary>
        /// Draws the table.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which to draw the table.</param>
        /// <param name="position">The position in screen coordinates at which to draw the table.</param>
        /// <param name="color">The table's default text color.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            Contract.Require(spriteBatch, "spriteBatch");

            PerformLayout();

            foreach (var row in rows)
            {
                foreach (var cell in row.Cells)
                {
                    var cellX = position.X + cell.X;
                    var cellY = position.Y + cell.Y;
                    cell.Draw(renderer, spriteBatch, cellX, cellY, color);
                }
            }
        }

        /// <summary>
        /// Updates the table's data bindings from the view model.
        /// </summary>
        public void Refresh()
        {
            foreach (var row in rows)
            {
                foreach (var cell in row.Cells)
                {
                    cell.Refresh();
                }
            }
            MarkDirty();
        }

        /// <summary>
        /// Gets the table's collection of rows.
        /// </summary>
        public TextTableRowCollection<ViewModelType> Rows
        {
            get { return rows; }
        }

        /// <summary>
        /// Gets the table's default font.
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }

        /// <summary>
        /// Gets the table's specified width in pixels.
        /// </summary>
        public Int32 Width
        {
            get { return width; }
        }

        /// <summary>
        /// Gets the table's specified height in pixels.
        /// </summary>
        public Int32 Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets the table's actual width as of its last layout calculation.
        /// </summary>
        public Int32 ActualWidth
        {
            get { return actualWidth; }
        }

        /// <summary>
        /// Gets the table's actual height as of its last layout calculation.
        /// </summary>
        public Int32 ActualHeight
        {
            get { return actualHeight; }
        }

        /// <summary>
        /// Gets or sets the table's view model.
        /// </summary>
        public ViewModelType ViewModel
        {
            get { return viewModel; }
            set 
            {
                UnsubscribeFromChangeEvents(viewModel as INotifyPropertyChanged);
                viewModel = value;
                SubscribeToChangeEvents(viewModel as INotifyPropertyChanged);
                Refresh();
            }
        }

        /// <summary>
        /// Calculates the width of automatically-sized cells on the specified row.
        /// </summary>
        /// <param name="row">The row to evaluate.</param>
        /// <returns>The width of automatically-sized cells on the specified row.</returns>
        private Int32 CalculateAutomaticWidth(TextTableRow<ViewModelType> row)
        {
            var availableWidth = width;
            var automaticallySizedCells = 0;
            foreach (var cell in row.Cells)
            {
                if (cell.Width == null)
                {
                    automaticallySizedCells++;
                }
                else
                {
                    availableWidth -= cell.Width.GetValueOrDefault();
                }
            }
            return (automaticallySizedCells == 0) ? 0 : availableWidth / automaticallySizedCells;
        }

        /// <summary>
        /// Unsubscribes from Change events on the specified object.
        /// </summary>
        /// <param name="inc">The object on which to unsubscribe from events.</param>
        private void UnsubscribeFromChangeEvents(INotifyPropertyChanged inc)
        {
            if (inc == null)
                return;

            inc.PropertyChanged -= ViewModelChanged;
        }

        /// <summary>
        /// Subscribes to Change events on the specified object.
        /// </summary>
        /// <param name="inc">The object on which to subscribe to events.</param>
        private void SubscribeToChangeEvents(INotifyPropertyChanged inc)
        {
            if (inc == null)
                return;

            inc.PropertyChanged += ViewModelChanged;
        }

        /// <summary>
        /// Handles the view model's <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="instance">The object instance that changed.</param>
        /// <param name="propertyName">The name of the property that was changed. If all of the object's properties have
        /// changed, this value can be either <see cref="String.Empty"/> or <c>null</c>.</param>
        private void ViewModelChanged(Object instance, String propertyName)
        {
            Refresh();
        }

        // Property values.
        private readonly TextRenderer renderer;
        private readonly SpriteFont font;
        private Int32 width;
        private Int32 height;
        private Int32 actualWidth;
        private Int32 actualHeight;
        
        // State values.
        private readonly TextTableRowCollection<ViewModelType> rows;
        private ViewModelType viewModel;
        private Boolean dirty;
    }
}
