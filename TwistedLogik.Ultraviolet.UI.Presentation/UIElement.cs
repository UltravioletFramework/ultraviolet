using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;
using TwistedLogik.Ultraviolet.UI.Presentation.Media.Effects;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the method that is called when a class is added to or removed from a UI element.
    /// </summary>
    /// <param name="element">The UI element that raised the event.</param>
    /// <param name="classname">The name of the class that was added or removed.</param>
    public delegate void UIElementClassEventHandler(UIElement element, String classname);

    /// <summary>
    /// Represents the method that is called when a UI element is drawn.
    /// </summary>
    /// <param name="element">The element being drawn.</param>
    /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
    /// <param name="dc">The drawing context that describes the render state of the layout.</param>
    public delegate void UIElementDrawingEventHandler(UIElement element, UltravioletTime time, DrawingContext dc);

    /// <summary>
    /// Represents the method that is called when a UI element is updated.
    /// </summary>
    /// <param name="element">The element being updated.</param>
    /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
    public delegate void UIElementUpdatingEventHandler(UIElement element, UltravioletTime time);

    /// <summary>
    /// Represents the base class for all elements within the Ultraviolet Presentation Foundation.
    /// </summary>
    public abstract partial class UIElement : Visual
    {
        /// <summary>
        /// Initialies the <see cref="UIElement"/> type.
        /// </summary>
        static UIElement()
        {
            RegisterInputClassHandlers();

            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.QueryCursorEvent, new UpfQueryCursorEventHandler(OnQueryCursorProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Presentation.View.ViewModelChangedEvent, new UpfRoutedEventHandler(OnViewModelChangedProxy));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElement"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public UIElement(UltravioletContext uv)
        {
            Contract.Require(uv, "uv");

            var attr = (UvmlKnownTypeAttribute)GetType().GetCustomAttributes(typeof(UvmlKnownTypeAttribute), false).SingleOrDefault();

            this.uv = uv;
            this.classes = new UIElementClassCollection(this);
            this.uvmlName = (attr == null || attr.Name == null) ? GetType().Name : attr.Name;
        }
        
        /// <summary>
        /// Initializes the element's dependency properties and the dependency properties
        /// of any children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the dependency
        /// properties of this element's child elements.</param>
        public void InitializeDependencyProperties(Boolean recursive = true)
        {
            InitializeDependencyPropertiesCore(recursive);
        }

        /// <summary>
        /// Reloads this element's content and the content of any children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to reload the content
        /// of this element's child elements.</param>
        public void ReloadContent(Boolean recursive = true)
        {
            ReloadEffect();
            ReloadContentCore(recursive);
        }

        /// <summary>
        /// Clears any bindings which are set on dependency properties of this element, and optionally
        /// any bindings which are set on children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the bindings
        /// of this element's child elements.</param>
        public void ClearBindings(Boolean recursive = true)
        {
            ClearBindingsCore(recursive);
        }

        /// <summary>
        /// Clears the animations which are attached to this element, and optionally
        /// any animations attached to children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the animations
        /// of this element's child elements.</param>
        public void ClearAnimations(Boolean recursive = true)
        {
            ClearAnimationsCore(recursive);
        }

        /// <summary>
        /// Clears the local values of this element's dependency properties,
        /// and optionally the local values of any dependency properties belonging
        /// to children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the local dependency
        /// property values of this element's child elements.</param>
        public void ClearLocalValues(Boolean recursive = true)
        {
            ClearLocalValuesCore(recursive);
        }

        /// <summary>
        /// Clears the styled values of this element's dependency properties,
        /// and optionally the styled values of any dependency properties belonging
        /// to children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the styled dependency
        /// property values of this element's child elements.</param>
        public void ClearStyledValues(Boolean recursive = true)
        {
            ClearStyledValuesCore(recursive);
            isStyleValid = false;
        }

        /// <summary>
        /// Clears the triggered values of this element's dependency properties,
        /// and optionally the triggered values of any dependency properties belonging
        /// to children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the triggered dependency
        /// property values of this element's child elements.</param>
        public void ClearTriggeredValues(Boolean recursive = true)
        {
            ClearTriggeredValuesCore(recursive);
        }

        /// <summary>
        /// Draws the element using the specified <see cref="DrawingContext"/>.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        public void Draw(UltravioletTime time, DrawingContext dc)
        {
            EnsureOutOfBandRenderTargetsExist();

            var clip = ClipRectangle;
            if (clip != null)
                dc.PushClipRectangle(clip.Value);

            var shouldDraw = true;

            var popup = this as Popup;
            if (popup == null || View.Popups.IsDrawingPopup(popup))
            {
                if (clip.HasValue && clip.Value.IsEmpty)
                    shouldDraw = false;
            }

            if (shouldDraw)
            {
                var drawingOutOfBand = false;
                if (!dc.IsInsideOutOfBandElement && DrawOutOfBandTexture(dc))
                {
                    drawingOutOfBand = true;
                    dc.PushDrawingOutOfBand();
                }

                var forceFullOpacity = false;

                var upf = Ultraviolet.GetUI().GetPresentationFoundation();
                if (upf.OutOfBandRenderer.IsDrawingRenderTargetFor(this))
                    forceFullOpacity = true;

                if (!forceFullOpacity)
                    dc.PushOpacity(Opacity);

                var hasNonIdentityTransform = HasNonIdentityTransform;
                if (hasNonIdentityTransform)
                {
                    dc.PushTransform();
                }

                var state = dc.GetCurrentState();
                var flush = false;

                if (this is PopupRoot)
                {
                    var parentPopup = this.Parent as Popup;
                    if (parentPopup != null)
                    {
                        var transformMatrix = parentPopup.PopupTransformToViewInDevicePixels;
                        var transformIsNotIdentity = !transformMatrix.Equals(Matrix.Identity);
                        if (transformIsNotIdentity)
                        {
                            dc.End();
                            dc.Begin(SpriteSortMode.Deferred, null, transformMatrix);

                            flush = true;
                        }
                    }
                }
                else
                {
                    if (HasNonIdentityTransform)
                    {
                        var mtxTransformParent = state.LocalTransform;
                        var mtxTransformBatch = GetVisualTransformMatrix(ref mtxTransformParent);

                        dc.End();
                        dc.Begin(SpriteSortMode.Deferred, null, mtxTransformBatch);

                        flush = true;
                    }
                }

                if (dc.IsInsideOutOfBandElement)
                {
                    RegisterPopupsInVisualSubTree(time, dc);
                }
                else
                {
                    DrawCore(time, dc);
                    OnDrawing(time, dc);
                }

                if (flush)
                {
                    dc.End();
                    dc.Begin(SpriteSortMode.Deferred, null, state.LocalTransform);
                }

                if (hasNonIdentityTransform)
                {
                    dc.PopTransform();
                }

                if (!forceFullOpacity)
                    dc.PopOpacity();

                if (drawingOutOfBand)
                    dc.PopDrawingOutOfBand();
            }

            if (clip != null)
                dc.PopClipRectangle();
        }

        /// <summary>
        /// Draws the element to a render target using the specified <see cref="DrawingContext"/>.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        /// <param name="target">The render target to which to draw the element.</param>
        /// <param name="transform">The transformation matrix to apply to the element, or <c>null</c> to use the cumulative sprite batch transformation.</param>
        public void DrawToRenderTarget(UltravioletTime time, DrawingContext dc, Graphics.RenderTarget2D target, Matrix? transform = null)
        {
            Contract.Require(dc, "dc");
            Contract.Require(target, "target");

            var graphics = Ultraviolet.GetGraphics();
            graphics.SetRenderTarget(target);
            graphics.Clear(Color.Transparent);

            var bounds = Display.DipsToPixels(TransformedVisualBounds);
            var x = (Int32)bounds.X + (bounds.Width - target.Width) / 2.0;
            var y = (Int32)bounds.Y + (bounds.Height - target.Height) / 2.0;

            var visualBounds = (Vector2)new Point2D(x, y);
            dc.GlobalTransform = Matrix.CreateTranslation(-visualBounds.X, -visualBounds.Y, 0);
            dc.Begin(SpriteSortMode.Deferred, null, transform ?? GetVisualTransformMatrix());

            Draw(time, dc);

            dc.End();
            dc.GlobalTransform = Matrix.Identity;

            graphics.SetRenderTarget(null);
        }

        /// <summary>
        /// Updates the element's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            Digest(time);
            
            UpdateCore(time);
            OnUpdating(time);
        }

        /// <summary>
        /// Immediately updates the element's layout.
        /// </summary>
        public void UpdateLayout()
        {
            if (!IsMeasureValid && !IsArrangeValid)
                return;

            Measure(MostRecentAvailableSize);
            Arrange(MostRecentFinalRect);
        }

        /// <summary>
        /// Performs cleanup operations and releases any internal framework resources.
        /// The object remains usable after this method is called, but certain aspects
        /// of its state, such as animations, may be reset.
        /// </summary>
        public void Cleanup()
        {
            ClearAnimations(false);
            ClearTriggeredValues();

            CleanupOutOfBandTargets();
            CleanupStoryboards();
            CleanupCore();
        }

        /// <summary>
        /// Caches layout parameters related to the element's position within the element hierarchy.
        /// </summary>
        public void CacheLayoutParameters()
        {
            if (suppressCacheLayoutParameters)
                return;

            var view = View;
            
            CacheLayoutDepth();
            CacheView();

            if (View != null && View.LayoutRoot.IsLoaded)
                EnsureOutOfBandRenderTargetsExist();

            CacheLayoutParametersCore();
        }

        /// <summary>
        /// Applies the specified style sheet to this element.
        /// </summary>
        /// <param name="styleSheet">The style sheet to apply to this element.</param>
        public void Style(UvssDocument styleSheet)
        {
            if (View == null)
            {
                this.isStyleValid = true;
                return;
            }

            if (isStyleValid && mostRecentStyleSheet == styleSheet)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.StyleCount++;

            this.mostRecentStyleSheet = styleSheet;

            isStyling = true;
            try
            {
                ApplyStyles(styleSheet);
                StyleCore(styleSheet);
                ReloadContent(false);
            }
            finally
            {
                isStyling = false;
            }

            this.isStyleValid = true;

            InvalidateMeasure();

            upf.StyleQueue.Remove(this);
        }

        /// <summary>
        /// Calculates the element's desired size.
        /// </summary>
        /// <param name="availableSize">The size of the area which the element's parent has 
        /// specified is available for the element's layout.</param>
        public void Measure(Size2D availableSize)
        {
            if (availableSize.Width < 0 || availableSize.Height < 0)
                availableSize = Size2D.Zero;

            if (View == null)
            {
                this.isMeasureValid = true;
                return;
            }

            if (isMeasureValid && mostRecentAvailableSize.Equals(availableSize))
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.MeasureCount++;

            this.mostRecentAvailableSize = availableSize;

            var desiredSizeChanged = false;
            var desiredSize        = Size2D.Zero;

            isMeasuring = true;
            try
            {
                desiredSize = MeasureCore(availableSize);
            }
            finally
            {
                isMeasuring = false;
            }

            if (Double.IsPositiveInfinity(desiredSize.Width) || Double.IsPositiveInfinity(desiredSize.Height))
                throw new InvalidOperationException(PresentationStrings.MeasureMustProduceFiniteDesiredSize);

            if (!this.desiredSize.Equals(desiredSize))
                desiredSizeChanged = true;

            this.desiredSize    = desiredSize;
            this.isMeasureValid = true;

            InvalidateArrange();

            upf.MeasureQueue.Remove(this);

            if (desiredSizeChanged)
            {
                IndicateDesiredSizeChanged();
            }
        }

        /// <summary>
        /// Sets the element's final area relative to its parent and arranges
        /// the element's children within its layout area.
        /// </summary>
        /// <param name="finalRect">The element's final position and size relative to its parent element.</param>
        /// <param name="options">A set of <see cref="ArrangeOptions"/> values specifying the options for this arrangement.</param>
        public void Arrange(RectangleD finalRect, ArrangeOptions options = ArrangeOptions.None)
        {
            Contract.EnsureRange(finalRect.Width >= 0 && finalRect.Height >= 0, "finalRect");

            if (View == null)
            {
                this.isArrangeValid = true;
                return;
            }

            if (isArrangeValid && mostRecentFinalRect.Equals(finalRect) && ((Int32)mostRecentArrangeOptions).Equals((Int32)options))
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.ArrangeCount++;

            this.mostRecentArrangeOptions = options;
            this.mostRecentFinalRect = finalRect;

            if (Visibility == Visibility.Collapsed)
            {
                this.renderSize = Size2.Zero;
            }
            else
            {
                isArranging = true;
                try
                {
                    finalRect = PerformLayoutRounding(finalRect);

                    this.renderSize = ArrangeCore(finalRect, options);
                    this.renderSize = PerformLayoutRounding(this.renderSize);

                    SetValue(ActualWidthPropertyKey, this.renderSize.Width);
                    SetValue(ActualHeightPropertyKey, this.renderSize.Height);
                }
                finally
                {
                    isArranging = false;
                }
            }
            this.isArrangeValid = true;

            var forceInvalidatePosition = this.forceInvalidatePosition || ((options & ArrangeOptions.ForceInvalidatePosition) == ArrangeOptions.ForceInvalidatePosition);
            PositionElementAndPotentiallyChildren(forceInvalidatePosition);

            upf.ArrangeQueue.Remove(this);

            InvalidateVisualBounds();
        }

        /// <summary>
        /// Updates the element's position in absolute screen space.
        /// </summary>
        /// <param name="offset">The amount by which to offset the element's position from its desired position.</param>
        public void Position(Size2D offset)
        {
            if (View == null)
                return;

            mostRecentPositionOffset = offset;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.PositionCount++;

            var parent         = VisualTreeHelper.GetParent(this) as UIElement;
            var parentPosition = (parent == null) ? Point2D.Zero : parent.UntransformedAbsolutePosition;

            var totalOffsetX = mostRecentFinalRect.X + RenderOffset.X + offset.Width;
            var totalOffsetY = mostRecentFinalRect.Y + RenderOffset.Y + offset.Height;
            var totalOffset  = new Size2D(totalOffsetX, totalOffsetY);

            this.untransformedRelativeBounds = new RectangleD(Point2D.Zero + totalOffset, RenderSize);
            this.untransformedAbsoluteBounds = new RectangleD(parentPosition + totalOffset, RenderSize);

            PositionCore();

            Clip();

            InvalidateVisualBounds();
        }

        /// <summary>
        /// Updates the positions of the element's visual children in absolute screen space.
        /// </summary>
        public void PositionChildren()
        {
            PositionChildrenCore();
        }

        /// <summary>
        /// Calculates the clipping rectangle for the element.
        /// </summary>
        public void Clip()
        {
            var clip = ClipCore();
            if (clip.HasValue)
            {
                var clipValue = clip.Value;
                if (clipValue.Width < 0 || clipValue.Height < 0)
                {
                    throw new InvalidOperationException(PresentationStrings.ClipRectangleMustHaveValidDimensions);
                }
            }
            clipRectangle = clip;
        }

        /// <summary>
        /// Invalidates the element's styling state.
        /// </summary>
        public void InvalidateStyle(Boolean recursive = false)
        {
            if (View == null)
                return;

            if (IsStyleValid && !IsStyling)
            {
                this.isStyleValid = false;

                var upf = uv.GetUI().GetPresentationFoundation();
                upf.PerformanceStats.InvalidateStyleCount++;
                upf.StyleQueue.Enqueue(this);
            }

            if (recursive)
            {
                VisualTreeHelper.ForEachChild<UIElement>(this, null, (child, state) =>
                {
                    child.InvalidateStyle(true);
                });
            }
        }

        /// <summary>
        /// Invalidates the element's measurement state.
        /// </summary>
        public void InvalidateMeasure()
        {
            if (View == null || !IsMeasureValid || IsMeasuring)
                return;

            this.isMeasureValid = false;

            var upf = uv.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.InvalidateMeasureCount++;
            upf.MeasureQueue.Enqueue(this);
        }

        /// <summary>
        /// Invalidates the element's arrangement state.
        /// </summary>
        /// <param name="forceInvalidatePosition">A value indicating whether to force the element to reposition its children.</param>
        public void InvalidateArrange(Boolean forceInvalidatePosition = false)
        {
            if (View == null || !IsArrangeValid || IsArranging)
                return;

            this.isArrangeValid = false;
            this.forceInvalidatePosition = forceInvalidatePosition;

            var upf = uv.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.InvalidateArrangeCount++;
            upf.ArrangeQueue.Enqueue(this);
        }

        /// <summary>
        /// Adds a handler for a routed event to the element.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> that identifies the routed event for which to add a handler.</param>
        /// <param name="handler">A delegate that represents the handler to add to the element for the specified routed event.</param>
        public void AddHandler(RoutedEvent evt, Delegate handler)
        {
            Contract.Require(evt, "evt");
            Contract.Require(handler, "handler");

            AddHandler(evt, handler, false);
        }

        /// <summary>
        /// Adds a handler for a routed event to the element.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> that identifies the routed event for which to add a handler.</param>
        /// <param name="handler">A delegate that represents the handler to add to the element for the specified routed event.</param>
        /// <param name="handledEventsToo">A value indicating whether the handler should receive events which have already been handled by other handlers.</param>
        public void AddHandler(RoutedEvent evt, Delegate handler, Boolean handledEventsToo)
        {
            Contract.Require(evt, "evt");
            Contract.Require(handler, "handler");

            routedEventManager.Add(evt, handler, handledEventsToo);
        }

        /// <summary>
        /// Removes a handler for a routed event from the element.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> that identifies the routed event for which to remove a handler.</param>
        /// <param name="handler">A delegate that represents the handler to remove from the element for the specified routed event.</param>
        public void RemoveHandler(RoutedEvent evt, Delegate handler)
        {
            Contract.Require(evt, "evt");
            Contract.Require(handler, "handler");

            routedEventManager.Remove(evt, handler);
        }

        /// <summary>
        /// Requests that focus be moved from this element to another element.
        /// </summary>
        /// <param name="direction">The direction in which to move focus.</param>
        /// <returns><c>true</c> if focus was moved successfully; otherwise, <c>false</c>.</returns>
        public virtual Boolean MoveFocus(FocusNavigationDirection direction)
        {
            return false;
        }

        /// <inheritdoc/>
        public Boolean Focus()
        {
            if (View == null)
                return false;

            if (!View.FocusElement(this))
            {
                if (Keyboard.IsFocusable(this))
                {
                    var focusScope = FocusManager.GetFocusScope(this);
                    var focusScopeElement = FocusManager.GetFocusedElement(focusScope);
                    if (focusScopeElement == null)
                    {
                        FocusManager.SetFocusedElement(focusScope, this);
                    }
                }
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Captures the mouse to this element.
        /// </summary>
        /// <returns><c>true</c> if the mouse was successfully captured; otherwise, <c>false</c>.</returns>
        public Boolean CaptureMouse()
        {
            return View != null && Mouse.Capture(View, this);
        }

        /// <summary>
        /// Releases mouse capture from this element.
        /// </summary>
        public void ReleaseMouseCapture()
        {
            if (View != null && Mouse.GetCaptured(View) == this)
            {
                Mouse.Capture(View, null);
            }
        }

        /// <summary>
        /// Gets the Ultraviolet context that created this element.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get { return uv; }
        }

        /// <summary>
        /// Gets the collection of styling classes associated with this element.
        /// </summary>
        public UIElementClassCollection Classes
        {
            get { return classes; }
        }

        /// <summary>
        /// Gets the name of this element's type within UVML markup.
        /// </summary>
        public String UvmlName
        {
            get { return uvmlName; }
        }

        /// <summary>
        /// Gets the element's associated view.
        /// </summary>
        public PresentationFoundationView View
        {
            get { return view; }
            internal set { view = value; }
        }

        /// <summary>
        /// Gets the element's associated view model.
        /// </summary>
        public Object ViewModel
        {
            get { return view == null ? null : View.ViewModel; }
        }

        /// <summary>
        /// Gets the element's parent element.
        /// </summary>
        public UIElement Parent
        {
            get { return parent; }
            internal set
            {
                if (parent != value)
                {
                    parent = value;
                    OnLogicalParentChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this element has a transform applied to it.
        /// </summary>
        public Boolean HasTransform
        {
            get { return HasLayoutTransform || HasRenderTransform; }
        }

        /// <summary>
        /// Gets a value indicating whether the element's styling state is valid.
        /// </summary>
        public Boolean IsStyleValid
        {
            get { return isStyleValid; }
        }

        /// <summary>
        /// Gets a value indicating whether the element's arrangement state is valid.
        /// </summary>
        public Boolean IsArrangeValid
        {
            get { return isArrangeValid; }
        }

        /// <summary>
        /// Gets a value indicating whether the element's measurement state is valid.
        /// </summary>
        public Boolean IsMeasureValid
        {
            get { return isMeasureValid; }
        }

        /// <summary>
        /// Gets a value indicating whether the element is visible.
        /// </summary>
        public Boolean IsVisible
        {
            get { return GetValue<Boolean>(IsVisibleProperty); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is enabled.
        /// </summary>
        public Boolean IsEnabled
        {
            get { return GetValue<Boolean>(IsEnabledProperty); }
            set { SetValue<Boolean>(IsEnabledProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether this element has logical focus.
        /// </summary>
        public Boolean IsFocused
        {
            get { return GetValue<Boolean>(IsFocusedProperty); }
        }

        /// <summary>
        /// Gets a value indicating whether this element should display a focus visual, if it has one.
        /// </summary>
        public Boolean IsFocusVisualVisible
        {
            get
            {
                if (View == null)
                    return false;

                return IsKeyboardFocused && view.FocusWasMostRecentlyChangedByKeyboardOrGamePad;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is visible to hit tests.
        /// </summary>
        public Boolean IsHitTestVisible
        {
            get { return GetValue<Boolean>(IsHitTestVisibleProperty); }
            set { SetValue<Boolean>(IsHitTestVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element can receive input focus.
        /// </summary>
        public Boolean Focusable
        {
            get { return GetValue<Boolean>(FocusableProperty); }
            set { SetValue<Boolean>(FocusableProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value specifying the element's visibility state.
        /// </summary>
        public Visibility Visibility
        {
            get { return GetValue<Visibility>(VisibilityProperty); }
            set { SetValue<Visibility>(VisibilityProperty, value); }
        }

        /// <summary>
        /// Gets the untransformed position of the element relative to its parent as of the
        /// last call to the <see cref="Position(Size2D)"/> method.
        /// </summary>
        /// <remarks>Untransformed bounds are computed as if no transforms have been applied to the element or any of its ancestors.</remarks>
        public Point2D UntransformedRelativePosition
        {
            get { return untransformedRelativeBounds.Location; }
        }

        /// <summary>
        /// Gets the untransformed position of the element in absolute screen coordinates as of the
        /// last call to the <see cref="Position(Size2D)"/> method.
        /// </summary>
        /// <remarks>Untransformed bounds are computed as if no transforms have been applied to the element or any of its ancestors.</remarks>
        public Point2D UntransformedAbsolutePosition
        {
            get { return untransformedAbsoluteBounds.Location; }
        }
        
        /// <summary>
        /// Gets or sets the offset from the top-left corner of the element's layout
        /// area to the top-left corner of the element itself.
        /// </summary>
        public Point2D RenderOffset
        {
            get { return renderOffset; }
            protected set { renderOffset = value; }
        }

        /// <summary>
        /// Gets the final render size of the element.
        /// </summary>
        public Size2D RenderSize
        {
            get
            {
                if (Visibility == Visibility.Collapsed)
                {
                    return Size2D.Zero;
                }
                return renderSize;
            }
        }

        /// <summary>
        /// Gets the element's desired size as calculated during the most recent measure pass.
        /// </summary>
        public Size2D DesiredSize
        {
            get
            {
                if (Visibility == Visibility.Collapsed)
                {
                    return Size2D.Zero;
                }
                return desiredSize;
            }
        }
        
        /// <summary>
        /// Gets the element's bounds in client space.
        /// </summary>
        public RectangleD Bounds
        {
            get { return new RectangleD(0, 0, RenderSize.Width, RenderSize.Height); }
        }

        /// <summary>
        /// Gets the element's untransformed bounds relative to its parent element.
        /// </summary>
        /// <remarks>Untransformed bounds are computed as if no transforms have been applied to the element or any of its ancestors.</remarks>
        public RectangleD UntransformedRelativeBounds
        {
            get { return untransformedRelativeBounds; }
        }

        /// <summary>
        /// Gets the element's untransformed bounds in absolute screen space.
        /// </summary>
        /// <remarks>Untransformed bounds are computed as if no transforms have been applied to the element or any of its ancestors.</remarks>
        public RectangleD UntransformedAbsoluteBounds
        {
            get { return untransformedAbsoluteBounds; }
        }

        /// <summary>
        /// Gets the element's visual bounds in client space.
        /// </summary>
        public RectangleD VisualBounds
        {
            get
            {
                if (visualBounds.HasValue)
                    return visualBounds.Value;

                visualBounds = CalculateVisualBounds();
                return visualBounds.Value;
            }
        }
        
        /// <summary>
        /// Gets the element's transformed visual bounds in screen space.
        /// </summary>
        public RectangleD TransformedVisualBounds
        {
            get
            {
                if (transformedVisualBounds.HasValue)
                    return transformedVisualBounds.Value;

                transformedVisualBounds = CalculateTransformedVisualBounds();
                return transformedVisualBounds.Value;
            }
        }

        /// <summary>
        /// Gets the element's clipping rectangle. A value of <c>null</c> indicates that
        /// clipping is disabled for this element.
        /// </summary>
        public RectangleD? ClipRectangle
        {
            get { return clipRectangle; }
        }

        /// <summary>
        /// Gets or sets the opacity of the element and its children.
        /// </summary>
        public Single Opacity
        {
            get { return GetValue<Single>(OpacityProperty); }
            set { SetValue<Single>(OpacityProperty, value); }
        }

        /// <summary>
        /// Gets the rendered width of this element.
        /// </summary>
        public Double ActualWidth
        {
            get { return GetValue<Double>(ActualWidthProperty); }
        }

        /// <summary>
        /// Gets the rendered height of this element.
        /// </summary>
        public Double ActualHeight
        {
            get { return GetValue<Double>(ActualHeightProperty); }
        }

        /// <summary>
        /// Gets or sets the element's rendering transformation.
        /// </summary>
        public Transform RenderTransform
        {
            get { return GetValue<Transform>(RenderTransformProperty); }
            set { SetValue<Transform>(RenderTransformProperty, value); }
        }

        /// <summary>
        /// Gets or sets the relative center point of render transforms applied to this element.
        /// </summary>
        public Point2D RenderTransformOrigin
        {
            get { return GetValue<Point2D>(RenderTransformOriginProperty); }
            set { SetValue<Point2D>(RenderTransformOriginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="Effect"/> which is applied to this element.
        /// </summary>
        public Effect Effect
        {
            get { return GetValue<Effect>(EffectProperty); }
            set { SetValue(EffectProperty, value); }
        }

        /// <summary>
        /// Occurs after the Presentation Foundation has finished a layout pass.
        /// </summary>
        public event EventHandler LayoutUpdated
        {
            add
            {
                var oldNull = (layoutUpdated == null);
                layoutUpdated += value;
                var newNull = (layoutUpdated == null);

                if (oldNull && !newNull)
                {
                    Ultraviolet.GetUI().GetPresentationFoundation().RegisterForLayoutUpdated(this);
                }
            }
            remove
            {
                var oldNull = (layoutUpdated == null);
                layoutUpdated -= value;
                var newNull = (layoutUpdated == null);

                if (!oldNull && newNull)
                {
                    Ultraviolet.GetUI().GetPresentationFoundation().UnregisterForLayoutUpdated(this);
                }
            }
        }

        /// <summary>
        /// Occurs when a class is added to the element.
        /// </summary>
        public event UIElementClassEventHandler ClassAdded;

        /// <summary>
        /// Occurs when a class is removed from the element.
        /// </summary>
        public event UIElementClassEventHandler ClassRemoved;

        /// <summary>
        /// Occurs when the element is being drawn.
        /// </summary>
        public event UIElementDrawingEventHandler Drawing;

        /// <summary>
        /// Occurs when the element is being updated.
        /// </summary>
        public event UIElementUpdatingEventHandler Updating;

        /// <summary>
        /// Occurs when the value of the <see cref="Visibility"/> property changes.
        /// </summary>
        public event UpfEventHandler IsVisibleChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsEnabled"/> property changes.
        /// </summary>
        public event UpfEventHandler IsEnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsHitTestVisible"/> property changes.
        /// </summary>
        public event UpfEventHandler IsHitTestVisibleChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Focusable"/> dependency property changes.
        /// </summary>
        public event UpfEventHandler FocusableChanged;

        /// <summary>
        /// Occurs when the element is queried to determine which cursor to display.
        /// </summary>
        public event UpfQueryCursorEventHandler QueryCursor
        {
            add { AddHandler(Mouse.QueryCursorEvent, value); }
            remove { RemoveHandler(Mouse.QueryCursorEvent, value); }
        }

        /// <summary>
        /// The private access key for the <see cref="ActualWidth"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey ActualWidthPropertyKey = DependencyProperty.RegisterReadOnly("ActualWidth", typeof(Double), typeof(UIElement),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero));

        /// <summary>
        /// Identifies the <see cref="ActualWidth"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'actual-width'.</remarks>
        public static readonly DependencyProperty ActualWidthProperty = ActualWidthPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="ActualHeight"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey ActualHeightPropertyKey = DependencyProperty.RegisterReadOnly("ActualHeight", typeof(Double), typeof(UIElement),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero));

        /// <summary>
        /// Identifies the <see cref="ActualHeight"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'actual-height'.</remarks>
        public static readonly DependencyProperty ActualHeightProperty = ActualHeightPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="IsVisible"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsVisiblePropertyKey = DependencyProperty.RegisterReadOnly("IsVisible", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.True, HandleIsVisibleChanged));

        /// <summary>
        /// Identifies the <see cref="IsVisible"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'visible'.</remarks>
        public static readonly DependencyProperty IsVisibleProperty = IsVisiblePropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="IsEnabled"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'enabled'.</remarks>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.True, HandleIsEnabledChanged, CoerceIsEnabled));

        /// <summary>
        /// Identifies the <see cref="IsHitTestVisible"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'hit-test-visible'.</remarks>
        public static readonly DependencyProperty IsHitTestVisibleProperty = DependencyProperty.Register("IsHitTestVisible", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.True, HandleIsHitTestVisibleChanged));

        /// <summary>
        /// The private access key for the <see cref="IsFocused"/> read-only dependency property.
        /// </summary>
        internal static readonly DependencyPropertyKey IsFocusedPropertyKey = DependencyProperty.RegisterReadOnly("IsFocused", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="IsFocused"/> dependency property.
        /// </summary>
        /// <remarks>The styling name for this dependency property is 'focused'.</remarks>
        public static readonly DependencyProperty IsFocusedProperty = IsFocusedPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="Focusable"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'focusable'.</remarks>
        public static readonly DependencyProperty FocusableProperty = DependencyProperty.Register("Focusable", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, HandleFocusableChanged));

        /// <summary>
        /// Identifies the <see cref="Visibility"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'visibility'.</remarks>
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(UIElement),
            new PropertyMetadata<Visibility>(PresentationBoxedValues.Visibility.Visible, PropertyMetadataOptions.AffectsArrange, HandleVisibilityChanged));

        /// <summary>
        /// Identifies the <see cref="Opacity"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'opacity'.</remarks>
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register("Opacity", typeof(Single), typeof(UIElement),
            new PropertyMetadata<Single>(CommonBoxedValues.Single.One));

        /// <summary>
        /// Identifies the <see cref="RenderTransform"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RenderTransformProperty = DependencyProperty.Register("RenderTransform", typeof(Transform), typeof(UIElement),
            new PropertyMetadata<Transform>(Transform.Identity, PropertyMetadataOptions.AffectsVisualBounds, HandleRenderTransformChanged));

        /// <summary>
        /// Identifies the <see cref="RenderTransformOrigin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RenderTransformOriginProperty = DependencyProperty.Register("RenderTransformOrigin", typeof(Point2D), typeof(UIElement),
            new PropertyMetadata<Point2D>(new Point2D(0.5, 0.5), PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="Effect"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EffectProperty = DependencyProperty.Register("Effect", typeof(Effect), typeof(UIElement),
            new PropertyMetadata<Effect>(null, PropertyMetadataOptions.None, HandleEffectChanged));

        /// <summary>
        /// Identifies the GotFocus routed event.
        /// </summary>
        public static readonly RoutedEvent GotFocusEvent = FocusManager.GotFocusEvent.AddOwner(typeof(UIElement));

        /// <summary>
        /// Identifies the LostFocus routed event.
        /// </summary>
        public static readonly RoutedEvent LostFocusEvent = FocusManager.LostFocusEvent.AddOwner(typeof(UIElement));

        /// <summary>
        /// Occurs when the element receives logical focus.
        /// </summary>
        public event UpfRoutedEventHandler GotFocus
        {
            add { AddHandler(GotFocusEvent, value); }
            remove { RemoveHandler(GotFocusEvent, value); }
        }

        /// <summary>
        /// Occurs when the element loses logical focus.
        /// </summary>
        public event UpfRoutedEventHandler LostFocus
        {
            add { AddHandler(LostFocusEvent, value); }
            remove { RemoveHandler(LostFocusEvent, value); }
        }

        /// <inheritdoc/>
        internal override void OnVisualParentChangedInternal(Visual oldParent, Visual newParent)
        {
            CacheLayoutParameters();
            
            if (VisualParent != null)
            {
                InvalidateStyle();

                var parent = VisualParent as UIElement;
                if (parent != null && parent.MostRecentStyleSheet != null)
                {
                    Style(parent.MostRecentStyleSheet);
                }
            }

            base.OnVisualParentChangedInternal(oldParent, newParent);
        }
        
        /// <summary>
        /// Applies a visual state transition to the element.
        /// </summary>
        /// <param name="style">The style which defines the state transition.</param>
        internal virtual void ApplyStyledVisualStateTransition(UvssRule style)
        {

        }
        
        /// <summary>
        /// Performs a union between the specified visual bounds and the visual bounds of the element's children, then
        /// applies the element's clipping rectangle, if it has one.
        /// </summary>
        /// <param name="absoluteVisualBounds">The visual bounds to extend and clip.</param>
        /// <param name="clipTransformMatrix">The transformation matrix to apply to the clip rectangle.</param>
        /// <returns>The extended and clipped visual bounds.</returns>
        internal RectangleD UnionAbsoluteVisualBoundsWithChildrenAndApplyClipping(RectangleD absoluteVisualBounds, ref Matrix clipTransformMatrix)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(this);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(this, i) as UIElement;
                if (child != null && !(child is Popup))
                {
                    var childBounds = child.TransformedVisualBounds;
                    if (childBounds.IsEmpty)
                        continue;

                    RectangleD.Union(ref absoluteVisualBounds, ref childBounds, out absoluteVisualBounds);
                }
            }

            if (ClipRectangle.HasValue)
            {
                var relativeClip = ClipRectangle.Value - UntransformedAbsolutePosition;
                RectangleD.TransformAxisAligned(ref relativeClip, ref clipTransformMatrix, out relativeClip);
                RectangleD.Intersect(ref absoluteVisualBounds, ref relativeClip, out absoluteVisualBounds);
            }

            return absoluteVisualBounds;
        }

        /// <summary>
        /// Raises the <see cref="LayoutUpdated"/> event.
        /// </summary>
        internal void RaiseLayoutUpdated()
        {
            var temp = this.layoutUpdated;
            if (temp != null)
            {
                temp(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Changes the element's logical and visual parents.
        /// </summary>
        /// <param name="logicalParent">The element's new logical parent.</param>
        /// <param name="visualParent">The element's new visual parent.</param>
        internal void ChangeLogicalAndVisualParents(UIElement logicalParent, Visual visualParent)
        {
            suppressCacheLayoutParameters = true;
            try
            {
                if (this.Parent != logicalParent)
                {
                    if (this.Parent != null)
                        this.Parent = null;

                    this.Parent = logicalParent;
                }

                if (this.VisualParent != visualParent)
                {
                    if (this.VisualParent != null)
                        this.VisualParent.RemoveVisualChild(this);

                    if (visualParent != null)
                        visualParent.AddVisualChild(this);
                }
            }
            finally
            {
                suppressCacheLayoutParameters = false;
            }
            CacheLayoutParameters();
        }

        /// <summary>
        /// Changes the element's logical parent.
        /// </summary>
        /// <param name="logicalParent">The element's new logical parent.</param>
        internal void ChangeLogicalParent(UIElement logicalParent)
        {
            if (this.Parent == logicalParent)
                return;

            if (this.Parent != null)
                this.Parent = null;

            this.Parent = logicalParent;
        }

        /// <summary>
        /// Changes the element's visual parent.
        /// </summary>
        /// <param name="visualParent">The element's new visual parent.</param>
        internal void ChangeVisualParent(Visual visualParent)
        {
            if (this.VisualParent == visualParent)
                return;

            if (this.VisualParent != null)
                this.VisualParent.RemoveVisualChild(this);

            if (visualParent != null)
                visualParent.AddVisualChild(this);
        }

        /// <summary>
        /// Invalidates the element's style.
        /// </summary>
        internal void InvalidateStyleInternal()
        {
            isStyleValid = false;
        }

        /// <summary>
        /// Invalidates the element's measure.
        /// </summary>
        internal void InvalidateMeasureInternal()
        {
            isMeasureValid = false;
        }

        /// <summary>
        /// Invalidates the element's arrangement.
        /// </summary>
        internal void InvalidateArrangeInternal()
        {
            isArrangeValid = false;
        }

        /// <summary>
        /// Begins the specified storyboard on this element.
        /// </summary>
        /// <param name="storyboard">The storyboard to begin on this element.</param>
        internal void BeginStoryboard(Storyboard storyboard)
        {
            UpfPool<StoryboardInstance>.PooledObject existingInstance;
            storyboardInstances.TryGetValue(storyboard, out existingInstance);
            
            var storyboardInstance = StoryboardInstancePool.Instance.Retrieve(this);
            var storyboardClock = StoryboardClockPool.Instance.Retrieve(storyboardInstance.Value);
            storyboardInstance.Value.AssociateWith(storyboard, storyboardClock, this);

            storyboardInstances[storyboard] = storyboardInstance;

            Animate(storyboardInstance.Value);
            storyboardInstance.Value.Start();

            if (existingInstance != null)
            {
                existingInstance.Value.Stop();
                StoryboardInstancePool.Instance.Release(existingInstance);
            }
        }

        /// <summary>
        /// Stops the specified storyboard on this element.
        /// </summary>
        /// <param name="storyboard">The storyboard to stop on this element.</param>
        internal void StopStoryboard(Storyboard storyboard)
        {
            UpfPool<StoryboardInstance>.PooledObject storyboardInstance;
            if (storyboardInstances.TryGetValue(storyboard, out storyboardInstance))
            {
                storyboardInstance.Value.Stop();
                StoryboardInstancePool.Instance.Release(storyboardInstance);

                storyboardInstances.Remove(storyboard);
            }
        }

        /// <summary>
        /// Animates this element using the specified storyboard.
        /// </summary>
        /// <param name="storyboardInstance">The storyboard instance for which to animate this element.</param>
        internal void Animate(StoryboardInstance storyboardInstance)
        {
            Contract.Require(storyboardInstance, "storyboardInstance");

            foreach (var target in storyboardInstance.Storyboard.Targets)
            {
                var targetAppliesToElement = false;
                if (target.Selector == null)
                {
                    if (this == storyboardInstance.Target)
                    {
                        targetAppliesToElement = true;
                    }
                }
                else
                {
                    targetAppliesToElement = target.Selector.MatchesElement(this, storyboardInstance.Target);
                }

                if (targetAppliesToElement)
                {
                    foreach (var animation in target.Animations)
                    {
                        var propertyName = animation.Key.PropertyName;

                        var dpSource = animation.Key.NavigationExpression.HasValue ?
                            animation.Key.NavigationExpression.Value.ApplyExpression(Ultraviolet, this) : this;
                        if (dpSource != null)
                        {
                            var dp = DependencyProperty.FindByName(Ultraviolet, dpSource, propertyName.Owner, propertyName.Name);
                            if (dp != null)
                            {
                                dpSource.EnlistDependencyPropertyInStoryboard(dp, storyboardInstance, animation.Value);
                            }
                        }
                    }
                }
            }

            AnimateCore(storyboardInstance);
        }

        /// <summary>
        /// Gets the transformation matrix which is passed into the element's sprite batch prior to rendering the element.
        /// </summary>
        /// <param name="mtxParentTransform">The visual transform of the element's parent.</param>
        /// <returns>The transformation matrix which is passed into the element's sprite batch prior to rendering the element.</returns>
        internal Matrix GetVisualTransformMatrix(ref Matrix mtxParentTransform)
        {
            var pixPosition = (Vector2)Display.DipsToPixels(UntransformedAbsolutePosition);

            var mtxTranslateToClientSpace = Matrix.CreateTranslation(-pixPosition.X, -pixPosition.Y, 0f);
            var mtxTransform = GetTransformMatrix(true);
            var mtxTranslateToScreenSpace = Matrix.CreateTranslation(+pixPosition.X, +pixPosition.Y, 0f);

            Matrix mtxFinal;
            Matrix.Concat(ref mtxTranslateToClientSpace, ref mtxTransform, out mtxFinal);
            Matrix.Concat(ref mtxFinal, ref mtxTranslateToScreenSpace, out mtxFinal);
            Matrix.Concat(ref mtxFinal, ref mtxParentTransform, out mtxFinal);
            
            return mtxFinal;
        }

        /// <summary>
        /// Gets the transformation matrix which is passed into the element's sprite batch prior to rendering the element.
        /// </summary>
        /// <returns>The transformation matrix which is passed into the element's sprite batch prior to rendering the element.</returns>
        internal Matrix GetVisualTransformMatrix()
        {
            var mtxParentTransform = Matrix.Identity;

            if (this is PopupRoot)
            {
                var popup = this.Parent as Popup;
                if (popup == null)
                    return Matrix.Identity;

                return popup.PopupTransformToViewInDevicePixels;
            }
            else
            {
                var parent = VisualTreeHelper.GetParent(this) as UIElement;
                if (parent != null)
                    mtxParentTransform = parent.GetVisualTransformMatrix();
            }

            return GetVisualTransformMatrix(ref mtxParentTransform);
        }
        
        /// <summary>
        /// Gets the element's list of event handlers for the specified routed event.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> that identifies the routed event for which to retrieve handlers.</param>
        /// <returns>The element's internal list of event handlers for the specified routed event.</returns>
        internal List<RoutedEventHandlerMetadata> GetHandlers(RoutedEvent evt)
        {
            return routedEventManager.GetHandlers(evt);
        }

        /// <summary>
        /// Gets the out-of-band render target which is currently assigned to this element, if any.
        /// </summary>
        internal UpfPool<OutOfBandRenderTarget>.PooledObject OutOfBandRenderTarget
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the style sheet that was most recently passed to the <see cref="Style(UvssDocument)"/> method.
        /// </summary>
        internal UvssDocument MostRecentStyleSheet
        {
            get { return mostRecentStyleSheet; }
        }

        /// <summary>
        /// Gets the arrangement options that were most recently passed to the <see cref="Arrange(RectangleD, ArrangeOptions)"/> method.
        /// </summary>
        internal ArrangeOptions MostRecentArrangeOptions
        {
            get { return mostRecentArrangeOptions; }
        }

        /// <summary>
        /// Gets the final rectangle that was most recently passed to the <see cref="Arrange(RectangleD, ArrangeOptions)"/> method.
        /// </summary>
        internal RectangleD MostRecentFinalRect
        {
            get { return mostRecentFinalRect; }
        }

        /// <summary>
        /// Gets the available size that was most recently passed to the <see cref="Measure(Size2D)"/> method.
        /// </summary>
        internal Size2D MostRecentAvailableSize
        {
            get { return mostRecentAvailableSize; }
        }

        /// <summary>
        /// Gets the position offset that was most recently passed to the <see cref="Position(Size2D)"/> method.
        /// </summary>
        internal Size2D MostRecentPositionOffset
        {
            get { return mostRecentPositionOffset; }
        }
        
        /// <summary>
        /// Gets the element's depth within the layout tree.
        /// </summary>
        internal Int32 LayoutDepth
        {
            get { return layoutDepth; }
        }

        /// <summary>
        /// Gets a value indicating whether the element is in the process of being styled.
        /// </summary>
        internal Boolean IsStyling
        {
            get { return isStyling; }
        }

        /// <summary>
        /// Gets a value indicating whether the element is in the process of being measured.
        /// </summary>
        internal Boolean IsMeasuring
        {
            get { return isMeasuring; }
        }

        /// <summary>
        /// Gets a value indicating whether the element is in the process of being arranged.
        /// </summary>
        internal Boolean IsArranging
        {
            get { return isArranging; }
        }

        /// <summary>
        /// Gets a value indicating whether the element is visually connected to the view's root element.
        /// </summary>
        internal virtual Boolean IsVisuallyConnectedToViewRoot
        {
            get { return isVisuallyConnectedToViewRoot; }
        }

        /// <summary>
        /// Gets a value indicating whether this element has a non-identity layout transform.
        /// </summary>
        internal virtual Boolean HasLayoutTransform
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether this element has a non-identity render transform
        /// </summary>
        internal virtual Boolean HasRenderTransform
        {
            get { return !Transform.IsIdentityTransform(RenderTransform); }
        }
        
        /// <summary>
        /// Gets a value indicating whether this element has a non-identity transform.
        /// </summary>
        internal Boolean HasNonIdentityTransform
        {
            get { return HasLayoutTransform || HasRenderTransform; }
        }        

        /// <inheritdoc/>
        internal override Object DependencyDataSource
        {
            get { return DeclarativeViewModelOrTemplate ?? ViewModel; }
        }

        /// <inheritdoc/>
        internal override DependencyObject DependencyContainer
        {
            get { return VisualTreeHelper.GetParent(this); }
        }

        /// <inheritdoc/>
        protected internal sealed override void OnMeasureAffectingPropertyChanged()
        {
            InvalidateMeasure();
            base.OnMeasureAffectingPropertyChanged();
        }

        /// <inheritdoc/>
        protected internal sealed override void OnArrangeAffectingPropertyChanged()
        {
            InvalidateArrange();
            base.OnMeasureAffectingPropertyChanged();
        }

        /// <inheritdoc/>
        protected internal sealed override void OnVisualBoundsAffectingPropertyChanged()
        {
            InvalidateVisualBounds();
            base.OnVisualBoundsAffectingPropertyChanged();
        }
        
        /// <inheritdoc/>
        protected internal sealed override void ApplyStyles(UvssDocument styleSheet)
        {
            styleSheet.ApplyStyles(this);
        }

        /// <inheritdoc/>
        protected internal sealed override void ApplyStyle(UvssRule style, UvssSelector selector, NavigationExpression? navigationExpression, DependencyProperty dprop)
        {
            Contract.Require(style, "style");
            Contract.Require(selector, "selector");

            var target = (DependencyObject)this;
            if (navigationExpression.HasValue)
            {
                target = navigationExpression.Value.ApplyExpression(Ultraviolet, this);
                if (target == null)
                    return;

                dprop = DependencyProperty.FindByStylingName(Ultraviolet, target, style.Owner, style.Name);
            }

            var name = style.Name;
            if (name == "transition" && !navigationExpression.HasValue)
            {
                ApplyStyledVisualStateTransition(style);
            }
            else
            {
                if (dprop != null)
                {
                    dprop.ApplyStyle(target, style, CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        /// Invalidates the element's cached visual bounds.
        /// </summary>
        /// <param name="invalidateAncestors">A value indicating whether to invalidate the visual bounds of the element's ancestors.</param>
        protected internal virtual void InvalidateVisualBounds(Boolean invalidateAncestors = true)
        {
            var invalidated = true;

            if (visualBounds.HasValue)
            {
                visualBounds = null;
                invalidated = true;
            }
            
            if (transformedVisualBounds.HasValue)
            {
                transformedVisualBounds = null;
                invalidated = true;
            }

            if (invalidated)
            {
                var parent = VisualTreeHelper.GetParent(this) as UIElement;
                if (parent != null)
                {
                    parent.InvalidateVisualBounds();
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="ClassAdded"/> event.
        /// </summary>
        /// <param name="classname">The name of the class that was added to the element.</param>
        protected internal virtual void OnClassAdded(String classname)
        {
            var temp = ClassAdded;
            if (temp != null)
            {
                temp(this, classname);
            }
        }

        /// <summary>
        /// Raises the <see cref="ClassRemoved"/> event.
        /// </summary>
        /// <param name="classname">The name of the class that was removed from the element.</param>
        protected internal virtual void OnClassRemoved(String classname)
        {
            var temp = ClassRemoved;
            if (temp != null)
            {
                temp(this, classname);
            }
        }

        /// <summary>
        /// Removes the specified child element from this element.
        /// </summary>
        /// <param name="child">The child element to remove from this element.</param>
        protected internal virtual void RemoveLogicalChild(UIElement child)
        {

        }
        
        /// <summary>
        /// Called when the desired size of one of the element's children is changed.
        /// </summary>
        /// <param name="child">The child element that was resized.</param>
        protected virtual void OnChildDesiredSizeChanged(UIElement child)
        {
            if (IsMeasureValid)
            {
                InvalidateMeasure();
            }
        }

        /// <summary>
        /// Raises the <see cref="Drawing"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected virtual void OnDrawing(UltravioletTime time, DrawingContext dc)
        {
            var temp = Drawing;
            if (temp != null)
            {
                temp(this, time, dc);
            }
        }

        /// <summary>
        /// Raises the <see cref="Updating"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected virtual void OnUpdating(UltravioletTime time)
        {
            var temp = Updating;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        /// <summary>
        /// Occurs when the view to which the element is connected changes.
        /// </summary>
        /// <param name="oldView">The view to which the element was previously connected.</param>
        /// <param name="newView">The view to which the element is now connected.</param>
        protected virtual void OnViewChanged(PresentationFoundationView oldView, PresentationFoundationView newView)
        {

        }

        /// <summary>
        /// Occurs when the element's view model has potentially changed.
        /// </summary>
        protected virtual void OnViewModelChanged()
        {

        }

        /// <summary>
        /// Occurs when the element's logical parent is changed.
        /// </summary>
        protected virtual void OnLogicalParentChanged()
        {
            CacheLayoutParameters();
            if (LogicalTreeHelper.GetParent(this) != null)
            {
                InvalidateStyle();
            }
        }

        /// <summary>
        /// Occurs when the element's transform state changes.
        /// </summary>
        protected virtual void OnTransformChanged()
        {
            var thisElementIsTransformed = HasNonIdentityTransform;

            InvalidateVisualBounds();
            
            VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(thisElementIsTransformed), (child, state) =>
            {
                child.OnAncestorTransformChanged((Boolean)state);
            });
        }

        /// <summary>
        /// Occurs when the transform of one of this element's ancestors is changed.
        /// </summary>
        /// <param name="transformed">A value indicating whether any of this element's ancestors are currently transformed.</param>
        protected virtual void OnAncestorTransformChanged(Boolean transformed)
        {
            var thisElementIsTransformed = transformed || HasNonIdentityTransform;

            InvalidateVisualBounds(false);

            VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(thisElementIsTransformed), (child, state) =>
            {
                child.OnAncestorTransformChanged((Boolean)state);
            });
        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.QueryCursorEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="cursor">The cursor to display.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnQueryCursor(MouseDevice device, ref Cursor cursor, ref RoutedEventData data)
        {

        }

        /// <inheritdoc/>
        protected override Visual HitTestCore(Point2D point)
        {
            if (!HitTestUtil.IsPotentialHit(this, point))
                return null;

            var children = VisualTreeHelper.GetChildrenCount(this);
            for (int i = children - 1; i >= 0; i--)
            {
                var child = VisualTreeHelper.GetChildByZOrder(this, i) as UIElement;
                if (child == null)
                    continue;

                var childMatch = child.HitTest(TransformToDescendant(child, point));
                if (childMatch != null)
                {
                    return childMatch;
                }
            }

            return Bounds.Contains(point) ? this : null;
        }
        
        /// <inheritdoc/>
        protected override Matrix GetTransformMatrix(Boolean inDevicePixels = false)
        {
            var scaledRenderSize = inDevicePixels ? Display.DipsToPixels(RenderSize) : RenderSize;

            var rtoInClientSpace = new Vector2(
                (Single)(scaledRenderSize.Width * RenderTransformOrigin.X),
                (Single)(scaledRenderSize.Height * RenderTransformOrigin.Y));

            var mtxTranslateToOrigin = Matrix.CreateTranslation(-rtoInClientSpace.X, -rtoInClientSpace.Y, 0);
            var mtxRenderTransform = (RenderTransform ?? Transform.Identity).Value;
            var mtxTranslateToClient = Matrix.CreateTranslation(+rtoInClientSpace.X, +rtoInClientSpace.Y, 0);

            Matrix mtxFinal;
            Matrix.Concat(ref mtxTranslateToOrigin, ref mtxRenderTransform, out mtxFinal);
            Matrix.Concat(ref mtxFinal, ref mtxTranslateToClient, out mtxFinal);

            return mtxFinal;
        }

        /// <summary>
        /// When overridden in a derived class, draws the element using the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected virtual void DrawCore(UltravioletTime time, DrawingContext dc)
        {

        }

        /// <summary>
        /// When overridden in a derived class, updates the element's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected virtual void UpdateCore(UltravioletTime time)
        {

        }

        /// <summary>
        /// When overridden in a derived class, initializes the element's dependency
        /// properties and the dependency properties of any children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the dependency
        /// properties of this element's child elements.</param>
        protected virtual void InitializeDependencyPropertiesCore(Boolean recursive)
        {
            ((DependencyObject)this).InitializeDependencyProperties();

            VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(recursive), (child, state) =>
            {
                child.InitializeDependencyProperties((Boolean)state);
            });
        }

        /// <summary>
        /// When overridden in a derived class, reloads this element's content 
        /// and, optionally, the content of any children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to reload content recursively.</param>
        protected virtual void ReloadContentCore(Boolean recursive)
        {

        }

        /// <summary>
        /// When overridden in a derived class, clears any bindings which are set on dependency properties of this element, and optionally
        /// any bindings which are set on children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the bindings
        /// of this element's child elements.</param>
        protected virtual void ClearBindingsCore(Boolean recursive)
        {
            ((DependencyObject)this).ClearBindings();

            if (recursive)
            {
                VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(recursive), (child, state) =>
                {
                    child.ClearBindings((Boolean)state);
                });
            }
        }

        /// <summary>
        /// When overridden in a derived class, clears the animations which are attached to 
        /// this element, and optionally any animations attached to children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the animations
        /// of this element's child elements.</param>
        protected virtual void ClearAnimationsCore(Boolean recursive)
        {
            ((DependencyObject)this).ClearAnimations();

            if (recursive)
            {
                VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(recursive), (child, state) =>
                {
                    child.ClearAnimations((Boolean)state);
                });
            }
        }

        /// <summary>
        /// When overridden in a derived class, clears the local values of this element's 
        /// dependency properties, and optionally the local values of any dependency properties belonging
        /// to children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the local dependency
        /// property values of this element's child elements.</param>
        protected virtual void ClearLocalValuesCore(Boolean recursive)
        {
            ((DependencyObject)this).ClearLocalValues();

            if (recursive)
            {
                VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(recursive), (child, state) =>
                {
                    child.ClearLocalValues((Boolean)state);
                });
            }
        }

        /// <summary>
        /// When overridden in a derived class, clears the styled values of this element's 
        /// dependency properties, and optionally the styled values of any dependency properties belonging
        /// to children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the styled dependency
        /// property values of this element's child elements.</param>
        protected virtual void ClearStyledValuesCore(Boolean recursive)
        {
            ((DependencyObject)this).ClearStyledValues();

            if (recursive)
            {
                VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(recursive), (child, state) =>
                {
                    child.ClearStyledValues((Boolean)state);
                });
            }
        }

        /// <summary>
        /// When overridden in a derived class, clears the triggered values of this element's 
        /// dependency properties, and optionally the triggered values of any dependency properties belonging
        /// to children of this element.
        /// </summary>
        /// <param name="recursive">A value indicating whether to clear the triggered dependency
        /// property values of this element's child elements.</param>
        protected virtual void ClearTriggeredValuesCore(Boolean recursive)
        {
            ((DependencyObject)this).ClearTriggeredValues();

            if (recursive)
            {
                VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(recursive), (child, state) =>
                {
                    child.ClearTriggeredValues((Boolean)state);
                });
            }
        }

        /// <summary>
        /// When overridden in a derived class, performs cleanup operations and releases any 
        /// internal framework resources for this element and any child elements.
        /// </summary>
        protected virtual void CleanupCore()
        {
            VisualTreeHelper.ForEachChild<UIElement>(this, null, (child, state) =>
            {
                child.Cleanup();
            });
        }

        /// <summary>
        /// When overridden in a derived class, caches layout parameters related to the
        /// element's position within the element hierarchy for this element and for
        /// any child elements.
        /// </summary>
        protected virtual void CacheLayoutParametersCore()
        {
            VisualTreeHelper.ForEachChild<UIElement>(this, null, (child, state) =>
            {
                child.CacheLayoutParameters();
            });
        }

        /// <summary>
        /// When overridden in a derived class, animates this element using the specified storyboard.
        /// </summary>
        /// <param name="storyboardInstance">The storyboard instance being applied to the element.</param>
        protected virtual void AnimateCore(StoryboardInstance storyboardInstance)
        {
            var children = VisualTreeHelper.GetChildrenCount(this);
            for (int i = 0; i < children; i++)
            {
                var child = VisualTreeHelper.GetChild(this, i) as UIElement;
                if (child != null)
                {
                    child.Animate(storyboardInstance);
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, applies the specified style sheet
        /// to this element and to any child elements.
        /// </summary>
        /// <param name="styleSheet">The style sheet to apply to this element and its children.</param>
        protected virtual void StyleCore(UvssDocument styleSheet)
        {
            VisualTreeHelper.ForEachChild<UIElement>(this, styleSheet, (child, state) =>
            {
                child.Style((UvssDocument)state);
            });
        }

        /// <summary>
        /// When overridden in a derived class, calculates the element's desired size and 
        /// the desired sizes of any child elements.
        /// </summary>
        /// <param name="availableSize">The size of the area which the element's parent has 
        /// specified is available for the element's layout.</param>
        /// <returns>The element's desired size, considering the size of any content elements.</returns>
        protected virtual Size2D MeasureCore(Size2D availableSize)
        {
            return Size2D.Zero;
        }

        /// <summary>
        /// When overridden in a derived class, sets the element's final area relative to its 
        /// parent and arranges the element's children within its layout area.
        /// </summary>
        /// <param name="finalRect">The element's final position and size relative to its parent element.</param>
        /// <param name="options">A set of <see cref="ArrangeOptions"/> values specifying the options for this arrangement.</param>
        protected virtual Size2D ArrangeCore(RectangleD finalRect, ArrangeOptions options)
        {
            return new Size2D(finalRect.Width, finalRect.Height);
        }

        /// <summary>
        /// When overridden in a derived class, updates the element's position 
        /// in absolute screen space.
        /// </summary>
        protected virtual void PositionCore()
        {

        }

        /// <summary>
        /// When overridden in a derived class, updates the positions of the element's 
        /// visual children in absolute screen space.
        /// </summary>
        protected virtual void PositionChildrenCore()
        {
            VisualTreeHelper.ForEachChild<UIElement>(this, this, (child, state) =>
            {
                child.Position(Size2D.Zero);
                child.PositionChildren();
            });
        }

        /// <summary>
        /// When overridden in a derived class, calculates the clipping rectangle for this element.
        /// </summary>
        /// <returns>The clipping rectangle for this element in absolute screen coordinates, or <c>null</c> to disable clipping.</returns>
        protected virtual RectangleD? ClipCore()
        {
            return null;
        }

        /// <summary>
        /// Calculates the element's visual bounds in client space.
        /// </summary>
        /// <returns>The element's visual bounds in client space.</returns>
        protected virtual RectangleD CalculateVisualBounds()
        {
            var elementBounds = Bounds;

            var childCount = VisualTreeHelper.GetChildrenCount(this);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(this, i) as UIElement;
                if (child != null && !(child is Popup))
                {
                    var childBounds = child.VisualBounds;
                    var childTransform = child.GetTransformMatrix();
                    RectangleD.TransformAxisAligned(ref childBounds, ref childTransform, out childBounds);

                    if (childBounds.IsEmpty)
                        continue;

                    childBounds = new RectangleD(childBounds.Location + child.UntransformedRelativePosition, childBounds.Size);
                    RectangleD.Union(ref elementBounds, ref childBounds, out elementBounds);
                }
            }

            if (ClipRectangle.HasValue)
            {
                var relativeClip = ClipRectangle.Value - UntransformedAbsolutePosition;
                RectangleD.Intersect(ref elementBounds, ref relativeClip, out elementBounds);
            }

            return elementBounds;
        }

        /// <summary>
        /// Calculates the element's transformed visual bounds in screen space.
        /// </summary>
        /// <returns>The element's transformed visual bounds in screen space.</returns>
        protected virtual RectangleD CalculateTransformedVisualBounds()
        {
            var visualBounds = Bounds;            

            var parent = VisualTreeHelper.GetParent(this) as UIElement;
            if (parent == null)
                return visualBounds;
            
            var visualTreeRoot = VisualTreeHelper.GetRoot(parent) as UIElement;
            if (visualTreeRoot == null)
                return visualBounds;

            var transform = GetTransformToViewMatrix();
            RectangleD.TransformAxisAligned(ref visualBounds, ref transform, out visualBounds);

            return UnionAbsoluteVisualBoundsWithChildrenAndApplyClipping(visualBounds, ref transform);
        }

        /// <summary>
        /// Loads the specified asset from the global content manager.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="asset">The identifier of the asset to load.</param>
        /// <returns>The asset that was loaded.</returns>
        protected TOutput LoadGlobalContent<TOutput>(AssetID asset)
        {
            if (View == null)
                return default(TOutput);

            return View.LoadLocalContent<TOutput>(asset);
        }

        /// <summary>
        /// Loads the specified asset from the local content manager.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="asset">The identifier of the asset to load.</param>
        /// <returns>The asset that was loaded.</returns>
        protected TOutput LoadLocalContent<TOutput>(AssetID asset)
        {
            if (View == null)
                return default(TOutput);

            return View.LoadLocalContent<TOutput>(asset);
        }

        /// <summary>
        /// Loads the specified image from the global content manager.
        /// </summary>
        /// <param name="image">The image to load.</param>
        protected void LoadGlobalImage<T>(T image) where T : TextureImage
        {
            if (View == null)
                return;

            View.LoadGlobalImage(image);
        }

        /// <summary>
        /// Loads the specified image from the local content manager.
        /// </summary>
        /// <param name="image">The image to load.</param>
        protected void LoadLocalImage<T>(T image) where T : TextureImage
        {
            if (View == null)
                return;

            View.LoadLocalImage(image);
        }

        /// <summary>
        /// Loads the specified resource from the global content manager.
        /// </summary>
        /// <param name="resource">The resource to load.</param>
        /// <param name="asset">The asset identifier that specifies which resource to load.</param>
        protected void LoadGlobalResource<T>(FrameworkResource<T> resource, AssetID asset) where T : class
        {
            if (View == null)
                return;

            View.LoadGlobalResource(resource, asset);
        }

        /// <summary>
        /// Loads the specified resource from the local content manager.
        /// </summary>
        /// <param name="resource">The resource to load.</param>
        /// <param name="asset">The asset identifier that specifies which resource to load.</param>
        protected void LoadLocalResource<T>(FrameworkResource<T> resource, AssetID asset) where T : class
        {
            if (View == null)
                return;

            View.LoadLocalResource(resource, asset);
        }

        /// <summary>
        /// Loads the specified sourced image.
        /// </summary>
        /// <param name="image">The sourced image to load.</param>
        protected void LoadImage(SourcedImage image)
        {
            if (View == null)
                return;

            View.LoadImage(image);
        }

        /// <summary>
        /// Loads the specified sourced cursor.
        /// </summary>
        /// <param name="cursor">The sourced cursor to load.</param>
        protected void LoadCursor(SourcedCursor cursor)
        {
            if (View == null)
                return;

            View.LoadCursor(cursor);
        }

        /// <summary>
        /// Loads the specified sourced resource.
        /// </summary>
        /// <typeparam name="T">The type of resource being loaded.</typeparam>
        /// <param name="resource">The sourced resource to load.</param>
        protected void LoadResource<T>(SourcedResource<T> resource) where T : class
        {
            if (View == null)
                return;

            View.LoadResource(resource);
        }

        /// <summary>
        /// Draws an image that fills the entire element.
        /// </summary>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        /// <param name="image">The image to draw.</param>
        /// <param name="color">The color with which to draw the image.</param>
        /// <param name="drawBlank">A value indicating whether a blank placeholder should be drawn if 
        /// the specified image does not exist or is not loaded.</param>
        protected void DrawImage(DrawingContext dc, SourcedImage image, Color color, Boolean drawBlank = false)
        {
            DrawImage(dc, image, null, Point2D.Zero, color, drawBlank);
        }

        /// <summary>
        /// Draws the specified image.
        /// </summary>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        /// <param name="image">The image to draw.</param>
        /// <param name="area">The area, relative to the element, in which to draw the image. A value of
        /// <c>null</c> specifies that the image should fill the element's entire area on the screen.</param>
        /// <param name="color">The color with which to draw the image.</param>
        /// <param name="drawBlank">A value indicating whether a blank placeholder should be drawn if 
        /// the specified image does not exist or is not loaded.</param>
        protected void DrawImage(DrawingContext dc, SourcedImage image, RectangleD? area, Color color, Boolean drawBlank = false)
        {
            DrawImage(dc, image, area, Point2D.Zero, color, drawBlank);
        }

        /// <summary>
        /// Draws the specified image.
        /// </summary>
        /// <param name="dc">The drawing context that describes the current rendering state.</param>
        /// <param name="image">The image to draw.</param>
        /// <param name="area">The area, relative to the element, in which to draw the image. A value of
        /// <c>null</c> specifies that the image should fill the element's entire area on the screen.</param>
        /// <param name="origin">The rotational origin of the image.</param>
        /// <param name="color">The color with which to draw the image.</param>
        /// <param name="drawBlank">A value indicating whether a blank placeholder should be drawn if 
        /// the specified image does not exist or is not loaded.</param>
        protected void DrawImage(DrawingContext dc, SourcedImage image, RectangleD? area, Point2D origin, Color color, Boolean drawBlank = false)
        {
            Contract.Require(dc, "dc");
            
            var imageResource = image.Resource;
            if (imageResource == null || !imageResource.IsLoaded)
            {
                if (drawBlank)
                {
                    DrawBlank(dc, area, origin, color);
                }
            }
            else
            {
                var originPix = (Vector2)Display.DipsToPixels(origin);

                var imageAreaRel = area ?? new RectangleD(0, 0, RenderSize.Width, RenderSize.Height);
                var imageAreaAbs = imageAreaRel + UntransformedAbsolutePosition;
                var imageAreaPix = (RectangleF)Display.DipsToPixels(imageAreaAbs);

                var positionIsRounded = !dc.IsTransformed;
                var position = new Vector2(
                    (positionIsRounded ? (Single)Math.Round(imageAreaPix.X, MidpointRounding.AwayFromZero) : imageAreaPix.X) + originPix.X,
                    (positionIsRounded ? (Single)Math.Round(imageAreaPix.Y, MidpointRounding.AwayFromZero) : imageAreaPix.Y) + originPix.Y);

                dc.DrawImage(imageResource, position, imageAreaPix.Width, imageAreaPix.Height,
                    color, 0f, originPix, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Draws a blank rectangle.
        /// </summary>
        /// <param name="dc">The drawing context that describes the current rendering state.</param>
        /// <param name="color">The color with which to draw the image.</param>
        protected void DrawBlank(DrawingContext dc, Color color)
        {
            DrawBlank(dc, null, Point2D.Zero, color);

        }

        /// <summary>
        /// Draws a blank rectangle.
        /// </summary>
        /// <param name="dc">The drawing context that describes the current rendering state.</param>
        /// <param name="area">The area, relative to the element, in which to draw the rectangle. A value of
        /// <c>null</c> specifies that the image should fill the element's entire area on the screen.</param>
        /// <param name="color">The color with which to draw the image.</param>
        protected void DrawBlank(DrawingContext dc, RectangleD? area, Color color)
        {
            DrawBlank(dc, area, Point2D.Zero, color);
        }

        /// <summary>
        /// Draws a blank rectangle.
        /// </summary>
        /// <param name="dc">The drawing context that describes the current rendering state.</param>
        /// <param name="area">The area, relative to the element, in which to draw the rectangle. A value of
        /// <c>null</c> specifies that the image should fill the element's entire area on the screen.</param>
        /// <param name="origin">The rotational origin of the rectangle.</param>
        /// <param name="color">The color with which to draw the image.</param>
        protected void DrawBlank(DrawingContext dc, RectangleD? area, Point2D origin, Color color)
        {
            Contract.Require(dc, "dc");

            var imageResource = View.Resources.BlankImage.Resource;
            if (imageResource == null || !imageResource.IsLoaded)
                return;

            var originPix = (Vector2)Display.DipsToPixels(origin);

            var imageAreaRel = area ?? new RectangleD(0, 0, RenderSize.Width, RenderSize.Height);
            var imageAreaAbs = imageAreaRel + UntransformedAbsolutePosition;
            var imageAreaPix = (RectangleF)Display.DipsToPixels(imageAreaAbs);

            var positionIsRounded = !dc.IsTransformed;
            var position = new Vector2(
                (positionIsRounded ? (Single)Math.Round(imageAreaPix.X, MidpointRounding.AwayFromZero) : imageAreaPix.X) + originPix.X,
                (positionIsRounded ? (Single)Math.Round(imageAreaPix.Y, MidpointRounding.AwayFromZero) : imageAreaPix.Y) + originPix.Y);

            dc.DrawImage(imageResource, position, imageAreaPix.Width, imageAreaPix.Height,
                color, 0f, originPix, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Performs layout rounding on the specified value.
        /// </summary>
        /// <param name="value">The value to round.</param>
        /// <returns>The rounded value.</returns>
        protected Double PerformLayoutRounding(Double value)
        {
            var scale = Display.DensityScale;
            if (scale == 1.0f)
                return Math.Round(value);

            var rounded = Math.Round(value * scale) / scale;
            if (Double.IsNaN(rounded) || Double.IsInfinity(rounded))
            {
                rounded = value;
            }

            return rounded;
        }

        /// <summary>
        /// Performs layout rounding on the specified point.
        /// </summary>
        /// <param name="point">The point to round.</param>
        /// <returns>The rounded point.</returns>
        protected Point2D PerformLayoutRounding(Point2D point)
        {
            var x = PerformLayoutRounding(point.X);
            var y = PerformLayoutRounding(point.Y);
            return new Point2D(x, y);
        }

        /// <summary>
        /// Performs layout rounding on the specified size.
        /// </summary>
        /// <param name="size">The size to round.</param>
        /// <returns>The rounded size.</returns>
        protected Size2D PerformLayoutRounding(Size2D size)
        {
            var width = PerformLayoutRounding(size.Width);
            var height = PerformLayoutRounding(size.Height);
            return new Size2D(width, height);
        }

        /// <summary>
        /// Performs layout rounding on the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to round.</param>
        /// <returns>The rounded rectangle.</returns>
        protected RectangleD PerformLayoutRounding(RectangleD rect)
        {
            var x = PerformLayoutRounding(rect.X);
            var y = PerformLayoutRounding(rect.Y);
            var width = PerformLayoutRounding(rect.Width);
            var height = PerformLayoutRounding(rect.Height);
            return new RectangleD(x, y, width, height);
        }

        /// <summary>
        /// Performs layout rounding on the specified margin.
        /// </summary>
        /// <param name="thickness">The margin to round.</param>
        /// <returns>The rounded margin.</returns>
        protected Thickness PerformLayoutRounding(Thickness thickness)
        {
            var left = PerformLayoutRounding(thickness.Left);
            var top = PerformLayoutRounding(thickness.Top);
            var right = PerformLayoutRounding(thickness.Right);
            var bottom = PerformLayoutRounding(thickness.Bottom);
            return new Thickness(left, top, right, bottom);
        }

        /// <summary>
        /// Checks to see whether this element, or any of its ancestors, is transformed.
        /// </summary>
        /// <returns></returns>
        protected Boolean CheckIsTransformed()
        {
            var current = (DependencyObject)this;
            while (current != null)
            {
                var uiElement = current as UIElement;
                if (uiElement != null && uiElement.HasTransform)
                {
                    return true;
                }

                current = VisualTreeHelper.GetParent(current);
            }
            return false;
        }

        /// <summary>
        /// Gets the window in which the element is being displayed.
        /// </summary>
        protected IUltravioletWindow Window
        {
            get { return (View == null) ? null : View.Window; }
        }

        /// <summary>
        /// Gets the display on which the element is being displayed.
        /// </summary>
        protected IUltravioletDisplay Display
        {
            get { return (View == null) ? null : View.Display; }
        }

        /// <summary>
        /// Gets a value that determines whether the element is enabled.
        /// </summary>
        protected virtual Boolean IsEnabledCore
        {
            get { return true; }
        }

        /// <summary>
        /// Invokes the <see cref="OnQueryCursor"/> method.
        /// </summary>
        private static void OnQueryCursorProxy(DependencyObject element, MouseDevice device, ref Cursor cursor, ref RoutedEventData data)
        {
            ((UIElement)element).OnQueryCursor(device, ref cursor, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnViewModelChanged"/> method.
        /// </summary>
        private static void OnViewModelChangedProxy(DependencyObject element, ref RoutedEventData data)
        {
            ((UIElement)element).OnViewModelChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsVisible"/> dependency property changes.
        /// </summary>
        private static void HandleIsVisibleChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var element = (UIElement)dobj;
            element.OnIsVisibleChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsEnabled"/> dependency property changes.
        /// </summary>
        private static void HandleIsEnabledChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var element = (UIElement)dobj;
            element.OnIsEnabledChanged();

            VisualTreeHelper.ForEachChild<UIElement>(dobj, null, (child, state) =>
            {
                child.CoerceValue(IsEnabledProperty);
            });
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsHitTestVisible"/> dependency property changes.
        /// </summary>
        private static void HandleIsHitTestVisibleChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var element = (UIElement)dobj;
            element.OnIsHitTestVisibleChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Focusable"/> dependency property changes.
        /// </summary>
        private static void HandleFocusableChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var element = (UIElement)dobj;
            element.OnFocusableChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Visibility"/> dependency property changes.
        /// </summary>
        private static void HandleVisibilityChanged(DependencyObject dobj, Visibility oldValue, Visibility newValue)
        {
            var element = (UIElement)dobj;

            if ((oldValue == Visibility.Hidden || oldValue == Visibility.Collapsed))
            {
                element.SetValue<Boolean>(IsVisiblePropertyKey, true);
                element.IndicateDesiredSizeChanged();
            }
            else if (oldValue == Visibility.Visible)
            {
                element.SetValue<Boolean>(IsVisiblePropertyKey, false);
                element.IndicateDesiredSizeChanged();
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RenderTransform"/> dependency property changes.
        /// </summary>
        private static void HandleRenderTransformChanged(DependencyObject dobj, Transform oldValue, Transform newValue)
        {
            var element = (UIElement)dobj;
            element.OnTransformChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Effect"/> dependency property changes.
        /// </summary>
        private static void HandleEffectChanged(DependencyObject dobj, Effect oldValue, Effect newValue)
        {
            var element = (UIElement)dobj;
            
            if (oldValue == null && newValue != null)
            {
                element.RequiredOutOfBandTargets = Math.Max(1, 1 + newValue.AdditionalRenderTargetsRequired);
            }
            else if (oldValue != null && newValue == null)
            {
                element.RequiredOutOfBandTargets = 0;
            }

            element.ReloadEffect();
        }

        /// <summary>
        /// Coerces the value of <see cref="IsEnabled"/> to ensure that elements cannot be disabled
        /// if their parents are disabled.
        /// </summary>
        private static Boolean CoerceIsEnabled(DependencyObject dobj, Boolean value)
        {
            if (value)
            {
                var parent = VisualTreeHelper.GetParent(dobj);
                if (parent == null || parent.GetValue<Boolean>(IsEnabledProperty))
                {
                    var uiElement = (UIElement)dobj;
                    return uiElement.IsEnabledCore;
                }
            }
            return false;
        }

        /// <summary>
        /// Draws the element's out-of-band texture, if it has one.
        /// </summary>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        /// <returns><c>true</c> if the element's out-of-band texture was drawn; otherwise, <c>false</c>.</returns>
        private Boolean DrawOutOfBandTexture(DrawingContext dc)
        {
            if (this is Popup || dc.IsOutOfBandRenderingSuppressed)
                return false;

            var element = this;

            if (this is PopupRoot)
                element = Parent as Popup;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            if (upf.OutOfBandRenderer.IsTextureReady(element))
            {
                var target = upf.OutOfBandRenderer.GetElementRenderTarget(element);
                if (target != null && target.IsReady)
                {
                    dc.PushOpacity(Opacity);

                    var effect = element.Effect;
                    if (effect != null)
                    {
                        effect.Draw(dc, element, target);
                    }
                    else
                    {
                        Effect.DrawRenderTargetAtVisualBounds(dc, element, target);
                    }

                    dc.PopOpacity();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the element's position and, if that position changes, the positions of the element's children.
        /// </summary>
        private void PositionElementAndPotentiallyChildren(Boolean forceInvalidatePosition)
        {
            this.forceInvalidatePosition = false;

            var oldAbsolutePosition = UntransformedAbsolutePosition;
            Position(mostRecentPositionOffset);

            if (forceInvalidatePosition || !oldAbsolutePosition.Equals(UntransformedAbsolutePosition))
            {
                PositionChildren();
            }
        }

        /// <summary>
        /// Updates the value of the <see cref="LayoutDepth"/> property.
        /// </summary>
        private void CacheLayoutDepth()
        {
            this.layoutDepth = (Parent == null) ? 0 : Parent.LayoutDepth + 1;
        }

        /// <summary>
        /// Updates the value of the <see cref="View"/> property.
        /// </summary>
        private void CacheView()
        {
            if (this is PresentationFoundationViewRoot)
            {
                isVisuallyConnectedToViewRoot = true;
                return;
            }

            var oldView = View;

            var logicalParent = LogicalTreeHelper.GetParent(this) as UIElement;
            if (logicalParent != null)
            {
                this.view = null;

                for (var current = logicalParent; current != null; current = LogicalTreeHelper.GetParent(current) as UIElement)
                {
                    if (current.View != null)
                    {
                        this.view = current.View;
                        break;
                    }
                }
            }
            else
            {
                var visualParent = VisualTreeHelper.GetParent(this) as UIElement;
                if (visualParent != null)
                {
                    this.view = null;

                    for (var current = visualParent; current != null; current = VisualTreeHelper.GetParent(current) as UIElement)
                    {
                        if (current.View != null)
                        {
                            this.view = current.View;
                            break;
                        }
                    }
                }
                else
                {
                    this.view = null;
                }
            }

            var visualParentElement = VisualParent as UIElement;
            isVisuallyConnectedToViewRoot = (visualParentElement != null && visualParentElement.IsVisuallyConnectedToViewRoot) || this is Popup;

            if (oldView != View)
                HandleViewChanged(oldView, View);
        }

        /// <summary>
        /// Cleans up the element's out-of-band render targets by returning them to the global pool.
        /// </summary>
        private void CleanupOutOfBandTargets()
        {
            if (RequiredOutOfBandTargets == 0)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.OutOfBandRenderer.Unregister(this);
        }

        /// <summary>
        /// Cleans up the element's storyboards by returning any storyboard clocks to the global pool.
        /// </summary>
        private void CleanupStoryboards()
        {
            foreach (var kvp in storyboardInstances)
                StoryboardInstancePool.Instance.Release(kvp.Value);

            storyboardInstances.Clear();
        }

        /// <summary>
        /// Informs the element's parent that its desired size has changed.
        /// </summary>
        private void IndicateDesiredSizeChanged()
        {
            var parent = VisualTreeHelper.GetParent(this) as UIElement;
            if (parent != null)
            {
                parent.OnChildDesiredSizeChanged(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="IsVisibleChanged"/> event.
        /// </summary>
        private void OnIsVisibleChanged()
        {
            var temp = IsVisibleChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="IsEnabledChanged"/> event.
        /// </summary>
        private void OnIsEnabledChanged()
        {
            var temp = IsEnabledChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="IsHitTestVisibleChanged"/> event.
        /// </summary>
        private void OnIsHitTestVisibleChanged()
        {
            var temp = IsHitTestVisibleChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="FocusableChanged"/> event.
        /// </summary>
        private void OnFocusableChanged()
        {
            var temp = FocusableChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the element's associated view changes.
        /// </summary>
        private void HandleViewChanged(PresentationFoundationView oldView, PresentationFoundationView newView)
        {
            OnViewChanged(oldView, newView);
            OnViewModelChanged();

            if (oldView == null)
            {
                if (newView.LayoutRoot.IsLoaded)
                {
                    EnsureOutOfBandRenderTargetsExist();
                }
            }
            else
            {
                Cleanup();
            }
        }

        /// <summary>
        /// Reloads the element's effect resources.
        /// </summary>
        private void ReloadEffect()
        {
            if (View == null)
                return;

            var effect = Effect;
            if (effect == null)
                return;

            var global = View.GlobalContent;
            var local = View.LocalContent;
            if (global != null || local != null)
            {
                effect.ReloadResources(global, local);
            }
        }

        /// <summary>
        /// Recurses through the element's descendants and registers any open popups for drawing.
        /// </summary>
        private void RegisterPopupsInVisualSubTree(UltravioletTime time, DrawingContext dc)
        {
            var popup = this as Popup;
            if (popup != null)
                popup.EnqueueForDrawingIfOpen(time, dc);

            var childCount = VisualTreeHelper.GetChildrenCount(this);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(this, i) as UIElement;
                if (child != null)
                {
                    child.Draw(time, dc);
                }
            }
        }

        /// <summary>
        /// Ensures that this element has been assigned its required out-of-band render targets.
        /// </summary>
        private void EnsureOutOfBandRenderTargetsExist()
        {
            if (OutOfBandRenderTarget != null || RequiredOutOfBandTargets <= 0)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            if (upf.OutOfBandRenderer.IsDrawingRenderTargets)
                return;

            upf.OutOfBandRenderer.Register(this, RequiredOutOfBandTargets - 1);
        }

        /// <summary>
        /// Gets or sets the number of out-of-band render targets required by this element.
        /// </summary>
        private Int32 RequiredOutOfBandTargets
        {
            get { return requiredOutOfBandTargets; }
            set
            {
                if (requiredOutOfBandTargets == value)
                    return;

                if (requiredOutOfBandTargets > 0)
                {
                    var upf = Ultraviolet.GetUI().GetPresentationFoundation();
                    upf.OutOfBandRenderer.Unregister(this);
                }

                requiredOutOfBandTargets = value;
                EnsureOutOfBandRenderTargetsExist();
            }
        }

        // Property values.
        private readonly UltravioletContext uv;
        private readonly UIElementClassCollection classes;
        private readonly String uvmlName;
        private PresentationFoundationView view;
        private UIElement parent;
        private Boolean isStyleValid;
        private Boolean isMeasureValid;
        private Boolean isArrangeValid;
        private Point2D renderOffset;
        private Size2D renderSize;
        private Size2D desiredSize;
        private RectangleD untransformedRelativeBounds;
        private RectangleD untransformedAbsoluteBounds;
        private RectangleD? visualBounds;
        private RectangleD? transformedVisualBounds;
        private RectangleD? clipRectangle;
        private event EventHandler layoutUpdated;

        // Layout parameters.
        private UvssDocument mostRecentStyleSheet;
        private ArrangeOptions mostRecentArrangeOptions;
        private RectangleD mostRecentFinalRect;
        private Size2D mostRecentAvailableSize;
        private Size2D mostRecentPositionOffset;
        private Int32 layoutDepth;
        private Boolean forceInvalidatePosition;
        private Boolean suppressCacheLayoutParameters;

        // State values.
        private Boolean isStyling;
        private Boolean isMeasuring;
        private Boolean isArranging;
        private Boolean isVisuallyConnectedToViewRoot;
        private Int32 requiredOutOfBandTargets;
        
        // The collection of active storyboard instances on this element.
        private readonly Dictionary<Storyboard, UpfPool<StoryboardInstance>.PooledObject> storyboardInstances = 
            new Dictionary<Storyboard, UpfPool<StoryboardInstance>.PooledObject>();

        // The element's routed event manager.
        private readonly RoutedEventManager routedEventManager = new RoutedEventManager();
    }
}
