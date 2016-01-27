using System;
using System.ComponentModel;
using TwistedLogik.Nucleus;
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
        /// Initializes the <see cref="ProgressBar"/> type.
        /// </summary>
        static ProgressBar()
        {
            FocusableProperty.OverrideMetadata(typeof(ProgressBar), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));
            MaximumProperty.OverrideMetadata(typeof(ProgressBar), new PropertyMetadata<Double>(100.0));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ProgressBar(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the image used to draw the progress bar's background.
        /// </summary>
        /// <value>A <see cref="SourcedImage"/> value which represents the image used
        /// to draw the progress bar's background. The default value is an invalid image.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="BarImageProperty"/></dpropField>
        ///     <dpropStylingName>bar-image</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SourcedImage BarImage
        {
            get { return GetValue<SourcedImage>(BarImageProperty); }
            set { SetValue(BarImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the progress bar's background.
        /// </summary>
        /// <value>A <see cref="Color"/> value which represents the color used
        /// to draw the progress bar's background. The default value is <see cref="Color.White"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="BarColorProperty"/></dpropField>
        ///     <dpropStylingName>bar-color</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color BarColor
        {
            get { return GetValue<Color>(BarColorProperty); }
            set { SetValue(BarColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to draw the progress bar's fill.
        /// </summary>
        /// <value>A <see cref="SourcedImage"/> value which represents the image used
        /// to draw the progress bar's fill. The default value is an invalid image.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="FillImageProperty"/></dpropField>
        ///     <dpropStylingName>fill-image</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SourcedImage FillImage
        {
            get { return GetValue<SourcedImage>(FillImageProperty); }
            set { SetValue(FillImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the progress bar's fill.
        /// </summary>
        /// <value>A <see cref="Color"/> value which represents the color used
        /// to draw the progress bar's fill. The default value is <see cref="Color.White"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="FillColorProperty"/></dpropField>
        ///     <dpropStylingName>fill-color</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color FillColor
        {
            get { return GetValue<Color>(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the progress bar's overlay image.
        /// </summary>
        /// <value>A <see cref="SourcedImage"/> value which represents the image used
        /// to draw the progress bar's overlay. The default value is an invalid image.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="OverlayImageProperty"/></dpropField>
        ///     <dpropStylingName>overlay-image</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SourcedImage OverlayImage
        {
            get { return GetValue<SourcedImage>(OverlayImageProperty); }
            set { SetValue(OverlayImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the progress bar's overlay.
        /// </summary>
        /// <value>A <see cref="Color"/> value which represents the color used
        /// to draw the progress bar's overlay. The default value is <see cref="Color.White"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="OverlayColorProperty"/></dpropField>
        ///     <dpropStylingName>overlay-color</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color OverlayColor
        {
            get { return GetValue<Color>(OverlayColorProperty); }
            set { SetValue(OverlayColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BarImage"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="BarImage"/> dependency property.</value>
        public static readonly DependencyProperty BarImageProperty = DependencyProperty.Register("BarImage", typeof(SourcedImage), typeof(ProgressBar),
            new PropertyMetadata<SourcedImage>(HandleBarImageChanged));

        /// <summary>
        /// Identifies the <see cref="BarColor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="BarColor"/> dependency property.</value>
        public static readonly DependencyProperty BarColorProperty = DependencyProperty.Register("BarColor", typeof(Color), typeof(ProgressBar),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));

        /// <summary>
        /// Identifies the <see cref="FillImage"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="FillImage"/> dependency property.</value>
        public static readonly DependencyProperty FillImageProperty = DependencyProperty.Register("FillImage", typeof(SourcedImage), typeof(ProgressBar),
            new PropertyMetadata<SourcedImage>(HandleFillImageChanged));

        /// <summary>
        /// Identifies the <see cref="FillColor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="FillColor"/> dependency property.</value>
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(Color), typeof(ProgressBar),
            new PropertyMetadata<Color>(Color.Lime));

        /// <summary>
        /// Identifies the <see cref="OverlayImage"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="OverlayImage"/> dependency property.</value>
        public static readonly DependencyProperty OverlayImageProperty = DependencyProperty.Register("OverlayImage", typeof(SourcedImage), typeof(ProgressBar),
            new PropertyMetadata<SourcedImage>(HandleOverlayImageChanged));

        /// <summary>
        /// Identifies the <see cref="OverlayColor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="OverlayColor"/> dependency property.</value>
        public static readonly DependencyProperty OverlayColorProperty = DependencyProperty.Register("OverlayColor", typeof(Color), typeof(ProgressBar),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));

        /// <inheritdoc/>
        protected override void ReloadContentOverride(Boolean recursive)
        {
            ReloadBarImage();
            ReloadFillImage();
            ReloadOverlayImage();

            base.ReloadContentOverride(recursive);
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
