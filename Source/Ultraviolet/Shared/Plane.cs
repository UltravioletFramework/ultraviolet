using System;
using System.Globalization;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a plane in three-dimensional space.
    /// </summary>
    public struct Plane : IEquatable<Plane>, IInterpolatable<Plane>
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
        /// Compares two planes for equality.
        /// </summary>
        /// <param name="p1">The first <see cref="Plane"/> to compare.</param>
        /// <param name="p2">The second <see cref="Plane"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified planes are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(Plane p1, Plane p2) => p1.Equals(p2);

        /// <summary>
        /// Compares two planes for inequality.
        /// </summary>
        /// <param name="p1">The first <see cref="Plane"/> to compare.</param>
        /// <param name="p2">The second <see cref="Plane"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified planes are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(Plane p1, Plane p2) => !p1.Equals(p2);

        /// <summary>
        /// Converts the string representation of a plane into an instance of the <see cref="Plane"/> structure.
        /// </summary>
        /// <param name="s">A string containing a plane to convert.</param>
        /// <returns>A instance of the <see cref="Plane"/> structure equivalent to the plane contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Plane Parse(String s) =>
            Parse(s, NumberStyles.Float, NumberFormatInfo.CurrentInfo);

        /// <summary>
        /// Converts the string representation of a plane into an instance of the <see cref="Plane"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a plane to convert.</param>
        /// <param name="plane">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Plane plane) =>
            TryParse(s, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out plane);

        /// <summary>
        /// Converts the string representation of a plane into an instance of the <see cref="Plane"/> structure.
        /// </summary>
        /// <param name="s">A string containing a plane to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Plane"/> structure equivalent to the plane contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Plane Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            if (!TryParse(s, style, provider, out Plane plane))
                throw new FormatException();

            return plane;
        }

        /// <summary>
        /// Converts the string representation of a plane into an instance of the <see cref="Plane"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a plane to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="plane">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Plane plane)
        {
            plane = default(Plane);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 4)
                return false;

            if (!Single.TryParse(components[0], style, provider, out Single x))
                return false;
            if (!Single.TryParse(components[1], style, provider, out Single y))
                return false;
            if (!Single.TryParse(components[2], style, provider, out Single z))
                return false;
            if (!Single.TryParse(components[3], style, provider, out Single d))
                return false;

            plane = new Plane(x, y, z, d);
            return true;
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

        /// <summary>
        /// Gets the object's hash code.
        /// </summary>
        /// <returns>The object's hash code.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Normal.GetHashCode();
                hash = hash * 23 + D.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString() => 
            ToString(null);

        /// <summary>
        /// Converts the object to a human-readable string using the specified culture information.
        /// </summary>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A human-readable string that represents the object.</returns>
        public String ToString(IFormatProvider provider) =>
            String.Format(provider, "{0} {1}", Normal, D);

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Plane))
                return false;
            return Equals((Plane)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(Plane other)
        {
            return Normal.Equals(other.Normal) && D.Equals(other.D);
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        [Preserve]
        public Plane Interpolate(Plane target, Single t)
        {
            Plane result;

            result.Normal = Tweening.Lerp(this.Normal, target.Normal, t);
            result.D = Tweening.Lerp(this.D, target.D, t);

            return result;
        }

        /// <summary>
        /// The plane's normal vector.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "normal", Required = Required.Always)]
        public Vector3 Normal;

        /// <summary>
        /// The plane's distance along its normal from the origin.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "d", Required = Required.Always)]
        public Single D;
    }
}
