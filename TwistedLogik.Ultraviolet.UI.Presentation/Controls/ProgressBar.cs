using System;
using System.ComponentModel;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a UI element which displays progress towards some goal.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.ProgressBar.xml")]
    [DefaultProperty("Value")]
    public class ProgressBar : RangeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ProgressBar(UltravioletContext uv, String id)
            : base(uv, id)
        {
            SetDefaultValue<Double>(MaximumProperty, 100.0);
        }

        /// <summary>
        /// Gets or sets the image used to draw the progress bar's background.
        /// </summary>
        public SourcedImage BarImage
        {
            get { return GetValue<SourcedImage>(BarImageProperty); }
            set { SetValue<SourcedImage>(BarImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the progress bar's background.
        /// </summary>
        public Color BarColor
        {
            get { return GetValue<Color>(BarColorProperty); }
            set { SetValue<Color>(BarColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the progress bar's fill.
        /// </summary>
        public SourcedImage FillImage
        {
            get { return GetValue<SourcedImage>(FillImageProperty); }
            set { SetValue<SourcedImage>(FillImageProperty, value); }
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
        public SourcedImage OverlayImage
        {
            get { return GetValue<SourcedImage>(OverlayImageProperty); }
            set { SetValue<SourcedImage>(OverlayImageProperty, value); }
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
        /// Occurs when the value of the <see cref="BarImage"/> property changes.
        /// </summary>
        public event UpfEventHandler BarImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BarColor"/> property changes.
        /// </summary>
        public event UpfEventHandler BarColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FillImage"/> property changes.
        /// </summary>
        public event UpfEventHandler FillImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="FillColor"/> property changes.
        /// </summary>
        public event UpfEventHandler FillColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="OverlayImage"/> property changes.
        /// </summary>
        public event UpfEventHandler OverlayImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="OverlayColor"/> property changes.
        /// </summary>
        public event UpfEventHandler OverlayColorChanged;

        /// <summary>
        /// Identifies the <see cref="BarImage"/> dependency property.
        /// </summary>
        [Styled("bar-image")]
        public static readonly DependencyProperty BarImageProperty = DependencyProperty.Register("BarImage", typeof(SourcedImage), typeof(ProgressBar),
            new DependencyPropertyMetadata(HandleBarImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="BarColor"/> dependency property.
        /// </summary>
        [Styled("bar-color")]
        public static readonly DependencyProperty BarColorProperty = DependencyProperty.Register("BarColor", typeof(Color), typeof(ProgressBar),
            new DependencyPropertyMetadata(HandleBarColorChanged, () => Color.White, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="FillImage"/> dependency property.
        /// </summary>
        [Styled("fill-image")]
        public static readonly DependencyProperty FillImageProperty = DependencyProperty.Register("FillImage", typeof(SourcedImage), typeof(ProgressBar),
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
        public static readonly DependencyProperty OverlayImageProperty = DependencyProperty.Register("OverlayImage", typeof(SourcedImage), typeof(ProgressBar),
            new DependencyPropertyMetadata(HandleOverlayImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="OverlayColor"/> dependency property.
        /// </summary>
        [Styled("overlay-color")]
        public static readonly DependencyProperty OverlayColorProperty = DependencyProperty.Register("OverlayColor", typeof(Color), typeof(ProgressBar),
            new DependencyPropertyMetadata(HandleOverlayColorChanged, () => Color.White, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override void ReloadContentCore(Boolean recursive)
        {
            ReloadBarImage();
            ReloadFillImage();
            ReloadOverlayImage();

            base.ReloadContentCore(recursive);
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            DrawBarImage(dc);
            DrawFillImage(dc);
            DrawOverlayImage(dc);

            base.DrawOverride(time, dc);
        }

        /// <summary>
        /// Draws the progress bar's background.
        /// </summary>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected virtual void DrawBarImage(DrawingContext dc)
        {
            DrawImage(dc, BarImage, BarColor);
        }

        /// <summary>
        /// Draws the progress bar's fill.
        /// </summary>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected virtual void DrawFillImage(DrawingContext dc)
        {
            var range         = (Maximum - Minimum);
            var percentFilled = (Value - Minimum) / range;
            if (percentFilled > 0)
            {
                var area = new RectangleD(Bounds.X, Bounds.Y,
                    Bounds.Width * percentFilled, Bounds.Height);

                DrawImage(dc, FillImage, area, FillColor);
            }
        }

        /// <summary>
        /// Draws the progress bar's overlay.
        /// </summary>
        /// <param name="dc">The drawing context that describes the render state of the layout.</param>
        protected virtual void DrawOverlayImage(DrawingContext dc)
        {
            DrawImage(dc, OverlayImage, OverlayColor);
        }

        /// <summary>
        /// Raises the <see cref="BarImageChanged"/> event.
        /// </summary>
        protected virtual void OnBarImageChanged()
        {
            var temp = BarImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="BarColorChanged"/> event.
        /// </summary>
        protected virtual void OnBarColorChanged()
        {
            var temp = BarColorChanged;
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
        /// Reloads the progress bar's background image.
        /// </summary>
        protected void ReloadBarImage()
        {
            LoadImage(BarImage);
        }

        /// <summary>
        /// Reloads the progress bar's fill image.
        /// </summary>
        protected void ReloadFillImage()
        {
            LoadImage(FillImage);
        }

        /// <summary>
        /// Reloads the progress bar's overlay image.
        /// </summary>
        protected void ReloadOverlayImage()
        {
            LoadImage(OverlayImage);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BarImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleBarImageChanged(DependencyObject dobj)
        {
            var element = (ProgressBar)dobj;
            element.ReloadBarImage();
            element.OnBarImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BarColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleBarColorChanged(DependencyObject dobj)
        {
            var element = (ProgressBar)dobj;
            element.OnBarColorChanged();
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
