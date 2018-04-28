using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents the definition for a row in a <see cref="Grid"/> control.
    /// </summary>
    [UvmlKnownType]
    public class RowDefinition : DefinitionBase
    {
        /// <summary>
        /// Gets or sets the row's height in device independent pixels.
        /// </summary>
        /// <value>A <see cref="GridLength"/> value that specifies the row's height.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="HeightProperty"/></dpropField>
        ///     <dpropStylingName>height</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public GridLength Height
        {
            get { return GetValue<GridLength>(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the row's minimum height in device independent pixels.
        /// </summary>
        /// <value>A <see cref="Double"/> value that specifies the row's minimum 
        /// height in device-independent pixels.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="MinHeightProperty"/></dpropField>
        ///     <dpropStylingName>min-height</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double MinHeight
        {
            get { return GetValue<Double>(MinHeightProperty); }
            set { SetValue(MinHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the row's maximum height in device independent pixels.
        /// </summary>
        /// <value>A <see cref="Double"/> value that specifies the row's maximum 
        /// height in device-independent pixels.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="MaxHeightProperty"/></dpropField>
        ///     <dpropStylingName>max-height</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double MaxHeight
        {
            get { return GetValue<Double>(MaxHeightProperty); }
            set { SetValue(MaxHeightProperty, value); }
        }

        /// <summary>
        /// Gets the row's actual height after measurement and arrangement.
        /// </summary>
        public Double ActualHeight
        {
            get { return AssumedUnitType == GridUnitType.Auto ? MeasuredContentDimension : MeasuredDimension; }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Height"/> property changes.
        /// </summary>
        public event DefinitionEventHandler HeightChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MinHeight"/> property changes.
        /// </summary>
        public event DefinitionEventHandler MinHeightChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MaxHeight"/> property changes.
        /// </summary>
        public event DefinitionEventHandler MaxHeightChanged;

        /// <summary>
        /// Identifies the <see cref="Height"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Height"/> dependency property.</value>
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(GridLength), typeof(RowDefinition),
            new PropertyMetadata<GridLength>(PresentationBoxedValues.GridLength.One, HandleHeightChanged));

        /// <summary>
        /// Identifies the <see cref="MinHeight"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="MinHeight"/> dependency property.</value>
        public static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register("MinHeight", typeof(Double), typeof(RowDefinition),
            new PropertyMetadata<Double>(HandleMinHeightChanged));

        /// <summary>
        /// Identifies the <see cref="MaxHeight"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="MaxHeight"/> dependency property.</value>
        public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register("MaxHeight", typeof(Double), typeof(RowDefinition),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.PositiveInfinity, HandleMaxHeightChanged));

        /// <inheritdoc/>
        internal override GridLength Dimension
        {
            get { return Height; }
        }

        /// <inheritdoc/>
        internal override Double MinDimension
        {
            get { return MinHeight; }
        }

        /// <inheritdoc/>
        internal override Double MaxDimension
        {
            get { return MaxHeight; }
        }

        /// <inheritdoc/>
        internal override Double MeasuredDimension
        {
            get;
            set;
        }

        /// <inheritdoc/>
        internal override Double MeasuredContentDimension
        {
            get;
            set;
        }

        /// <inheritdoc/>
        internal override Double ActualDimension
        {
            get { return ActualHeight; }
        }

        /// <inheritdoc/>
        internal override Double Position
        {
            get;
            set;
        }

        /// <summary>
        /// Raises the <see cref="HeightChanged"/> event.
        /// </summary>
        protected virtual void OnHeightChanged() =>
            HeightChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="MinHeightChanged"/> event.
        /// </summary>
        protected virtual void OnMinHeightChanged() =>
            MinHeightChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="MaxHeightChanged"/> event.
        /// </summary>
        protected virtual void OnMaxHeightChanged() =>
            MaxHeightChanged?.Invoke(this);

        /// <summary>
        /// Occurs when the value of the <see cref="Height"/> dependency property changes.
        /// </summary>
        private static void HandleHeightChanged(DependencyObject dobj, GridLength oldValue, GridLength newValue)
        {
            var definition = (RowDefinition)dobj;
            definition.OnHeightChanged();
            definition.OnMeasureChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MinHeight"/> dependency property changes.
        /// </summary>
        private static void HandleMinHeightChanged(DependencyObject dobj, Double oldValue, Double newValue)
        {
            var definition = (RowDefinition)dobj;
            definition.OnMinHeightChanged();
            definition.OnMeasureChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MaxHeight"/> dependency property changes.
        /// </summary>
        private static void HandleMaxHeightChanged(DependencyObject dobj, Double oldValue, Double newValue)
        {
            var definition = (RowDefinition)dobj;
            definition.OnMaxHeightChanged();
            definition.OnMeasureChanged();
        }

        /// <summary>
        /// Called when a property which affects the measure of this row is changed.
        /// </summary>
        private void OnMeasureChanged()
        {
            if (Grid != null)
                Grid.OnRowsModified();
        }
    }
}
