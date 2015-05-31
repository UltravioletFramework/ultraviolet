using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the base class for buttons.
    /// </summary>
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
            SetDefaultValue<Boolean>(FocusableProperty, true);

            VisualStateGroups.Create("common", new[] { "normal", "hover", "pressed", "disabled" });
        }

        /// <summary>
        /// Gets a value indicating whether the button is being pressed.
        /// </summary>
        public Boolean IsPressed
        {
            get { return GetValue<Boolean>(IsPressedProperty); }
            protected set { SetValue<Boolean>(IsPressedPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the button's click mode.
        /// </summary>
        public ClickMode ClickMode
        {
            get { return GetValue<ClickMode>(ClickModeProperty); }
            set { SetValue<ClickMode>(ClickModeProperty, value); }
        }

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        public event UpfRoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        /// <summary>
        /// The private access key for the <see cref="IsPressed"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsPressedPropertyKey = DependencyProperty.RegisterReadOnly("IsPressed", typeof(Boolean), typeof(ButtonBase),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, HandleIsPressedChanged));

        /// <summary>
        /// Identifies the <see cref="IsPressed"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'pressed'.</remarks>
        public static readonly DependencyProperty IsPressedProperty = IsPressedPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="ClickMode"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'click-mode'.</remarks>
        public static readonly DependencyProperty ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(ButtonBase),
            new PropertyMetadata<ClickMode>(PresentationBoxedValues.ClickMode.Release));

        /// <summary>
        /// Identifies the <see cref="Click"/> routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is 'click'.</remarks>
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(ButtonBase));

        /// <inheritdoc/>
        protected override void OnIsMouseOverChanged()
        {
            UpdateCommonState();
            base.OnIsMouseOverChanged();
        }

        /// <inheritdoc/>
        protected override void OnLostMouseCapture(ref RoutedEventData data)
        {
            IsPressed = false;
            base.OnLostMouseCapture(ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, ref RoutedEventData data)
        {
            if (ClickMode != ClickMode.Hover)
            {
                if (IsMouseCaptured && device.IsButtonDown(MouseButton.Left))
                {
                    IsPressed = AbsoluteBounds.Contains(x, y);
                }
            }
            base.OnMouseMove(device, x, y, dx, dy, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                Focus();
                CaptureMouse();

                IsPressed = true;

                if (ClickMode == ClickMode.Press)
                {
                    OnClick();
                }

                data.Handled = true;
            }
            base.OnMouseDown(device, button, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                var clicked = IsPressed;

                ReleaseMouseCapture();

                IsPressed = false;

                if (clicked && ClickMode == ClickMode.Release)
                {
                    var position = device.GetPositionInWindow(View.Window);
                    if (position != null && AbsoluteBounds.Contains(position.Value))
                    {
                        OnClick();
                    }
                }

                data.Handled = true;
            }
            base.OnMouseUp(device, button, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseEnter(MouseDevice device, ref RoutedEventData data)
        {
            if (ClickMode == ClickMode.Hover)
            {
                OnClick();
            }
            base.OnMouseEnter(device, ref data);
        }

        /// <summary>
        /// Raises the <see cref="Click"/> event.
        /// </summary>
        protected virtual void OnClick()
        {
            var evtData     = new RoutedEventData(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(ClickEvent);
            evtDelegate(this, ref evtData);
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
