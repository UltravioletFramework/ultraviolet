using System;
using System.Diagnostics;
using System.Globalization;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a two-dimensional size with integer components.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{Width:{Width} Height:{Height}\}")]
    public struct Size2 : IEquatable<Size2>, IInterpolatable<Size2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size2"/> structure.
        /// </summary>
        /// <param name="width">The size's width.</param>
        /// <param name="height">The size's height.</param>
        public Size2(Int32 width, Int32 height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Compares two sizes for equality.
        /// </summary>
        /// <param name="s1">The first <see cref="Size2"/> to compare.</param>
        /// <param name="s2">The second <see cref="Size2"/> to compare.</param>
        /// <returns><c>true</c> if the specified sizes are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Size2 s1, Size2 s2)
        {
            return s1.Equals(s2);
        }

        /// <summary>
        /// Compares two sizes for inequality.
        /// </summary>
        /// <param name="s1">The first <see cref="Size2"/> to compare.</param>
        /// <param name="s2">The second <see cref="Size2"/> to compare.</param>
        /// <returns><c>true</c> if the specified sizes are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Size2 s1, Size2 s2)
        {
            return !s1.Equals(s2);
        }

        /// <summary>
        /// Implicitly converts a <see cref="Size2"/> structure to a <see cref="Size2F"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static implicit operator Size2F(Size2 size)
        {
            return new Size2F(size.width, size.height);
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size2"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="size">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out Size2 size)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out size);
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size2"/> structure.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <returns>A instance of the <see cref="Size2"/> structure equivalent to the size contained in <paramref name="s"/>.</returns>
        public static Size2 Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size2"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="size">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Size2 size)
        {
            size = default(Size2);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split(' ');
            if (components.Length != 2)
                return false;

            Int32 width, height;
            if (!Int32.TryParse(components[0], style, provider, out width))
                return false;
            if (!Int32.TryParse(components[1], style, provider, out height))
                return false;

            size = new Size2(width, height);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size2"/> structure.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Size2"/> structure equivalent to the size contained in <paramref name="s"/>.</returns>
        public static Size2 Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Size2 size;
            if (!TryParse(s, style, provider, out size))
                throw new FormatException();
            return size;
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
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Size2))
                return false;
            return Equals((Size2)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Size2 other)
        {
            return width == other.width && height == other.height;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Size2 Interpolate(Size2 target, Single t)
        {
            var width  = Tweening.Lerp(this.width, target.width, t);
            var height = Tweening.Lerp(this.height, target.height, t);
            return new Size2(width, height);
        }

        /// <summary>
        /// Gets a size with zero width and height.
        /// </summary>
        public static Size2 Zero
        {
            get { return new Size2(0, 0); }
        }

        /// <summary>
        /// Gets the size's width.
        /// </summary>
        public Int32 Width
        {
            get { return width; }
        }

        /// <summary>
        /// Gets the size's height.
        /// </summary>
        public Int32 Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets the size's total area (width times height).
        /// </summary>
        public Int32 Area
        {
            get { return width * height; }
        }

        // Property values.
        private readonly Int32 width;
        private readonly Int32 height;
    }
}
