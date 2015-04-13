using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a popup window.
    /// </summary>
    [UvmlKnownType]
    public class Popup : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkElement"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The identifying name of this element within its layout.</param>
        public Popup(UltravioletContext uv, String name)
            : base(uv, name)
        {
            this.root = new PopupRoot(uv, () =>
            {
                this.popupWidth  = root.DesiredSize.Width;
                this.popupHeight = root.DesiredSize.Height;
                this.UpdatePopupArrange(MostRecentFinalRect.Size);
            });
            this.root.ChangeLogicalParent(this);
        }

        /// <summary>
        /// Gets or sets the popup's content.
        /// </summary>
        public UIElement Child
        {
            get { return GetValue<UIElement>(ChildProperty); }
            set { SetValue<UIElement>(ChildProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the popup is currently open.
        /// </summary>
        public Boolean IsOpen
        {
            get { return GetValue<Boolean>(IsOpenProperty); }
            set { SetValue<Boolean>(IsOpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal offset between the popup's placement point and its position on the screen.
        /// </summary>
        public Double HorizontalOffset
        {
            get { return GetValue<Double>(HorizontalOffsetProperty); }
            set { SetValue<Double>(HorizontalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical offset between the popup's placement point and its position on the screen.
        /// </summary>
        public Double VerticalOffset
        {
            get { return GetValue<Double>(VerticalOffsetProperty); }
            set { SetValue<Double>(VerticalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="PlacementMode"/> value which specifies how the <see cref="Popup"/> is
        /// positioned relative to its placement target.
        /// </summary>
        public PlacementMode Placement
        {
            get { return GetValue<PlacementMode>(PlacementProperty); }
            set { SetValue<PlacementMode>(PlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets <see cref="UIElement"/> relative to which the <see cref="Popup"/> will be positioned.
        /// </summary>
        public UIElement PlacementTarget
        {
            get { return GetValue<UIElement>(PlacementTargetProperty); }
            set { SetValue<UIElement>(PlacementTargetProperty, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="IsOpen"/> property changes to <c>true</c>.
        /// </summary>
        public event UpfEventHandler Opened;

        /// <summary>
        /// Occurs when the <see cref="IsOpen"/> property changes to <c>false</c>.
        /// </summary>
        public event UpfEventHandler Closed;

        /// <summary>
        /// Identifies the <see cref="Child"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register("Child", typeof(UIElement), typeof(Popup),
            new PropertyMetadata<UIElement>(null, PropertyMetadataOptions.None, HandleChildChanged));

        /// <summary>
        /// Identifies the <see cref="IsOpen"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(Boolean), typeof(Popup),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsOpenChanged));

        /// <summary>
        /// Identifies the <see cref="HorizontalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(Double), typeof(Popup),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="VerticalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(Double), typeof(Popup),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="Placement"/> property.
        /// </summary>
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(Popup),
            new PropertyMetadata<PlacementMode>(PlacementMode.Bottom, PropertyMetadataOptions.None, HandlePlacementChanged));

        /// <summary>
        /// Identifies the <see cref="PlacementTarget"/> property.
        /// </summary>
        public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(Popup),
            new PropertyMetadata<UIElement>(null, PropertyMetadataOptions.None, HandlePlacementTargetChanged));

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            var child = Child;
            if (child != null)
            {
                return (childIndex == 0) ? child : base.GetLogicalChild(childIndex - 1);
            }
            return base.GetLogicalChild(childIndex);
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(int childIndex)
        {
            return base.GetVisualChild(childIndex);
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get { return base.LogicalChildrenCount + (Child == null ? 0 : 1); }
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get { return base.VisualChildrenCount; }
        }

        /// <inheritdoc/>
        protected override void ReloadContentCore(Boolean recursive)
        {
            base.ReloadContentCore(recursive);
            root.ReloadContent(recursive);
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            if (View.Popups.IsDrawingPopup(this))
            {
                root.Draw(time, dc);
            }
            else
            {
                if (IsOpen)
                {
                    View.Popups.Enqueue(this);
                }
            }
        }

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            base.UpdateOverride(time);
            root.Update(time);
        }

        /// <inheritdoc/>
        protected override void CacheLayoutParametersCore()
        {
            base.CacheLayoutParametersCore();
            root.CacheLayoutParameters();
        }

        /// <summary>
        /// Raises the <see cref="Opened"/> event.
        /// </summary>
        protected virtual void OnOpened()
        {
            var temp = Opened;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Closed"/> event.
        /// </summary>
        protected virtual void OnClosed()
        {
            var temp = Closed;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <inheritdoc/>
        protected override void StyleCore(UvssDocument stylesheet)
        {
            UpdatePopupStyle(stylesheet);

            base.StyleCore(stylesheet);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            UpdatePopupMeasure();

            return Size2D.Zero;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            UpdatePopupArrange(root.DesiredSize);

            return Size2D.Zero;
        }

        /// <inheritdoc/>
        protected override Visual HitTestCore(Point2D point)
        {
            return root.HitTest(point - new Point2D(popupX, popupY));
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Child"/> dependency property changes.
        /// </summary>
        private static void HandleChildChanged(DependencyObject dobj, UIElement oldValue, UIElement newValue)
        {
            if (oldValue != null)
                oldValue.ChangeLogicalParent(null);

            var popup = (Popup)dobj;
            popup.root.Child = newValue;

            if (newValue != null)
            {
                newValue.ChangeLogicalParent((UIElement)dobj);

                if (popup.IsOpen)
                {
                    popup.UpdatePopupStyle(popup.MostRecentStylesheet);
                    popup.UpdatePopupMeasure();
                }
            } 
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsOpen"/> dependency property changes.
        /// </summary>
        private static void HandleIsOpenChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var popup = (Popup)dobj;

            var child = popup.Child;
            var root  = popup.root;

            if (newValue)
            {
                if (child != null)
                {
                    child.ChangeVisualParent(root);

                    popup.UpdatePopupStyle(popup.MostRecentStylesheet);
                    popup.UpdatePopupMeasure();
                }

                popup.OnOpened();
            }
            else
            {
                child.ChangeVisualParent(null);

                popup.OnClosed();
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Placement"/> dependency property changes.
        /// </summary>
        private static void HandlePlacementChanged(DependencyObject dobj, PlacementMode oldValue, PlacementMode newValue)
        {
            var popup = (Popup)dobj;
            popup.UpdatePopupArrange(popup.MostRecentFinalRect.Size);
        }
        
        /// <summary>
        /// Occurs when the value of the <see cref="PlacementTarget"/> dependency property changes.
        /// </summary>
        private static void HandlePlacementTargetChanged(DependencyObject dobj, UIElement oldValue, UIElement newValue)
        {
            var popup = (Popup)dobj;
            popup.UpdatePopupArrange(popup.MostRecentFinalRect.Size);
        }

        /// <summary>
        /// Updates the stylesheet which is applied to the popup content.
        /// </summary>
        /// <param name="stylesheet">The stylesheet to apply to the popup content.</param>
        private void UpdatePopupStyle(UvssDocument stylesheet)
        {
            if (!IsOpen || stylesheet == null)
                return;

            root.InvalidateStyle();
            root.Style(MostRecentStylesheet);
        }

        /// <summary>
        /// Updates the popup's measurement state.
        /// </summary>
        private void UpdatePopupMeasure()
        {
            root.InvalidateMeasure();
            root.Measure(new Size2D(Double.PositiveInfinity, Double.PositiveInfinity));

            popupWidth  = root.DesiredSize.Width;
            popupHeight = root.DesiredSize.Height;
        }

        /// <summary>
        /// Updates the popup's arrangement state.
        /// </summary>
        private void UpdatePopupArrange(Size2D finalSize)
        {
            switch (Placement)
            {
                case PlacementMode.Absolute:
                    CalculatePopupPosition_Absolute(finalSize, out popupX, out popupY);
                    break;

                case PlacementMode.Relative:
                    break;

                case PlacementMode.Bottom:
                    CalculatePopupPosition_Bottom(finalSize, out popupX, out popupY);
                    break;

                case PlacementMode.Center:
                    break;
                case PlacementMode.Right:
                    break;
                case PlacementMode.AbsolutePoint:
                    break;
                case PlacementMode.RelativePoint:
                    break;
                case PlacementMode.Mouse:
                    break;
                case PlacementMode.MousePoint:
                    break;
                case PlacementMode.Left:
                    break;
                case PlacementMode.Top:
                    break;
            }

            popupX += HorizontalOffset;
            popupY += VerticalOffset;

            root.Arrange(new RectangleD(popupX, popupY, finalSize.Width, finalSize.Height), ArrangeOptions.None);
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.Absolute"/>.
        /// </summary>
        private void CalculatePopupPosition_Absolute(Size2D finalSize, out Double popupX, out Double popupY)
        {
            popupX = 0;
            popupY = 0;
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.Bottom"/>.
        /// </summary>
        private void CalculatePopupPosition_Bottom(Size2D finalSize, out Double popupX, out Double popupY)
        {
            var target = PlacementTarget;
            if (target == null)
            {
                popupX = 0;
                popupY = 0;
                return;
            }

            var targetBounds = target.AbsoluteBounds;

            popupX = targetBounds.Left;
            popupY = targetBounds.Bottom;
        }

        // The root visual of the popup's content.
        private readonly PopupRoot root;

        // The popup's size and position.
        private Double popupX;
        private Double popupY;
        private Double popupWidth;
        private Double popupHeight;
    }
}
