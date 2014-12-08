using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents a label on a user interface.
    /// </summary>
    [UIElement("Label")]
    public class Label : TextualElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Label(UltravioletContext uv, String id)
            : base(uv, id)
        {
            var dpBackgroundColor = DependencyProperty.FindByName("BackgroundColor", typeof(UIElement));
            SetDefaultValue<Color>(dpBackgroundColor, Color.Transparent);
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
