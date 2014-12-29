using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a check box with binary states.
    /// </summary>
    [UIElement("CheckBox")]
    public class CheckBox : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public CheckBox(UltravioletContext uv, String id)
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

            var checkBoxSize    = (Int32)display.DipsToPixels(CheckBoxSize);
            var checkBoxPadding = (Int32)display.DipsToPixels(CheckBoxPadding);

            width  = (width ?? 0) + checkBoxSize + checkBoxPadding;
            height = Math.Max((height ?? 0), checkBoxSize);
        }

        /// <inheritdoc/>
        public override void ReloadContent()
        {
            ReloadCheckBoxImage();

            base.ReloadContent();
        }

        /// <summary>
        /// Gets or sets the element's check box image.
        /// </summary>
        public SourcedRef<StretchableImage9> CheckBoxImage
        {
            get { return GetValue<SourcedRef<StretchableImage9>>(CheckBoxImageProperty); }
            set { SetValue<SourcedRef<StretchableImage9>>(CheckBoxImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the element's check box image in device independent pixels (1/96 of an inch).
        /// </summary>
        public Double CheckBoxSize
        {
            get { return GetValue<Double>(CheckBoxSizeProperty); }
            set { SetValue<Double>(CheckBoxSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount of padding between the element's check box image and its text in device 
        /// independent pixels (1/96 of an inch).
        /// </summary>
        public Double CheckBoxPadding
        {
            get { return GetValue<Double>(CheckBoxPaddingProperty); }
            set { SetValue<Double>(CheckBoxPaddingProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler CheckBoxImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxSize"/> property changes.
        /// </summary>
        public event UIElementEventHandler CheckBoxSizeChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxPadding"/> property changes.
        /// </summary>
        public event UIElementEventHandler CheckBoxPaddingChanged;

        /// <summary>
        /// Identifies the CheckBoxImage dependency property.
        /// </summary>
        [Styled("checkbox-image")]
        public static readonly DependencyProperty CheckBoxImageProperty = DependencyProperty.Register("CheckBoxImage", typeof(SourcedRef<StretchableImage9>), typeof(CheckBox),
            new DependencyPropertyMetadata(HandleCheckBoxImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the CheckBoxSize dependency property.
        /// </summary>
        [Styled("checkbox-size")]
        public static readonly DependencyProperty CheckBoxSizeProperty = DependencyProperty.Register("CheckBoxSize", typeof(Double), typeof(CheckBox),
            new DependencyPropertyMetadata(HandleCheckBoxSizeChanged, () => 32.0, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the CheckBoxPadding dependency property.
        /// </summary>
        [Styled("checkbox-padding")]
        public static readonly DependencyProperty CheckBoxPaddingProperty = DependencyProperty.Register("CheckBoxPadding", typeof(Double), typeof(CheckBox),
            new DependencyPropertyMetadata(HandleCheckBoxPaddingChanged, () => 8.0, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            DrawCheckBoxImage(spriteBatch);
            DrawText(spriteBatch);

            //base.OnDrawing(time, spriteBatch);
        }

        /// <inheritdoc/>
        protected override void OnContainerRelativeLayoutChanged()
        {
            UpdateTextArea();
            base.OnContainerRelativeLayoutChanged();
        }

        /// <summary>
        /// Raises the <see cref="CheckBoxImageChanged"/> event.
        /// </summary>
        protected virtual void OnCheckBoxImageChanged()
        {
            var temp = CheckBoxImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="CheckBoxSizeChanged"/> event.
        /// </summary>
        protected virtual void OnCheckBoxSizeChanged()
        {
            UpdateCachedTextLayout();

            var temp = CheckBoxSizeChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="CheckBoxPaddingChanged"/> event.
        /// </summary>
        protected virtual void OnCheckBoxPaddingChanged()
        {
            UpdateCachedTextLayout();

            var temp = CheckBoxPaddingChanged;
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
        /// Reloads the element's check box image.
        /// </summary>
        protected void ReloadCheckBoxImage()
        {
            LoadContent(CheckBoxImage);
        }

        /// <summary>
        /// Draws the element's check box image.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        protected void DrawCheckBoxImage(SpriteBatch spriteBatch)
        {
            var checkBoxImage = CheckBoxImage.Value;
            if (checkBoxImage.IsLoaded)
            {
                var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
                var size    = (Int32)display.DipsToPixels(CheckBoxSize);

                var effects  = SpriteEffects.None;
                var origin   = new Vector2(size / 2f, size / 2f);
                var position = new Vector2(
                    AbsoluteScreenX + (size / 2f),
                    AbsoluteScreenY + (size / 2f));

                spriteBatch.DrawImage(checkBoxImage, position, size, size, Color.White, 0f, origin, effects, 0f);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCheckBoxImageChanged(DependencyObject dobj)
        {
            var element = (CheckBox)dobj;
            element.ReloadCheckBoxImage();
            element.OnCheckBoxImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxSize"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCheckBoxSizeChanged(DependencyObject dobj)
        {
            var element = (CheckBox)dobj;
            element.OnCheckBoxSizeChanged();
            element.UpdateTextArea();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxPadding"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCheckBoxPaddingChanged(DependencyObject dobj)
        {
            var element = (CheckBox)dobj;
            element.OnCheckBoxPaddingChanged();
            element.UpdateTextArea();
        }

        /// <summary>
        /// Updates the size of the area in which the element draws its text.
        /// </summary>
        private void UpdateTextArea()
        {
            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

            var checkBoxWidth   = (Int32)display.DipsToPixels(CheckBoxSize);
            var checkBoxPadding = (Int32)display.DipsToPixels(CheckBoxPadding);

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
