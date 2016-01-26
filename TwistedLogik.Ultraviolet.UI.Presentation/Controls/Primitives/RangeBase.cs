using System;
using System.ComponentModel;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the base class for controls which represent a value within a specified range.
    /// </summary>
    [UvmlKnownType]
    [DefaultProperty("Value")]
    public abstract class RangeBase : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public RangeBase(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Increases the value of the <see cref="Value"/> property by a small amount.
        /// </summary>
        public void IncreaseSmall()
        {
            Value += SmallChange;
        }

        /// <summary>
        /// Increases the value of the <see cref="Value"/> property by a large amount.
        /// </summary>
        public void IncreaseLarge()
        {
            Value += LargeChange;
        }

        /// <summary>
        /// Decreases the value of the <see cref="Value"/> property by a small amount.
        /// </summary>
        public void DecreaseSmall()
        {
            Value -= SmallChange;
        }

        /// <summary>
        /// Decreases the value of the <see cref="Value"/> property by a large amount.
        /// </summary>
        public void DecreaseLarge()
        {
            Value -= LargeChange;
        }

        /// <summary>
        /// Gets or sets the range control's current value.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the value of the control. The default
        /// value is 0.0.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="ValueProperty"/></dpropField>
        ///		<dpropStylingName>value</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double Value
        {
            get { return GetValue<Double>(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value of the <see cref="Value"/> property.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the minimum value of the <see cref="Value"/> property.
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
        /// Gets or sets the maximum allowed value of the <see cref="Value"/> property.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the maximum value of the <see cref="Value"/> property.
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
        /// Gets or sets the value that is added or removed from the <see cref="Value"/> property
        /// when the range control applies a small change.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the amount by which the <see cref="Value"/> property
        /// is incremented or decremented when the range control applies a small change.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="SmallChangeProperty"/></dpropField>
        ///		<dpropStylingName>small-change</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double SmallChange
        {
            get { return GetValue<Double>(SmallChangeProperty); }
            set { SetValue(SmallChangeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that is added or removed from the <see cref="Value"/> property
        /// when the range control applies a large change.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the amount by which the <see cref="Value"/> property
        /// is incremented or decremented when the range control applies a large change.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="LargeChangeProperty"/></dpropField>
        ///		<dpropStylingName>large-change</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double LargeChange
        {
            get { return GetValue<Double>(LargeChangeProperty); }
            set { SetValue(LargeChangeProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="ValueChangedEvent"/></revtField>
        ///		<revtStylingName>value-changed</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Value"/> dependency property.</value>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Double), typeof(RangeBase),
            new PropertyMetadata<Double>(HandleValueChanged, CoerceValue));

        /// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Minimum"/> dependency property.</value>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(Double), typeof(RangeBase),
            new PropertyMetadata<Double>(HandleMinimumChanged));

        /// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Maximum"/> dependency property.</value>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(Double), typeof(RangeBase),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.One, HandleMaximumChanged));

        /// <summary>
        /// Identifies the <see cref="SmallChange"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="SmallChange"/> dependency property.</value>
        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register("SmallChange", typeof(Double), typeof(RangeBase),
            new PropertyMetadata<Double>(0.1));

        /// <summary>
        /// Identifies the <see cref="LargeChange"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="LargeChange"/> dependency property.</value>
        public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register("LargeChange", typeof(Double), typeof(RangeBase),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.One));

        /// <summary>
        /// Identifies the <see cref="ValueChanged"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="ValueChanged"/> routed event.</value>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(RangeBase));

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            var evtData = new RoutedEventData(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(ValueChangedEvent);
            evtDelegate(this, ref evtData);
        }

        /// <summary>
        /// Called when the value of the <see cref="Maximum"/> property changes.
        /// </summary>
        protected virtual void OnMinimumChanged()
        {

        }

        /// <summary>
        /// Called when the value of the <see cref="Minimum"/> property changes.
        /// </summary>
        protected virtual void OnMaximumChanged()
        {

        }

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> dependency property changes.
        /// </summary>
        private static void HandleValueChanged(DependencyObject dobj, Double oldValue, Double newValue)
        {
            var range = (RangeBase)dobj;
            range.OnValueChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Minimum"/> dependency property changes.
        /// </summary>
        private static void HandleMinimumChanged(DependencyObject dobj, Double oldValue, Double newValue)
        {
            var range = (RangeBase)dobj;
            range.CoerceValue(ValueProperty);
            range.CoerceValue(MaximumProperty);
            range.OnMinimumChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Maximum"/> dependency property changes.
        /// </summary>
        private static void HandleMaximumChanged(DependencyObject dobj, Double oldValue, Double newValue)
        {
            var range = (RangeBase)dobj;
            range.CoerceValue(ValueProperty);
            range.OnMaximumChanged();
        }

        /// <summary>
        /// Coerces the <see cref="Value"/> property so that it remains within
        /// the range specified by <see cref="Minimum"/> and <see cref="Maximum"/>.
        /// </summary>
        private static Double CoerceValue(DependencyObject dobj, Double value)
        {
            var rangeBase = (RangeBase)dobj;
            
            var min = rangeBase.Minimum;
            if (min > value)
                return min;
            
            var max = rangeBase.Maximum;
            if (max < value)
                return max;

            return value;
        }

        /// <summary>
        /// Coerces the <see cref="Maximum"/> property so that it remains larger
        /// than the <see cref="Minimum"/> property.
        /// </summary>
        private static Double CoerceMaximum(DependencyObject dobj, Double value)
        {
            var rangeBase = (RangeBase)dobj;

            var min = rangeBase.Minimum;
            if (min > value)
                return min;

            return value;
        }
    }
}
