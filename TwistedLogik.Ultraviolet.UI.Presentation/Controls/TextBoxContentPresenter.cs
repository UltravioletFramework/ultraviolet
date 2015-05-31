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
            var container = ContainingTextBox;
            if (container == null)
                return Size2D.Zero;

            var font = container.Font;
            if (font.IsLoaded)
            {
                var fontResource = font.Resource.Value.GetFace(container.FontStyle);

                return new Size2D(fontResource.SpaceWidth, fontResource.LineSpacing);
            }

            return Size2D.Zero;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            return finalSize;
        }

        /// <inheritdoc/>
        protected override void PositionOverride()
        {
            base.PositionOverride();

            var textBox = ContainingTextBox;
            if (textBox != null)
                textBox.InvalidateTextClip();
        }

        /// <summary>
        /// Gets the <see cref="TextBox"/> that contains this content presenter.
        /// </summary>
        protected TextBox ContainingTextBox
        {
            get { return TemplatedParent as TextBox; }
        }
    }
}
