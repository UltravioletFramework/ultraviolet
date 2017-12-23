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
    }
}
