using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the definition for a column in a <see cref="Grid"/> control.
    /// </summary>
    public class ColumnDefinition : DefinitionBase
    {
        /// <summary>
        /// Gets or sets the column's width in device independent pixels.
        /// </summary>
        public GridLength Width
        {
            get { return GetValue<GridLength>(WidthProperty); }
            set { SetValue<GridLength>(WidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the column's minimum width in device independent pixels.
        /// </summary>
        public Double MinWidth
        {
            get { return GetValue<Double>(MinWidthProperty); }
            set { SetValue<Double>(MinWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the column's maximum width in device independent pixels.
        /// </summary>
        public Double MaxWidth
        {
            get { return GetValue<Double>(MaxWidthProperty); }
            set { SetValue<Double>(MaxWidthProperty, value); }
        }

        /// <summary>
        /// Gets the column's measured width in device-independent pixels.
        /// </summary>
        public Double MeasuredWidth
        {
            get { return actualWidth; }
            internal set { actualWidth = value; }
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
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(GridLength), typeof(ColumnDefinition),
            new DependencyPropertyMetadata(HandleWidthChanged, () => new GridLength(1.0), DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="MinWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(Double), typeof(ColumnDefinition),
            new DependencyPropertyMetadata(HandleMinWidthChanged, () => 0.0, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="MaxWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(Double), typeof(ColumnDefinition),
            new DependencyPropertyMetadata(HandleMaxWidthChanged, () => Double.PositiveInfinity, DependencyPropertyOptions.None));

        /// <summary>
        /// Gets or sets the distance between the left edge of the column's grid and the column's left edge.
        /// </summary>
        internal Double OffsetX
        {
            get;
            set;
        }

        /// <summary>
        /// Raises the <see cref="WidthChanged"/> event.
        /// </summary>
        protected void OnWidthChanged()
        {
            var temp = WidthChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MinWidthChanged"/> event.
        /// </summary>
        protected void OnMinWidthChanged()
        {
            var temp = MinWidthChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MaxWidthChanged"/> event.
        /// </summary>
        protected void OnMaxWidthChanged()
        {
            var temp = MaxWidthChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Width"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleWidthChanged(DependencyObject dobj)
        {
            var definition = (ColumnDefinition)dobj;
            definition.OnWidthChanged();
            definition.OnMeasureChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MinWidth"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMinWidthChanged(DependencyObject dobj)
        {
            var definition = (ColumnDefinition)dobj;
            definition.OnMinWidthChanged();
            definition.OnMeasureChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MaxWidth"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMaxWidthChanged(DependencyObject dobj)
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

        // Property values.
        private Double actualWidth;
    }
}
