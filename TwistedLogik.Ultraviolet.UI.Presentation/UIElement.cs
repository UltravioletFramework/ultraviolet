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
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;
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
    /// Represents the method that is called when a UI element receives an event from a keyboard device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The keyboard device.</param>
    public delegate void UIElementKeyboardEventHandler(UIElement element, KeyboardDevice device);

    /// <summary>
    /// Represents the method that is called when a keyboard key is pressed while an element has focus.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
    /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
    /// <param name="ctrl">A value indicating whether the Control modifier is active.</param>
    /// <param name="alt">A value indicating whether the Alt modifier is active.</param>
    /// <param name="shift">A value indicating whether the Shift modifier is active.</param>
    /// <param name="repeat">A value indicating whether this is a repeated key press.</param>
    public delegate void UIElementKeyPressedEventHandler(UIElement element, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat);

    /// <summary>
    /// Represents the method that is called when a keyboard key is released while an element has focus.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
    /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
    public delegate void UIElementKeyReleasedEventHandler(UIElement element, KeyboardDevice device, Key key);

    /// <summary>
    /// Represents the method that is called when the mouse cursor enters or leaves a UI element.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    public delegate void UIElementMouseEventHandler(UIElement element, MouseDevice device);

    /// <summary>
    /// Represents the method that is called when a button is pressed or released while an element is under the mouse.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="button">The mouse button that was pressed or released.</param>
    public delegate void UIElementMouseButtonEventHandler(UIElement element, MouseDevice device, MouseButton button);

    /// <summary>
    /// Represents the method that is called when the mouse moves over a control.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The mouse device.</param>
    /// <param name="x">The x-coordinate of the cursor in device-independent screen coordinates.</param>
    /// <param name="y">The y-coordinate of the cursor in device-independent screen coordinates.</param>
    /// <param name="dx">The difference between the x-coordinate of the mouse's 
    /// current position and the x-coordinate of the mouse's previous position.</param>
    /// <param name="dy">The difference between the y-coordinate of the mouse's 
    /// current position and the y-coordinate of the mouse's previous position.</param>
    public delegate void UIElementMouseMotionEventHandler(UIElement element, MouseDevice device, Double x, Double y, Double dx, Double dy);

    /// <summary>
    /// Represents the method that is called when a UI element raises an event.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    public delegate void UIElementEventHandler(UIElement element);

    /// <summary>
    /// Represents the base class for all elements within the Ultraviolet Presentation Foundation.
    /// </summary>
    public abstract class UIElement : StyledDependencyObject
    {
        /// <summary>
        /// Initialies the <see cref="UIElement"/> type.
        /// </summary>
        static UIElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElement"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public UIElement(UltravioletContext uv, String id)
        {
            Contract.Require(uv, "uv");

            this.uv      = uv;
            this.id      = id;
            this.classes = new UIElementClassCollection(this);

            var attr = (UIElementAttribute)GetType().GetCustomAttributes(typeof(UIElementAttribute), false).SingleOrDefault();
            if (attr != null)
            {
                this.name = attr.Name;
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

            dc.PushOpacity(Opacity);

            DrawCore(time, dc);
            OnDrawing(time, dc);

            dc.PopOpacity();

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
                ReloadContent(false);
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
            this.mostRecentStylesheet = stylesheet; 
            
            ApplyStyles(stylesheet);
            StyleCore(stylesheet);

            this.isStyleValid = true;

            InvalidateMeasure();

            Ultraviolet.GetUI().GetPresentationFoundation().StyleQueue.Remove(this);
        }

        /// <summary>
        /// Calculates the element's desired size.
        /// </summary>
        /// <param name="availableSize">The size of the area which the element's parent has 
        /// specified is available for the element's layout.</param>
        public void Measure(Size2D availableSize)
        {
            if (isMeasureValid && mostRecentAvailableSize.Equals(availableSize))
                return;

            this.mostRecentAvailableSize = availableSize;

            this.desiredSize = MeasureCore(availableSize);
            this.isMeasureValid = true;

            InvalidateArrange();

            Ultraviolet.GetUI().GetPresentationFoundation().MeasureQueue.Remove(this);
        }

        /// <summary>
        /// Sets the element's final area relative to its parent and arranges
        /// the element's children within its layout area.
        /// </summary>
        /// <param name="finalRect">The element's final position and size relative to its parent element.</param>
        /// <param name="options">A set of <see cref="ArrangeOptions"/> values specifying the options for this arrangement.</param>
        public void Arrange(RectangleD finalRect, ArrangeOptions options = ArrangeOptions.None)
        {
            if (isArrangeValid && mostRecentFinalRect.Equals(finalRect) && ((Int32)mostRecentArrangeOptions).Equals((Int32)options))
                return;

            this.mostRecentArrangeOptions = options;
            this.mostRecentFinalRect = finalRect;

            if (Visibility == Visibility.Collapsed)
            {
                this.renderSize = Size2.Zero;
            }
            else
            {
                this.renderSize = ArrangeCore(finalRect, options);
            }
            this.isArrangeValid = true;

            InvalidatePosition();

            Ultraviolet.GetUI().GetPresentationFoundation().ArrangeQueue.Remove(this);
        }

        /// <summary>
        /// Positions the element in absolute screen space.
        /// </summary>
        /// <param name="position">The position of the element's parent element in absolute screen space.</param>
        public void Position(Point2D position)
        {
            this.mostRecentPosition = position;

            var contentRegionOffset = (Parent == null || IsComponent) ? Point2D.Zero : Parent.RelativeContentRegion.Location;

            var offsetX = mostRecentFinalRect.X + RenderOffset.X + contentRegionOffset.X;
            var offsetY = mostRecentFinalRect.Y + RenderOffset.Y + contentRegionOffset.Y;

            this.relativeBounds = new RectangleD(offsetX, offsetY, RenderSize.Width, RenderSize.Height);
            this.absoluteBounds = new RectangleD(position.X + offsetX, position.Y + offsetY, RenderSize.Width, RenderSize.Height);

            PositionCore(position);
            this.isPositionValid = true;

            Clip();

            Ultraviolet.GetUI().GetPresentationFoundation().PositionQueue.Remove(this);
        }

        /// <summary>
        /// Calculates the clipping rectangle for the element.
        /// </summary>
        public void Clip()
        {
            this.clipRectangle = ClipCore();
            ClipContent();
        }

        /// <summary>
        /// Calculates the clipping rectangle for the element's content.
        /// </summary>
        public void ClipContent()
        {
            this.clipContentRectangle = ClipContentCore();
        }
        
        /// <summary>
        /// Invalidates the element's styling state.
        /// </summary>
        public void InvalidateStyle()
        {
            this.isStyleValid = false;
            uv.GetUI().GetPresentationFoundation().StyleQueue.Enqueue(this);
        }

        /// <summary>
        /// Invalidates the element's measurement state.
        /// </summary>
        public void InvalidateMeasure()
        {
            this.isMeasureValid = false;
            uv.GetUI().GetPresentationFoundation().MeasureQueue.Enqueue(this);
        }

        /// <summary>
        /// Invalidates the element's arrangement state.
        /// </summary>
        public void InvalidateArrange()
        {
            this.isArrangeValid = false;
            uv.GetUI().GetPresentationFoundation().ArrangeQueue.Enqueue(this);
        }

        /// <summary>
        /// Invalidates the element's position state.
        /// </summary>
        public void InvalidatePosition()
        {
            this.isPositionValid = false;
            uv.GetUI().GetPresentationFoundation().PositionQueue.Enqueue(this);
        }

        /// <summary>
        /// Gets the specified logical child of this element.
        /// </summary>
        /// <param name="ix">The index of the logical child to retrieve.</param>
        /// <returns>The logical child of this element with the specified index.</returns>
        public virtual UIElement GetLogicalChild(Int32 ix)
        {
            throw new ArgumentOutOfRangeException("ix");
        }

        /// <summary>
        /// Gets the element at the specified pixel coordinates relative to this element's bounds.
        /// </summary>
        /// <param name="x">The element-relative x-coordinate of the pixel to evaluate.</param>
        /// <param name="y">The element-relative y-coordinate of the pixel to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified pixel coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtPixel(Int32 x, Int32 y, Boolean isHitTest)
        {
            var dipsX = Display.PixelsToDips(x);
            var dipsY = Display.PixelsToDips(y);

            return GetElementAtPoint(dipsX, dipsY, isHitTest);
        }

        /// <summary>
        /// Gets the element at the specified pixel coordinates relative to this element's bounds.
        /// </summary>
        /// <param name="pt">The relative coordinates of the pixel to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified pixel coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtPixel(Point2 pt, Boolean isHitTest)
        {
            return GetElementAtPixel(pt.X, pt.Y, isHitTest);
        }

        /// <summary>
        /// Gets the element at the specified device-independent coordinates relative to this element's bounds.
        /// </summary>
        /// <param name="x">The element-relative x-coordinate of the point to evaluate.</param>
        /// <param name="y">The element-relative y-coordinate of the point to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtPoint(Double x, Double y, Boolean isHitTest)
        {
            if (!Bounds.Contains(x, y))
                return null;

            if (!IsEnabled)
                return null;

            if (isHitTest && !IsHitTestVisible)
                return null;

            if (Visibility != Visibility.Visible)
                return null;

            return GetElementAtPointCore(x, y, isHitTest);
        }

        /// <summary>
        /// Gets the element at the specified device-independent coordinates relative to this element's bounds.
        /// </summary>
        /// <param name="pt">The relative coordinates of the point to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the
        /// value of the <see cref="IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified coordinates, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementAtPoint(Point2D pt, Boolean isHitTest)
        {
            return GetElementAtPoint(pt.X, pt.Y, isHitTest);
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
        /// Gets the element's unique identifier within its layout.
        /// </summary>
        public String ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the name that identifies this element type within the Presentation Foundation.
        /// </summary>
        public String Name
        {
            get { return name; }
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
                    CacheLayoutParameters();
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
        /// Gets a value indicating whether the element's position state is valid.
        /// </summary>
        public Boolean IsPositionValid
        {
            get { return isPositionValid; }
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
        /// Gets a value indicating whether the mouse cursor is currently hovering over this element.
        /// </summary>
        public Boolean IsHovering
        {
            get { return isHovering; }
            private set
            {
                if (isHovering != value)
                {
                    isHovering = value;
                    OnIsHoveringChanged();
                }
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
        /// Gets the element's content clipping rectangle. A value of <c>null</c> indicates that
        /// content clipping is disabled for this element.
        /// </summary>
        public RectangleD? ClipContentRectangle
        {
            get { return clipContentRectangle; }
        }

        /// <summary>
        /// Gets the element's desired content region as of the last call to <see cref="Measure(Size2D)"/>.
        /// </summary>
        public virtual RectangleD DesiredContentRegion
        {
            get { return new RectangleD(Point2D.Zero, DesiredSize); }
        }

        /// <summary>
        /// Gets the element's final rendered content region as of the last call to <see cref="Arrange(RectangleD, ArrangeOptions)"/>.
        /// </summary>
        public virtual RectangleD RenderContentRegion
        {
            get { return new RectangleD(Point2D.Zero, RenderSize); }
        }

        /// <summary>
        /// Gets the element's final rendered content region in element-relative space as of the last call to <see cref="Position(Point2D)"/>.
        /// </summary>
        public virtual RectangleD RelativeContentRegion
        {
            get { return RelativeBounds; }
        }

        /// <summary>
        /// Gets the element's final rendered content region in absolute screen space as of the last call to <see cref="Position(Point2D)"/>.
        /// </summary>
        public virtual RectangleD AbsoluteContentRegion
        {
            get { return AbsoluteBounds; }
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
        /// Gets the number of logical children which belong to this element.
        /// </summary>
        public virtual Int32 LogicalChildren
        {
            get { return 0; }
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
        /// Occurs when the element gains input focus.
        /// </summary>
        public event UIElementEventHandler Focused;

        /// <summary>
        /// Occurs when the element loses input focus.
        /// </summary>
        public event UIElementEventHandler Blurred;

        /// <summary>
        /// Occurs when a key is pressed while the element has input focus.
        /// </summary>
        public event UIElementKeyPressedEventHandler KeyPressed;

        /// <summary>
        /// Occurs when a key is released while the element has input focus.
        /// </summary>
        public event UIElementKeyReleasedEventHandler KeyReleased;

        /// <summary>
        /// Occurs when the element receives text input from the keyboard.
        /// </summary>
        public event UIElementKeyboardEventHandler TextInput;

        /// <summary>
        /// Occurs when the element gains mouse capture.
        /// </summary>
        public event UIElementEventHandler GainedMouseCapture;

        /// <summary>
        /// Occurs when the element loses mouse capture.
        /// </summary>
        public event UIElementEventHandler LostMouseCapture;

        /// <summary>
        /// Occurs when the mouse moves over the control.
        /// </summary>
        public event UIElementMouseMotionEventHandler MouseMotion;

        /// <summary>
        /// Occurs when the mouse cursor enters the element.
        /// </summary>
        public event UIElementMouseEventHandler MouseEnter;

        /// <summary>
        /// Occurs when the mouse cursor leaves the element.
        /// </summary>
        public event UIElementMouseEventHandler MouseLeave;

        /// <summary>
        /// Occurs when a mouse button is pressed while the cursor is over the element.
        /// </summary>
        public event UIElementMouseButtonEventHandler MouseButtonPressed;

        /// <summary>
        /// Occurs when a mouse button is released while the cursor is over the element.
        /// </summary>
        public event UIElementMouseButtonEventHandler MouseButtonReleased;

        /// <summary>
        /// Occurs when the value of the <see cref="IsEnabled"/> property changes.
        /// </summary>
        public event UIElementEventHandler IsEnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsHovering"/> property changes.
        /// </summary>
        public event UIElementEventHandler IsHoveringChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsHitTestVisible"/> property changes.
        /// </summary>
        public event UIElementEventHandler IsHitTestVisibleChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Focusable"/> dependency property changes.
        /// </summary>
        public event UIElementEventHandler FocusableChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Visibility"/> property changes.
        /// </summary>
        public event UIElementEventHandler VisibilityChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Opacity"/> property changes.
        /// </summary>
        public event UIElementEventHandler OpacityChanged;

        /// <summary>
        /// Identifies the <see cref="IsEnabled"/> dependency property.
        /// </summary>
        [Styled("enabled")]
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(Boolean), typeof(UIElement),
            new DependencyPropertyMetadata(HandleIsEnabledChanged, () => true, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="IsHitTestVisible"/> dependency property.
        /// </summary>
        [Styled("hit-test-visible")]
        public static readonly DependencyProperty IsHitTestVisibleProperty = DependencyProperty.Register("IsHitTestVisible", typeof(Boolean), typeof(UIElement),
            new DependencyPropertyMetadata(HandleIsHitTestVisibleChanged, () => true, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="Focusable"/> dependency property.
        /// </summary>
        [Styled("focusable")]
        public static readonly DependencyProperty FocusableProperty = DependencyProperty.Register("Focusable", typeof(Boolean), typeof(UIElement),
            new DependencyPropertyMetadata(HandleFocusableChanged, () => false, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="Visibility"/> dependency property.
        /// </summary>
        [Styled("visibility")]
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(UIElement),
            new DependencyPropertyMetadata(HandleVisibilityChanged, () => Visibility.Visible, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Opacity"/> dependency property.
        /// </summary>
        [Styled("opacity")]
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register("Opacity", typeof(Single), typeof(UIElement),
            new DependencyPropertyMetadata(HandleOpacityChanged, () => 1.0f, DependencyPropertyOptions.None));

        /// <summary>
        /// Applies a visual state transition to the element.
        /// </summary>
        /// <param name="style">The style which defines the state transition.</param>
        /// <param name="value">The transition value.</param>
        internal virtual void ApplyStyledVisualStateTransition(UvssStyle style, String value)
        {

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
                if (Parent != null && String.Equals(Parent.Name, name.Container, StringComparison.OrdinalIgnoreCase))
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
        /// Gets the position that was most recently passed to the <see cref="Position(Point2D)"/> method.
        /// </summary>
        internal Point2D MostRecentPosition
        {
            get { return mostRecentPosition; }
        }

        /// <summary>
        /// Gets the element's depth within the layout tree.
        /// </summary>
        internal Int32 LayoutDepth
        {
            get { return layoutDepth; }
        }

        /// <inheritdoc/>
        protected internal sealed override void OnMeasureAffectingPropertyChanged()
        {
            if (Parent != null)
            {
                Parent.InvalidateMeasure();
            }
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

            var name  = style.Name;
            var value = style.Value.Trim();

            if (name == "transition")
            {
                ApplyStyledVisualStateTransition(style, value);
            }
            else
            {
                var setter = attached ? Parent.GetStyleSetter(name, selector.PseudoClass) : GetStyleSetter(name, selector.PseudoClass);
                if (setter == null)
                    return;

                setter(this, value, CultureInfo.InvariantCulture);
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
        /// Raises the <see cref="Focused"/> event.
        /// </summary>
        protected internal virtual void OnFocused()
        {
            var temp = Focused;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Blurred"/> event.
        /// </summary>
        protected internal virtual void OnBlurred()
        {
            var temp = Blurred;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyPressed"/> event.
        /// </summary>
        /// <param name="device">The keyboard device.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="ctrl">A value indicating whether the Control modifier is active.</param>
        /// <param name="alt">A value indicating whether the Alt modifier is active.</param>
        /// <param name="shift">A value indicating whether the Shift modifier is active.</param>
        /// <param name="repeat">A value indicating whether this is a repeated key press.</param>
        protected internal virtual void OnKeyPressed(KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat)
        {
            var temp = KeyPressed;
            if (temp != null)
            {
                temp(this, device, key, ctrl, alt, shift, repeat);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyReleased"/> event.
        /// </summary>
        /// <param name="device">The keyboard device.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was released.</param>
        protected internal virtual void OnKeyReleased(KeyboardDevice device, Key key)
        {
            var temp = KeyReleased;
            if (temp != null)
            {
                temp(this, device, key);
            }
        }

        /// <summary>
        /// Raises the <see cref="TextInput"/> event.
        /// </summary>
        /// <param name="device">The keyboard device.</param>
        protected internal virtual void OnTextInput(KeyboardDevice device)
        {
            var temp = TextInput;
            if (temp != null)
            {
                temp(this, device);
            }
        }

        /// <summary>
        /// Raises the <see cref="GainedMouseCapture"/> event.
        /// </summary>
        protected internal virtual void OnGainedMouseCapture()
        {
            var temp = GainedMouseCapture;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="LostMouseCapture"/> event.
        /// </summary>
        protected internal virtual void OnLostMouseCapture()
        {
            var temp = LostMouseCapture;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseMotion"/> event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The x-coordinate of the cursor in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the cursor in device-independent screen coordinates.</param>
        /// <param name="dx">The difference between the x-coordinate of the mouse's 
        /// current position and the x-coordinate of the mouse's previous position.</param>
        /// <param name="dy">The difference between the y-coordinate of the mouse's 
        /// current position and the y-coordinate of the mouse's previous position.</param>
        protected internal virtual void OnMouseMotion(MouseDevice device, Double x, Double y, Double dx, Double dy)
        {
            var temp = MouseMotion;
            if (temp != null)
            {
                temp(this, device, x, y, dx, dy);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseEnter"/> event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        protected internal virtual void OnMouseEnter(MouseDevice device)
        {
            IsHovering = true;

            var temp = MouseEnter;
            if (temp != null)
            {
                temp(this, device);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseLeave"/> event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        protected internal virtual void OnMouseLeave(MouseDevice device)
        {
            IsHovering = false;

            var temp = MouseLeave;
            if (temp != null)
            {
                temp(this, device);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseButtonPressed"/> event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        protected internal virtual void OnMouseButtonPressed(MouseDevice device, MouseButton button)
        {
            var temp = MouseButtonPressed;
            if (temp != null)
            {
                temp(this, device, button);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseButtonReleased"/> event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        protected internal virtual void OnMouseButtonReleased(MouseDevice device, MouseButton button)
        {
            var temp = MouseButtonReleased;
            if (temp != null)
            {
                temp(this, device, button);
            }
        }

        /// <summary>
        /// Removes the specified child element from this element.
        /// </summary>
        /// <param name="child">The child element to remove from this element.</param>
        protected internal virtual void RemoveChild(UIElement child)
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
            get { return Parent; }
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
        /// Raises the <see cref="IsHoveringChanged"/> event.
        /// </summary>
        protected virtual void OnIsHoveringChanged()
        {
            var temp = IsHoveringChanged;
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
        }

        /// <summary>
        /// When overridden in a derived class, reloads this element's content 
        /// and the content of any children of this element.
        /// </summary>
        protected virtual void ReloadContentCore(Boolean recursive)
        {

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
        }

        /// <summary>
        /// When overridden in a derived class, performs cleanup operations and releases any 
        /// internal framework resources for this element and any child elements.
        /// </summary>
        protected virtual void CleanupCore()
        {

        }

        /// <summary>
        /// When overridden in a derived class, caches layout parameters related to the
        /// element's position within the element hierarchy for this element and for
        /// any child elements.
        /// </summary>
        protected virtual void CacheLayoutParametersCore()
        {

        }

        /// <summary>
        /// When overridden in a derived class, animates this element using the specified storyboard.
        /// </summary>
        /// <param name="storyboard">The storyboard being applied to the element.</param>
        /// <param name="clock">The storyboard clock that tracks playback.</param>
        /// <param name="root">The root element to which the storyboard is being applied.</param>
        protected virtual void AnimateCore(Storyboard storyboard, StoryboardClock clock, UIElement root)
        {

        }

        /// <summary>
        /// When overridden in a derived class, applies the specified stylesheet
        /// to this element and to any child elements.
        /// </summary>
        /// <param name="stylesheet">The stylesheet to apply to this element and its children.</param>
        protected virtual void StyleCore(UvssDocument stylesheet)
        {

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
        /// When overridden in a derived class, positions the element in absolute screen space.
        /// </summary>
        /// <param name="position">The position of the element's parent element in absolute screen space.</param>
        protected virtual void PositionCore(Point2D position)
        {

        }

        /// <summary>
        /// When overridden in a derived class, calculates the clipping rectangle for this element.
        /// </summary>
        /// <returns>The clipping rectangle for this element in absolute screen coordinates, or <c>null</c> to disable clipping.</returns>
        protected virtual RectangleD? ClipCore()
        {
            var clipOffset = (Parent == null ? Point2D.Zero : IsComponent ? Parent.AbsolutePosition : Parent.AbsoluteContentRegion.Location);
            var clip       = mostRecentFinalRect + clipOffset;

            if (clip.Contains(AbsoluteBounds))
            {
                return null;
            }

            return clip;
        }

        /// <summary>
        /// When overridden in a derived class, calculates the content clipping rectangle for this element.
        /// </summary>
        /// <returns>The content clipping rectangle for this element in absolute screen coordinates, or <c>null</c> to disable clipping.</returns>
        protected virtual RectangleD? ClipContentCore()
        {
            return null;
        }

        /// <summary>
        /// When overridden in a derived class, gets the element at the specified device-independent 
        /// coordinates relative to this element's bounds.
        /// </summary>
        /// <param name="x">The element-relative x-coordinate of the point to evaluate.</param>
        /// <param name="y">The element-relative y-coordinate of the point to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="IsHitTestVisible"/> property.</param>
        /// <returns>The element at the specified coordinates, or <c>null</c> if no such element exists.</returns>
        protected virtual UIElement GetElementAtPointCore(Double x, Double y, Boolean isHitTest)
        {
            return Bounds.Contains(x, y) ? this : null;
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
        /// Occurs when the value of the <see cref="IsEnabled"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleIsEnabledChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnIsEnabledChanged();
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
            if (Parent == null)
                return;

            this.view = null;
            for (var current = Parent; current != null; current = current.Parent)
            {
                if (current.View != null)
                {
                    this.view = current.View;
                    break;
                }
            }
        }

        /// <summary>
        /// Updates the value of the <see cref="Control"/> property.
        /// </summary>
        private void CacheControl()
        {
            UnregisterElement();

            this.control = FindControl();

            RegisterElement();
        }

        /// <summary>
        /// Adds the element to the current view's element registry.
        /// </summary>
        private void RegisterElement()
        {
            if (String.IsNullOrEmpty(id))
                return;

            elementRegistrationContext = FindElementRegistry();
            if (elementRegistrationContext != null)
            {
                elementRegistrationContext.RegisterElement(this);
            }
        }

        /// <summary>
        /// Removes the element from the current view's element registry.
        /// </summary>
        private void UnregisterElement()
        {
            if (String.IsNullOrEmpty(id))
                return;

            if (elementRegistrationContext != null)
            {
                elementRegistrationContext.UnregisterElement(this);
                elementRegistrationContext = null;
            }
        }

        /// <summary>
        /// Finds the element registration context for this element.
        /// </summary>
        /// <returns>The element registration context for this element.</returns>
        private UIElementRegistry FindElementRegistry()
        {
            if (Control != null)
            {
                return Control.ComponentRegistry;
            }
            return (view == null) ? null : view.ElementRegistry;
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

        // Property values.
        private readonly UltravioletContext uv;
        private readonly UIElementClassCollection classes;
        private readonly String id;
        private readonly String name;
        private PresentationFoundationView view;
        private UIElement parent;
        private Control control = null;
        private Boolean isStyleValid;
        private Boolean isMeasureValid;
        private Boolean isArrangeValid;
        private Boolean isPositionValid;
        private Boolean isHovering;
        private Point2D renderOffset;
        private Size2D renderSize;
        private Size2D desiredSize;
        private RectangleD relativeBounds;
        private RectangleD absoluteBounds;
        private RectangleD? clipRectangle;
        private RectangleD? clipContentRectangle;

        // Layout parameters.
        private UvssDocument mostRecentStylesheet;
        private ArrangeOptions mostRecentArrangeOptions;
        private RectangleD mostRecentFinalRect;
        private Size2D mostRecentAvailableSize;
        private Point2D mostRecentPosition;
        private Int32 layoutDepth;

        // State values.
        private UIElementRegistry elementRegistrationContext;

        // The collection of active storyboard clocks on this element.
        private readonly Dictionary<Storyboard, StoryboardClock> storyboardClocks = 
            new Dictionary<Storyboard, StoryboardClock>();
    }
}
