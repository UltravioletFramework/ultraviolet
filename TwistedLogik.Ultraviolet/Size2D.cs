using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a two-dimensional area with double-precision floating point components.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{Width:{Width} Height:{Height}\}")]
    public struct Size2D : IEquatable<Size2D>, IInterpolatable<Size2D>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size2D"/> structure.
        /// </summary>
        /// <param name="width">The area's width.</param>
        /// <param name="height">The area's height.</param>
        [Preserve]
        [JsonConstructor]
        public Size2D(Double width, Double height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Compares two sizes for equality.
        /// </summary>
        /// <param name="s1">The first <see cref="Size2D"/> to compare.</param>
        /// <param name="s2">The second <see cref="Size2D"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified sizes are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Size2D s1, Size2D s2)
        {
            return s1.Equals(s2);
        }

        /// <summary>
        /// Compares two sizes for inequality.
        /// </summary>
        /// <param name="s1">The first <see cref="Size2D"/> to compare.</param>
        /// <param name="s2">The second <see cref="Size2D"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified sizes are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(Size2D s1, Size2D s2)
        {
            return !s1.Equals(s2);
        }

        /// <summary>
        /// Adds a <see cref="Size2D"/> to another <see cref="Size2D"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size2D"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size2D"/> on the right side of the operator.</param>
        /// <returns>The result of adding the two instances.</returns>
        [Preserve]
        public static Size2D operator +(Size2D s1, Size2D s2)
        {
            return new Size2D(s1.Width + s2.Width, s1.Height + s2.Height);
        }

        /// <summary>
        /// Subtracts a <see cref="Size2D"/> from another <see cref="Size2D"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size2D"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size2D"/> on the right side of the operator.</param>
        /// <returns>The result of subtracting the two instances.</returns>
        [Preserve]
        public static Size2D operator -(Size2D s1, Size2D s2)
        {
            return new Size2D(s1.Width - s2.Width, s1.Height - s2.Height);
        }

        /// <summary>
        /// Multiplies a <see cref="Size2D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size2D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2D operator *(Size2D size, Int32 multiplier)
        {
            return new Size2D(size.Width * multiplier, size.Height * multiplier);
        }

        /// <summary>
        /// Multiplies a <see cref="Size2D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size2D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2D operator *(Size2D size, Single multiplier)
        {
            return new Size2D(size.Width * multiplier, size.Height * multiplier);
        }

        /// <summary>
        /// Multiplies a <see cref="Size2D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size2D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2D operator *(Size2D size, Double multiplier)
        {
            return new Size2D(size.Width * multiplier, size.Height * multiplier);
        }

        /// <summary>
        /// Divides a <see cref="Size2D"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size2D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2D operator /(Size2D size, Int32 divisor)
        {
            return new Size2D(size.Width / divisor, size.Height / divisor);
        }

        /// <summary>
        /// Divides a <see cref="Size2D"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size2D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2D operator /(Size2D size, Single divisor)
        {
            return new Size2D(size.Width / divisor, size.Height / divisor);
        }

        /// <summary>
        /// Divides a <see cref="Size2D"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size2D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2D operator /(Size2D size, Double divisor)
        {
            return new Size2D(size.Width / divisor, size.Height / divisor);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2D"/> structure to a <see cref="Vector2"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Vector2(Size2D size)
        {
            return new Vector2((Single)size.Width, (Single)size.Height);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2D"/> structure to a <see cref="Point2"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Point2(Size2D size)
        {
            return new Point2((Int32)size.Width, (Int32)size.Height);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2D"/> structure to a <see cref="Point2F"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Point2F(Size2D size)
        {
            return new Point2F((Single)size.Width, (Single)size.Height);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2D"/> structure to a <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Point2D(Size2D size)
        {
            return new Point2D(size.Width, size.Height);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2D"/> structure to a <see cref="Size2"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Size2(Size2D size)
        {
            return new Size2((Int32)size.width, (Int32)size.height);
        }
        
        /// <summary>
        /// Explicitly converts a <see cref="Size2D"/> structure to a <see cref="Size2F"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Size2F(Size2D size)
        {
            return new Size2F((Single)size.width, (Single)size.height);
        }

        /// <summary>
        /// Converts the string representation of an area into an instance of the <see cref="Size2D"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="size">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Size2D size)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out size);
        }

        /// <summary>
        /// Converts the string representation of an area into an instance of the <see cref="Size2D"/> structure.
        /// </summary>
        /// <param name="s">A string containing an area to convert.</param>
        /// <returns>A instance of the <see cref="Size2D"/> structure equivalent to the area contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Size2D Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of an area into an instance of the <see cref="Size2D"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="size">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Size2D size)
        {
            size = default(Size2D);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 2)
                return false;

            Double width, height;
            if (!Double.TryParse(components[0], style, provider, out width))
                return false;
            if (!Double.TryParse(components[1], style, provider, out height))
                return false;

            size = new Size2D(width, height);
            return true;
        }

        /// <summary>
        /// Converts the string representation of an area into an instance of the <see cref="Size2D"/> structure.
        /// </summary>
        /// <param name="s">A string containing an area to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Size2D"/> structure equivalent to the area contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Size2D Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Size2D area;
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
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Size2D))
                return false;
            return Equals((Size2D)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(Size2D other)
        {
            return width == other.width && height == other.height;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        [Preserve]
        public Size2D Interpolate(Size2D target, Single t)
        {
            var width  = Tweening.Lerp(this.width, target.width, t);
            var height = Tweening.Lerp(this.height, target.height, t);
            return new Size2D(width, height);
        }

        /// <summary>
        /// Gets an area with zero width and height.
        /// </summary>
        public static Size2D Zero
        {
            get { return new Size2D(0, 0); }
        }

        /// <summary>
        /// Gets the area's width.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "width", Required = Required.Always)]
        public Double Width
        {
            get { return width; }
        }

        /// <summary>
        /// Gets the area's height.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "height", Required = Required.Always)]
        public Double Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets the size's total area (width times height).
        /// </summary>
        [JsonIgnore]
        public Double Area
        {
            get { return width * height; }
        }

        // Property values.
        private readonly Double width;
        private readonly Double height;
    }
}
