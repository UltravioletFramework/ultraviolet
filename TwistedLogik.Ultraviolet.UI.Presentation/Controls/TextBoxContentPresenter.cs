using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a content presenter for a <see cref="TextBox"/> control.
    /// </summary>
    [UvmlKnownType]
    public class TextBoxContentPresenter : ContentPresenter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxContentPresenter"/> control.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextBoxContentPresenter(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var size = base.MeasureOverride(availableSize);

            var font = Control.Font;
            if (font != null && font.IsLoaded)
            {
                var fontResource = font.Resource.Value.Regular;

                return new Size2D(
                    Math.Max(size.Width, fontResource.SpaceWidth),
                    Math.Max(size.Height, fontResource.LineSpacing));
            }

            return size;
        }
    }
}
