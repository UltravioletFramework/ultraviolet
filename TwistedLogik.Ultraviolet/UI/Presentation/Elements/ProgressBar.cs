using System;
using System.ComponentModel;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a UI element which displays progress towards some goal.
    /// </summary>
    [UIElement("ProgressBar")]
    [DefaultProperty("Value")]
    public class ProgressBar : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ProgressBar(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        public override void ReloadContent()
        {
            ReloadFillImage();

            base.ReloadContent();
        }

        /// <summary>
        /// Gets or sets the current value of the progress bar.
        /// </summary>
        public Double Value
        {
            get 
            {
                var value = GetValue<Double>(ValueProperty);
                var min = Minimum;
                var max = Maximum;

                if (value < min) return min;
                if (value > max) return max;

                return value;
            }
            set { SetValue<Double>(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the smallest possible value of the progress bar.
        /// </summary>
        public Double Minimum
        {
            get { return GetValue<Double>(MinimumProperty); }
            set { SetValue<Double>(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the largest possible value of the progress bar.
        /// </summary>
        public Double Maximum
        {
            get { return GetValue<Double>(MaximumProperty); }
            set { SetValue<Double>(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the progress bar's fill.
        /// </summary>
        public SourcedRef<Image> FillImage
        {
            get { return GetValue<SourcedRef<Image>>(FillImageProperty); }
            set { SetValue<SourcedRef<Image>>(FillImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the progress bar's fill.
        /// </summary>
        public Color FillColor
        {
            get { return GetValue<Color>(FillColorProperty); }
            set { SetValue<Color>(FillColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the progress bar's overlay image.
        /// </summary>
        public SourcedRef<Image> OverlayImage
        {
            get { return GetValue<SourcedRef<Image>>(OverlayImageProperty); }
            set { SetValue<SourcedRef<Image>>(OverlayImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the progress bar's overlay.
        /// </summary>
        public Color OverlayColor
        {
            get { return GetValue<Color>(OverlayColorProperty); }
            set { SetValue<Color>(OverlayColorProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> property changes.
        /// </summary>
        public event UIElementEventHandler ValueChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Minimum"/> property changes.
        /// </summary>
        public event UIElementEventHandler MinimumChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Maximum"/> property changes.
        /// </summary>
        public event UIElementEventHandler MaximumChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FillImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler FillImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FillColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler FillColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="OverlayImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler OverlayImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="OverlayColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler OverlayColorChanged;

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Double), typeof(ProgressBar),
            new DependencyPropertyMetadata(HandleValueChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(Double), typeof(ProgressBar),
            new DependencyPropertyMetadata(HandleMinimumChanged, () => 0.0, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(Double), typeof(ProgressBar),
            new DependencyPropertyMetadata(HandleMaximumChanged, () => 100.0, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="FillImage"/> dependency property.
        /// </summary>
        [Styled("fill-image")]
        public static readonly DependencyProperty FillImageProperty = DependencyProperty.Register("FillImage", typeof(SourcedRef<Image>), typeof(ProgressBar),
            new DependencyPropertyMetadata(HandleFillImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="FillColor"/> dependency property.
        /// </summary>
        [Styled("fill-color")]
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(Color), typeof(ProgressBar),
            new DependencyPropertyMetadata(HandleFillColorChanged, () => Color.Lime, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="OverlayImage"/> dependency property.
        /// </summary>
        [Styled("overlay-image")]
        public static readonly DependencyProperty OverlayImageProperty = DependencyProperty.Register("OverlayImage", typeof(SourcedRef<Image>), typeof(ProgressBar),
            new DependencyPropertyMetadata(HandleOverlayImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="OverlayColor"/> dependency property.
        /// </summary>
        [Styled("overlay-color")]
        public static readonly DependencyProperty OverlayColorProperty = DependencyProperty.Register("OverlayColor", typeof(Color), typeof(ProgressBar),
            new DependencyPropertyMetadata(HandleOverlayColorChanged, () => Color.White, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch)
        {
            DrawBackgroundImage(spriteBatch);
            DrawFill(spriteBatch);
            DrawOverlay(spriteBatch);

            base.OnDrawing(time, spriteBatch);
        }

        /// <summary>
        /// Draws the progress bar's fill.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the progress bar's fill.</param>
        protected void DrawFill(SpriteBatch spriteBatch)
        {
            var img      = FillImage.Value;
            var imgColor = FillColor;

            if (imgColor.Equals(Color.Transparent) || img == null || !img.IsLoaded)
                return;

            var range         = (Maximum - Minimum);
            var percentFilled = (Value - Minimum) / range;
            if (percentFilled > 0)
            {
                var fillPosition = new Vector2(AbsoluteScreenX, AbsoluteScreenY);
                var fillWidth = (Int32)(ActualWidth * percentFilled);
                var fillHeight = ActualHeight;
                spriteBatch.DrawImage(img, fillPosition, fillWidth, fillHeight, imgColor);
            }
        }

        /// <summary>
        /// Draws the progress bar's overlay.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the progress bar's overlay.</param>
        protected void DrawOverlay(SpriteBatch spriteBatch)
        {
            var img      = OverlayImage.Value;
            var imgColor = OverlayColor;

            if (imgColor.Equals(Color.Transparent) || img == null || !img.IsLoaded)
                return;

            var overlayPosition = new Vector2(AbsoluteScreenX, AbsoluteScreenY);
            spriteBatch.DrawImage(img, overlayPosition, ActualWidth, ActualHeight, imgColor);
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            var temp = ValueChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MinimumChanged"/> event.
        /// </summary>
        protected virtual void OnMinimumChanged()
        {
            var temp = MinimumChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MaximumChanged"/> event.
        /// </summary>
        protected virtual void OnMaximumChanged()
        {
            var temp = MaximumChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="FillImageChanged"/> event.
        /// </summary>
        protected virtual void OnFillImageChanged()
        {
            var temp = FillImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="FillColorChanged"/> event.
        /// </summary>
        protected virtual void OnFillColorChanged()
        {
            var temp = FillColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="OverlayImageChanged"/> event.
        /// </summary>
        protected virtual void OnOverlayImageChanged()
        {
            var temp = OverlayImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="OverlayColorChanged"/> event.
        /// </summary>
        protected virtual void OnOverlayColorChanged()
        {
            var temp = OverlayColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Reloads the progress bar's fill image.
        /// </summary>
        protected void ReloadFillImage()
        {
            LoadContent(FillImage);
        }

        /// <summary>
        /// Reloads the progress bar's overlay image.
        /// </summary>
        protected void ReloadOverlayImage()
        {
            LoadContent(OverlayImage);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleValueChanged(DependencyObject dobj)
        {
            var element = (ProgressBar)dobj;
            element.OnValueChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Minimum"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMinimumChanged(DependencyObject dobj)
        {
            var element = (ProgressBar)dobj;
            element.OnMinimumChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Maximum"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMaximumChanged(DependencyObject dobj)
        {
            var element = (ProgressBar)dobj;
            element.OnMaximumChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="FillImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleFillImageChanged(DependencyObject dobj)
        {
            var element = (ProgressBar)dobj;
            element.ReloadFillImage();
            element.OnFillImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="FillColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleFillColorChanged(DependencyObject dobj)
        {
            var element = (ProgressBar)dobj;
            element.OnFillColorChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="OverlayImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleOverlayImageChanged(DependencyObject dobj)
        {
            var element = (ProgressBar)dobj;
            element.ReloadOverlayImage();
            element.OnOverlayImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="OverlayColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleOverlayColorChanged(DependencyObject dobj)
        {
            var element = (ProgressBar)dobj;
            element.OnOverlayColorChanged();
        }
    }
}
