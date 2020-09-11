using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a two-dimensional vector.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = sizeof(Single) * 2)]
    public partial struct Vector2 : IEquatable<Vector2>, IInterpolatable<Vector2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> structure with all of its components set to the specified value.
        /// </summary>
        /// <param name="value">The value to which to set the vector's components.</param>
        public Vector2(Single value)
        {
            this.X = value;
            this.Y = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> structure with the specified x and y components.
        /// </summary>
        /// <param name="x">The vector's x component.</param>
        /// <param name="y">The vector's y component.</param>
        [JsonConstructor]
        public Vector2(Single x, Single y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Implicitly converts an instance of the <see cref="System.Numerics.Vector2"/> structure
        /// to an instance of the <see cref="Vector2"/> structure.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static unsafe implicit operator Vector2(System.Numerics.Vector2 value)
        {
            var x = (Vector2*)&value;
            return *x;
        }

        /// <summary>
        /// Implicitly converts an instance of the <see cref="Vector2"/> structure
        /// to an instance of the <see cref="System.Numerics.Vector2"/> structure.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static unsafe implicit operator System.Numerics.Vector2(Vector2 value)
        {
            var x = (System.Numerics.Vector2*)&value;
            return *x;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the addition operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator +(Vector2 vector1, Vector2 vector2)
        {
            Vector2 result;

            result.X = vector1.X + vector2.X;
            result.Y = vector1.Y + vector2.Y;

            return result;
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the subtraction operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator -(Vector2 vector1, Vector2 vector2)
        {
            Vector2 result;

            result.X = vector1.X - vector2.X;
            result.Y = vector1.Y - vector2.Y;

            return result;
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator *(Vector2 vector1, Vector2 vector2)
        {
            Vector2 result;

            result.X = vector1.X * vector2.X;
            result.Y = vector1.Y * vector2.Y;

            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator *(Single factor, Vector2 vector)
        {
            Vector2 result;

            result.X = vector.X * factor;
            result.Y = vector.Y * factor;

            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator *(Vector2 vector, Single factor)
        {
            Vector2 result;

            result.X = vector.X * factor;
            result.Y = vector.Y * factor;

            return result;
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator /(Vector2 vector1, Vector2 vector2)
        {
            Vector2 result;

            result.X = vector1.X / vector2.X;
            result.Y = vector1.Y / vector2.Y;

            return result;
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator /(Vector2 vector, Single factor)
        {
            Vector2 result;

            result.X = vector.X / factor;
            result.Y = vector.Y / factor;

            return result;
        }

        /// <summary>
        /// Reverses the signs of a vector's components.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to reverse.</param>
        /// <returns>The reversed <see cref="Vector2"/>.</returns>
        public static Vector2 operator -(Vector2 vector)
        {
            Vector2 result;

            result.X = -vector.X;
            result.Y = -vector.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector2"/> structure to a <see cref="Point2"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Point2(Vector2 vector)
        {
            Point2 result;

            result.X = (Int32)vector.X;
            result.Y = (Int32)vector.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector2"/> structure to a <see cref="Point2F"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Point2F(Vector2 vector)
        {
            Point2F result;

            result.X = vector.X;
            result.Y = vector.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector2"/> structure to a <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Point2D(Vector2 vector)
        {
            Point2D result;

            result.X = vector.X;
            result.Y = vector.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector2"/> structure to a <see cref="Size2"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size2(Vector2 vector)
        {
            Size2 result;

            result.Width = (Int32)vector.X;
            result.Height = (Int32)vector.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector2"/> structure to a <see cref="Size2F"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size2F(Vector2 vector)
        {
            Size2F result;

            result.Width = vector.X;
            result.Height = vector.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector2"/> structure to a <see cref="Size2D"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size2D(Vector2 vector)
        {
            Size2D result;

            result.Width = vector.X;
            result.Height = vector.Y;

            return result;
        }
        
        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <returns>The dot product of the specified vectors.</returns>
        public static Single Dot(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">The dot product of the specified vectors.</param>
        public static void Dot(ref Vector2 vector1, ref Vector2 vector2, out Single result)
        {
            result = vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the addition operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Add(Vector2 vector1, Vector2 vector2)
        {
            Vector2 result;

            result.X = vector1.X + vector2.X;
            result.Y = vector1.Y + vector2.Y;

            return result;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the addition operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the addition operator.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Add(ref Vector2 vector1, ref Vector2 vector2, out Vector2 result)
        {
            result.X = vector1.X + vector2.X;
            result.Y = vector1.Y + vector2.Y;
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the subtraction operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Subtract(Vector2 vector1, Vector2 vector2)
        {
            Vector2 result;

            result.X = vector1.X - vector2.X;
            result.Y = vector1.Y - vector2.Y;

            return result;
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the subtraction operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the subtraction operator.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Subtract(ref Vector2 vector1, ref Vector2 vector2, out Vector2 result)
        {
            result.X = vector1.X - vector2.X;
            result.Y = vector1.Y - vector2.Y;
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector2"/> to the left of the multiplication operator.</param>
        /// <param name="right">The <see cref="Vector2"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Multiply(Vector2 left, Vector2 right)
        {
            Vector2 result;

            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;

            return result;
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector2"/> to the left of the multiplication operator.</param>
        /// <param name="right">The <see cref="Vector2"/> to the right of the multiplication operator.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Multiply(ref Vector2 left, ref Vector2 right, out Vector2 result)
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Multiply(Vector2 vector, Single factor)
        {
            Vector2 result;

            result.X = vector.X * factor;
            result.Y = vector.Y * factor;

            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Multiply(ref Vector2 vector, Single factor, out Vector2 result)
        {
            result.X = vector.X * factor;
            result.Y = vector.Y * factor;
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector2"/> to the left of the division operator.</param>
        /// <param name="right">The <see cref="Vector2"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Divide(Vector2 left, Vector2 right)
        {
            Vector2 result;

            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;

            return result;
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector2"/> to the left of the division operator.</param>
        /// <param name="right">The <see cref="Vector2"/> to the right of the division operator.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Divide(ref Vector2 left, ref Vector2 right, out Vector2 result)
        {
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Divide(Vector2 vector, Single factor)
        {
            Vector2 result;

            result.X = vector.X / factor;
            result.Y = vector.Y / factor;

            return result;
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Divide(ref Vector2 vector, Single factor, out Vector2 result)
        {
            result.X = vector.X / factor;
            result.Y = vector.Y / factor;
        }

        /// <summary>
        /// Transforms a vector by a quaternion.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector2"/>.</returns>
        public static Vector2 Transform(Vector2 vector, Quaternion quaternion)
        {
            var x2 = quaternion.X + quaternion.X;
            var y2 = quaternion.Y + quaternion.Y;
            var z2 = quaternion.Z + quaternion.Z;

            var wz2 = quaternion.W * z2;
            var xx2 = quaternion.X * x2;
            var xy2 = quaternion.X * y2;
            var yy2 = quaternion.Y * y2;
            var zz2 = quaternion.Z * z2;

            Vector2 result;

            result.X = vector.X * (1.0f - yy2 - zz2) + vector.Y * (xy2 - wz2);
            result.Y = vector.X * (xy2 + wz2) + vector.Y * (1.0f - xx2 - zz2);

            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector2"/>.</param>
        public static void Transform(ref Vector2 vector, ref Quaternion quaternion, out Vector2 result)
        {
            var x2 = quaternion.X + quaternion.X;
            var y2 = quaternion.Y + quaternion.Y;
            var z2 = quaternion.Z + quaternion.Z;

            var wz2 = quaternion.W * z2;
            var xx2 = quaternion.X * x2;
            var xy2 = quaternion.X * y2;
            var yy2 = quaternion.Y * y2;
            var zz2 = quaternion.Z * z2;

            Vector2 temp;

            temp.X = vector.X * (1.0f - yy2 - zz2) + vector.Y * (xy2 - wz2);
            temp.Y = vector.X * (xy2 + wz2) + vector.Y * (1.0f - xx2 - zz2);

            result = temp;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector2"/>.</returns>
        public static Vector2 Transform(Vector2 vector, Matrix matrix)
        {
            Vector2 result;

            result.X = (matrix.M11 * vector.X + matrix.M21 * vector.Y) + matrix.M41;
            result.Y = (matrix.M12 * vector.X + matrix.M22 * vector.Y) + matrix.M42;

            return result;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector2"/>.</param>
        public static void Transform(ref Vector2 vector, ref Matrix matrix, out Vector2 result)
        {
            Vector2 temp;

            temp.X = (matrix.M11 * vector.X + matrix.M21 * vector.Y) + matrix.M41;
            temp.Y = (matrix.M12 * vector.X + matrix.M22 * vector.Y) + matrix.M42;

            result = temp;
        }

        /// <summary>
        /// Transforms a vector normal by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector2"/>.</returns>
        public static Vector2 TransformNormal(Vector2 vector, Matrix matrix)
        {
            Vector2 result;

            result.X = (matrix.M11 * vector.X + matrix.M21 * vector.Y);
            result.Y = (matrix.M12 * vector.X + matrix.M22 * vector.Y);

            return result;
        }

        /// <summary>
        /// Transforms a vector normal by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector2"/>.</param>
        public static void TransformNormal(ref Vector2 vector, ref Matrix matrix, out Vector2 result)
        {
            Vector2 temp;

            temp.X = (matrix.M11 * vector.X + matrix.M21 * vector.Y);
            temp.Y = (matrix.M12 * vector.X + matrix.M22 * vector.Y);

            result = temp;
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to normalize.</param>
        /// <returns>The normalized <see cref="Vector2"/>.</returns>
        public static Vector2 Normalize(Vector2 vector)
        {
            var magnitude = (Single)Math.Sqrt(
                vector.X * vector.X +
                vector.Y * vector.Y);
            var inverseMagnitude = 1f / magnitude;

            Vector2 result;

            result.X = vector.X * inverseMagnitude;
            result.Y = vector.Y * inverseMagnitude;

            return result;
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to normalize.</param>
        /// <param name="result">The normalized <see cref="Vector2"/>.</param>
        public static void Normalize(ref Vector2 vector, out Vector2 result)
        {
            var magnitude = (Single)Math.Sqrt(
                vector.X * vector.X +
                vector.Y * vector.Y);
            var inverseMagnitude = 1f / magnitude;

            result.X = vector.X * inverseMagnitude;
            result.Y = vector.Y * inverseMagnitude;
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to negate.</param>
        /// <returns>The negated <see cref="Vector2"/>.</returns>
        public static Vector2 Negate(Vector2 vector)
        {
            Vector2 result;

            result.X = -vector.X;
            result.Y = -vector.Y;

            return result;
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to negate.</param>
        /// <param name="result">The negated <see cref="Vector2"/>.</param>
        public static void Negate(ref Vector2 vector, out Vector2 result)
        {
            result.X = -vector.X;
            result.Y = -vector.Y;
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped <see cref="Vector2"/>.</returns>
        public static Vector2 Clamp(Vector2 vector, Vector2 min, Vector2 max)
        {
            Vector2 result;

            result.X = vector.X < min.X ? min.X : vector.X > max.X ? max.X : vector.X;
            result.Y = vector.Y < min.Y ? min.Y : vector.Y > max.Y ? max.Y : vector.Y;

            return result;
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">The clamped <see cref="Vector2"/>.</param>
        public static void Clamp(ref Vector2 vector, ref Vector2 min, ref Vector2 max, out Vector2 result)
        {
            result.X = vector.X < min.X ? min.X : vector.X > max.X ? max.X : vector.X;
            result.Y = vector.Y < min.Y ? min.Y : vector.Y > max.Y ? max.Y : vector.Y;
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <returns>A <see cref="Vector2"/> which contains the lowest value of each component of the specified vectors.</returns>
        public static Vector2 Min(Vector2 vector1, Vector2 vector2)
        {
            Vector2 result;

            result.X = vector1.X < vector2.X ? vector1.X : vector2.X;
            result.Y = vector1.Y < vector2.Y ? vector1.Y : vector2.Y;

            return result;
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">A <see cref="Vector2"/> which contains the lowest value of each component of the specified vectors.</param>
        public static void Min(ref Vector2 vector1, ref Vector2 vector2, out Vector2 result)
        {
            result.X = vector1.X < vector2.X ? vector1.X : vector2.X;
            result.Y = vector1.Y < vector2.Y ? vector1.Y : vector2.Y;
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <returns>A <see cref="Vector2"/> which contains the highest value of each component of the specified vectors.</returns>
        public static Vector2 Max(Vector2 vector1, Vector2 vector2)
        {
            Vector2 result;

            result.X = vector1.X < vector2.X ? vector2.X : vector1.X;
            result.Y = vector1.Y < vector2.Y ? vector2.Y : vector1.Y;

            return result;
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">A <see cref="Vector2"/> which contains the highest value of each component of the specified vectors.</param>
        public static void Max(ref Vector2 vector1, ref Vector2 vector2, out Vector2 result)
        {
            result.X = vector1.X < vector2.X ? vector2.X : vector1.X;
            result.Y = vector1.Y < vector2.Y ? vector2.Y : vector1.Y;
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <returns>The reflected <see cref="Vector2"/>.</returns>
        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
        {
            var dot = vector.X * normal.X + vector.Y * normal.Y;

            Vector2 result;

            result.X = vector.X - 2f * dot * normal.X;
            result.Y = vector.Y - 2f * dot * normal.Y;

            return result;
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <param name="result">The reflected <see cref="Vector2"/>.</param>
        public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
        {
            var dot = vector.X * normal.X + vector.Y * normal.Y;

            result.X = vector.X - 2f * dot * normal.X;
            result.Y = vector.Y - 2f * dot * normal.Y;
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector2"/>.</returns>
        public static Vector2 Lerp(Vector2 vector1, Vector2 vector2, Single amount)
        {
            Vector2 result;

            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;

            return result;
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector2"/>.</param>
        public static void Lerp(ref Vector2 vector1, ref Vector2 vector2, Single amount, out Vector2 result)
        {
            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;
        }

        /// <summary>
        /// Performs a cubic Hermite spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="vector2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector2"/>.</returns>
        public static Vector2 Hermite(Vector2 vector1, Vector2 tangent1, Vector2 vector2, Vector2 tangent2, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            var polynomial1 = (2.0f * t3 - 3.0f * t2 + 1f); // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0f * t2 + amount);    // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0f * t3 + 3.0f * t2);     // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                    // (t^3 - t^2)

            Vector2 result;

            result.X = vector1.X * polynomial1 + tangent1.X * polynomial2 + vector2.X * polynomial3 + tangent2.X * polynomial4;
            result.Y = vector1.Y * polynomial1 + tangent1.Y * polynomial2 + vector2.Y * polynomial3 + tangent2.Y * polynomial4;

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
        /// <param name="result">The interpolated <see cref="Vector2"/>.</param>
        public static void Hermite(ref Vector2 vector1, ref Vector2 tangent1, ref Vector2 vector2, ref Vector2 tangent2, Single amount, out Vector2 result)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            var polynomial1 = (2.0f * t3 - 3.0f * t2 + 1f); // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0f * t2 + amount);    // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0f * t3 + 3.0f * t2);     // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                    // (t^3 - t^2)
            
            result.X = vector1.X * polynomial1 + tangent1.X * polynomial2 + vector2.X * polynomial3 + tangent2.X * polynomial4;
            result.Y = vector1.Y * polynomial1 + tangent1.Y * polynomial2 + vector2.Y * polynomial3 + tangent2.Y * polynomial4;
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated vector.</returns>
        public static Vector2 SmoothStep(Vector2 vector1, Vector2 vector2, Single amount)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = amount * amount * (3.0f - 2.0f * amount);

            Vector2 result;

            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;

            return result;
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector2"/>.</param>
        public static void SmoothStep(ref Vector2 vector1, ref Vector2 vector2, Single amount, out Vector2 result)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = amount * amount * (3.0f - 2.0f * amount);

            result.X = vector1.X + (vector2.X - vector1.X) * amount;
            result.Y = vector1.Y + (vector2.Y - vector1.Y) * amount;
        }

        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="vector3">The third vector.</param>
        /// <param name="vector4">The fourth vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector2"/>.</returns>
        public static Vector2 CatmullRom(Vector2 vector1, Vector2 vector2, Vector2 vector3, Vector2 vector4, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            Vector2 result;

            result.X = 0.5f * (2.0f * vector2.X + (-vector1.X + vector3.X) * amount + (2.0f * vector1.X - 5.0f * vector2.X + 4.0f * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0f * vector2.X - 3.0f * vector3.X + vector4.X) * t3);
            result.Y = 0.5f * (2.0f * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0f * vector1.Y - 5.0f * vector2.Y + 4.0f * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0f * vector2.Y - 3.0f * vector3.Y + vector4.Y) * t3);

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
        /// <param name="result">The interpolated <see cref="Vector2"/>.</param>
        public static void CatmullRom(ref Vector2 vector1, ref Vector2 vector2, ref Vector2 vector3, ref Vector2 vector4, Single amount, out Vector2 result)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            result.X = 0.5f * (2.0f * vector2.X + (-vector1.X + vector3.X) * amount + (2.0f * vector1.X - 5.0f * vector2.X + 4.0f * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0f * vector2.X - 3.0f * vector3.X + vector4.X) * t3);
            result.Y = 0.5f * (2.0f * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0f * vector1.Y - 5.0f * vector2.Y + 4.0f * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0f * vector2.Y - 3.0f * vector3.Y + vector4.Y) * t3);
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The distance between the specified coordinates.</returns>
        public static Single Distance(Vector2 vector1, Vector2 vector2)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;

            return (Single)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The distance between the specified coordinates.</param>
        public static void Distance(ref Vector2 vector1, ref Vector2 vector2, out Single result)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;

            result = (Single)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculates the square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The square of the distance between the specified coordinates.</returns>
        public static Single DistanceSquared(Vector2 vector1, Vector2 vector2)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;

            return dx * dx + dy * dy;
        }

        /// <summary>
        /// Calculates the square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The square of the distance between the specified coordinates.</param>
        public static void DistanceSquared(ref Vector2 vector1, ref Vector2 vector2, out Single result)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;

            result = dx * dx + dy * dy;
        }

        /// <summary>
        /// Computes the Cartesian coordinates of a point specified in areal Barycentric coordinates relative to a triangle.
        /// </summary>
        /// <param name="v1">The first vertex of the triangle.</param>
        /// <param name="v2">The second vertex of the triangle.</param>
        /// <param name="v3">The third vertex of the triangle.</param>
        /// <param name="b2">Barycentric coordinate b2, which expresses the weighting factor towards the second triangle vertex.</param>
        /// <param name="b3">Barycentric coordinate b3, which expresses the weighting factor towards the third triangle vertex.</param>
        /// <returns>A <see cref="Vector2"/> containing the Cartesian coordinates of the specified point.</returns>
        public static Vector2 Barycentric(Vector2 v1, Vector2 v2, Vector2 v3, Single b2, Single b3)
        {
            var b1 = (1 - b2 - b3);

            Vector2 result;

            result.X = (b1 * v1.X) + (b2 * v2.X) + (b3 * v3.X);
            result.Y = (b1 * v1.Y) + (b2 * v2.Y) + (b3 * v3.Y);

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
        /// <param name="result">A <see cref="Vector2"/> containing the Cartesian coordinates of the specified point.</param>
        public static void Barycentric(ref Vector2 v1, ref Vector2 v2, ref Vector2 v3, Single b2, Single b3, out Vector2 result)
        {
            var b1 = (1 - b2 - b3);

            result.X = (b1 * v1.X) + (b2 * v2.X) + (b3 * v3.X);
            result.Y = (b1 * v1.Y) + (b2 * v2.Y) + (b3 * v3.Y);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{X:{X} Y:{Y}}}";

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public Single Length()
        {
            return (Single)Math.Sqrt(X * X + Y * Y);
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <returns>The squared length of the vector.</returns>
        public Single LengthSquared()
        {
            return X * X + Y * Y;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Vector2 Interpolate(Vector2 target, Single t)
        {
            Vector2 result;

            result.X = Tweening.Lerp(this.X, target.X, t);
            result.Y = Tweening.Lerp(this.Y, target.Y, t);

            return result;
        }

        /// <summary>
        /// Gets a vector with both components set to zero.
        /// </summary>
        public static Vector2 Zero
        {
            get { return new Vector2(0, 0); }
        }

        /// <summary>
        /// Gets a vector with both components set to one.
        /// </summary>
        public static Vector2 One
        {
            get { return new Vector2(1, 1); }
        }

        /// <summary>
        /// Returns the unit vector for the x-axis.
        /// </summary>
        public static Vector2 UnitX
        {
            get { return new Vector2(1, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the y-axis.
        /// </summary>
        public static Vector2 UnitY
        {
            get { return new Vector2(0, 1); }
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
    }
}
