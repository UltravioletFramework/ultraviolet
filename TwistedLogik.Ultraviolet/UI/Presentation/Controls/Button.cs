using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UIElement("Button")]
    public class Button : TextualElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Button(UltravioletContext uv, String id)
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
        /// Occurs when the button is clicked.
        /// </summary>
        public event UIElementEventHandler Click;

        /// <summary>
        /// Occurs when the value of the <see cref="Depressed"/> property changes.
        /// </summary>
        public event UIElementEventHandler DepressedChanged;

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
                View.CaptureMouse(this);
                Depressed = true;
            }
            base.OnMouseButtonPressed(device, button);
        }

        /// <inheritdoc/>
        protected internal override void OnMouseButtonReleased(MouseDevice device, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                if (depressed)
                {
                    var position = device.GetPositionInWindow(View.Window);
                    if (position != null && ScreenBounds.Contains(position.Value))
                    {
                        OnClick();
                    }
                    View.ReleaseMouse(this);
                }
                Depressed = false;
            }
            base.OnMouseButtonReleased(device, button);
        }

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            DrawText(spriteBatch);

            base.OnDrawing(time, spriteBatch);
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
