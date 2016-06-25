using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the base class for buttons.
    /// </summary>
    [Preserve(AllMembers = true)]
    [UvmlKnownType]
    public abstract class ButtonBase : ContentControl
    {
        /// <summary>
        /// Initializes the <see cref="ButtonBase"/> type.
        /// </summary>
        static ButtonBase()
        {
            IsEnabledProperty.OverrideMetadata(typeof(ButtonBase), new PropertyMetadata<Boolean>(HandleIsEnabledChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ButtonBase(UltravioletContext uv, String name)
            : base(uv, name)
        {
            VisualStateGroups.Create("common", new[] { "normal", "hover", "pressed", "disabled" });
        }

        /// <summary>
        /// Gets a value indicating whether the button is being pressed.
        /// </summary>
        /// <value><see langword="true"/> if the button is being pressed; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsPressedProperty"/></dpropField>
        ///     <dpropStylingName>pressed</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsPressed
        {
            get { return GetValue<Boolean>(IsPressedProperty); }
            protected set { SetValue<Boolean>(IsPressedPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the button's click mode.
        /// </summary>
        /// <value>A <see cref="ClickMode"/> value specifying when the button's 
        /// <see cref="Click"/> event occurs.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ClickModeProperty"/></dpropField>
        ///     <dpropStylingName>click-mode</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public ClickMode ClickMode
        {
            get { return GetValue<ClickMode>(ClickModeProperty); }
            set { SetValue<ClickMode>(ClickModeProperty, value); }
        }

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="ClickEvent"/></revtField>
        ///     <revtStylingName>click</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        /// <summary>
        /// Occurs when the button is clicked as a result of user interaction.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="ClickByUserEvent"/></revtField>
        ///     <revtStylingName>click-by-user</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler ClickByUser
        {
            add { AddHandler(ClickByUserEvent, value); }
            remove { RemoveHandler(ClickByUserEvent, value); }
        }

        /// <summary>
        /// The private access key for the <see cref="IsPressed"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsPressedPropertyKey = DependencyProperty.RegisterReadOnly("IsPressed", typeof(Boolean), typeof(ButtonBase),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, HandleIsPressedChanged));

        /// <summary>
        /// Identifies the <see cref="IsPressed"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsPressed"/> dependency property.</value>
        public static readonly DependencyProperty IsPressedProperty = IsPressedPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="ClickMode"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ClickMode"/> dependency property.</value>
        public static readonly DependencyProperty ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(ButtonBase),
            new PropertyMetadata<ClickMode>(PresentationBoxedValues.ClickMode.Release));

        /// <summary>
        /// Identifies the <see cref="Click"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="Click"/> event.</value>
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(ButtonBase));

        /// <summary>
        /// Identifies the <see cref="ClickByUser"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="ClickByUser"/> event.</value>
        public static readonly RoutedEvent ClickByUserEvent = EventManager.RegisterRoutedEvent("ClickByUser", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(ButtonBase));

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            if (View != null && !View.IsInputEnabledAndAllowed)
            {
                IsPressed = false;
            }
            base.UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected override void OnIsMouseOverChanged()
        {
            UpdateCommonState();
            base.OnIsMouseOverChanged();
        }

        /// <inheritdoc/>
        protected override void OnLostMouseCapture(RoutedEventData data)
        {
            IsPressed = false;
            base.OnLostMouseCapture(data);
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            if (IsPressed)
            {
                IsPressed = false;
            }
            base.OnLostKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {
            if (ClickMode != ClickMode.Hover)
            {
                if (IsMouseCaptured && device.IsButtonDown(MouseButton.Left))
                {
                    var position = Mouse.GetPosition(this);
                    IsPressed = Bounds.Contains(position);
                }
            }
            base.OnMouseMove(device, x, y, dx, dy, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                HandlePressed();
                data.Handled = true;
            }
            base.OnMouseDown(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                HandleReleased();
                data.Handled = true;
            }
            base.OnMouseUp(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseEnter(MouseDevice device, RoutedEventData data)
        {
            if (ClickMode == ClickMode.Hover)
            {
                OnClick();
                OnClickByUser();
            }
            base.OnMouseEnter(device, data);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            if (key == Key.Return || key == Key.Space)
            {
                HandlePressed();
                data.Handled = true;
            }
            base.OnKeyDown(device, key, modifiers, data);
        }

        /// <inheritdoc/>
        protected override void OnKeyUp(KeyboardDevice device, Key key, RoutedEventData data)
        {
            if (key == Key.Return || key == Key.Space)
            {
                HandleReleased(false);
                data.Handled = true;
            }
            base.OnKeyUp(device, key, data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            if (GamePad.ConfirmButton == button && !repeat)
            {
                HandlePressed();
                data.Handled = true;
            }
            base.OnGamePadButtonDown(device, button, repeat, data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonUp(GamePadDevice device, GamePadButton button, RoutedEventData data)
        {
            if (GamePad.ConfirmButton == button)
            {
                HandleReleased(false);
                data.Handled = true;
            }
            base.OnGamePadButtonUp(device, button, data);
        }

        /// <summary>
        /// Raises the <see cref="Click"/> event.
        /// </summary>
        protected virtual void OnClick()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(ClickEvent);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Raises the <see cref="ClickByUser"/> event.
        /// </summary>
        protected virtual void OnClickByUser()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(ClickByUserEvent);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="UIElement.IsEnabledChanged"/> dependency property changes.
        /// </summary>
        private static void HandleIsEnabledChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var buttonBase = (ButtonBase)dobj;
            buttonBase.UpdateCommonState();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsPressed"/> dependency property changes.
        /// </summary>
        private static void HandleIsPressedChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var buttonBase = (ButtonBase)dobj;
            buttonBase.UpdateCommonState();
        }

        /// <summary>
        /// Modifies state and raises events relating to the button being pressed.
        /// </summary>
        private void HandlePressed()
        {
            Focus();
            CaptureMouse();

            IsPressed = true;

            if (ClickMode == ClickMode.Press)
            {
                OnClick();
                OnClickByUser();
            }
        }

        /// <summary>
        /// Modifies state and raises events relating to the button being released.
        /// </summary>
        /// <param name="checkMousePosition">A value indicating whether to confirm that the mouse is inside of the button before calling <see cref="OnClick()"/>.</param>
        private void HandleReleased(Boolean checkMousePosition = true)
        {
            var clicked = IsPressed;

            ReleaseMouseCapture();

            IsPressed = false;

            if (clicked && ClickMode == ClickMode.Release)
            {
                if (checkMousePosition)
                {
                    var position = Mouse.GetPosition(this);
                    if (Bounds.Contains(position))
                    {
                        OnClick();
                        OnClickByUser();
                    }
                }
                else
                {
                    OnClick();
                    OnClickByUser();
                }
            }
        }

        /// <summary>
        /// Transitions the button into the appropriate common state.
        /// </summary>
        private void UpdateCommonState()
        {
            if (IsEnabled)
            {
                if (IsPressed)
                {
                    VisualStateGroups.GoToState("common", "pressed");
                }
                else
                {
                    if (IsMouseOver)
                    {
                        VisualStateGroups.GoToState("common", "hover");
                    }
                    else
                    {
                        VisualStateGroups.GoToState("common", "normal");
                    }
                }
            }
            else
            {
                VisualStateGroups.GoToState("common", "disabled");
            }
        }
    }
}
