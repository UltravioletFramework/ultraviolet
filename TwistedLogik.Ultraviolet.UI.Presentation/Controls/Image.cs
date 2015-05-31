using System;
using System.ComponentModel;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a framework element which displays a particular image.
    /// </summary>
    [UvmlKnownType]
    [DefaultProperty("Source")]
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
        public SourcedImage Source
        {
            get { return GetValue<SourcedImage>(SourceProperty); }
            set { SetValue<SourcedImage>(SourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the image is drawn.
        /// </summary>
        public Color SourceColor
        {
            get { return GetValue<Color>(SourceColorProperty); }
            set { SetValue<Color>(SourceColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Source"/> property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'source-image'.</remarks>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", "source-image",
            typeof(SourcedImage), typeof(Image), new PropertyMetadata<SourcedImage>(HandleSourceChanged));
        
        /// <summary>
        /// Identifies the <see cref="SourceColor"/> property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'source-color'.</remarks>
        public static readonly DependencyProperty SourceColorProperty = DependencyProperty.Register("SourceColor", typeof(Color), typeof(Image),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));

        /// <inheritdoc/>
        protected override void ReloadContentCore(Boolean recursive)
        {
            ReloadSource();

            base.ReloadContentCore(recursive);
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
            LoadImage(Source);
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
