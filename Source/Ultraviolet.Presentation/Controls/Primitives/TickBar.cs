using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the element used to draw a <see cref="Slider"/> control's ticks.
    /// </summary>
    [UvmlKnownType]
    public class TickBar : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TickBar(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Identifies the <see cref="IsDirectionReversed"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsDirectionReversed"/> dependency property.</value>
        public static readonly DependencyProperty IsDirectionReversedProperty = Slider.IsDirectionReversedProperty.AddOwner(typeof(TickBar),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));

        /// <summary>
        /// Identifies the <see cref="TickColor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="TickColor"/> dependency property.</value>
        public static readonly DependencyProperty TickColorProperty = DependencyProperty.Register("TickColor", typeof(Color), typeof(TickBar),
            new PropertyMetadata<Color>(Color.Black));

        /// <summary>
        /// Identifies the <see cref="TickLengthMajor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="TickLengthMajor"/> dependency property.</value>
        public static readonly DependencyProperty TickLengthMajorProperty = DependencyProperty.Register("TickLengthMajor", typeof(Double), typeof(TickBar),
            new PropertyMetadata<Double>(1.0));

        /// <summary>
        /// Identifies the <see cref="TickLengthMinor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="TickLengthMinor"/> dependency property.</value>
        public static readonly DependencyProperty TickLengthMinorProperty = DependencyProperty.Register("TickLengthMinor", typeof(Double), typeof(TickBar),
            new PropertyMetadata<Double>(0.75));

        /// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Minimum"/> dependency property.</value>
        public static readonly DependencyProperty MinimumProperty = RangeBase.MinimumProperty.AddOwner(typeof(TickBar), 
            new PropertyMetadata<Double>(0.0, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Maximum"/> dependency property.</value>
        public static readonly DependencyProperty MaximumProperty = RangeBase.MaximumProperty.AddOwner(typeof(TickBar),
            new PropertyMetadata<Double>(100.0, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="ReservedSpace"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ReservedSpace"/> dependency property.</value>
        public static readonly DependencyProperty ReservedSpaceProperty = DependencyProperty.Register("ReservedSpace", typeof(Double), typeof(TickBar),
            new PropertyMetadata<Double>(0.0));

        /// <summary>
        /// Identifies the <see cref="TickFrequency"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="TickFrequency"/> dependency property.</value>
        public static readonly DependencyProperty TickFrequencyProperty = Slider.TickFrequencyProperty.AddOwner(typeof(TickBar),
            new PropertyMetadata<Double>(1.0));

        /// <summary>
        /// Identifies the <see cref="Placement"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Placement"/> dependency property.</value>
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(TickBarPlacement), typeof(TickBar),
            new PropertyMetadata<TickBarPlacement>(TickBarPlacement.Top));

        /// <summary>
        /// Gets or sets a value indicating whether the slider's direction of increasing value is reversed.
        /// </summary>
        /// <value>A <see cref="Boolean"/> value indicating whether the slider's direction of increasing value is reversed.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsDirectionReversedProperty"/></dpropField>
        ///     <dpropStylingName>direction-reversed</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsDirectionReversed
        {
            get { return GetValue<Boolean>(IsDirectionReversedProperty); }
            set { SetValue(IsDirectionReversedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a the color of the tick bar's ticks.
        /// </summary>
        /// <value>A <see cref="Color"/> value which represents the color of the tick bar's ticks.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TickColorProperty"/></dpropField>
        ///     <dpropStylingName>tick-color</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color TickColor
        {
            get { return GetValue<Color>(TickColorProperty); }
            set { SetValue(TickColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a the relative length of the tick bar's major ticks.
        /// </summary>
        /// <value>A <see cref="Double"/> value which represents the proportion 
        /// of the tick bar which is filled by the tick bar's major ticks.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TickLengthMajorProperty"/></dpropField>
        ///     <dpropStylingName>tick-length-major</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double TickLengthMajor
        {
            get { return GetValue<Double>(TickLengthMajorProperty); }
            set { SetValue(TickLengthMajorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a the relative length of the tick bar's minor ticks.
        /// </summary>
        /// <value>A <see cref="Double"/> value which represents the proportion 
        /// of the tick bar which is filled by the tick bar's minor ticks.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TickLengthMajorProperty"/></dpropField>
        ///     <dpropStylingName>tick-length-minor</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double TickLengthMinor
        {
            get { return GetValue<Double>(TickLengthMinorProperty); }
            set { SetValue(TickLengthMinorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value of the <see cref="RangeBase.Minimum"/> property.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the minimum value of the <see cref="RangeBase.Minimum"/> property.
        /// The default value is 0.0.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="MinimumProperty"/></dpropField>
        ///		<dpropStylingName>minimum</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double Minimum
        {
            get { return GetValue<Double>(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum allowed value of the <see cref="RangeBase.Maximum"/> property.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the maximum value of the <see cref="RangeBase.Maximum"/> property.
        /// The default value is 1.0.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="MaximumProperty"/></dpropField>
        ///		<dpropStylingName>maximum</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double Maximum
        {
            get { return GetValue<Double>(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the space buffer for the area that contains the tick marks that are
        /// specified for a <see cref="TickBar"/>.
        /// </summary>
        /// <value>A <see cref="Double"/> value that the space buffer for the area that
        /// contains the tick marks that are specified for a <see cref="TickBar"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ReservedSpaceProperty"/></dpropField>
        ///     <dpropStylingName>reserved-space</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double ReservedSpace
        {
            get { return GetValue<Double>(ReservedSpaceProperty); }
            set { SetValue(ReservedSpaceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interval between tick marks.
        /// </summary>
        /// <value>A <see cref="Double"/> value that represents the interval between tick marks.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TickFrequencyProperty"/></dpropField>
        ///     <dpropStylingName>tick-frequency</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double TickFrequency
        {
            get { return GetValue<Double>(TickFrequencyProperty); }
            set { SetValue(TickFrequencyProperty, value); }
        }

        /// <summary>
        /// Gets or sets where tick marks appear relative to a <see cref="Slider"/> control.
        /// </summary>
        /// <value>A <see cref="TickBarPlacement"/> value that represents where tick marks appear
        /// relative to a <see cref="Slider"/> control.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="PlacementProperty"/></dpropField>
        ///     <dpropStylingName>placement</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public TickBarPlacement Placement
        {
            get { return GetValue<TickBarPlacement>(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        /// <inheritdoc/>
        internal override void OnPreApplyTemplate()
        {
            if (TemplatedParent is OrientedSlider parent)
            {
                var templateWrapperType = PresentationFoundation.GetDataSourceWrapper(parent).GetType();

                if (HasDefaultValue(MinimumProperty))
                    BindValue(MinimumProperty, templateWrapperType, "{{Minimum}}");
                if (HasDefaultValue(MaximumProperty))
                    BindValue(MaximumProperty, templateWrapperType, "{{Maximum}}");
                if (HasDefaultValue(TickFrequencyProperty))
                    BindValue(TickFrequencyProperty, templateWrapperType, "{{TickFrequency}}");
                if (HasDefaultValue(IsDirectionReversedProperty))
                    BindValue(IsDirectionReversedProperty, templateWrapperType, "{{IsDirectionReversed}}");
                if (HasDefaultValue(ReservedSpaceProperty))
                {
                    if (TemplatedParent is HSlider)
                    {
                        BindValue(ReservedSpaceProperty, templateWrapperType, "{{Track.Thumb.ActualWidth}}");
                    }
                    else
                    {
                        BindValue(ReservedSpaceProperty, templateWrapperType, "{{Track.Thumb.ActualHeight}}");
                    }
                }
            }
            base.OnPreApplyTemplate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="dc"></param>
        protected override void OnDrawing(UltravioletTime time, DrawingContext dc)
        {
            RectangleD NormalizeRect(Double x, Double y, Double width, Double height)
            {
                var x1 = x;
                var x2 = x + width;
                var y1 = y;
                var y2 = y + height;
                return new RectangleD(Math.Min(x1, x2), Math.Min(y1, y2), Math.Abs(width), Math.Abs(height));
            };

            // Determine tick placement based on orientation.
            var size = new Size2D(ActualWidth, ActualHeight);
            var range = Math.Max(0.0, Maximum - Minimum);
            var frequency = TickFrequency;
            var reserved = ReservedSpace / 2.0;
            var conversion = 1.0;

            var tickColor = TickColor;
            var tickLength = 1.0;
            var tickPosStart = Point2D.Zero;
            var tickPosEnd = Point2D.Zero;

            switch (Placement)
            {
                case TickBarPlacement.Left:
                    if (MathUtil.AreApproximatelyEqual(ReservedSpace, size.Height))
                        return;
                    size.Height -= ReservedSpace;
                    tickLength = -size.Width;
                    tickPosStart = new Point2D(size.Width, reserved);
                    tickPosEnd = new Point2D(size.Width, reserved + size.Height);
                    conversion = Math.Abs(size.Height) / range;
                    break;

                case TickBarPlacement.Top:
                    if (MathUtil.AreApproximatelyEqual(ReservedSpace, size.Width))
                        return;
                    size.Width -= ReservedSpace;
                    tickLength = -size.Height;
                    tickPosStart = new Point2D(reserved, size.Height);
                    tickPosEnd = new Point2D(reserved + size.Width, size.Height);
                    conversion = Math.Abs(size.Width) / range;
                    break;

                case TickBarPlacement.Right:
                    if (MathUtil.AreApproximatelyEqual(ReservedSpace, size.Height))
                        return;
                    size.Height -= ReservedSpace;
                    tickLength = size.Width;
                    tickPosStart = new Point2D(0, reserved);
                    tickPosEnd = new Point2D(0, reserved + size.Height);
                    conversion = Math.Abs(size.Height) / range;
                    break;

                case TickBarPlacement.Bottom:
                    if (MathUtil.AreApproximatelyEqual(ReservedSpace, size.Width))
                        return;
                    size.Width -= ReservedSpace;
                    tickLength = size.Height;
                    tickPosStart = new Point2D(reserved, 0);
                    tickPosEnd = new Point2D(reserved + size.Width, 0);
                    conversion = Math.Abs(size.Width) / range;
                    break;
            }

            // Account for reversed directionality.
            if (IsDirectionReversed)
            {
                var tmp = tickPosStart;
                tickPosStart = tickPosEnd;
                tickPosEnd = tmp;
                conversion = -conversion;
            }

            // Draw the bar's ticks.
            var isVertical = (Placement == TickBarPlacement.Left || Placement == TickBarPlacement.Right);
            if (isVertical)
            {
                var minFrequency = range / size.Height;
                if (minFrequency > frequency && MathUtil.IsApproximatelyGreaterThan(frequency, 0.0))
                    frequency = minFrequency;

                // Draw end ticks
                DrawBlank(dc, NormalizeRect(tickPosStart.X, tickPosStart.Y, tickLength * TickLengthMajor, 1), tickColor);
                DrawBlank(dc, NormalizeRect(tickPosEnd.X, tickPosEnd.Y, tickLength * TickLengthMajor, 1), tickColor);

                // Draw minor ticks
                for (var f = frequency; MathUtil.IsApproximatelyLessThan(f, range); f += frequency)
                {
                    var x = tickPosStart.X;
                    var y = f * conversion + tickPosStart.Y;
                    var w = tickLength * TickLengthMinor;
                    var h = 1;
                    DrawBlank(dc, NormalizeRect(x, y, w, h), tickColor);
                }
            }
            else
            {
                var minFrequency = range / size.Width;
                if (minFrequency > frequency && MathUtil.IsApproximatelyGreaterThan(frequency, 0.0))
                    frequency = minFrequency;

                // Draw end ticks
                DrawBlank(dc, NormalizeRect(tickPosStart.X, tickPosStart.Y, 1, tickLength * TickLengthMajor), tickColor);
                DrawBlank(dc, NormalizeRect(tickPosEnd.X, tickPosEnd.Y, 1, tickLength * TickLengthMajor), tickColor);

                // Draw minor ticks
                for (var f = frequency; MathUtil.IsApproximatelyLessThan(f, range); f += frequency)
                {
                    var x = f * conversion + tickPosStart.X;
                    var y = tickPosStart.Y;
                    var w = 1.0;
                    var h = tickLength * TickLengthMinor;
                    DrawBlank(dc, NormalizeRect(x, y, w, h), tickColor);
                }
            }
            
            base.OnDrawing(time, dc);
        }
    }
}
