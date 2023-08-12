using System;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a line proceeding from a point in space.
    /// </summary>
    [Serializable]
    public partial struct Ray : IEquatable<Ray>, IInterpolatable<Ray>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ray"/> structure.
        /// </summary>
        /// <param name="position">The ray's position in space.</param>
        /// <param name="direction">The ray's direction vector.</param>
        [JsonConstructor]
        public Ray(Vector3 position, Vector3 direction)
        {
            this.Position = position;
            this.Direction = direction;
        }

        /// <summary>
        /// Determines whether this <see cref="Ray"/> intersects a specified <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane">The <see cref="Plane"/> to evaluate.</param>
        /// <returns>The distance along the ray at which it intersects the plane, or <see langword="null"/> if there is no intersection.</returns>
        public Single? Intersects(Plane plane)
        {
            var normalDotDirection = plane.Normal.X * Direction.X + plane.Normal.Y * Direction.Y + plane.Normal.Z * Direction.Z;
            if (MathUtil.IsApproximatelyZero(normalDotDirection))
                return null;

            var normalDotPosition = plane.Normal.X * Position.X + plane.Normal.Y * Position.Y + plane.Normal.Z * Position.Z;
            var distance = -(normalDotPosition + plane.D) / normalDotDirection;
            if (MathUtil.IsApproximatelyZero(distance))
                return 0f;

            if (distance < 0)
                return null;

            return distance;
        }

        /// <summary>
        /// Determines whether this <see cref="Ray"/> intersects a specified <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane">The <see cref="Plane"/> to evaluate.</param>
        /// <param name="result">The distance along the ray at which it intersects the plane, or <see langword="null"/> if there is no intersection.</param>
        public void Intersects(ref Plane plane, out Single? result)
        {
            var normalDotDirection = plane.Normal.X * Direction.X + plane.Normal.Y * Direction.Y + plane.Normal.Z * Direction.Z;
            if (MathUtil.IsApproximatelyZero(normalDotDirection))
            {
                result = null;
            }
            else
            {
                var normalDotPosition = plane.Normal.X * Position.X + plane.Normal.Y * Position.Y + plane.Normal.Z * Position.Z;
                var distance = -(normalDotPosition + plane.D) / normalDotDirection;
                if (MathUtil.IsApproximatelyZero(distance))
                {
                    result = 0f;
                }
                else
                {
                    if (distance < 0)
                    {
                        result = null;
                    }
                    else
                    {
                        result = distance;
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether this <see cref="Ray"/> intersects a specified <see cref="BoundingFrustum"/>.
        /// </summary>
        /// <param name="frustum">The <see cref="BoundingFrustum"/> to evaluate.</param>
        /// <returns>The distance along the ray at which it intersects the frustum, or <see langword="null"/> if there is no intersection.</returns>
        public Single? Intersects(BoundingFrustum frustum)
        {
            Contract.Require(frustum, nameof(frustum));

            frustum.Intersects(ref this, out Single? result);
            return result;
        }

        /// <summary>
        /// Determines whether this <see cref="Ray"/> intersects a specified <see cref="BoundingFrustum"/>.
        /// </summary>
        /// <param name="frustum">The <see cref="BoundingFrustum"/> to evaluate.</param>
        /// <param name="result">The distance along the ray at which it intersects the frustum, or <see langword="null"/> if there is no intersection.</param>
        public void Intersects(BoundingFrustum frustum, out Single? result)
        {
            Contract.Require(frustum, nameof(frustum));

            frustum.Intersects(ref this, out result);
        }

        /// <summary>
        /// Determines whether this <see cref="Ray"/> intersects a specified <see cref="BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere"/> to evaluate.</param>
        /// <returns>The distance along the ray at which it intersects the sphere, or <see langword="null"/> if there is no intersection.</returns>
        public Single? Intersects(BoundingSphere sphere)
        {
            Intersects(ref sphere, out Single? result);
            return result;
        }

        /// <summary>
        /// Determines whether this <see cref="Ray"/> intersects a specified <see cref="BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere"/> to evaluate.</param>
        /// <param name="result">The distance along the ray at which it intersects the sphere, or <see langword="null"/> if there is no intersection.</param>
        public void Intersects(ref BoundingSphere sphere, out Single? result)
        {
            var radiusSquared = sphere.Radius * sphere.Radius;

            var offset = sphere.Center - Position;
            var offsetLengthSquared = offset.LengthSquared();
            if (offsetLengthSquared < radiusSquared)
            {
                result = 0.0f;
                return;
            }

            Vector3.Dot(ref Direction, ref offset, out Single distanceToCenter);
            if (distanceToCenter < 0)
            {
                result = null;
                return;
            }

            var distanceToCenterSquared = distanceToCenter * distanceToCenter;
            var distanceToSphere = radiusSquared + distanceToCenterSquared - offsetLengthSquared;

            result = (distanceToSphere < 0) ? null : distanceToCenter - (Single?)Math.Sqrt(distanceToSphere);
        }

        /// <summary>
        /// Determines whether this <see cref="Ray"/> intersects a specified <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to evaluate.</param>
        /// <returns>The distance along the ray at which it intersects the sphere, or <see langword="null"/> if there is no intersection.</returns>
        public Single? Intersects(BoundingBox box)
        {
            Intersects(ref box, out Single? result);
            return result;
        }

        /// <summary>
        /// Determines whether this <see cref="Ray"/> intersects a specified <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to evaluate.</param>
        /// <param name="result">The distance along the ray at which it intersects the bounding box, or <see langword="null"/> if there is no intersection.</param>
        public void Intersects(ref BoundingBox box, out Single? result)
        {
            var min = default(Single?);
            var max = default(Single?);

            if (MathUtil.IsApproximatelyZero(Direction.X))
            {
                if (Position.X < box.Min.X || Position.X > box.Max.X)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                min = (box.Min.X - Position.X) / Direction.X;
                max = (box.Max.X - Position.X) / Direction.X;

                if (min > max)
                {
                    var temp = min;
                    min = max;
                    max = temp;
                }
            }

            if (MathUtil.IsApproximatelyZero(Direction.Y))
            {
                if (Position.Y < box.Min.Y || Position.Y > box.Max.Y)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                var minY = (box.Min.Y - Position.Y) / Direction.Y;
                var maxY = (box.Max.Y - Position.Y) / Direction.Y;

                if (minY > maxY)
                {
                    var temp = minY;
                    minY = maxY;
                    maxY = temp;
                }

                if (min == null || minY > min)
                    min = minY;

                if (max == null || maxY > max)
                    max = maxY;
            }

            if (MathUtil.IsApproximatelyZero(Direction.Z))
            {
                if (Position.Z < box.Min.Z || Position.Z > box.Max.Z)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                var minZ = (box.Min.Z - Position.Z) / Direction.Z;
                var maxZ = (box.Max.Z - Position.Z) / Direction.Z;

                if (minZ > maxZ)
                {
                    var temp = minZ;
                    minZ = maxZ;
                    maxZ = temp;
                }

                if (min == null || minZ > min)
                    min = minZ;

                if (max == null || maxZ > max)
                    max = maxZ;
            }

            if (min != null && min < 0 && max > 0)
            {
                result = 0;
                return;
            }

            if (min < 0)
            {
                result = null;
                return;
            }

            result = min;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{Position:{Position} Direction:{Direction}}}";

        /// <inheritdoc/>
        public Ray Interpolate(Ray target, Single t)
        {
            Ray result;

            result.Position = this.Position.Interpolate(target.Position, t);
            result.Direction = this.Direction.Interpolate(target.Direction, t);

            return result;
        }

        /// <summary>
        /// The ray's position in space.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The ray's direction vector.
        /// </summary>
        public Vector3 Direction;
    }
}
