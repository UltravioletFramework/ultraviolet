using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElement"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public UIElement(UltravioletContext uv)
        {
            Contract.Require(uv, "uv");

            this.uv      = uv;
            this.classes = new UIElementClassCollection(this);

            var attr = (UvmlKnownTypeAttribute)GetType().GetCustomAttributes(typeof(UvmlKnownTypeAttribute), false).SingleOrDefault();
            if (attr != null)
            {
                this.uvmlName = attr.Name ?? GetType().Name;
            }
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
            ReloadContentCore(recursive);
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
        }

        /// <summary>
        /// Draws the element using the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        public void Draw(UltravioletTime time, DrawingContext dc)
        {
            var clip = ClipRectangle;
            if (clip != null)
                dc.PushClipRectangle(clip.Value);

            var shouldDraw = true;

            if (clip.HasValue && clip.Value.IsEmpty)
                shouldDraw = false;

            var scissor = Ultraviolet.GetGraphics().GetScissorRectangle();
            if (scissor.HasValue)
            {
                var absBounds = Display.DipsToPixels(AbsoluteBounds);
                if (!absBounds.Intersects(scissor.Value))
                    shouldDraw = false;
            }

            if (shouldDraw)
            {
                dc.PushOpacity(Opacity);

                DrawCore(time, dc);
                OnDrawing(time, dc);

                dc.PopOpacity();
            }

            if (clip != null)
                dc.PopClipRectangle();
        }

        /// <summary>
        /// Updates the element's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            foreach (var clock in storyboardClocks)
                clock.Value.Update(time);

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
            CleanupStoryboards();
            CleanupCore();
        }

        /// <summary>
        /// Caches layout parameters related to the element's position within the element hierarchy.
        /// </summary>
        public void CacheLayoutParameters()
        {
            var view = View;

            CacheLayoutDepth();
            CacheView();
            CacheControl();

            if (View != view)
            {
                InvalidateStyle();
            }

            CacheLayoutParametersCore();
        }

        /// <summary>
        /// Animates this element using the specified storyboard.
        /// </summary>
        /// <param name="storyboard">The storyboard being applied to the element.</param>
        /// <param name="clock">The storyboard clock that tracks playback.</param>
        /// <param name="root">The root element to which the storyboard is being applied.</param>
        public void Animate(Storyboard storyboard, StoryboardClock clock, UIElement root)
        {
            Contract.Require(storyboard, "storyboard");
            Contract.Require(clock, "clock");
            Contract.Require(root, "root");

            foreach (var target in storyboard.Targets)
            {
                var targetAppliesToElement = false;
                if (target.Selector == null)
                {
                    if (this == root)
                    {
                        targetAppliesToElement = true;
                    }
                }
                else
                {
                    targetAppliesToElement = target.Selector.MatchesElement(this, root);
                }

                if (targetAppliesToElement)
                {
                    foreach (var animation in target.Animations)
                    {
                        var dp = FindDependencyPropertyByName(animation.Key);
                        if (dp != null)
                        {
                            Animate(dp, animation.Value, clock);
                        }
                    }
                }
            }

            AnimateCore(storyboard, clock, root);
        }

        /// <summary>
        /// Applies the specified stylesheet to this element.
        /// </summary>
        /// <param name="stylesheet">The stylesheet to apply to this element.</param>
        public void Style(UvssDocument stylesheet)
        {
            if (View == null)
            {
                this.isStyleValid = true;
                return;
            }

            if (isStyleValid && mostRecentStylesheet == stylesheet)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.StyleCountLastFrame++;

            this.mostRecentStylesheet = stylesheet;

            isStyling = true;
            try
            {
                ApplyStyles(stylesheet);
                StyleCore(stylesheet);
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
            Contract.EnsureRange(availableSize.Width >= 0 && availableSize.Height >= 0, "availableSize");

            if (View == null)
            {
                this.isMeasureValid = true;
                return;
            }

            if (isMeasureValid && mostRecentAvailableSize.Equals(availableSize))
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.MeasureCountLastFrame++;

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
            upf.PerformanceStats.ArrangeCountLastFrame++;

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
                    this.renderSize = ArrangeCore(finalRect, options);
                }
                finally
                {
                    isArranging = false;
                }
            }
            this.isArrangeValid = true;

            PositionElementAndPotentiallyChildren();

            upf.ArrangeQueue.Remove(this);
        }

        /// <summary>
        /// Updates the element's position in absolute screen space.
        /// </summary>
        public void Position()
        {
            if (View == null)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.PositionCountLastFrame++;

            var parent         = VisualTreeHelper.GetParent(this) as UIElement;
            var parentPosition = (parent == null) ? Point2D.Zero : parent.AbsolutePosition;

            var contentRegionOffset = (parent == null || parent == Control) ? Point2D.Zero : parent.ContentOffset;

            var offsetX = mostRecentFinalRect.X + RenderOffset.X + contentRegionOffset.X;
            var offsetY = mostRecentFinalRect.Y + RenderOffset.Y + contentRegionOffset.Y;
            var offset  = new Point2D(offsetX, offsetY);

            this.relativeBounds = new RectangleD(offset.X, offset.Y, RenderSize.Width, RenderSize.Height);
            this.absoluteBounds = new RectangleD(parentPosition.X + offset.X, parentPosition.Y + offset.Y, RenderSize.Width, RenderSize.Height);

            PositionCore();

            Clip();
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
        public void InvalidateStyle()
        {
            if (View == null || !IsStyleValid || IsStyling)
                return;

            this.isStyleValid = false;

            var upf = uv.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.InvalidateStyleCountLastFrame++;
            upf.StyleQueue.Enqueue(this);
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
            upf.PerformanceStats.InvalidateMeasureCountLastFrame++;
            upf.MeasureQueue.Enqueue(this);
        }

        /// <summary>
        /// Invalidates the element's arrangement state.
        /// </summary>
        public void InvalidateArrange()
        {
            if (View == null || !IsArrangeValid || IsArranging)
                return;

            this.isArrangeValid = false;

            var upf = uv.GetUI().GetPresentationFoundation();
            upf.PerformanceStats.InvalidateArrangeCountLastFrame++;
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
                    OnLogicalParentChangedInternal();
                }
            }
        }

        /// <summary>
        /// Gets the control that owns this element, if this element is a control component.
        /// </summary>
        public Control Control
        {
            get { return control; }
        }

        /// <summary>
        /// Gets a value indicating whether this element is a control component.
        /// </summary>
        public Boolean IsComponent
        {
            get { return control != null; }
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
        /// Gets or sets a value indicating whether the element is enabled.
        /// </summary>
        public Boolean IsEnabled
        {
            get { return GetValue<Boolean>(IsEnabledProperty); }
            set { SetValue<Boolean>(IsEnabledProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether this element has input focus.
        /// </summary>
        public Boolean IsFocused
        {
            get { return (View == null) ? false : View.ElementWithFocus == this; }
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
        /// Gets the position of the element in absolute screen coordinates as of the
        /// last call to the <see cref="Position(Point2D)"/> method.
        /// </summary>
        public Point2D AbsolutePosition
        {
            get { return absoluteBounds.Location; }
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
        /// Gets the absolute bounding box of the layout area in which the element was most recently arranged.
        /// </summary>
        public RectangleD AbsoluteLayoutBounds
        {
            get { return mostRecentFinalRect; }
        }

        /// <summary>
        /// Gets the element's bounds in element-local space.
        /// </summary>
        public RectangleD Bounds
        {
            get { return new RectangleD(0, 0, RenderSize.Width, RenderSize.Height); }
        }

        /// <summary>
        /// Gets the element's bounds relative to its parent element.
        /// </summary>
        public RectangleD RelativeBounds
        {
            get { return relativeBounds; }
        }

        /// <summary>
        /// Gets the element's bounds in absolute screen space.
        /// </summary>
        public RectangleD AbsoluteBounds
        {
            get { return absoluteBounds; }
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
        /// Gets the offset applied to the region's content. This is usually used to scroll the 
        /// element's content within its content region.
        /// </summary>
        public virtual Point2D ContentOffset
        {
            get { return Point2D.Zero; }
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
        /// Occurs when the value of the <see cref="Visibility"/> property changes.
        /// </summary>
        public event UpfEventHandler VisibilityChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Opacity"/> property changes.
        /// </summary>
        public event UpfEventHandler OpacityChanged;

        /// <summary>
        /// Identifies the <see cref="IsEnabled"/> dependency property.
        /// </summary>
        [Styled("enabled")]
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata(CommonBoxedValues.Boolean.True, HandleIsEnabledChanged, new CoerceValueCallback<Boolean>(CoerceIsEnabled)));

        /// <summary>
        /// Identifies the <see cref="IsHitTestVisible"/> dependency property.
        /// </summary>
        [Styled("hit-test-visible")]
        public static readonly DependencyProperty IsHitTestVisibleProperty = DependencyProperty.Register("IsHitTestVisible", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata(CommonBoxedValues.Boolean.True, HandleIsHitTestVisibleChanged));
        
        /// <summary>
        /// Identifies the <see cref="Focusable"/> dependency property.
        /// </summary>
        [Styled("focusable")]
        public static readonly DependencyProperty FocusableProperty = DependencyProperty.Register("Focusable", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata(CommonBoxedValues.Boolean.False, HandleFocusableChanged));

        /// <summary>
        /// Identifies the <see cref="Visibility"/> dependency property.
        /// </summary>
        [Styled("visibility")]
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(UIElement),
            new PropertyMetadata(PresentationBoxedValues.Visibility.Visible, PropertyMetadataOptions.AffectsArrange, HandleVisibilityChanged));

        /// <summary>
        /// Identifies the <see cref="Opacity"/> dependency property.
        /// </summary>
        [Styled("opacity")]
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register("Opacity", typeof(Single), typeof(UIElement),
            new PropertyMetadata(CommonBoxedValues.Single.One, HandleOpacityChanged));
        
        /// <summary>
        /// Applies a visual state transition to the element.
        /// </summary>
        /// <param name="style">The style which defines the state transition.</param>
        internal virtual void ApplyStyledVisualStateTransition(UvssStyle style)
        {

        }

        /// <summary>
        /// Called when something occurs which requires the element to invalidate its cached
        /// layout information, such as the visual or logical parent changing.
        /// </summary>
        internal virtual void OnLayoutCacheInvalidatedInternal()
        {
            CacheLayoutParameters();
            InvalidateStyle();

            OnLayoutCacheInvalidated();
        }

        /// <summary>
        /// Invokes the <see cref="OnLogicalParentChanged()"/> method.
        /// </summary>
        internal virtual void OnLogicalParentChangedInternal()
        {
            OnLayoutCacheInvalidatedInternal();
            OnLogicalParentChanged();
        }

        /// <inheritdoc/>
        internal override void OnVisualParentChangedInternal()
        {
            OnLayoutCacheInvalidatedInternal();
            base.OnVisualParentChangedInternal();
        }

        /// <summary>
        /// Registers the element with the specified namescope.
        /// </summary>
        /// <param name="namescope">The namescope with which to register the element.</param>
        internal virtual void RegisterElementWithNamescope(Namescope namescope)
        {

        }

        /// <summary>
        /// Unregisters the element from the specified namescope.
        /// </summary>
        /// <param name="namescope">The namescope from which to unregister the element.</param>
        internal virtual void UnregisterElementFromNamescope(Namescope namescope)
        {

        }

        /// <summary>
        /// Changes the element's logical and visual parents.
        /// </summary>
        /// <param name="logicalParent">The element's new logical parent.</param>
        /// <param name="visualParent">The element's new visual parent.</param>
        internal void ChangeLogicalAndVisualParents(UIElement logicalParent, Visual visualParent)
        {
            if (this.Parent != null)
                this.Parent = null;

            this.Parent = logicalParent;

            if (this.VisualParent != null)
                this.VisualParent.RemoveVisualChild(this);

            if (visualParent != null)
                visualParent.AddVisualChild(this);
        }

        /// <summary>
        /// Changes the element's logical parent.
        /// </summary>
        /// <param name="logicalParent">The element's new logical parent.</param>
        internal void ChangeLogicalParent(UIElement logicalParent)
        {
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
        /// Searches the object for a dependency property which matches the specified name.
        /// </summary>
        /// <param name="name">The name of the dependency property for which to search.</param>
        /// <returns>The <see cref="DependencyProperty"/> instance which matches the specified name, or <c>null</c> if no
        /// such property exists on this object.</returns>
        internal DependencyProperty FindDependencyPropertyByName(DependencyPropertyName name)
        {
            if (name.IsAttachedProperty)
            {
                if (Parent != null && String.Equals(Parent.UvmlName, name.Container, StringComparison.OrdinalIgnoreCase))
                {
                    return DependencyProperty.FindByName(name.Name, Parent.GetType());
                }
                return null;
            }
            return DependencyProperty.FindByName(name.Name, GetType());
        }

        /// <summary>
        /// Finds a styled dependency property according to its styling name.
        /// </summary>
        /// <param name="name">The styling name of the dependency property to retrieve.</param>
        /// <returns>The <see cref="DependencyProperty"/> instance which matches the specified styling name, or <c>null</c> if no
        /// such dependency property exists on this object.</returns>
        internal DependencyProperty FindStyledDependencyProperty(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            return FindStyledDependencyProperty(name, GetType());
        }

        /// <summary>
        /// Begins the specified storyboard on this element.
        /// </summary>
        /// <param name="storyboard">The storyboard to begin on this element.</param>
        internal void BeginStoryboard(Storyboard storyboard)
        {
            StoryboardClock existingClock;
            storyboardClocks.TryGetValue(storyboard, out existingClock);

            var clock = StoryboardClockPool.Instance.Retrieve(storyboard);
            storyboardClocks[storyboard] = clock;

            Animate(storyboard, clock, this);

            clock.Start();

            if (existingClock != null)
            {
                existingClock.Stop();
                StoryboardClockPool.Instance.Release(existingClock);
            }
        }

        /// <summary>
        /// Stops the specified storyboard on this element.
        /// </summary>
        /// <param name="storyboard">The storyboard to stop on this element.</param>
        internal void StopStoryboard(Storyboard storyboard)
        {
            StoryboardClock clock;
            if (storyboardClocks.TryGetValue(storyboard, out clock))
            {
                clock.Stop();
                storyboardClocks.Remove(storyboard);
                StoryboardClockPool.Instance.Release(clock);
            }
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
        /// Gets the stylesheet that was most recently passed to the <see cref="Style(UvssDocument)"/> method.
        /// </summary>
        internal UvssDocument MostRecentStylesheet
        {
            get { return mostRecentStylesheet; }
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

        /// <summary>
        /// Applies the specified stylesheet's styles to this element and its children.
        /// </summary>
        /// <param name="stylesheet">The stylesheet to apply to the element.</param>
        protected internal sealed override void ApplyStyles(UvssDocument stylesheet)
        {
            stylesheet.ApplyStyles(this);
        }

        /// <summary>
        /// Applies a style to the element.
        /// </summary>
        /// <param name="style">The style which is being applied.</param>
        /// <param name="selector">The selector which caused the style to be applied.</param>
        /// <param name="attached">A value indicating whether thie style represents an attached property.</param>
        protected internal sealed override void ApplyStyle(UvssStyle style, UvssSelector selector, Boolean attached)
        {
            Contract.Require(style, "style");
            Contract.Require(selector, "selector");

            var name = style.Name;
            if (name == "transition")
            {
                ApplyStyledVisualStateTransition(style);
            }
            else
            {
                var setter = attached ? Parent.GetStyleSetter(name, selector.PseudoClass) : GetStyleSetter(name, selector.PseudoClass);
                if (setter == null)
                    return;

                setter(this, style, CultureInfo.InvariantCulture);
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
        /// Removes the specified child element from this element.
        /// </summary>
        /// <param name="child">The child element to remove from this element.</param>
        protected internal virtual void RemoveLogicalChild(UIElement child)
        {

        }

        /// <inheritdoc/>
        protected internal sealed override Object DependencyDataSource
        {
            get { return IsComponent ? Control : ViewModel; }
        }

        /// <inheritdoc/>
        protected internal sealed override DependencyObject DependencyContainer
        {
            get { return VisualTreeHelper.GetParent(this); }
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
        /// Occurs when something happens which requires the element to invalidate any
        /// cached layout information, such as changing the element's visual or logical parent.
        /// </summary>
        protected virtual void OnLayoutCacheInvalidated()
        {

        }

        /// <summary>
        /// Occurs when the element's logical parent is changed.
        /// </summary>
        protected virtual void OnLogicalParentChanged()
        {
            CacheLayoutParameters();
            InvalidateStyle();
        }

        /// <summary>
        /// Raises the <see cref="IsEnabledChanged"/> event.
        /// </summary>
        protected virtual void OnIsEnabledChanged()
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
        protected virtual void OnIsHitTestVisibleChanged()
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
        protected virtual void OnFocusableChanged()
        {
            var temp = FocusableChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="VisibilityChanged"/> event.
        /// </summary>
        protected virtual void OnVisibilityChanged()
        {
            var temp = VisibilityChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="OpacityChanged"/> event.
        /// </summary>
        protected virtual void OnOpacityChanged()
        {
            var temp = OpacityChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <inheritdoc/>
        protected override Visual HitTestCore(Point2D point)
        {
            if (!IsHitTestVisible || !Bounds.Contains(point))
                return null;

            var children = VisualTreeHelper.GetChildrenCount(this);
            for (int i = children - 1; i >= 0; i--)
            {
                var child = VisualTreeHelper.GetChild(this, i) as UIElement;
                if (child == null)
                    continue;

                var childMatch = child.HitTest(point - child.RelativeBounds.Location);
                if (childMatch != null)
                {
                    return childMatch;
                }
            }

            return this;
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
        /// and the content of any children of this element.
        /// </summary>
        protected virtual void ReloadContentCore(Boolean recursive)
        {
            VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(recursive), (child, state) =>
            {
                child.ReloadContent((Boolean)state);
            });
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

            VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(recursive), (child, state) =>
            {
                child.ClearAnimations((Boolean)state);
            });
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

            VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(recursive), (child, state) =>
            {
                child.ClearLocalValues((Boolean)state);
            });
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

            VisualTreeHelper.ForEachChild<UIElement>(this, CommonBoxedValues.Boolean.FromValue(recursive), (child, state) =>
            {
                child.ClearStyledValues((Boolean)state);
            });
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
        /// <param name="storyboard">The storyboard being applied to the element.</param>
        /// <param name="clock">The storyboard clock that tracks playback.</param>
        /// <param name="root">The root element to which the storyboard is being applied.</param>
        protected virtual void AnimateCore(Storyboard storyboard, StoryboardClock clock, UIElement root)
        {
            var children = VisualTreeHelper.GetChildrenCount(this);
            for (int i = 0; i < children; i++)
            {
                var child = VisualTreeHelper.GetChild(this, i) as UIElement;
                if (child != null)
                {
                    child.Animate(storyboard, clock, root);
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, applies the specified stylesheet
        /// to this element and to any child elements.
        /// </summary>
        /// <param name="stylesheet">The stylesheet to apply to this element and its children.</param>
        protected virtual void StyleCore(UvssDocument stylesheet)
        {
            VisualTreeHelper.ForEachChild<UIElement>(this, stylesheet, (child, state) =>
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
                child.Position();
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
        /// <param name="drawBlankImage">A value indicating whether a blank placeholder should be drawn if 
        /// the specified image does not exist or is not loaded.</param>
        protected void DrawImage(DrawingContext dc, SourcedImage image, Color color, Boolean drawBlankImage = false)
        {
            DrawImage(dc, image, null, color, drawBlankImage);
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
            Contract.Require(dc, "dc");

            var colorPlusOpacity = color * dc.Opacity;
            if (colorPlusOpacity.Equals(Color.Transparent))
                return;

            var imageResource = image.Resource;
            if (imageResource == null || !imageResource.IsLoaded)
            {
                if (drawBlank)
                {
                    DrawBlank(dc, area, colorPlusOpacity);
                }
            }
            else
            {
                var imageAreaRel = area ?? new RectangleD(0, 0, RenderSize.Width, RenderSize.Height);
                var imageAreaAbs = imageAreaRel + AbsolutePosition;
                var imageAreaPix = (Rectangle)Display.DipsToPixels(imageAreaAbs);

                var origin = new Vector2(
                    (Int32)(imageAreaPix.Width / 2f),
                    (Int32)(imageAreaPix.Height / 2f));

                var position = new Vector2(
                    (Int32)(imageAreaPix.X + (imageAreaPix.Width / 2f)),
                    (Int32)(imageAreaPix.Y + (imageAreaPix.Height / 2f)));

                dc.SpriteBatch.DrawImage(imageResource, position, (Int32)imageAreaPix.Width, (Int32)imageAreaPix.Height,
                    colorPlusOpacity, 0f, origin, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Draws a blank rectangle.
        /// </summary>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        /// <param name="area">The area, relative to the element, in which to draw the image. A value of
        /// <c>null</c> specifies that the image should fill the element's entire area on the screen.</param>
        /// <param name="color">The color with which to draw the image.</param>
        protected void DrawBlank(DrawingContext dc, RectangleD? area, Color color)
        {
            Contract.Require(dc, "dc");

            var colorPlusOpacity = color * dc.Opacity;
            if (colorPlusOpacity.Equals(Color.Transparent))
                return;

            var imageResource = View.Resources.BlankImage.Resource;
            if (imageResource == null || !imageResource.IsLoaded)
                return;

            var imageAreaRel = area ?? new RectangleD(0, 0, RenderSize.Width, RenderSize.Height);
            var imageAreaAbs = imageAreaRel + AbsolutePosition;
            var imageAreaPix = (RectangleF)Display.DipsToPixels(imageAreaAbs);

            var origin = new Vector2(
                (Int32)(imageAreaPix.Width / 2f),
                (Int32)(imageAreaPix.Height / 2f));

            var position = new Vector2(
                (Int32)(imageAreaPix.X + (imageAreaPix.Width / 2f)),
                (Int32)(imageAreaPix.Y + (imageAreaPix.Height / 2f)));

            dc.SpriteBatch.DrawImage(imageResource, position, (Int32)imageAreaPix.Width, (Int32)imageAreaPix.Height,
                colorPlusOpacity, 0f, origin, SpriteEffects.None, 0f);
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
        /// Occurs when the value of the <see cref="IsEnabled"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleIsEnabledChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnIsEnabledChanged();

            VisualTreeHelper.ForEachChild<UIElement>(element, null, (child, state) =>
            {
                child.CoerceValue(IsEnabledProperty);
            });
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsHitTestVisible"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleIsHitTestVisibleChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnIsHitTestVisibleChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Focusable"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleFocusableChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnFocusableChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Visibility"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleVisibilityChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnVisibilityChanged();
            element.IndicateDesiredSizeChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Opacity"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleOpacityChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnOpacityChanged();
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
        /// Updates the element's position and, if that position changes, the positions of the element's children.
        /// </summary>
        private void PositionElementAndPotentiallyChildren()
        {
            var oldAbsolutePosition = AbsolutePosition;

            Position();

            if (!oldAbsolutePosition.Equals(AbsolutePosition))
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
            this.logicalOrder = 0;

            var parent = LogicalTreeHelper.GetParent(this);
            if (parent != null)
            {
                var children = LogicalTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < children; i++)
                {
                    if (LogicalTreeHelper.GetChild(parent, i) == this)
                    {
                        this.logicalOrder = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the value of the <see cref="View"/> property.
        /// </summary>
        private void CacheView()
        {
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
                if (visualParent == null)
                    return;

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
        }

        /// <summary>
        /// Updates the value of the <see cref="Control"/> property.
        /// </summary>
        private void CacheControl()
        {
            if (namescope != null)
            {
                UnregisterElementFromNamescope(namescope);
                namescope = null;
            }

            this.control = FindControl();

            if (namescope == null)
                namescope = FindNamescope();

            if (namescope != null)
                RegisterElementWithNamescope(namescope);
        }

        /// <summary>
        /// Finds the element registration context for this element.
        /// </summary>
        /// <returns>The element registration context for this element.</returns>
        private Namescope FindNamescope()
        {
            if (Control != null)
            {
                return Control.ComponentNamescope;
            }
            return (view == null) ? null : view.Namescope;
        }

        /// <summary>
        /// Searches the element hierarchy for the control that owns
        /// this element, if this element is a component.
        /// </summary>
        /// <returns>The control that owns this element, or <c>null</c> if this element is not a component.</returns>
        private Control FindControl()
        {
            if (Parent is Control && ((Control)Parent).ComponentRoot == this)
                return (Control)Parent;

            var current = Parent;
            while (current != null)
            {
                if (current is ContentControl)
                    return null;

                if (current.Control != null)
                    return current.Control;

                current = current.Parent;
            }
            return null;
        }

        /// <summary>
        /// Cleans up the element's storyboards by returning any storyboard clocks to the global pool.
        /// </summary>
        private void CleanupStoryboards()
        {
            var pool = StoryboardClockPool.Instance;
            foreach (var kvp in storyboardClocks)
            {
                kvp.Value.Stop();
                pool.Release(kvp.Value);
            }
            storyboardClocks.Clear();
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

        // Property values.
        private readonly UltravioletContext uv;
        private readonly UIElementClassCollection classes;
        private readonly String uvmlName;
        private PresentationFoundationView view;
        private UIElement parent;
        private Control control = null;
        private Boolean isStyleValid;
        private Boolean isMeasureValid;
        private Boolean isArrangeValid;
        private Point2D renderOffset;
        private Size2D renderSize;
        private Size2D desiredSize;
        private RectangleD relativeBounds;
        private RectangleD absoluteBounds;
        private RectangleD? clipRectangle;

        // Layout parameters.
        private UvssDocument mostRecentStylesheet;
        private ArrangeOptions mostRecentArrangeOptions;
        private RectangleD mostRecentFinalRect;
        private Size2D mostRecentAvailableSize;
        private Int32 layoutDepth;
        private Int32 logicalOrder;

        // State values.
        private Namescope namescope;
        private Boolean isStyling;
        private Boolean isMeasuring;
        private Boolean isArranging;

        // The collection of active storyboard clocks on this element.
        private readonly Dictionary<Storyboard, StoryboardClock> storyboardClocks = 
            new Dictionary<Storyboard, StoryboardClock>();

        // The element's routed event manager.
        private readonly RoutedEventManager routedEventManager = new RoutedEventManager();
    }
}
