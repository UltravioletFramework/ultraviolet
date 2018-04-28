using System;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the track for a scroll bar or slider.
    /// </summary>
    [UvmlKnownType]
    public class Track : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> type.
        /// </summary>
        static Track()
        {
            CommandManager.RegisterClassCommandBinding(typeof(Track), new CommandBinding(IncreaseCommand, null,
                (element, command, parameter, data) => data.CanExecute = true));
            CommandManager.RegisterClassCommandBinding(typeof(Track), new CommandBinding(DecreaseCommand, null,
                (element, command, parameter, data) => data.CanExecute = true));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Track(UltravioletContext uv, String name)
            : base(uv, name)
        {
            this.thumb = new Thumb(uv, null)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Focusable = false,
            };
            this.thumb.Classes.Add("track-thumb");
            this.thumb.ChangeLogicalAndVisualParents(this, this);
            KeyboardNavigation.SetIsTabStop(this.thumb, false);

            this.increaseRepeatButton = new RepeatButton(uv, null)
            {
                Width = Double.NaN,
                Height = Double.NaN,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Opacity = 0,
                Focusable = false,
            };
            this.increaseRepeatButton.Classes.Add("track-increase");
            this.increaseRepeatButton.Command = IncreaseCommand;
            this.increaseRepeatButton.CommandTarget = this;
            this.increaseRepeatButton.ChangeLogicalAndVisualParents(this, this);
            KeyboardNavigation.SetIsTabStop(this.increaseRepeatButton, false);

            this.decreaseRepeatButton = new RepeatButton(uv, null)
            {
                Width = Double.NaN,
                Height = Double.NaN,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Opacity = 0,
                Focusable = false,
            };
            this.decreaseRepeatButton.Classes.Add("track-decrease");
            this.decreaseRepeatButton.Command = DecreaseCommand;
            this.decreaseRepeatButton.CommandTarget = this;
            this.decreaseRepeatButton.ChangeLogicalAndVisualParents(this, this);
            KeyboardNavigation.SetIsTabStop(this.decreaseRepeatButton, false);
        }

        /// <summary>
        /// Calculates the change in <see cref="Value"/> that corresponds to moving the track's thumb
        /// by the specified amount along the horizontal and vertical axes.
        /// </summary>
        /// <param name="horizontal">The horizontal distance.</param>
        /// <param name="vertical">The vertical distance.</param>
        /// <returns>The change in <see cref="Value"/> which corresponds to the specified distances.</returns>
        public virtual Double ValueFromDistance(Double horizontal, Double vertical)
        {
            if (Orientation == Orientation.Horizontal)
            {
                return Math.Sign(horizontal) *
                    (OffsetToValue(Math.Abs(horizontal), RenderSize.Width, thumb.RenderSize.Width) - OffsetToValue(0, RenderSize.Width, thumb.RenderSize.Width));
            }
            else
            {
                return Math.Sign(vertical) *
                    (OffsetToValue(Math.Abs(vertical), RenderSize.Height, thumb.RenderSize.Height) - OffsetToValue(0, RenderSize.Height, thumb.RenderSize.Height));
            }
        }

        /// <summary>
        /// Calculates the <see cref="Value"/> that corresponds to the specified position within the track.
        /// </summary>
        /// <param name="pt">The point to evaluate.</param>
        /// <returns>The value that corresponds to the specified position. This value is not guaranteed to fall
        /// within the valid range between <see cref="Minimum"/> and <see cref="Maximum"/>.</returns>
        public virtual Double ValueFromPoint(Point2D pt)
        {
            if (Orientation == Orientation.Horizontal)
            {
                var relX = pt.X - (thumb.RenderSize.Width / 2);
                return OffsetToValue(relX, RenderSize.Width, thumb.RenderSize.Width);
            }
            else
            {
                var relY = pt.Y - (thumb.RenderSize.Height / 2);
                return OffsetToValue(relY, RenderSize.Height, thumb.RenderSize.Height);
            }
        }

        /// <summary>
        /// Gets the <see cref="Thumb"/> control which is used to adjust
        /// the value of the track.
        /// </summary>
        public Thumb Thumb
        {
            get { return thumb; }
        }
        
        /// <summary>
        /// Gets the <see cref="RepeatButton"/> control which is used to
        /// increase the value of the track.
        /// </summary>
        public RepeatButton IncreaseRepeatButton
        {
            get { return increaseRepeatButton; }
        }

        /// <summary>
        /// Gets the <see cref="RepeatButton"/> control which is used to
        /// decrease the value of the track.
        /// </summary>
        public RepeatButton DecreaseRepeatButton
        {
            get { return decreaseRepeatButton; }
        }

        /// <summary>
        /// Gets or sets the track's orientation.
        /// </summary>
        /// <value>A <see cref="Controls.Orientation"/> value which indicates whether the track is oriented vertically
        /// or horizontally. The default value is <see cref="Controls.Orientation.Horizontal"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="OrientationProperty"/></dpropField>
        ///		<dpropStylingName>orientation</dpropStylingName>
        ///		<dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Orientation Orientation
        {
            get { return GetValue<Orientation>(OrientationProperty); }
            set { SetValue<Orientation>(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the track's associated viewport.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the size in device independent pixels of the 
        /// track's associated viewport.</value>
        public Double ViewportSize
        {
            get { return GetValue<Double>(ViewportSizeProperty); }
            set { SetValue(ViewportSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the track's minimum value.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the minimum value of the <see cref="Value"/> property.</value>
        public Double Minimum
        {
            get { return GetValue<Double>(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the track's maximum value.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the maximum value of the <see cref="Value"/> property.</value>
        public Double Maximum
        {
            get { return GetValue<Double>(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the track's current value.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the track's current value.</value>
        public Double Value
        {
            get { return GetValue<Double>(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the track's direction of increasing value is reversed.
        /// </summary>
        /// <value>A <see cref="Boolean"/> value indicating whether the track's direction of increasing value is reversed.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsDirectionReversedProperty"/></dpropField>
        ///     <dpropStylingName>direction-reversed</dpropStylingName>
        ///     <dpropMetadata>AffectsMeasure</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsDirectionReversed
        {
            get { return GetValue<Boolean>(IsDirectionReversedProperty); }
            set { SetValue(IsDirectionReversedProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Orientation"/> dependency property.</value>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Track),
            new PropertyMetadata<Orientation>(null, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="ViewportSize"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ViewportSize"/> dependency property.</value>
        public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register("ViewportSize", typeof(Double), typeof(Track),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.NaN, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Minimum"/> dependency property.</value>
        public static readonly DependencyProperty MinimumProperty = RangeBase.MinimumProperty.AddOwner(typeof(Track),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Maximum"/> dependency property.</value>
        public static readonly DependencyProperty MaximumProperty = RangeBase.MaximumProperty.AddOwner(typeof(Track),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.One, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Value"/> dependency property.</value>
        public static readonly DependencyProperty ValueProperty = RangeBase.ValueProperty.AddOwner(typeof(Track),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="IsDirectionReversed"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsDirectionReversed"/> dependency property.</value>
        public static readonly DependencyProperty IsDirectionReversedProperty = DependencyProperty.Register("IsDirectionReversed", typeof(Boolean), typeof(Track),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// A command that increases the track's value.
        /// </summary>
        public static readonly RoutedCommand IncreaseCommand = new RoutedCommand("Increase", typeof(Track));

        /// <summary>
        /// A command that decreases the track's value.
        /// </summary>
        public static readonly RoutedCommand DecreaseCommand = new RoutedCommand("Decrease", typeof(Track));

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
                case 0: return decreaseRepeatButton;
                case 1: return thumb;
                case 2: return increaseRepeatButton;
            }
            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            switch (childIndex)
            {
                case 0: return decreaseRepeatButton;
                case 1: return thumb;
                case 2: return increaseRepeatButton;
            }
            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var desiredSize = Size2D.Zero;

            thumb.Measure(availableSize);
            desiredSize = thumb.DesiredSize;

            return desiredSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            // Calculate the sizes of the track's components.
            var thumbSize = CalculateThumbSize(finalSize);
            var decreaseButtonSize = CalculateDecreaseButtonSize(finalSize, thumbSize);
            var increaseButtonSize = CalculateIncreaseButtonSize(finalSize, decreaseButtonSize, thumbSize);

            // Arrange the track's components.
            var position = new Point2D(0, 0);
            var orientation = this.Orientation;

            if (IsDirectionReversed)
            {
                increaseRepeatButton.Arrange(new RectangleD(position, increaseButtonSize));
                position += (orientation == Orientation.Horizontal) ? new Point2D(increaseButtonSize.Width, 0) : new Point2D(0, increaseButtonSize.Height);

                thumb.Arrange(new RectangleD(position, thumbSize));
                position += (orientation == Orientation.Horizontal) ? new Point2D(thumbSize.Width, 0) : new Point2D(0, thumbSize.Height);

                decreaseRepeatButton.Arrange(new RectangleD(position, decreaseButtonSize));
            }
            else
            {
                decreaseRepeatButton.Arrange(new RectangleD(position, decreaseButtonSize));
                position += (orientation == Orientation.Horizontal) ? new Point2D(decreaseButtonSize.Width, 0) : new Point2D(0, decreaseButtonSize.Height);

                thumb.Arrange(new RectangleD(position, thumbSize));
                position += (orientation == Orientation.Horizontal) ? new Point2D(thumbSize.Width, 0) : new Point2D(0, thumbSize.Height);

                increaseRepeatButton.Arrange(new RectangleD(position, increaseButtonSize));
            }

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

            if (IsDirectionReversed)
            {
                value = Maximum - value;
            }

            var percent = (value - min) / (max - min);
            var used = available * percent;

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
            var value = (percent * (Maximum - Minimum)) + Minimum;

            if (IsDirectionReversed)
            {
                value = Maximum - value;
            }

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
                return thumb.DesiredSize;

            var orientation = this.Orientation;
            var max = Maximum;
            var min = Minimum;
            var vps = ViewportSize;

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
            var val = Value;
            var min = Minimum;
            var max = Maximum;
            
            if (min == max)
            {
                return (orientation == Orientation.Horizontal) ?
                    new Size2D(0, trackSize.Height) :
                    new Size2D(trackSize.Width, 0);
            }

            var trackLength = (orientation == Orientation.Horizontal) ? trackSize.Width : trackSize.Height;
            var thumbLength = (orientation == Orientation.Horizontal) ? thumbSize.Width : thumbSize.Height;
            var available = trackLength - thumbLength;
            var percent = (val - min) / (max - min);
            var used = available * percent;

            return (orientation == Orientation.Horizontal) ?
                    new Size2D(Math.Max(0, Math.Floor(used)), trackSize.Height) :
                    new Size2D(trackSize.Width, Math.Max(0, Math.Floor(used)));
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
               new Size2D(Math.Max(0, trackSize.Width - (decreaseButtonSize.Width + thumbSize.Width)), trackSize.Height) :
               new Size2D(trackSize.Width, Math.Max(0, trackSize.Height - (decreaseButtonSize.Height + thumbSize.Height)));
        }

        /// <summary>
        /// Occurs when the value of the <see cref="DelayProperty"/> dependency property changes.
        /// </summary>
        private static void HandleDelayChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            var track = element as Track;
            if (track == null)
                return;

            track.increaseRepeatButton.Delay = newValue;
            track.decreaseRepeatButton.Delay = newValue;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IntervalProperty"/> dependency property changes.
        /// </summary>
        private static void HandleIntervalChanged(DependencyObject element, Double oldValue, Double newValue)
        {
            var track = element as Track;
            if (track == null)
                return;

            track.increaseRepeatButton.Interval = newValue;
            track.decreaseRepeatButton.Interval = newValue;
        }

#pragma warning disable 414
        /// <summary>
        /// Identifies the <see cref="RepeatButton.Delay"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="RepeatButton.Delay"/> dependency property.</value>
        private static readonly DependencyProperty DelayProperty = RepeatButton.DelayProperty.AddOwner(typeof(Track),
            new PropertyMetadata<Double>(HandleDelayChanged));

        /// <summary>
        /// Identifies the <see cref="RepeatButton.Interval"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="RepeatButton.Interval"/> dependency property.</value>
        private static readonly DependencyProperty IntervalProperty = RepeatButton.IntervalProperty.AddOwner(typeof(Track),
            new PropertyMetadata<Double>(HandleIntervalChanged));
#pragma warning restore 414

        // Component element references.
        private readonly Thumb thumb = null;
        private readonly RepeatButton decreaseRepeatButton = null;
        private readonly RepeatButton increaseRepeatButton = null;
    }
}