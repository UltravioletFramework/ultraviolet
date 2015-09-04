using System;
using System.ComponentModel;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a popup window.
    /// </summary>
    /// <remarks>For more information on the calculations used to position popups, see the MSDN article on how it's 
    /// handled in WPF (https://msdn.microsoft.com/en-us/library/bb613596(v=vs.90).aspx). Ultraviolet follows the algorithms 
    /// described on that page. See in particular the tables under the "How the Properties Work Together" and 
    /// "When the Popup Encounters the Edge of the Screen" headings.</remarks>
    [UvmlKnownType]
    [DefaultProperty("Child")]
    public partial class Popup : FrameworkElement
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
        /// Gets or sets the rectangle relative to which the popup is positioned.
        /// </summary>
        public RectangleD PlacementRectangle
        {
            get { return GetValue<RectangleD>(PlacementRectangleProperty); }
            set { SetValue<RectangleD>(PlacementRectangleProperty, value); }
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
        /// Gets the root element of the popup's visual tree.
        /// </summary>
        public UIElement Root
        {
            get { return root; }
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
        /// <remarks>The styling name of this dependency property is 'child'.</remarks>
        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register("Child", typeof(UIElement), typeof(Popup),
            new PropertyMetadata<UIElement>(null, PropertyMetadataOptions.None, HandleChildChanged));

        /// <summary>
        /// Identifies the <see cref="IsOpen"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'open'.</remarks>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(Boolean), typeof(Popup),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsOpenChanged, CoerceIsOpen));

        /// <summary>
        /// Identifies the <see cref="HorizontalOffset"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'hoffset'.</remarks>
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", "hoffset", typeof(Double), typeof(Popup),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="VerticalOffset"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'voffset'.</remarks>
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", "voffset", typeof(Double), typeof(Popup),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="Placement"/> property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'placement'.</remarks>
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(Popup),
            new PropertyMetadata<PlacementMode>(PlacementMode.Bottom, PropertyMetadataOptions.None, HandlePlacementChanged));

        /// <summary>
        /// Identifies the <see cref="PlacementTarget"/> property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'placement-target'.</remarks>
        public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(Popup),
            new PropertyMetadata<UIElement>(null, PropertyMetadataOptions.None, HandlePlacementTargetChanged));

        /// <summary>
        /// Identifies the <see cref="PlacementRectangle"/> property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'placement-rectangle'.</remarks>
        public static readonly DependencyProperty PlacementRectangleProperty = DependencyProperty.Register("PlacementRectangle", typeof(RectangleD), typeof(Popup),
            new PropertyMetadata<RectangleD>(RectangleD.Empty, PropertyMetadataOptions.None, HandlePlacementRectangleChanged));

        /// <summary>
        /// Converts the specified point in screen coordinates to popup coordinates.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        /// <returns>The converted point.</returns>
        internal Point2D ScreenToPopup(Point2D point)
        {
            Point2D.Transform(ref point, ref transformToAncestorInverse, out point);
            return point + popupTransformOrigin;
        }

        /// <summary>
        /// Adds the popup to the view's popup queue for drawing.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        internal void EnqueueForDrawing(UltravioletTime time, DrawingContext dc)
        {
            if (View.Popups.IsDrawingPopup(this))
                return;

            var mtxDips = transformToAncestor;
            var mtxPixs = new Matrix(
                mtxDips.M11, mtxDips.M12, mtxDips.M13, (Int32)Display.DipsToPixels(mtxDips.M14),
                mtxDips.M21, mtxDips.M22, mtxDips.M23, (Int32)Display.DipsToPixels(mtxDips.M24),
                mtxDips.M31, mtxDips.M32, mtxDips.M33, mtxDips.M34,
                mtxDips.M41, mtxDips.M42, mtxDips.M43, mtxDips.M44);

            View.Popups.Enqueue(this, mtxPixs);
        }

        /// <summary>
        /// If the popup is currently open, this method adds it to the view's popup queue for drawing.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        internal void EnqueueForDrawingIfOpen(UltravioletTime time, DrawingContext dc)
        {
            if (!IsOpen)
                return;

            EnqueueForDrawing(time, dc);
        }

        /// <summary>
        /// Performs a hit test against the popup.
        /// </summary>
        /// <param name="point">The point in screen space to evaluate.</param>
        /// <returns>The <see cref="Visual"/> at the specified point in screen space, or <c>null</c> if there is no such visual.</returns>
        internal Visual PopupHitTest(Point2D point)
        {
            if (!IsOpen)
                return null;

            Point2D.Transform(ref point, ref transformToAncestorInverse, out point);
            return root.HitTest(point + popupTransformOrigin);
        }
        
        /// <summary>
        /// Gets the transformation matrix which is used to render the popup.
        /// </summary>
        internal Matrix PopupTransformToAncestor
        {
            get { return transformToAncestor; }
        }

        /// <summary>
        /// Gets the transformation matrix which is used to render the popup, offset by the popup's transform origin.
        /// </summary>
        internal Matrix PopupTransformToAncestorWithOrigin
        {
            get { return transformToAncestorWithOrigin; }
        }

        /// <inheritdoc/>
        protected internal override void InvalidateVisualBounds()
        {
            root.InvalidateVisualBounds();
            base.InvalidateVisualBounds();
        }

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
                EnqueueForDrawingIfOpen(time, dc);
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

        /// <inheritdoc/>
        protected override void StyleCore(UvssDocument styleSheet)
        {
            UpdatePopupStyle(styleSheet);

            base.StyleCore(styleSheet);
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
            return null;
        }

        /// <inheritdoc/>
        protected override RectangleD CalculateVisualBounds()
        {
            if (IsOpen)
            {
                var relativePosition = root.AbsolutePosition - this.AbsolutePosition;
                return new RectangleD(relativePosition, root.VisualBounds.Size);
            }
            return RectangleD.Empty;
        }

        /// <inheritdoc/>
        protected override RectangleD CalculateTransformedVisualBounds()
        {
            if (IsOpen)
            {
                return root.TransformedVisualBounds;
            }
            return RectangleD.Empty;
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

        /// <summary>
        /// Coerces the value of the <see cref="IsOpen"/> property in order to allow popup
        /// opening to be deferred in case the popup hasn't fully loaded yet.
        /// </summary>
        private static Boolean CoerceIsOpen(DependencyObject dobj, Boolean value)
        {
            if (value)
            {
                var popup = (Popup)dobj;
                if (popup.IsLoaded)
                    return true;

                if (!popup.loadingDeferred)
                {
                    popup.loadingDeferred = true;
                    popup.Loaded += OpenDeferred;
                }
                return false;
            }
            return value;
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
                    popup.UpdatePopupStyle(popup.MostRecentStyleSheet);
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
            var root = popup.root;

            if (newValue)
            {
                root.EnsureIsLoaded(true);
                root.IsOpen = true;

                root.HookIntoVisualTree();

                popup.UpdatePopupStyle(popup.MostRecentStyleSheet);
                popup.UpdatePopupMeasure();
                popup.UpdatePopupArrange(popup.MostRecentFinalRect.Size);
                
                popup.OnOpened();
            }
            else
            {
                root.EnsureIsLoaded(false);
                root.IsOpen = false;

                root.UnhookFromVisualTree();
                
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
        /// Occurs when the value of the <see cref="PlacementRectangle"/> dependency property changes.
        /// </summary>
        private static void HandlePlacementRectangleChanged(DependencyObject dobj, RectangleD oldValue, RectangleD newValue)
        {
            var popup = (Popup)dobj;
            popup.UpdatePopupArrange(popup.MostRecentFinalRect.Size);
        }

        /// <summary>
        /// An event handler which is used to defer the opening of a popup until after it has
        /// been fully loaded, in order to ensure proper positioning.
        /// </summary>
        private static void OpenDeferred(DependencyObject element, ref RoutedEventData data)
        {
            var popup = (Popup)element;
            popup.loadingDeferred = false;
            popup.Loaded -= OpenDeferred;
            popup.IsOpen = true;
        }

        /// <summary>
        /// Updates the style sheet which is applied to the popup content.
        /// </summary>
        /// <param name="styleSheet">The style sheet to apply to the popup content.</param>
        private void UpdatePopupStyle(UvssDocument styleSheet)
        {
            if (!IsOpen || styleSheet == null)
                return;

            root.InvalidateStyle(true);
            root.Style(MostRecentStyleSheet);
        }

        /// <summary>
        /// Updates the popup's measurement state.
        /// </summary>
        private void UpdatePopupMeasure()
        {
            root.InvalidateMeasure();
            root.Measure(new Size2D(Double.PositiveInfinity, Double.PositiveInfinity));
        }

        /// <summary>
        /// Updates the popup's arrangement state.
        /// </summary>
        private void UpdatePopupArrange(Size2D finalSize)
        {
            if (!IsOpen)
                return;

            alignmentX = 0;
            alignmentY = 0;

            var screenArea = Display.PixelsToDips(View.Area);
            var popupArea  = RectangleD.Empty;

            switch (Placement)
            {
                case PlacementMode.Absolute:
                    popupArea = CalculatePopupPosition_Absolute(ref screenArea, root.DesiredSize);
                    break;

                case PlacementMode.Relative:
                    popupArea = CalculatePopupPosition_Relative(ref screenArea, root.DesiredSize);
                    break;

                case PlacementMode.Bottom:
                    popupArea = CalculatePopupPosition_Bottom(ref screenArea, root.DesiredSize);
                    break;

                case PlacementMode.Center:
                    popupArea = CalculatePopupPosition_Center(ref screenArea, root.DesiredSize);
                    break;

                case PlacementMode.Right:
                    popupArea = CalculatePopupPosition_Right(ref screenArea, root.DesiredSize);
                    break;

                case PlacementMode.AbsolutePoint:
                    popupArea = CalculatePopupPosition_AbsolutePoint(ref screenArea, root.DesiredSize);
                    break;

                case PlacementMode.RelativePoint:
                    popupArea = CalculatePopupPosition_RelativePoint(ref screenArea, root.DesiredSize);
                    break;

                case PlacementMode.Mouse:
                    popupArea = CalculatePopupPosition_Mouse(ref screenArea, root.DesiredSize);
                    break;

                case PlacementMode.MousePoint:
                    popupArea = CalculatePopupPosition_MousePoint(ref screenArea, root.DesiredSize);
                    break;

                case PlacementMode.Left:
                    popupArea = CalculatePopupPosition_Left(ref screenArea, root.DesiredSize);
                    break;

                case PlacementMode.Top:
                    popupArea = CalculatePopupPosition_Top(ref screenArea, root.DesiredSize);
                    break;
            }

            AlignToScreenEdges(ref screenArea, ref popupArea, PopupScreenEdges.All);

            root.Arrange(popupArea, ArrangeOptions.ForceInvalidatePosition);

            InvalidateVisualBounds();
        }

        /// <summary>
        /// Aligns the given rectangle to the edges of the screen.
        /// </summary>
        /// <param name="screenArea">The rectangle that represents the area of the screen.</param>
        /// <param name="popupArea">The area to align to the edges of the screen.</param>
        /// <param name="edges">A set of <see cref="PopupScreenEdges"/> values indicating which edges should be aligned.</param>
        private void AlignToScreenEdges(ref RectangleD screenArea, ref RectangleD popupArea, PopupScreenEdges edges)
        {
            RectangleD transformedPopupArea;
            RectangleD.TransformAxisAligned(ref popupArea, ref transformToAncestor, out transformedPopupArea);
            
            if ((edges & PopupScreenEdges.Left) == PopupScreenEdges.Left)
            {
                if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Left))
                {
                    alignmentX -= transformedPopupArea.X;
                    ComputeTransformationMatrices();
                }
            }

            if ((edges & PopupScreenEdges.Top) == PopupScreenEdges.Top)
            {
                if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Top))
                {
                    alignmentY -= transformedPopupArea.Y;
                    ComputeTransformationMatrices();
                }
            }

            if ((edges & PopupScreenEdges.Right) == PopupScreenEdges.Right)
            {
                if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Right))
                {
                    alignmentX -= transformedPopupArea.Right - screenArea.Width;
                    ComputeTransformationMatrices();
                }
            }

            if ((edges & PopupScreenEdges.Bottom) == PopupScreenEdges.Bottom)
            {
                if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Bottom))
                {
                    alignmentY -= transformedPopupArea.Bottom - screenArea.Height;
                    ComputeTransformationMatrices();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified popup area is crossing the specified edge of the screen.
        /// </summary>
        /// <param name="screenArea">A rectangle that describes the area of the screen.</param>
        /// <param name="popupArea">A rectangle that describes the area of the popup.</param>
        /// <param name="edge">A <see cref="PopupScreenEdges"/> value that specifies which edge to examine.</param>
        /// <returns><c>true</c> if the popup is crossing the specified screen edge; otherwise, <c>false</c>.</returns>
        private Boolean IsCrossingScreenEdge(ref RectangleD screenArea, ref RectangleD popupArea, PopupScreenEdges edge)
        {
            var transformedPopupArea = popupArea;
            RectangleD.TransformAxisAligned(ref popupArea, ref transformToAncestor, out transformedPopupArea);

            if (((edge & PopupScreenEdges.Left) == PopupScreenEdges.Left) && transformedPopupArea.Left < screenArea.Left)
                return true;
            
            if (((edge & PopupScreenEdges.Top) == PopupScreenEdges.Top) && transformedPopupArea.Top < screenArea.Top)
                return true;

            if (((edge & PopupScreenEdges.Right) == PopupScreenEdges.Right) && transformedPopupArea.Right > screenArea.Right)
                return true;

            if (((edge & PopupScreenEdges.Bottom) == PopupScreenEdges.Bottom) && transformedPopupArea.Bottom > screenArea.Bottom)
                return true;

            return false;
        }

        /// <summary>
        /// Positions one rectangle relative to another, according to the specified <see cref="PopupAlignmentPoint"/> values.
        /// </summary>
        /// <param name="actualTargetElement">The element that was actually targeted for placement of the popup.</param>
        /// <param name="targetOrigin">The origin point of the target rectangle.</param>
        /// <param name="target">The target rectangle.</param>
        /// <param name="popupOrigin">The origin point of the popup rectangle.</param>
        /// <param name="popup">The popup size.</param>
        /// <returns>The popup rectangle positioned relative to the target rectangle and including the 
        /// values of <see cref="HorizontalOffset"/> and <see cref="VerticalOffset"/> properties.</returns>
        private RectangleD PositionRects(UIElement actualTargetElement, PopupAlignmentPoint targetOrigin, RectangleD target, PopupAlignmentPoint popupOrigin, Size2D popup)
        {
            var transformOriginX = 0.0;
            switch (popupOrigin)
            {
                case PopupAlignmentPoint.TopCenter:
                case PopupAlignmentPoint.Center:
                case PopupAlignmentPoint.BottomCenter:
                    transformOriginX = popup.Width / 2.0;
                    break;

                case PopupAlignmentPoint.TopRight:
                case PopupAlignmentPoint.MiddleRight:
                case PopupAlignmentPoint.BottomRight:
                    transformOriginX = popup.Width;
                    break;
            }

            var transformOriginY = 0.0;
            switch (popupOrigin)
            {
                case PopupAlignmentPoint.MiddleLeft:
                case PopupAlignmentPoint.Center:
                case PopupAlignmentPoint.MiddleRight:
                    transformOriginY = popup.Height / 2.0;
                    break;

                case PopupAlignmentPoint.BottomLeft:
                case PopupAlignmentPoint.BottomCenter:
                case PopupAlignmentPoint.BottomRight:
                    transformOriginY = popup.Height;
                    break;
            }

            var resultX = 0.0;
            var resultY = 0.0;

            switch (targetOrigin)
            {
                case PopupAlignmentPoint.TopLeft:
                    resultX = target.Left;
                    resultY = target.Top;
                    break;

                case PopupAlignmentPoint.TopCenter:
                    resultX = target.Left + (target.Width / 2.0);
                    resultY = target.Top;
                    break;

                case PopupAlignmentPoint.TopRight:
                    resultX = target.Right;
                    resultY = target.Top;
                    break;

                case PopupAlignmentPoint.MiddleLeft:
                    resultX = target.Left;
                    resultY = target.Top + (target.Height / 2.0);
                    break;

                case PopupAlignmentPoint.Center:
                    resultX = target.Left + (target.Width / 2.0);
                    resultY = target.Top + (target.Height / 2.0);
                    break;
                
                case PopupAlignmentPoint.MiddleRight:
                    resultX = target.Right;
                    resultY = target.Top + (target.Height / 2.0);
                    break;
                
                case PopupAlignmentPoint.BottomLeft:
                    resultX = target.Left;
                    resultY = target.Bottom;
                    break;
                
                case PopupAlignmentPoint.BottomCenter:
                    resultX = target.Left + (target.Width / 2.0);
                    resultY = target.Bottom;
                    break;
                
                case PopupAlignmentPoint.BottomRight:
                    resultX = target.Right;
                    resultY = target.Bottom;
                    break;
            }

            popupTransformOrigin = new Point2D(transformOriginX, transformOriginY);

            if (actualTargetElement != null)
            {
                var actualTargetTransform = actualTargetElement.GetTransformToViewMatrix();
                var actualTargetPosition = new Point2D(HorizontalOffset + resultX, VerticalOffset + resultY);
                Point2D.Transform(ref actualTargetPosition, ref actualTargetTransform, out popupPosition);
            }
            else
            {
                popupPosition = new Point2D(HorizontalOffset + resultX - transformOriginX, VerticalOffset + resultY - transformOriginY);
            }

            ComputeTransformationMatrices();

            return new RectangleD(-transformOriginX, -transformOriginY, popup.Width, popup.Height);
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.Absolute"/>.
        /// </summary>
        private RectangleD CalculatePopupPosition_Absolute(ref RectangleD screenArea, Size2D popupSize)
        {
            UIElement actualTargetElement;

            var placementArea = GetPlacementTargetArea(out actualTargetElement, true);
            if (placementArea.IsEmpty)
                placementArea = Display.PixelsToDips(View.Area);

            var popupArea = PositionRects(actualTargetElement,
                PopupAlignmentPoint.TopLeft, placementArea,
                PopupAlignmentPoint.TopLeft, popupSize);
            
            return popupArea;
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.Relative"/>.
        /// </summary>
        private RectangleD CalculatePopupPosition_Relative(ref RectangleD screenArea, Size2D popupSize)
        {
            UIElement actualTargetElement;

            var placementArea = GetPlacementTargetArea(out actualTargetElement);

            var popupArea = PositionRects(actualTargetElement,
                PopupAlignmentPoint.TopLeft, placementArea,
                PopupAlignmentPoint.TopLeft, popupSize);

            return popupArea;
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.Bottom"/>.
        /// </summary>
        private RectangleD CalculatePopupPosition_Bottom(ref RectangleD screenArea, Size2D popupSize)
        {
            UIElement actualTargetElement;

            var placementArea = GetPlacementTargetArea(out actualTargetElement);

            var popupArea = PositionRects(actualTargetElement,
                PopupAlignmentPoint.BottomLeft, placementArea,
                PopupAlignmentPoint.TopLeft, popupSize);

            if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Bottom))
            {
                popupArea = PositionRects(actualTargetElement,
                    PopupAlignmentPoint.TopLeft, placementArea,
                    PopupAlignmentPoint.BottomLeft, popupSize);
            }

            return popupArea;
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.Center"/>.
        /// </summary>
        private RectangleD CalculatePopupPosition_Center(ref RectangleD screenArea, Size2D popupSize)
        {
            UIElement actualTargetElement;

            var placementArea = GetPlacementTargetArea(out actualTargetElement);

            var popupArea = PositionRects(actualTargetElement,
                PopupAlignmentPoint.Center, placementArea,
                PopupAlignmentPoint.Center, popupSize);
            
            return popupArea;
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.Right"/>.
        /// </summary>
        private RectangleD CalculatePopupPosition_Right(ref RectangleD screenArea, Size2D popupSize)
        {
            UIElement actualTargetElement;

            var placementArea = GetPlacementTargetArea(out actualTargetElement);

            var popupArea = PositionRects(actualTargetElement,
                PopupAlignmentPoint.TopRight, placementArea,
                PopupAlignmentPoint.TopLeft, popupSize);

            if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Right))
            {
                popupArea = PositionRects(actualTargetElement,
                    PopupAlignmentPoint.TopLeft, placementArea,
                    PopupAlignmentPoint.TopRight, popupSize);
            }

            return popupArea;
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.AbsolutePoint"/>.
        /// </summary>
        private RectangleD CalculatePopupPosition_AbsolutePoint(ref RectangleD screenArea, Size2D popupSize)
        {
            UIElement actualTargetElement;

            var placementArea = GetPlacementTargetArea(out actualTargetElement, true);
            if (placementArea.IsEmpty)
                placementArea = Display.PixelsToDips(View.Area);

            var popupArea = PositionRects(actualTargetElement,
                PopupAlignmentPoint.TopLeft, placementArea,
                PopupAlignmentPoint.TopLeft, popupSize);

            if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Bottom))
            {
                popupArea = PositionRects(actualTargetElement,
                    PopupAlignmentPoint.TopLeft, placementArea,
                    PopupAlignmentPoint.BottomLeft, popupSize);
            }

            if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Right))
            {
                popupArea = PositionRects(actualTargetElement,
                    PopupAlignmentPoint.TopLeft, placementArea,
                    PopupAlignmentPoint.TopRight, popupSize);
            }
            
            return popupArea;
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.RelativePoint"/>.
        /// </summary>
        private RectangleD CalculatePopupPosition_RelativePoint(ref RectangleD screenArea, Size2D popupSize)
        {
            UIElement actualTargetElement;

            var placementArea = GetPlacementTargetArea(out actualTargetElement);

            var popupArea = PositionRects(actualTargetElement,
                PopupAlignmentPoint.TopLeft, placementArea,
                PopupAlignmentPoint.TopLeft, popupSize);

            if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Bottom))
            {
                popupArea = PositionRects(actualTargetElement,
                    PopupAlignmentPoint.TopLeft, placementArea,
                    PopupAlignmentPoint.BottomLeft, popupSize);
            }

            if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Right))
            {
                popupArea = PositionRects(actualTargetElement,
                    PopupAlignmentPoint.TopLeft, placementArea,
                    PopupAlignmentPoint.TopRight, popupSize);
            }

            return popupArea;
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.Mouse"/>.
        /// </summary>
        private RectangleD CalculatePopupPosition_Mouse(ref RectangleD screenArea, Size2D popupSize)
        {
            UIElement actualTargetElement;

            var placementArea = GetPlacementTargetArea(out actualTargetElement, true);

            var popupArea = PositionRects(actualTargetElement,
                PopupAlignmentPoint.BottomLeft, placementArea,
                PopupAlignmentPoint.TopLeft, popupSize);

            if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Bottom))
            {
                popupArea = PositionRects(actualTargetElement,
                    PopupAlignmentPoint.TopLeft, placementArea,
                    PopupAlignmentPoint.BottomLeft, popupSize);
            }

            return popupArea;
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.MousePoint"/>.
        /// </summary>
        private RectangleD CalculatePopupPosition_MousePoint(ref RectangleD screenArea, Size2D popupSize)
        {
            UIElement actualTargetElement;

            var placementArea = GetPlacementTargetArea(out actualTargetElement, true);

            var popupArea = PositionRects(actualTargetElement,
                PopupAlignmentPoint.BottomLeft, placementArea,
                PopupAlignmentPoint.TopLeft, popupSize);

            if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Bottom))
            {
                popupArea = PositionRects(actualTargetElement,
                    PopupAlignmentPoint.BottomLeft, placementArea,
                    PopupAlignmentPoint.BottomLeft, popupSize);
            }

            if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Right))
            {
                popupArea = PositionRects(actualTargetElement,
                    PopupAlignmentPoint.BottomLeft, placementArea,
                    PopupAlignmentPoint.TopRight, popupSize);
            }
            
            return popupArea;
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.Left"/>.
        /// </summary>
        private RectangleD CalculatePopupPosition_Left(ref RectangleD screenArea, Size2D popupSize)
        {
            UIElement actualTargetElement;

            var placementArea = GetPlacementTargetArea(out actualTargetElement);

            var popupArea = PositionRects(actualTargetElement,
                PopupAlignmentPoint.TopLeft, placementArea,
                PopupAlignmentPoint.TopRight, popupSize);

            if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Left))
            {
                popupArea = PositionRects(actualTargetElement,
                    PopupAlignmentPoint.TopRight, placementArea,
                    PopupAlignmentPoint.TopLeft, popupSize);
            }

            return popupArea;
        }

        /// <summary>
        /// Calculates the popup's position when <see cref="Placement"/> is set to <see cref="PlacementMode.Top"/>.
        /// </summary>
        private RectangleD CalculatePopupPosition_Top(ref RectangleD screenArea, Size2D popupSize)
        {
            UIElement actualTargetElement;

            var placementArea = GetPlacementTargetArea(out actualTargetElement);

            var popupArea = PositionRects(actualTargetElement,
                PopupAlignmentPoint.TopLeft, placementArea,
                PopupAlignmentPoint.BottomLeft, popupSize);

            if (IsCrossingScreenEdge(ref screenArea, ref popupArea, PopupScreenEdges.Top))
            {
                popupArea = PositionRects(actualTargetElement,
                    PopupAlignmentPoint.BottomLeft, placementArea,
                    PopupAlignmentPoint.TopLeft, popupSize);
            }

            return popupArea;
        }

        /// <summary>
        /// Gets the bounds of the region around which the popup is placed.
        /// </summary>
        /// <param name="ignorePlacementTarget">A value indicating whether to ignore the value of the <see cref="PlacementTarget"/> property.</param>
        /// <returns>The bounds of the region around which the popup is placed.</returns>
        private RectangleD GetPlacementTargetArea(out UIElement target, Boolean ignorePlacementTarget = false)
        {
            target = null;

            var placement = Placement;
            if (placement == PlacementMode.Mouse || placement == PlacementMode.MousePoint)
                return GetMouseCursorArea();

            target = (PlacementTarget ?? VisualTreeHelper.GetParent(this)) as UIElement;
            if (target == null || ignorePlacementTarget)
            {
                target = null;
                return RectangleD.Empty;
            }
            else
            {
                var placementArea = PlacementRectangle;
                if (placementArea.IsEmpty)
                {
                    return target.Bounds;
                }
                target = null;
                return placementArea;
            }
        }

        /// <summary>
        /// Gets the area on the screen that is inhabited by the mouse cursor.
        /// </summary>
        /// <returns>The area in device-independent coordinates which is inhabited by the mouse cursor.</returns>
        private RectangleD GetMouseCursorArea()
        {
            var mouse = Ultraviolet.GetInput().GetMouse();
            if (mouse == null)
                return RectangleD.Empty;

            var cursor = Ultraviolet.GetPlatform().Cursor;

            var mousePos    = Display.PixelsToDips(mouse.Position);
            var mouseWidth  = Display.PixelsToDips(cursor == null ? DefaultCursorWidth : cursor.Width);
            var mouseHeight = Display.PixelsToDips(cursor == null ? DefaultCursorHeight : cursor.Height);

            return new RectangleD(mousePos.X, mousePos.Y, mouseWidth, mouseHeight);
        }

        /// <summary>
        /// Computes the values of the popup's transformation matrices.
        /// </summary>
        private void ComputeTransformationMatrices()
        {
            var rawTransform = GetTransformToAncestorMatrix((UIElement)VisualTreeHelper.GetRoot(this));

            transformToAncestor = new Matrix(
                rawTransform.M11, rawTransform.M12, rawTransform.M13, (Single)(popupPosition.X + alignmentX),
                rawTransform.M21, rawTransform.M22, rawTransform.M23, (Single)(popupPosition.Y + alignmentY),
                rawTransform.M31, rawTransform.M32, rawTransform.M33, 0,
                rawTransform.M41, rawTransform.M42, rawTransform.M44, 1);

            transformToAncestorWithOrigin = Matrix.Concat(
                Matrix.CreateTranslation(-(Single)popupTransformOrigin.X, -(Single)popupTransformOrigin.Y, 0), transformToAncestor);

            transformToAncestorInverse = Matrix.Invert(transformToAncestor);
        }

        // State values.
        private readonly PopupRoot root;
        private Boolean loadingDeferred;
        
        // The assumed size of the default cursor, since there's currently no way to query it.
        private const Int32 DefaultCursorWidth = 16;
        private const Int32 DefaultCursorHeight = 16;

        // Popup position values
        private Point2D popupTransformOrigin;
        private Point2D popupPosition;
        private Matrix transformToAncestor = Matrix.Identity;
        private Matrix transformToAncestorWithOrigin = Matrix.Identity;
        private Matrix transformToAncestorInverse = Matrix.Identity;
        private Double alignmentX;
        private Double alignmentY;
    }
}
