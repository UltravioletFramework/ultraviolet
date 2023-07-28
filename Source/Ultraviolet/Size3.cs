using System;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a three-dimensional size with integer components.
    /// </summary>
    [Serializable]
    public partial struct Size3 : IEquatable<Size3>, IInterpolatable<Size3>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size3"/> structure.
        /// </summary>
        /// <param name="width">The area's width.</param>
        /// <param name="height">The area's height.</param>
        /// <param name="depth">The area's depth</param>
        [JsonConstructor]
        public Size3(Int32 width, Int32 height, Int32 depth)
        {
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
        }
        
        /// <summary>
        /// Adds a <see cref="Size3"/> to another <see cref="Size3"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size3"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size3"/> on the right side of the operator.</param>
        /// <returns>The result of adding the two instances.</returns>
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
        public static implicit operator Size3D(Size3 size)
        {
            Size3D result;

            result.Width = size.Width;
            result.Height = size.Height;
            result.Depth = size.Depth;

            return result;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{Width:{Width} Height:{Height} Depth:{Depth}}}";

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
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
        [JsonProperty(Required = Required.Always)]
        public Int32 Width;

        /// <summary>
        /// The size's height.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Int32 Height;

        /// <summary>
        /// The size's depth.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Int32 Depth;
    }
}
