using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.Button.xml")]
    public class Button : ButtonBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Button(UltravioletContext uv, String id)
            : base(uv, id)
        {
            SetDefaultValue<HorizontalAlignment>(HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
            SetDefaultValue<VerticalAlignment>(VerticalContentAlignmentProperty, VerticalAlignment.Center);
        }

        /// <inheritdoc/>
        protected override void OnButtonPressed()
        {
            View.CaptureMouse(this);
            base.OnButtonPressed();
        }

        /// <inheritdoc/>
        protected override void OnButtonReleased()
        {
            View.ReleaseMouse(this);
            base.OnButtonReleased();
        }
    }
}
