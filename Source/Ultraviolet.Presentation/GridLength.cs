using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the length of a row or column in a grid.
    /// </summary>
    public partial struct GridLength : IEquatable<GridLength>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridLength"/> structure.
        /// </summary>
        /// <param name="value">The value of this instance.</param>
        public GridLength(Double value)
        {
            this.value        = value;
            this.gridUnitType = GridUnitType.Pixel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridLength"/> structure.
        /// </summary>
        /// <param name="value">The value of this instance.</param>
        /// <param name="gridUnitType">The grid length's unit type.</param>
        public GridLength(Double value, GridUnitType gridUnitType)
        {
            this.value        = value;
            this.gridUnitType = gridUnitType;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            switch (gridUnitType)
            {
                case GridUnitType.Auto:
                    return "Auto";
                case GridUnitType.Pixel:
                    return String.Format("{0}", Value);
                case GridUnitType.Star:
                    return String.Format("{0}*", Value);
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets an instance of the <see cref="GridLength"/> structure that holds a value whose
        /// size is determined by the size properties of the content object.
        /// </summary>
        public static GridLength Auto
        {
            get { return new GridLength(0.0, GridUnitType.Auto); }
        }

        /// <summary>
        /// Gets the grid length's associated unit type.
        /// </summary>
        public GridUnitType GridUnitType
        {
            get { return gridUnitType; }
        }

        /// <summary>
        /// Gets a value indicating whether this grid length's value is expressed in pixels.
        /// </summary>
        public Boolean IsAbsolute
        {
            get { return gridUnitType == GridUnitType.Pixel; }
        }

        /// <summary>
        /// Gets a value indicating whether this grid length's value is determined
        /// by the size of its content.
        /// </summary>
        public Boolean IsAuto
        {
            get { return gridUnitType == GridUnitType.Auto; }
        }

        /// <summary>
        /// Gets a value indicating whether this grid length's value is expressed
        /// as a weighted proportion of available space.
        /// </summary>
        public Boolean IsStar
        {
            get { return gridUnitType == GridUnitType.Star; }
        }

        /// <summary>
        /// Gets the grid length's value.
        /// </summary>
        public Double Value
        {
            get { return value; }
        }

        // Property values.
        private readonly Double value;
        private readonly GridUnitType gridUnitType;
    }
}
