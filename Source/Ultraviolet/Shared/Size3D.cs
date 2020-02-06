using System;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a three-dimensional size with double-precision floating point components.
    /// </summary>
    [Serializable]
    public partial struct Size3D : IEquatable<Size3D>, IInterpolatable<Size3D>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size3D"/> structure.
        /// </summary>
        /// <param name="width">The area's width.</param>
        /// <param name="height">The area's height.</param>
        /// <param name="depth">The area's depth</param>
        [JsonConstructor]
        public Size3D(Double width, Double height, Double depth)
        {
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
        }
        
        /// <summary>
        /// Adds a <see cref="Size3D"/> to another <see cref="Size3D"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size3D"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size3D"/> on the right side of the operator.</param>
        /// <returns>The result of adding the two instances.</returns>
        public static Size3D operator +(Size3D s1, Size3D s2)
        {
            Size3D result;

            result.Width = s1.Width + s2.Width;
            result.Height = s1.Height + s2.Height;
            result.Depth = s1.Depth + s2.Depth;

            return result;
        }

        /// <summary>
        /// Subtracts a <see cref="Size3D"/> from another <see cref="Size3D"/>.
        /// </summary>
        /// <param name="s1">The <see cref="Size3D"/> on the left side of the operator.</param>
        /// <param name="s2">The <see cref="Size3D"/> on the right side of the operator.</param>
        /// <returns>The result of subtracting the two instances.</returns>
        public static Size3D operator -(Size3D s1, Size3D s2)
        {
            Size3D result;

            result.Width = s1.Width - s2.Width;
            result.Height = s1.Height - s2.Height;
            result.Depth = s1.Depth - s2.Depth;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Size3D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        public static Size3D operator *(Size3D size, Int32 multiplier)
        {
            Size3D result;

            result.Width = size.Width * multiplier;
            result.Height = size.Height * multiplier;
            result.Depth = size.Depth * multiplier;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Size3D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        public static Size3D operator *(Size3D size, Single multiplier)
        {
            Size3D result;

            result.Width = size.Width * multiplier;
            result.Height = size.Height * multiplier;
            result.Depth = size.Depth * multiplier;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Size3D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="size">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        public static Size3D operator *(Size3D size, Double multiplier)
        {
            Size3D result;

            result.Width = size.Width * multiplier;
            result.Height = size.Height * multiplier;
            result.Depth = size.Depth * multiplier;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Size3D"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        public static Size3D operator /(Size3D size, Int32 divisor)
        {
            Size3D result;

            result.Width = size.Width / divisor;
            result.Height = size.Height / divisor;
            result.Depth = size.Depth / divisor;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Size3D"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        public static Size3D operator /(Size3D size, Single divisor)
        {
            Size3D result;

            result.Width = size.Width / divisor;
            result.Height = size.Height / divisor;
            result.Depth = size.Depth / divisor;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Size3D"/> by a scalar divisor.
        /// </summary>
        /// <param name="size">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Size3D"/> which is the result of the muliplication.</returns>
        public static Size3D operator /(Size3D size, Double divisor)
        {
            Size3D result;

            result.Width = size.Width / divisor;
            result.Height = size.Height / divisor;
            result.Depth = size.Depth / divisor;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size3D"/> structure to a <see cref="Vector3"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Vector3(Size3D size)
        {
            Vector3 result;

            result.X = (Single)size.Width;
            result.Y = (Single)size.Height;
            result.Z = (Single)size.Depth;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size3D"/> structure to a <see cref="Size3"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size3(Size3D size)
        {
            Size3 result;

            result.Width = (Int32)size.Width;
            result.Height = (Int32)size.Height;
            result.Depth = (Int32)size.Depth;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Size3D"/> structure to a <see cref="Size3F"/> structure.
        /// </summary>
        /// <param name="size">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size3F(Size3D size)
        {
            Size3F result;

            result.Width = (Single)size.Width;
            result.Height = (Single)size.Height;
            result.Depth = (Single)size.Depth;

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
        public Size3D Interpolate(Size3D target, Single t)
        {
            Size3D result;

            result.Width = Tweening.Lerp(this.Width, target.Width, t);
            result.Height = Tweening.Lerp(this.Height, target.Height, t);
            result.Depth = Tweening.Lerp(this.Depth, target.Depth, t);

            return result;
        }

        /// <summary>
        /// A size with zero width, height, and depth.
        /// </summary>
        public static Size3D Zero
        {
            get { return new Size3D(0, 0, 0); }
        }

        /// <summary>
        /// Gets the size's total volume (width times height times depth).
        /// </summary>
        [JsonIgnore]
        public Double Volume
        {
            get { return Width * Height * Depth; }
        }

        /// <summary>
        /// The size's width.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Double Width;

        /// <summary>
        /// The size's height.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Double Height;

        /// <summary>
        /// The size's depth.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Double Depth;
    }
}
