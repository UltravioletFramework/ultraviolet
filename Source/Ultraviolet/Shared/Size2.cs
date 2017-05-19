using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
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
        [Preserve]
        [JsonConstructor]
        public Size2(Int32 width, Int32 height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Compares two sizes for equality.
        /// </summary>
        /// <param name="s1">The first <see cref="Size2"/> to compare.</param>
        /// <param name="s2">The second <see cref="Size2"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified sizes are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Size2 s1, Size2 s2)
        {
            return s1.Equals(s2);
        }

        /// <summary>
        /// Compares two sizes for inequality.
        /// </summary>
        /// <param name="s1">The first <see cref="Size2"/> to compare.</param>
        /// <param name="s2">The second <see cref="Size2"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified sizes are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(Size2 s1, Size2 s2)
        {
            return !s1.Equals(s2);
        }

        /// <summary>
        /// Adds a <see cref="Size2"/> to another <see cref="Size2"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size2"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size2"/> on the right side of the operator.</param>
        /// <returns>The result of adding the two instances.</returns>
        [Preserve]
        public static Size2 operator +(Size2 s1, Size2 s2)
        {
            return new Size2(s1.Width + s2.Width, s1.Height + s2.Height);
        }

        /// <summary>
        /// Subtracts a <see cref="Size2"/> from another <see cref="Size2"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size2"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size2"/> on the right side of the operator.</param>
        /// <returns>The result of subtracting the two instances.</returns>
        [Preserve]
        public static Size2 operator -(Size2 s1, Size2 s2)
        {
            return new Size2(s1.Width - s2.Width, s1.Height - s2.Height);
        }

        /// <summary>
        /// Multiplies a <see cref="Size2"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size2"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2 operator *(Size2 size, Int32 multiplier)
        {
            return new Size2(size.Width * multiplier, size.Height * multiplier);
        }

        /// <summary>
        /// Multiplies a <see cref="Size2"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size2F"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2F operator *(Size2 size, Single multiplier)
        {
            return new Size2F(size.Width * multiplier, size.Height * multiplier);
        }

        /// <summary>
        /// Multiplies a <see cref="Size2"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size2D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2D operator *(Size2 size, Double multiplier)
        {
            return new Size2D(size.Width * multiplier, size.Height * multiplier);
        }

        /// <summary>
        /// Divides a <see cref="Size2"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size2"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2 operator /(Size2 size, Int32 divisor)
        {
            return new Size2(size.Width / divisor, size.Height / divisor);
        }

        /// <summary>
        /// Divides a <see cref="Size2"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size2F"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2F operator /(Size2 size, Single divisor)
        {
            return new Size2F(size.Width / divisor, size.Height / divisor);
        }

        /// <summary>
        /// Divides a <see cref="Size2"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size2D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size2D operator /(Size2 size, Double divisor)
        {
            return new Size2D(size.Width / divisor, size.Height / divisor);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2"/> structure to a <see cref="Vector2"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Vector2(Size2 size)
        {
            return new Vector2(size.Width, size.Height);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2"/> structure to a <see cref="Point2"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Point2(Size2 size)
        {
            return new Point2(size.Width, size.Height);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2"/> structure to a <see cref="Point2F"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Point2F(Size2 size)
        {
            return new Point2F(size.Width, size.Height);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2"/> structure to a <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Point2D(Size2 size)
        {
            return new Point2D(size.Width, size.Height);
        }

        /// <summary>
        /// Implicitly converts a <see cref="Size2"/> structure to a <see cref="Size2F"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static implicit operator Size2F(Size2 size)
        {
            return new Size2F(size.Width, size.Height);
        }

        /// <summary>
        /// Implicitly converts a <see cref="Size2"/> structure to a <see cref="Size2D"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static implicit operator Size2D(Size2 size)
        {
            return new Size2D(size.Width, size.Height);
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size2"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="size">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Size2 size)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out size);
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size2"/> structure.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <returns>A instance of the <see cref="Size2"/> structure equivalent to the size contained in <paramref name="s"/>.</returns>
        [Preserve]
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
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Size2 size)
        {
            size = default(Size2);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
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
        [Preserve]
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
                hash = hash * 23 + Width.GetHashCode();
                hash = hash * 23 + Height.GetHashCode();
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
            return String.Format(provider, "{0} {1}", Width, Height);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
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
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(Size2 other)
        {
            return Width == other.Width && Height == other.Height;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        [Preserve]
        public Size2 Interpolate(Size2 target, Single t)
        {
            var width  = Tweening.Lerp(this.Width, target.Width, t);
            var height = Tweening.Lerp(this.Height, target.Height, t);
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
        /// Gets the size's total area (width times height).
        /// </summary>
        [JsonIgnore]
        public Int32 Area
        {
            get { return Width * Height; }
        }

        /// <summary>
        /// The size's width.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "width", Required = Required.Always)]
        public Int32 Width;

        /// <summary>
        /// The size's height.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "height", Required = Required.Always)]
        public Int32 Height;
    }
}
