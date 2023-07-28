using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a bounding sphere.
    /// </summary>
    [Serializable]
    public partial struct BoundingSphere : IEquatable<BoundingSphere>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingSphere"/> structure.
        /// </summary>
        /// <param name="center">The sphere's center position.</param>
        /// <param name="radius">The sphere's radius.</param>
        public BoundingSphere(Vector3 center, Single radius)
        {
            Contract.EnsureRange(radius >= 0, nameof(radius));

            this.Center = center;
            this.Radius = radius;
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere"/> by merging two existing bounding spheres.
        /// </summary>
        /// <param name="original">The first <see cref="BoundingSphere"/> to merge.</param>
        /// <param name="additional">The second <see cref="BoundingSphere"/> to merge.></param>
        /// <returns name="result">The merged <see cref="BoundingSphere"/> which was created.</returns>
        public static BoundingSphere CreateMerged(BoundingSphere original, BoundingSphere additional)
        {
            CreateMerged(ref original, ref additional, out BoundingSphere result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere"/> by merging two existing bounding spheres.
        /// </summary>
        /// <param name="original">The first <see cref="BoundingSphere"/> to merge.</param>
        /// <param name="additional">The second <see cref="BoundingSphere"/> to merge.></param>
        /// <param name="result">The merged <see cref="BoundingSphere"/> which was created.</param>
        public static void CreateMerged(ref BoundingSphere original, ref BoundingSphere additional, out BoundingSphere result)
        {
            Vector3.Subtract(ref additional.Center, ref original.Center, out Vector3 offset);

            var distance = offset.Length();
            if (original.Radius + additional.Radius >= distance)
            {
                if (distance <= original.Radius - additional.Radius)
                {
                    result = original;
                    return;
                }

                if (distance <= additional.Radius - original.Radius)
                {
                    result = additional;
                    return;
                }
            }

            var normalizedOffset = offset * (1.0f / distance);
            var min = Math.Min(-original.Radius, distance - additional.Radius);
            var max = (Math.Max(original.Radius, distance + additional.Radius) - min) * 0.5f;

            result.Center = original.Center + normalizedOffset * (max + min);
            result.Radius = max;
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere"/> which encompasses all of the points in the specified collection.
        /// </summary>
        /// <param name="points">The collection of points from which to create the bounding sphere.</param>
        /// <returns>The <see cref="BoundingSphere"/> which was created.</returns>
        public static BoundingSphere CreateFromPoints(IEnumerable<Vector3> points)
        {
            CreateFromPoints(points, out BoundingSphere result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere"/> which encompasses all of the points in the specified collection.
        /// </summary>
        /// <param name="points">The collection of points from which to create the bounding sphere.</param>
        /// <param name="result">The <see cref="BoundingSphere"/> which was created.</param>
        public static void CreateFromPoints(IEnumerable<Vector3> points, out BoundingSphere result)
        {
            Contract.Require(points, nameof(points));
            
            var enumerator = points.GetEnumerator();
            var empty = !enumerator.MoveNext();
            if (empty)
                throw new ArgumentException(nameof(points));

            var minX = enumerator.Current;
            var maxX = enumerator.Current;
            var minY = enumerator.Current;
            var maxY = enumerator.Current;
            var minZ = enumerator.Current;
            var maxZ = enumerator.Current;

            foreach (var point in points)
            {
                if (point.X < minX.X)
                    minX = point;

                if (point.X > maxX.X)
                    maxX = point;

                if (point.Y < minY.Y)
                    minY = point;

                if (point.Y > maxY.Y)
                    maxY = point;

                if (point.Z < minZ.Z)
                    minZ = point;

                if (point.Z > maxZ.Z)
                    maxZ = point;
            }

            Vector3.Distance(ref minX, ref maxX, out Single diameterX);
            Vector3.Distance(ref minY, ref maxY, out Single diameterY);
            Vector3.Distance(ref minZ, ref maxZ, out Single diameterZ);

            var center = default(Vector3);
            var radius = default(Single);

            if (diameterX > diameterY && diameterX > diameterZ)
            {
                Vector3.Lerp(ref minX, ref minY, 0.5f, out center);
                radius = diameterX * 0.5f;
            }
            else
            {
                if (diameterY > diameterZ)
                {
                    Vector3.Lerp(ref minY, ref maxY, 0.5f, out center);
                    radius = diameterY * 0.5f;
                }
                else
                {
                    Vector3.Lerp(ref minZ, ref maxZ, 0.5f, out center);
                    radius = diameterZ * 0.5f;
                }
            }

            foreach (var point in points)
            {
                var pointRelativeToCenter = default(Vector3);
                pointRelativeToCenter.X = point.X - center.X;
                pointRelativeToCenter.Y = point.Y - center.Y;
                pointRelativeToCenter.Z = point.Z - center.Z;

                var pointDistanceToCenter = pointRelativeToCenter.Length();
                if (pointDistanceToCenter > radius)
                {
                    radius = pointDistanceToCenter;
                    center = center + ((1.0f - radius / pointDistanceToCenter) * pointRelativeToCenter);
                }
            }

            result.Center = center;
            result.Radius = radius;
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere"/> which encompasses all of the points in the specified collection.
        /// </summary>
        /// <param name="points">The collection of points from which to create the bounding sphere.</param>
        /// <returns>The <see cref="BoundingSphere"/> which was created.</returns>
        public static BoundingSphere CreateFromPoints(IList<Vector3> points)
        {
            CreateFromPoints(points, out BoundingSphere result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere"/> which encompasses all of the points in the specified collection.
        /// </summary>
        /// <param name="points">The collection of points from which to create the bounding sphere.</param>
        /// <param name="result">The <see cref="BoundingSphere"/> which was created.</param>
        public static void CreateFromPoints(IList<Vector3> points, out BoundingSphere result)
        {
            Contract.Require(points, nameof(points));

            if (points.Count == 0)
                throw new ArgumentException(nameof(points));

            var minX = points[0];
            var maxX = minX;
            var minY = minX;
            var maxY = minX;
            var minZ = minX;
            var maxZ = minX;

            for (int i = 1; i < points.Count; i++)
            {
                var point = points[i];

                if (point.X < minX.X)
                    minX = point;

                if (point.X > maxX.X)
                    maxX = point;

                if (point.Y < minY.Y)
                    minY = point;

                if (point.Y > maxY.Y)
                    maxY = point;

                if (point.Z < minZ.Z)
                    minZ = point;

                if (point.Z > maxZ.Z)
                    maxZ = point;
            }

            Vector3.Distance(ref minX, ref maxX, out Single diameterX);
            Vector3.Distance(ref minY, ref maxY, out Single diameterY);
            Vector3.Distance(ref minZ, ref maxZ, out Single diameterZ);

            var center = default(Vector3);
            var radius = default(Single);
            
            if (diameterX > diameterY && diameterX > diameterZ)
            {
                Vector3.Lerp(ref minX, ref minY, 0.5f, out center);
                radius = diameterX * 0.5f;
            }
            else
            {
                if (diameterY > diameterZ)
                {
                    Vector3.Lerp(ref minY, ref maxY, 0.5f, out center);
                    radius = diameterY * 0.5f;
                }
                else
                {
                    Vector3.Lerp(ref minZ, ref maxZ, 0.5f, out center);
                    radius = diameterZ * 0.5f;
                }
            }

            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];

                Vector3.Subtract(ref point, ref center, out Vector3 pointRelativeToCenter);

                var pointDistanceToCenter = pointRelativeToCenter.Length();
                if (pointDistanceToCenter > radius)
                {
                    radius = (radius + pointDistanceToCenter) * 0.5f;
                    center = center + ((1.0f - radius / pointDistanceToCenter) * pointRelativeToCenter);
                }
            }

            result.Center = center;
            result.Radius = radius;
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere"/> which encompasses the specified frustum.
        /// </summary>
        /// <param name="frustum">The frustum from which to create the bounding sphere.</param>
        /// <returns>The <see cref="BoundingSphere"/> which was created.</returns>
        public static BoundingSphere CreateFromFrustum(BoundingFrustum frustum)
        {
            CreateFromPoints(frustum.CornersInternal, out BoundingSphere result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere"/> which encompasses the specified frustum.
        /// </summary>
        /// <param name="frustum">The frustum from which to create the bounding sphere.</param>
        /// <param name="result">The <see cref="BoundingSphere"/> which was created.</param>
        public static void CreateFromFrustum(BoundingFrustum frustum, out BoundingSphere result)
        {
            CreateFromPoints(frustum.CornersInternal, out result);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{Center:{Center} Radius:{Radius}}}";

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> contains the specified point.
        /// </summary>
        /// <param name="point">A <see cref="Vector3"/> which represents the point to evaluate.</param>
        /// <returns>A <see cref="ContainmentType"/> value representing the relationship between this sphere and the evaluated point.</returns>
        public ContainmentType Contains(Vector3 point)
        {
            Vector3.DistanceSquared(ref point, ref Center, out Single distanceSquared);
            return distanceSquared < Radius * Radius ? ContainmentType.Contains : ContainmentType.Disjoint;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> contains the specified point.
        /// </summary>
        /// <param name="point">A <see cref="Vector3"/> which represents the point to evaluate.</param>
        /// <param name="result">A <see cref="ContainmentType"/> value representing the relationship between this sphere and the evaluated point.</param>
        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            Vector3.DistanceSquared(ref point, ref Center, out Single distanceSquared);
            result = distanceSquared < Radius * Radius ? ContainmentType.Contains : ContainmentType.Disjoint;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> contains the specified frustum.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustum"/> which represents the frustum to evaluate.</param>
        /// <returns>A <see cref="ContainmentType"/> value representing the relationship between this sphere and the evaluated frustum.</returns>
        public ContainmentType Contains(BoundingFrustum frustum)
        {
            Contract.Require(frustum, nameof(frustum));

            if (!frustum.Intersects(this))
                return ContainmentType.Disjoint;

            var radiusSquared = Radius * Radius;

            foreach (var corner in frustum.CornersInternal)
            {
                Vector3 cornerRelativeToCenter;
                cornerRelativeToCenter.X = corner.X - Center.X;
                cornerRelativeToCenter.Y = corner.Y - Center.Y;
                cornerRelativeToCenter.Z = corner.Z - Center.Z;

                if (cornerRelativeToCenter.LengthSquared() > radiusSquared)
                    return ContainmentType.Intersects;
            }

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> contains the specified frustum.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustum"/> which represents the frustum to evaluate.</param>
        /// <param name="result">A <see cref="ContainmentType"/> value representing the relationship between this sphere and the evaluated frustum.</param>
        public void Contains(BoundingFrustum frustum, out ContainmentType result)
        {
            result = Contains(frustum);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> contains the specified sphere.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere"/> which represents the sphere to evaluate.</param>
        /// <returns>A <see cref="ContainmentType"/> value representing the relationship between this sphere and the evaluated sphere.</returns>
        public ContainmentType Contains(BoundingSphere sphere)
        {
            Contains(ref sphere, out ContainmentType result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> contains the specified sphere.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere"/> which represents the sphere to evaluate.</param>
        /// <param name="result">A <see cref="ContainmentType"/> value representing the relationship between this sphere and the evaluated sphere.</param>
        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            Vector3.DistanceSquared(ref Center, ref sphere.Center, out Single distanceSquared);

            var combinedRadii = Radius + sphere.Radius;
            var combinedRadiiSquared = combinedRadii * combinedRadii;

            if (distanceSquared > combinedRadiiSquared)
            {
                result = ContainmentType.Disjoint;
            }
            else
            {
                var subtractedRadii = Radius - sphere.Radius;
                var subtractedRadiiSquared = subtractedRadii * subtractedRadii;

                result = (subtractedRadiiSquared < distanceSquared) ? ContainmentType.Intersects : ContainmentType.Contains;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> contains the specified bounding box.
        /// </summary>
        /// <param name="box">A <see cref="BoundingSphere"/> which represents the sphere to evaluate.</param>
        /// <returns>A <see cref="ContainmentType"/> value representing the relationship between this sphere and the evaluated box.</returns>
        public ContainmentType Contains(BoundingBox box)
        {
            Contains(ref box, out ContainmentType result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> contains the specified bounding box.
        /// </summary>
        /// <param name="box">A <see cref="BoundingSphere"/> which represents the sphere to evaluate.</param>
        /// <param name="result">A <see cref="ContainmentType"/> value representing the relationship between this sphere and the evaluated box.</param>
        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            var inside = true;

            for (int i = 0; i < BoundingBox.CornerCount; i++)
            {
                var corner = box.GetCorner(i);
                if (Contains(corner) == ContainmentType.Disjoint)
                {
                    inside = false;
                    break;
                }
            }

            if (inside)
            {
                result = ContainmentType.Contains;
                return;
            }

            var distance = 0.0;

            if (Center.X < box.Min.X)
                distance += (Center.X - box.Min.X) * (Center.X - box.Min.X);
            else if (Center.X > box.Max.X)
                distance += (Center.X - box.Max.X) * (Center.X - box.Max.X);
            
            if (Center.Y < box.Min.Y)
                distance += (Center.Y - box.Min.Y) * (Center.Y - box.Min.Y);
            else if (Center.Y > box.Max.Y)
                distance += (Center.Y - box.Max.Y) * (Center.Y - box.Max.Y);
            
            if (Center.Z < box.Min.Z)
                distance += (Center.Z - box.Min.Z) * (Center.Z - box.Min.Z);
            else if (Center.Z > box.Max.Z)
                distance += (Center.Z - box.Max.Z) * (Center.Z - box.Max.Z);

            if (distance <= Radius * Radius)
            {
                result = ContainmentType.Intersects;
                return;
            }

            result = ContainmentType.Disjoint;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> intersects the specified frustum.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustum"/> which represents the frustum to evaluate.</param>
        /// <returns><see langword="true"/> if this sphere intersects the evaluated frustum; otherwise, <see langword="false"/>.</returns>
        public Boolean Intersects(BoundingFrustum frustum)
        {
            Contract.Require(frustum, nameof(frustum));

            frustum.Intersects(ref this, out Boolean result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> intersects the specified frustum.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustum"/> which represents the frustum to evaluate.</param>
        /// <param name="result"><see langword="true"/> if this sphere intersects the evaluated frustum; otherwise, <see langword="false"/>.</param>
        public void Intersects(BoundingFrustum frustum, out Boolean result)
        {
            Contract.Require(frustum, nameof(frustum));

            frustum.Intersects(ref this, out result);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> intersects the specified sphere.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere"/> which represents the sphere to evaluate.</param>
        /// <returns><see langword="true"/> if this sphere intersects the evaluated frustum; otherwise, <see langword="false"/>.</returns>
        public Boolean Intersects(BoundingSphere sphere)
        {
            Intersects(ref sphere, out Boolean result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> intersects the specified sphere.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere"/> which represents the sphere to evaluate.</param>
        /// <param name="result"><see langword="true"/> if this sphere intersects the evaluated frustum; otherwise, <see langword="false"/>.</param>
        public void Intersects(ref BoundingSphere sphere, out Boolean result)
        {
            Vector3.DistanceSquared(ref Center, ref sphere.Center, out Single distanceSquared);

            var combinedRadii = Radius + sphere.Radius;
            var combinedRadiiSquared = combinedRadii * combinedRadii;

            result = distanceSquared <= combinedRadiiSquared;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> intersects the specified bounding box.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox"/> which represents the box to evaluate.</param>
        /// <returns><see langword="true"/> if this sphere intersects the evaluated box; otherwise, <see langword="false"/>.</returns>
        public Boolean Intersects(BoundingBox box)
        {
            return box.Intersects(this);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> intersects the specified bounding box.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox"/> which represents the box to evaluate.</param>
        /// <param name="result"><see langword="true"/> if this sphere intersects the evaluated box; otherwise, <see langword="false"/>.</param>
        public void Intersects(ref BoundingBox box, out Boolean result)
        {
            box.Intersects(ref this, out result);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> intersects the specified plane.
        /// </summary>
        /// <param name="plane">A <see cref="Plane"/> which represents the plane to evaluate.</param>
        /// <returns><see langword="true"/> if this sphere intersects the evaluated plane; otherwise, <see langword="false"/>.</returns>
        public PlaneIntersectionType Intersects(Plane plane)
        {
            Intersects(ref plane, out PlaneIntersectionType result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> intersects the specified plane.
        /// </summary>
        /// <param name="plane">A <see cref="Plane"/> which represents the plane to evaluate.</param>
        /// <param name="result"><see langword="true"/> if this sphere intersects the evaluated plane; otherwise, <see langword="false"/>.</param>
        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            Vector3.Dot(ref plane.Normal, ref Center, out Single distance);
            distance += plane.D;

            if (distance > Radius)
            {
                result = PlaneIntersectionType.Front;
            }
            else
            {
                if (distance < -this.Radius)
                {
                    result = PlaneIntersectionType.Back;
                }
                else
                {
                    result = PlaneIntersectionType.Intersecting;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> intersects the specified ray.
        /// </summary>
        /// <param name="ray">A <see cref="Ray"/> which represents the ray to evaluate.</param>
        /// <returns>The distance along the ray at which it intersects this sphere, or <see langword="null"/> if there is no intersection.</returns>
        public Single? Intersects(Ray ray)
        {
            ray.Intersects(ref this, out Single? result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingSphere"/> intersects the specified ray.
        /// </summary>
        /// <param name="ray">A <see cref="Ray"/> which represents the ray to evaluate.</param>
        /// <param name="result">The distance along the ray at which it intersects this sphere, or <see langword="null"/> if there is no intersection.</param>
        public void Intersects(ref Ray ray, out Single? result)
        {
            ray.Intersects(ref this, out result);
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere"/> which is the result of applying the specified
        /// transformation matrix to this bounding sphere.
        /// </summary>
        /// <param name="matrix">The transformation matrix to apply to the sphere.</param>
        /// <returns>The transformed <see cref="BoundingSphere"/> that was created.</returns>
        public BoundingSphere Transform(Matrix matrix)
        {
            var result = new BoundingSphere()
            {
                Center = Vector3.Transform(Center, matrix),
                Radius = this.Radius * (Single)Math.Sqrt(
                    Math.Max(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13,
                        Math.Max(
                            matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23,
                            matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33)))
            };
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere"/> which is the result of applying the specified
        /// transformation matrix to this bounding sphere.
        /// </summary>
        /// <param name="matrix">The transformation matrix to apply to the sphere.</param>
        /// <param name="result">The transformed <see cref="BoundingSphere"/> that was created.</param>
        public void Transform(ref Matrix matrix, out BoundingSphere result)
        {
            result = new BoundingSphere()
            {
                Center = Vector3.Transform(Center, matrix),
                Radius = this.Radius * (Single)Math.Sqrt(
                    Math.Max(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13,
                        Math.Max(
                            matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23,
                            matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33)))
            };
        }

        /// <summary>
        /// The sphere's center position.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Vector3 Center;

        /// <summary>
        /// The sphere's radius.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Single Radius;        
    }
}
