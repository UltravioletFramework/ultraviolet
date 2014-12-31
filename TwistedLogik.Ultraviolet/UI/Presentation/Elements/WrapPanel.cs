using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an element container which positions its children according to their distance from the container's
    /// left, top, right, and bottom edges.
    /// </summary>
    [UIElement("WrapPanel")]
    public class WrapPanel : Container
    {
        /// <summary>
        /// Initializes the <see cref="WrapPanel"/> type.
        /// </summary>
        static WrapPanel()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(WrapPanel).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.WrapPanel.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        /// <param name="viewModelType">The type of view model to which the element will be bound.</param>
        /// <param name="bindingContext">The binding context to apply to the element which is instantiated.</param>
        public WrapPanel(UltravioletContext uv, String id, Type viewModelType, String bindingContext = null)
            : base(uv, id)
        {
            SetDefaultValue<Color>(UIElement.BackgroundColorProperty, Color.Transparent);

            if (ComponentTemplate != null)
                LoadComponentRoot(ComponentTemplate, viewModelType, bindingContext);
        }

        /// <inheritdoc/>
        public sealed override void CalculateContentSize(ref Int32? width, ref Int32? height)
        {
            if (width == null || height == null)
            {
                var index       = 0;
                var totalWidth  = 0;
                var totalHeight = 0;

                if (Orientation == Orientation.Horizontal)
                {
                    var extent    = width ?? ((Parent == null) ? 0 : Parent.ActualWidth - ContainerRelativeArea.X);
                    var rowCount  = 0;
                    var rowWidth  = 0;
                    var rowHeight = 0;

                    while (CalculateRowProperties(index, out rowCount, out rowWidth, out rowHeight, extent))
                    {
                        totalWidth  = Math.Max(totalWidth, rowWidth);
                        totalHeight = totalHeight + rowHeight;

                        index += rowCount;
                    }
                }
                else
                {
                    var extent    = height ?? ((Parent == null) ? 0 : Parent.ActualHeight - ContainerRelativeArea.Y);
                    var columnCount  = 0;
                    var columnWidth  = 0;
                    var columnHeight = 0;

                    while (CalculateColumnProperties(index, out columnCount, out columnWidth, out columnHeight, extent))
                    {
                        totalWidth  = totalWidth + columnWidth;
                        totalHeight = Math.Max(totalHeight, columnHeight);

                        index += columnCount;
                    }
                }

                if (width == null)
                    width = totalWidth;

                if (height == null)
                    height = totalHeight;
            }

            base.CalculateContentSize(ref width, ref height);
        }

        /// <inheritdoc/>
        public sealed override void PerformContentLayout()
        {
            var index     = 0;
            var positionX = 0;
            var positionY = 0;

            if (Orientation == Orientation.Horizontal)
            {
                var rowCount  = 0;
                var rowWidth  = 0;
                var rowHeight = 0;

                while (CalculateRowProperties(index, out rowCount, out rowWidth, out rowHeight))
                {
                    positionX = 0;
                    PerformRowLayout(index, rowCount, rowHeight, ref positionX, ref positionY);
                    index += rowCount;
                }
            }
            else
            {
                var columnCount  = 0;
                var columnHeight = 0;
                var columnWidth  = 0;

                while (CalculateColumnProperties(index, out columnCount, out columnWidth, out columnHeight))
                {
                    positionY = 0;
                    PerformColumnLayout(index, columnCount, columnWidth, ref positionX, ref positionY);
                    index += columnCount;
                }
            }
            UpdateScissorRectangle();
        }

        /// <inheritdoc/>
        public sealed override void PerformPartialLayout(UIElement content)
        {
            Contract.Require(content, "content");

            RequestLayout();
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
        /// Gets or sets the wrap panel's orientation.
        /// </summary>
        public Orientation Orientation
        {
            get { return GetValue<Orientation>(OrientationProperty); }
            set { SetValue<Orientation>(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the width of elements which are contained by the wrap panel.
        /// </summary>
        public Double ItemWidth
        {
            get { return GetValue<Double>(ItemWidthProperty); }
            set { SetValue<Double>(ItemWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the height of elements which are contained by the wrap panel.
        /// </summary>
        public Double ItemHeight
        {
            get { return GetValue<Double>(ItemHeightProperty); }
            set { SetValue<Double>(ItemHeightProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        public event UIElementEventHandler OrientationChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="ItemWidth"/> property changes.
        /// </summary>
        public event UIElementEventHandler ItemWidthChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="ItemHeight"/> property changes.
        /// </summary>
        public event UIElementEventHandler ItemHeightChanged;

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(WrapPanel),
            new DependencyPropertyMetadata(HandleOrientationChanged, () => Orientation.Vertical, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="ItemWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(Double), typeof(WrapPanel),
            new DependencyPropertyMetadata(HandleItemWidthChanged, () => Double.NaN, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="ItemHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(Double), typeof(WrapPanel),
            new DependencyPropertyMetadata(HandleItemHeightChanged, () => Double.NaN, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            base.OnDrawing(time, spriteBatch);
        }

        /// <summary>
        /// Raises the <see cref="OrientationChanged"/> event.
        /// </summary>
        protected virtual void OnOrientationChanged()
        {
            var temp = OrientationChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemWidthChanged"/> event.
        /// </summary>
        protected virtual void OnItemWidthChanged()
        {
            var temp = ItemWidthChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemHeightChanged"/> event.
        /// </summary>
        protected virtual void OnItemHeightChanged()
        {
            var temp = ItemHeightChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleOrientationChanged(DependencyObject dobj)
        {
            var element = (WrapPanel)dobj;
            element.OnOrientationChanged();
            element.RequestLayout();

            var parent = element.Parent;
            if (parent != null)
                parent.PerformPartialLayout(element);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ItemWidth"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleItemWidthChanged(DependencyObject dobj)
        {
            var element = (WrapPanel)dobj;
            element.OnItemWidthChanged();
            element.RequestLayout();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ItemHeight"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleItemHeightChanged(DependencyObject dobj)
        {
            var element = (WrapPanel)dobj;
            element.OnItemHeightChanged();
            element.RequestLayout();
        }

        /// <summary>
        /// Immediately recalculates the layout of the specified child element.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout.</param>
        private void UpdateChildLayout(UIElement child)
        {
            if (Orientation == Orientation.Horizontal)
            {
                UpdateChildLayoutHorizontal(child);
            }
            else
            {
                UpdateChildLayoutVertical(child);
            }
        }

        /// <summary>
        /// Immediately recalculates the layout of the specified child element when
        /// the wrap panel is in a vertical orientation.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout.</param>
        private void UpdateChildLayoutVertical(UIElement child)
        {

        }

        /// <summary>
        /// Immediately recalculates the layout of the specified child element when
        /// the wrap panel is in a horizontal orientation.
        /// </summary>
        /// <param name="child">The child element for which to calculate a layout.</param>
        private void UpdateChildLayoutHorizontal(UIElement child)
        {

        }

        /// <summary>
        /// Lays out the elements in the specified layout row.
        /// </summary>
        /// <param name="rowIndex">The index of the first child element in the row.</param>
        /// <param name="rowCount">The number of elements in the row.</param>
        /// <param name="rowHeight">The height of the row in pixels.</param>
        /// <param name="positionX">The distance in pixels between the left edge of the container and the left edge of the row.</param>
        /// <param name="positionY">The distance in pixels between the top edge of the container and the top edge of the row.</param>
        private void PerformRowLayout(Int32 rowIndex, Int32 rowCount, Int32 rowHeight, ref Int32 positionX, ref Int32 positionY)
        {
            positionX = 0;

            for (int i = 0; i < rowCount; i++)
            {
                var child  = Children[rowIndex + i];
                var size   = CalculateElementSize(child);
                var margin = ConvertThicknessToPixels(child.Margin, 0);

                var elementPosX = positionX + (Int32)margin.Left;
                var elementPosY = 0;

                switch (child.VerticalAlignment)
                {
                    case VerticalAlignment.Center:
                    case VerticalAlignment.Stretch:
                        {
                            var layoutHeight = rowHeight - ((Int32)margin.Top + (Int32)margin.Bottom);
                            elementPosY = (positionY + (Int32)margin.Top) + ((layoutHeight - size.Height) / 2);
                        }
                        break;
                    
                    case VerticalAlignment.Top:
                        elementPosY = (positionY + (Int32)margin.Top);
                        break;

                    case VerticalAlignment.Bottom:
                        elementPosY = (positionY + rowHeight) - ((Int32)margin.Bottom + size.Height);
                        break;
                }

                child.ContainerRelativeArea = new Rectangle(elementPosX, elementPosY, size.Width, size.Height);

                positionX += size.Width + (Int32)margin.Left + (Int32)margin.Right;
            }

            positionY += rowHeight;
        }

        /// <summary>
        /// Lays out the elements in the specified layout column.
        /// </summary>
        /// <param name="columnIndex">The index of the first child element in the column.</param>
        /// <param name="columnCount">The number of elements in the column.</param>
        /// <param name="columnWidth">The width of the column in pixels.</param>
        /// <param name="positionX">The distance in pixels between the left edge of the container and the left edge of the column.</param>
        /// <param name="positionY">The distance in pixels between the top edge of the container and the top edge of the column.</param>
        private void PerformColumnLayout(Int32 columnIndex, Int32 columnCount, Int32 columnWidth, ref Int32 positionX, ref Int32 positionY)
        {
            positionY = 0;

            for (int i = 0; i < columnCount; i++)
            {
                var child  = Children[columnIndex + i];
                var size   = CalculateElementSize(child);
                var margin = ConvertThicknessToPixels(child.Margin, 0);

                var elementPosX = 0;
                var elementPosY = positionY + (Int32)margin.Top;

                switch (child.HorizontalAlignment)
                {
                    case HorizontalAlignment.Center:
                    case HorizontalAlignment.Stretch:
                        {
                            var layoutWidth = columnWidth - ((Int32)margin.Left + (Int32)margin.Right);
                            elementPosX = (positionX + (Int32)margin.Left) + ((layoutWidth - size.Width) / 2);
                        }
                        break;

                    case HorizontalAlignment.Left:
                        elementPosX = (positionX + (Int32)margin.Left);
                        break;

                    case HorizontalAlignment.Right:
                        elementPosX = (positionX + columnWidth) - ((Int32)margin.Right + size.Width);
                        break;
                }

                child.ContainerRelativeArea = new Rectangle(elementPosX, elementPosY, size.Width, size.Height);

                positionY += size.Height + (Int32)margin.Top + (Int32)margin.Bottom;
            }

            positionX += columnWidth;
        }

        /// <summary>
        /// Calculates the properties of the specified layout row.
        /// </summary>
        /// <param name="index">The index of the first element in the layout row.</param>
        /// <param name="rowCount">The number of elements in the layout row.</param>
        /// <param name="rowWidth">The width of the layout row in pixels.</param>
        /// <param name="rowHeight">The height of the layout row in pixels.</param>
        /// <param name="extent">The maximum extent of a row. If <c>null</c>, this is the width of the container.</param>
        /// <returns><c>true</c> if the specified index is the beginning of a valid layout row; otherwise, <c>false</c>.</returns>
        private Boolean CalculateRowProperties(Int32 index, out Int32 rowCount, out Int32 rowWidth, out Int32 rowHeight, Int32? extent = null)
        {
            rowCount  = 0;
            rowWidth  = 0;
            rowHeight = 0;

            if (index >= Children.Count)
                return false;

            var position = 0;
            var height   = 0;
            var count    = 0;

            for (int i = index; i < Children.Count; i++)
            {
                var size = CalculateElementRegionSize(Children[i]);
                if (position + size.Width > (extent ?? ActualWidth))
                    break;

                count  = count + 1;
                height = Math.Max(height, size.Height);

                position += size.Width;
            }

            rowCount  = count;
            rowWidth  = position;
            rowHeight = height;

            return rowCount > 0;
        }

        /// <summary>
        /// Calculates the properties of the specified layout column.
        /// </summary>
        /// <param name="index">The index of the first element in the layout column.</param>
        /// <param name="columnCount">The number of elements in the layout column.</param>
        /// <param name="columnWidth">The width of the layout column in pixels.</param>
        /// <param name="columnHeight">The height of the layout column in pixels.</param>
        /// <param name="extent">The maximum extent of a column. If <c>null</c>, this is the height of the container.</param>
        /// <returns><c>true</c> if the specified index is the beginning of a valid layout column; otherwise, <c>false</c>.</returns>
        private Boolean CalculateColumnProperties(Int32 index, out Int32 columnCount, out Int32 columnWidth, out Int32 columnHeight, Int32? extent = null)
        {
            columnCount  = 0;
            columnWidth  = 0;
            columnHeight = 0;
            
            if (index >= Children.Count)
                return false;

            var position = 0;
            var width    = 0;
            var count    = 0;

            for (int i = index; i < Children.Count; i++)
            {
                var size = CalculateElementRegionSize(Children[i]);
                if (position + size.Height > (extent ?? ActualHeight))
                    break;

                count = count + 1;
                width = Math.Max(width, size.Width);

                position += size.Height;
            }

            columnCount  = count;
            columnWidth  = width;
            columnHeight = position;

            return columnCount > 0;
        }

        /// <summary>
        /// Calculates the size of the specified child element.
        /// </summary>
        /// <param name="element">The element for which to calculate a size.</param>
        /// <returns>The size of the specified child element.</returns>
        private Size2 CalculateElementSize(UIElement element)
        {
            int? pxWidth  = Double.IsNaN(ItemWidth)  ? null : (Int32?)ConvertMeasureToPixels(ItemWidth, 0);
            int? pxHeight = Double.IsNaN(ItemHeight) ? null : (Int32?)ConvertMeasureToPixels(ItemHeight, 0);
            element.CalculateActualSize(ref pxWidth, ref pxHeight);

            return new Size2(pxWidth ?? 0, pxHeight ?? 0);
        }

        /// <summary>
        /// Calculates the size of the layout region that contains the specified child element, including its margins.
        /// </summary>
        /// <param name="element">The element for which to calculate a region size.</param>
        /// <returns>The size of the specified child element's layout region.</returns>
        private Size2 CalculateElementRegionSize(UIElement element)
        {
            var margin = ConvertThicknessToPixels(element.Margin, 0);
            var size   = CalculateElementSize(element);

            return new Size2(
                (Int32)margin.Left + (Int32)margin.Right  + size.Width,
                (Int32)margin.Top  + (Int32)margin.Bottom + size.Height);
        }
    }
}
