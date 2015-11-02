using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the part of a <see cref="TabControl"/> which displays the tabbed list of items.
    /// </summary>
    public class TabPanel : Panel
    {
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
                var rowCount = 1;
                var rowWidth = 0.0;
                var rowWidthMax = 0.0;
                var rowHeight = 0.0;

                foreach (var child in Children)
                {
                    child.Measure(availableSize);

                    var childDesiredSize = (child.DesiredSize - child.GetValue<Thickness>(MarginProperty));
                    rowHeight = Math.Max(rowHeight, childDesiredSize.Height);

                    if (childDesiredSize.Width + rowWidth > availableSize.Width)
                    {
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

                var sizeToContent = (Double.IsInfinity(desiredSize.Width) || rowWidthMax < desiredSize.Width);
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
            }

            return desiredSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var placement = ControlTabStripPlacement;
            if (placement == Dock.Top || placement == Dock.Bottom)
            {
                // TODO
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

        // State values.
        private Double maxItemWidth;
        private Double maxItemHeight;
    }
}
