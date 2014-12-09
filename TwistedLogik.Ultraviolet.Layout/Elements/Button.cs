using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.Layout.Elements
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

        }

        /// <summary>
        /// Gets a value indicating whether the button is in the "depressed" state.
        /// </summary>
        public Boolean Depressed
        {
            get { return depressed; }
        }

        /// <summary>
        /// Gets a value indicating whether the button is in both the "hovering" and "depressed" states.
        /// </summary>
        public Boolean HoveringDepressed
        {
            get { return Hovering && Depressed; }
        }

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        public event UIElementEventHandler Click;

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            DrawText(spriteBatch);

            base.OnDrawing(time, spriteBatch);
        }

        /// <inheritdoc/>
        protected internal override void OnMouseButtonPressed(MouseDevice device, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                depressed = true;
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
                    OnClick();
                }
                depressed = false;
            }
            base.OnMouseButtonReleased(device, button);
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

        // Property values.
        private Boolean depressed;
    }
}
