using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a blank pane with a background image.
    /// </summary>
    [UIElement("Pane")]
    public class Pane : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pane"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Pane(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        internal override Boolean Draw(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            DrawBackgroundImage(spriteBatch, opacity);
            return base.Draw(time, spriteBatch, opacity);
        }
    }
}
