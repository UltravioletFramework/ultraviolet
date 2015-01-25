using System;
using System.ComponentModel;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a framework element which displays a particular image.
    /// </summary>
    [UIElement("Image")]
    [DefaultProperty("Source")]
    public class Image : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public Image(UltravioletContext uv, String id)
            : base(uv, id)
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
        /// Occurs when the value of the <see cref="Source"/> property changes.
        /// </summary>
        public event UIElementEventHandler SourceChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SourceColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler SourceColorChanged;

        /// <summary>
        /// Identifies the <see cref="Source"/> property.
        /// </summary>
        [Styled("source-image")]
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(SourcedImage), typeof(Image),
            new DependencyPropertyMetadata(HandleSourceChanged, null, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="SourceColor"/> property.
        /// </summary>
        [Styled("source-color")]
        public static readonly DependencyProperty SourceColorProperty = DependencyProperty.Register("SourceColor", typeof(Color), typeof(Image),
            new DependencyPropertyMetadata(HandleSourceColorChanged, () => Color.White, DependencyPropertyOptions.None));

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
        /// Raises the <see cref="SourceChanged"/> event.
        /// </summary>
        protected virtual void OnSourceChanged()
        {
            var temp = SourceChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="SourceColorChanged"/> event.
        /// </summary>
        protected virtual void OnSourceColorChanged()
        {
            var temp = SourceColorChanged;
            if (temp != null)
            {
                temp(this);
            }
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
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleSourceChanged(DependencyObject dobj)
        {
            var image = (Image)dobj;
            image.ReloadSource();
            image.OnSourceChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SourceColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleSourceColorChanged(DependencyObject dobj)
        {
            var image = (Image)dobj;
            image.OnSourceColorChanged();
        }
    }
}
