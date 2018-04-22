using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an axis-aligned 3D bounding box.
    /// </summary>
    [Serializable]
    public partial struct BoundingBox : IEquatable<BoundingBox>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingBox"/> structure.
        /// </summary>
        /// <param name="min">The minimum point included within the bounding box.</param>
        /// <param name="max">The maximum point included within the bounding box.</param>
        [JsonConstructor]
        public BoundingBox(Vector3 min, Vector3 max)
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Creates a <see cref="BoundingBox"/> which contains the specified collection of points.
        /// </summary>
        /// <param name="points">The collection of points from which to create a bounding box.</param>
        /// <returns>The <see cref="BoundingBox"/> which was created from the specified collection of points.</returns>
        public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
        {
            CreateFromPoints(points, out var result);
            return result;
        }

        /// <summary>
        /// Creates a <see cref="BoundingBox"/> which contains the specified collection of points.
        /// </summary>
        /// <param name="points">The collection of points from which to create a bounding box.</param>
        /// <param name="result">The <see cref="BoundingBox"/> which was created from the specified collection of points.</param>
        public static void CreateFromPoints(IEnumerable<Vector3> points, out BoundingBox result)
        {
            Contract.Require(points, nameof(points));

            var empty = false;
            var min = new Vector3(Single.MaxValue);
            var max = new Vector3(Single.MinValue);

            foreach (var point in points)
            {
                empty = false;

                if (point.X < min.X)
                    min.X = point.X;

                if (point.Y < min.Y)
                    min.Y = point.Y;

                if (point.Z < min.Z)
                    min.Z = point.Z;

                if (point.X > max.X)
                    max.X = point.X;

                if (point.Y > max.Y)
                    max.Y = point.Y;

                if (point.Z > max.Z)
                    max.Z = point.Z;
            }

            if (empty)
                throw new ArgumentException(nameof(points));

            result = new BoundingBox(min, max);
        }

        /// <summary>
        /// Creates a <see cref="BoundingBox"/> which contains the specified collection of points.
        /// </summary>
        /// <param name="points">The collection of points from which to create a bounding box.</param>
        /// <returns>The <see cref="BoundingBox"/> which was created from the specified collection of points.</returns>
        public static BoundingBox CreateFromPoints(IList<Vector3> points)
        {
            CreateFromPoints(points, out var result);
            return result;
        }

        /// <summary>
        /// Creates a <see cref="BoundingBox"/> which contains the specified collection of points.
        /// </summary>
        /// <param name="points">The collection of points from which to create a bounding box.</param>
        /// <param name="result">The <see cref="BoundingBox"/> which was created from the specified collection of points.</param>
        public static void CreateFromPoints(IList<Vector3> points, out BoundingBox result)
        {
            Contract.Require(points, nameof(points));

            var empty = false;
            var min = new Vector3(Single.MaxValue);
            var max = new Vector3(Single.MinValue);

            foreach (var point in points)
            {
                empty = false;

                if (point.X < min.X)
                    min.X = point.X;

                if (point.Y < min.Y)
                    min.Y = point.Y;

                if (point.Z < min.Z)
                    min.Z = point.Z;

                if (point.X > max.X)
                    max.X = point.X;

                if (point.Y > max.Y)
                    max.Y = point.Y;

                if (point.Z > max.Z)
                    max.Z = point.Z;
            }

            if (empty)
                throw new ArgumentException(nameof(points));

            result = new BoundingBox(min, max);
        }

        /// <summary>
        /// Creates a <see cref="BoundingBox"/> which contains the specified sphere.
        /// </summary>
        /// <param name="sphere">The bounding sphere from which to create a bounding box.</param>
        /// <returns>The <see cref="BoundingBox"/> which was created from the specified sphere.</returns>
        public static BoundingBox CreateFromSphere(BoundingSphere sphere)
        {
            CreateFromSphere(ref sphere, out var result);
            return result;
        }

        /// <summary>
        /// Creates a <see cref="BoundingBox"/> which contains the specified sphere.
        /// </summary>
        /// <param name="sphere">The bounding sphere from which to create a bounding box.</param>
        /// <param name="result">The <see cref="BoundingBox"/> which was created from the specified sphere.</param>
        public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBox result)
        {
            var corner = new Vector3(sphere.Radius);
            result.Min = sphere.Center - corner;
            result.Max = sphere.Center + corner;
        }

        /// <summary>
        /// Creates a <see cref="BoundingBox"/> by merging the specified bounding boxes together.
        /// </summary>
        /// <param name="original">The original bounding box.</param>
        /// <param name="additional">The bounding box to merge with the original bounding box.</param>
        /// <returns>The <see cref="BoundingBox"/> which was created from merging the specified bounding boxes.</returns>
        public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
        {
            CreateMerged(ref original, ref additional, out var result);
            return result;
        }

        /// <summary>
        /// Creates a <see cref="BoundingBox"/> by merging the specified bounding boxes together.
        /// </summary>
        /// <param name="original">The original bounding box.</param>
        /// <param name="additional">The bounding box to merge with the original bounding box.</param>
        /// <param name="result">The <see cref="BoundingBox"/> which was created from merging the specified bounding boxes.</param>
        public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
        {
            result.Min.X = Math.Min(original.Min.X, additional.Min.X);
            result.Min.Y = Math.Min(original.Min.Y, additional.Min.Y);
            result.Min.Z = Math.Min(original.Min.Z, additional.Min.Z);

            result.Max.X = Math.Max(original.Max.X, additional.Max.X);
            result.Max.Y = Math.Max(original.Max.Y, additional.Max.Y);
            result.Max.Z = Math.Max(original.Max.Z, additional.Max.Z);
        }
        
        /// <inheritdoc/>
        public override String ToString() => $"{{Min:{Min} Max:{Max}}}";
        
        /// <summary>
        /// Gets the corner with the specified index.
        /// </summary>
        /// <param name="index">The index of the corner to retrieve.</param>
        /// <returns>The corner with the specified index.</returns>
        public Vector3 GetCorner(Int32 index)
        {
            GetCorner(index, out var result);
            return result;
        }

        /// <summary>
        /// Gets the corner with the specified index.
        /// </summary>
        /// <param name="index">The index of the corner to retrieve.</param>
        /// <param name="result">The corner with the specified index.</param>
        public void GetCorner(Int32 index, out Vector3 result)
        {
            switch (index)
            {
                case 0:
                    result.X = this.Min.X;
                    result.Y = this.Max.Y;
                    result.Z = this.Max.Z;
                    break;

                case 1:
                    result.X = this.Max.X;
                    result.Y = this.Max.Y;
                    result.Z = this.Max.Z;
                    break;

                case 2:
                    result.X = this.Max.X;
                    result.Y = this.Min.Y;
                    result.Z = this.Max.Z;
                    break;

                case 3:
                    result.X = this.Min.X;
                    result.Y = this.Min.Y;
                    result.Z = this.Max.Z;
                    break;

                case 4:
                    result.X = this.Min.X;
                    result.Y = this.Max.Y;
                    result.Z = this.Min.Z;
                    break;

                case 5:
                    result.X = this.Max.X;
                    result.Y = this.Max.Y;
                    result.Z = this.Min.Z;
                    break;

                case 6:
                    result.X = this.Max.X;
                    result.Y = this.Min.Y;
                    result.Z = this.Min.Z;
                    break;

                case 7:
                    result.X = this.Min.X;
                    result.Y = this.Min.Y;
                    result.Z = this.Min.Z;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Populates the specified array with the set of points that describe the frustum's corners.
        /// </summary>
        /// <param name="corners">The array to populate.</param>
        public void GetCorners(Vector3[] corners)
        {
            Contract.Require(corners, nameof(corners));
            Contract.Ensure<ArgumentException>(corners.Length >= CornerCount, nameof(corners));

            corners[0].X = this.Min.X;
            corners[0].Y = this.Max.Y;
            corners[0].Z = this.Max.Z;

            corners[1].X = this.Max.X;
            corners[1].Y = this.Max.Y;
            corners[1].Z = this.Max.Z;

            corners[2].X = this.Max.X;
            corners[2].Y = this.Min.Y;
            corners[2].Z = this.Max.Z;

            corners[3].X = this.Min.X;
            corners[3].Y = this.Min.Y;
            corners[3].Z = this.Max.Z;

            corners[4].X = this.Min.X;
            corners[4].Y = this.Max.Y;
            corners[4].Z = this.Min.Z;

            corners[5].X = this.Max.X;
            corners[5].Y = this.Max.Y;
            corners[5].Z = this.Min.Z;

            corners[6].X = this.Max.X;
            corners[6].Y = this.Min.Y;
            corners[6].Z = this.Min.Z;

            corners[7].X = this.Min.X;
            corners[7].Y = this.Min.Y;
            corners[7].Z = this.Min.Z;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> contains the specified point.
        /// </summary>
        /// <param name="point">A <see cref="Vector3"/> which represents the point to evaluate.</param>
        /// <returns>A <see cref="ContainmentType"/> value representing the relationship between this box and the evaluated point.</returns>
        public ContainmentType Contains(Vector3 point)
        {
            Contains(ref point, out var result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> contains the specified point.
        /// </summary>
        /// <param name="point">A <see cref="Vector3"/> which represents the point to evaluate.</param>
        /// <param name="result">A <see cref="ContainmentType"/> value representing the relationship between this box and the evaluated point.</param>
        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            if (point.X < Min.X ||
                point.X > Max.X ||
                point.Y < Min.Y ||
                point.Y > Max.Y ||
                point.Z < Min.Z ||
                point.Z > Max.Z)
            {
                result = ContainmentType.Disjoint;
            }
            else
            {
                result = ContainmentType.Contains;
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> contains the specified bounding box.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox"/> which represents the box to evaluate.</param>
        /// <returns>A <see cref="ContainmentType"/> value representing the relationship between this box and the evaluated box.</returns>
        public ContainmentType Contains(BoundingBox box)
        {
            Contains(ref box, out var result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> contains the specified bounding box.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox"/> which represents the box to evaluate.</param>
        /// <param name="result">A <see cref="ContainmentType"/> value representing the relationship between this box and the evaluated box.</param>
        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            if (box.Max.X < Min.X ||
                box.Min.X > Max.X ||
                box.Max.Y < Min.Y ||
                box.Min.Y > Max.Y ||
                box.Max.Z < Min.Z ||
                box.Min.Z > Max.Z)
            {
                result = ContainmentType.Disjoint;
                return;
            }

            if (box.Min.X >= Min.X &&
                box.Max.X <= Max.X &&
                box.Min.Y >= Min.Y &&
                box.Max.Y <= Max.Y &&
                box.Min.Z >= Min.Z &&
                box.Max.Z <= Max.Z)
            {
                result = ContainmentType.Contains;
                return;
            }

            result = ContainmentType.Intersects;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> contains the specified bounding sphere.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere"/> which represents the box to evaluate.</param>
        /// <returns>A <see cref="ContainmentType"/> value representing the relationship between this box and the evaluated sphere.</returns>
        public ContainmentType Contains(BoundingSphere sphere)
        {
            Contains(ref sphere, out var result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> contains the specified bounding sphere.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere"/> which represents the sphere to evaluate.</param>
        /// <param name="result">A <see cref="ContainmentType"/> value representing the relationship between this box and the evaluated sphere.</param>
        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            Vector3.Clamp(ref sphere.Center, ref Min, ref Max, out var clampedCenter);
            Vector3.DistanceSquared(ref sphere.Center, ref clampedCenter, out var clampedCenterOffset);

            var radiusSquared = sphere.Radius * sphere.Radius;
            if (radiusSquared <= clampedCenterOffset)
            {
                result = ContainmentType.Disjoint;
                return;
            }

            if (sphere.Center.X > Max.X - sphere.Radius ||
                sphere.Center.Y > Max.Y - sphere.Radius ||
                sphere.Center.Z > Max.Z - sphere.Radius ||
                Min.X + sphere.Radius > sphere.Center.X ||
                Min.Y + sphere.Radius > sphere.Center.Y ||
                Min.Z + sphere.Radius > sphere.Center.Z ||
                Max.X - Min.X <= sphere.Radius ||
                Max.Y - Min.Y <= sphere.Radius ||
                Max.X - Min.X <= sphere.Radius)
            {
                result = ContainmentType.Intersects;
                return;
            }

            result = ContainmentType.Contains;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> contains the specified bounding frustum.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustum"/> which represents the frustum to evaluate.</param>
        /// <returns>A <see cref="ContainmentType"/> value representing the relationship between this box and the evaluated frustum.</returns>
        public ContainmentType Contains(BoundingFrustum frustum)
        {
            Contains(frustum, out var result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> contains the specified bounding frustum.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustum"/> which represents the frustum to evaluate.</param>
        /// <param name="result">A <see cref="ContainmentType"/> value representing the relationship between this box and the evaluated frustum.</param>
        public void Contains(BoundingFrustum frustum, out ContainmentType result)
        {
            Contract.Require(frustum, nameof(frustum));

            if (!frustum.Intersects(this))
            {
                result = ContainmentType.Disjoint;
                return;
            }

            foreach (var corner in frustum.CornersInternal)
            {
                if (Contains(corner) == ContainmentType.Disjoint)
                {
                    result = ContainmentType.Intersects;
                    return;
                }
            }

            result = ContainmentType.Contains;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> intersects the specified bounding box.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox"/> which represents the box to evaluate.</param>
        /// <returns><see langword="true"/> if this box intersects the evaluated box; otherwise, <see langword="false"/>.</returns>
        public Boolean Intersects(BoundingBox box)
        {
            Intersects(ref box, out var result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> intersects the specified bounding box.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox"/> which represents the box to evaluate.</param>
        /// <param name="result"><see langword="true"/> if this box intersects the evaluated box; otherwise, <see langword="false"/>.</param>
        public void Intersects(ref BoundingBox box, out Boolean result)
        {
            if (Max.X >= box.Min.X && Min.X <= box.Max.X &&
                Max.Y >= box.Min.Y && Min.Y <= box.Max.Y &&
                Max.Z >= box.Min.Z && Min.Z <= box.Max.Z)
            {
                result = true;
                return;
            }

            result = false;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> intersects the specified bounding sphere.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere"/> which represents the sphere to evaluate.</param>
        /// <returns><see langword="true"/> if this box intersects the evaluated sphere; otherwise, <see langword="false"/>.</returns>
        public Boolean Intersects(BoundingSphere sphere)
        {
            Intersects(ref sphere, out var result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> intersects the specified bounding sphere.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere"/> which represents the sphere to evaluate.</param>
        /// <param name="result"><see langword="true"/> if this box intersects the evaluated sphere; otherwise, <see langword="false"/>.</param>
        public void Intersects(ref BoundingSphere sphere, out Boolean result)
        {
            Vector3.Clamp(ref sphere.Center, ref Min, ref Max, out var clampedCenter);
            Vector3.DistanceSquared(ref sphere.Center, ref clampedCenter, out var clampedCenterOffset);
            result = (clampedCenterOffset <= sphere.Radius * sphere.Radius);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> intersects the specified bounding frustum.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustum"/> which represents the frustum to evaluate.</param>
        /// <returns><see langword="true"/> if this box intersects the evaluated frustum; otherwise, <see langword="false"/>.</returns>
        public Boolean Intersects(BoundingFrustum frustum)
        {
            Contract.Require(frustum, nameof(frustum));

            frustum.Intersects(ref this, out var result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> intersects the specified bounding frustum.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustum"/> which represents the frustum to evaluate.</param>
        /// <param name="result"><see langword="true"/> if this box intersects the evaluated frustum; otherwise, <see langword="false"/>.</param>
        public void Intersects(BoundingFrustum frustum, out Boolean result)
        {
            Contract.Require(frustum, nameof(frustum));

            frustum.Intersects(ref this, out result);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> intersects the specified plane.
        /// </summary>
        /// <param name="plane">A <see cref="Plane"/> which represents the plane to evaluate.</param>
        /// <returns><see langword="true"/> if this box intersects the evaluated plane; otherwise, <see langword="false"/>.</returns>
        public PlaneIntersectionType Intersects(Plane plane)
        {
            plane.Intersects(ref this, out var result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="BoundingBox"/> intersects the specified plane.
        /// </summary>
        /// <param name="plane">A <see cref="Plane"/> which represents the plane to evaluate.</param>
        /// <param name="result"><see langword="true"/> if this box intersects the evaluated plane; otherwise, <see langword="false"/>.</param>
        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            Vector3 positiveVertex;
            positiveVertex.X = plane.Normal.X >= 0.0 ? this.Min.X : this.Max.X;
            positiveVertex.Y = plane.Normal.Y >= 0.0 ? this.Min.Y : this.Max.Y;
            positiveVertex.Z = plane.Normal.Z >= 0.0 ? this.Min.Z : this.Max.Z;

            Vector3 negativeVertex;
            negativeVertex.X = plane.Normal.X >= 0.0 ? this.Max.X : this.Min.X;
            negativeVertex.Y = plane.Normal.Y >= 0.0 ? this.Max.Y : this.Min.Y;
            negativeVertex.Z = plane.Normal.Z >= 0.0 ? this.Max.Z : this.Min.Z;

            var normalDotPositive = plane.Normal.X * positiveVertex.X + plane.Normal.Y * positiveVertex.Y + plane.Normal.Z * positiveVertex.Z + plane.D;
            if (normalDotPositive > 0.0f)
            {
                result = PlaneIntersectionType.Front;
                return;
            }

            var normalDotNegative = plane.Normal.X * negativeVertex.X + plane.Normal.Y * negativeVertex.Y + plane.Normal.Z * negativeVertex.Z + plane.D;
            if (normalDotNegative < 0.0f)
            {
                result = PlaneIntersectionType.Back;
                return;
            }

            result = PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Gets a value indicating the distance at which this <see cref="BoundingBox"/> is intersted by the specified ray,
        /// if an intersection occurs.
        /// </summary>
        /// <param name="ray">A <see cref="Ray"/> which represents the ray to evaluate.</param>
        /// <returns>The distance along the ray at which it intersects the bounding box, or <see langword="null"/> if there is no intersection.</returns>
        public Single? Intersects(Ray ray)
        {
            ray.Intersects(ref this, out var result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating the distance at which this <see cref="BoundingBox"/> is intersted by the specified ray,
        /// if an intersection occurs.
        /// </summary>
        /// <param name="ray">A <see cref="Ray"/> which represents the ray to evaluate.</param>
        /// <param name="result">The distance along the ray at which it intersects the bounding box, or <see langword="null"/> if there is no intersection.</param>
        public void Intersects(ref Ray ray, out Single? result)
        {
            ray.Intersects(ref this, out result);
        }

        /// <summary>
        /// The number of corners in a bounding box.
        /// </summary>
        public const Int32 CornerCount = 8;

        /// <summary>
        /// The minimum point included within the bounding box.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Vector3 Min;

        /// <summary>
        /// The maximum point included within the bounding box.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Vector3 Max;
    }
}
