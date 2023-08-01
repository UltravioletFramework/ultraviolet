using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a four-dimensional vector.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(Single) * 4)]
    public partial struct Vector4 : IEquatable<Vector4>, IInterpolatable<Vector4>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> structure with all of its components set to the specified value.
        /// </summary>
        /// <param name="value">The value to which to set the vector's components.</param>
        public Vector4(Single value)
        {
            this.X = value;
            this.Y = value;
            this.Z = value;
            this.W = value;
        }

        /// <summary>
        /// Initializes a new instance of the Vector4 structure with the specified x, y, z, and w components.
        /// </summary>
        /// <param name="x">The vector's x component.</param>
        /// <param name="y">The vector's y component.</param>
        /// <param name="z">The vector's z component.</param>
        /// <param name="w">The vector's w component.</param>
        [JsonConstructor]
        public Vector4(Single x, Single y, Single z, Single w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> structure with its x and y components set to the 
        /// x and y components of the specified vector, and its z and w components set to the specified values.
        /// </summary>
        /// <param name="vector">The vector from which to set the vector's x and y components.</param>
        /// <param name="z">The vector's z component.</param>
        /// <param name="w">The vector's w component.</param>
        public Vector4(Vector2 vector, Single z, Single w)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> structure with its x, y, and z components set to the 
        /// x, y, and z components of the specified vector, and its w component set to the specified values.
        /// </summary>
        /// <param name="vector">The vector from which to set the vector's x, y, and z components.</param>
        /// <param name="w">The vector's w component.</param>
        public Vector4(Vector3 vector, Single w)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
            this.W = w;
        }

        /// <summary>
        /// Implicitly converts an instance of the <see cref="System.Numerics.Vector4"/> structure
        /// to an instance of the <see cref="Vector4"/> structure.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static unsafe implicit operator Vector4(System.Numerics.Vector4 value)
        {
            var x = (Vector4*)&value;
            return *x;
        }

        /// <summary>
        /// Implicitly converts an instance of the <see cref="Vector4"/> structure
        /// to an instance of the <see cref="System.Numerics.Vector4"/> structure.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static unsafe implicit operator System.Numerics.Vector4(Vector4 value)
        {
            var x = (System.Numerics.Vector4*)&value;
            return *x;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the addition operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 operator +(Vector4 vector1, Vector4 vector2)
        {
            Vector4 result;

            result.X = vector1.X + vector2.X;
            result.Y = vector1.Y + vector2.Y;
            result.Z = vector1.Z + vector2.Z;
            result.W = vector1.W + vector2.W;

            return result;
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the subtraction operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 operator -(Vector4 vector1, Vector4 vector2)
        {
            Vector4 result;

            result.X = vector1.X - vector2.X;
            result.Y = vector1.Y - vector2.Y;
            result.Z = vector1.Z - vector2.Z;
            result.W = vector1.W - vector2.W;

            return result;
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 operator *(Vector4 vector1, Vector4 vector2)
        {
            Vector4 result;

            result.X = vector1.X * vector2.X;
            result.Y = vector1.Y * vector2.Y;
            result.Z = vector1.Z * vector2.Z;
            result.W = vector1.W * vector2.W;

            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 operator *(Single factor, Vector4 vector)
        {
            Vector4 result;

            result.X = vector.X * factor;
            result.Y = vector.Y * factor;
            result.Z = vector.Z * factor;
            result.W = vector.W * factor;

            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 operator *(Vector4 vector, Single factor)
        {
            Vector4 result;

            result.X = vector.X * factor;
            result.Y = vector.Y * factor;
            result.Z = vector.Z * factor;
            result.W = vector.W * factor;

            return result;
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 operator /(Vector4 vector1, Vector4 vector2)
        {
            Vector4 result;

            result.X = vector1.X / vector2.X;
            result.Y = vector1.Y / vector2.Y;
            result.Z = vector1.Z / vector2.Z;
            result.W = vector1.W / vector2.W;

            return result;
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 operator /(Vector4 vector, Single factor)
        {
            Vector4 result;

            result.X = vector.X / factor;
            result.Y = vector.Y / factor;
            result.Z = vector.Z / factor;
            result.W = vector.W / factor;

            return result;
        }

        /// <summary>
        /// Reverses the signs of a vector's components.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to reverse.</param>
        /// <returns>The reversed <see cref="Vector4"/>.</returns>
        public static Vector4 operator -(Vector4 vector)
        {
            Vector4 result;

            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;
            result.W = -vector.W;

            return result;
        }
        
        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <returns>The dot product of the specified vectors.</returns>
        public static Single Dot(Vector4 vector1, Vector4 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z + vector1.W * vector2.W;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <param name="result">The dot product of the specified vectors.</param>
        public static void Dot(ref Vector4 vector1, ref Vector4 vector2, out Single result)
        {
            result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z + vector1.W * vector2.W;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the addition operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Add(Vector4 left, Vector4 right)
        {
            Vector4 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;

            return result;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the addition operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the addition operator.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Add(ref Vector4 left, ref Vector4 right, out Vector4 result)
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the subtraction operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Subtract(Vector4 left, Vector4 right)
        {
            Vector4 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;

            return result;
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the subtraction operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the subtraction operator.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Subtract(ref Vector4 left, ref Vector4 right, out Vector4 result)
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the multiplication operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Multiply(Vector4 left, Vector4 right)
        {
            Vector4 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
            result.W = left.W * right.W;

            return result;
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the multiplication operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the multiplication operator.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Multiply(ref Vector4 left, ref Vector4 right, out Vector4 result)
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
            result.W = left.W * right.W;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Multiply(Vector4 vector, Single factor)
        {
            Vector4 result;

            result.X = vector.X * factor;
            result.Y = vector.Y * factor;
            result.Z = vector.Z * factor;
            result.W = vector.W * factor;

            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Multiply(ref Vector4 vector, Single factor, out Vector4 result)
        {
            result.X = vector.X * factor;
            result.Y = vector.Y * factor;
            result.Z = vector.Z * factor;
            result.W = vector.W * factor;
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the division operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Divide(Vector4 left, Vector4 right)
        {
            Vector4 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
            result.W = left.W / right.W;

            return result;
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the division operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the division operator.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Divide(ref Vector4 left, ref Vector4 right, out Vector4 result)
        {
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
            result.W = left.W / right.W;
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Divide(Vector4 vector, Single factor)
        {
            Vector4 result;

            result.X = vector.X / factor;
            result.Y = vector.Y / factor;
            result.Z = vector.Z / factor;
            result.W = vector.W / factor;

            return result;
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Divide(ref Vector4 vector, Single factor, out Vector4 result)
        {
            result.X = vector.X / factor;
            result.Y = vector.Y / factor;
            result.Z = vector.Z / factor;
            result.W = vector.W / factor;
        }
        
        /// <summary>
        /// Transforms a vector by a quaternion.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector4"/>.</returns>
        public static Vector4 Transform(Vector2 vector, Quaternion quaternion)
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

            Vector4 result;

            result.X = vector.X * (1.0f - yy2 - zz2) + vector.Y * (xy2 - wz2);
            result.Y = vector.X * (xy2 + wz2) + vector.Y * (1.0f - xx2 - zz2);
            result.Z = vector.X * (xz2 - wy2) + vector.Y * (yz2 + wx2);
            result.W = 1.0f;

            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector4"/>.</param>
        public static void Transform(ref Vector2 vector, ref Quaternion quaternion, out Vector4 result)
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

            Vector4 temp;

            temp.X = vector.X * (1.0f - yy2 - zz2) + vector.Y * (xy2 - wz2);
            temp.Y = vector.X * (xy2 + wz2) + vector.Y * (1.0f - xx2 - zz2);
            temp.Z = vector.X * (xz2 - wy2) + vector.Y * (yz2 + wx2);
            temp.W = 1.0f;

            result = temp;
        }
        
        /// <summary>
        /// Transforms a vector by a quaternion.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector4"/>.</returns>
        public static Vector4 Transform(Vector3 vector, Quaternion quaternion)
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

            Vector4 result;

            result.X = vector.X * (1.0f - yy2 - zz2) + vector.Y * (xy2 - wz2) + vector.Z * (xz2 + wy2);
            result.Y = vector.X * (xy2 + wz2) + vector.Y * (1.0f - xx2 - zz2) + vector.Z * (yz2 - wx2);
            result.Z = vector.X * (xz2 - wy2) + vector.Y * (yz2 + wx2) + vector.Z * (1.0f - xx2 - yy2);
            result.W = 1.0f;

            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector4"/>.</param>
        public static void Transform(ref Vector3 vector, ref Quaternion quaternion, out Vector4 result)
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

            Vector4 temp;

            temp.X = vector.X * (1.0f - yy2 - zz2) + vector.Y * (xy2 - wz2) + vector.Z * (xz2 + wy2);
            temp.Y = vector.X * (xy2 + wz2) + vector.Y * (1.0f - xx2 - zz2) + vector.Z * (yz2 - wx2);
            temp.Z = vector.X * (xz2 - wy2) + vector.Y * (yz2 + wx2) + vector.Z * (1.0f - xx2 - yy2);
            temp.W = 1.0f;

            result = temp;
        }

        /// <summary>
        /// Transforms a vector by a quaternion.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector4"/>.</returns>
        public static Vector4 Transform(Vector4 vector, Quaternion quaternion)
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

            Vector4 result;

            result.X = vector.X * (1.0f - yy2 - zz2) + vector.Y * (xy2 - wz2) + vector.Z * (xz2 + wy2);
            result.Y = vector.X * (xy2 + wz2) + vector.Y * (1.0f - xx2 - zz2) + vector.Z * (yz2 - wx2);
            result.Z = vector.X * (xz2 - wy2) + vector.Y * (yz2 + wx2) + vector.Z * (1.0f - xx2 - yy2);
            result.W = vector.W;

            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector4"/>.</param>
        public static void Transform(ref Vector4 vector, ref Quaternion quaternion, out Vector4 result)
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

            Vector4 temp;

            temp.X = vector.X * (1.0f - yy2 - zz2) + vector.Y * (xy2 - wz2) + vector.Z * (xz2 + wy2);
            temp.Y = vector.X * (xy2 + wz2) + vector.Y * (1.0f - xx2 - zz2) + vector.Z * (yz2 - wx2);
            temp.Z = vector.X * (xz2 - wy2) + vector.Y * (yz2 + wx2) + vector.Z * (1.0f - xx2 - yy2);
            temp.W = vector.W;

            result = temp;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector4"/>.</returns>
        public static Vector4 Transform(Vector2 vector, Matrix matrix)
        {
            Vector4 result;

            result.X = vector.X * matrix.M11 + vector.Y * matrix.M21 + matrix.M41;
            result.Y = vector.X * matrix.M12 + vector.Y * matrix.M22 + matrix.M42;
            result.Z = vector.X * matrix.M13 + vector.Y * matrix.M23 + matrix.M43;
            result.W = vector.X * matrix.M14 + vector.Y * matrix.M24 + matrix.M44;

            return result;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector4"/>.</param>
        public static void Transform(ref Vector2 vector, ref Matrix matrix, out Vector4 result)
        {
            Vector4 temp;

            temp.X = vector.X * matrix.M11 + vector.Y * matrix.M21 + matrix.M41;
            temp.Y = vector.X * matrix.M12 + vector.Y * matrix.M22 + matrix.M42;
            temp.Z = vector.X * matrix.M13 + vector.Y * matrix.M23 + matrix.M43;
            temp.W = vector.X * matrix.M14 + vector.Y * matrix.M24 + matrix.M44;

            result = temp;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector4"/>.</returns>
        public static Vector4 Transform(Vector3 vector, Matrix matrix)
        {
            Vector4 result;

            result.X = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + matrix.M41;
            result.Y = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + matrix.M42;
            result.Z = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + matrix.M43;
            result.W = vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + matrix.M44;

            return result;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector4"/>.</param>
        public static void Transform(ref Vector3 vector, ref Matrix matrix, out Vector4 result)
        {
            Vector4 temp;

            temp.X = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + matrix.M41;
            temp.Y = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + matrix.M42;
            temp.Z = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + matrix.M43;
            temp.W = vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + matrix.M44;

            result = temp;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector4"/>.</returns>
        public static Vector4 Transform(Vector4 vector, Matrix matrix)
        {
            Vector4 result;

            result.X = (matrix.M11 * vector.X + matrix.M21 * vector.Y + matrix.M31 * vector.Z + matrix.M41 * vector.W);
            result.Y = (matrix.M12 * vector.X + matrix.M22 * vector.Y + matrix.M32 * vector.Z + matrix.M42 * vector.W);
            result.Z = (matrix.M13 * vector.X + matrix.M23 * vector.Y + matrix.M33 * vector.Z + matrix.M43 * vector.W);
            result.W = (matrix.M14 * vector.X + matrix.M24 * vector.Y + matrix.M34 * vector.Z + matrix.M44 * vector.W);

            return result;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector4"/>.</param>
        public static void Transform(ref Vector4 vector, ref Matrix matrix, out Vector4 result)
        {
            Vector4 temp;

            temp.X = (matrix.M11 * vector.X + matrix.M21 * vector.Y + matrix.M31 * vector.Z + matrix.M41 * vector.W);
            temp.Y = (matrix.M12 * vector.X + matrix.M22 * vector.Y + matrix.M32 * vector.Z + matrix.M42 * vector.W);
            temp.Z = (matrix.M13 * vector.X + matrix.M23 * vector.Y + matrix.M33 * vector.Z + matrix.M43 * vector.W);
            temp.W = (matrix.M14 * vector.X + matrix.M24 * vector.Y + matrix.M34 * vector.Z + matrix.M44 * vector.W);

            result = temp;
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to normalize.</param>
        /// <returns>The normalized <see cref="Vector4"/>.</returns>
        public static Vector4 Normalize(Vector4 vector)
        {
            var magnitude = (Single)Math.Sqrt(
                vector.X * vector.X +
                vector.Y * vector.Y +
                vector.Z * vector.Z +
                vector.W * vector.W);
            var inverseMagnitude = 1f / magnitude;

            Vector4 result;

            result.X = vector.X * inverseMagnitude;
            result.Y = vector.Y * inverseMagnitude;
            result.Z = vector.Z * inverseMagnitude;
            result.W = vector.W * inverseMagnitude;

            return result;
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to normalize.</param>
        /// <param name="result">The normalized <see cref="Vector4"/>.</param>
        public static void Normalize(ref Vector4 vector, out Vector4 result)
        {
            var magnitude = (Single)Math.Sqrt(
                vector.X * vector.X +
                vector.Y * vector.Y +
                vector.Z * vector.Z +
                vector.W * vector.W);
            var inverseMagnitude = 1f / magnitude;

            result.X = vector.X * inverseMagnitude;
            result.Y = vector.Y * inverseMagnitude;
            result.Z = vector.Z * inverseMagnitude;
            result.W = vector.W * inverseMagnitude;
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to negate.</param>
        /// <returns>The negated <see cref="Vector4"/>.</returns>
        public static Vector4 Negate(Vector4 vector)
        {
            Vector4 result;

            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;
            result.W = -vector.W;

            return result;
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to negate.</param>
        /// <param name="result">The negated <see cref="Vector4"/>.</param>
        public static void Negate(ref Vector4 vector, out Vector4 result)
        {
            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;
            result.W = -vector.W;
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped <see cref="Vector4"/>.</returns>
        public static Vector4 Clamp(Vector4 vector, Vector4 min, Vector4 max)
        {
            Vector4 result;

            result.X = vector.X < min.X ? min.X : vector.X > max.X ? max.X : vector.X;
            result.Y = vector.Y < min.Y ? min.Y : vector.Y > max.Y ? max.Y : vector.Y;
            result.Z = vector.Z < min.Z ? min.Z : vector.Z > max.Z ? max.Z : vector.Z;
            result.W = vector.W < min.W ? min.W : vector.W > max.W ? max.W : vector.W;

            return result;
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">The clamped <see cref="Vector4"/>.</param>
        public static void Clamp(ref Vector4 vector, ref Vector4 min, ref Vector4 max, out Vector4 result)
        {
            result.X = vector.X < min.X ? min.X : vector.X > max.X ? max.X : vector.X;
            result.Y = vector.Y < min.Y ? min.Y : vector.Y > max.Y ? max.Y : vector.Y;
            result.Z = vector.Z < min.Z ? min.Z : vector.Z > max.Z ? max.Z : vector.Z;
            result.W = vector.W < min.W ? min.W : vector.W > max.W ? max.W : vector.W;
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <returns>A <see cref="Vector4"/> which contains the lowest value of each component of the specified vectors.</returns>
        public static Vector4 Min(Vector4 vector1, Vector4 vector2)
        {
            Vector4 result;

            result.X = vector1.X < vector2.X ? vector1.X : vector2.X;
            result.Y = vector1.Y < vector2.Y ? vector1.Y : vector2.Y;
            result.Z = vector1.Z < vector2.Z ? vector1.Z : vector2.Z;
            result.W = vector1.W < vector2.W ? vector1.W : vector2.W;

            return result;
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <param name="result">A <see cref="Vector4"/> which contains the lowest value of each component of the specified vectors.</param>
        public static void Min(ref Vector4 vector1, ref Vector4 vector2, out Vector4 result)
        {
            result.X = vector1.X < vector2.X ? vector1.X : vector2.X;
            result.Y = vector1.Y < vector2.Y ? vector1.Y : vector2.Y;
            result.Z = vector1.Z < vector2.Z ? vector1.Z : vector2.Z;
            result.W = vector1.W < vector2.W ? vector1.W : vector2.W;
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <returns>A <see cref="Vector4"/> which contains the highest value of each component of the specified vectors.</returns>
        public static Vector4 Max(Vector4 vector1, Vector4 vector2)
        {
            Vector4 result;

            result.X = vector1.X < vector2.X ? vector2.X : vector1.X;
            result.Y = vector1.Y < vector2.Y ? vector2.Y : vector1.Y;
            result.Z = vector1.Z < vector2.Z ? vector2.Z : vector1.Z;
            result.W = vector1.W < vector2.W ? vector2.W : vector1.W;

            return result;
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <param name="result">A <see cref="Vector4"/> which contains the highest value of each component of the specified vectors.</param>
        public static void Max(ref Vector4 vector1, ref Vector4 vector2, out Vector4 result)
        {
            result.X = vector1.X < vector2.X ? vector2.X : vector1.X;
            result.Y = vector1.Y < vector2.Y ? vector2.Y : vector1.Y;
            result.Z = vector1.Z < vector2.Z ? vector2.Z : vector1.Z;
            result.W = vector1.W < vector2.W ? vector2.W : vector1.W;
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <returns>The reflected <see cref="Vector4"/>.</returns>
        public static Vector4 Reflect(Vector4 vector, Vector4 normal)
        {
            var dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z + vector.W * normal.W;

            Vector4 result;

            result.X = vector.X - 2f * dot * normal.X;
            result.Y = vector.Y - 2f * dot * normal.Y;
            result.Z = vector.Z - 2f * dot * normal.Z;
            result.W = vector.W - 2f * dot * normal.W;

            return result;
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <param name="result">The reflected <see cref="Vector4"/>.</param>
        public static void Reflect(ref Vector4 vector, ref Vector4 normal, out Vector4 result)
        {
            var dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z + vector.W * normal.W;

            result.X = vector.X - 2f * dot * normal.X;
            result.Y = vector.Y - 2f * dot * normal.Y;
            result.Z = vector.Z - 2f * dot * normal.Z;
            result.W = vector.W - 2f * dot * normal.W;
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector4"/>.</returns>
        public static Vector4 Lerp(Vector4 vector1, Vector4 vector2, Single amount)
        {
            Vector4 result;
            
            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;
            result.Z = vector1.Z + (vector2.Z - vector1.Z) * amount;
            result.W = vector1.W + (vector2.W - vector1.W) * amount;

            return result;
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector4"/>.</param>
        public static void Lerp(ref Vector4 vector1, ref Vector4 vector2, Single amount, out Vector4 result)
        {
            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;
            result.Z = vector1.Z + (vector2.Z - vector1.Z) * amount;
            result.W = vector1.W + (vector2.W - vector1.W) * amount;
        }

        /// <summary>
        /// Performs a cubic Hermite spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="vector2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector4"/>.</returns>
        public static Vector4 Hermite(Vector4 vector1, Vector4 tangent1, Vector4 vector2, Vector4 tangent2, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            var polynomial1 = (2.0f * t3 - 3.0f * t2 + 1);  // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0f * t2 + amount);    // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0f * t3 + 3.0f * t2);     // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                    // (t^3 - t^2)

            Vector4 result;

            result.X = vector1.X * polynomial1 + tangent1.X * polynomial2 + vector2.X * polynomial3 + tangent2.X * polynomial4;
            result.Y = vector1.Y * polynomial1 + tangent1.Y * polynomial2 + vector2.Y * polynomial3 + tangent2.Y * polynomial4;
            result.Z = vector1.Z * polynomial1 + tangent1.Z * polynomial2 + vector2.Z * polynomial3 + tangent2.Z * polynomial4;
            result.W = vector1.W * polynomial1 + tangent1.W * polynomial2 + vector2.W * polynomial3 + tangent2.W * polynomial4;

            return result;
        }

        /// <summary>
        /// Performs a cubic Hermite spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="vector2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector4"/>.</param>
        public static void Hermite(ref Vector4 vector1, ref Vector4 tangent1, ref Vector4 vector2, ref Vector4 tangent2, Single amount, out Vector4 result)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            var polynomial1 = (2.0f * t3 - 3.0f * t2 + 1);  // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0f * t2 + amount);    // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0f * t3 + 3.0f * t2);     // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                    // (t^3 - t^2)
            
            result.X = vector1.X * polynomial1 + tangent1.X * polynomial2 + vector2.X * polynomial3 + tangent2.X * polynomial4;
            result.Y = vector1.Y * polynomial1 + tangent1.Y * polynomial2 + vector2.Y * polynomial3 + tangent2.Y * polynomial4;
            result.Z = vector1.Z * polynomial1 + tangent1.Z * polynomial2 + vector2.Z * polynomial3 + tangent2.Z * polynomial4;
            result.W = vector1.W * polynomial1 + tangent1.W * polynomial2 + vector2.W * polynomial3 + tangent2.W * polynomial4;
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector4"/>.</returns>
        public static Vector4 SmoothStep(Vector4 vector1, Vector4 vector2, Single amount)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = amount * amount * (3.0f - 2.0f * amount);

            Vector4 result;

            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;
            result.Z = vector1.Z + (vector2.Z - vector1.Z) * amount;
            result.W = vector1.W + (vector2.W - vector1.W) * amount;

            return result;
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector4"/>.</param>
        public static void SmoothStep(ref Vector4 vector1, ref Vector4 vector2, Single amount, out Vector4 result)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = amount * amount * (3.0f - 2.0f * amount);

            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;
            result.Z = vector1.Z + (vector2.Z - vector1.Z) * amount;
            result.W = vector1.W + (vector2.W - vector1.W) * amount;
        }

        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="vector3">The third vector.</param>
        /// <param name="vector4">The fourth vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector4"/>.</returns>
        public static Vector4 CatmullRom(Vector4 vector1, Vector4 vector2, Vector4 vector3, Vector4 vector4, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            Vector4 result;

            result.X = 0.5f * (2.0f * vector2.X + (-vector1.X + vector3.X) * amount + (2.0f * vector1.X - 5.0f * vector2.X + 4.0f * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0f * vector2.X - 3.0f * vector3.X + vector4.X) * t3);
            result.Y = 0.5f * (2.0f * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0f * vector1.Y - 5.0f * vector2.Y + 4.0f * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0f * vector2.Y - 3.0f * vector3.Y + vector4.Y) * t3);
            result.Z = 0.5f * (2.0f * vector2.Z + (-vector1.Z + vector3.Z) * amount + (2.0f * vector1.Z - 5.0f * vector2.Z + 4.0f * vector3.Z - vector4.Z) * t2 + (-vector1.Z + 3.0f * vector2.Z - 3.0f * vector3.Z + vector4.Z) * t3);
            result.W = 0.5f * (2.0f * vector2.W + (-vector1.W + vector3.W) * amount + (2.0f * vector1.W - 5.0f * vector2.W + 4.0f * vector3.W - vector4.W) * t2 + (-vector1.W + 3.0f * vector2.W - 3.0f * vector3.W + vector4.W) * t3);

            return result;
        }

        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="vector3">The third vector.</param>
        /// <param name="vector4">The fourth vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector4"/>.</param>
        public static void CatmullRom(ref Vector4 vector1, ref Vector4 vector2, ref Vector4 vector3, ref Vector4 vector4, Single amount, out Vector4 result)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            result.X = 0.5f * (2.0f * vector2.X + (-vector1.X + vector3.X) * amount + (2.0f * vector1.X - 5.0f * vector2.X + 4.0f * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0f * vector2.X - 3.0f * vector3.X + vector4.X) * t3);
            result.Y = 0.5f * (2.0f * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0f * vector1.Y - 5.0f * vector2.Y + 4.0f * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0f * vector2.Y - 3.0f * vector3.Y + vector4.Y) * t3);
            result.Z = 0.5f * (2.0f * vector2.Z + (-vector1.Z + vector3.Z) * amount + (2.0f * vector1.Z - 5.0f * vector2.Z + 4.0f * vector3.Z - vector4.Z) * t2 + (-vector1.Z + 3.0f * vector2.Z - 3.0f * vector3.Z + vector4.Z) * t3);
            result.W = 0.5f * (2.0f * vector2.W + (-vector1.W + vector3.W) * amount + (2.0f * vector1.W - 5.0f * vector2.W + 4.0f * vector3.W - vector4.W) * t2 + (-vector1.W + 3.0f * vector2.W - 3.0f * vector3.W + vector4.W) * t3);
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The distance between the specified coordinates.</returns>
        public static Single Distance(Vector4 vector1, Vector4 vector2)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;
            var dw = vector2.W - vector1.W;

            return (Single)Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The distance between the specified coordinates.</param>
        public static void Distance(ref Vector4 vector1, ref Vector4 vector2, out Single result)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;
            var dw = vector2.W - vector1.W;

            result = (Single)Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        /// <summary>
        /// Calculates the square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The square of the distance between the specified coordinates.</returns>
        public static Single DistanceSquared(Vector4 vector1, Vector4 vector2)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;
            var dw = vector2.W - vector1.W;

            return dx * dx + dy * dy + dz * dz + dw * dw;
        }

        /// <summary>
        /// Calculates square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The square of the distance between the specified coordinates.</param>
        public static void DistanceSquared(ref Vector4 vector1, ref Vector4 vector2, out Single result)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;
            var dw = vector2.W - vector1.W;

            result = dx * dx + dy * dy + dz * dz + dw * dw;
        }

        /// <summary>
        /// Computes the Cartesian coordinates of a point specified in areal Barycentric coordinates relative to a triangle.
        /// </summary>
        /// <param name="v1">The first vertex of the triangle.</param>
        /// <param name="v2">The second vertex of the triangle.</param>
        /// <param name="v3">The third vertex of the triangle.</param>
        /// <param name="b2">Barycentric coordinate b2, which expresses the weighting factor towards the second triangle vertex.</param>
        /// <param name="b3">Barycentric coordinate b3, which expresses the weighting factor towards the third triangle vertex.</param>
        /// <returns>A <see cref="Vector4"/> containing the Cartesian coordinates of the specified point.</returns>
        public static Vector4 Barycentric(Vector4 v1, Vector4 v2, Vector4 v3, Single b2, Single b3)
        {
            var b1 = (1 - b2 - b3);

            Vector4 result;

            result.X = (b1 * v1.X) + (b2 * v2.X) + (b3 * v3.X);
            result.Y = (b1 * v1.Y) + (b2 * v2.Y) + (b3 * v3.Y);
            result.Z = (b1 * v1.Z) + (b2 * v2.Z) + (b3 * v3.Z);
            result.W = (b1 * v1.W) + (b2 * v2.W) + (b3 * v3.W);

            return result;
        }

        /// <summary>
        /// Computes the Cartesian coordinates of a point specified in areal Barycentric coordinates relative to a triangle.
        /// </summary>
        /// <param name="v1">The first vertex of the triangle.</param>
        /// <param name="v2">The second vertex of the triangle.</param>
        /// <param name="v3">The third vertex of the triangle.</param>
        /// <param name="b2">Barycentric coordinate b2, which expresses the weighting factor towards the second triangle vertex.</param>
        /// <param name="b3">Barycentric coordinate b3, which expresses the weighting factor towards the third triangle vertex.</param>
        /// <param name="result">A <see cref="Vector4"/> containing the Cartesian coordinates of the specified point.</param>
        public static void Barycentric(ref Vector4 v1, ref Vector4 v2, ref Vector4 v3, Single b2, Single b3, out Vector4 result)
        {
            var b1 = (1 - b2 - b3);

            result.X = (b1 * v1.X) + (b2 * v2.X) + (b3 * v3.X);
            result.Y = (b1 * v1.Y) + (b2 * v2.Y) + (b3 * v3.Y);
            result.Z = (b1 * v1.Z) + (b2 * v2.Z) + (b3 * v3.Z);
            result.W = (b1 * v1.W) + (b2 * v2.W) + (b3 * v3.W);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{X:{X} Y:{Y} Z:{Z} W:{W}}}";

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public Single Length()
        {
            return (Single)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <returns>The squared length of the vector.</returns>
        public Single LengthSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Vector4 Interpolate(Vector4 target, Single t)
        {
            Vector4 result;

            result.X = Tweening.Lerp(this.X, target.X, t);
            result.Y = Tweening.Lerp(this.Y, target.Y, t);
            result.Z = Tweening.Lerp(this.Z, target.Z, t);
            result.W = Tweening.Lerp(this.W, target.W, t);

            return result;
        }

        /// <summary>
        /// Gets a vector with all four components set to zero.
        /// </summary>
        public static Vector4 Zero
        {
            get { return new Vector4(0, 0, 0, 0); }
        }

        /// <summary>
        /// Gets a vector with all four components set to one.
        /// </summary>
        public static Vector4 One
        {
            get { return new Vector4(1, 1, 1, 1); }
        }

        /// <summary>
        /// Returns the unit vector for the x-axis.
        /// </summary>
        public static Vector4 UnitX
        {
            get { return new Vector4(1, 0, 0, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the y-axis.
        /// </summary>
        public static Vector4 UnitY
        {
            get { return new Vector4(0, 1, 0, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the z-axis.
        /// </summary>
        public static Vector4 UnitZ
        {
            get { return new Vector4(0, 0, 1, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the w-axis.
        /// </summary>
        public static Vector4 UnitW
        {
            get { return new Vector4(0, 0, 0, 1); }
        }

        /// <summary>
        /// The vector's x-coordinate.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [FieldOffset(0)]
        public Single X;

        /// <summary>
        /// The vector's y-coordinate.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [FieldOffset(4)]
        public Single Y;

        /// <summary>
        /// The vector's z-coordinate.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [FieldOffset(8)]
        public Single Z;

        /// <summary>
        /// The vector's w-coordinate.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [FieldOffset(12)]
        public Single W;
    }
}
