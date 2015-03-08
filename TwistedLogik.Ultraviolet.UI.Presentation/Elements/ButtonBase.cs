using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the base class for buttons.
    /// </summary>
    [UvmlKnownType]
    public abstract class ButtonBase : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ButtonBase(UltravioletContext uv, String id)
            : base(uv, id)
        {
            SetDefaultValue<Boolean>(FocusableProperty, true);

            VisualStateGroups.Create("common", new[] { "normal", "hover", "pressed", "disabled" });
        }

        /// <summary>
        /// Gets a value indicating whether the button is in the "depressed" state.
        /// </summary>
        public Boolean IsDepressed
        {
            get { return depressed; }
            private set
            {
                if (depressed != value)
                {
                    depressed = value;
                    OnIsDepressedChanged();
                }
            }
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
        /// Occurs when the value of the <see cref="IsDepressed"/> property changes.
        /// </summary>
        public event UIElementEventHandler IsDepressedChanged;

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        public event UIElementEventHandler Click;

        /// <summary>
        /// Occurs when the button is pressed.
        /// </summary>
        public event UIElementEventHandler ButtonPressed;

        /// <summary>
        /// Occurs when the button is released.
        /// </summary>
        public event UIElementEventHandler ButtonReleased;

        /// <summary>
        /// Occurs when the value of the <see cref="ClickMode"/> property changes.
        /// </summary>
        public event UIElementEventHandler ClickModeChanged;

        /// <summary>
        /// Identifies the <see cref="ClickMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(ButtonBase),
            new DependencyPropertyMetadata(HandleClickModeChanged, () => ClickMode.Release, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override void OnLostMouseCapture(ref Boolean handled)
        {
            IsDepressed = false;
            base.OnLostMouseCapture(ref handled);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, ref Boolean handled)
        {
            if (button == MouseButton.Left)
            {
                IsDepressed = true;
                OnButtonPressed();

                if (ClickMode == ClickMode.Press)
                {
                    OnClick();
                }

                handled = true;
            }
            base.OnMouseDown(device, button, ref handled);
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseDevice device, MouseButton button, ref Boolean handled)
        {
            if (button == MouseButton.Left)
            {
                var clicked = IsDepressed;

                IsDepressed = false;
                OnButtonReleased();

                if (clicked && ClickMode == ClickMode.Release)
                {
                    var position = device.GetPositionInWindow(View.Window);
                    if (position != null && AbsoluteBounds.Contains(position.Value))
                    {
                        OnClick();
                    }
                }

                handled = true;
            }
            base.OnMouseUp(device, button, ref handled);
        }

        /// <inheritdoc/>
        protected override void OnMouseEnter(MouseDevice device, ref Boolean handled)
        {
            if (ClickMode == ClickMode.Hover)
            {
                OnClick();
            }
            base.OnMouseEnter(device, ref handled);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(MouseDevice device, ref Boolean handled)
        {
            if (View.ElementWithMouseCapture != this)
            {
                IsDepressed = false;
            }
            base.OnMouseLeave(device, ref handled);
        }

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            UpdateContent(time);

            base.UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected override void OnIsEnabledChanged()
        {
            base.OnIsEnabledChanged();

            UpdateCommonState();
        }

        /// <inheritdoc/>
        protected override void OnIsHoveringChanged()
        {
            base.OnIsHoveringChanged();

            UpdateCommonState();
        }

        /// <summary>
        /// Raises the <see cref="IsDepressedChanged"/> event.
        /// </summary>
        protected virtual void OnIsDepressedChanged()
        {
            var temp = IsDepressedChanged;
            if (temp != null)
            {
                temp(this);
            }

            UpdateCommonState();
        }

        /// <summary>
        /// Raises the <see cref="Click"/> event.
        /// </summary>
        protected virtual void OnClick()
        {
            var temp = Click;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ButtonPressed"/> event.
        /// </summary>
        protected virtual void OnButtonPressed()
        {
            var temp = ButtonPressed;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ButtonReleased"/> event.
        /// </summary>
        protected virtual void OnButtonReleased()
        {
            var temp = ButtonReleased;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ClickModeChanged"/> property.
        /// </summary>
        protected virtual void OnClickModeChanged()
        {
            var temp = ClickModeChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ClickMode"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleClickModeChanged(DependencyObject dobj)
        {
            var button = (ButtonBase)dobj;
            button.OnClickModeChanged();
        }

        /// <summary>
        /// Transitions the button into the appropriate common state.
        /// </summary>
        private void UpdateCommonState()
        {
            if (IsEnabled)
            {
                if (IsDepressed)
                {
                    VisualStateGroups.GoToState("common", "pressed");
                }
                else
                {
                    if (IsHovering)
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

        // Property values.
        private Boolean depressed;
    }
}
