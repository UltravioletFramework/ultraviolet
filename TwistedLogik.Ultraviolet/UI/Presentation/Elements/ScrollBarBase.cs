using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the base class for scroll bars.
    /// </summary>
    public abstract class ScrollBarBase : RangeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBarBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ScrollBarBase(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        public override void ReloadContent()
        {
            ReloadTrackImage();

            base.ReloadContent();
        }

        /// <summary>
        /// Gets or sets the amount of scrollable content that is currently visible.
        /// </summary>
        public Double ViewportSize
        {
            get { return GetValue<Double>(ViewportSizeProperty); }
            set { SetValue<Double>(ViewportSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the scroll bar's track is drawn.
        /// </summary>
        public Color TrackColor
        {
            get { return GetValue<Color>(TrackColorProperty); }
            set { SetValue<Color>(TrackColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image used to render the scroll bar's track.
        /// </summary>
        public SourcedRef<Image> TrackImage
        {
            get { return GetValue<SourcedRef<Image>>(TrackImageProperty); }
            set { SetValue<SourcedRef<Image>>(TrackImageProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ViewportSize"/> property changes.
        /// </summary>
        public event UIElementEventHandler ViewportSizeChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TrackColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler TrackColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TrackImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler TrackImageChanged;

        /// <summary>
        /// Identifies the <see cref="ViewportSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register("ViewportSize", typeof(Double), typeof(ScrollBarBase),
            new DependencyPropertyMetadata(HandleViewportSizeChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="TrackColor"/> dependency property.
        /// </summary>
        [Styled("track-color")]
        public static readonly DependencyProperty TrackColorProperty = DependencyProperty.Register("TrackColor", typeof(Color), typeof(ScrollBarBase),
            new DependencyPropertyMetadata(HandleTrackColorChanged, () => Color.White, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="TrackImage"/> dependency property.
        /// </summary>
        [Styled("track-image")]
        public static readonly DependencyProperty TrackImageProperty = DependencyProperty.Register("TrackImage", typeof(SourcedRef<Image>), typeof(ScrollBarBase),
            new DependencyPropertyMetadata(HandleTrackImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Updates the layout of the scroll bar's components.
        /// </summary>
        protected abstract void UpdateComponentLayout();

        /// <inheritdoc/>
        protected override void OnParentRelativeLayoutChanged()
        {
            UpdateComponentLayout();
            base.OnParentRelativeLayoutChanged();
        }

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            DrawTrackImage(spriteBatch, opacity);

            base.OnDrawing(time, spriteBatch, opacity);
        }

        /// <inheritdoc/>
        protected override void OnValueChanged()
        {
            UpdateComponentLayout();
            base.OnValueChanged();
        }

        /// <summary>
        /// Draws the scroll bar's track image.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected virtual void DrawTrackImage(SpriteBatch spriteBatch, Single opacity)
        {
            var area = new Rectangle(ActualTrackOffsetX, ActualTrackOffsetY, ActualTrackWidth, ActualTrackHeight);
            DrawElementImage(spriteBatch, TrackImage, area, TrackColor * Opacity * opacity);
        }

        /// <summary>
        /// Raises the <see cref="ViewportSize"/> event.
        /// </summary>
        protected virtual void OnViewportSizeChanged()
        {
            var temp = ViewportSizeChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="TrackColorChanged"/> event.
        /// </summary>
        protected virtual void OnTrackColorChanged()
        {
            var temp = TrackColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="TrackImageChanged"/> event.
        /// </summary>
        protected virtual void OnTrackImageChanged()
        {
            var temp = TrackImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Reloads the scroll bar's track image.
        /// </summary>
        protected void ReloadTrackImage()
        {
            LoadContent(TrackImage);
        }

        /// <summary>
        /// Calculates the scroll thumb's offset.
        /// </summary>
        /// <returns>The scroll thumb's current offset.</returns>
        protected GridLength CalculateThumbOffset()
        {
            var available = ActualTrackLength - ActualThumbLength;
            var display   = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var percent   = (Value - Minimum) / (Maximum - Minimum);
            var used      = display.PixelsToDips(available * percent);

            return new GridLength(used);
        }

        /// <summary>
        /// Updates the length of the scroll bar's thumb.
        /// </summary>
        /// <param name="minimumLength">The minimum length of the thumb.</param>
        /// <returns>The calculated thumb length.</returns>
        protected GridLength CalculateThumbLength(Double minimumLength)
        {
            var pxLengthMin   = (Int32)ConvertMeasureToPixels(minimumLength, 0);
            var pxLengthThumb = (Int32)((ViewportSize / (Maximum - Minimum + ViewportSize)) * ActualTrackLength);
            if (pxLengthThumb < pxLengthMin)
            {
                pxLengthThumb = pxLengthMin;
            }

            return new GridLength(pxLengthThumb);
        }

        /// <summary>
        /// Converts a range value to a pixel offset into the scroll bar's track.
        /// </summary>
        /// <param name="value">The range value to convert.</param>
        /// <returns>The converted pixel value.</returns>
        protected Double ValueToOffset(Double value)
        {
            var available = ActualTrackLength - ActualThumbLength;

            var min = Minimum;
            var max = Maximum;

            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var percent = (value - min) / (max - min);
            var used    = display.PixelsToDips(available * percent);

            return used;
        }

        /// <summary>
        /// Converts a pixel offset into the scroll bar's track to a range value.
        /// </summary>
        /// <param name="pixels">The pixel value to convert.</param>
        /// <returns>The converted range value.</returns>
        protected Double OffsetToValue(Double pixels)
        {
            var available = ActualTrackLength - ActualThumbLength;

            var min = Minimum;
            var max = Maximum;

            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var percent = pixels / available;
            var value   = (percent * (Maximum - Minimum)) + Minimum;

            return value;
        }

        /// <summary>
        /// Gets the offset in pixels from the left edge of the control to the left edge of the scroll bar's track.
        /// </summary>
        protected abstract Int32 ActualTrackOffsetX
        {
            get;
        }

        /// <summary>
        /// Gets the offset in pixels from the top edge of the control to the top edge of the scroll bar's track.
        /// </summary>
        protected abstract Int32 ActualTrackOffsetY
        {
            get;
        }

        /// <summary>
        /// Gets the width of the scroll bar's track in pixels.
        /// </summary>
        protected abstract Int32 ActualTrackWidth
        {
            get;
        }

        /// <summary>
        /// Gets the height of the scroll bar's track in pixels.
        /// </summary>
        protected abstract Int32 ActualTrackHeight
        {
            get;
        }

        /// <summary>
        /// Gets the length in pixels of the scroll bar's track.
        /// </summary>
        protected abstract Int32 ActualTrackLength
        {
            get;
        }

        /// <summary>
        /// Gets the length in pixels of the scroll bar's thumb.
        /// </summary>
        protected abstract Int32 ActualThumbLength
        {
            get;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ViewportSize"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleViewportSizeChanged(DependencyObject dobj)
        {
            var scrollbar = (ScrollBarBase)dobj;
            scrollbar.OnViewportSizeChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TrackColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleTrackColorChanged(DependencyObject dobj)
        {
            var scrollbar = (ScrollBarBase)dobj;
            scrollbar.OnTrackColorChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TrackImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleTrackImageChanged(DependencyObject dobj)
        {
            var scrollbar = (ScrollBarBase)dobj;
            scrollbar.OnTrackImageChanged();
        }
    }
}
