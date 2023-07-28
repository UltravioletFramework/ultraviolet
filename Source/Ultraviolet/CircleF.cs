using System;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a circle with single-precision floating point radius and position.
    /// </summary>
    [Serializable]
    public partial struct CircleF : IEquatable<CircleF>, IInterpolatable<CircleF>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircleF"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</param>
        /// <param name="radius">The circle's radius.</param>
        [JsonConstructor]
        public CircleF(Single x, Single y, Single radius)
        {
            this.X = x;
            this.Y = y;
            this.Radius = radius;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleF"/> structure.
        /// </summary>
        /// <param name="position">The position of the circle's center.</param>
        /// <param name="radius">The circle's radius.</param>
        public CircleF(Point2F position, Single radius)
            : this(position.X, position.Y, radius)
        {

        }
        
        /// <summary>
        /// Offsets the specified <see cref="CircleF"/> by adding the specified <see cref="Point2F"/> to its location.
        /// </summary>
        /// <param name="circle">The <see cref="CircleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="CircleF"/> that has been offset by the specified amount.</returns>
        public static CircleF operator +(CircleF circle, Point2 point)
        {
            CircleF result;

            result.X = circle.X + point.X;
            result.Y = circle.Y + point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="CircleF"/> by subtracting the specified <see cref="Point2F"/> from its location.
        /// </summary>
        /// <param name="circle">The <see cref="CircleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="CircleF"/> that has been offset by the specified amount.</returns>
        public static CircleF operator -(CircleF circle, Point2 point)
        {
            CircleF result;

            result.X = circle.X - point.X;
            result.Y = circle.Y - point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="CircleF"/> by adding the specified <see cref="Point2F"/> to its location.
        /// </summary>
        /// <param name="circle">The <see cref="CircleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="CircleF"/> that has been offset by the specified amount.</returns>
        public static CircleF operator +(CircleF circle, Point2F point)
        {
            CircleF result;

            result.X = circle.X + point.X;
            result.Y = circle.Y + point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="CircleF"/> by subtracting the specified <see cref="Point2F"/> from its location.
        /// </summary>
        /// <param name="circle">The <see cref="CircleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="CircleF"/> that has been offset by the specified amount.</returns>
        public static CircleF operator -(CircleF circle, Point2F point)
        {
            CircleF result;

            result.X = circle.X - point.X;
            result.Y = circle.Y - point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="CircleF"/> by adding the specified <see cref="Point2D"/> to its location.
        /// </summary>
        /// <param name="circle">The <see cref="CircleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="CircleD"/> that has been offset by the specified amount.</returns>
        public static CircleD operator +(CircleF circle, Point2D point)
        {
            CircleD result;

            result.X = circle.X + point.X;
            result.Y = circle.Y + point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="RectangleF"/> by subtracting the specified <see cref="Point2D"/> from its location.
        /// </summary>
        /// <param name="circle">The <see cref="RectangleF"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the rectangle.</param>
        /// <returns>A <see cref="CircleD"/> that has been offset by the specified amount.</returns>
        public static CircleD operator -(CircleF circle, Point2D point)
        {
            CircleD result;

            result.X = circle.X - point.X;
            result.Y = circle.Y - point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="CircleF"/> structure to a <see cref="Circle"/> structure.
        /// </summary>
        /// <param name="circle">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Circle(CircleF circle)
        {
            Circle result;

            result.X = (Int32)circle.X;
            result.Y = (Int32)circle.Y;
            result.Radius = (Int32)circle.Radius;

            return result;
        }
        
        /// <summary>
        /// Implicitly converts a <see cref="CircleF"/> structure to a <see cref="CircleD"/> structure.
        /// </summary>
        /// <param name="circle">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static implicit operator CircleD(CircleF circle)
        {
            CircleD result;

            result.X = circle.X;
            result.Y = circle.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{Position:{Position} Radius:{Radius}}}";

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public CircleF Interpolate(CircleF target, Single t)
        {
            CircleF result;

            result.X = Tweening.Lerp(this.X, target.X, t);
            result.Y = Tweening.Lerp(this.Y, target.Y, t);
            result.Radius = Tweening.Lerp(this.Radius, target.Radius, t);

            return result;
        }

        /// <summary>
        /// Gets an instance which represents the unit circle.
        /// </summary>
        public static CircleF Unit
        {
            get { return new CircleF(0, 0, 1f); }
        }

        /// <summary>
        /// Gets the circle's position.
        /// </summary>
        [JsonIgnore]
        public Point2F Position
        {
            get { return new Point2F(X, Y); }
        }

        /// <summary>
        /// Gets the x-coordinate of the circle's center.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Single X;

        /// <summary>
        /// Gets the y-coordinate of the circle's center.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Single Y;

        /// <summary>
        /// Gets the circle's radius.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Single Radius;
    }
}
