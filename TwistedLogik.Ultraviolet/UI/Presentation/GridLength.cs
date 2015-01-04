using System;
using System.Globalization;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the length of a row or column in a grid.
    /// </summary>
    public struct GridLength : IEquatable<GridLength>
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

        /// <summary>
        /// Compares two <see cref="GridLength"/> values for equality.
        /// </summary>
        /// <param name="gl1">The first <see cref="GridLength"/> to compare.</param>
        /// <param name="gl2">The second <see cref="GridLength"/> to compare.</param>
        /// <returns><c>true</c> if the specified grid lengths are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(GridLength gl1, GridLength gl2)
        {
            return gl1.Equals(gl2);
        }

        /// <summary>
        /// Compares two <see cref="GridLength"/> values for inequality.
        /// </summary>
        /// <param name="gl1">The first <see cref="GridLength"/> to compare.</param>
        /// <param name="gl2">The second <see cref="GridLength"/> to compare.</param>
        /// <returns><c>true</c> if the specified grid lengths are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(GridLength gl1, GridLength gl2)
        {
            return !gl1.Equals(gl2);
        }

        /// <summary>
        /// Converts the string representation of a grid length into an instance of the <see cref="GridLength"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a grid length to convert.</param>
        /// <param name="gridLength">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out GridLength gridLength)
        {
            return TryParse(s, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out gridLength);
        }

        /// <summary>
        /// Converts the string representation of a grid length into an instance of the <see cref="GridLength"/> structure.
        /// </summary>
        /// <param name="s">A string containing a grid length to convert.</param>
        /// <returns>A instance of the <see cref="GridLength"/> structure equivalent to the grid length contained in <paramref name="s"/>.</returns>
        public static GridLength Parse(String s)
        {
            return Parse(s, NumberStyles.Float, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a grid length into an instance of the <see cref="GridLength"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a grid length to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="gridLength">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out GridLength gridLength)
        {
            Contract.Require(s, "s");

            gridLength = default(GridLength);

            if (String.Equals("Auto", s, StringComparison.InvariantCultureIgnoreCase))
            {
                gridLength = GridLength.Auto;
                return true;
            }

            var value  = 1.0;
            var isStar = s.EndsWith("*");
            if (isStar)
            {
                var valuePart = s.Substring(0, s.Length - 1);
                if (valuePart.Length > 0)
                {
                    if (!Double.TryParse(valuePart, out value))
                        return false;
                }
                gridLength = new GridLength(value, GridUnitType.Star);
                return true;
            }

            if (!Double.TryParse(s, out value))
                return false;

            gridLength = new GridLength(value);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a grid length into an instance of the <see cref="GridLength"/> structure.
        /// </summary>
        /// <param name="s">A string containing a grid length to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="GridLength"/> structure equivalent to the grid length contained in <paramref name="s"/>.</returns>
        public static GridLength Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            GridLength thickness;
            if (!TryParse(s, style, provider, out thickness))
            {
                throw new FormatException();
            }
            return thickness;
        }

        /// <summary>
        /// Gets the object's hash code.
        /// </summary>
        /// <returns>The object's hash code.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + value.GetHashCode();
                hash = hash * 23 + gridUnitType.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return ToString(null);
        }

        /// <summary>
        /// Converts the object to a human-readable string using the specified culture information.
        /// </summary>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A human-readable string that represents the object.</returns>
        public String ToString(IFormatProvider provider)
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
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is GridLength))
                return false;
            return Equals((GridLength)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(GridLength other)
        {
            return this.value == other.value && this.gridUnitType == other.gridUnitType;
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
