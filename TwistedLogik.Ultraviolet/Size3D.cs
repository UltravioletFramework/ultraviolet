using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a three-dimensional size with double-precision floating point components.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{Width:{Width} Height:{Height} Depth:{Depth}\}")]
    public struct Size3D : IEquatable<Size3D>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size3D"/> structure.
        /// </summary>
        /// <param name="width">The area's width.</param>
        /// <param name="height">The area's height.</param>
        /// <param name="depth">The area's depth</param>
        [Preserve]
        [JsonConstructor]
        public Size3D(Double width, Double height, Double depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

        /// <summary>
        /// Compares two sizes for equality.
        /// </summary>
        /// <param name="s1">The first <see cref="Size3D"/> to compare.</param>
        /// <param name="s2">The second <see cref="Size3D"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified sizes are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Size3D s1, Size3D s2)
        {
            return s1.Equals(s2);
        }

        /// <summary>
        /// Compares two sizes for inequality.
        /// </summary>
        /// <param name="s1">The first <see cref="Size3D"/> to compare.</param>
        /// <param name="s2">The second <see cref="Size3D"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified sizes are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(Size3D s1, Size3D s2)
        {
            return !s1.Equals(s2);
        }

        /// <summary>
        /// Adds a <see cref="Size3D"/> to another <see cref="Size3D"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size3D"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size3D"/> on the right side of the operator.</param>
        /// <returns>The result of adding the two instances.</returns>
        [Preserve]
        public static Size3D operator +(Size3D s1, Size3D s2)
        {
            return new Size3D(s1.Width + s2.Width, s1.Height + s2.Height, s1.Depth + s2.Depth);
        }

        /// <summary>
        /// Subtracts a <see cref="Size3D"/> from another <see cref="Size3D"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size3D"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size3D"/> on the right side of the operator.</param>
        /// <returns>The result of subtracting the two instances.</returns>
        [Preserve]
        public static Size3D operator -(Size3D s1, Size3D s2)
        {
            return new Size3D(s1.Width - s2.Width, s1.Height - s2.Height, s1.Depth + s2.Depth);
        }

        /// <summary>
        /// Multiplies a <see cref="Size3D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3D operator *(Size3D size, Int32 multiplier)
        {
            return new Size3D(size.Width * multiplier, size.Height * multiplier, size.Depth * multiplier);
        }

        /// <summary>
        /// Multiplies a <see cref="Size3D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3D operator *(Size3D size, Single multiplier)
        {
            return new Size3D(size.Width * multiplier, size.Height * multiplier, size.Depth * multiplier);
        }

        /// <summary>
        /// Multiplies a <see cref="Size3D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3D operator *(Size3D size, Double multiplier)
        {
            return new Size3D(size.Width * multiplier, size.Height * multiplier, size.Depth * multiplier);
        }

        /// <summary>
        /// Divides a <see cref="Size3D"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3D operator /(Size3D size, Int32 divisor)
        {
            return new Size3D(size.Width / divisor, size.Height / divisor, size.Depth / divisor);
        }

        /// <summary>
        /// Divides a <see cref="Size3D"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3D operator /(Size3D size, Single divisor)
        {
            return new Size3D(size.Width / divisor, size.Height / divisor, size.Depth / divisor);
        }

        /// <summary>
        /// Divides a <see cref="Size3D"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3D operator /(Size3D size, Double divisor)
        {
            return new Size3D(size.Width / divisor, size.Height / divisor, size.Depth / divisor);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size3D"/> structure to a <see cref="Vector3"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Vector3(Size3D size)
        {
            return new Vector3((Single)size.Width, (Single)size.Height, (Single)size.Depth);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size3D"/> structure to a <see cref="Size3"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Size3(Size3D size)
        {
            return new Size3((Int32)size.width, (Int32)size.height, (Int32)size.depth);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size3D"/> structure to a <see cref="Size3F"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Size3F(Size3D size)
        {
            return new Size3F((Single)size.width, (Single)size.height, (Single)size.depth);
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size3D"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="size">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Size3D size)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out size);
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size3D"/> structure.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <returns>A instance of the <see cref="Size3D"/> structure equivalent to the size contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Size3D Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size3D"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="size">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Size3D size)
        {
            size = default(Size3D);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 3)
                return false;

            Double width, height, depth;
            if (!Double.TryParse(components[0], style, provider, out width))
                return false;
            if (!Double.TryParse(components[1], style, provider, out height))
                return false;
            if (!Double.TryParse(components[2], style, provider, out depth))
                return false;

            size = new Size3D(width, height, depth);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size3D"/> structure.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Size3D"/> structure equivalent to the size contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Size3D Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Size3D size;
            if (!TryParse(s, style, provider, out size))
                throw new FormatException();
            return size;
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
            return String.Format(provider, "{0} {1} {2}", width, height, depth);
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
                hash = hash * 23 + depth.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Size3D))
                return false;
            return Equals((Size3D)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(Size3D other)
        {
            return width == other.width && height == other.height && depth == other.depth;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        [Preserve]
        public Size3D Interpolate(Size3D target, Single t)
        {
            var width  = Tweening.Lerp(this.width, target.width, t);
            var height = Tweening.Lerp(this.height, target.height, t);
            var depth  = Tweening.Lerp(this.depth, target.depth, t);
            return new Size3D(width, height, depth);
        }

        /// <summary>
        /// A size with zero width, height, and depth.
        /// </summary>
        public static Size3D Zero
        {
            get { return new Size3D(0, 0, 0); }
        }

        /// <summary>
        /// Gets the size's width.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "width", Required = Required.Always)]
        public Double Width
        {
            get { return width; }
        }

        /// <summary>
        /// Gets the size's height.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "height", Required = Required.Always)]
        public Double Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets the size's depth.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "depth", Required = Required.Always)]
        public Double Depth
        {
            get { return depth; }
        }

        /// <summary>
        /// Gets the size's total volume (width times height times depth).
        /// </summary>
        [JsonIgnore]
        public Double Volume
        {
            get { return width * height * depth; }
        }

        // Property values.
        private readonly Double width;
        private readonly Double height;
        private readonly Double depth;
    }
}
