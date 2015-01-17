using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a grid of columns and columns which can contain child elements in each cell.
    /// </summary>
    [UIElement("Grid")]
    public partial class Grid : Panel
    {
        /// <summary>
        /// Initializes the <see cref="Grid"/> type.
        /// </summary>
        static Grid()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(Grid).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.Grid.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Grid(UltravioletContext uv, String id)
            : base(uv, id)
        {
            this.rowDefinitions    = new RowDefinitionCollection(this);
            this.columnDefinitions = new ColumnDefinitionCollection(this);

            LoadComponentRoot(ComponentTemplate);
        }

        /// <summary>
        /// Sets a value that indicates which column a specified content element should occupy within its <see cref="Grid"/> container.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="column">The index of the column that the element should occupy.</param>
        public static void SetRow(UIElement element, Int32 column)
        {
            Contract.Require(element, "element");

            element.SetValue<Int32>(RowProperty, column);
        }

        /// <summary>
        /// Sets a value that indicates which column a specified content element should occupy within its <see cref="Grid"/> container.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="column">The index of the column that the element should occupy.</param>
        public static void SetColumn(UIElement element, Int32 column)
        {
            Contract.Require(element, "element");

            element.SetValue<Int32>(ColumnProperty, column);
        }

        /// <summary>
        /// Gets a value that indicates which column a specified content element should occupy within its <see cref="Grid"/> container.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The index of the column that the element should occupy.</returns>
        public static Int32 GetRow(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Int32>(RowProperty);
        }

        /// <summary>
        /// Gets a value that indicates which column a specified content element should occupy within its <see cref="Grid"/> container.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The index of the column that the element should occupy.</returns>
        public static Int32 GetColumn(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Int32>(ColumnProperty);
        }

        /// <summary>
        /// Gets or sets the template used to create the control's component tree.
        /// </summary>
        public static XDocument ComponentTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the grid's collection of column definitions.
        /// </summary>
        public RowDefinitionCollection RowDefinitions
        {
            get { return rowDefinitions; }
        }

        /// <summary>
        /// Gets the grid's collection of column definitions.
        /// </summary>
        public ColumnDefinitionCollection ColumnDefinitions
        {
            get { return columnDefinitions; }
        }

        /// <summary>
        /// Identifies the Row attached property.
        /// </summary>
        public static readonly DependencyProperty RowProperty = DependencyProperty.Register("Row", typeof(Int32), typeof(Grid),
            new DependencyPropertyMetadata(HandleRowChanged, () => 0, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the Column attached property.
        /// </summary>
        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register("Column", typeof(Int32), typeof(Grid),
            new DependencyPropertyMetadata(HandleColumnChanged, () => 0, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Occurs when the grid's column definitions are modified.
        /// </summary>
        protected internal virtual void OnColumnsModified()
        {

        }

        /// <summary>
        /// Occurs when the grid's column definitions are modified.
        /// </summary>
        protected internal virtual void OnRowsModified()
        {

        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            foreach (var child in Children)
                child.Measure(new Size2D(Double.PositiveInfinity, Double.PositiveInfinity));

            var desiredWidth  = MeasureWidth(availableSize.Width);
            var desiredHeight = MeasureHeight(availableSize.Height);

            var contentSize = new Size2D(desiredWidth, desiredHeight);
            return MeasureComponents(availableSize, contentSize);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            ArrangeComponents(finalSize);

            var contentRegionSize = GetContentRegionSize(finalSize);

            MeasureWidth(contentRegionSize.Width);
            MeasureHeight(contentRegionSize.Height);

            var grx = 0.0;
            var gry = 0.0;

            for (var col = 0; col < ColumnDefinitions.Count; col++)
            {
                ColumnDefinitions[col].OffsetX = grx;
                grx += ColumnDefinitions[col].MeasuredWidth;
            }

            for (var row = 0; row < RowDefinitions.Count; row++)
            {
                RowDefinitions[row].OffsetY = gry;
                gry += RowDefinitions[row].MeasuredHeight;
            }

            UpdateCellMetadata(finalSize);

            foreach (var cell in cells)
            {
                var cellRect = new RectangleD(
                    cell.GridRelativeX, 
                    cell.GridRelativeY, 
                    cell.Width, 
                    cell.Height);

                foreach (var child in cell.Elements)
                {
                    child.Arrange(cellRect);
                }
            }

            return finalSize;
        }

        /// <summary>
        /// Gets the number of columns in the grid, including any implicit columns.
        /// </summary>
        protected Int32 RowCount
        {
            get { return columnDefinitions.Count == 0 ? 1 : columnDefinitions.Count; }
        }

        /// <summary>
        /// Gets the number of columns in the grid, including any implicit columns.
        /// </summary>
        protected Int32 ColumnCount
        {
            get { return columnDefinitions.Count == 0 ? 1 : columnDefinitions.Count; }
        }

        /// <summary>
        /// Occurs when the value of the Row attached property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleRowChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            // TODO
        }

        /// <summary>
        /// Occurs when the value of the Column attached property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleColumnChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            // TODO
        }
        
        /// <summary>
        /// Measures the combined width of the grid's columns.
        /// </summary>
        /// <param name="available">The amount of available space for columns.</param>
        /// <returns>The desired width for the grid's columns.</returns>
        private Double MeasureWidth(Double available)
        {
            if (ColumnDefinitions.Count == 0)
                return available;

            var proportionalFactor = 0.0;

            available = MeasureAutoAndStaticColumns(available, out proportionalFactor);

            var proportionalUnit = (proportionalFactor == 0) ? 0 : available / proportionalFactor;

            MeasureProportionalColumns(proportionalUnit);

            var desired = 0.0;

            foreach (var column in ColumnDefinitions)
                desired += column.MeasuredWidth;

            return desired;
        }

        /// <summary>
        /// Calculates and updates the size of auto- and statically-sized columns.
        /// </summary>
        /// <param name="available">The amount of space available for columns.</param>
        /// <param name="proportionalFactor">A value specifying the total number of proportionally sized units.</param>
        /// <returns>The amount of remaining space available for columns after accounting for auto- and statically-sized columns.</returns>
        private Double MeasureAutoAndStaticColumns(Double available, out Double proportionalFactor)
        {
            proportionalFactor = 0.0;

            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                var column = ColumnDefinitions[i];
                if (column.Width.GridUnitType == GridUnitType.Star)
                {
                    proportionalFactor += column.Width.Value;
                }
                else
                {
                    available -= MeasureAutoOrStaticColumn(column, i);
                }
            }

            return Math.Max(0, available);
        }

        /// <summary>
        /// Calculates the size of an Auto- or statically-sized column.
        /// </summary>
        /// <param name="column">The column for which to calculate a size.</param>
        /// <param name="ix">The index of the column within the grid.</param>
        /// <returns>The actual size of the column in pixels.</returns>
        private Double MeasureAutoOrStaticColumn(ColumnDefinition column, Int32 ix)
        {
            switch (column.Width.GridUnitType)
            {
                case GridUnitType.Auto:
                    return MeasureAutoColumn(column, ix);
                case GridUnitType.Pixel:
                    return MeasureStaticColumn(column, ix);
            }
            return 0;
        }

        /// <summary>
        /// Calculates the size of an Auto-sized column.
        /// </summary>
        /// <param name="column">The column for which to calculate a size.</param>
        /// <param name="ix">The index of the column within the grid.</param>
        /// <returns>The actual size of the column in pixels.</returns>
        private Double MeasureAutoColumn(ColumnDefinition column, Int32 ix)
        {
            var size = 0.0;

            foreach (var child in Children)
            {
                if (!LayoutUtil.IsSpaceFilling(child))
                    continue;

                if (GetColumn(child) != ix)
                    continue;

                var childSize = child.DesiredSize.Width;
                if (childSize > size)
                {
                    size = childSize;
                }
            }

            column.MeasuredWidth = size;
            return size;
        }

        /// <summary>
        /// Calculates the size of an statically-sized column.
        /// </summary>
        /// <param name="column">The column for which to calculate a size.</param>
        /// <param name="ix">The index of the column within the grid.</param>
        /// <returns>The actual size of the column in pixels.</returns>
        private Double MeasureStaticColumn(ColumnDefinition column, Int32 ix)
        {
            Double lower, upper;
            LayoutUtil.GetBoundedMeasure(column.Width.Value, column.MinWidth, column.MaxWidth, out lower, out upper);
            column.MeasuredWidth = lower;
            return lower;
        }

        /// <summary>
        /// Calculates and updates the size of proportional columns.
        /// </summary>
        /// <param name="proportionalUnit">The size of the base unit (corresponding to 1*) for proportional columns, in pixels.</param>
        private void MeasureProportionalColumns(Double proportionalUnit)
        {
            foreach (var column in ColumnDefinitions)
            {
                if (column.Width.GridUnitType != GridUnitType.Star)
                    continue;

                column.MeasuredWidth = (Int32)(column.Width.Value * proportionalUnit);
            }
        }

        /// <summary>
        /// Measures the combined height of the grid's columns.
        /// </summary>
        /// <param name="available">The amount of available space for columns.</param>
        /// <returns>The desired height for the grid's columns.</returns>
        private Double MeasureHeight(Double available)
        {
            if (RowDefinitions.Count == 0)
                return available;

            var proportionalFactor = 0.0;

            available = MeasureAutoAndStaticRows(available, out proportionalFactor);

            var proportionalUnit = (proportionalFactor == 0) ? 0 : available / proportionalFactor;

            MeasureProportionalRows(proportionalUnit);

            var desired = 0.0;

            foreach (var column in RowDefinitions)
                desired += column.MeasuredHeight;

            return desired;
        }

        /// <summary>
        /// Calculates and updates the size of auto- and statically-sized columns.
        /// </summary>
        /// <param name="available">The amount of space available for columns.</param>
        /// <param name="proportionalFactor">A value specifying the total number of proportionally sized units.</param>
        /// <returns>The amount of remaining space available for columns after accounting for auto- and statically-sized columns.</returns>
        private Double MeasureAutoAndStaticRows(Double available, out Double proportionalFactor)
        {
            proportionalFactor = 0.0;

            for (int i = 0; i < RowDefinitions.Count; i++)
            {
                var column = RowDefinitions[i];
                if (column.Height.GridUnitType == GridUnitType.Star)
                {
                    proportionalFactor += column.Height.Value;
                }
                else
                {
                    available -= MeasureAutoOrStaticRow(column, i);
                }
            }

            return Math.Max(0, available);
        }

        /// <summary>
        /// Calculates the size of an Auto- or statically-sized column.
        /// </summary>
        /// <param name="column">The column for which to calculate a size.</param>
        /// <param name="ix">The index of the column within the grid.</param>
        /// <returns>The actual size of the column in pixels.</returns>
        private Double MeasureAutoOrStaticRow(RowDefinition column, Int32 ix)
        {
            switch (column.Height.GridUnitType)
            {
                case GridUnitType.Auto:
                    return MeasureAutoRow(column, ix);
                case GridUnitType.Pixel:
                    return MeasureStaticRow(column, ix);
            }
            return 0;
        }

        /// <summary>
        /// Calculates the size of an Auto-sized column.
        /// </summary>
        /// <param name="column">The column for which to calculate a size.</param>
        /// <param name="ix">The index of the column within the grid.</param>
        /// <returns>The actual size of the column in pixels.</returns>
        private Double MeasureAutoRow(RowDefinition column, Int32 ix)
        {
            var size = 0.0;

            foreach (var child in Children)
            {
                if (!LayoutUtil.IsSpaceFilling(child))
                    continue;

                if (GetRow(child) != ix)
                    continue;

                var childSize = child.DesiredSize.Height;
                if (childSize > size)
                {
                    size = childSize;
                }
            }

            column.MeasuredHeight = size;
            return size;
        }

        /// <summary>
        /// Calculates the size of an statically-sized column.
        /// </summary>
        /// <param name="column">The column for which to calculate a size.</param>
        /// <param name="ix">The index of the column within the grid.</param>
        /// <returns>The actual size of the column in pixels.</returns>
        private Double MeasureStaticRow(RowDefinition column, Int32 ix)
        {
            Double lower, upper;
            LayoutUtil.GetBoundedMeasure(column.Height.Value, column.MinHeight, column.MaxHeight, out lower, out upper);
            column.MeasuredHeight = lower;
            return lower;
        }

        /// <summary>
        /// Calculates and updates the size of proportional rows.
        /// </summary>
        /// <param name="proportionalUnit">The size of the base unit (corresponding to 1*) for proportional rows, in pixels.</param>
        private void MeasureProportionalRows(Double proportionalUnit)
        {
            foreach (var row in RowDefinitions)
            {
                if (row.Height.GridUnitType != GridUnitType.Star)
                    continue;

                row.MeasuredHeight = (Int32)(row.Height.Value * proportionalUnit);
            }
        }

        /// <summary>
        /// Updates the metadata for each of the grid's cells.
        /// </summary>
        /// <param name="finalSize">The size of the layout region that has been assigned to the grid by its parent.</param>
        private void UpdateCellMetadata(Size2D finalSize)
        {
            ExpandCellMetadataArray();

            var contentRegionSize = GetContentRegionSize(finalSize);

            for (var row = 0; row < RowCount; row++)
            {
                var rowdef = (row >= RowDefinitions.Count) ? null : RowDefinitions[row];

                for (var col = 0; col < ColumnCount; col++)
                {
                    var coldef = (col >= ColumnDefinitions.Count) ? null : ColumnDefinitions[col];

                    var cell = cells[(row * ColumnCount) + col];

                    cell.GridRelativeX = (coldef == null) ? 0 : coldef.OffsetX;
                    cell.GridRelativeY = (rowdef == null) ? 0 : rowdef.OffsetY;
                    cell.Width   = (coldef == null) ? contentRegionSize.Width : coldef.MeasuredWidth;
                    cell.Height  = (rowdef == null) ? contentRegionSize.Height : rowdef.MeasuredHeight;

                    cell.Elements.Clear();
                }
            }

            foreach (var child in Children)
            {
                var row = Grid.GetRow(child);
                var col = Grid.GetColumn(child);

                var cell = cells[(row * ColumnCount) + col];
                cell.Elements.Add(child);
            }
        }

        /// <summary>
        /// If necessary, expands the cell metadata array to accomodate all of the grid's rows and columns.
        /// </summary>
        private void ExpandCellMetadataArray()
        {
            if (cells.Length >= RowCount * ColumnCount)
                return;

            cells = new CellMetadata[RowCount * ColumnCount];
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new CellMetadata();
            }
        }

        // Property values.
        private readonly RowDefinitionCollection rowDefinitions;
        private readonly ColumnDefinitionCollection columnDefinitions;

        // Cached cell metadata.
        private CellMetadata[] cells = new[] { new CellMetadata() };
    }
}
