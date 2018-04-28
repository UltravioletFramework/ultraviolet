using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a panel which allows its children to dock to one of its four edges.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.DockPanel.xml")]
    public class DockPanel : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DockPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public DockPanel(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets the <see cref="Dock"/> value that specifies how an element is positioned within its parent <see cref="DockPanel"/>.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The <see cref="Dock"/> value that specifies how an element is positioned within its parent <see cref="DockPanel"/>.</returns>
        public static Dock GetDock(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Dock>(DockProperty);
        }

        /// <summary>
        /// Sets the <see cref="Dock"/> value that specifies how an element is positioned within its parent <see cref="DockPanel"/>.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <param name="value">The <see cref="Dock"/> value that specifies how an element is positioned within its parent <see cref="DockPanel"/>.</param>
        public static void SetDock(DependencyObject element, Dock value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue<Dock>(DockProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the panel's last child stretches to fit the available space.
        /// </summary>
        /// <value><see langword="true"/> if the panel's last child stretches to fit the available space;
        /// otherwise, <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="LastChildFillProperty"/></dpropField>
        ///     <dpropStylingName>last-child-fill</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean LastChildFill
        {
            get { return GetValue<Boolean>(LastChildFillProperty); }
            set { SetValue(LastChildFillProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.DockPanel.Dock"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.DockPanel.Dock"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets a <see cref="Dock"/> value which specifies how a child element is arranged
        /// within its parent <see cref="DockPanel"/>.
        /// </summary>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="DockProperty"/></dpropField>
        ///     <dpropStylingName>dock</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty DockProperty = DependencyProperty.RegisterAttached("Dock", typeof(Dock), typeof(DockPanel),
            new PropertyMetadata<Dock>(PresentationBoxedValues.Dock.Left, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="LastChildFill"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="LastChildFill"/> dependency property.</value>
        public static readonly DependencyProperty LastChildFillProperty = DependencyProperty.Register("LastChildFill", typeof(Boolean), typeof(DockPanel),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.AffectsArrange));

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var panelWidth = 0.0;
            var panelHeight = 0.0;

            var childrenWidth = 0.0;
            var childrenHeight = 0.0;
            
            foreach (var child in Children)
            {
                var availableSizeForChild = new Size2D(
                    availableSize.Width - childrenWidth,
                    availableSize.Height - childrenHeight);

                child.Measure(availableSizeForChild);

                switch (GetDock(child))
                {
                    case Dock.Left:
                    case Dock.Right:
                        panelHeight = Math.Max(panelHeight, childrenHeight + child.DesiredSize.Height);
                        childrenWidth += child.DesiredSize.Width;
                        break;

                    case Dock.Top:
                    case Dock.Bottom:
                        panelWidth = Math.Max(panelWidth, childrenWidth + child.DesiredSize.Width);
                        childrenHeight += child.DesiredSize.Height;
                        break;
                }
            }

            var totalWidth = Math.Max(panelWidth, childrenWidth);
            var totalHeight = Math.Max(panelHeight, childrenHeight);

            return new Size2D(totalWidth, totalHeight);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var sizeLeft   = 0.0;
            var sizeTop    = 0.0;
            var sizeRight  = 0.0;
            var sizeBottom = 0.0;

            var childWidth  = 0.0;
            var childHeight = 0.0;

            var lastChildFill = LastChildFill;

            for (int i = 0; i < Children.Count; i++)
            {
                var child       = Children[i];
                var isLastChild = (i == Children.Count - 1);

                var remainingWidth  = finalSize.Width - (sizeLeft + sizeRight);
                var remainingHeight = finalSize.Height - (sizeTop + sizeBottom);

                var childRect = RectangleD.Empty;

                switch (GetDock(child))
                {
                    case Dock.Left:
                        childWidth  = isLastChild && lastChildFill ? remainingWidth : Math.Min(child.DesiredSize.Width, remainingWidth);
                        childHeight = remainingHeight;
                        childRect   = new RectangleD(sizeLeft, sizeTop, childWidth, childHeight);
                        sizeLeft    = sizeLeft + childRect.Width;
                        break;

                    case Dock.Top:
                        childWidth  = remainingWidth;
                        childHeight = isLastChild && lastChildFill ? remainingHeight : Math.Min(child.DesiredSize.Height, remainingHeight);
                        childRect   = new RectangleD(sizeLeft, sizeTop, childWidth, childHeight);
                        sizeTop     = sizeTop + childRect.Height;
                        break;

                    case Dock.Right:
                        childWidth  = isLastChild && lastChildFill ? remainingWidth : Math.Min(child.DesiredSize.Width, remainingWidth);
                        childHeight = remainingHeight;
                        childRect   = new RectangleD(finalSize.Width - (sizeRight + childWidth), sizeTop, childWidth, childHeight);
                        sizeRight   = sizeRight + childRect.Width;
                        break;

                    case Dock.Bottom:
                        childWidth  = remainingWidth;
                        childHeight = isLastChild && lastChildFill ? remainingHeight : Math.Min(child.DesiredSize.Height, remainingHeight);
                        childRect   = new RectangleD(sizeLeft, finalSize.Height - (sizeBottom + childHeight), childWidth, childHeight);
                        sizeBottom  = sizeBottom + childRect.Height;
                        break;
                }

                child.Arrange(childRect);
            }

            return finalSize;
        }
    }
}
