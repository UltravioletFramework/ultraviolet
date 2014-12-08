using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

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

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            if (Font != null && CachedLayoutResult.Count > 0)
            {
                var position = new Vector2(AbsoluteScreenX + Padding, AbsoluteScreenY + Padding);
                UIElementResources.TextRenderer.Draw(spriteBatch, CachedLayoutResult, position, FontColor);
            }
            base.OnDrawing(time, spriteBatch);
        }
    }
}
