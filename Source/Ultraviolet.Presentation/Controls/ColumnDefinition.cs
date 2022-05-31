using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents the definition for a column in a <see cref="Grid"/> control.
    /// </summary>
    [UvmlKnownType]
    public class ColumnDefinition : DefinitionBase
    {
        /// <summary>
        /// Gets or sets the column's width in device independent pixels.
        /// </summary>
        /// <value>A <see cref="GridLength"/> value that specifies the column's width.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="WidthProperty"/></dpropField>
        ///     <dpropStylingName>width</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public GridLength Width
        {
            get { return GetValue<GridLength>(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the column's minimum width in device independent pixels.
        /// </summary>
        /// <value>A <see cref="Double"/> value that specifies the column's minimum 
        /// width in device-independent pixels.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="MinWidthProperty"/></dpropField>
        ///     <dpropStylingName>min-width</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double MinWidth
        {
            get { return GetValue<Double>(MinWidthProperty); }
            set { SetValue(MinWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the column's maximum width in device independent pixels.
        /// </summary>
        /// <value>A <see cref="Double"/> value that specifies the column's maximum 
        /// width in device-independent pixels.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="MaxWidthProperty"/></dpropField>
        ///     <dpropStylingName>max-width</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double MaxWidth
        {
            get { return GetValue<Double>(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }

        /// <summary>
        /// Gets the column's actual width after measurement and arrangement.
        /// </summary>
        public Double ActualWidth
        {
            get { return AssumedUnitType == GridUnitType.Auto ? MeasuredContentDimension : MeasuredDimension; }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Width"/> property changes.
        /// </summary>
        public event DefinitionEventHandler WidthChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MinWidth"/> property changes.
        /// </summary>
        public event DefinitionEventHandler MinWidthChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MaxWidth"/> property changes.
        /// </summary>
        public event DefinitionEventHandler MaxWidthChanged;

        /// <summary>
        /// Identifies the <see cref="Width"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Width"/> dependency property.</value>
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(GridLength), typeof(ColumnDefinition),
            new PropertyMetadata<GridLength>(PresentationBoxedValues.GridLength.One, HandleWidthChanged));

        /// <summary>
        /// Identifies the <see cref="MinWidth"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="MinWidth"/> dependency property.</value>
        public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(Double), typeof(ColumnDefinition),
            new PropertyMetadata<Double>(HandleMinWidthChanged));

        /// <summary>
        /// Identifies the <see cref="MaxWidth"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="MaxWidth"/> dependency property.</value>
        public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(Double), typeof(ColumnDefinition),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.PositiveInfinity, HandleMaxWidthChanged));
        
        /// <inheritdoc/>
        internal override GridLength Dimension
        {
            get { return Width; }
        }

        /// <inheritdoc/>
        internal override Double MinDimension
        {
            get { return MinWidth; }
        }

        /// <inheritdoc/>
        internal override Double MaxDimension
        {
            get { return MaxWidth; }
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
            get { return ActualWidth; }
        }

        /// <inheritdoc/>
        internal override Double Position
        {
            get;
            set;
        }

        /// <summary>
        /// Raises the <see cref="WidthChanged"/> event.
        /// </summary>
        protected void OnWidthChanged() =>
            WidthChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="MinWidthChanged"/> event.
        /// </summary>
        protected void OnMinWidthChanged() =>
            MinWidthChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="MaxWidthChanged"/> event.
        /// </summary>
        protected void OnMaxWidthChanged() =>
            MaxWidthChanged?.Invoke(this);

        /// <summary>
        /// Occurs when the value of the <see cref="Width"/> dependency property changes.
        /// </summary>
        private static void HandleWidthChanged(DependencyObject dobj, GridLength oldValue, GridLength newValue)
        {
            var definition = (ColumnDefinition)dobj;
            definition.OnWidthChanged();
            definition.OnMeasureChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MinWidth"/> dependency property changes.
        /// </summary>
        private static void HandleMinWidthChanged(DependencyObject dobj, Double oldValue, Double newValue)
        {
            var definition = (ColumnDefinition)dobj;
            definition.OnMinWidthChanged();
            definition.OnMeasureChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MaxWidth"/> dependency property changes.
        /// </summary>
        private static void HandleMaxWidthChanged(DependencyObject dobj, Double oldValue, Double newValue)
        {
            var definition = (ColumnDefinition)dobj;
            definition.OnMaxWidthChanged();
            definition.OnMeasureChanged();
        }

        /// <summary>
        /// Called when a property which affects the measure of this column is changed.
        /// </summary>
        private void OnMeasureChanged()
        {
            if (Grid != null)
                Grid.OnColumnsModified();
        }
    }
}
