using System;
using System.Linq.Expressions;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a cell in a formatted text table.
    /// </summary>
    /// <typeparam name="ViewModelType">The type of view model which is bound to this table.</typeparam>
    public sealed class TextTableCell<ViewModelType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextTableCell{ViewModelType}"/> class.
        /// </summary>
        /// <param name="row">The row that owns the cell.</param>
        internal TextTableCell(TextTableRow<ViewModelType> row)
        {
            Contract.Require(row, "row");

            this.row = row;
        }

        /// <summary>
        /// Updates the cell's data bindings from the view model.
        /// </summary>
        public void Refresh()
        {
            if (getBoundData == null)
                return;

            Text = getBoundData(row.Table.ViewModel);
        }

        /// <summary>
        /// Binds the cell to the specified property on its view model.
        /// </summary>
        /// <param name="name">The name of the property to which to bind the cell.</param>
        public void Bind(String name)
        {
            var property = typeof(ViewModelType).GetProperty(name);
            if (property == null)
                throw new ArgumentException(UltravioletStrings.InvalidViewModelProperty.Format(name));

            var getter = property.GetGetMethod();
            if (getter == null)
                throw new ArgumentException(UltravioletStrings.InvalidViewModelProperty.Format(name));

            this.getBoundData = (vm) =>
            {
                var obj = getter.Invoke(vm, null);
                var fmt = String.IsNullOrWhiteSpace(Format) ? "{0}" : Format;
                return String.Format(fmt, obj);
            };
            this.row.Table.MarkDirty();
        }

        /// <summary>
        /// Binds the cell to the specified property on its view model.
        /// </summary>
        /// <typeparam name="T">The return type of the property to bind.</typeparam>
        /// <param name="exp">An expression </param>
        public void Bind<T>(Expression<Func<ViewModelType, T>> exp)
        {
            var getter = ((LambdaExpression)exp).Compile() as Func<ViewModelType, T>;
            this.getBoundData = (vm) => 
            { 
                var obj = getter(vm);
                var fmt = String.IsNullOrWhiteSpace(Format) ? "{0}" : Format;
                return String.Format(fmt, obj);
            };
            this.row.Table.MarkDirty();
        }

        /// <summary>
        /// Gets the row that owns the cell.
        /// </summary>
        public TextTableRow<ViewModelType> Row
        {
            get { return row; }
        }

        /// <summary>
        /// Gets or sets a string used to specify how the cell's text should be formatted.
        /// If null or empty, no special formatting is applied.
        /// </summary>
        public String Format
        {
            get { return format; }
            set
            {
                if (format != value)
                {
                    format = value;
                    row.Table.MarkDirty();
                }
            }
        }

        /// <summary>
        /// Gets the cell's text.
        /// </summary>
        public String Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    row.Table.MarkDirty();
                }
            }
        }

        /// <summary>
        /// Gets the cell's text flags.
        /// </summary>
        public TextFlags TextFlags
        {
            get { return textFlags; }
            set
            {
                if (textFlags != value)
                {
                    textFlags = value;
                    row.Table.MarkDirty();
                }
            }
        }

        /// <summary>
        /// Gets the cell's width in pixels.
        /// </summary>
        public Int32? Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    width = value;
                    row.Table.MarkDirty();
                }
            }
        }

        /// <summary>
        /// Gets the cell's height in pixels.
        /// </summary>
        public Int32? Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    row.Table.MarkDirty();
                }
            }
        }

        /// <summary>
        /// Draws the cell's contents.
        /// </summary>
        /// <param name="renderer">The text renderer used to lay out and render the table's text.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the cell.</param>
        /// <param name="x">The x-coordinate at which to draw the cell.</param>
        /// <param name="y">The y-coordinate at which to draw the cell.</param>
        /// <param name="color">The cell's default text color.</param>
        internal void Draw(TextRenderer renderer, SpriteBatch spriteBatch, Single x, Single y, Color color)
        {
            renderer.Draw(spriteBatch, layout, new Vector2(x, y), color);
        }

        /// <summary>
        /// Positions the cell relative to its table.
        /// </summary>
        /// <param name="x">The cell's x-coordinate relative to its table.</param>
        /// <param name="y">The cell's y-coordinate relative to its table.</param>
        internal void Position(Single x, Single y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Performs layout calculations for this cell.
        /// </summary>
        /// <param name="renderer">The text renderer used to lay out and render the table's text.</param>
        /// <param name="width">The cell's width in pixels.</param>
        /// <param name="height">The cell's height in pixels.</param>
        internal void PerformLayout(TextRenderer renderer, Int32 width, Int32 height)
        {
            var settings = new TextLayoutSettings(row.Table.Font, width, null, textFlags);
            renderer.CalculateLayout(text, layout, settings);
        }

        /// <summary>
        /// Gets the cell's x-coordinate relative to its table as of its last layout calculation.
        /// </summary>
        public Single X
        {
            get { return x; }
        }

        /// <summary>
        /// Gets the cell's y-coordinate relative to its table as of its last layout calculation.
        /// </summary>
        public Single Y
        {
            get { return y; }
        }

        /// <summary>
        /// Gets the cell's calculated width as of its last layout calculation.
        /// </summary>
        public Int32 CalculatedWidth
        {
            get { return layout.ActualWidth; }
        }

        /// <summary>
        /// Gets the cell's calculated height as of its last layout calculation.
        /// </summary>
        public Int32 CalculatedHeight
        {
            get { return layout.ActualHeight; }
        }

        // Property values.
        private readonly TextTableRow<ViewModelType> row;
        private String format;
        private String text;
        private TextFlags textFlags;
        private Int32? width;
        private Int32? height;
        private Single x;
        private Single y;

        // State values.
        private TextLayoutResult layout = new TextLayoutResult();
        private Func<ViewModelType, String> getBoundData;        
    }
}
