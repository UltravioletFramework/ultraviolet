﻿using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the definition for a row in a <see cref="Grid"/> control.
    /// </summary>
    public class RowDefinition : DefinitionBase
    {
        /// <summary>
        /// Gets or sets the row's height in device independent pixels.
        /// </summary>
        public GridLength Height
        {
            get { return GetValue<GridLength>(HeightProperty); }
            set { SetValue<GridLength>(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the row's minimum height in device independent pixels.
        /// </summary>
        public Double MinHeight
        {
            get { return GetValue<Double>(MinHeightProperty); }
            set { SetValue<Double>(MinHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the row's maximum height in device independent pixels.
        /// </summary>
        public Double MaxHeight
        {
            get { return GetValue<Double>(MaxHeightProperty); }
            set { SetValue<Double>(MaxHeightProperty, value); }
        }

        /// <summary>
        /// Gets the row's actual height after measurement and arrangement.
        /// </summary>
        public Double ActualHeight
        {
            get { return Height.GridUnitType == GridUnitType.Auto ? MeasuredContentDimension : MeasuredDimension; }
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
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(GridLength), typeof(RowDefinition),
            new DependencyPropertyMetadata(HandleHeightChanged, () => new GridLength(1.0), DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="MinHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register("MinHeight", typeof(Double), typeof(RowDefinition),
            new DependencyPropertyMetadata(HandleMinHeightChanged, () => 0.0, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="MaxHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register("MaxHeight", typeof(Double), typeof(RowDefinition),
            new DependencyPropertyMetadata(HandleMaxHeightChanged, () => Double.PositiveInfinity, DependencyPropertyOptions.None));

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
        internal override Double FinalDimension
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
        protected virtual void OnHeightChanged()
        {
            var temp = HeightChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MinHeightChanged"/> event.
        /// </summary>
        protected virtual void OnMinHeightChanged()
        {
            var temp = MinHeightChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="MaxHeightChanged"/> event.
        /// </summary>
        protected virtual void OnMaxHeightChanged()
        {
            var temp = MaxHeightChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Height"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleHeightChanged(DependencyObject dobj)
        {
            var definition = (RowDefinition)dobj;
            definition.OnHeightChanged();
            definition.OnMeasureChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MinHeight"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMinHeightChanged(DependencyObject dobj)
        {
            var definition = (RowDefinition)dobj;
            definition.OnMinHeightChanged();
            definition.OnMeasureChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MaxHeight"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleMaxHeightChanged(DependencyObject dobj)
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
