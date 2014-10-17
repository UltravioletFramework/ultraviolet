using System;
using System.Diagnostics;
using System.Globalization;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a two-dimensional area with single-precision floating point components.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{Width:{Width} Height:{Height}\}")]
    public struct Size2F : IEquatable<Size2F>, IInterpolatable<Size2F>
    {
        /// <summary>
        /// Initializes a new instance of the Size2F structure.
        /// </summary>
        /// <param name="width">The area's width.</param>
        /// <param name="height">The area's height.</param>
        public Size2F(Single width, Single height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Compares two areas for equality.
        /// </summary>
        /// <param name="a1">The first area to compare.</param>
        /// <param name="a2">The second area to compare.</param>
        /// <returns>true if the specified areas are equal; otherwise, false.</returns>
        public static Boolean operator ==(Size2F a1, Size2F a2)
        {
            return a1.Equals(a2);
        }

        /// <summary>
        /// Compares two areas for inequality.
        /// </summary>
        /// <param name="a1">The first area to compare.</param>
        /// <param name="a2">The second area to compare.</param>
        /// <returns>true if the specified areas are unequal; otherwise, false.</returns>
        public static Boolean operator !=(Size2F a1, Size2F a2)
        {
            return !a1.Equals(a2);
        }

        /// <summary>
        /// Explicitly converts a Size2F structure to a Size2 structure.
        /// </summary>
        /// <param name="area">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size2(Size2F area)
        {
            return new Size2((int)area.width, (int)area.height);
        }

        /// <summary>
        /// Converts the string representation of an area into an instance of the Size2F structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="size">A variable to populate with the converted value.</param>
        /// <returns>true if <paramref name="s"/> was converted successfully; otherwise, false.</returns>
        public static Boolean TryParse(String s, out Size2F size)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out size);
        }

        /// <summary>
        /// Converts the string representation of an area into an instance of the Size2F structure.
        /// </summary>
        /// <param name="s">A string containing an area to convert.</param>
        /// <returns>A instance of the Size2 structure equivalent to the area contained in <paramref name="s"/>.</returns>
        public static Size2F Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of an area into an instance of the Size2F structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="style">A set of <see cref="System.Globalization.NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="size">A variable to populate with the converted value.</param>
        /// <returns>true if <paramref name="s"/> was converted successfully; otherwise, false.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Size2F size)
        {
            size = default(Size2F);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split(' ');
            if (components.Length != 2)
                return false;

            Single width, height;
            if (!Single.TryParse(components[0], style, provider, out width))
                return false;
            if (!Single.TryParse(components[1], style, provider, out height))
                return false;

            size = new Size2F(width, height);
            return true;
        }

        /// <summary>
        /// Converts the string representation of an area into an instance of the Size2F structure.
        /// </summary>
        /// <param name="s">A string containing an area to convert.</param>
        /// <param name="style">A set of <see cref="System.Globalization.NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the Size2 structure equivalent to the area contained in <paramref name="s"/>.</returns>
        public static Size2F Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Size2F area;
            if (!TryParse(s, style, provider, out area))
            {
                throw new FormatException();
            }
            return area;
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
                hash = hash * 23 + width.GetHashCode();
                hash = hash * 23 + height.GetHashCode();
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
            return String.Format(provider, "{0} {1}", width, height);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns>true if this instance is equal to the specified object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Size2F))
                return false;
            return Equals((Size2F)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns>true if this instance is equal to the specified object; otherwise, false.</returns>
        public Boolean Equals(Size2F other)
        {
            return width == other.width && height == other.height;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Size2F Interpolate(Size2F target, Single t)
        {
            var width  = Tweening.Lerp(this.width, target.width, t);
            var height = Tweening.Lerp(this.height, target.height, t);
            return new Size2F(width, height);
        }

        /// <summary>
        /// Gets an area with zero width and height.
        /// </summary>
        public static Size2F Zero
        {
            get { return new Size2F(0, 0); }
        }

        /// <summary>
        /// Gets the area's width.
        /// </summary>
        public Single Width
        {
            get { return width; }
        }

        /// <summary>
        /// Gets the area's height.
        /// </summary>
        public Single Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets the size's total area (width times height).
        /// </summary>
        public Single Area
        {
            get { return width * height; }
        }

        // Property values.
        private readonly Single width;
        private readonly Single height;
    }
}
