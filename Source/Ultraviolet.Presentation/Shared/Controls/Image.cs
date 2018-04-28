using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a framework element which displays a particular image.
    /// </summary>
    [UvmlKnownType]
    [UvmlDefaultProperty("Source")]
    public class Image : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Image(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>A <see cref="SourcedImage"/> which represents the image that is drawn
        /// by this control. The default value is an invalid image.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="SourceProperty"/></dpropField>
        ///     <dpropStylingName>source-image</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SourcedImage Source
        {
            get { return GetValue<SourcedImage>(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the image is drawn.
        /// </summary>
        /// <value>A <see cref="Color"/> value that specifies the color with which
        /// the control draws its image. The default value is <see cref="Color.White"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="SourceColorProperty"/></dpropField>
        ///     <dpropStylingName>source-color</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color SourceColor
        {
            get { return GetValue<Color>(SourceColorProperty); }
            set { SetValue(SourceColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Source"/> property.
        /// </summary>
        /// <value>The identifier for the <see cref="Source"/> dependency property.</value>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", "source-image",
            typeof(SourcedImage), typeof(Image), new PropertyMetadata<SourcedImage>(HandleSourceChanged));
        
        /// <summary>
        /// Identifies the <see cref="SourceColor"/> property.
        /// </summary>
        /// <value>The identifier for the <see cref="SourceColor"/> property.</value>
        public static readonly DependencyProperty SourceColorProperty = DependencyProperty.Register("SourceColor", typeof(Color), typeof(Image),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));

        /// <inheritdoc/>
        protected override void ReloadContentOverride(Boolean recursive)
        {
            ReloadSource();

            base.ReloadContentOverride(recursive);
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            DrawImage(dc, Source, SourceColor);
            base.DrawOverride(time, dc);
        }

        /// <summary>
        /// Reloads the element's source image.
        /// </summary>
        protected void ReloadSource()
        {
            LoadResource(Source);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Source"/> dependency property changes.
        /// </summary>
        private static void HandleSourceChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var image = (Image)dobj;
            image.ReloadSource();
        }
    }
}
