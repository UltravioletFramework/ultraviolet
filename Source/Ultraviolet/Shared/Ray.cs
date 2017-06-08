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
        [Preserve]
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

        /// <inheritdoc/>
        public override String ToString() => $"{{Position:{Position} Direction:{Direction}}}";

        /// <inheritdoc/>
        [Preserve]
        public Ray Interpolate(Ray target, Single t)
        {
            Ray result;

            result.Position = Tweening.Lerp(this.Position, target.Position, t);
            result.Direction = Tweening.Lerp(this.Direction, target.Direction, t);

            return result;
        }

        /// <summary>
        /// The ray's position in space.
        /// </summary>
        [Preserve]
        [JsonProperty("position")]
        public Vector3 Position;

        /// <summary>
        /// The ray's direction vector.
        /// </summary>
        [Preserve]
        [JsonProperty("direction")]
        public Vector3 Direction;
    }
}
