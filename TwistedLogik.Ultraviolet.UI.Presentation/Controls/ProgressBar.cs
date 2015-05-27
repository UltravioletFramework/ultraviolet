using System;
using System.ComponentModel;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

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
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ProgressBar(UltravioletContext uv, String name)
            : base(uv, name)
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
        /// Identifies the <see cref="BarImage"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'bar-image'.</remarks>
        public static readonly DependencyProperty BarImageProperty = DependencyProperty.Register("BarImage", typeof(SourcedImage), typeof(ProgressBar),
            new PropertyMetadata<SourcedImage>(HandleBarImageChanged));

        /// <summary>
        /// Identifies the <see cref="BarColor"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'bar-color'.</remarks>
        public static readonly DependencyProperty BarColorProperty = DependencyProperty.Register("BarColor", typeof(Color), typeof(ProgressBar),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));

        /// <summary>
        /// Identifies the <see cref="FillImage"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'fill-image'.</remarks>
        public static readonly DependencyProperty FillImageProperty = DependencyProperty.Register("FillImage", typeof(SourcedImage), typeof(ProgressBar),
            new PropertyMetadata<SourcedImage>(HandleFillImageChanged));

        /// <summary>
        /// Identifies the <see cref="FillColor"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'fill-color'.</remarks>
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(Color), typeof(ProgressBar),
            new PropertyMetadata<Color>(Color.Lime));

        /// <summary>
        /// Identifies the <see cref="OverlayImage"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'overlay-image'.</remarks>
        public static readonly DependencyProperty OverlayImageProperty = DependencyProperty.Register("OverlayImage", typeof(SourcedImage), typeof(ProgressBar),
            new PropertyMetadata<SourcedImage>(HandleOverlayImageChanged));

        /// <summary>
        /// Identifies the <see cref="OverlayColor"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'overlay-color'.</remarks>
        public static readonly DependencyProperty OverlayColorProperty = DependencyProperty.Register("OverlayColor", typeof(Color), typeof(ProgressBar),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));

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
                var fillWidth  = Math.Min(ActualWidth, Math.Max(Bounds.Width * percentFilled, FillImage.Resource.MinimumRecommendedSize.Width));
                var fillHeight = Bounds.Height;

                var area = new RectangleD(Bounds.X, Bounds.Y, fillWidth, fillHeight);
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
        private static void HandleBarImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var element = (ProgressBar)dobj;
            element.ReloadBarImage();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="FillImage"/> dependency property changes.
        /// </summary>
        private static void HandleFillImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var element = (ProgressBar)dobj;
            element.ReloadFillImage();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="OverlayImage"/> dependency property changes.
        /// </summary>
        private static void HandleOverlayImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var element = (ProgressBar)dobj;
            element.ReloadOverlayImage();
        }
    }
}
