using System;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the track for a scroll bar or slider.
    /// </summary>
    [UvmlKnownType]
    public class Track : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public Track(UltravioletContext uv, String id)
            : base(uv, id)
        {
            this.Thumb = new Button(uv, null) 
            { 
                HorizontalAlignment = HorizontalAlignment.Stretch, 
                VerticalAlignment   = VerticalAlignment.Stretch 
            };
            this.Thumb.Classes.Add("track-thumb");
            this.Thumb.ChangeLogicalAndVisualParents(this, this);

            this.IncreaseButton = new RepeatButton(uv, null) 
            { 
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment   = VerticalAlignment.Stretch,
                Opacity             = 0
            };
            this.IncreaseButton.Classes.Add("track-increase");
            this.IncreaseButton.Click += HandleIncreaseButtonClick;
            this.IncreaseButton.ChangeLogicalAndVisualParents(this, this);

            this.DecreaseButton = new RepeatButton(uv, null) 
            { 
                HorizontalAlignment = HorizontalAlignment.Stretch, 
                VerticalAlignment   = VerticalAlignment.Stretch,
                Opacity             = 0
            };
            this.DecreaseButton.Classes.Add("track-decrease");
            this.DecreaseButton.Click += HandleDecreaseButtonClick;
            this.DecreaseButton.ChangeLogicalAndVisualParents(this, this);

            Mouse.AddPreviewMouseMoveHandler(this.Thumb, HandleThumbPreviewMouseMove);
            Mouse.AddPreviewMouseDownHandler(this.Thumb, HandleThumbPreviewMouseDown);
        }

        /// <summary>
        /// Gets or sets the track's orientation.
        /// </summary>
        public Orientation Orientation
        {
            get { return GetValue<Orientation>(OrientationProperty); }
            set { SetValue<Orientation>(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the track's associated viewport.
        /// </summary>
        public Double ViewportSize
        {
            get
            {
                var scrollbar = Control as ScrollBarBase;
                return (scrollbar == null) ? Double.NaN : scrollbar.ViewportSize;
            }
            set
            {
                var scrollbar = Control as ScrollBarBase;
                if (scrollbar != null)
                {
                    scrollbar.ViewportSize = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the track's minimum value.
        /// </summary>
        public Double Minimum
        {
            get
            {
                var scrollbar = Control as RangeBase;
                return (scrollbar == null) ? 0 : scrollbar.Minimum;
            }
            set
            {
                var scrollbar = Control as RangeBase;
                if (scrollbar != null)
                {
                    scrollbar.Minimum = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the track's maximum value.
        /// </summary>
        public Double Maximum
        {
            get
            {
                var scrollbar = Control as RangeBase;
                return (scrollbar == null) ? 0 : scrollbar.Maximum;
            }
            set
            {
                var scrollbar = Control as RangeBase;
                if (scrollbar != null)
                {
                    scrollbar.Maximum = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the track's current value.
        /// </summary>
        public Double Value
        {
            get
            {
                var scrollbar = Control as RangeBase;
                return (scrollbar == null) ? 0 : scrollbar.Value;
            }
            set
            {
                var scrollbar = Control as RangeBase;
                if (scrollbar != null)
                {
                    scrollbar.Value = value;
                }
            }
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Track),
            new DependencyPropertyMetadata(null, null, DependencyPropertyOptions.AffectsMeasure));

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get { return 3; }
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get { return 3; }
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            switch (childIndex)
            {
                case 0: return DecreaseButton;
                case 1: return Thumb;
                case 2: return IncreaseButton;
            }
            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            switch (childIndex)
            {
                case 0: return DecreaseButton;
                case 1: return Thumb;
                case 2: return IncreaseButton;
            }
            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var desiredSize = Size2D.Zero;

            Thumb.Measure(availableSize);
            desiredSize = Thumb.DesiredSize;

            return desiredSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            // Calculate the sizes of the track's components.
            var thumbSize          = CalculateThumbSize(finalSize);
            var decreaseButtonSize = CalculateDecreaseButtonSize(finalSize, thumbSize);
            var increaseButtonSize = CalculateIncreaseButtonSize(finalSize, decreaseButtonSize, thumbSize);

            // Arrange the track's components.
            var position    = new Point2D(0, 0);
            var orientation = this.Orientation;

            DecreaseButton.Arrange(new RectangleD(position, decreaseButtonSize));
            position += (orientation == Orientation.Horizontal) ? new Point2D(decreaseButtonSize.Width, 0) : new Point2D(0, decreaseButtonSize.Height);

            Thumb.Arrange(new RectangleD(position, thumbSize));
            position += (orientation == Orientation.Horizontal) ? new Point2D(thumbSize.Width, 0) : new Point2D(0, thumbSize.Height);

            IncreaseButton.Arrange(new RectangleD(position, increaseButtonSize));

            return finalSize;
        }

        /// <summary>
        /// Converts a range value to a pixel offset into the scroll bar's track.
        /// </summary>
        /// <param name="value">The range value to convert.</param>
        /// <param name="thumbSize">The size of the track.</param>
        /// <param name="trackSize">The size of the thumb.</param>
        /// <returns>The converted pixel value.</returns>
        protected Double ValueToOffset(Double value, Double trackSize, Double thumbSize)
        {
            var available = trackSize - thumbSize;

            var min = Minimum;
            var max = Maximum;

            if (max == min)
                return 0;

            var percent = (value - min) / (max - min);
            var used    = available * percent;

            return used;
        }

        /// <summary>
        /// Converts a pixel offset into the scroll bar's track to a range value.
        /// </summary>
        /// <param name="pixels">The pixel value to convert.</param>
        /// <param name="thumbSize">The size of the track.</param>
        /// <param name="trackSize">The size of the thumb.</param>
        /// <returns>The converted range value.</returns>
        protected Double OffsetToValue(Double pixels, Double trackSize, Double thumbSize)
        {
            var available = trackSize - thumbSize;

            var min = Minimum;
            var max = Maximum;

            if (max == min)
                return 0;

            var percent = pixels / available;
            var value   = (percent * (Maximum - Minimum)) + Minimum;

            return value;
        }

        /// <summary>
        /// Calculates the size of the track's thumb.
        /// </summary>
        /// <param name="trackSize">The size of the track.</param>
        /// <returns>The size of the track's thumb.</returns>
        protected Size2D CalculateThumbSize(Size2D trackSize)
        {
            if (Double.IsNaN(ViewportSize))
                return Thumb.DesiredSize;

            var orientation = this.Orientation;
            var max         = Maximum;
            var min         = Minimum;
            var vps         = ViewportSize;

            if (max - min + vps == 0)
            {
                return (orientation == Orientation.Horizontal) ? 
                    new Size2D(0, trackSize.Height) :
                    new Size2D(trackSize.Width, 0);
            }
            else
            {
                var trackLength = (orientation == Orientation.Horizontal) ? trackSize.Width : trackSize.Height;
                var thumbLength = ((vps / (max - min + vps)) * trackLength);

                return (orientation == Orientation.Horizontal) ?
                    new Size2D(Math.Ceiling(thumbLength), trackSize.Height) :
                    new Size2D(trackSize.Width, Math.Ceiling(thumbLength));                    
            }
        }

        /// <summary>
        /// Calculates the size of the track's decrease button.
        /// </summary>
        /// <param name="trackSize">The size of the track.</param>
        /// <param name="thumbSize">The size of the track's thumb.</param>
        /// <returns>The size of the track's decrease button.</returns>
        protected Size2D CalculateDecreaseButtonSize(Size2D trackSize, Size2D thumbSize)
        {
            var orientation = this.Orientation;
            var val         = Value;
            var min         = Minimum;
            var max         = Maximum;

            if (min == max)
            {
                return (orientation == Orientation.Horizontal) ? 
                    new Size2D(0, trackSize.Height) :
                    new Size2D(trackSize.Width, 0);
            }

            var trackLength = (orientation == Orientation.Horizontal) ? trackSize.Width : trackSize.Height;
            var thumbLength = (orientation == Orientation.Horizontal) ? thumbSize.Width : thumbSize.Height;
            var available   = trackLength - thumbLength;
            var percent     = (val - min) / (max - min);
            var used        = available * percent;

            return (orientation == Orientation.Horizontal) ?
                    new Size2D(Math.Floor(used), trackSize.Height) :
                    new Size2D(trackSize.Width, Math.Floor(used));  
        }

        /// <summary>
        /// Calculates the size of the track's increase button.
        /// </summary>
        /// <param name="trackSize">The size of the track.</param>
        /// <param name="decreaseButtonSize">The size of the track's decrease button.</param>
        /// <param name="thumbSize">The size of the track's thumb.</param>
        /// <returns>The size of the track's increase button.</returns>
        protected Size2D CalculateIncreaseButtonSize(Size2D trackSize, Size2D decreaseButtonSize, Size2D thumbSize)
        {
            return (Orientation == Orientation.Horizontal) ? 
               new Size2D((trackSize.Width - (decreaseButtonSize.Width + thumbSize.Width)), trackSize.Height) :
               new Size2D(trackSize.Width, (trackSize.Height - (decreaseButtonSize.Height + thumbSize.Height)));
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the decrease button.
        /// </summary>
        private void HandleDecreaseButtonClick(DependencyObject element)
        {
            var scrollbar = Control as RangeBase;
            if (scrollbar != null)
            {
                scrollbar.DecreaseLarge();
            }
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the increase button.
        /// </summary>
        private void HandleIncreaseButtonClick(DependencyObject element)
        {
            var scrollbar = Control as RangeBase;
            if (scrollbar != null)
            {
                scrollbar.IncreaseLarge();
            }
        }

        /// <summary>
        /// Handles the <see cref="Mouse.PreviewMouseMove"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbPreviewMouseMove(DependencyObject element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref Boolean handled)
        {
            var button = element as Button;
            if (button != null && button.IsDepressed)
            {
                if (Orientation == Orientation.Vertical)
                {
                    var relY = y - (AbsolutePosition.Y + thumbDragOffset);
                    Value = OffsetToValue(relY, RenderSize.Height, Thumb.RenderSize.Height);
                }
                else
                {
                    var relX = x - (AbsolutePosition.X + thumbDragOffset);
                    Value = OffsetToValue(relX, RenderSize.Width, Thumb.RenderSize.Width);
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="Mouse.PreviewMouseDown"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbPreviewMouseDown(DependencyObject element, MouseDevice device, MouseButton pressed, ref Boolean handled)
        {
            var uiElement = (UIElement)element;

            if (Orientation == Orientation.Vertical)
            {
                thumbDragOffset = Display.PixelsToDips(device.Y) - uiElement.AbsoluteBounds.Y;
            }
            else
            {
                thumbDragOffset = Display.PixelsToDips(device.X) - uiElement.AbsoluteBounds.X;
            }
        }

        // Component element references.
        private readonly Button Thumb = null;
        private readonly RepeatButton DecreaseButton = null;
        private readonly RepeatButton IncreaseButton = null;

        // State values.
        private Double thumbDragOffset;
    }
}
