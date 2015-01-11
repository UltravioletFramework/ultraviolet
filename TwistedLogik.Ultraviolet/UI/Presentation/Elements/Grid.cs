using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a grid of columns and rows which can contain child elements in each cell.
    /// </summary>
    [UIElement("Grid")]
    public partial class Grid : Container
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
            SetDefaultValue<Color>(BackgroundColorProperty, Color.Transparent);

            this.rowDefinitions    = new RowDefinitionCollection(this);
            this.columnDefinitions = new ColumnDefinitionCollection(this);

            LoadComponentRoot(ComponentTemplate);
        }

        /// <summary>
        /// Sets a value that indicates which row a specified content element should occupy within its <see cref="Grid"/> container.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="row">The index of the row that the element should occupy.</param>
        public static void SetRow(UIElement element, Int32 row)
        {
            Contract.Require(element, "element");

            element.SetValue<Int32>(RowProperty, row);
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
        /// Gets a value that indicates which row a specified content element should occupy within its <see cref="Grid"/> container.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The index of the row that the element should occupy.</returns>
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

        /// <inheritdoc/>
        public override void CalculateContentSize(ref Int32? width, ref Int32? height)
        {
            if (width == null && ColumnDefinitions.Count > 0)
            {
                var totalWidth = 0;
                foreach (var column in ColumnDefinitions)
                {
                    totalWidth += column.ActualWidth;
                }
                width = totalWidth;
            }
            
            if (height == null && RowDefinitions.Count > 0)
            {
                var totalHeight = 0;
                foreach (var row in RowDefinitions)
                {
                    totalHeight += row.ActualHeight;
                }
                height = totalHeight;
            }

            base.CalculateContentSize(ref width, ref height);
        }

        /// <inheritdoc/>
        public override void RequestPartialLayout(UIElement content)
        {
            Contract.Require(content, "content");

            RequestLayout();
        }

        /// <inheritdoc/>
        public override void PerformPartialLayout(UIElement content)
        {
            Contract.Require(content, "content");

            RequestLayout();

            base.PerformPartialLayout(content);
        }

        /// <inheritdoc/>
        public override void PerformContentLayout()
        {
            var pxAvailableWidth  = ContentPanel.ActualWidth;
            var pxAvailableHeight = ContentPanel.ActualHeight;

            var proportionalFactorColumns = 0.0;
            var proportionalFactorRows    = 0.0;
            
            pxAvailableWidth  = MeasureAutoAndStaticColumns(pxAvailableWidth, out proportionalFactorColumns);
            pxAvailableHeight = MeasureAutoAndStaticRows(pxAvailableHeight, out proportionalFactorRows);

            var pxProportionalUnitRows    = (proportionalFactorRows == 0)    ? 0 : (Int32)(pxAvailableHeight / proportionalFactorRows);
            var pxProportionalUnitColumns = (proportionalFactorColumns == 0) ? 0 : (Int32)(pxAvailableWidth  / proportionalFactorColumns);

            MeasureProportionalColumns(pxProportionalUnitColumns);
            MeasureProportionalRows(pxProportionalUnitRows);

            PositionColumns();
            PositionRows();

            foreach (var child in Children)
            {
                if (!ElementParticipatesInLayout(child))
                    continue;

                PerformContentLayout(child);
            }

            UpdateScissorRectangle();
            UpdateCellMetadata();
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
        /// Gets the grid's collection of row definitions.
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

        /// <inheritdoc/>
        protected override void DrawChildren(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            // Draw cells which do not require a scissor rect.
            for (int column = 0; column < ColumnCount; column++)
            {
                for (int row = 0; row < RowCount; row++)
                {
                    DrawCell(time, spriteBatch, opacity, column, row, false);
                }
            }

            // Draw cells which require a scissor rect.
            for (int column = 0; column < ColumnCount; column++)
            {
                for (int row = 0; row < RowCount; row++)
                {
                    DrawCell(time, spriteBatch, opacity, column, row, true);
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnUpdating(UltravioletTime time)
        {
            foreach (var columnDefinition in columnDefinitions)
                columnDefinition.Digest(time);

            base.OnUpdating(time);
        }

        /// <inheritdoc/>
        protected override void UpdateScissorRectangle()
        {
            var required = false;

            var cx = 0;
            var cy = 0;

            for (int y = 0; y < RowDefinitions.Count; y++)
            {
                cx = 0;

                for (int x = 0; x < ColumnDefinitions.Count; x++)
                {
                    cx = cx + ColumnDefinitions[x].ActualWidth;

                    if (cx > ActualWidth || cy > ActualHeight)
                    {
                        required = true;
                    }

                    if (required)
                        break;
                }

                cy = cy + RowDefinitions[y].ActualHeight;
            }

            RequiresScissorRectangle = required;
        }

        /// <summary>
        /// Draws the specified cell.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the view.</param>
        /// <param name="opacity">The opacity with which to draw the cell.</param>
        /// <param name="column">The index of the cell's column.</param>
        /// <param name="row">The index of the cell's row.</param>
        /// <param name="scissor">A value indicating whether to draw cells which require a scissor rectangle.</param>
        protected virtual void DrawCell(UltravioletTime time, SpriteBatch spriteBatch, Single opacity, Int32 column, Int32 row, Boolean scissor)
        {
            var cellIndex = (row * ColumnCount ) + column;
            var cell      = cells[cellIndex];

            if (cell.ActualWidth == 0 || cell.ActualHeight == 0)
                return;

            if (cell.RequiresScissorRectangle != scissor)
                return;

            var cx = AbsoluteScreenX + ContentPanelX + cell.GridRelativeX;
            var cy = AbsoluteScreenY + ContentPanelY + cell.GridRelativeY;

            var graphics    = Ultraviolet.GetGraphics();
            var gridScissor = (Rectangle?)null;

            if (cell.RequiresScissorRectangle)
            {
                var cellScissor = new Rectangle(cx, cy, cell.ActualWidth, cell.ActualHeight);
                if (!ApplyScissorRectangle(spriteBatch, cellScissor))
                    return;
            }

            if (Ultraviolet.GetUI().PresentationFramework.DebugRenderingEnabled)
            {
                spriteBatch.Draw(UIElementResources.BlankTexture,
                    new RectangleF(cx, cy, cell.ActualWidth, cell.ActualHeight), DebugColors.Get(cellIndex) * 0.4f);
            }

            var cumulativeOpacity = Opacity * opacity;
            foreach (var child in cell.Elements)
            {
                if (!ElementIsDrawn(child))
                    continue;

                child.Draw(time, spriteBatch, cumulativeOpacity);
            }

            if (cell.RequiresScissorRectangle)
            {
                RevertScissorRectangle(spriteBatch);
            }
        }
        
        /// <summary>
        /// Occurs when the grid's column definitions are modified.
        /// </summary>
        protected internal virtual void OnColumnsModified()
        {
            RequestLayout();
        }

        /// <summary>
        /// Occurs when the grid's row definitions are modified.
        /// </summary>
        protected internal virtual void OnRowsModified()
        {
            RequestLayout();
        }

        /// <summary>
        /// Gets the number of rows in the grid, including any implicit rows.
        /// </summary>
        protected Int32 RowCount
        {
            get { return rowDefinitions.Count == 0 ? 1 : rowDefinitions.Count; }
        }

        /// <summary>
        /// Gets the number of columns in the grid, including any implicit columns.
        /// </summary>
        protected Int32 ColumnCount
        {
            get { return columnDefinitions.Count == 0 ? 1 : columnDefinitions.Count; }
        }

        /// <inheritdoc/>
        internal override UIElement GetElementAtPointInternal(int x, int y, bool hitTest)
        {
            if (!Bounds.Contains(x, y))
                return null;

            var ixRow  = GetRowAtPosition(x, y);
            var ixCol  = GetColumnAtPosition(x, y);
            var ixCell = (ixRow * ColumnCount) + ixCol;

            var cell = cells[ixCell];

            var contentX = x - ContentPanelX;
            var contentY = y - ContentPanelY;
            for (int i = cell.Elements.Count - 1; i >= 0; i--)
            {
                var child   = cell.Elements[i];

                var element = child.GetElementAtPointInternal(contentX - child.ParentRelativeX, contentY - child.ParentRelativeY, hitTest);
                if (element != null)
                {
                    return element;
                }
            }

            return HitTestVisible ? this : null;
        }

        /// <summary>
        /// Occurs when the value of the Row attached property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleRowChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;

            var grid = element.Parent as Grid;
            if (grid != null)
                grid.UpdateCellMetadata();
        }

        /// <summary>
        /// Occurs when the value of the Column attached property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleColumnChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;

            var grid = element.Parent as Grid;
            if (grid != null)
                grid.UpdateCellMetadata();
        }

        /// <summary>
        /// Calculates and updates the size of auto- and statically-sized columns.
        /// </summary>
        /// <param name="availableSize">The amount of space available for columns.</param>
        /// <param name="proportionalColumnFactor">A value specifying the total number of proportionally sized units.</param>
        /// <returns>The amount of remaining space available for columns after accounting for auto- and statically-sized columns.</returns>
        private Int32 MeasureAutoAndStaticColumns(Int32 availableSize, out Double proportionalColumnFactor)
        {
            proportionalColumnFactor = 0.0;

            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                var column = ColumnDefinitions[i];
                if (column.Width.GridUnitType == GridUnitType.Star)
                {
                    proportionalColumnFactor += column.Width.Value;
                }
                else
                {
                    availableSize -= MeasureAutoOrStaticColumn(column, i);
                }
            }

            return Math.Max(0, availableSize);
        }

        /// <summary>
        /// Calculates the size of an Auto- or statically-sized column.
        /// </summary>
        /// <param name="column">The column for which to calculate a size.</param>
        /// <param name="ix">The index of the column within the grid.</param>
        /// <returns>The actual size of the column in pixels.</returns>
        private Int32 MeasureAutoOrStaticColumn(ColumnDefinition column, Int32 ix)
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
        private Int32 MeasureAutoColumn(ColumnDefinition column, Int32 ix)
        {
            var pxSize = 0;

            foreach (var child in Children)
            {
                if (!ElementParticipatesInLayout(child))
                    continue;

                if (GetColumn(child) != ix)
                    continue;

                var size = CalculateChildSizeWithMargin(child);
                if (size.Width > pxSize)
                    pxSize = size.Width;
            }

            column.ActualWidth = pxSize;
            return pxSize;
        }

        /// <summary>
        /// Calculates the size of an statically-sized column.
        /// </summary>
        /// <param name="column">The column for which to calculate a size.</param>
        /// <param name="ix">The index of the column within the grid.</param>
        /// <returns>The actual size of the column in pixels.</returns>
        private Int32 MeasureStaticColumn(ColumnDefinition column, Int32 ix)
        {
            var size = CalculateStaticSize(column.Width.Value, column.MinWidth, column.MaxWidth);
            column.ActualWidth = size;
            return size;
        }

        /// <summary>
        /// Calculates and updates the size of auto- and statically-sized rows.
        /// </summary>
        /// <param name="availableSize">The amount of space available for rows.</param>
        /// <param name="proportionalRowFactor">A value specifying the total number of proportionally sized units.</param>
        /// <returns>The amount of remaining space available for rows after accounting for auto- and statically-sized rows.</returns>
        private Int32 MeasureAutoAndStaticRows(Int32 availableSize, out Double proportionalRowFactor)
        {
            proportionalRowFactor = 0.0;

            for (int i = 0; i < RowDefinitions.Count; i++)
            {
                var row = RowDefinitions[i];
                if (row.Height.GridUnitType == GridUnitType.Star)
                {
                    proportionalRowFactor += row.Height.Value;
                }
                else
                {
                    availableSize -= MeasureAutoOrStaticRow(row, i);
                }
            }

            return Math.Max(0, availableSize);
        }

        /// <summary>
        /// Calculates the size of an Auto- or statically-sized row.
        /// </summary>
        /// <param name="row">The row for which to calculate a size.</param>
        /// <param name="ix">The index of the row within the grid.</param>
        /// <returns>The actual size of the row in pixels.</returns>
        private Int32 MeasureAutoOrStaticRow(RowDefinition row, Int32 ix)
        {
            switch (row.Height.GridUnitType)
            {
                case GridUnitType.Auto:
                    return MeasureAutoRow(row, ix);
                case GridUnitType.Pixel:
                    return MeasureStaticRow(row, ix);
            }
            return 0;
        }

        /// <summary>
        /// Calculates the size of an Auto-sized row.
        /// </summary>
        /// <param name="row">The row for which to calculate a size.</param>
        /// <param name="ix">The index of the row within the grid.</param>
        /// <returns>The actual size of the row in pixels.</returns>
        private Int32 MeasureAutoRow(RowDefinition row, Int32 ix)
        {
            var pxSize = 0;

            foreach (var child in Children)
            {
                if (!ElementParticipatesInLayout(child))
                    continue;

                if (GetRow(child) != ix)
                    continue;

                var size = CalculateChildSizeWithMargin(child);
                if (size.Height > pxSize)
                    pxSize = size.Height;
            }

            row.ActualHeight = pxSize;
            return pxSize;
        }

        /// <summary>
        /// Calculates the size of an statically-sized row.
        /// </summary>
        /// <param name="row">The row for which to calculate a size.</param>
        /// <param name="ix">The index of the row within the grid.</param>
        /// <returns>The actual size of the row in pixels.</returns>
        private Int32 MeasureStaticRow(RowDefinition row, Int32 ix)
        {
            var size = CalculateStaticSize(row.Height.Value, row.MinHeight, row.MaxHeight);
            row.ActualHeight = size;
            return size;
        }

        /// <summary>
        /// Calculates and updates the size of proportional columns.
        /// </summary>
        /// <param name="pxProportionalUnit">The size of the base unit (corresponding to 1*) for proportional columns, in pixels.</param>
        private void MeasureProportionalColumns(Int32 pxProportionalUnit)
        {
            foreach (var column in ColumnDefinitions)
            {
                if (column.Width.GridUnitType != GridUnitType.Star)
                    continue;

                column.ActualWidth = (Int32)(column.Width.Value * pxProportionalUnit);
            }
        }

        /// <summary>
        /// Calculates and updates the size of proportional rows.
        /// </summary>
        /// <param name="pxProportionalUnit">The size of the base unit (corresponding to 1*) for proportional rows, in pixels.</param>
        private void MeasureProportionalRows(Int32 pxProportionalUnit)
        {
            foreach (var row in RowDefinitions)
            {
                if (row.Height.GridUnitType != GridUnitType.Star)
                    continue;

                row.ActualHeight = (Int32)(row.Height.Value * pxProportionalUnit);
            }
        }

        /// <summary>
        /// Positions the grid's columns within the grid's content area.
        /// </summary>
        private void PositionColumns()
        {
            var cx = 0;
            foreach (var column in ColumnDefinitions)
            {
                column.GridRelativeX = cx;
                cx += column.ActualWidth;
            }
        }

        /// <summary>
        /// Positions the grid's rows within the grid's content area.
        /// </summary>
        private void PositionRows()
        {
            var cy = 0;
            foreach (var row in RowDefinitions)
            {
                row.GridRelativeY = cy;
                cy += row.ActualHeight;
            }
        }

        /// <summary>
        /// Lays out the specified element within its containing cell.
        /// </summary>
        /// <param name="child">The child element to lay out.</param>
        private void PerformContentLayout(UIElement child)
        {
            var ixCol = GetColumn(child);
            var ixRow = GetRow(child);

            var col = (ixCol >= ColumnDefinitions.Count) ? null : ColumnDefinitions[ixCol];
            var row = (ixRow >= RowDefinitions.Count)    ? null : RowDefinitions[ixRow];

            var colX = (col == null) ? 0 : col.GridRelativeX;
            var rowY = (row == null) ? 0 : row.GridRelativeY;

            var cellSize = GetCellSize(ixCol, ixRow);
            var cellArea = new Rectangle(colX, rowY, cellSize.Width, cellSize.Height);

            var margin = ConvertThicknessToPixels(child.Margin, 0);

            var pxActualWidth  = (Int32?)null;
            var pxActualHeight = (Int32?)null;
            child.CalculateActualSize(ref pxActualWidth, ref pxActualHeight);

            var childX          = 0;
            var childY          = 0;
            var childWidth      = pxActualWidth ?? cellSize.Width;
            var childHeight     = pxActualHeight ?? cellSize.Height;
            var childAreaWidth  = cellSize.Width - ((Int32)margin.Left + (Int32)margin.Right);
            var childAreaHeight = cellSize.Height - ((Int32)margin.Top + (Int32)margin.Bottom);

            switch (child.HorizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    childX = (Int32)(cellSize.Width - childWidth) / 2;
                    break;

                case HorizontalAlignment.Left:
                case HorizontalAlignment.Stretch:
                    childX = (Int32)margin.Left;
                    if (child.HorizontalAlignment == HorizontalAlignment.Stretch)
                    {
                        childWidth = childAreaWidth;
                    }
                    break;

                case HorizontalAlignment.Right:
                    childX = cellSize.Width - ((Int32)margin.Right + childWidth);
                    break;
            }

            switch (child.VerticalAlignment)
            {
                case VerticalAlignment.Center:
                    childY = (Int32)(cellSize.Height - childHeight) / 2;
                    break;

                case VerticalAlignment.Top:
                case VerticalAlignment.Stretch:
                    childY = (Int32)margin.Top;
                    if (child.VerticalAlignment == VerticalAlignment.Stretch)
                    {
                        childHeight = childAreaHeight;
                    }
                    break;
                
                case VerticalAlignment.Bottom:
                    childY = cellSize.Height - ((Int32)margin.Bottom + childHeight);
                    break;
            }

            child.ParentRelativeArea = new Rectangle(colX + childX, rowY + childY, childWidth, childHeight);
            child.RequestLayout();

            UpdateContentElementPosition(child);
        }

        /// <summary>
        /// Gets the size of the specified cell in device pixels.
        /// </summary>
        /// <param name="column">The index of the column that contains the cell.</param>
        /// <param name="row">The index of the row that contains the cell.</param>
        /// <returns>A <see cref="Size2"/> representing the size of the specified cell.</returns>
        private Size2 GetCellSize(Int32 column, Int32 row)
        {
            var rowDef =    row >= RowDefinitions.Count    ? null : RowDefinitions[row];
            var colDef = column >= ColumnDefinitions.Count ? null : ColumnDefinitions[column];

            var rowSize = (rowDef == null) ? ActualHeight : rowDef.ActualHeight;
            var colSize = (colDef == null) ? ActualWidth  : colDef.ActualWidth;

            return new Size2(colSize, rowSize);
        }

        /// <summary>
        /// Calculates the actual size of a row or column with static sizing.
        /// </summary>
        /// <param name="size">The row or column's specified size value.</param>
        /// <param name="min">The row or column's minimum size.</param>
        /// <param name="max">The row or column's maximum size.</param>
        /// <returns>The size of the row or column in pixels.</returns>
        private Int32 CalculateStaticSize(Double size, Double min, Double max)
        {
            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

            var clampedSize = size;

            if (clampedSize < min)
                clampedSize = min;

            if (clampedSize > max)
                clampedSize = max;

            if (clampedSize < 0)
                clampedSize = 0;

            return (Int32)display.DipsToPixels(clampedSize);
        }

        /// <summary>
        /// Calculates the size of the specified child element, including its margins.
        /// </summary>
        /// <param name="child">The child for which to calculate a size.</param>
        /// <returns>A <see cref="Size2"/> representing the specified child's size.</returns>
        private Size2 CalculateChildSizeWithMargin(UIElement child)
        {
            var margin = ConvertThicknessToPixels(child.Margin, 0);

            var pxChildWidth  = (Int32?)null;
            var pxChildHeight = (Int32?)null;
            child.CalculateActualSize(ref pxChildWidth, ref pxChildHeight);

            pxChildWidth  = (pxChildWidth ?? 0)  + (Int32)margin.Left + (Int32)margin.Right;
            pxChildHeight = (pxChildHeight ?? 0) + (Int32)margin.Top  + (Int32)margin.Bottom;

            return new Size2(pxChildWidth.Value, pxChildHeight.Value);
        }

        /// <summary>
        /// Gets the row at the specified relative position within the grid.
        /// </summary>
        /// <param name="x">The relative x-coordinate, in pixels, for which to retrieve a row index.</param>
        /// <param name="y">The relative y-coordinate, in pixels, for which to retrieve a row index.</param>
        /// <returns>The index of the row at the specified relative position.</returns>
        private Int32 GetRowAtPosition(Int32 x, Int32 y)
        {
            x -= ContentPanelX;
            y -= ContentPanelY;

            for (int i = 0; i < RowDefinitions.Count; i++)
            {
                var row = RowDefinitions[i];

                var top    = row.GridRelativeY;
                var bottom = row.GridRelativeY + row.ActualHeight;

                if (y >= top && y < bottom)
                    return i;
            }

            return 0;

        }

        /// <summary>
        /// Gets the columns at the specified relative position within the grid.
        /// </summary>
        /// <param name="x">The relative x-coordinate, in pixels, for which to retrieve a columns index.</param>
        /// <param name="y">The relative y-coordinate, in pixels, for which to retrieve a columns index.</param>
        /// <returns>The index of the columns at the specified relative position.</returns>
        private Int32 GetColumnAtPosition(Int32 x, Int32 y)
        {
            x -= ContentPanelX;
            y -= ContentPanelY;

            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                var column = ColumnDefinitions[i];

                var left  = column.GridRelativeX;
                var right = column.GridRelativeX + column.ActualWidth;

                if (x >= left && x < right)
                    return i;
            }

            return 0;
        }

        /// <summary>
        /// Updates the metadata for each of the grid's cells.
        /// </summary>
        private void UpdateCellMetadata()
        {
            if (cells.Length < RowCount * ColumnCount)
            {
                var temp = new CellMetadata[RowCount * ColumnCount];
                Array.Copy(cells, temp, cells.Length);

                for (int i = cells.Length; i < temp.Length; i++)
                    temp[i] = new CellMetadata();

                cells = temp;
            }

            var cx = 0;
            var cy = 0;

            for (int row = 0; row < RowCount; row++)
            {
                cx = 0;

                var rowHeight = (row >= RowDefinitions.Count) ? ActualHeight : RowDefinitions[row].ActualHeight;

                for (int col = 0; col < ColumnCount; col++)
                {
                    var colWidth = (col >= ColumnDefinitions.Count) ? ActualWidth : ColumnDefinitions[col].ActualWidth;

                    var cell       = cells[(row * ColumnCount) + col];
                    var cellWidth  = 0;
                    var cellHeight = 0;

                    cell.Elements.Clear();

                    foreach (var child in Children)
                    {
                        var childCol = GetColumn(child);
                        var childRow = GetRow(child);
                        if (childCol != col || childRow != row)
                            continue;

                        if (ElementIsDrawn(child))
                        {
                            var childSize = CalculateChildSizeWithMargin(child);

                            if (childSize.Width > cellWidth)
                                cellWidth = childSize.Width;

                            if (childSize.Height > cellHeight)
                                cellHeight = childSize.Height;
                        }

                        cell.Elements.Add(child);
                    }

                    cell.GridRelativeX = cx;
                    cell.GridRelativeY = cy;
                    cell.ActualWidth = colWidth;
                    cell.ActualHeight = rowHeight;
                    cell.RequiresScissorRectangle = cellWidth > colWidth || cellHeight > rowHeight;

                    cx += colWidth;
                }

                cy += rowHeight;
            }
        }

        // Property values.
        private readonly RowDefinitionCollection rowDefinitions;
        private readonly ColumnDefinitionCollection columnDefinitions;

        // Cached cell metadata.
        private CellMetadata[] cells = new[] { new CellMetadata() };
    }
}
