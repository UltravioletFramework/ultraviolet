using System;
using System.ComponentModel;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a UI element which is primarily concerned with rendering a single string of text.
    /// </summary>
    [DefaultProperty("Text")]
    public abstract class TextualElement : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextualElement"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's identifier.</param>
        public TextualElement(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Calculates the element's recommended size based on its content
        /// and the specified constraints.
        /// </summary>
        /// <param name="width">The element's recommended width.</param>
        /// <param name="height">The element's recommended height.</param>
        public override void CalculateRecommendedSize(ref Int32? width, ref Int32? height)
        {
            if (Font == null || String.IsNullOrEmpty(Text))
                return;

            var settings = new TextLayoutSettings(Font, width ?? Int32.MaxValue, height ?? Int32.MaxValue, TextAlignment);
            UIElementResources.TextRenderer.CalculateLayout(cachedParserResult, cachedLayoutResult, settings);

            if (width == null)
                width = (2 * Padding) + cachedLayoutResult.ActualWidth;

            if (height == null)
                height = (2 * Padding) + cachedLayoutResult.ActualHeight;
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
            get { return GetValue<TextFlags>(dpTextAlignment); }
            set { SetValue<TextFlags>(dpTextAlignment, value); }
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
        /// Draws the element's text.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        protected void DrawText(SpriteBatch spriteBatch)
        {
            if (cachedLayoutResult.Count > 0)
            {
                var position = new Vector2(AbsoluteScreenX + Padding, AbsoluteScreenY + Padding);
                UIElementResources.TextRenderer.Draw(spriteBatch, CachedLayoutResult, position, FontColor);
            }
        }

        /// <summary>
        /// Gets the cached result of parsing the element's text.
        /// </summary>
        protected TextParserResult CachedParserResult
        {
            get { return cachedParserResult; }
        }

        /// <summary>
        /// Gets the cached result of laying out the element's text.
        /// </summary>
        protected TextLayoutResult CachedLayoutResult
        {
            get { return cachedLayoutResult; }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> property is changed.
        /// </summary>
        /// <param name="dobj">The dependency object for which the value was changed.</param>
        private static void HandleTextChanged(DependencyObject dobj)
        {
            var element = (TextualElement)dobj;
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
            var label = (TextualElement)dobj;
            label.UpdateCachedTextLayout();
        }

        /// <summary>
        /// Updates the parsed text cache.
        /// </summary>
        private void UpdateCachedTextParse()
        {
            if (String.IsNullOrEmpty(Text))
            {
                cachedParserResult.Clear();
            }
            else
            {
                UIElementResources.TextRenderer.Parse(Text, cachedParserResult);
            }
            UpdateCachedTextLayout();
        }

        /// <summary>
        /// Updates the text layout cache.
        /// </summary>
        private void UpdateCachedTextLayout()
        {
            cachedLayoutResult.Clear();

            if (cachedParserResult.Count > 0 && Font != null)
            {
                var width    = CalculatedWidth - (2 * Padding);
                var height   = CalculatedHeight - (2 * Padding);
                var settings = new TextLayoutSettings(Font, width, height, TextAlignment);
                UIElementResources.TextRenderer.CalculateLayout(cachedParserResult, cachedLayoutResult, settings);
            }
        }

        // Dependency properties.
        private static readonly DependencyProperty dpText = DependencyProperty.Register("Text", typeof(String), typeof(TextualElement),
            new DependencyPropertyMetadata(HandleTextChanged, null, DependencyPropertyOptions.None));

        [Styled("text-align")]
        private static readonly DependencyProperty dpTextAlignment = DependencyProperty.Register("TextAlignment", typeof(TextFlags), typeof(TextualElement),
            new DependencyPropertyMetadata(HandleTextAlignmentChanged, () => TextFlags.AlignCenter | TextFlags.AlignMiddle, DependencyPropertyOptions.None));

        // State values.
        private readonly TextParserResult cachedParserResult = new TextParserResult();
        private readonly TextLayoutResult cachedLayoutResult = new TextLayoutResult();
    }
}
