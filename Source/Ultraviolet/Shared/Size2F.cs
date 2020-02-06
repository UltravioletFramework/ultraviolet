using System;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a two-dimensional area with single-precision floating point components.
    /// </summary>
    [Serializable]
    public partial struct Size2F : IEquatable<Size2F>, IInterpolatable<Size2F>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size2F"/> structure.
        /// </summary>
        /// <param name="width">The area's width.</param>
        /// <param name="height">The area's height.</param>
        [JsonConstructor]
        public Size2F(Single width, Single height)
        {
            this.Width = width;
            this.Height = height;
        }
        
        /// <summary>
        /// Adds a <see cref="Size2F"/> to another <see cref="Size2F"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size2F"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size2F"/> on the right side of the operator.</param>
        /// <returns>The result of adding the two instances.</returns>
        public static Size2F operator +(Size2F s1, Size2F s2)
        {
            Size2F result;

            result.Width = s1.Width + s2.Width;
            result.Height = s1.Height + s2.Height;

            return result;
        }

        /// <summary>
        /// Subtracts a <see cref="Size2F"/> from another <see cref="Size2F"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size2F"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size2F"/> on the right side of the operator.</param>
        /// <returns>The result of subtracting the two instances.</returns>
        public static Size2F operator -(Size2F s1, Size2F s2)
        {
            Size2F result;

            result.Width = s1.Width - s2.Width;
            result.Height = s1.Height - s2.Height;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Size2F"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size2F"/> which is the result of the muliplication.</returns>
        public static Size2F operator *(Size2F size, Int32 multiplier)
        {
            Size2F result;

            result.Width = size.Width * multiplier;
            result.Height = size.Height * multiplier;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Size2F"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size2F"/> which is the result of the muliplication.</returns>
        public static Size2F operator *(Size2F size, Single multiplier)
        {
            Size2F result;

            result.Width = size.Width * multiplier;
            result.Height = size.Height * multiplier;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Size2F"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size2D"/> which is the result of the muliplication.</returns>
        public static Size2D operator *(Size2F size, Double multiplier)
        {
            Size2D result;

            result.Width = size.Width * multiplier;
            result.Height = size.Height * multiplier;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Size2F"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size2F"/> which is the result of the muliplication.</returns>
        public static Size2F operator /(Size2F size, Int32 divisor)
        {
            Size2F result;

            result.Width = size.Width / divisor;
            result.Height = size.Height / divisor;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Size2F"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size2F"/> which is the result of the muliplication.</returns>
        public static Size2F operator /(Size2F size, Single divisor)
        {
            Size2F result;

            result.Width = size.Width / divisor;
            result.Height = size.Height / divisor;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Size2F"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size2D"/> which is the result of the muliplication.</returns>
        public static Size2D operator /(Size2F size, Double divisor)
        {
            Size2D result;

            result.Width = size.Width / divisor;
            result.Height = size.Height / divisor;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2F"/> structure to a <see cref="Vector2"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Vector2(Size2F size)
        {
            Vector2 result;

            result.X = size.Width;
            result.Y = size.Height;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2F"/> structure to a <see cref="Point2"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Point2(Size2F size)
        {
            Point2 result;

            result.X = (Int32)size.Width;
            result.Y = (Int32)size.Height;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2F"/> structure to a <see cref="Point2F"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Point2F(Size2F size)
        {
            Point2F result;

            result.X = size.Width;
            result.Y = size.Height;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2F"/> structure to a <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Point2D(Size2F size)
        {
            Point2D result;

            result.X = size.Width;
            result.Y = size.Height;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size2F"/> structure to a <see cref="Size2"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size2(Size2F size)
        {
            Size2 result;

            result.Width = (Int32)size.Width;
            result.Height = (Int32)size.Height;

            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="Size2F"/> structure to a <see cref="Size2D"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static implicit operator Size2D(Size2F size)
        {
            Size2D result;

            result.Width = size.Width;
            result.Height = size.Height;

            return result;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{Width:{Width} Height:{Height}}}";

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Size2F Interpolate(Size2F target, Single t)
        {
            Size2F result;

            result.Width = Tweening.Lerp(this.Width, target.Width, t);
            result.Height = Tweening.Lerp(this.Height, target.Height, t);

            return result;
        }

        /// <summary>
        /// Gets an area with zero width and height.
        /// </summary>
        public static Size2F Zero
        {
            get { return new Size2F(0, 0); }
        }

        /// <summary>
        /// Gets the size's total area (width times height).
        /// </summary>
        [JsonIgnore]
        public Single Area
        {
            get { return Width * Height; }
        }

        /// <summary>
        /// The area's width.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Single Width;

        /// <summary>
        /// The area's height.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Single Height;
    }
}
