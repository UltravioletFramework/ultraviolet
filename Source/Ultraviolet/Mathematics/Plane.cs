using System;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a plane in three-dimensional space.
    /// </summary>
    [Serializable]
    public partial struct Plane : IEquatable<Plane>, IInterpolatable<Plane>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> structure.
        /// </summary>
        /// <param name="x">The x-component of the plane's normal vector.</param>
        /// <param name="y">The y-component of the plane's normal vector.</param>
        /// <param name="z">The z-component of the plane's normal vector.</param>
        /// <param name="d">The plane's distance along its normal from the origin.</param>
        public Plane(Single x, Single y, Single z, Single d)
        {
            Normal.X = x;
            Normal.Y = y;
            Normal.Z = z;
            D = d;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> structure.
        /// </summary>
        /// <param name="normal">The plane's normal vector.</param>
        /// <param name="d">The plane's distance along its normal from the origin.</param>
        [JsonConstructor]
        public Plane(Vector3 normal, Single d)
        {
            Normal = normal;
            D = d;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> structure from the specified <see cref="Vector4"/> instance.
        /// </summary>
        /// <param name="value">A vector whose first three elements represent the plane's normal vector, and whose last element
        /// represents the plane's distance along its normal from the origin.</param>
        public Plane(Vector4 value)
        {
            Normal.X = value.X;
            Normal.Y = value.Y;
            Normal.Z = value.Z;
            D = value.W;
        }
        
        /// <summary>
        /// Calculates the dot product of a <see cref="Plane"/> and a <see cref="Vector4"/>.
        /// </summary>
        /// <param name="plane">The plane for which to calculate the dot product.</param>
        /// <param name="value">The vector for which to calculate the dot product.</param>
        /// <returns>The dot product of the specified plane and vector.</returns>
        public static Single Dot(Plane plane, Vector4 value)
        {
            return
                plane.Normal.X * value.X +
                plane.Normal.Y * value.Y +
                plane.Normal.Z * value.Z +
                plane.D * value.W;
        }

        /// <summary>
        /// Calculates the dot product of a <see cref="Plane"/> and a <see cref="Vector4"/>.
        /// </summary>
        /// <param name="plane">The plane for which to calculate the dot product.</param>
        /// <param name="value">The vector for which to calculate the dot product.</param>
        /// <param name="result">The dot product of the specified plane and vector.</param>
        public static void Dot(ref Plane plane, ref Vector4 value, out Single result)
        {
            result = 
                plane.Normal.X * value.X +
                plane.Normal.Y * value.Y +
                plane.Normal.Z * value.Z +
                plane.D * value.W;
        }

        /// <summary>
        /// Calculates the dot product of a <see cref="Plane"/> and a <see cref="Vector3"/> plus the distance value of the plane.
        /// </summary>
        /// <param name="plane">The plane for which to calculate the dot product.</param>
        /// <param name="value">The vector for which to calculate the dot product.</param>
        /// <returns>The dot product of the specified plane and vector.</returns>
        public static Single DotCoordinate(Plane plane, Vector3 value)
        {
            return
                plane.Normal.X * value.X +
                plane.Normal.Y * value.Y +
                plane.Normal.Z * value.Z +
                plane.D;
        }

        /// <summary>
        /// Calculates the dot product of a <see cref="Plane"/> and a <see cref="Vector4"/> plus the distance value of the plane.
        /// </summary>
        /// <param name="plane">The plane for which to calculate the dot product.</param>
        /// <param name="value">The vector for which to calculate the dot product.</param>
        /// <param name="result">The dot product of the specified plane and vector.</param>
        public static void DotCoordinate(ref Plane plane, ref Vector3 value, out Single result)
        {
            result =
                plane.Normal.X * value.X +
                plane.Normal.Y * value.Y +
                plane.Normal.Z * value.Z +
                plane.D;
        }
        
        /// <summary>
        /// Calculates the dot product of the normal vector of a <see cref="Plane"/> and a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="plane">The plane for which to calculate the dot product.</param>
        /// <param name="value">The vector for which to calculate the dot product.</param>
        /// <returns>The dot product of the specified plane and vector.</returns>
        public static Single DotNormal(Plane plane, Vector4 value)
        {
            return
                plane.Normal.X * value.X +
                plane.Normal.Y * value.Y +
                plane.Normal.Z * value.Z;
        }

        /// <summary>
        /// Calculates the dot product of the normal vector of a <see cref="Plane"/> and a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="plane">The plane for which to calculate the dot product.</param>
        /// <param name="value">The vector for which to calculate the dot product.</param>
        /// <param name="result">The dot product of the specified plane and vector.</param>
        public static void DotNormal(ref Plane plane, ref Vector4 value, out Single result)
        {
            result = 
                plane.Normal.X * value.X +
                plane.Normal.Y * value.Y +
                plane.Normal.Z * value.Z;
        }

        /// <summary>
        /// Changes the coefficients of the normal vector of the specified plane to create a new plane
        /// with a normal vector of unit length.
        /// </summary>
        /// <param name="plane">The plane to normalize.</param>
        /// <returns>The normalized plane.</returns>
        public static Plane Normalize(Plane plane)
        {
            var length = plane.Normal.X * plane.Normal.X + plane.Normal.Y * plane.Normal.Y + plane.Normal.Z * plane.Normal.Z;
            if (MathUtil.IsApproximatelyZero(1f - length))
                return plane;

            var lengthInv = 1.0f / (Single)Math.Sqrt(length);

            Plane result;

            result.Normal.X = plane.Normal.X * lengthInv;
            result.Normal.Y = plane.Normal.Y * lengthInv;
            result.Normal.Z = plane.Normal.Z * lengthInv;
            result.D = plane.D * lengthInv;

            return result;
        }

        /// <summary>
        /// Changes the coefficients of the normal vector of the specified plane to create a new plane
        /// with a normal vector of unit length.
        /// </summary>
        /// <param name="plane">The plane to normalize.</param>
        /// <param name="result">The normalized plane.</param>
        public static void Normalize(ref Plane plane, out Plane result)
        {
            var length = plane.Normal.X * plane.Normal.X + plane.Normal.Y * plane.Normal.Y + plane.Normal.Z * plane.Normal.Z;
            if (MathUtil.IsApproximatelyZero(1f - length))
            {
                result = plane;
                return;
            }

            var lengthInv = 1.0f / (Single)Math.Sqrt(length);

            result.Normal.X = plane.Normal.X * lengthInv;
            result.Normal.Y = plane.Normal.Y * lengthInv;
            result.Normal.Z = plane.Normal.Z * lengthInv;
            result.D = plane.D * lengthInv;
        }

        /// <summary>
        /// Transforms a normalized plane by the specified matrix.
        /// </summary>
        /// <param name="plane">The plane to transform.</param>
        /// <param name="matrix">The matrix with which to transform the plane.</param>
        /// <returns>The transformed plane.</returns>
        public static Plane Transform(Plane plane, Matrix matrix)
        {
            Matrix matrixInv;
            Matrix.Invert(ref matrix, out matrixInv);

            var x = plane.Normal.X;
            var y = plane.Normal.Y;
            var z = plane.Normal.Z;
            var w = plane.D;

            return new Plane(
                x * matrixInv.M11 + y * matrixInv.M12 + z * matrixInv.M13 + w * matrixInv.M14,
                x * matrixInv.M21 + y * matrixInv.M22 + z * matrixInv.M23 + w * matrixInv.M24,
                x * matrixInv.M31 + y * matrixInv.M32 + z * matrixInv.M33 + w * matrixInv.M34,
                x * matrixInv.M41 + y * matrixInv.M42 + z * matrixInv.M43 + w * matrixInv.M44);
        }

        /// <summary>
        /// Transforms a normalized plane by the specified matrix.
        /// </summary>
        /// <param name="plane">The plane to transform.</param>
        /// <param name="matrix">The matrix with which to transform the plane.</param>
        /// <param name="result">The transformed plane.</param>
        public static void Transform(ref Plane plane, ref Matrix matrix, out Plane result)
        {
            Matrix matrixInv;
            Matrix.Invert(ref matrix, out matrixInv);

            var x = plane.Normal.X;
            var y = plane.Normal.Y;
            var z = plane.Normal.Z;
            var w = plane.D;

            result = new Plane(
                x * matrixInv.M11 + y * matrixInv.M12 + z * matrixInv.M13 + w * matrixInv.M14,
                x * matrixInv.M21 + y * matrixInv.M22 + z * matrixInv.M23 + w * matrixInv.M24,
                x * matrixInv.M31 + y * matrixInv.M32 + z * matrixInv.M33 + w * matrixInv.M34,
                x * matrixInv.M41 + y * matrixInv.M42 + z * matrixInv.M43 + w * matrixInv.M44);
        }

        /// <summary>
        /// Transforms a normalized plane by the specified quaternion.
        /// </summary>
        /// <param name="plane">The plane to transform.</param>
        /// <param name="quaternion">The quaternion with which to transform the plane.</param>
        /// <returns>The transformed plane.</returns>
        public static Plane Transform(Plane plane, Quaternion quaternion)
        {
            var x2 = quaternion.X + quaternion.X;
            var y2 = quaternion.Y + quaternion.Y;
            var z2 = quaternion.Z + quaternion.Z;

            var wx2 = quaternion.W * x2;
            var wy2 = quaternion.W * y2;
            var wz2 = quaternion.W * z2;
            var xx2 = quaternion.X * x2;
            var xy2 = quaternion.X * y2;
            var xz2 = quaternion.X * z2;
            var yy2 = quaternion.Y * y2;
            var yz2 = quaternion.Y * z2;
            var zz2 = quaternion.Z * z2;

            var m11 = 1.0f - yy2 - zz2;
            var m21 = xy2 - wz2;
            var m31 = xz2 + wy2;

            var m12 = xy2 + wz2;
            var m22 = 1.0f - xx2 - zz2;
            var m32 = yz2 - wx2;

            var m13 = xz2 - wy2;
            var m23 = yz2 + wx2;
            var m33 = 1.0f - xx2 - yy2;

            var x = plane.Normal.X;
            var y = plane.Normal.Y;
            var z = plane.Normal.Z;

            return new Plane(
                x * m11 + y * m21 + z * m31,
                x * m12 + y * m22 + z * m32,
                x * m13 + y * m23 + z * m33,
                plane.D);
        }

        /// <summary>
        /// Transforms a normalized plane by the specified quaternion.
        /// </summary>
        /// <param name="plane">The plane to transform.</param>
        /// <param name="quaternion">The quaternion with which to transform the plane.</param>
        /// <param name="result">The transformed plane.</param>
        public static void Transform(ref Plane plane, ref Quaternion quaternion, out Plane result)
        {
            var x2 = quaternion.X + quaternion.X;
            var y2 = quaternion.Y + quaternion.Y;
            var z2 = quaternion.Z + quaternion.Z;

            var wx2 = quaternion.W * x2;
            var wy2 = quaternion.W * y2;
            var wz2 = quaternion.W * z2;
            var xx2 = quaternion.X * x2;
            var xy2 = quaternion.X * y2;
            var xz2 = quaternion.X * z2;
            var yy2 = quaternion.Y * y2;
            var yz2 = quaternion.Y * z2;
            var zz2 = quaternion.Z * z2;

            var m11 = 1.0f - yy2 - zz2;
            var m21 = xy2 - wz2;
            var m31 = xz2 + wy2;

            var m12 = xy2 + wz2;
            var m22 = 1.0f - xx2 - zz2;
            var m32 = yz2 - wx2;

            var m13 = xz2 - wy2;
            var m23 = yz2 + wx2;
            var m33 = 1.0f - xx2 - yy2;

            var x = plane.Normal.X;
            var y = plane.Normal.Y;
            var z = plane.Normal.Z;

            result = new Plane(
                x * m11 + y * m21 + z * m31,
                x * m12 + y * m22 + z * m32,
                x * m13 + y * m23 + z * m33,
                plane.D);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{Normal:{Normal} D:{D}}}";

        /// <summary>
        /// Gets a value indicating whether this <see cref="Plane"/> intersects the specified frustum.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustum"/> which represents the frustum to evaluate.</param>
        /// <returns>A <see cref="PlaneIntersectionType"/> value which describes the relationship between this plane and the evaluated frustum.</returns>
        public PlaneIntersectionType Intersects(BoundingFrustum frustum)
        {
            Contract.Require(frustum, nameof(frustum));

            return frustum.Intersects(this);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Plane"/> intersects the specified frustum.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustum"/> which represents the frustum to evaluate.</param>
        /// <param name="result">A <see cref="PlaneIntersectionType"/> value which describes the relationship between this plane and the evaluated frustum.</param>
        public void Intersects(BoundingFrustum frustum, out PlaneIntersectionType result)
        {
            Contract.Require(frustum, nameof(frustum));

            result = frustum.Intersects(this);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Plane"/> intersects the specified sphere.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere"/> which represents the sphere to evaluate.</param>
        /// <returns>A <see cref="PlaneIntersectionType"/> value which describes the relationship between this plane and the evaluated sphere.</returns>
        public PlaneIntersectionType Intersects(BoundingSphere sphere)
        {
            sphere.Intersects(ref this, out PlaneIntersectionType result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Plane"/> intersects the specified sphere.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere"/> which represents the sphere to evaluate.</param>
        /// <param name="result">A <see cref="PlaneIntersectionType"/> value which describes the relationship between this plane and the evaluated sphere.</param>
        public void Intersects(ref BoundingSphere sphere, out PlaneIntersectionType result)
        {
            sphere.Intersects(ref this, out result);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Plane"/> intersects the specified bounding box.
        /// </summary>
        /// <param name="box">A <see cref="BoundingSphere"/> which represents the box to evaluate.</param>
        /// <returns>A <see cref="PlaneIntersectionType"/> value which describes the relationship between this plane and the evaluated box.</returns>
        public PlaneIntersectionType Intersects(BoundingBox box)
        {
            box.Intersects(ref this, out var result);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Plane"/> intersects the specified bounding box.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox"/> which represents the box to evaluate.</param>
        /// <param name="result">A <see cref="PlaneIntersectionType"/> value which describes the relationship between this plane and the evaluated box.</param>
        public void Intersects(ref BoundingBox box, out PlaneIntersectionType result)
        {
            box.Intersects(ref this, out result);
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Plane Interpolate(Plane target, Single t)
        {
            Plane result;

            result.Normal = this.Normal.Interpolate(target.Normal, t);
            result.D = Tweening.Lerp(this.D, target.D, t);

            return result;
        }

        /// <summary>
        /// The plane's normal vector.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Vector3 Normal;

        /// <summary>
        /// The plane's distance along its normal from the origin.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Single D;
    }
}
