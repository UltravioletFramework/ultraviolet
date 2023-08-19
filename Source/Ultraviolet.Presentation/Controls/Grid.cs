using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a grid of columns and columns which can contain child elements in each cell.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.Grid.xml")]
    public partial class Grid : Panel
    {
        /// <summary>
        /// Initializes the <see cref="Grid"/> type.
        /// </summary>
        static Grid()
        {
            cmpByPreferredDesiredDimension = new FunctorComparer<DefinitionBase>((def1, def2) =>
            {
                return def1.PreferredDesiredDimension.CompareTo(def2.PreferredDesiredDimension);
            });

            cmpByMaxDimension = new FunctorComparer<DefinitionBase>((def1, def2) =>
            {
                return def1.MaxDimension.CompareTo(def2.MaxDimension);
            });

            cmpByMeasuredContentDimension = new FunctorComparer<DefinitionBase>((def1, def2) =>
            {
                return def1.MeasuredContentDimension.CompareTo(def2.MeasuredContentDimension);
            });

            cmpByMinArrangedSize = new FunctorComparer<DefinitionBase>((def1, def2) =>
            {
                var def1Min = Math.Max(def1.MinDimension, def1.MeasuredContentDimension);
                var def2Min = Math.Max(def2.MinDimension, def2.MeasuredContentDimension);
                return def2Min.CompareTo(def1Min);
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Grid(UltravioletContext uv, String name)
            : base(uv, name)
        {
            this.rowDefinitions    = new RowDefinitionCollection(this);
            this.columnDefinitions = new ColumnDefinitionCollection(this);
        }

        /// <summary>
        /// Sets a value that indicates which row a specified content element should occupy within its <see cref="Grid"/> container.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="row">The index of the row that the element should occupy.</param>
        public static void SetRow(DependencyObject element, Int32 row)
        {
            Contract.Require(element, nameof(element));

            element.SetValue<Int32>(RowProperty, row);
        }

        /// <summary>
        /// Sets a value that indicates which column a specified content element should occupy within its <see cref="Grid"/> container.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="column">The index of the column that the element should occupy.</param>
        public static void SetColumn(DependencyObject element, Int32 column)
        {
            Contract.Require(element, nameof(element));

            element.SetValue<Int32>(ColumnProperty, column);
        }

        /// <summary>
        /// Sets a value that indicates the total number of rows that the specified element spans within its parent grid.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="rowSpan">The total number of rows that the specified element spans within its parent grid.</param>
        public static void SetRowSpan(DependencyObject element, Int32 rowSpan)
        {
            Contract.Require(element, nameof(element));

            element.SetValue<Int32>(RowSpanProperty, rowSpan);
        }

        /// <summary>
        /// Sets a value that indicates the total number of columns that the specified element spans within its parent grid.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="columnSpan">The total number of columns that the specified element spans within its parent grid.</param>
        public static void SetColumnSpan(DependencyObject element, Int32 columnSpan)
        {
            Contract.Require(element, nameof(element));

            element.SetValue<Int32>(ColumnSpanProperty, columnSpan);
        }

        /// <summary>
        /// Gets a value that indicates which row a specified content element should occupy within its <see cref="Grid"/> container.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The index of the row that the element should occupy.</returns>
        public static Int32 GetRow(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Int32>(RowProperty);
        }

        /// <summary>
        /// Gets a value that indicates which column a specified content element should occupy within its <see cref="Grid"/> container.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The index of the column that the element should occupy.</returns>
        public static Int32 GetColumn(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Int32>(ColumnProperty);
        }

        /// <summary>
        /// Gets a value that indicates the total number of rows that the specified element spans within its parent grid.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The total number of rows that the specified element spans within its parent grid.</returns>
        public static Int32 GetRowSpan(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Int32>(RowSpanProperty);
        }

        /// <summary>
        /// Gets a value that indicates the total number of columns that the specified element spans within its parent grid.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The total number of columns that the specified element spans within its parent grid.</returns>
        public static Int32 GetColumnSpan(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

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
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.Grid.Row"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.Grid.Row"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the index of the <see cref="Grid"/> row that contains the element.
        /// </summary>
        /// <value>An <see cref="Int32"/> which represents the index of the row that contains the element.
        /// The default value is 0.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="RowProperty"/></dpropField>
        ///     <dpropStylingName>row</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty RowProperty = DependencyProperty.RegisterAttached("Row", typeof(Int32), typeof(Grid),
            new PropertyMetadata<Int32>(null, PropertyMetadataOptions.AffectsMeasure, HandleRowChanged));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.Grid.Column"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.Grid.Column"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the index of the <see cref="Grid"/> column that contains the element.
        /// </summary>
        /// <value>An <see cref="Int32"/> that represents the index of the column that contains the element.
        /// The default value is 0.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ColumnProperty"/></dpropField>
        ///     <dpropStylingName>column</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty ColumnProperty = DependencyProperty.RegisterAttached("Column", typeof(Int32), typeof(Grid),
            new PropertyMetadata<Int32>(null, PropertyMetadataOptions.AffectsMeasure, HandleColumnChanged));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.Grid.RowSpan"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.Grid.RowSpan"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the number of rows that are spanned by the specified child of a <see cref="Grid"/>.
        /// </summary>
        /// <value>An <see cref="Int32"/> that represents the number of rows that are spanned by the element.
        /// The default value is 1.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="RowSpanProperty"/></dpropField>
        ///     <dpropStylingName>row-span</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty RowSpanProperty = DependencyProperty.RegisterAttached("RowSpan", typeof(Int32), typeof(Grid),
            new PropertyMetadata<Int32>(CommonBoxedValues.Int32.One));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.Grid.ColumnSpan"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.Grid.ColumnSpan"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the number of columns that are spanned by the specified child of a <see cref="Grid"/>.
        /// </summary>
        /// <value>An <see cref="Int32"/> that represents the number of columns that are spanned by the element.
        /// The default value is 1.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ColumnSpanProperty"/></dpropField>
        ///     <dpropStylingName>column-span</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty ColumnSpanProperty = DependencyProperty.RegisterAttached("ColumnSpan", typeof(Int32), typeof(Grid),
            new PropertyMetadata<Int32>(CommonBoxedValues.Int32.One));

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
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            /* NOTE:
             * By pre-emptively setting the clip region in the Grid itself, we can prevent
             * multiple batch flushes when drawing consecutive children within the same clip region.
             * This is because DrawingContext evaluates the current clip state and only performs
             * a flush if something has changed. */

            var clip = (RectangleD?)null;

            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children.GetByZOrder(i);
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
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var infiniteHeight = Double.IsPositiveInfinity(availableSize.Height);
            var infiniteWidth  = Double.IsPositiveInfinity(availableSize.Width);

            PrepareForMeasure(RowDefinitions, infiniteHeight);
            PrepareForMeasure(ColumnDefinitions, infiniteWidth);

            UpdateVirtualCellMetadata();

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

            RoundDimensions(ColumnDefinitions);
            RoundDimensions(RowDefinitions);

            var desiredWidth = 0.0;
            var desiredHeight = 0.0;

            foreach (var column in ColumnDefinitions)
                desiredWidth += column.MinimumDesiredDimension;

            foreach (var row in RowDefinitions)
                desiredHeight += row.MinimumDesiredDimension;

            return new Size2D(desiredWidth, desiredHeight);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var rtl = (FlowDirection == FlowDirection.RightToLeft);

            PrepareForArrange(ColumnDefinitions);
            PrepareForArrange(RowDefinitions);

            FinalizeDimension(ColumnDefinitions, finalSize.Width, FlowDirection == FlowDirection.RightToLeft);
            FinalizeDimension(RowDefinitions, finalSize.Height, false);

            foreach (var cell in virtualCells)
            {
                var flowedColumnIndex = cell.ColumnIndex + (rtl ? cell.ColumnSpan - 1 : 0);

                var childElement = cell.Element;
                var childColumn = ColumnDefinitions[flowedColumnIndex];
                var childRow = RowDefinitions[cell.RowIndex];

                var childArea = new RectangleD(childColumn.Position, childRow.Position,
                    Math.Max(0, CalculateSpanDimension(ColumnDefinitions, cell.ColumnIndex, cell.ColumnSpan)),
                    Math.Max(0, CalculateSpanDimension(RowDefinitions, cell.RowIndex, cell.RowSpan)));

                childElement.Arrange(childArea);
            }

            return finalSize;
        }

        /// <inheritdoc/>
        protected override Visual HitTestCore(Point2D point)
        {
            if (!HitTestUtil.IsPotentialHit(this, point))
                return null;

            var childMatch = HitTestChildren(point);
            if (childMatch != null)
            {
                return childMatch;
            }

            return null;
        }

        /// <inheritdoc/>
        protected override Visual HitTestChildren(Point2D point)
        {
            var col = GetColumnAtPoint(point);
            var row = GetRowAtPoint(point);
            if (row < 0 || col < 0)
                return null;

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                var child = Children.GetByZOrder(i);

                var childCol = GetColumn(child);
                var childColSpan = GetColumnSpan(child);

                if (col < childCol || col >= childCol + childColSpan)
                    continue;

                var childRow = GetRow(child);
                var childRowSpan = GetRowSpan(child);

                if (row < childRow || row >= childRow + childRowSpan)
                    continue;

                var childMatch = child.HitTest(TransformToDescendant(child, point));
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
        private static void HandleRowChanged(DependencyObject dobj, Int32 oldValue, Int32 newValue)
        {
            var element = (UIElement)dobj;

            var grid = element.Parent as Grid;
            if (grid != null)
                grid.InvalidateMeasure();
        }

        /// <summary>
        /// Occurs when the value of the Column attached property changes.
        /// </summary>
        private static void HandleColumnChanged(DependencyObject dobj, Int32 oldValue, Int32 newValue)
        {
            var element = (UIElement)dobj;

            var grid = element.Parent as Grid;
            if (grid != null)
                grid.InvalidateMeasure();
        }

        /// <summary>
        /// Gets the number of definitions in the specified collection with a unit
        /// type of <see cref="GridUnitType.Star"/>.
        /// </summary>
        private Int32 CountStars(IDefinitionBaseCollection definitions)
        {
            var count = 0;

            if (definitions != null)
            {
                for (int i = 0; i < definitions.Count; i++)
                {
                    var def = definitions[i];
                    if (def.AssumedUnitType == GridUnitType.Star)
                        count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Gets the index of the row at the specified column in element space.
        /// </summary>
        /// <param name="point">The point in element space to evaluate.</param>
        /// <returns>The index of the column at the specified point in element space.</returns>
        private Int32 GetColumnAtPoint(Point2D point)
        {
            var rtl = (FlowDirection == FlowDirection.RightToLeft);
            if (rtl)
            {
                var position = RenderSize.Width;
                for (int i = 0; i < ColumnDefinitions.Count; i++)
                {
                    var width = ColumnDefinitions[i].ActualWidth;
                    if (point.X >= position - width && point.X < position + width)
                    {
                        return i;
                    }
                    position -= width;
                }
            }
            else
            {
                var position = 0.0;
                for (int i = 0; i < ColumnDefinitions.Count; i++)
                {
                    var width = ColumnDefinitions[i].ActualWidth;
                    if (point.X >= position && point.X < position + width)
                    {
                        return i;
                    }
                    position += width;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the index of the row at the specified point in element space.
        /// </summary>
        /// <param name="point">The point in element space to evaluate.</param>
        /// <returns>The index of the row at the specified point in element space.</returns>
        private Int32 GetRowAtPoint(Point2D point)
        {
            var position = 0.0;

            for (int i = 0; i < RowDefinitions.Count; i++)
            {
                var height = RowDefinitions[i].ActualHeight;
                if (point.Y >= position && point.Y < position + height)
                {
                    return i;
                }
                position += height;
            }

            return -1;
        }

        /// <summary>
        /// Measures the cells in the specified priority group.
        /// </summary>
        /// <param name="priority">The measurement priority group to measure.</param>
        /// <param name="options">The measurement options for this priority group.</param>
        private void MeasureVirtualCells(Int32 priority, GridMeasurementOptions options = GridMeasurementOptions.None)
        {
            if (!hasCellsOfPriority[priority])
                return;

            var discardDesiredWidth  = ((options & GridMeasurementOptions.DiscardDesiredWidth) == GridMeasurementOptions.DiscardDesiredWidth);
            var discardDesiredHeight = ((options & GridMeasurementOptions.DiscardDesiredHeight) == GridMeasurementOptions.DiscardDesiredHeight);

            foreach (var cell in virtualCells)
            {
                if (cell.MeasurementPriority != priority)
                    continue;

                MeasureVirtualCell(cell, options);

                if (!discardDesiredWidth && cell.ColumnSpan == 1)
                    ColumnDefinitions[cell.ColumnIndex].ExpandContentDimension(cell.Element.DesiredSize.Width);

                if (!discardDesiredHeight && cell.RowSpan == 1)
                    RowDefinitions[cell.RowIndex].ExpandContentDimension(cell.Element.DesiredSize.Height);
            }

            foreach (var cell in virtualCells)
            {
                if (!discardDesiredWidth && cell.ColumnSpan > 1)
                    DistributeSpanDimension(ColumnDefinitions, cell.ColumnIndex, cell.ColumnSpan, cell.Element.DesiredSize.Width);

                if (!discardDesiredHeight && cell.RowSpan > 1)
                    DistributeSpanDimension(RowDefinitions, cell.RowIndex, cell.RowSpan, cell.Element.DesiredSize.Height);
            }
        }

        /// <summary>
        /// Measures the element contained by the specified virtual cell.
        /// </summary>
        /// <param name="cell">The virtual cell to measure.</param>
        /// <param name="options">The measurement options for this cell.</param>
        private void MeasureVirtualCell(VirtualCellMetadata cell, GridMeasurementOptions options)
        {
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
        /// <param name="infiniteDimension">A value indicating whether the constraint for this dimension is infinite.</param>
        private void PrepareForMeasure(IDefinitionBaseCollection definitions, Boolean infiniteDimension)
        {
            for (int i = 0; i < definitions.Count; i++)
            {
                var def = definitions[i];

                var dim    = 0.0;
                var dimMin = def.MinDimension;
                var dimMax = Math.Max(dimMin, def.MaxDimension);

                switch (def.Dimension.GridUnitType)
                {
                    case GridUnitType.Auto:
                        def.AssumedUnitType = GridUnitType.Auto;
                        dim                 = Double.PositiveInfinity;
                        break;

                    case GridUnitType.Pixel:
                        def.AssumedUnitType   = GridUnitType.Pixel;
                        dim                   = def.Dimension.Value;
                        dimMin                = Math.Max(dimMin, Math.Min(dim, dimMax));
                        break;

                    case GridUnitType.Star:
                        def.AssumedUnitType = infiniteDimension ? GridUnitType.Auto : GridUnitType.Star;
                        dim                 = Double.PositiveInfinity;
                        break;
                }

                def.ResetContentDimension(dimMin);
                def.MeasuredDimension = Math.Max(dimMin, Math.Min(dim, dimMax));
            }
        }

        /// <summary>
        /// Prepares the grid's rows or columns for arrangement.
        /// </summary>
        /// <param name="definitions">The collection of row or column definitions to prepare for measurement.</param>
        private void PrepareForArrange(IDefinitionBaseCollection definitions)
        {
            for (int i = 0; i < definitions.Count; i++)
            {
                var def = definitions[i];
                def.AssumedUnitType = def.Dimension.GridUnitType;
            }
        }

        /// <summary>
        /// Distributes the specified dimension amongst all of the cells in the specified row or column span.
        /// </summary>
        /// <param name="definitions">The collection of row or column definitions amongst which to distribute the specified dimension.</param>
        /// <param name="index">The index of the first row or column in the span.</param>
        /// <param name="span">The number of rows or columns in the span.</param>
        /// <param name="dimension">The amount of space to distribute between the rows or columns in the span.</param>
        private void DistributeSpanDimension(IDefinitionBaseCollection definitions, Int32 index, Int32 span, Double dimension)
        {
            if (dimension == 0.0)
                return;

            var spanContentDimension = 0.0;
            var spanDesiredDimension = 0.0;
            var spanMaximumDimension = 0.0;

            for (int i = 0; i < span; i++)
            {
                var def = definitions[index + i];

                spanContentDimension += def.MeasuredContentDimension;
                spanDesiredDimension += def.PreferredDesiredDimension;
                spanMaximumDimension += Math.Max(def.MaxDimension, spanContentDimension);
            }

            /* When the dimension to distribute is less than the current content 
             * size of the span, we don't need to do anything. */
            if (dimension <= spanContentDimension)
                return;

            /* Dimension to distribute is less than the desired dimension of the span.
             * Ignore auto defs, distribute equally into content sizes of other defs in order 
             * of lowest DesiredDimension to highest. */
            if (dimension <= spanDesiredDimension)
            {
                var undistributed = dimension;

                for (int i = 0; i < span; i++)
                {
                    var def = definitions[index + i];
                    if (def.AssumedUnitType == GridUnitType.Auto)
                        undistributed -= def.MeasuredContentDimension;
                }

                var defsInSpan = EnumerateNonAutoDefinitionsInSpanByDesiredDimension(definitions, index, span);
                for (int i = 0; i < defsInSpan.Count; i++)
                {
                    var def             = defsInSpan[i];
                    var defDistribution = undistributed / (defsInSpan.Count - i);
                    var defContentSize  = Math.Min(defDistribution, def.PreferredDesiredDimension);

                    def.ExpandContentDimension(defContentSize);

                    undistributed -= defContentSize;
                }
                defsInSpan.Clear();

                return;
            }

            /* Dimension to distribute is less than the maximum dimension of the span.
             * Distribute into non-auto defs first, in order of lowest MaxSize to highest,
             * then distribute any remaining space into auto defs. */
            if (dimension <= spanMaximumDimension)
            {
                var undistributed = dimension - spanDesiredDimension;

                var defsInSpan = EnumerateNonAutoDefinitionsInSpanByMaxDimension(definitions, index, span);
                var autoInSpan = span - defsInSpan.Count;

                // Non-auto defs
                for (int i = 0; i < defsInSpan.Count; i++)
                {
                    var def             = defsInSpan[i];
                    var defDesiredSize  = def.PreferredDesiredDimension;
                    var defDistribution = defDesiredSize + undistributed / (span - autoInSpan - i);
                    var defContentSize  = Math.Min(defDistribution, def.MaxDimension);

                    def.ExpandContentDimension(defContentSize);

                    var delta = def.MeasuredContentDimension - defDesiredSize;
                    undistributed -= delta;
                }

                // Auto defs
                defsInSpan = EnumerateAutoDefinitionsInSpanByContentDimension(definitions, index, span);
                for (int i = 0; i < defsInSpan.Count; i++)
                {
                    var def = defsInSpan[i];
                    var defDesiredSize = def.PreferredDesiredDimension;
                    var defDistribution = defDesiredSize + undistributed / (autoInSpan - i);
                    var defContentSize = Math.Min(defDistribution, def.MaxDimension);

                    def.ExpandContentDimension(defContentSize);

                    var delta = def.MeasuredContentDimension - defDesiredSize;
                    undistributed -= delta;
                }

                return;
            }

            /* Dimension to distribute is greater than the max size of the span.
             * Distribute equally into all spans. */
            for (int i = 0; i < span; i++)
            {
                definitions[index + i].ExpandContentDimension(dimension / span);
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
                size += definitions[index + i].ActualDimension;
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

                switch (def.AssumedUnitType)
                {
                    case GridUnitType.Auto:
                    case GridUnitType.Pixel:
                        dimension += def.ActualDimension;
                        break;
                }
            }

            return dimension;
        }

        /// <summary>
        /// Gets a value indicating whether the grid can resolve the width of its star-sized columns.
        /// </summary>
        /// <returns><see langword="true"/> if the columns can be resolved; otherwise, <see langword="false"/>.</returns>
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
        /// <returns><see langword="true"/> if the rows can be resolved; otherwise, <see langword="false"/>.</returns>
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
                if (def.AssumedUnitType != GridUnitType.Star)
                    continue;

                sumOfStarFactors += def.Dimension.Value;
            }

            var starFactorUnit = (sumOfStarFactors == 0) ? 0 : availableDimension / sumOfStarFactors;

            for (int i = 0; i < definitions.Count; i++)
            {
                var def = definitions[i];
                if (def.AssumedUnitType != GridUnitType.Star)
                    continue;

                def.MeasuredDimension = Math.Max(0, starFactorUnit * def.Dimension.Value);
            }
        }

        /// <summary>
        /// Finalizes the dimensions of the grid's star rows or columns.
        /// </summary>
        /// <param name="dimension">The collection of row or column definitions to finalize.</param>
        /// <param name="definitions">The grid's final arranged dimension.</param>
        private void FinalizeStars(IDefinitionBaseCollection definitions, Double dimension)
        {
            var undistributed = dimension - CalculateUsedDimension(definitions);

            var starCombinedWeight = 0.0;

            for (int i = 0; i < definitions.Count; i++)
            {
                var def = definitions[i];
                if (def.AssumedUnitType == GridUnitType.Star)
                {
                    starCombinedWeight += def.Dimension.Value;
                }
            }

            if (starCombinedWeight <= 0)
                return;

            var starCountVisited = 0.0;

            var buffer = EnumerateDefinitionsByMinArrangedSize(definitions);
            for (int i = 0; i < buffer.Count; i++)
            {
                var def = buffer[i];
                if (def.AssumedUnitType != GridUnitType.Star)
                    continue;

                var defShare = def.Dimension.Value * (undistributed / (starCombinedWeight - starCountVisited));
                var defMinSize = Math.Max(def.MinDimension, def.MeasuredContentDimension);
                var defDimension = Math.Max(defMinSize, Math.Max(0, Math.Min(defShare, def.MaxDimension)));
                def.MeasuredDimension = Math.Min(undistributed, defDimension);

                undistributed -= defDimension;

                starCountVisited += def.Dimension.Value;
            }
            buffer.Clear();
        }

        /// <summary>
        /// Finalizes the dimension of the grid's rows or columns during the arrange phase.
        /// </summary>
        /// <param name="dimension">The collection of row or column definitions to finalize.</param>
        /// <param name="definitions">The grid's final arranged dimension.</param>
        /// <param name="reverse">A value indicating whether to reverse the order of the rows/columns.</param>
        private void FinalizeDimension(IDefinitionBaseCollection definitions, Double dimension, Boolean reverse)
        {
            FinalizeStars(definitions, dimension);

            if (reverse)
            {
                var position = dimension;
                for (int i = 0; i < definitions.Count; i++)
                {
                    var def = definitions[i];
                    position -= def.ActualDimension;
                    def.Position = position;
                }
            }
            else
            {
                var position = 0.0;
                for (int i = 0; i < definitions.Count; i++)
                {
                    var def = definitions[i];
                    def.Position = position;
                    position += def.ActualDimension;
                }
            }
        }
        
        /// <summary>
        /// Performs layout rounding on the specified definition collection.
        /// </summary>
        /// <param name="definitions">The definition collection on which to perform layout rounding.</param>
        private void RoundDimensions(IDefinitionBaseCollection definitions)
        {
            if (definitions.Count == 0)
                return;

            var errorContent = 0.0;
            var error = 0.0;

            for (int i = 0; i < definitions.Count - 1; i++)
            {
                var oldValueContent = definitions[i].MeasuredContentDimension;
                var oldValue = definitions[i].MeasuredDimension;

                if (!Double.IsInfinity(oldValueContent))
                {
                    definitions[i].MeasuredContentDimension = PerformLayoutRounding(oldValueContent);
                    errorContent += (definitions[i].MeasuredContentDimension - oldValueContent);
                }

                if (!Double.IsInfinity(oldValue))
                {
                    definitions[i].MeasuredDimension = PerformLayoutRounding(oldValue);
                    error += (definitions[i].MeasuredDimension - oldValue);
                }
            }

            definitions[definitions.Count - 1].MeasuredContentDimension += errorContent;
            definitions[definitions.Count - 1].MeasuredDimension += error;
        }

        /// <summary>
        /// Updates the metadata for each of the grid's virtual cells.
        /// </summary>
        private void UpdateVirtualCellMetadata()
        {
            starColumnCount = CountStars(ColumnDefinitions);
            starRowCount    = CountStars(RowDefinitions);            

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
                    var colUnit = ColumnDefinitions[cell.ColumnIndex + col].AssumedUnitType;
                    switch (colUnit)
                    {
                        case GridUnitType.Auto:
                            cell.ContainsAutoColumns = true;
                            break;

                        case GridUnitType.Star:
                            cell.ContainsStarColumns = true;
                            break;
                    }
                }

                for (int row = 0; row < cell.RowSpan; row++)
                {
                    var rowUnit = RowDefinitions[cell.RowIndex + row].AssumedUnitType;
                    switch (rowUnit)
                    {
                        case GridUnitType.Auto:
                            cell.ContainsAutoRows = true;
                            break;

                        case GridUnitType.Star:
                            cell.ContainsStarRows = true;
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

        /// <summary>
        /// Retrieves a buffer containing all of the row/column definitions in the specified span.
        /// </summary>
        /// <param name="definitions">The collection of definitions from which to retrieve the span.</param>
        /// <param name="index">The index of the first definition in the span.</param>
        /// <param name="span">The number of definitions in the span.</param>
        /// <param name="auto">A value indicating whether to include auto-sized definitions.</param>
        /// <param name="nonauto">A value indicating whether to include non-auto-sized definitions.</param>
        /// <returns>A buffer containing the specified span of row/column definitions.</returns>
        private List<DefinitionBase> EnumerateDefinitionsInSpan(IDefinitionBaseCollection definitions, Int32 index, Int32 span, 
            Boolean auto = true, Boolean nonauto = true)
        {
            spanEnumerationBuffer.Clear();

            for (int i = 0; i < span; i++)
            {
                var def = definitions[index + i];
                switch (def.AssumedUnitType)
                {
                    case GridUnitType.Auto:
                        if (auto)
                        {
                            spanEnumerationBuffer.Add(def);
                        }
                        break;

                    case GridUnitType.Pixel:
                    case GridUnitType.Star:
                        if (nonauto)
                        {
                            spanEnumerationBuffer.Add(def);
                        }
                        break;
                }
            }

            return spanEnumerationBuffer;
        }

        /// <summary>
        /// Retrieves a buffer containing all of the non-auto-sized row/column definitions in the specified span.
        /// The buffer is sorted in ascending order of <see cref="DefinitionBase.PreferredDesiredDimension"/>. 
        /// </summary>
        /// <param name="definitions">The collection of definitions from which to retrieve the span.</param>
        /// <param name="index">The index of the first definition in the span.</param>
        /// <param name="span">The number of definitions in the span.</param>
        /// <returns>A buffer containing the specified span of row/column definitions.</returns>
        private List<DefinitionBase> EnumerateNonAutoDefinitionsInSpanByDesiredDimension(IDefinitionBaseCollection definitions, Int32 index, Int32 span)
        {
            var buffer = EnumerateDefinitionsInSpan(definitions, index, span, auto: false);

            buffer.Sort(cmpByPreferredDesiredDimension);

            return buffer;
        }

        /// <summary>
        /// Retrieves a buffer containing all of the non-auto-sized row/column definitions in the specified span.
        /// The buffer is sorted in ascending order of <see cref="DefinitionBase.MaxDimension"/>.
        /// </summary>
        /// <param name="definitions">The collection of definitions from which to retrieve the span.</param>
        /// <param name="index">The index of the first definition in the span.</param>
        /// <param name="span">The number of definitions in the span.</param>
        /// <returns>A buffer containing the specified span of row/column definitions.</returns>
        private List<DefinitionBase> EnumerateNonAutoDefinitionsInSpanByMaxDimension(IDefinitionBaseCollection definitions, Int32 index, Int32 span)
        {
            var buffer = EnumerateDefinitionsInSpan(definitions, index, span, auto: false);

            buffer.Sort(cmpByMaxDimension);

            return buffer;
        }

        /// <summary>
        /// Retrieves a buffer containing all of the auto-sized row/column definitions in the specified span.
        /// The buffer is sorted in ascending order of <see cref="DefinitionBase.MeasuredContentDimension"/>.
        /// </summary>
        /// <param name="definitions">The collection of definitions from which to retrieve the span.</param>
        /// <param name="index">The index of the first definition in the span.</param>
        /// <param name="span">The number of definitions in the span.</param>
        /// <returns>A buffer containing the specified span of row/column definitions.</returns>
        private List<DefinitionBase> EnumerateAutoDefinitionsInSpanByContentDimension(IDefinitionBaseCollection definitions, Int32 index, Int32 span)
        {
            var buffer = EnumerateDefinitionsInSpan(definitions, index, span, nonauto: false);

            buffer.Sort(cmpByMeasuredContentDimension);

            return buffer;
        }

        /// <summary>
        /// Retrieves a buffer containing all of the specified definitions, sorted by their minimum arranged size.
        /// </summary>
        private List<DefinitionBase> EnumerateDefinitionsByMinArrangedSize(IDefinitionBaseCollection definitions)
        {
            spanEnumerationBuffer.Clear();
            for (int i = 0; i < definitions.Count; i++)
                spanEnumerationBuffer.Add(definitions[i]);

            spanEnumerationBuffer.Sort(cmpByMinArrangedSize);

            return spanEnumerationBuffer;
        }

        // A buffer used to sort spanned definitions during measurement.
        private readonly List<DefinitionBase> spanEnumerationBuffer = new List<DefinitionBase>(8);
        private static readonly FunctorComparer<DefinitionBase> cmpByPreferredDesiredDimension;
        private static readonly FunctorComparer<DefinitionBase> cmpByMaxDimension;
        private static readonly FunctorComparer<DefinitionBase> cmpByMeasuredContentDimension;
        private static readonly FunctorComparer<DefinitionBase> cmpByMinArrangedSize;

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
