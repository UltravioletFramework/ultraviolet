using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a three-dimensional size with integer components.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{Width:{Width} Height:{Height} Depth:{Depth}\}")]
    public struct Size3 : IEquatable<Size3>, IInterpolatable<Size3>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size3"/> structure.
        /// </summary>
        /// <param name="width">The area's width.</param>
        /// <param name="height">The area's height.</param>
        /// <param name="depth">The area's depth</param>
        [Preserve]
        [JsonConstructor]
        public Size3(Int32 width, Int32 height, Int32 depth)
        {
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
        }

        /// <summary>
        /// Compares two sizes for equality.
        /// </summary>
        /// <param name="s1">The first <see cref="Size3"/> to compare.</param>
        /// <param name="s2">The second <see cref="Size3"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified sizes are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Size3 s1, Size3 s2)
        {
            return s1.Equals(s2);
        }

        /// <summary>
        /// Compares two sizes for inequality.
        /// </summary>
        /// <param name="s1">The first <see cref="Size3"/> to compare.</param>
        /// <param name="s2">The second <see cref="Size3"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified sizes are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(Size3 s1, Size3 s2)
        {
            return !s1.Equals(s2);
        }

        /// <summary>
        /// Adds a <see cref="Size3"/> to another <see cref="Size3"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size3"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size3"/> on the right side of the operator.</param>
        /// <returns>The result of adding the two instances.</returns>
        [Preserve]
        public static Size3 operator +(Size3 s1, Size3 s2)
        {
            Size3 result;

            result.Width = s1.Width + s2.Width;
            result.Height = s1.Height + s2.Height;
            result.Depth = s1.Depth + s2.Depth;

            return result;
        }

        /// <summary>
        /// Subtracts a <see cref="Size3"/> from another <see cref="Size3"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size3"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size3"/> on the right side of the operator.</param>
        /// <returns>The result of subtracting the two instances.</returns>
        [Preserve]
        public static Size3 operator -(Size3 s1, Size3 s2)
        {
            Size3 result;

            result.Width = s1.Width - s2.Width;
            result.Height = s1.Height - s2.Height;
            result.Depth = s1.Depth - s2.Depth;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Size3"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size3"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3 operator *(Size3 size, Int32 multiplier)
        {
            Size3 result;

            result.Width = size.Width * multiplier;
            result.Height = size.Height * multiplier;
            result.Depth = size.Depth * multiplier;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Size3"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size3F"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3F operator *(Size3 size, Single multiplier)
        {
            Size3F result;

            result.Width = size.Width * multiplier;
            result.Height = size.Height * multiplier;
            result.Depth = size.Depth * multiplier;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Size3"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3D operator *(Size3 size, Double multiplier)
        {
            Size3D result;

            result.Width = size.Width * multiplier;
            result.Height = size.Height * multiplier;
            result.Depth = size.Depth * multiplier;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Size3"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size3"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3 operator /(Size3 size, Int32 divisor)
        {
            Size3 result;

            result.Width = size.Width / divisor;
            result.Height = size.Height / divisor;
            result.Depth = size.Depth / divisor;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Size3"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size3F"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3F operator /(Size3 size, Single divisor)
        {
            Size3F result;

            result.Width = size.Width / divisor;
            result.Height = size.Height / divisor;
            result.Depth = size.Depth / divisor;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Size3"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Size3D operator /(Size3 size, Double divisor)
        {
            Size3D result;

            result.Width = size.Width / divisor;
            result.Height = size.Height / divisor;
            result.Depth = size.Depth / divisor;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size3"/> structure to a <see cref="Vector3"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Vector3(Size3 size)
        {
            Vector3 result;

            result.X = size.Width;
            result.Y = size.Height;
            result.Z = size.Depth;

            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="Size3"/> structure to a <see cref="Size3F"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static implicit operator Size3F(Size3 size)
        {
            Size3F result;

            result.Width = size.Width;
            result.Height = size.Height;
            result.Depth = size.Depth;

            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="Size3"/> structure to a <see cref="Size3D"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static implicit operator Size3D(Size3 size)
        {
            Size3D result;

            result.Width = size.Width;
            result.Height = size.Height;
            result.Depth = size.Depth;

            return result;
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size3"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="size">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Size3 size)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out size);
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size3"/> structure.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <returns>A instance of the <see cref="Size3"/> structure equivalent to the size contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Size3 Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size3"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="size">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Size3 size)
        {
            size = default(Size3);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 3)
                return false;

            Int32 width, height, depth;
            if (!Int32.TryParse(components[0], style, provider, out width))
                return false;
            if (!Int32.TryParse(components[1], style, provider, out height))
                return false;
            if (!Int32.TryParse(components[2], style, provider, out depth))
                return false;

            size = new Size3(width, height, depth);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a size into an instance of the <see cref="Size3"/> structure.
        /// </summary>
        /// <param name="s">A string containing a size to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Size3"/> structure equivalent to the size contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Size3 Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Size3 size;
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
                hash = hash * 23 + Depth.GetHashCode();
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
            return String.Format(null, "{0} {1} {2}", Width, Height, Depth);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Size3))
                return false;
            return Equals((Size3)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(Size3 other)
        {
            return Width == other.Width && Height == other.Height && Depth == other.Depth;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        [Preserve]
        public Size3 Interpolate(Size3 target, Single t)
        {
            Size3 result;

            result.Width = Tweening.Lerp(this.Width, target.Width, t);
            result.Height = Tweening.Lerp(this.Height, target.Height, t);
            result.Depth = Tweening.Lerp(this.Depth, target.Depth, t);

            return result;
        }

        /// <summary>
        /// A size with zero width, height, and depth.
        /// </summary>
        public static Size3 Zero
        {
            get { return new Size3(0, 0, 0); }
        }

        /// <summary>
        /// Gets the size's total volume (width times height times depth).
        /// </summary>
        [JsonIgnore]
        public Int32 Volume
        {
            get { return Width * Height * Depth; }
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

        /// <summary>
        /// The size's depth.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "depth", Required = Required.Always)]
        public Int32 Depth;
    }
}
