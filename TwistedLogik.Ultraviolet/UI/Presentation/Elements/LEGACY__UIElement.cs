using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the method that is called when a UI element is drawn.
    /// </summary>
    /// <param name="element">The element being drawn.</param>
    /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
    /// <param name="spriteBatch">The sprite batch with which to draw the view.</param>
    /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
    public delegate void UIElementDrawingEventHandler(UIElement element, UltravioletTime time, SpriteBatch spriteBatch, Single opacity);

    /// <summary>
    /// Represents the method that is called when a UI element is updated.
    /// </summary>
    /// <param name="element">The element being updated.</param>
    /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
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
    /// <param name="x">The x-coordinate of the cursor in screen coordinates.</param>
    /// <param name="y">The y-coordinate of the cursor in screen coordinates.</param>
    /// <param name="dx">The difference between the x-coordinate of the mouse's 
    /// current position and the x-coordinate of the mouse's previous position.</param>
    /// <param name="dy">The difference between the y-coordinate of the mouse's 
    /// current position and the y-coordinate of the mouse's previous position.</param>
    public delegate void UIElementMouseMotionEventHandler(UIElement element, MouseDevice device, Int32 x, Int32 y, Int32 dx, Int32 dy);

    /// <summary>
    /// Represents the method that is called when a UI element raises an event.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    public delegate void UIElementEventHandler(UIElement element);

    /// <summary>
    /// The base class for all UI elements.
    /// </summary>
    [UIElement("Element")]
    public abstract class UIElement : DependencyObject
    {
        /// <summary>
        /// Represents a method which sets the value of a styled property on a UI element.
        /// </summary>
        /// <param name="element">The UI element on which to set the style.</param>
        /// <param name="value">The string representation of the value to set for the style.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        internal delegate void StyleSetter(UIElement element, String value, IFormatProvider provider);

        /// <summary>
        /// Initialies the <see cref="UIElement"/> type.
        /// </summary>
        static UIElement()
        {
            miFromString = typeof(ObjectResolver).GetMethod("FromString", new Type[] { typeof(String), typeof(Type), typeof(IFormatProvider) });
            miSetStyledValue = typeof(DependencyObject).GetMethod("SetStyledValue");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElement"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public UIElement(UltravioletContext uv, String id)
        {
            Contract.Require(uv, "uv");

            this.uv                = uv;
            this.id                = id;
            this.classes           = new UIElementClassCollection(this);
            this.visualStateGroups = new VisualStateGroupCollection(this);
            
            var attr = (UIElementAttribute)GetType().GetCustomAttributes(typeof(UIElementAttribute), false).SingleOrDefault();
            if (attr != null)
            {
                this.name = attr.Name;
            }

            CreateStyleSetters();

            VisualStateGroups.Create("focus", new[] { "blurred", "focused" });
        }

        /// <summary>
        /// Gets a value indicating whether the specified UI element should be drawn.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the element should be drawn; otherwise, <c>false</c>.</returns>
        public static Boolean ElementIsDrawn(UIElement element)
        {
            return element.Visibility == Visibility.Visible && element.Opacity > 0f;
        }

        /// <summary>
        /// Gets a value indicating whether the specified element participates in layout operations.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the element is part of the layout; otherwise, <c>false</c>.</returns>
        public static Boolean ElementParticipatesInLayout(UIElement element)
        {
            return (element.Visibility != Visibility.Collapsed);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is visible to the input system.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <param name="hitTest">A value indicating whether a hit test is being performed.</param>
        /// <returns><c>true</c> if the element receives user input; otherwise, <c>false</c>.</returns>
        public static Boolean ElementIsVisibleToInput(UIElement element, Boolean hitTest)
        {
            return (element.Visibility == Visibility.Visible && element.Enabled && (!hitTest || element.HitTestVisible));
        }

        /// <summary>
        /// Cleans up the element and its descendants.
        /// </summary>
        /// <remarks>The <see cref="Cleanup()"/> method releases any internal resources which the element
        /// and its descendants may be holding, allowing them to be reused by the Presentation Framework.
        /// The object remains usable after a call to this method, but certain state values (such as
        /// animations, dependency property values, etc.) may be reset.</remarks>
        public void Cleanup()
        {
            ClearAnimationsRecursive();
        }

        /// <summary>
        /// Updates the layout of this element and any child elements.
        /// </summary>
        public void UpdateLayout()
        {
            if (!IsMeasureValid)
                Measure(PreviousMeasureSize);

            if (!IsArrangeValid)
                Arrange(PreviousArrangeRect);
        }

        /// <summary>
        /// Updates the <see cref="DesiredSize"/> of this element.
        /// </summary>
        /// <param name="availableSize">The available space for this element.</param>
        public void Measure(Size2 availableSize)
        {
            this.previousMeasureSize = availableSize;

            desiredSize = MeasureCore(availableSize);

            var pad = MeasureUtil.ConvertThicknessToPixels(Ultraviolet, Padding, 0);

            desiredSize = new Size2(desiredSize.Width + (int)pad.Left + (int)pad.Right,
                desiredSize.Height + (int)pad.Top + (int)pad.Bottom);

            isMeasureValid = true;
        }

        /// <summary>
        /// Positions child elements and determines a size for the element.
        /// </summary>
        /// <param name="finalRect">The final size that the parent computes for the child element.</param>
        public void Arrange(Rectangle finalRect)
        {
            this.finalRect = finalRect;
            this.previousArrangeRect = finalRect;

            ArrangeCore(finalRect);

            isArrangeValid = true;

            UpdateAbsoluteParentOrigin(AbsoluteParentOriginX, AbsoluteParentOriginY);
        }

        internal Int32 LayoutDepth
        {
            get { return layoutDepth; }
        }

        internal Size2 PreviousMeasureSize
        {
            get { return previousMeasureSize; }
        }

        internal Rectangle PreviousArrangeRect
        {
            get { return previousArrangeRect; }
        }

        /// <summary>
        /// Invalidates the measurement state of the element.
        /// </summary>
        public void InvalidateMeasure()
        {
            isMeasureValid = false;
            Ultraviolet.GetUI().PresentationFramework.MeasureQueue.Enqueue(this);
        }

        /// <summary>
        /// Invalidates the arrangement state of the element.
        /// </summary>
        public void InvalidateArrange()
        {
            isArrangeValid = false;
            Ultraviolet.GetUI().PresentationFramework.ArrangeQueue.Enqueue(this);
        }

        /// <summary>
        /// Recursively clears the animations applied to all of the element's dependency properties
        /// and all of the dependency properties of the element's descendents.
        /// </summary>
        public virtual void ClearAnimationsRecursive()
        {
            ClearAnimations();
        }

        /// <summary>
        /// Recursively clears the local values of all of the element's dependency properties
        /// and all of the dependency properties of the element's descendents.
        /// </summary>
        public virtual void ClearLocalValuesRecursive()
        {
            ClearLocalValues();
        }

        /// <summary>
        /// Recursively clears the styled values of all of the element's dependency properties
        /// and all of the dependency properties of the element's descendents.
        /// </summary>
        public virtual void ClearStyledValuesRecursive()
        {
            ClearStyledValues();
            ClearVisualStateTransitions();
        }

        /// <summary>
        /// Resets the element's visual state transitions.
        /// </summary>
        public virtual void ClearVisualStateTransitions()
        {
            visualStateGroups.ClearVisualStateTransitions();
        }

        /// <summary>
        /// Resets the element's visual state transitions.
        /// </summary>
        public virtual void ClearVisualStateTransitionsRecursive()
        {
            ClearVisualStateTransitions();
        }

        /// <summary>
        /// Called when the element should reload its content.
        /// </summary>
        public virtual void ReloadContent()
        {
            ReloadFont();
            ReloadBackgroundImage();
            ReloadFocusedImage();

            OnReloadingContent();
        }

        /// <summary>
        /// Called when the element and its children should reload their content.
        /// </summary>
        public virtual void ReloadContentRecursive()
        {
            ReloadContent();
        }

        /// <summary>
        /// Converts a position in screen space to a position in element space.
        /// </summary>
        /// <param name="x">The x-coordinate of the screen space position to convert.</param>
        /// <param name="y">The y-coordinate of the screen space position to convert.</param>
        /// <returns>The converted element space position.</returns>
        public Vector2 ScreenPositionToElementPosition(Int32 x, Int32 y)
        {
            return new Vector2(x - AbsoluteScreenX, y - AbsoluteScreenY);
        }

        /// <summary>
        /// Converts a position in screen space to a position in element space.
        /// </summary>
        /// <param name="position">The screen space position to convert.</param>
        /// <returns>The converted element space position.</returns>
        public Vector2 ScreenPositionToElementPosition(Vector2 position)
        {
            return ScreenPositionToElementPosition((Int32)position.X, (Int32)position.Y);
        }

        /// <summary>
        /// Converts a position in element space to a position in screen space.
        /// </summary>
        /// <param name="x">The x-coordinate of the element space position to convert.</param>
        /// <param name="y">The y-coordinate of the element space position to convert.</param>
        /// <returns>The converted screen space position.</returns>
        public Vector2 ElementPositionToScreenPosition(Int32 x, Int32 y)
        {
            return new Vector2(x + AbsoluteScreenX, y + AbsoluteScreenY);
        }

        /// <summary>
        /// Converts a position in element space to a position in screen space.
        /// </summary>
        /// <param name="position">The element space position to convert.</param>
        /// <returns>The converted screen space position.</returns>
        public Vector2 ElementPositionToScreenPosition(Vector2 position)
        {
            return ElementPositionToScreenPosition((Int32)position.X, (Int32)position.Y);
        }

        /// <summary>
        /// Gets the element at the specified point in element space.
        /// </summary>
        /// <param name="x">The x-coordinate of the point to evaluate.</param>
        /// <param name="y">The y-coordinate of the point to evaluate.</param>
        /// <param name="hitTest">A value indicating whether to honor the value of the <see cref="HitTestVisible"/> property.</param>
        /// <returns>The element at the specified point in element space, or null if no such element exists.</returns>
        public UIElement GetElementAtPoint(Int32 x, Int32 y, Boolean hitTest)
        {
            return GetElementAtPointInternal(x, y, hitTest);
        }

        /// <summary>
        /// Gets the element at the specified point in element space.
        /// </summary>
        /// <param name="position">The point to evaluate.</param>
        /// <param name="hitTest">A value indicating whether to honor the value of the <see cref="HitTestVisible"/> property.</param>
        /// <returns>The element at the specified point in element space, or null if no such element exists.</returns>
        public UIElement GetElementAtPoint(Vector2 position, Boolean hitTest)
        {
            return GetElementAtPointInternal((Int32)position.X, (Int32)position.Y, hitTest);
        }

        /// <summary>
        /// Gets the Ultraviolet context that created the element.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get { return uv; }
        }

        /// <summary>
        /// Gets the element's collection of styling classes.
        /// </summary>
        public UIElementClassCollection Classes
        {
            get { return classes; }
        }

        /// <summary>
        /// Gets the element's collection of visual state groups.
        /// </summary>
        public VisualStateGroupCollection VisualStateGroups
        {
            get { return visualStateGroups; }
        }

        /// <summary>
        /// Gets the element's unique identifier within its view.
        /// </summary>
        public String ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the element's name based on its type.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the <see cref="UIView"/> that is the top-level container for this element.
        /// </summary>
        public UIView View
        {
            get { return view; }
        }

        /// <summary>
        /// Gets the element's view model.
        /// </summary>
        public Object ViewModel
        {
            get { return viewModel; }
        }

        /// <summary>
        /// Gets the element's parent element.
        /// </summary>
        public UIElement Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// Gets the element's associated control. This will be <c>null</c> unless the element is
        /// a component; in that case, this property will hold a reference to the element of which
        /// this element is a component.
        /// </summary>
        public Control Control
        {
            get { return control; }
        }

        /// <summary>
        /// Gets the element's bounding box in element space.
        /// </summary>
        public Rectangle Bounds
        {
            get { return new Rectangle(0, 0, ActualWidth, ActualHeight); }
        }

        /// <summary>
        /// Gets the element's bounding box in screen space.
        /// </summary>
        public Rectangle ScreenBounds
        {
            get { return new Rectangle(AbsoluteScreenX, AbsoluteScreenY, ActualWidth, ActualHeight); }
        }

        /// <summary>
        /// Gets the size that was computed for this element during the measure pass of the layout process.
        /// </summary>
        public Size2 DesiredSize
        {
            get { return desiredSize; }
        }

        /// <summary>
        /// Gets a value indicating whether this element is a component of a control.
        /// </summary>
        public Boolean IsComponent
        {
            get { return control != null; }
        }

        /// <summary>
        /// Gets a value indicating whether the element's current measurement state is valid.
        /// </summary>
        public Boolean IsMeasureValid
        {
            get
            {
                return isMeasureValid;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the element's current arrangement state is valid.
        /// </summary>
        public Boolean IsArrangeValid
        {
            get
            {
                return isArrangeValid;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is visible to a hit test (i.e., for picking via the cursor).
        /// </summary>
        public Boolean HitTestVisible
        {
            get { return GetValue<Boolean>(HitTestVisibleProperty); }
            set { SetValue<Boolean>(HitTestVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element can gain input focus.
        /// </summary>
        public Boolean Focusable
        {
            get { return GetValue<Boolean>(FocusableProperty); }
            set { SetValue<Boolean>(FocusableProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is enabled.
        /// </summary>
        public Boolean Enabled
        {
            get { return GetValue<Boolean>(EnabledProperty); }
            set { SetValue<Boolean>(EnabledProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse cursor is hovering over this element.
        /// </summary>
        public Boolean Hovering
        {
            get { return hovering; }
            private set
            {
                if (hovering != value)
                {
                    hovering = value;
                    OnHoveringChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value specifying the element's visibility.
        /// </summary>
        public Visibility Visibility
        {
            get { return GetValue<Visibility>(VisibleProperty); }
            set { SetValue<Visibility>(VisibleProperty, value); }
        }

        /// <summary>
        /// Gets the element's width in device independent pixel units (1/96 of an inch).
        /// </summary>
        public Double Width
        {
            get { return GetValue<Double>(WidthProperty); }
            set { SetValue<Double>(WidthProperty, value); }
        }

        /// <summary>
        /// Gets the element's minimum width in device independent pixel units (1/96 of an inch).
        /// </summary>
        public Double MinWidth
        {
            get { return GetValue<Double>(MinWidthProperty); }
            set { SetValue<Double>(MinWidthProperty, value); }
        }

        /// <summary>
        /// Gets the element's maximum width in device independent pixel units (1/96 of an inch).
        /// </summary>
        public Double MaxWidth
        {
            get { return GetValue<Double>(MaxWidthProperty); }
            set { SetValue<Double>(MaxWidthProperty, value); }
        }

        /// <summary>
        /// Gets the element's height in device independent pixel units (1/96 of an inch).
        /// </summary>
        public Double Height
        {
            get { return GetValue<Double>(HeightProperty); }
            set { SetValue<Double>(HeightProperty, value); }
        }

        /// <summary>
        /// Gets the element's minimum height in device independent pixel units (1/96 of an inch).
        /// </summary>
        public Double MinHeight
        {
            get { return GetValue<Double>(MinHeightProperty); }
            set { SetValue<Double>(MinHeightProperty, value); }
        }

        /// <summary>
        /// Gets the element's maximum height in device independent pixel units (1/96 of an inch).
        /// </summary>
        public Double MaxHeight
        {
            get { return GetValue<Double>(MaxHeightProperty); }
            set { SetValue<Double>(MaxHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount of padding around the element's content.
        /// </summary>
        public Thickness Padding
        {
            get { return GetValue<Thickness>(PaddingProperty); }
            set { SetValue<Thickness>(PaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's outer margin.
        /// </summary>
        public Thickness Margin
        {
            get { return GetValue<Thickness>(MarginProperty); }
            set { SetValue<Thickness>(MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's horizontal alignment relative to its parent element.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment
        {
            get { return GetValue<HorizontalAlignment>(HorizontalAlignmentProperty); }
            set { SetValue<HorizontalAlignment>(HorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's vertical alignment relative to its parent element.
        /// </summary>
        public VerticalAlignment VerticalAlignment
        {
            get { return GetValue<VerticalAlignment>(VerticalAlignmentProperty); }
            set { SetValue<VerticalAlignment>(VerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the opacity applied to this element and its children. Expected values are
        /// between 0.0 (fully transparent) and 1.0 (fully opaque).
        /// </summary>
        public Single Opacity
        {
            get { return GetValue<Single>(OpacityProperty); }
            set { SetValue<Single>(OpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's font color.
        /// </summary>
        public Color FontColor
        {
            get { return GetValue<Color>(FontColorProperty); }
            set { SetValue<Color>(FontColorProperty, value); }
        }

        /// <summary>
        /// Gets the asset identifier of the element's font.
        /// </summary>
        public SourcedVal<AssetID> FontAssetID
        {
            get { return GetValue<SourcedVal<AssetID>>(FontAssetIDProperty); }
            set { SetValue<SourcedVal<AssetID>>(FontAssetIDProperty, value); }
        }

        /// <summary>
        /// Gets the element's font.
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }

        /// <summary>
        /// Gets or sets the element's background color.
        /// </summary>
        public Color BackgroundColor
        {
            get { return GetValue<Color>(BackgroundColorProperty); }
            set { SetValue<Color>(BackgroundColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's background image.
        /// </summary>
        public SourcedRef<Image> BackgroundImage
        {
            get { return GetValue<SourcedRef<Image>>(BackgroundImageProperty); }
            set { SetValue<SourcedRef<Image>>(BackgroundImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color which is used to display the image that indicates
        /// that the element has input focus.
        /// </summary>
        public Color FocusedColor
        {
            get { return GetValue<Color>(FocusedColorProperty); }
            set { SetValue<Color>(FocusedColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image that indicates that the element has input focus.
        /// </summary>
        public SourcedRef<Image> FocusedImage
        {
            get { return GetValue<SourcedRef<Image>>(FocusedImageProperty); }
            set { SetValue<SourcedRef<Image>>(FocusedImageProperty, value); }
        }

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
        /// Occurs when the value of the <see cref="HitTestVisible"/> property changes.
        /// </summary>
        public event UIElementEventHandler HitTestVisibleChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Focusable"/> property changes.
        /// </summary>
        public event UIElementEventHandler FocusableChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Enabled"/> property changes.
        /// </summary>
        public event UIElementEventHandler EnabledChanged;
        
        /// <summary>
        /// Occurs when the value of the <see cref="Hovering"/> property changes.
        /// </summary>
        public event UIElementEventHandler HoveringChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Visibility"/> property changes.
        /// </summary>
        public event UIElementEventHandler VisibilityChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Width"/> property changes.
        /// </summary>
        public event UIElementEventHandler WidthChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MinWidth"/> property changes.
        /// </summary>
        public event UIElementEventHandler MinWidthChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MaxWidth"/> property changes.
        /// </summary>
        public event UIElementEventHandler MaxWidthChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Height"/> property changes.
        /// </summary>
        public event UIElementEventHandler HeightChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MinHeight"/> property changes.
        /// </summary>
        public event UIElementEventHandler MinHeightChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MaxHeight"/> property changes.
        /// </summary>
        public event UIElementEventHandler MaxHeightChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Padding"/> property changes.
        /// </summary>
        public event UIElementEventHandler PaddingChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        public event UIElementEventHandler MarginChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalAlignment"/> property changes.
        /// </summary>
        public event UIElementEventHandler HorizontalAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalAlignment"/> property changes.
        /// </summary>
        public event UIElementEventHandler VerticalAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Opacity"/> property changes.
        /// </summary>
        public event UIElementEventHandler OpacityChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FontColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler FontColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FontAssetID"/> property changes.
        /// </summary>
        public event UIElementEventHandler FontAssetIDChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler BackgroundColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler BackgroundImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FocusedColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler FocusedColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FocusedImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler FocusedImageChanged;

        /// <summary>
        /// Identifies the <see cref="HitTestVisible"/> dependency property.
        /// </summary>
        [Styled("hit-visible")]
        public static readonly DependencyProperty HitTestVisibleProperty = DependencyProperty.Register("HitTestVisible", typeof(Boolean), typeof(UIElement),
            new DependencyPropertyMetadata(HandleHitTestVisibleChanged, () => true, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="Enabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register("Enabled", typeof(Boolean), typeof(UIElement),
            new DependencyPropertyMetadata(HandleEnabledChanged, () => true, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="Focusable"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FocusableProperty = DependencyProperty.Register("Focusable", typeof(Boolean), typeof(UIElement),
            new DependencyPropertyMetadata(HandleFocusableChanged, () => false, DependencyPropertyOptions.None));
        
        /// <summary>
        /// Identifies the <see cref="Visibility"/> dependency property.
        /// </summary>
        [Styled("visibility")]
        public static readonly DependencyProperty VisibleProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(UIElement),
            new DependencyPropertyMetadata(HandleVisibilityChanged, () => Visibility.Visible, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Width"/> dependency property.
        /// </summary>
        [Styled("width")]
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(Double), typeof(UIElement),
            new DependencyPropertyMetadata(HandleWidthChanged, () => Double.NaN, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MinWidth"/> dependency property.
        /// </summary>
        [Styled("min-width")]
        public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(Double), typeof(UIElement),
            new DependencyPropertyMetadata(HandleMinWidthChanged, null, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MaxWidth"/> dependency property.
        /// </summary>
        [Styled("max-width")]
        public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(Double), typeof(UIElement),
            new DependencyPropertyMetadata(HandleMaxWidthChanged, () => Double.PositiveInfinity, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Height"/> dependency property.
        /// </summary>
        [Styled("height")]
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(Double), typeof(UIElement),
            new DependencyPropertyMetadata(HandleHeightChanged, () => Double.NaN, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MinHeight"/> dependency property.
        /// </summary>
        [Styled("min-height")]
        public static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register("MinHeight", typeof(Double), typeof(UIElement),
            new DependencyPropertyMetadata(HandleMinHeightChanged, null, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="MaxHeight"/> dependency property.
        /// </summary>
        [Styled("max-height")]
        public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register("MaxHeight", typeof(Double), typeof(UIElement),
            new DependencyPropertyMetadata(HandleMaxHeightChanged, () => Double.PositiveInfinity, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Padding"/> dependency property.
        /// </summary>
        [Styled("padding")]
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(UIElement),
            new DependencyPropertyMetadata(HandlePaddingChanged, null, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Margin"/> dependency property.
        /// </summary>
        [Styled("margin")]
        public static readonly DependencyProperty MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), typeof(UIElement),
            new DependencyPropertyMetadata(HandleMarginChanged, null, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="HorizontalAlignment"/> dependency property.
        /// </summary>
        [Styled("halign")]
        public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register("HorizontalAlignment", typeof(HorizontalAlignment), typeof(UIElement),
            new DependencyPropertyMetadata(HandleHorizontalAlignmentChanged, () => HorizontalAlignment.Left, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="VerticalAlignment"/> dependency property.
        /// </summary>
        [Styled("valign")]
        public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register("VerticalAlignment", typeof(VerticalAlignment), typeof(UIElement),
            new DependencyPropertyMetadata(HandleVerticalAlignmentChanged, () => VerticalAlignment.Top, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Opacity"/> dependency property.
        /// </summary>
        [Styled("opacity")]
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register("Opacity", typeof(Single), typeof(UIElement),
            new DependencyPropertyMetadata(HandleOpacityChanged, () => 1.0f, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="FontColor"/> dependency property.
        /// </summary>
        [Styled("font-color")]
        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register("FontColor", typeof(Color), typeof(UIElement),
            new DependencyPropertyMetadata(HandleFontColorChanged, () => Color.White, DependencyPropertyOptions.Inherited));

        /// <summary>
        /// Identifies the <see cref="FontAssetID"/> dependency property.
        /// </summary>
        [Styled("font-asset")]
        public static readonly DependencyProperty FontAssetIDProperty = DependencyProperty.Register("FontAssetID", typeof(SourcedVal<AssetID>), typeof(UIElement),
            new DependencyPropertyMetadata(HandleFontAssetIDChanged, null, DependencyPropertyOptions.Inherited | DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="BackgroundColor"/> dependency property.
        /// </summary>
        [Styled("background-color")]
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(UIElement),
            new DependencyPropertyMetadata(HandleBackgroundColorChanged, () => Color.White, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="BackgroundImage"/> dependency property.
        /// </summary>
        [Styled("background-image")]
        public static readonly DependencyProperty BackgroundImageProperty = DependencyProperty.Register("BackgroundImage", typeof(SourcedRef<Image>), typeof(UIElement),
            new DependencyPropertyMetadata(HandleBackgroundImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="FocusedColor"/> dependency property.
        /// </summary>
        [Styled("focused-color")]
        public static readonly DependencyProperty FocusedColorProperty = DependencyProperty.Register("FocusedColor", typeof(Color), typeof(UIElement),
            new DependencyPropertyMetadata(HandleFocusedColorChanged, () => Color.Cyan, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="FocusedImage"/> dependency property.
        /// </summary>
        [Styled("focused-image")]
        public static readonly DependencyProperty FocusedImageProperty = DependencyProperty.Register("FocusedImage", typeof(SourcedRef<Image>), typeof(UIElement),
            new DependencyPropertyMetadata(HandleFocusedImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Finds a styled dependency property according to its styling name.
        /// </summary>
        /// <param name="name">The styling name of the dependency property to retrieve.</param>
        /// <param name="type">The type to search for a dependency property.</param>
        /// <returns>The <see cref="DependencyProperty"/> instance which matches the specified styling name, or <c>null</c> if no
        /// such dependency property exists on this object.</returns>
        internal static DependencyProperty FindStyledDependencyProperty(String name, Type type)
        {
            Contract.RequireNotEmpty("name", name);
            Contract.Require(type, "type");

            lock (styleSyncObject)
            {
                while (type != null)
                {
                    Dictionary<String, DependencyProperty> styledPropertiesForCurrentType;
                    if (styledProperties.TryGetValue(type, out styledPropertiesForCurrentType))
                    {
                        DependencyProperty dp;
                        if (styledPropertiesForCurrentType.TryGetValue(name, out dp))
                        {
                            return dp;
                        }
                    }

                    type = type.BaseType;
                }
            }
            return null;
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
        /// Updates the element's layout depth, which represents its position
        /// within the element tree.
        /// </summary>
        /// <param name="layoutDepth">The element's layout depth.</param>
        internal void UpdateLayoutDepth(Int32 layoutDepth)
        {
            this.layoutDepth = layoutDepth;

            UpdateLayoutDepthCore(layoutDepth);
        }

        /// <summary>
        /// Updates the element's absolute parent origin, which is used to determine
        /// the element's absolute screen position based on its arrangement state.
        /// </summary>
        /// <param name="x">The x-coordinate of the element's parent origin.</param>
        /// <param name="y">The y-coordinate of the element's parent origin.</param>
        internal void UpdateAbsoluteParentOrigin(Int32 x, Int32 y)
        {
            this.absoluteParentOriginX = x;
            this.absoluteParentOriginY = y;

            UpdateAbsoluteParentOriginCore(x, y);
        }

        /// <summary>
        /// Applies a style to the element.
        /// </summary>
        /// <param name="style">The style which is being applied.</param>
        /// <param name="selector">The selector which caused the style to be applied.</param>
        /// <param name="attached">A value indicating whether thie style represents an attached property.</param>
        internal void ApplyStyle(UvssStyle style, UvssSelector selector, Boolean attached)
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
        /// Applies a visual state transition to the element.
        /// </summary>
        /// <param name="style">The style which defines the state transition.</param>
        /// <param name="value">The transition value.</param>
        internal void ApplyStyledVisualStateTransition(UvssStyle style, String value)
        {
            Contract.Require(style, "style");
            Contract.RequireNotEmpty(value, "value");

            if (View != null && View.Stylesheet != null)
            {
                var storyboard = View.Stylesheet.InstantiateStoryboardByName(Ultraviolet, value);
                if (storyboard != null)
                {
                    var group = default(String);
                    var from  = default(String);
                    var to    = default(String);

                    switch (style.Arguments.Count)
                    {
                        case 2:
                            group = style.Arguments[0];
                            from  = null;
                            to    = style.Arguments[1];
                            break;

                        case 3:
                            group = style.Arguments[0];
                            from  = style.Arguments[1];
                            to    = style.Arguments[2];
                            break;

                        default:
                            throw new NotSupportedException();
                    }

                    VisualStateGroups.SetVisualStateTransition(group, from, to, storyboard);
                }
            }
        }

        /// <summary>
        /// Begins playing the specified storyboard on this element.
        /// </summary>
        /// <param name="storyboard">The storyboard to play on this element.</param>
        internal void BeginStoryboard(Storyboard storyboard)
        {
            StoryboardClock existingClock;
            storyboardClocks.TryGetValue(storyboard, out existingClock);

            StoryboardClock clock;
            RetrieveStoryboardClock(storyboard, out clock);
            storyboardClocks[storyboard] = clock;

            ApplyStoryboard(storyboard, clock, this);

            clock.Start();

            if (existingClock != null)
            {
                existingClock.Stop();
                ReleaseStoryboardClock(existingClock);
            }
        }

        /// <summary>
        /// Stops playing the specified storyboard on this element.
        /// </summary>
        /// <param name="storyboard">The storyboard to stop playing on this element.</param>
        internal void StopStoryboard(Storyboard storyboard)
        {
            StoryboardClock clock;
            if (storyboardClocks.TryGetValue(storyboard, out clock))
            {
                clock.Stop();
                storyboardClocks.Remove(storyboard);
                ReleaseStoryboardClock(clock);
            }
        }

        /// <summary>
        /// Attempts to remove the specified child or content element from this element.
        /// </summary>
        /// <param name="element">The child or content element to remove.</param>
        /// <returns><c>true</c> if the child or content element was removed; otherwise, <c>false</c>.</returns>
        internal virtual Boolean RemoveContent(UIElement element)
        {
            return false;
        }

        /// <summary>
        /// Draws the element.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the view.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        /// <returns><c>true</c> if the element was drawn; otherwise, <c>false</c>.</returns>
        internal virtual Boolean Draw(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            OnDrawing(time, spriteBatch, opacity);
            return true;
        }

        /// <summary>
        /// Applies the specified stylesheet's styles to this element and its children.
        /// </summary>
        /// <param name="stylesheet">The stylesheet to apply to the element.</param>
        internal virtual void ApplyStyles(UvssDocument stylesheet)
        {
            stylesheet.ApplyStyles(this);

            VisualStateGroups.ReapplyStates();
        }

        /// <summary>
        /// Applies the specified storyboard to this element.
        /// </summary>
        /// <param name="storyboard">The storyboard being applied to the element.</param>
        /// <param name="clock">The storyboard clock that tracks playback.</param>
        /// <param name="root">The root element to which the storyboard is being applied.</param>
        internal virtual void ApplyStoryboard(Storyboard storyboard, StoryboardClock clock, UIElement root)
        {
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
        }

        /// <summary>
        /// Updates the element's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        internal virtual void Update(UltravioletTime time)
        {
            foreach (var clock in storyboardClocks)
                clock.Value.Update(time);

            Digest(time);
            OnUpdating(time);
        }

        /// <summary>
        /// Updates the view model associated with this element.
        /// </summary>
        /// <param name="viewModel">The view model to associate with this element.</param>
        internal virtual void UpdateViewModel(Object viewModel)
        {
            this.viewModel = viewModel;
        }

        /// <summary>
        /// Updates the view associated with this element.
        /// </summary>
        /// <param name="view">The view to associate with this element.</param>
        internal virtual void UpdateView(UIView view)
        {
            UnregisterElement();

            this.view = view;

            RegisterElement();

            if (view == null || view.Stylesheet == null)
            {
                ClearStyledValues();
                ClearVisualStateTransitions();
            }
            else
            {
                view.Stylesheet.ApplyStyles(this);
            }

            UpdateViewModel(view == null ? null : view.ViewModel);

            ReloadContent();

            InvalidateMeasure();
            InvalidateArrange();
        }

        /// <summary>
        /// Updates the container which holds this element.
        /// </summary>
        /// <param name="container">The container to associate with this element.</param>
        internal virtual void UpdateContainer(UIElement container)
        {
            UnregisterElement();

            this.parent = container;

            UpdateControl();
            UpdateLayoutDepth((container == null) ? 0 : container.LayoutDepth + 1);

            var view = (container == null) ? null : container.View;
            if (view != this.view)
            {
                UpdateView(view);
            }
            else
            {
                RegisterElement();
            }

            InvalidateMeasure();
            InvalidateArrange();
        }

        /// <summary>
        /// Updates the value of the <see cref="Control"/> property.
        /// </summary>
        internal virtual void UpdateControl()
        {
            UnregisterElement();

            this.control = FindControl();

            RegisterElement();
        }

        /// <summary>
        /// Gets the element at the specified point in element space.
        /// </summary>
        /// <param name="x">The x-coordinate of the point to evaluate.</param>
        /// <param name="y">The y-coordinate of the point to evaluate.</param>
        /// <param name="hitTest">A value indicating whether to honor the value of the <see cref="HitTestVisible"/> property.</param>
        /// <returns>The element at the specified point in element space, or null if no such element exists.</returns>
        internal virtual UIElement GetElementAtPointInternal(Int32 x, Int32 y, Boolean hitTest)
        {
            return Bounds.Contains(x, y) && ElementIsVisibleToInput(this, hitTest) ? this : null;
        }

        /// <summary>
        /// Searches the element hierarchy for an instance of the <see cref="ContentPanel"/> class.
        /// </summary>
        /// <returns>The content panel, if it was found; otherwise, <c>null</c>.</returns>
        internal virtual UIElement FindContentPanel()
        {
            //if (this is ContentPanel)
            //    return this;

            return null;
        }

        /// <inheritdoc/>
        protected internal override void OnMeasureAffectingPropertyChanged()
        {
            InvalidateMeasure();
            base.OnMeasureAffectingPropertyChanged();
        }

        /// <summary>
        /// Raises the <see cref="Focused"/> event.
        /// </summary>
        protected internal virtual void OnFocused()
        {
            VisualStateGroups.GoToState("focus", "focused");

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
            VisualStateGroups.GoToState("focus", "blurred");

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
        /// <param name="x">The x-coordinate of the cursor in screen coordinates.</param>
        /// <param name="y">The y-coordinate of the cursor in screen coordinates.</param>
        /// <param name="dx">The difference between the x-coordinate of the mouse's 
        /// current position and the x-coordinate of the mouse's previous position.</param>
        /// <param name="dy">The difference between the y-coordinate of the mouse's 
        /// current position and the y-coordinate of the mouse's previous position.</param>
        protected internal virtual void OnMouseMotion(MouseDevice device, Int32 x, Int32 y, Int32 dx, Int32 dy)
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
            Hovering = true;

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
            Hovering = false;

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
        /// Gets the dependency object's containing object.
        /// </summary>
        protected internal sealed override DependencyObject DependencyContainer
        {
            get { return Parent; }
        }

        /// <summary>
        /// Gets or sets the data source from which the object's dependency properties will retrieve values if they are data bound.
        /// </summary>
        protected internal sealed override Object DependencyDataSource
        {
            get { return Control ?? ViewModel; }
        }

        /// <summary>
        /// Gets the component element which contains this element's content.
        /// </summary>
        protected internal UIElement ContentPanel
        {
            get { return contentPanel ?? this; }
            internal set
            {
                if (contentPanel != null && contentPanel != this)
                { /* TODO */ }
   
                contentPanel = value;

                if (contentPanel != null && contentPanel != this)
                { /* TODO */ }
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the element's absolute screen position.
        /// </summary>
        protected Int32 AbsoluteScreenX
        {
            get { return AbsoluteParentOriginX + finalRect.X; }
        }

        /// <summary>
        /// Gets the y-coordinate of the element's absolute screen position.
        /// </summary>
        protected Int32 AbsoluteScreenY
        {
            get { return AbsoluteParentOriginY + finalRect.Y; }
        }

        protected Int32 AbsoluteParentOriginX
        {
            get { return absoluteParentOriginX; }
        }

        protected Int32 AbsoluteParentOriginY
        {
            get { return absoluteParentOriginY; }
        }

        /// <summary>
        /// Gets the element's actual width as calculated during layout.
        /// </summary>
        protected Int32 ActualWidth
        {
            get { return finalRect.Width; }
            private set { actualWidth = value; }
        }

        /// <summary>
        /// Gets the element's actual height as calculated during layout.
        /// </summary>
        protected Int32 ActualHeight
        {
            get { return finalRect.Height; }
            private set { actualHeight = value; }
        }

        /// <summary>
        /// Gets the distance in pixels between the left edge of this control and the left
        /// edge of the control's content region.
        /// </summary>
        protected Int32 ContentPanelX
        {
            get { return ContentPanel.AbsoluteScreenX - this.AbsoluteScreenX; }
        }

        /// <summary>
        /// Gets the distance in pixels between the top edge of this control and the top
        /// edge of the control's content region.
        /// </summary>
        protected Int32 ContentPanelY
        {
            get { return ContentPanel.AbsoluteScreenY - this.AbsoluteScreenY; }
        }

        /// <summary>
        /// Gets the width of the area in which the element renders its content.
        /// </summary>
        protected Int32 ContentPanelWidth
        {
            get { return ContentPanel.ActualWidth; }
        }

        /// <summary>
        /// Gets the height of the area in which the element renders its content.
        /// </summary>
        protected Int32 ContentPanelHeight
        {
            get { return ContentPanel.ActualHeight; }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing)
        {

        }

        /// <summary>
        /// Called when the element should reload its content.
        /// </summary>
        protected virtual void OnReloadingContent()
        {

        }

        /// <summary>
        /// Raises the <see cref="Drawing"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the view.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected virtual void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            var temp = Drawing;
            if (temp != null)
            {
                temp(this, time, spriteBatch, opacity);
            }
        }

        /// <summary>
        /// Raises the <see cref="Updating"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        protected virtual void OnUpdating(UltravioletTime time)
        {
            var temp = Updating;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        /// <summary>
        /// Calculates the size of this element, potentially including any child elements.
        /// </summary>
        /// <param name="availableSize">The amount of space that is available for the element.</param>
        /// <returns>The desired size of this element.</returns>
        protected virtual Size2 MeasureCore(Size2 availableSize)
        {
            return Size2.Zero;
        }

        /// <summary>
        /// Positions the element and arranges its children.
        /// </summary>
        /// <param name="finalRect">The region within the parent element that has been assigned
        /// to this element and its children for layout.</param>
        protected virtual void ArrangeCore(Rectangle finalRect)
        {

        }

        /// <summary>
        /// Updates the layout depth of the element's children.
        /// </summary>
        /// <param name="layoutDepth">The element's layout depth.</param>
        protected virtual void UpdateLayoutDepthCore(Int32 layoutDepth)
        {

        }

        /// <summary>
        /// Updates the absolute parent origin of the element's children.
        /// </summary>
        /// <param name="x">The x-coordinate of the element's parent origin.</param>
        /// <param name="y">The y-coordinate of the element's parent origin.</param>
        protected virtual void UpdateAbsoluteParentOriginCore(Int32 x, Int32 y)
        {

        }

        /// <summary>
        /// Raises the <see cref="HitTestVisibleChanged"/> event.
        /// </summary>
        protected virtual void OnHitTestVisibleChanged()
        {
            var temp = HitTestVisibleChanged;
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
        /// Raises the <see cref="EnabledChanged"/> event.
        /// </summary>
        protected virtual void OnEnabledChanged()
        {
            var temp = EnabledChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="HoveringChanged"/> event.
        /// </summary>
        protected virtual void OnHoveringChanged()
        {
            var temp = HoveringChanged;
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
        /// Raises the <see cref="WidthChanged"/> event.
        /// </summary>
        protected virtual void OnWidthChanged()
        {
            var temp = WidthChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MinWidthChanged"/> event.
        /// </summary>
        protected virtual void OnMinWidthChanged()
        {
            var temp = MinWidthChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MaxWidthChanged"/> event.
        /// </summary>
        protected virtual void OnMaxWidthChanged()
        {
            var temp = MaxWidthChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="HeightChanged"/> event.
        /// </summary>
        protected virtual void OnHeightChanged()
        {
            var temp = HeightChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MinHeightChanged"/> event.
        /// </summary>
        protected virtual void OnMinHeightChanged()
        {
            var temp = MinHeightChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MaxHeightChanged"/> event.
        /// </summary>
        protected virtual void OnMaxHeightChanged()
        {
            var temp = MaxHeightChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="PaddingChanged"/> event.
        /// </summary>
        protected virtual void OnPaddingChanged()
        {
            var temp = PaddingChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MarginChanged"/> event.
        /// </summary>
        protected virtual void OnMarginChanged()
        {
            var temp = MarginChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="HorizontalAlignmentChanged"/> event.
        /// </summary>
        protected virtual void OnHorizontalAlignmentChanged()
        {
            var temp = HorizontalAlignmentChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="VerticalAlignmentChanged"/> event.
        /// </summary>
        protected virtual void OnVerticalAlignmentChanged()
        {
            var temp = VerticalAlignmentChanged;
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
        /// Raises the <see cref="FontColorChanged"/> event.
        /// </summary>
        protected virtual void OnFontColorChanged()
        {
            var temp = FontColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="FontAssetIDChanged"/> event.
        /// </summary>
        protected virtual void OnFontAssetIDChanged()
        {
            var temp = FontAssetIDChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="BackgroundColorChanged"/> event.
        /// </summary>
        protected virtual void OnBackgroundColorChanged()
        {
            var temp = BackgroundColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="BackgroundImageChanged"/> event.
        /// </summary>
        protected virtual void OnBackgroundImageChanged()
        {
            var temp = BackgroundImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="FocusedColorChanged"/> event.
        /// </summary>
        protected virtual void OnFocusedColorChanged()
        {
            var temp = FocusedColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="FocusedImageChanged"/> event.
        /// </summary>
        protected virtual void OnFocusedImageChanged()
        {
            var temp = FocusedImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Draws the element's background image.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected virtual void DrawBackgroundImage(SpriteBatch spriteBatch, Single opacity)
        {
            DrawElementImage(spriteBatch, BackgroundImage, null, BackgroundColor * Opacity * opacity, true);
        }

        /// <summary>
        /// Draws the image which indicates that the element has input focus.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected virtual void DrawFocusedImage(SpriteBatch spriteBatch, Single opacity)
        {
            DrawElementImage(spriteBatch, FocusedImage, null, FocusedColor * Opacity * opacity);
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
        /// Loads the specified sourced asset.
        /// </summary>
        /// <typeparam name="TOutput">The type of object being loaded.</typeparam>
        /// <param name="asset">The identifier of the asset to load.</param>
        /// <returns>The asset that was loaded.</returns>
        protected TOutput LoadContent<TOutput>(SourcedVal<AssetID> asset)
        {
            if (View == null)
                return default(TOutput);

            return View.LoadContent<TOutput>(asset);
        }

        /// <summary>
        /// Loads the specified image from the global content manager.
        /// </summary>
        /// <param name="image">The identifier of the image to load.</param>
        protected void LoadGlobalContent<T>(T image) where T : Image
        {
            if (View == null)
                return;

            View.LoadGlobalContent(image);
        }

        /// <summary>
        /// Loads the specified image from the local content manager.
        /// </summary>
        /// <param name="image">The identifier of the image to load.</param>
        protected void LoadLocalContent<T>(T image) where T : Image
        {
            if (View == null)
                return;

            View.LoadLocalContent(image);
        }

        /// <summary>
        /// Loads the specified sourced image.
        /// </summary>
        /// <param name="image">The identifier of the image to load.</param>
        protected void LoadContent<T>(SourcedRef<T> image) where T : Image
        {
            if (View == null)
                return;

            View.LoadContent(image);
        }

        /// <summary>
        /// Reloads the element's font.
        /// </summary>
        protected void ReloadFont()
        {
            this.font = LoadContent<SpriteFont>(FontAssetID);
        }

        /// <summary>
        /// Reloads the element's background image.
        /// </summary>
        protected void ReloadBackgroundImage()
        {
            LoadContent(BackgroundImage);
        }

        /// <summary>
        /// Reloads the element's focused image.
        /// </summary>
        protected void ReloadFocusedImage()
        {
            LoadContent(FocusedImage);
        }

        /// <summary>
        /// Draws the specified element image.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the element image.</param>
        /// <param name="image">The image resource to draw.</param>
        /// <param name="area">The area, relative to the element, in which to draw the image. A value of
        /// <c>null</c> specifies that the image should fill the element's entire area on the screen.</param>
        /// <param name="color">The color with which to draw the element image.</param>
        /// <param name="drawBlankImage">A value indicating whether a blank placeholder should be drawn if 
        /// the specified image does not exist or is not loaded.</param>
        protected void DrawElementImage(SpriteBatch spriteBatch, SourcedRef<Image> image, Rectangle? area, Color color, Boolean drawBlankImage = false)
        {
            if (color.Equals(Color.Transparent))
                return;

            var imageAreaRel = area ?? new Rectangle(0, 0, ActualWidth, ActualHeight);
            var imageAreaAbs = new Rectangle(AbsoluteScreenX + imageAreaRel.X, AbsoluteScreenY + imageAreaRel.Y, imageAreaRel.Width, imageAreaRel.Height);

            var imageResource = image.Value;
            if (imageResource == null || !imageResource.IsLoaded)
            {
                if (drawBlankImage)
                {
                    spriteBatch.Draw(UIElementResources.BlankTexture, imageAreaAbs, color);
                }
            }
            else
            {
                var effects  = SpriteEffects.None;
                var origin   = new Vector2(imageAreaRel.Width / 2f, imageAreaRel.Height / 2f);// imageAreaRel.Center;
                var position = (Vector2)imageAreaAbs.Center;

                spriteBatch.DrawImage(imageResource, position, 
                    imageAreaAbs.Width, imageAreaAbs.Height, color, 0f, origin, effects, 0f);
            }
        }

        /// <summary>
        /// Applies a scissor rectangle.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch which is being used to render the element.</param>
        /// <param name="scissorRectangle">The scissor rectangle to apply.</param>
        /// <returns>If the resulting scissor rectangle is empty, it is not applied, and this method returns
        /// <c>false</c> to indicate that nothing should be drawn.</returns>
        protected Boolean ApplyScissorRectangle(SpriteBatch spriteBatch, Rectangle scissorRectangle)
        {
            var graphics = Ultraviolet.GetGraphics();

            var currentScissorRectangle = graphics.GetScissorRectangle();
            if (currentScissorRectangle != null)
            {
                scissorRectangle = Rectangle.Intersect(scissorRectangle, currentScissorRectangle.Value);
            }

            if (scissorRectangle.IsEmpty)
                return false;

            spriteBatch.Flush();
            graphics.SetScissorRectangle(scissorRectangle);

            previousScissorRectangle = currentScissorRectangle;
            scissorApplied = true;

            return true;
        }

        /// <summary>
        /// Reverts to the previous scissor rectangle, if <see cref="ApplyScissorRectangle(SpriteBatch, Rectangle)"/>
        /// was previously called by this element.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch which is being used to render the element.</param>
        /// <returns><c>true</c> if the scissor rectangle was reverted; otherwise, <c>false</c>.</returns>
        protected Boolean RevertScissorRectangle(SpriteBatch spriteBatch)
        {
            if (!scissorApplied)
                return false;

            spriteBatch.Flush();
            Ultraviolet.GetGraphics().SetScissorRectangle(previousScissorRectangle);
            scissorApplied = false;

            return true;
        }

        /// <summary>
        /// Gets the style setter for the style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style for which to retrieve a setter.</param>
        /// <returns>A function to set the value of the specified style.</returns>
        private StyleSetter GetStyleSetter(String name)
        {
            return GetStyleSetter(name, null);
        }

        /// <summary>
        /// Gets the style setter for the style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style for which to retrieve a setter.</param>
        /// <param name="pseudoClass">The pseudo-class of the style for which to retrieve a setter.</param>
        /// <returns>A function to set the value of the specified style.</returns>
        private StyleSetter GetStyleSetter(String name, String pseudoClass)
        {
            var currentType = GetType();

            lock (styleSetters)
            {
                while (currentType != null && typeof(UIElement).IsAssignableFrom(currentType))
                {   
                    Dictionary<UvssStyleKey, StyleSetter> styleSettersForCurrentType;
                    if (styleSetters.TryGetValue(currentType, out styleSettersForCurrentType))
                    {
                        StyleSetter setter;
                        if (styleSettersForCurrentType.TryGetValue(new UvssStyleKey(name, pseudoClass), out setter))
                        {
                            return setter;
                        }
                    }

                    currentType = currentType.BaseType;
                }
            }

            return null;
        }

        /// <summary>
        /// Dynamically compiles a collection of lambda methods which can be used to apply styles
        /// to the element's properties.
        /// </summary>
        private void CreateStyleSetters()
        {
            var currentType = GetType();

            lock (styleSyncObject)
            {
                while (currentType != null && typeof(UIElement).IsAssignableFrom(currentType))
                {
                    Dictionary<UvssStyleKey, StyleSetter> styleSettersForCurrentType;
                    Dictionary<String, DependencyProperty> styledPropertiesForCurrentType;
                    if (!styleSetters.TryGetValue(currentType, out styleSettersForCurrentType))
                    {
                        styleSettersForCurrentType     = new Dictionary<UvssStyleKey, StyleSetter>();
                        styledPropertiesForCurrentType = new Dictionary<String, DependencyProperty>();

                        var styledDependencyProperties = 
                            from field in currentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                            let attr = field.GetCustomAttributes(typeof(StyledAttribute), false).SingleOrDefault()
                            let type = field.FieldType
                            let name = field.Name
                            where
                                attr != null &&
                                type == typeof(DependencyProperty)
                            select new { Attribute = (StyledAttribute)attr, FieldInfo = field };

                        foreach (var prop in styledDependencyProperties)
                        {
                            var dp                  = (DependencyProperty)prop.FieldInfo.GetValue(null);
                            var dpType              = dp.PropertyType;

                            var setStyledValue      = miSetStyledValue.MakeGenericMethod(dpType);

                            var expParameterElement = Expression.Parameter(typeof(UIElement), "element");
                            var expParameterValue   = Expression.Parameter(typeof(String), "value");
                            var expParameterFmtProv = Expression.Parameter(typeof(IFormatProvider), "provider");
                            var expResolveValue     = Expression.Convert(Expression.Call(miFromString, expParameterValue, Expression.Constant(dpType), expParameterFmtProv), dpType);
                            var expCallMethod       = Expression.Call(expParameterElement, setStyledValue, Expression.Constant(dp), expResolveValue);

                            var lambda = Expression.Lambda<StyleSetter>(expCallMethod, expParameterElement, expParameterValue, expParameterFmtProv).Compile();

                            var styleKey = new UvssStyleKey(prop.Attribute.Name, prop.Attribute.PseudoClass);
                            styleSettersForCurrentType[styleKey] = lambda;
                            styledPropertiesForCurrentType[prop.Attribute.Name] = dp;
                        }

                        styleSetters[currentType]     = styleSettersForCurrentType;
                        styledProperties[currentType] = styledPropertiesForCurrentType;
                    }

                    currentType = currentType.BaseType;
                }
            }
        }
        
        /// <summary>
        /// Occurs when the value of the <see cref="HitTestVisible"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleHitTestVisibleChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnHitTestVisibleChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Focusable"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleFocusableChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnFocusableChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Enabled"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleEnabledChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnEnabledChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Visibility"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleVisibilityChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnVisibilityChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Width"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleWidthChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnWidthChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MinWidth"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMinWidthChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnMinWidthChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MaxWidth"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMaxWidthChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnMaxWidthChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Height"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleHeightChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnHeightChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MinHeight"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMinHeightChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnMinHeightChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MaxHeight"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMaxHeightChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnMaxHeightChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Padding"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandlePaddingChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnPaddingChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Margin"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMarginChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnMarginChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalAlignment"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleHorizontalAlignmentChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnHorizontalAlignmentChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalAlignment"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleVerticalAlignmentChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnVerticalAlignmentChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Opacity"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleOpacityChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnOpacityChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="FontColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleFontColorChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnFontColorChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="FontAssetID"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleFontAssetIDChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.ReloadFont();
            element.OnFontAssetIDChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleBackgroundColorChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnBackgroundColorChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BackgroundImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleBackgroundImageChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.ReloadBackgroundImage();
            element.OnBackgroundImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="FocusedColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleFocusedColorChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.OnFocusedColorChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="FocusedImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleFocusedImageChanged(DependencyObject dobj)
        {
            var element = (UIElement)dobj;
            element.ReloadFocusedImage();
            element.OnFocusedImageChanged();
        }

        /// <summary>
        /// Retrieves a storyboard clock from the pool.
        /// </summary>
        /// <param name="storyboard">The storyboard which the clock will track.</param>
        /// <param name="clock">The storyboard clock that was retrieved from the pool.</param>
        private static void RetrieveStoryboardClock(Storyboard storyboard, out StoryboardClock clock)
        {
            clock = storyboardClockPool.Retrieve();
            clock.Storyboard = storyboard;
        }

        /// <summary>
        /// Releases a storyboard clock back into the pool.
        /// </summary>
        /// <param name="clock">The clock to release back into the pool.</param>
        private static void ReleaseStoryboardClock(StoryboardClock clock)
        {
            Contract.Require(clock, "clock");

            storyboardClockPool.Release(clock);
        }

        /// <summary>
        /// Finds the element registration context for this element.
        /// </summary>
        /// <returns>The element registration context for this element.</returns>
        private UIElementRegistry FindElementRegistry()
        {
            if (Control != null)
            {
                return control.ComponentRegistry;
            }
            return (view == null) ? null : view.ElementRegistry;
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
        /// Handles the <see cref="PerformedLayout"/> event raised by the element's content panel.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        private void HandleContentPanelPerformedLayout(UIElement element)
        {
            InvalidateArrange();
        }

        /// <summary>
        /// Finds the control associated with this element.
        /// </summary>
        /// <returns>The control associated with this element.</returns>
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

        // Property values.
        private readonly UltravioletContext uv;
        private readonly String id;
        private readonly String name;
        private readonly UIElementClassCollection classes;
        private readonly VisualStateGroupCollection visualStateGroups;
        private Object viewModel;
        private UIView view;
        private UIElement parent;
        private Control control;
        private Size2 desiredSize;
        private Boolean isMeasureValid;
        private Boolean isArrangeValid;
        private Boolean hovering;
        private Int32 absoluteParentOriginX;
        private Int32 absoluteParentOriginY;
        private Int32 actualWidth;
        private Int32 actualHeight;
        private SpriteFont font;

        private Int32 layoutDepth;
        private Size2 previousMeasureSize;
        private Rectangle previousArrangeRect;

        // State values.
        private UIElement contentPanel;
        private UIElementRegistry elementRegistrationContext;
        private Rectangle? previousScissorRectangle;
        private Boolean scissorApplied;
        private Rectangle finalRect;

        // Storyboard clocks.
        private static readonly IPool<StoryboardClock> storyboardClockPool = 
            new ExpandingPool<StoryboardClock>(64, () => new StoryboardClock());
        private readonly Dictionary<Storyboard, StoryboardClock> storyboardClocks = 
            new Dictionary<Storyboard, StoryboardClock>();

        // Functions for setting styles on known element types.
        private static readonly MethodInfo miFromString;
        private static readonly MethodInfo miSetStyledValue;
        private static readonly Object styleSyncObject = new Object();
        private static readonly Dictionary<Type, Dictionary<String, DependencyProperty>> styledProperties = 
            new Dictionary<Type, Dictionary<String, DependencyProperty>>();
        private static readonly Dictionary<Type, Dictionary<UvssStyleKey, StyleSetter>> styleSetters = 
            new Dictionary<Type, Dictionary<UvssStyleKey, StyleSetter>>();
    }
}
