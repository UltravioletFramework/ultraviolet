using System;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the part of a <see cref="TabControl"/> which displays the tabbed list of items.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Primitives.Templates.TabPanel.xml")]
    public class TabPanel : Panel
    {
        /// <summary>
        /// Initializes the <see cref="TabPanel"/> class.
        /// </summary>
        static TabPanel()
        {
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TabPanel), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Once));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TabPanel), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Cycle));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TabPanel(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var desiredSize = Size2D.Zero;

            maxItemWidth = Double.MinValue;
            maxItemHeight = Double.MinValue;

            var placement = ControlTabStripPlacement;
            if (placement == Dock.Top || placement == Dock.Bottom)
            {
                rowCount = 1;

                var rowWidth = 0.0;
                var rowWidthMax = 0.0;
                var rowHeight = 0.0;

                foreach (var child in Children)
                {
                    child.Measure(availableSize);

                    var childDesiredSize = (child.DesiredSize - child.GetValue<Thickness>(MarginProperty));
                    maxItemWidth = Math.Max(maxItemWidth, childDesiredSize.Width);
                    maxItemHeight = Math.Max(maxItemHeight, childDesiredSize.Height);
                    rowHeight = Math.Max(rowHeight, childDesiredSize.Height);

                    if (childDesiredSize.Width + rowWidth > availableSize.Width)
                    {
                        rowWidth = childDesiredSize.Width;
                        rowWidthMax = Math.Max(rowWidthMax, rowWidth);
                        rowCount++;
                    }
                    else
                    {
                        rowWidth += childDesiredSize.Width;
                    }
                }

                rowWidthMax = Math.Max(rowWidthMax, rowWidth);
                desiredSize = new Size2D(desiredSize.Width, rowHeight * rowCount);

                var sizeToContent = (Double.IsInfinity(availableSize.Width) || rowWidthMax < availableSize.Width);
                desiredSize = sizeToContent ? new Size2D(rowWidthMax, desiredSize.Height) : new Size2D(availableSize.Width, desiredSize.Height);
            }
            else
            {
                foreach (var child in Children)
                {
                    child.Measure(availableSize);
                    
                    var childDesiredSize = (child.DesiredSize - child.GetValue<Thickness>(MarginProperty));
                    maxItemWidth = Math.Max(maxItemWidth, childDesiredSize.Width);
                    maxItemHeight = Math.Max(maxItemHeight, childDesiredSize.Height);

                    desiredSize = new Size2D(maxItemWidth, desiredSize.Height + childDesiredSize.Height);
                }

                rowCount = Children.Count;
            }

            return desiredSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var placement = ControlTabStripPlacement;
            if (placement == Dock.Top || placement == Dock.Bottom)
            {
                var stretchRows = (rowCount > 1);

                var rowStart = 0;
                var rowEnd = 0;
                var rowWidth = 0.0;

                var positionX = 0.0;
                var positionY = 0.0;

                var owner = TemplatedParent as Selector;
                var ownerSelectedIndex = (owner == null) ? -1 : owner.SelectedIndex;

                var selectedRowHeight = (ownerSelectedIndex >= 0) ? maxItemHeight : 0.0;

                while (rowStart < Children.Count)
                {
                    FindNextRowOfTabs(finalSize, rowStart, ref rowEnd, out rowWidth);
                    var rowIsSelected = ownerSelectedIndex >= rowStart && ownerSelectedIndex <= rowEnd;
                    var rowTabCount = 1 + (rowEnd - rowStart);

                    var rowWidthExcess = stretchRows ? (finalSize.Width - rowWidth) / rowTabCount : 0.0;
                    var rowWidthTotal = 0.0;

                    for (int i = rowStart; i <= rowEnd; i++)
                    {
                        var actualPositionX = positionX;
                        var actualPositionY = (placement == Dock.Top) ?
                            (rowIsSelected ? finalSize.Height - selectedRowHeight : positionY) :
                            (rowIsSelected ? 0 : positionY + selectedRowHeight);

                        var child = Children[i];
                        var childDesiredSize = child.DesiredSize - child.GetValue<Thickness>(MarginProperty);
                        var childWidth = PerformLayoutRounding(childDesiredSize.Width + rowWidthExcess);
                        var childHeight = maxItemHeight;
                        rowWidthTotal += childWidth;

                        var isLastInRow = (i == rowEnd);
                        if (isLastInRow && stretchRows)
                        {
                            childWidth += (finalSize.Width - rowWidthTotal);
                        }

                        child.Arrange(new RectangleD(actualPositionX, actualPositionY, childWidth, childHeight));

                        positionX += childWidth;
                    }

                    rowStart = rowEnd + 1;

                    positionX = 0;
                    positionY = rowIsSelected ? positionY : positionY + maxItemHeight;
                }
            }
            else
            {
                var positionX = 0.0;
                var positionY = 0.0;
                foreach (var child in Children)
                {
                    var childDesiredSize = (child.DesiredSize - child.GetValue<Thickness>(MarginProperty));
                    child.Arrange(new RectangleD(positionX, positionY, finalSize.Width, childDesiredSize.Height));
                    positionY += childDesiredSize.Height;
                }
            }

            return finalSize;
        }
        
        /// <summary>
        /// Gets the <see cref="TabControl.TabStripPlacement"/> value specified on this panel's templated parent.
        /// </summary>
        private Dock ControlTabStripPlacement
        {
            get
            {
                var parent = TemplatedParent as TabControl;
                if (parent == null)
                {
                    return Dock.Top;
                }
                return parent.TabStripPlacement;
            }
        }
        
        /// <summary>
        /// Calculates the parameters of the next row of tabs, starting at <paramref name="rowStart"/>.
        /// </summary>
        /// <param name="finalSize">The final arranged size of the panel.</param>
        /// <param name="rowStart">The index of the first child in the row.</param>
        /// <param name="rowEnd">The index of the last child in the row.</param>
        /// <param name="rowWidth">The width of the tabs in the row.</param>
        private void FindNextRowOfTabs(Size2D finalSize, Int32 rowStart, ref Int32 rowEnd, out Double rowWidth)
        {
            rowWidth = 0.0;
            
            for (var ix = 0; rowStart + ix < Children.Count; ix++)
            {
                var child = Children[rowStart + ix];
                var childSize = (child.DesiredSize - child.GetValue<Thickness>(MarginProperty));

                if (ix > 0 && rowWidth + childSize.Width > finalSize.Width)
                {
                    rowEnd = ix - 1;
                    return;
                }

                rowWidth += childSize.Width;
            }

            rowEnd = Children.Count - 1;
        }        
        
        // State values.
        private Double maxItemWidth;
        private Double maxItemHeight;
        private Int32 rowCount;
    }
}
