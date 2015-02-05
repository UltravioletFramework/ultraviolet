using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a grid of columns and columns which can contain child elements in each cell.
    /// </summary>
    [UIElement("Grid", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.Grid.xml")]
    public partial class Grid : Panel
    {
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
        /// Sets a value that indicates the total number of rows that the specified element spans within its parent grid.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="rowSpan">The total number of rows that the specified element spans within its parent grid.</param>
        public static void SetRowSpan(UIElement element, Int32 rowSpan)
        {
            Contract.Require(element, "element");

            element.SetValue<Int32>(RowSpanProperty, rowSpan);
        }

        /// <summary>
        /// Sets a value that indicates the total number of columns that the specified element spans within its parent grid.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="columnSpan">The total number of columns that the specified element spans within its parent grid.</param>
        public static void SetColumnSpan(UIElement element, Int32 columnSpan)
        {
            Contract.Require(element, "element");

            element.SetValue<Int32>(ColumnSpanProperty, columnSpan);
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

        /// <summary>
        /// Gets a value that indicates the total number of rows that the specified element spans within its parent grid.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The total number of rows that the specified element spans within its parent grid.</returns>
        public static Int32 GetRowSpan(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Int32>(RowSpanProperty);
        }

        /// <summary>
        /// Gets a value that indicates the total number of columns that the specified element spans within its parent grid.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The total number of columns that the specified element spans within its parent grid.</returns>
        public static Int32 GetColumnSpan(UIElement element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Int32>(ColumnSpanProperty);
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
        /// Identifies the <see cref="RowSpan"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RowSpanProperty = DependencyProperty.Register("RowSpan", typeof(Int32), typeof(Grid),
            new DependencyPropertyMetadata(null, () => 1, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="ColumnSpan"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSpanProperty = DependencyProperty.Register("ColumnSpan", typeof(Int32), typeof(Grid),
            new DependencyPropertyMetadata(null, () => 1, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected internal override void OnChildrenChanged()
        {
            UpdateVirtualCellMetadata();
            base.OnChildrenChanged();
        }

        /// <summary>
        /// Occurs when the grid's column definitions are modified.
        /// </summary>
        protected internal virtual void OnColumnsModified()
        {
            InvalidateMeasure();
            UpdateVirtualCellMetadata();
        }

        /// <summary>
        /// Occurs when the grid's column definitions are modified.
        /// </summary>
        protected internal virtual void OnRowsModified()
        {
            InvalidateMeasure();
            UpdateVirtualCellMetadata();
        }

        /// <inheritdoc/>
        protected override void DrawChildren(UltravioletTime time, DrawingContext dc)
        {
            /* NOTE:
             * By pre-emptively setting the clip region in the Grid itself, we can prevent
             * multiple batch flushes when drawing consecutive children within the same clip region.
             * This is because DrawingContext evaluates the current clip state and only performs
             * a flush if something has changed. */

            var clip = (RectangleD?)null;

            foreach (var child in Children)
            {
                if (child.ClipRectangle != clip)
                {
                    if (clip.HasValue)
                    {
                        clip = null;
                        dc.PopClipRectangle();
                    }

                    if (child.ClipRectangle.HasValue)
                    {
                        clip = child.ClipRectangle;
                        dc.PushClipRectangle(clip.Value);
                    }
                }

                child.Draw(time, dc);
            }

            if (clip.HasValue)
                dc.PopClipRectangle();

            base.DrawChildren(time, dc);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureContent(Size2D availableSize)
        {
            PrepareForMeasure(RowDefinitions);
            PrepareForMeasure(ColumnDefinitions);

            MeasureVirtualCells(0);

            if (CanResolveStarRows())
            {
                ResolveStars(RowDefinitions, availableSize.Height);
                MeasureVirtualCells(1);
                ResolveStars(ColumnDefinitions, availableSize.Width);
                MeasureVirtualCells(2);
            }
            else
            {
                if (CanResolveStarColumns())
                {
                    // NOTE: In this scenario there are no Priority 1 cells.
                    ResolveStars(ColumnDefinitions, availableSize.Width);
                    MeasureVirtualCells(2);
                    ResolveStars(RowDefinitions, availableSize.Height);
                }
                else
                {
                    MeasureVirtualCells(1, GridMeasurementOptions.AssumeInfiniteHeight);
                    ResolveStars(ColumnDefinitions, availableSize.Width);
                    MeasureVirtualCells(2);
                    ResolveStars(RowDefinitions, availableSize.Height);
                    MeasureVirtualCells(1, GridMeasurementOptions.DiscardDesiredWidth);
                }
            }

            MeasureVirtualCells(3);

            var desiredWidth = 0.0;
            var desiredHeight = 0.0;

            foreach (var column in ColumnDefinitions)
                desiredWidth += column.ActualWidth;

            foreach (var row in RowDefinitions)
                desiredHeight += row.ActualHeight;

            return new Size2D(desiredWidth, desiredHeight);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeContent(Size2D finalSize, ArrangeOptions options)
        {
            FinalizeDimension(ColumnDefinitions, finalSize.Width);
            FinalizeDimension(RowDefinitions, finalSize.Height);

            foreach (var cell in virtualCells)
            {
                var childElement = cell.Element;

                var childColumn = ColumnDefinitions[cell.ColumnIndex];
                var childRow    = RowDefinitions[cell.RowIndex];

                var childArea = new RectangleD(childColumn.Position, childRow.Position, 
                    CalculateSpanDimension(ColumnDefinitions, cell.ColumnIndex, cell.ColumnSpan),
                    CalculateSpanDimension(RowDefinitions, cell.RowIndex, cell.RowSpan));

                childElement.Arrange(childArea);
            }

            return finalSize;
        }

        /// <inheritdoc/>
        protected override void PositionContent(Point2D position)
        {
            foreach (var child in Children)
                child.Position(position);

            base.PositionContent(position);
        }

        /// <inheritdoc/>
        protected override UIElement GetChildAtPoint(Double x, Double y, Boolean isHitTest)
        {
            var col  = GetColumnAtPoint(x, y);
            var row  = GetRowAtPoint(x, y);

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                var child = Children[i];

                var childCol = GetColumn(child);
                var childColSpan = GetColumnSpan(child);

                if (col < childCol || col >= childCol + childColSpan)
                    continue;

                var childRow = GetRow(child);
                var childRowSpan = GetRowSpan(child);

                if (row < childRow || row >= childRow + childRowSpan)
                    continue;

                var childRelX = x - child.RelativeBounds.X;
                var childRelY = y - child.RelativeBounds.Y;

                var childMatch = child.GetElementAtPoint(childRelX, childRelY, isHitTest);
                if (childMatch != null)
                {
                    return childMatch;
                }
            }

            return null;
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
                grid.InvalidateMeasure();
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
                grid.InvalidateMeasure();
        }

        /// <summary>
        /// Gets the index of the row at the specified column in element space.
        /// </summary>
        /// <param name="x">The x-coordinate in element space to evaluate.</param>
        /// <param name="y">The y-coordinate in element space to evaluate.</param>
        /// <returns>The index of the column at the specified point in element space.</returns>
        private Int32 GetColumnAtPoint(Double x, Double y)
        {
            var position = 0.0;

            x -= RenderContentRegion.X;
            y -= RenderContentRegion.Y;

            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                var width = ColumnDefinitions[i].ActualWidth;
                if (x >= position && x < position + width)
                {
                    return i;
                }
                position += width;
            }

            return 0;
        }

        /// <summary>
        /// Gets the index of the row at the specified point in element space.
        /// </summary>
        /// <param name="x">The x-coordinate in element space to evaluate.</param>
        /// <param name="y">The y-coordinate in element space to evaluate.</param>
        /// <returns>The index of the row at the specified point in element space.</returns>
        private Int32 GetRowAtPoint(Double x, Double y)
        {
            var position = 0.0;

            x -= RenderContentRegion.X;
            y -= RenderContentRegion.Y;

            for (int i = 0; i < RowDefinitions.Count; i++)
            {
                var height = RowDefinitions[i].ActualHeight;
                if (y >= position && y < position + height)
                {
                    return i;
                }
                position += height;
            }

            return 0;
        }

        /// <summary>
        /// Measures the cells in the specified priority group.
        /// </summary>
        /// <param name="priority">The measurement priority group to measure.</param>
        private void MeasureVirtualCells(Int32 priority, GridMeasurementOptions options = GridMeasurementOptions.None)
        {
            if (!hasCellsOfPriority[priority])
                return;

            foreach (var cell in virtualCells)
            {
                if (cell.MeasurementPriority != priority)
                    continue;

                var childElement     = cell.Element;
                var childDesiredSize = childElement.DesiredSize;

                MeasureVirtualCell(cell, options);

                if ((options & GridMeasurementOptions.DiscardDesiredWidth) != GridMeasurementOptions.DiscardDesiredWidth)
                {
                    DistributeVirtualCellDimension(ColumnDefinitions, cell.ColumnIndex, cell.ColumnSpan, childElement.DesiredSize.Width);
                }

                if ((options & GridMeasurementOptions.DiscardDesiredHeight) != GridMeasurementOptions.DiscardDesiredHeight)
                {
                    DistributeVirtualCellDimension(RowDefinitions, cell.RowIndex, cell.RowSpan, childElement.DesiredSize.Height);
                }
            }
        }

        /// <summary>
        /// Measures the element contained by the specified virtual cell.
        /// </summary>
        /// <param name="cell">The virtual cell to measure.</param>
        /// <param name="options">The measurement options for this cell.</param>
        private void MeasureVirtualCell(VirtualCellMetadata cell, GridMeasurementOptions options)
        {
            UpdateVirtualCellMetadata();

            var cellWidth  = 0.0;
            var cellHeight = 0.0;

            if ((options & GridMeasurementOptions.AssumeInfiniteWidth) == GridMeasurementOptions.AssumeInfiniteWidth)
            {
                cellWidth = Double.PositiveInfinity;
            }
            else
            {
                // If we contain auto columns, then the content determines our width (so assume no constraint)
                if (cell.ContainsAutoColumns && !cell.ContainsStarColumns)
                {
                    cellWidth = Double.PositiveInfinity;
                }
                else
                {
                    cellWidth = CalculateSpanDimension(ColumnDefinitions, cell.ColumnIndex, cell.ColumnSpan);
                }
            }

            if ((options & GridMeasurementOptions.AssumeInfiniteHeight) == GridMeasurementOptions.AssumeInfiniteHeight)
            {
                cellHeight = Double.PositiveInfinity;
            }
            else
            {
                // If we contain auto rows, then the content determines our height (so assume no constraint)
                if (cell.ContainsAutoRows && !cell.ContainsStarRows)
                {
                    cellHeight = Double.PositiveInfinity;
                }
                else
                {
                    cellHeight = CalculateSpanDimension(RowDefinitions, cell.RowIndex, cell.RowSpan);
                }
            }

            var cellSize = new Size2D(cellWidth, cellHeight);
            cell.Element.Measure(cellSize);
        }

        /// <summary>
        /// Prepares the grid's rows or columns for measurement.
        /// </summary>
        /// <param name="definitions">The collection of row or column definitions to prepare for measurement.</param>
        private void PrepareForMeasure(IDefinitionBaseCollection definitions)
        {
            for (int i = 0; i < definitions.Count; i++)
            {
                var def = definitions[i];
                def.MeasuredDimension = Double.PositiveInfinity;
                def.ResetContentDimension();

                if (def.Dimension.GridUnitType == GridUnitType.Pixel)
                {
                    def.MeasuredDimension = def.Dimension.Value;
                }
            }
        }

        /// <summary>
        /// Distributes the specified dimension amongst all of the cells in the specified row or column span.
        /// </summary>
        /// <param name="definitions">The collection of row or column definitions amongst which to distribute the specified dimension.</param>
        /// <param name="index">The index of the first row or column in the span.</param>
        /// <param name="span">The number of rows or columns in the span.</param>
        /// <param name="dimension">The amount of space to distribute between the rows or columns in the span.</param>
        private void DistributeVirtualCellDimension(IDefinitionBaseCollection definitions, Int32 index, Int32 span, Double dimension)
        {
            if (span == 1)
            {
                definitions[index].ExpandContentDimension(dimension);
            }
            else
            {
                // TODO
            }
        }

        /// <summary>
        /// Measures the total dimension of the specified row or column span.
        /// </summary>
        /// <param name="definitions">The collection of row or column definitions for which to calculate a total dimension.</param>
        /// <param name="index">The index of the first row or column in the span.</param>
        /// <param name="span">The number of rows or columns in the span.</param>
        /// <returns>The total dimension of the specified row or column span.</returns>
        private Double CalculateSpanDimension(IDefinitionBaseCollection definitions, Int32 index, Int32 span)
        {
            var size = 0.0;

            for (int i = 0; i < span; i++)
            {
                size += definitions[index + i].FinalDimension;
            }

            return size;
        }

        /// <summary>
        /// Measures the total amount of space which is occupied by non-star rows or columns in the specified collection.
        /// </summary>
        /// <param name="definitions">The collection of row or column definitions for which to calculate a used dimension.</param>
        /// <returns>The total amount of space which is occupied by non-star columns in <paramref name="definitions"/>.</returns>
        private Double CalculateUsedDimension(IDefinitionBaseCollection definitions)
        {
            var dimension = 0.0;

            for (int i = 0; i < definitions.Count; i++)
            {
                var def = definitions[i];

                switch (def.Dimension.GridUnitType)
                {
                    case GridUnitType.Auto:
                    case GridUnitType.Pixel:
                        dimension += def.FinalDimension;
                        break;
                }
            }

            return dimension;
        }

        /// <summary>
        /// Gets a value indicating whether the grid can resolve the width of its star-sized columns.
        /// </summary>
        /// <returns><c>true</c> if the columns can be resolved; otherwise, <c>false</c>.</returns>
        private Boolean CanResolveStarColumns()
        {
            if (starColumnCount == 0)
                return true;

            foreach (var cell in virtualCells)
            {
                if (cell.MeasurementPriority == 1)
                    return false;

            }
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the grid can resolve the height of its star-sized rows.
        /// </summary>
        /// <returns><c>true</c> if the rows can be resolved; otherwise, <c>false</c>.</returns>
        private Boolean CanResolveStarRows()
        {
            if (starRowCount == 0)
                return true;

            foreach (var cell in virtualCells)
            {
                if (cell.MeasurementPriority == 2 && cell.ContainsAutoRows)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Resolves the size of the grid's star-sized rows or columns.
        /// </summary>
        /// <param name="definitions">The collection of rows or columns to resolve.</param>
        /// <param name="availableDimension">The amount of space which is available to the grid.</param>
        private void ResolveStars(IDefinitionBaseCollection definitions, Double availableDimension)
        {
            if ((definitions == RowDefinitions && starRowCount == 0) ||
                (definitions == ColumnDefinitions && starColumnCount == 0))
            {
                return;
            }

            availableDimension -= CalculateUsedDimension(definitions);
            var sumOfStarFactors = 0.0;

            for (int i = 0; i < definitions.Count; i++)
            {
                var def = definitions[i];
                if (def.Dimension.GridUnitType != GridUnitType.Star)
                    continue;

                sumOfStarFactors += def.Dimension.Value;
            }

            var starFactorUnit = (sumOfStarFactors == 0) ? 0 : availableDimension / sumOfStarFactors;

            for (int i = 0; i < definitions.Count; i++)
            {
                var def = definitions[i];
                if (def.Dimension.GridUnitType != GridUnitType.Star)
                    continue;

                def.MeasuredDimension = starFactorUnit * def.Dimension.Value;
            }
        }

        /// <summary>
        /// Finalizes the dimension of the grid's rows or columns during the arrange phase.
        /// </summary>
        /// <param name="dimension">The collection of row or column definitions to finalize.</param>
        /// <param name="definitions">The grid's final arranged dimension.</param>
        private void FinalizeDimension(IDefinitionBaseCollection definitions, Double dimension)
        {
            ResolveStars(definitions, dimension);

            var position = 0.0;

            for (int i = 0; i < definitions.Count; i++)
            {
                var def = definitions[i];
                def.Position = position;
                position += def.FinalDimension;
            }
        }

        /// <summary>
        /// Updates the metadata for each of the grid's virtual cells.
        /// </summary>
        private void UpdateVirtualCellMetadata()
        {
            starColumnCount = 0;
            starRowCount    = 0;

            for (int i = 0; i < hasCellsOfPriority.Length; i++)
                hasCellsOfPriority[i] = false;

            ExpandVirtualCellMetadataArray();

            for (int i = 0; i < Children.Count; i++)
            {
                var element = Children[i];
                var cell    = virtualCells[i];

                cell.Element     = element;

                cell.RowIndex    = GetRow(element);
                cell.RowSpan     = GetRowSpan(element);
                cell.ColumnIndex = GetColumn(element);
                cell.ColumnSpan  = GetColumnSpan(element);

                cell.ContainsAutoRows    = false;
                cell.ContainsStarRows    = false;
                cell.ContainsAutoColumns = false;
                cell.ContainsStarColumns = false;

                for (int col = 0; col < cell.ColumnSpan; col++)
                {
                    var colUnit = ColumnDefinitions[cell.ColumnIndex + col].Width.GridUnitType;
                    switch (colUnit)
                    {
                        case GridUnitType.Auto:
                            cell.ContainsAutoColumns = true;
                            break;

                        case GridUnitType.Star:
                            cell.ContainsStarColumns = true;
                            starColumnCount++;
                            break;
                    }
                }

                for (int row = 0; row < cell.RowSpan; row++)
                {
                    var rowUnit = RowDefinitions[cell.RowIndex + row].Height.GridUnitType;
                    switch (rowUnit)
                    {
                        case GridUnitType.Auto:
                            cell.ContainsAutoRows = true;
                            break;

                        case GridUnitType.Star:
                            cell.ContainsStarRows = true;
                            starRowCount++;
                            break;
                    }
                }

                cell.UpdateMeasurementPriority();

                hasCellsOfPriority[cell.MeasurementPriority] = true;
            }
        }

        /// <summary>
        /// If necessary, expands the virtual cell metadata array to accomodate all of the grid's virtual cells.
        /// </summary>
        private void ExpandVirtualCellMetadataArray()
        {
            if (virtualCells != null && virtualCells.Length == Children.Count)
                return;

            virtualCells = new VirtualCellMetadata[Children.Count];
            for (int i = 0; i < virtualCells.Length; i++)
            {
                virtualCells[i]         = new VirtualCellMetadata();
                virtualCells[i].Element = Children[i];
            }
        }

        // Property values.
        private readonly RowDefinitionCollection rowDefinitions;
        private readonly ColumnDefinitionCollection columnDefinitions;

        // Cached cell metadata.
        private VirtualCellMetadata[] virtualCells;
        private Int32 starColumnCount;
        private Int32 starRowCount;
        private readonly Boolean[] hasCellsOfPriority = new Boolean[4];
    }
}
