using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents the method that is called when the value of a property 
    /// changes on an instance of the <see cref="DefinitionBase"/> class.
    /// </summary>
    /// <param name="definition">The definition that raised the event.</param>
    public delegate void DefinitionEventHandler(DefinitionBase definition);

    /// <summary>
    /// Represents the base class for <see cref="RowDefinition"/> and <see cref="ColumnDefinition"/>.
    /// </summary>
    public abstract class DefinitionBase : DependencyObject
    {
        /// <summary>
        /// Resets the row or column's minimum content dimension.
        /// </summary>
        /// <param name="dimension">The value to which to reset the minimum content dimension.</param>
        internal void ResetContentDimension(Double dimension = 0)
        {
            MeasuredContentDimension = dimension;
        }

        /// <summary>
        /// Expands the row or columns's minimum content dimension to the specified dimension if
        /// it is smaller than that value.
        /// </summary>
        /// <param name="dimension">The dimension to which to expand the row or column's content.</param>
        internal void ExpandContentDimension(Double dimension)
        {
            if (dimension > MeasuredContentDimension)
            {
                MeasuredContentDimension = dimension;
            }
        }

        /// <summary>
        /// Gets or sets the definition's assumed unit type for purposes of measurement (which may be different
        /// than the unit type of its <see cref="Dimension"/> property).
        /// </summary>
        internal GridUnitType AssumedUnitType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the row or column's relevant user-specified dimension.
        /// </summary>
        internal abstract GridLength Dimension
        {
            get;
        }

        /// <summary>
        /// Gets the row or column's minimum dimension.
        /// </summary>
        internal abstract Double MinDimension
        {
            get;
        }

        /// <summary>
        /// Gets the row or column's maximum dimension.
        /// </summary>
        internal abstract Double MaxDimension
        {
            get;
        }

        /// <summary>
        /// Gets or sets the row or column's measured dimension.
        /// </summary>
        internal abstract Double MeasuredDimension
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the measured dimension of the row or column's content.
        /// </summary>
        internal abstract Double MeasuredContentDimension
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the row or column's actual dimension after measurement.
        /// </summary>
        internal abstract Double ActualDimension
        {
            get;
        }

        /// <summary>
        /// Gets or sets the row or column's final arranged position within the grid.
        /// </summary>
        internal abstract Double Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the preferred desired dimension of this row or column based on its current parameters.
        /// </summary>
        internal Double PreferredDesiredDimension
        {
            get
            {
                if (AssumedUnitType != GridUnitType.Auto)
                {
                    return Math.Max(MeasuredContentDimension, MeasuredDimension);
                }
                return MeasuredContentDimension;
            }
        }

        /// <summary>
        /// Gets the minimum desired dimension of this row or column based on its current parameters.
        /// </summary>
        internal Double MinimumDesiredDimension
        {
            get
            {
                return MeasuredContentDimension;
            }
        }

        /// <summary>
        /// Gets the <see cref="Grid"/> that owns the definition.
        /// </summary>
        internal Grid Grid
        {
            get;
            set;
        }
    }
}
