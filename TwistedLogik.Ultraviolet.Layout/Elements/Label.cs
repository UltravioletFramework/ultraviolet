using System;
using System.ComponentModel;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.Layout.Stylesheets;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents a label on a user interface.
    /// </summary>
    [UIElement("Label")]
    [DefaultProperty("Text")]
    public class Label : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its layout.</param>
        public Label(UltravioletContext uv, String id)
            : base(uv, id)
        {
            var dpBackgroundColor = DependencyProperty.FindByName("BackgroundColor", typeof(UIElement));
            SetDefaultValue<Color>(dpBackgroundColor, Color.Transparent);
        }

        /// <summary>
        /// Gets or sets the label's text.
        /// </summary>
        public String Text
        {
            get { return GetValue<String>(dpText); }
            set { SetValue<String>(dpText, value); }
        }

        /// <summary>
        /// Gets or sets the label's text alignment flags.
        /// </summary>
        public TextFlags TextAlignment
        {
            get { return GetValue<TextFlags>(dpTextAlign); }
            set { SetValue<TextFlags>(dpTextAlign, value); }
        }

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            if (Font != null && textLayoutResult.Count > 0)
            {
                var position = new Vector2(AbsoluteScreenX, AbsoluteScreenY);
                UIElementResources.TextRenderer.Draw(spriteBatch, textLayoutResult, position, FontColor);
            }
            base.OnDrawing(time, spriteBatch);
        }

        /// <inheritdoc/>
        protected override void OnContainerRelativeLayoutChanged()
        {
            UpdateCachedTextLayout();
            base.OnContainerRelativeLayoutChanged();
        }

        /// <inheritdoc/>
        protected override void OnFontAssetIDChanged()
        {
            UpdateCachedTextLayout();
            base.OnFontAssetIDChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> property is changed.
        /// </summary>
        /// <param name="dobj">The dependency object for which the value was changed.</param>
        private static void HandleTextChanged(DependencyObject dobj) 
        {
            var element = (Label)dobj;
            element.UpdateCachedTextParse();
            
            if (element.Container != null)
            {
                element.Container.PerformLayout(element);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextAlignment"/> property is changed.
        /// </summary>
        /// <param name="dobj">The dependency object for which the value was changed.</param>
        private static void HandleTextAlignmentChanged(DependencyObject dobj)
        {
            var label = (Label)dobj;
            label.UpdateCachedTextLayout();
        }

        /// <summary>
        /// Updates the parsed text cache.
        /// </summary>
        private void UpdateCachedTextParse()
        {
            if (String.IsNullOrEmpty(Text))
            {
                textParserResult.Clear();
            }
            else
            {
                UIElementResources.TextRenderer.Parse(Text, textParserResult);
            }
            UpdateCachedTextLayout();
        }

        /// <summary>
        /// Updates the text layout cache.
        /// </summary>
        private void UpdateCachedTextLayout()
        {
            if (textParserResult.Count == 0)
            {
                textLayoutResult.Clear();
            }
            else
            {
                var settings = new TextLayoutSettings(Font, CalculatedWidth, CalculatedHeight, TextAlignment);
                UIElementResources.TextRenderer.CalculateLayout(textParserResult, textLayoutResult, settings);
            }
        }

        // Dependency properties.
        private static readonly DependencyProperty dpText = DependencyProperty.Register("Text", typeof(String), typeof(Label),
            new DependencyPropertyMetadata(HandleTextChanged, null, DependencyPropertyOptions.None));

        [Styled("text-align")]
        private static readonly DependencyProperty dpTextAlign = DependencyProperty.Register("TextAlignment", typeof(TextFlags), typeof(Label),
            new DependencyPropertyMetadata(null, () => TextFlags.AlignCenter | TextFlags.AlignMiddle, DependencyPropertyOptions.None));

        // State values.
        private readonly TextParserResult textParserResult = new TextParserResult();
        private readonly TextLayoutResult textLayoutResult = new TextLayoutResult();
    }
}
