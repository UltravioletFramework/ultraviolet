using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the base class for buttons.
    /// </summary>
    [UIElement("ButtonBase")]
    public abstract class ButtonBase : TextualElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ButtonBase(UltravioletContext uv, String id)
            : base(uv, id)
        {
            VisualStateGroups.Create("common", new[] { "normal", "hover", "pressed", "disabled" });
        }

        /// <summary>
        /// Gets a value indicating whether the button is in the "depressed" state.
        /// </summary>
        public Boolean Depressed
        {
            get { return depressed; }
            private set
            {
                if (depressed != value)
                {
                    depressed = value;
                    OnDepressedChanged();
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
        /// Occurs when the value of the <see cref="Depressed"/> property changes.
        /// </summary>
        public event UIElementEventHandler DepressedChanged;

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
        protected internal override void OnLostMouseCapture()
        {
            Depressed = false;
            base.OnLostMouseCapture();
        }

        /// <inheritdoc/>
        protected internal override void OnMouseButtonPressed(MouseDevice device, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                Depressed = true;
                OnButtonPressed();

                if (ClickMode == ClickMode.Press)
                {
                    OnClick();
                }
            }
            base.OnMouseButtonPressed(device, button);
        }

        /// <inheritdoc/>
        protected internal override void OnMouseButtonReleased(MouseDevice device, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                var clicked = Depressed;

                Depressed = false;
                OnButtonReleased();

                if (clicked && ClickMode == ClickMode.Release)
                {
                    var position = device.GetPositionInWindow(View.Window);
                    if (position != null && ScreenBounds.Contains(position.Value))
                    {
                        OnClick();
                    }
                }
            }
            base.OnMouseButtonReleased(device, button);
        }

        /// <inheritdoc/>
        protected internal override void OnMouseEnter(MouseDevice device)
        {
            if (ClickMode == ClickMode.Hover)
            {
                OnClick();
            }
            base.OnMouseEnter(device);
        }

        /// <inheritdoc/>
        protected internal override void OnMouseLeave(MouseDevice device)
        {
            if (!View.HasMouseCapture(this))
            {
                Depressed = false;
            }
            base.OnMouseLeave(device);
        }

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            DrawText(spriteBatch);

            base.OnDrawing(time, spriteBatch);
        }

        /// <inheritdoc/>
        protected override void OnEnabledChanged()
        {
            base.OnEnabledChanged();

            UpdateCommonState();
        }

        /// <inheritdoc/>
        protected override void OnHoveringChanged()
        {
            base.OnHoveringChanged();

            UpdateCommonState();
        }

        /// <summary>
        /// Raises the <see cref="DepressedChanged"/> event.
        /// </summary>
        protected virtual void OnDepressedChanged()
        {
            var temp = DepressedChanged;
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
        /// <param name="dobj">The object that raised the event.</param>
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
            if (Enabled)
            {
                if (Depressed)
                {
                    VisualStateGroups.GoToState("common", "pressed");
                }
                else
                {
                    if (Hovering)
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
