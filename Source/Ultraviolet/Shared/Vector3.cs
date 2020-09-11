using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a three-dimensional vector.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(Single) * 3)]
    public partial struct Vector3 : IEquatable<Vector3>, IInterpolatable<Vector3>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> structure with all of its components set to the specified value.
        /// </summary>
        /// <param name="value">The value to which to set the vector's components.</param>
        public Vector3(Single value)
        {
            this.X = value;
            this.Y = value;
            this.Z = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> structure with the specified x, y, and z component.
        /// </summary>
        /// <param name="x">The vector's x component.</param>
        /// <param name="y">The vector's y component.</param>
        /// <param name="z">The vector's z component.</param>
        [JsonConstructor]
        public Vector3(Single x, Single y, Single z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> structure with its x and y components set to the 
        /// x and y components of the specified vector, and its z component set to the specified value.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> from which to set the vector's x and y components.</param>
        /// <param name="z">The vector's z component.</param>
        public Vector3(Vector2 vector, Single z)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = z;
        }

        /// <summary>
        /// Implicitly converts an instance of the <see cref="System.Numerics.Vector3"/> structure
        /// to an instance of the <see cref="Vector3"/> structure.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static unsafe implicit operator Vector3(System.Numerics.Vector3 value)
        {
            var x = (Vector3*)&value;
            return *x;
        }

        /// <summary>
        /// Implicitly converts an instance of the <see cref="Vector3"/> structure
        /// to an instance of the <see cref="System.Numerics.Vector3"/> structure.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static unsafe implicit operator System.Numerics.Vector3(Vector3 value)
        {
            var x = (System.Numerics.Vector3*)&value;
            return *x;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the addition operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator +(Vector3 vector1, Vector3 vector2)
        {
            Vector3 result;

            result.X = vector1.X + vector2.X;
            result.Y = vector1.Y + vector2.Y;
            result.Z = vector1.Z + vector2.Z;

            return result;
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the subtraction operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator -(Vector3 vector1, Vector3 vector2)
        {
            Vector3 result;

            result.X = vector1.X - vector2.X;
            result.Y = vector1.Y - vector2.Y;
            result.Z = vector1.Z - vector2.Z;

            return result;
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator *(Vector3 vector1, Vector3 vector2)
        {
            Vector3 result;

            result.X = vector1.X * vector2.X;
            result.Y = vector1.Y * vector2.Y;
            result.Z = vector1.Z * vector2.Z;

            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator *(Single factor, Vector3 vector)
        {
            Vector3 result;

            result.X = vector.X * factor;
            result.Y = vector.Y * factor;
            result.Z = vector.Z * factor;

            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator *(Vector3 vector, Single factor)
        {
            Vector3 result;

            result.X = vector.X * factor;
            result.Y = vector.Y * factor;
            result.Z = vector.Z * factor;

            return result;
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator /(Vector3 vector1, Vector3 vector2)
        {
            Vector3 result;

            result.X = vector1.X / vector2.X;
            result.Y = vector1.Y / vector2.Y;
            result.Z = vector1.Z / vector2.Z;

            return result;
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator /(Vector3 vector, Single factor)
        {
            Vector3 result;

            result.X = vector.X / factor;
            result.Y = vector.Y / factor;
            result.Z = vector.Z / factor;

            return result;
        }

        /// <summary>
        /// Reverses the signs of a vector's components.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to reverse.</param>
        /// <returns>The reversed <see cref="Vector3"/>.</returns>
        public static Vector3 operator -(Vector3 vector)
        {
            Vector3 result;

            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector3"/> structure to a <see cref="Size3"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size3(Vector3 vector)
        {
            Size3 result;

            result.Width = (Int32)vector.X;
            result.Height = (Int32)vector.Y;
            result.Depth = (Int32)vector.Z;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector3"/> structure to a <see cref="Size3F"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size3F(Vector3 vector)
        {
            Size3F result;

            result.Width = vector.X;
            result.Height = vector.Y;
            result.Depth = vector.Z;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector3"/> structure to a <see cref="Size3D"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size3D(Vector3 vector)
        {
            Size3D result;

            result.Width = vector.X;
            result.Height = vector.Y;
            result.Depth = vector.Z;

            return result;
        }
        
        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <returns>The cross product of the specified vectors.</returns>
        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            var cx = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            var cy = vector1.Z * vector2.X - vector1.X * vector2.Z;
            var cz = vector1.X * vector2.Y - vector1.Y * vector2.X;

            Vector3 result;

            result.X = cx;
            result.Y = cy;
            result.Z = cz;

            return result;
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="result">The cross product of the specified vectors.</param>
        public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            var cx = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            var cy = vector1.Z * vector2.X - vector1.X * vector2.Z;
            var cz = vector1.X * vector2.Y - vector1.Y * vector2.X;

            result.X = cx;
            result.Y = cy;
            result.Z = cz;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <returns>The dot product of the specified vectors.</returns>
        public static Single Dot(Vector3 vector1, Vector3 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="result">The dot product of the specified vectors.</param>
        public static void Dot(ref Vector3 vector1, ref Vector3 vector2, out Single result)
        {
            result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the addition operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Add(Vector3 left, Vector3 right)
        {
            Vector3 result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;

            return result;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the addition operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the addition operator.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Add(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the subtraction operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Subtract(Vector3 left, Vector3 right)
        {
            Vector3 result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;

            return result;
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the subtraction operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the subtraction operator.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Subtract(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the multiplication operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Multiply(Vector3 left, Vector3 right)
        {
            Vector3 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;

            return result;
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the multiplication operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the multiplication operator.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Multiply(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Multiply(Vector3 vector, Single factor)
        {
            Vector3 result;

            result.X = vector.X * factor;
            result.Y = vector.Y * factor;
            result.Z = vector.Z * factor;

            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Multiply(ref Vector3 vector, Single factor, out Vector3 result)
        {
            result.X = vector.X * factor;
            result.Y = vector.Y * factor;
            result.Z = vector.Z * factor;
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the division operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Divide(Vector3 left, Vector3 right)
        {
            Vector3 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;

            return result;
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the division operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the division operator.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Divide(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Divide(Vector3 vector, Single factor)
        {
            Vector3 result;

            result.X = vector.X / factor;
            result.Y = vector.Y / factor;
            result.Z = vector.Z / factor;

            return result;
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Divide(ref Vector3 vector, Single factor, out Vector3 result)
        {
            result.X = vector.X / factor;
            result.Y = vector.Y / factor;
            result.Z = vector.Z / factor;
        }

        /// <summary>
        /// Transforms a vector by a quaternion.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector3"/>.</returns>
        public static Vector3 Transform(Vector3 vector, Quaternion quaternion)
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

            Vector3 result;

            result.X = vector.X * (1.0f - yy2 - zz2) + vector.Y * (xy2 - wz2) + vector.Z * (xz2 + wy2);
            result.Y = vector.X * (xy2 + wz2) + vector.Y * (1.0f - xx2 - zz2) + vector.Z * (yz2 - wx2);
            result.Z = vector.X * (xz2 - wy2) + vector.Y * (yz2 + wx2) + vector.Z * (1.0f - xx2 - yy2);

            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector3"/>.</param>
        public static void Transform(ref Vector3 vector, ref Quaternion quaternion, out Vector3 result)
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

            Vector3 temp;

            temp.X = vector.X * (1.0f - yy2 - zz2) + vector.Y * (xy2 - wz2) + vector.Z * (xz2 + wy2);
            temp.Y = vector.X * (xy2 + wz2) + vector.Y * (1.0f - xx2 - zz2) + vector.Z * (yz2 - wx2);
            temp.Z = vector.X * (xz2 - wy2) + vector.Y * (yz2 + wx2) + vector.Z * (1.0f - xx2 - yy2);

            result = temp;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector3"/>.</returns>
        public static Vector3 Transform(Vector3 vector, Matrix matrix)
        {
            Vector3 result;

            result.X = (matrix.M11 * vector.X + matrix.M21 * vector.Y + matrix.M31 * vector.Z) + matrix.M41;
            result.Y = (matrix.M12 * vector.X + matrix.M22 * vector.Y + matrix.M32 * vector.Z) + matrix.M42;
            result.Z = (matrix.M13 * vector.X + matrix.M23 * vector.Y + matrix.M33 * vector.Z) + matrix.M43;

            return result;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector3"/>.</param>
        public static void Transform(ref Vector3 vector, ref Matrix matrix, out Vector3 result)
        {
            Vector3 temp;

            temp.X = (matrix.M11 * vector.X + matrix.M21 * vector.Y + matrix.M31 * vector.Z) + matrix.M41;
            temp.Y = (matrix.M12 * vector.X + matrix.M22 * vector.Y + matrix.M32 * vector.Z) + matrix.M42;
            temp.Z = (matrix.M13 * vector.X + matrix.M23 * vector.Y + matrix.M33 * vector.Z) + matrix.M43;

            result = temp;
        }

        /// <summary>
        /// Transforms a vector normal by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector3"/>.</returns>
        public static Vector3 TransformNormal(Vector3 vector, Matrix matrix)
        {
            Vector3 result;

            result.X = (matrix.M11 * vector.X + matrix.M21 * vector.Y + matrix.M31 * vector.Z);
            result.Y = (matrix.M12 * vector.X + matrix.M22 * vector.Y + matrix.M32 * vector.Z);
            result.Z = (matrix.M13 * vector.X + matrix.M23 * vector.Y + matrix.M33 * vector.Z);

            return result;
        }

        /// <summary>
        /// Transforms a vector normal by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector3"/>.</param>
        public static void TransformNormal(ref Vector3 vector, ref Matrix matrix, out Vector3 result)
        {
            Vector3 temp;

            temp.X = (matrix.M11 * vector.X + matrix.M21 * vector.Y + matrix.M31 * vector.Z);
            temp.Y = (matrix.M12 * vector.X + matrix.M22 * vector.Y + matrix.M32 * vector.Z);
            temp.Z = (matrix.M13 * vector.X + matrix.M23 * vector.Y + matrix.M33 * vector.Z);

            result = temp;
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to normalize.</param>
        /// <returns>The normalized <see cref="Vector3"/>.</returns>
        public static Vector3 Normalize(Vector3 vector)
        {
            var magnitude = (Single)Math.Sqrt(
                vector.X * vector.X +
                vector.Y * vector.Y +
                vector.Z * vector.Z);
            var inverseMagnitude = 1f / magnitude;

            Vector3 result;

            result.X = vector.X * inverseMagnitude;
            result.Y = vector.Y * inverseMagnitude;
            result.Z = vector.Z * inverseMagnitude;

            return result;
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to normalize.</param>
        /// <param name="result">The normalized <see cref="Vector3"/>.</param>
        public static void Normalize(ref Vector3 vector, out Vector3 result)
        {
            var magnitude = (Single)Math.Sqrt(
                vector.X * vector.X +
                vector.Y * vector.Y +
                vector.Z * vector.Z);
            var inverseMagnitude = 1f / magnitude;

            result.X = vector.X * inverseMagnitude;
            result.Y = vector.Y * inverseMagnitude;
            result.Z = vector.Z * inverseMagnitude;
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to negate.</param>
        /// <returns>The negated <see cref="Vector3"/>.</returns>
        public static Vector3 Negate(Vector3 vector)
        {
            Vector3 result;

            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;

            return result;
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to negate.</param>
        /// <param name="result">The negated <see cref="Vector3"/>.</param>
        public static void Negate(ref Vector3 vector, out Vector3 result)
        {
            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped <see cref="Vector3"/>.</returns>
        public static Vector3 Clamp(Vector3 vector, Vector3 min, Vector3 max)
        {
            Vector3 result;

            result.X = vector.X < min.X ? min.X : vector.X > max.X ? max.X : vector.X;
            result.Y = vector.Y < min.Y ? min.Y : vector.Y > max.Y ? max.Y : vector.Y;
            result.Z = vector.Z < min.Z ? min.Z : vector.Z > max.Z ? max.Z : vector.Z;

            return result;
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">The clamped <see cref="Vector3"/>.</param>
        public static void Clamp(ref Vector3 vector, ref Vector3 min, ref Vector3 max, out Vector3 result)
        {
            result.X = vector.X < min.X ? min.X : vector.X > max.X ? max.X : vector.X;
            result.Y = vector.Y < min.Y ? min.Y : vector.Y > max.Y ? max.Y : vector.Y;
            result.Z = vector.Z < min.Z ? min.Z : vector.Z > max.Z ? max.Z : vector.Z;
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <returns>A <see cref="Vector3"/> which contains the lowest value of each component of the specified vectors.</returns>
        public static Vector3 Min(Vector3 vector1, Vector3 vector2)
        {
            Vector3 result;

            result.X = vector1.X < vector2.X ? vector1.X : vector2.X;
            result.Y = vector1.Y < vector2.Y ? vector1.Y : vector2.Y;
            result.Z = vector1.Z < vector2.Z ? vector1.Z : vector2.Z;

            return result;
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="result">A <see cref="Vector3"/> which contains the lowest value of each component of the specified vectors.</param>
        public static void Min(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            result.X = vector1.X < vector2.X ? vector1.X : vector2.X;
            result.Y = vector1.Y < vector2.Y ? vector1.Y : vector2.Y;
            result.Z = vector1.Z < vector2.Z ? vector1.Z : vector2.Z;
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <returns>A <see cref="Vector3"/> which contains the highest value of each component of the specified vectors.</returns>
        public static Vector3 Max(Vector3 vector1, Vector3 vector2)
        {
            Vector3 result;
            
            result.X = vector1.X < vector2.X ? vector2.X : vector1.X;
            result.Y = vector1.Y < vector2.Y ? vector2.Y : vector1.Y;
            result.Z = vector1.Z < vector2.Z ? vector2.Z : vector1.Z;

            return result;
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="result">A <see cref="Vector3"/> which contains the highest value of each component of the specified vectors.</param>
        public static void Max(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            result.X = vector1.X < vector2.X ? vector2.X : vector1.X;
            result.Y = vector1.Y < vector2.Y ? vector2.Y : vector1.Y;
            result.Z = vector1.Z < vector2.Z ? vector2.Z : vector1.Z;
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <returns>The reflected <see cref="Vector3"/>.</returns>
        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            var dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;

            Vector3 result;

            result.X = vector.X - 2f * dot * normal.X;
            result.Y = vector.Y - 2f * dot * normal.Y;
            result.Z = vector.Z - 2f * dot * normal.Z;

            return result;
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <param name="result">The reflected <see cref="Vector3"/>.</param>
        public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
        {
            var dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;

            result.X = vector.X - 2f * dot * normal.X;
            result.Y = vector.Y - 2f * dot * normal.Y;
            result.Z = vector.Z - 2f * dot * normal.Z;
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector3"/>.</returns>
        public static Vector3 Lerp(Vector3 vector1, Vector3 vector2, Single amount)
        {
            Vector3 result;

            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;
            result.Z = vector1.Z + (vector2.Z - vector1.Z) * amount;

            return result;
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector3"/>.</param>
        public static void Lerp(ref Vector3 vector1, ref Vector3 vector2, Single amount, out Vector3 result)
        {
            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;
            result.Z = vector1.Z + (vector2.Z - vector1.Z) * amount;            
        }

        /// <summary>
        /// Performs a cubic Hermite spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="vector2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector3"/>.</returns>
        public static Vector3 Hermite(Vector3 vector1, Vector3 tangent1, Vector3 vector2, Vector3 tangent2, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            var polynomial1 = (2.0f * t3 - 3.0f * t2 + 1);  // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0f * t2 + amount);    // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0f * t3 + 3.0f * t2);     // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                    // (t^3 - t^2)

            Vector3 result;

            result.X = vector1.X * polynomial1 + tangent1.X * polynomial2 + vector2.X * polynomial3 + tangent2.X * polynomial4;
            result.Y = vector1.Y * polynomial1 + tangent1.Y * polynomial2 + vector2.Y * polynomial3 + tangent2.Y * polynomial4;
            result.Z = vector1.Z * polynomial1 + tangent1.Z * polynomial2 + vector2.Z * polynomial3 + tangent2.Z * polynomial4;

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
        /// <param name="result">The interpolated vector.</param>
        public static void Hermite(ref Vector3 vector1, ref Vector3 tangent1, ref Vector3 vector2, ref Vector3 tangent2, Single amount, out Vector3 result)
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
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector3"/>.</returns>
        public static Vector3 SmoothStep(Vector3 vector1, Vector3 vector2, Single amount)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = amount * amount * (3.0f - 2.0f * amount);

            Vector3 result;

            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;
            result.Z = vector1.Z + (vector2.Z - vector1.Z) * amount;

            return result;
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector3"/>.</param>
        public static void SmoothStep(ref Vector3 vector1, ref Vector3 vector2, Single amount, out Vector3 result)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = amount * amount * (3.0f - 2.0f * amount);

            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;
            result.Z = vector1.Z + (vector2.Z - vector1.Z) * amount;
        }

        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="vector3">The third vector.</param>
        /// <param name="vector4">The fourth vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector3"/>.</returns>
        public static Vector3 CatmullRom(Vector3 vector1, Vector3 vector2, Vector3 vector3, Vector3 vector4, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            Vector3 result;

            result.X = 0.5f * (2.0f * vector2.X + (-vector1.X + vector3.X) * amount + (2.0f * vector1.X - 5.0f * vector2.X + 4.0f * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0f * vector2.X - 3.0f * vector3.X + vector4.X) * t3);
            result.Y = 0.5f * (2.0f * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0f * vector1.Y - 5.0f * vector2.Y + 4.0f * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0f * vector2.Y - 3.0f * vector3.Y + vector4.Y) * t3);
            result.Z = 0.5f * (2.0f * vector2.Z + (-vector1.Z + vector3.Z) * amount + (2.0f * vector1.Z - 5.0f * vector2.Z + 4.0f * vector3.Z - vector4.Z) * t2 + (-vector1.Z + 3.0f * vector2.Z - 3.0f * vector3.Z + vector4.Z) * t3);

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
        /// <param name="result">The interpolated <see cref="Vector3"/>.</param>
        public static void CatmullRom(ref Vector3 vector1, ref Vector3 vector2, ref Vector3 vector3, ref Vector3 vector4, Single amount, out Vector3 result)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            result.X = 0.5f * (2.0f * vector2.X + (-vector1.X + vector3.X) * amount + (2.0f * vector1.X - 5.0f * vector2.X + 4.0f * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0f * vector2.X - 3.0f * vector3.X + vector4.X) * t3);
            result.Y = 0.5f * (2.0f * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0f * vector1.Y - 5.0f * vector2.Y + 4.0f * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0f * vector2.Y - 3.0f * vector3.Y + vector4.Y) * t3);
            result.Z = 0.5f * (2.0f * vector2.Z + (-vector1.Z + vector3.Z) * amount + (2.0f * vector1.Z - 5.0f * vector2.Z + 4.0f * vector3.Z - vector4.Z) * t2 + (-vector1.Z + 3.0f * vector2.Z - 3.0f * vector3.Z + vector4.Z) * t3);
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The distance between the specified coordinates.</returns>
        public static Single Distance(Vector3 vector1, Vector3 vector2)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;

            return (Single)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The distance between the specified coordinates.</param>
        public static void Distance(ref Vector3 vector1, ref Vector3 vector2, out Single result)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;

            result = (Single)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Calculates the square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The square of the distance between the specified coordinates.</returns>
        public static Single DistanceSquared(Vector3 vector1, Vector3 vector2)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;

            return dx * dx + dy * dy + dz * dz;
        }

        /// <summary>
        /// Calculates the square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The square of the distance between the specified coordinates.</param>
        public static void DistanceSquared(ref Vector3 vector1, ref Vector3 vector2, out Single result)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;

            result = dx * dx + dy * dy + dz * dz;
        }

        /// <summary>
        /// Computes the Cartesian coordinates of a point specified in areal Barycentric coordinates relative to a triangle.
        /// </summary>
        /// <param name="v1">The first vertex of the triangle.</param>
        /// <param name="v2">The second vertex of the triangle.</param>
        /// <param name="v3">The third vertex of the triangle.</param>
        /// <param name="b2">Barycentric coordinate b2, which expresses the weighting factor towards the second triangle vertex.</param>
        /// <param name="b3">Barycentric coordinate b3, which expresses the weighting factor towards the third triangle vertex.</param>
        /// <returns>A <see cref="Vector3"/> containing the Cartesian coordinates of the specified point.</returns>
        public static Vector3 Barycentric(Vector3 v1, Vector3 v2, Vector3 v3, Single b2, Single b3)
        {
            var b1 = (1 - b2 - b3);

            Vector3 result;

            result.X = (b1 * v1.X) + (b2 * v2.X) + (b3 * v3.X);
            result.Y = (b1 * v1.Y) + (b2 * v2.Y) + (b3 * v3.Y);
            result.Z = (b1 * v1.Z) + (b2 * v2.Z) + (b3 * v3.Z);

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
        /// <param name="result">A <see cref="Vector3"/> containing the Cartesian coordinates of the specified point.</param>
        public static void Barycentric(ref Vector3 v1, ref Vector3 v2, ref Vector3 v3, Single b2, Single b3, out Vector3 result)
        {
            var b1 = (1 - b2 - b3);

            result.X = (b1 * v1.X) + (b2 * v2.X) + (b3 * v3.X);
            result.Y = (b1 * v1.Y) + (b2 * v2.Y) + (b3 * v3.Y);
            result.Z = (b1 * v1.Z) + (b2 * v2.Z) + (b3 * v3.Z);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{X:{X} Y:{Y} Z:{Z}}}";

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public Single Length()
        {
            return (Single)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <returns>The squared length of the vector.</returns>
        public Single LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Vector3 Interpolate(Vector3 target, Single t)
        {
            Vector3 result;

            result.X = Tweening.Lerp(this.X, target.X, t);
            result.Y = Tweening.Lerp(this.Y, target.Y, t);
            result.Z = Tweening.Lerp(this.Z, target.Z, t);

            return result;
        }

        /// <summary>
        /// Gets a vector with all three components set to zero.
        /// </summary>
        public static Vector3 Zero
        {
            get { return new Vector3(0, 0, 0); }
        }

        /// <summary>
        /// Gets a vector with all three components set to one.
        /// </summary>
        public static Vector3 One
        {
            get { return new Vector3(1, 1, 1); }
        }

        /// <summary>
        /// Returns the unit vector for the x-axis.
        /// </summary>
        public static Vector3 UnitX
        {
            get { return new Vector3(1, 0, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the y-axis.
        /// </summary>
        public static Vector3 UnitY
        {
            get { return new Vector3(0, 1, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the z-axis.
        /// </summary>
        public static Vector3 UnitZ
        {
            get { return new Vector3(0, 0, 1); }
        }

        /// <summary>
        /// Returns a unit vector representing right in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Right
        {
            get { return new Vector3(1, 0, 0); }
        }

        /// <summary>
        /// Returns a unit vector representing left in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Left
        {
            get { return new Vector3(-1, 0, 0); }
        }

        /// <summary>
        /// Returns a unit vector representing up in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Up
        {
            get { return new Vector3(0, 1, 0); }
        }

        /// <summary>
        /// Returns a unit vector representing down in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Down
        {
            get { return new Vector3(0, -1, 0); }
        }

        /// <summary>
        /// Returns a unit vector representing backward in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Backward
        {
            get { return new Vector3(0, 0, 1); }
        }

        /// <summary>
        /// Returns a unit vector representing forward in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Forward
        {
            get { return new Vector3(0, 0, -1); }
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
    }
}
