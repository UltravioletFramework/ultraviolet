using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a radio button.
    /// </summary>
    [UIElement("RadioButton")]
    public class RadioButton : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public RadioButton(UltravioletContext uv, String id)
            : base(uv, id)
        {
            SetDefaultValue<Color>(BackgroundColorProperty, Color.Transparent);
            SetDefaultValue<TextFlags>(TextAlignmentProperty, TextFlags.AlignLeft | TextFlags.AlignMiddle);
        }

        /// <inheritdoc/>
        public override void CalculateContentSize(ref Int32? width, ref Int32? height)
        {
            base.CalculateContentSize(ref width, ref height);

            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

            var checkBoxSize    = (Int32)display.DipsToPixels(RadioButtonSize);
            var checkBoxPadding = (Int32)display.DipsToPixels(RadioButtonPadding);

            width  = (width ?? 0) + checkBoxSize + checkBoxPadding;
            height = Math.Max((height ?? 0), checkBoxSize);
        }

        /// <inheritdoc/>
        public override void ReloadContent()
        {
            ReloadRadioButtonImage();

            base.ReloadContent();
        }

        /// <summary>
        /// Gets or sets the element's radio button image.
        /// </summary>
        public SourcedRef<StaticImage> RadioButtonImage
        {
            get { return GetValue<SourcedRef<StaticImage>>(RadioButtonImageProperty); }
            set { SetValue<SourcedRef<StaticImage>>(RadioButtonImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the element's radio button image in device independent pixels (1/96 of an inch).
        /// </summary>
        public Double RadioButtonSize
        {
            get { return GetValue<Double>(RadioButtonSizeProperty); }
            set { SetValue<Double>(RadioButtonSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount of padding between the element's radio button image and its text in device 
        /// independent pixels (1/96 of an inch).
        /// </summary>
        public Double RadioButtonPadding
        {
            get { return GetValue<Double>(RadioButtonPaddingProperty); }
            set { SetValue<Double>(RadioButtonPaddingProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler RadioButtonImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonSize"/> property changes.
        /// </summary>
        public event UIElementEventHandler RadioButtonSizeChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonPadding"/> property changes.
        /// </summary>
        public event UIElementEventHandler RadioButtonPaddingChanged;

        /// <summary>
        /// Identifies the RadioButtonImage dependency property.
        /// </summary>
        [Styled("radiobutton-image")]
        public static readonly DependencyProperty RadioButtonImageProperty = DependencyProperty.Register("RadioButtonImage", typeof(SourcedRef<StaticImage>), typeof(RadioButton),
            new DependencyPropertyMetadata(HandleRadioButtonImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the RadioButtonSize dependency property.
        /// </summary>
        [Styled("radiobutton-size")]
        public static readonly DependencyProperty RadioButtonSizeProperty = DependencyProperty.Register("RadioButtonSize", typeof(Double), typeof(RadioButton),
            new DependencyPropertyMetadata(HandleRadioButtonSizeChanged, () => 32.0, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the RadioButtonPadding dependency property.
        /// </summary>
        [Styled("radiobutton-padding")]
        public static readonly DependencyProperty RadioButtonPaddingProperty = DependencyProperty.Register("RadioButtonPadding", typeof(Double), typeof(RadioButton),
            new DependencyPropertyMetadata(HandleRadioButtonPaddingChanged, () => 8.0, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            base.OnDrawing(time, spriteBatch);

            DrawRadioButtonImage(spriteBatch);
        }

        /// <inheritdoc/>
        protected override void OnContainerRelativeLayoutChanged()
        {
            UpdateTextArea();
            base.OnContainerRelativeLayoutChanged();
        }

        /// <inheritdoc/>
        protected override void OnCheckedChanged()
        {
            if (Parent != null && Checked)
            {
                var count = Parent.ContentElementCount;
                for (int i = 0; i < count; i++)
                {
                    var sibling = Parent.GetContentElement(i);
                    if (sibling == this)
                        continue;

                    if (sibling is RadioButton)
                    {
                        ((RadioButton)sibling).Checked = false;
                    }
                }
            }
            base.OnCheckedChanged();
        }

        /// <inheritdoc/>
        protected override void ToggleChecked()
        {
            if (!Checked)
            {
                Checked = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="RadioButtonImageChanged"/> event.
        /// </summary>
        protected virtual void OnRadioButtonImageChanged()
        {
            var temp = RadioButtonImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="RadioButtonSizeChanged"/> event.
        /// </summary>
        protected virtual void OnRadioButtonSizeChanged()
        {
            UpdateCachedTextLayout();

            var temp = RadioButtonSizeChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="RadioButtonPaddingChanged"/> event.
        /// </summary>
        protected virtual void OnRadioButtonPaddingChanged()
        {
            UpdateCachedTextLayout();

            var temp = RadioButtonPaddingChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <inheritdoc/>
        protected override Int32 TextAreaX
        {
            get { return textAreaX; }
        }

        /// <inheritdoc/>
        protected override Int32 TextAreaY
        {
            get { return textAreaY; }
        }

        /// <inheritdoc/>
        protected override Int32 TextAreaWidth
        {
            get { return textAreaWidth; }
        }

        /// <inheritdoc/>
        protected override Int32 TextAreaHeight
        {
            get { return textAreaHeight; }
        }

        /// <summary>
        /// Reloads the element's radio button image.
        /// </summary>
        protected void ReloadRadioButtonImage()
        {
            LoadContent(RadioButtonImage);
        }

        /// <summary>
        /// Draws the element's radio button image.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        protected void DrawRadioButtonImage(SpriteBatch spriteBatch)
        {
            var checkBoxImage = RadioButtonImage.Value;
            if (checkBoxImage.IsLoaded)
            {
                var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
                var size    = (Int32)display.DipsToPixels(RadioButtonSize);

                var effects  = SpriteEffects.None;
                var origin   = new Vector2(size / 2f, size / 2f);
                var position = new Vector2(
                    AbsoluteScreenX + (size / 2f),
                    AbsoluteScreenY + (size / 2f));

                spriteBatch.DrawImage(checkBoxImage, position, size, size, Color.White, 0f, origin, effects, 0f);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleRadioButtonImageChanged(DependencyObject dobj)
        {
            var element = (RadioButton)dobj;
            element.ReloadRadioButtonImage();
            element.OnRadioButtonImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonSize"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleRadioButtonSizeChanged(DependencyObject dobj)
        {
            var element = (RadioButton)dobj;
            element.OnRadioButtonSizeChanged();
            element.UpdateTextArea();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonPadding"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleRadioButtonPaddingChanged(DependencyObject dobj)
        {
            var element = (RadioButton)dobj;
            element.OnRadioButtonPaddingChanged();
            element.UpdateTextArea();
        }

        /// <summary>
        /// Updates the size of the area in which the element draws its text.
        /// </summary>
        private void UpdateTextArea()
        {
            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

            var checkBoxWidth   = (Int32)display.DipsToPixels(RadioButtonSize);
            var checkBoxPadding = (Int32)display.DipsToPixels(RadioButtonPadding);

            textAreaX      = checkBoxWidth + checkBoxPadding;
            textAreaY      = 0;
            textAreaWidth  = ActualWidth - (checkBoxWidth + checkBoxPadding);
            textAreaHeight = ActualHeight;
        }

        // Property values.
        private Int32 textAreaX;
        private Int32 textAreaY;
        private Int32 textAreaWidth;
        private Int32 textAreaHeight;
    }
}
