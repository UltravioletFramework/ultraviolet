using System;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a four-dimensional vector (x, y, z, w) which is used to efficiently rotate an object about the (x, y, z) vector by the angle theta,
    /// where w = cos(theta/2).
    /// </summary>
    [Serializable]
    public partial struct Quaternion : IEquatable<Quaternion>, IInterpolatable<Quaternion>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the quaternion's vector component.</param>
        /// <param name="y">The y-coordinate of the quaternion's vector component.</param>
        /// <param name="z">The z-coordinate of the quaternion's vector component.</param>
        /// <param name="w">The quaternion's scalar rotation component.</param>
        [JsonConstructor]
        public Quaternion(Single x, Single y, Single z, Single w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> structure.
        /// </summary>
        /// <param name="vector">The quaternion's vector component.</param>
        /// <param name="scalar">The quaternion's scalar component.</param>
        public Quaternion(Vector3 vector, Single scalar)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
            this.W = scalar;
        }

        /// <summary>
        /// Implicitly converts an instance of the <see cref="System.Numerics.Quaternion"/> structure
        /// to an instance of the <see cref="Quaternion"/> structure.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static unsafe implicit operator Quaternion(System.Numerics.Quaternion value)
        {
            var x = (Quaternion*)&value;
            return *x;
        }

        /// <summary>
        /// Implicitly converts an instance of the <see cref="Quaternion"/> structure
        /// to an instance of the <see cref="System.Numerics.Quaternion"/> structure.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static unsafe implicit operator System.Numerics.Quaternion(Quaternion value)
        {
            var x = (System.Numerics.Quaternion*)&value;
            return *x;
        }

        /// <summary>
        /// Adds two quaternions.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the addition operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion operator +(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion result;

            result.X = quaternion1.X + quaternion2.X;
            result.Y = quaternion1.Y + quaternion2.Y;
            result.Z = quaternion1.Z + quaternion2.Z;
            result.W = quaternion1.W + quaternion2.W;

            return result;
        }

        /// <summary>
        /// Subtracts one quaternion from another quaternion.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the subtraction operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion operator -(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion result;

            result.X = quaternion1.X - quaternion2.X;
            result.Y = quaternion1.Y - quaternion2.Y;
            result.Z = quaternion1.Z - quaternion2.Z;
            result.W = quaternion1.W - quaternion2.W;

            return result;
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the multiplication operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion operator *(Quaternion quaternion1, Quaternion quaternion2)
        {
            var q1x = quaternion1.X;
            var q1y = quaternion1.Y;
            var q1z = quaternion1.Z;
            var q1w = quaternion1.W;

            var q2x = quaternion2.X;
            var q2y = quaternion2.Y;
            var q2z = quaternion2.Z;
            var q2w = quaternion2.W;

            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            Quaternion result;

            result.X = q1x * q2w + q2x * q1w + cx;
            result.Y = q1y * q2w + q2y * q1w + cy;
            result.Z = q1z * q2w + q2z * q1w + cz;
            result.W = q1w * q2w - dot;

            return result;
        }

        /// <summary>
        /// Multiplies a quaternion by a scaling factor.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the quaternion.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion operator *(Single factor, Quaternion quaternion)
        {
            Quaternion result;

            result.X = quaternion.X * factor;
            result.Y = quaternion.Y * factor;
            result.Z = quaternion.Z * factor;
            result.W = quaternion.W * factor;

            return result;
        }

        /// <summary>
        /// Multiplies a quaternion by a scaling factor.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the quaternion.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion operator *(Quaternion quaternion, Single factor)
        {
            Quaternion result;

            result.X = quaternion.X * factor;
            result.Y = quaternion.Y * factor;
            result.Z = quaternion.Z * factor;
            result.W = quaternion.W * factor;

            return result;
        }

        /// <summary>
        /// Divides two quaternions.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the division operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion operator /(Quaternion quaternion1, Quaternion quaternion2)
        {
            var q1x = quaternion1.X;
            var q1y = quaternion1.Y;
            var q1z = quaternion1.Z;
            var q1w = quaternion1.W;

            var lengthSquared =
                quaternion2.X * quaternion2.X +
                quaternion2.Y * quaternion2.Y +
                quaternion2.Z * quaternion2.Z +
                quaternion2.W * quaternion2.W;
            var lengthSquaredInv = 1.0f / lengthSquared;

            var q2x = -quaternion2.X * lengthSquaredInv;
            var q2y = -quaternion2.Y * lengthSquaredInv;
            var q2z = -quaternion2.Z * lengthSquaredInv;
            var q2w = quaternion2.W * lengthSquaredInv;

            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            Quaternion result;

            result.X = q1x * q2w + q2x * q1w + cx;
            result.Y = q1y * q2w + q2y * q1w + cy;
            result.Z = q1z * q2w + q2z * q1w + cz;
            result.W = q1w * q2w - dot;

            return result;
        }

        /// <summary>
        /// Reverses the signs of a quaternion's components.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to reverse.</param>
        /// <returns>The reversed <see cref="Quaternion"/>.</returns>
        public static Quaternion operator -(Quaternion quaternion)
        {
            Quaternion result;

            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = -quaternion.W;

            return result;
        }
                
        /// <summary>
        /// Creates a <see cref="Quaternion"/> from a normalized vector axis and an angle to rotate about the vector.
        /// </summary>
        /// <param name="axis">The unit vector around which to rotate.</param>
        /// <param name="angle">The angle, in radians, to rotate around the vector.</param>
        /// <returns>The <see cref="Quaternion"/> that was created.</returns>
        public static Quaternion CreateFromAxisAngle(Vector3 axis, Single angle)
        {
            float halfAngle = angle * 0.5f;
            float sin = (Single)Math.Sin(halfAngle);
            float cos = (Single)Math.Cos(halfAngle);

            Quaternion result;

            result.X = axis.X * sin;
            result.Y = axis.Y * sin;
            result.Z = axis.Z * sin;
            result.W = cos;

            return result;
        }

        /// <summary>
        /// Creates a <see cref="Quaternion"/> from a normalized vector axis and an angle to rotate about the vector.
        /// </summary>
        /// <param name="axis">The unit vector around which to rotate.</param>
        /// <param name="angle">The angle, in radians, to rotate around the vector.</param>
        /// <param name="result">The <see cref="Quaternion"/> that was created.</param>
        public static void CreateFromAxisAngle(ref Vector3 axis, Single angle, out Quaternion result)
        {
            var halfAngle = angle * 0.5f;
            var sin = (Single)Math.Sin(halfAngle);
            var cos = (Single)Math.Cos(halfAngle);

            result.X = axis.X * sin;
            result.Y = axis.Y * sin;
            result.Z = axis.Z * sin;
            result.W = cos;
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from the given yaw, pitch, and roll, in radians.
        /// </summary>
        /// <param name="yaw">The yaw angle, in radians, around the y-axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the x-axis.</param>
        /// <param name="roll">The roll angle, in radians, around the z-axis.</param>
        /// <returns>The <see cref="Quaternion"/> that was created.</returns>
        public static Quaternion CreateFromYawPitchRoll(Single yaw, Single pitch, Single roll)
        {
            var halfRoll = roll * 0.5f;
            var sr = (Single)Math.Sin(halfRoll);
            var cr = (Single)Math.Cos(halfRoll);

            var halfPitch = pitch * 0.5f;
            var sp = (Single)Math.Sin(halfPitch);
            var cp = (Single)Math.Cos(halfPitch);

            var halfYaw = yaw * 0.5f;
            var sy = (Single)Math.Sin(halfYaw);
            var cy = (Single)Math.Cos(halfYaw);

            Quaternion result;

            result.X = cy * sp * cr + sy * cp * sr;
            result.Y = sy * cp * cr - cy * sp * sr;
            result.Z = cy * cp * sr - sy * sp * cr;
            result.W = cy * cp * cr + sy * sp * sr;

            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from the given yaw, pitch, and roll, in radians.
        /// </summary>
        /// <param name="yaw">The yaw angle, in radians, around the y-axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the x-axis.</param>
        /// <param name="roll">The roll angle, in radians, around the z-axis.</param>
        /// <param name="result">The <see cref="Quaternion"/> that was created.</param>
        public static void CreateFromYawPitchRoll(Single yaw, Single pitch, Single roll, out Quaternion result)
        {
            var halfRoll = roll * 0.5f;
            var sr = (Single)Math.Sin(halfRoll);
            var cr = (Single)Math.Cos(halfRoll);

            var halfPitch = pitch * 0.5f;
            var sp = (Single)Math.Sin(halfPitch);
            var cp = (Single)Math.Cos(halfPitch);

            var halfYaw = yaw * 0.5f;
            var sy = (Single)Math.Sin(halfYaw);
            var cy = (Single)Math.Cos(halfYaw);

            result.X = cy * sp * cr + sy * cp * sr;
            result.Y = sy * cp * cr - cy * sp * sr;
            result.Z = cy * cp * sr - sy * sp * cr;
            result.W = cy * cp * cr + sy * sp * sr;
        }

        /// <summary>
        /// Creates a <see cref="Quaternion"/> from the given rotation matrix.
        /// </summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <returns>The <see cref="Quaternion"/> that was created.</returns>
        public static Quaternion CreateFromRotationMatrix(Matrix matrix)
        {
            var trace = matrix.M11 + matrix.M22 + matrix.M33;

            Quaternion result;

            if (trace > 0.0f)
            {
                var s = (Single)Math.Sqrt(trace + 1.0f);

                result.W = s * 0.5f;
                s = 0.5f / s;
                result.X = (matrix.M23 - matrix.M32) * s;
                result.Y = (matrix.M31 - matrix.M13) * s;
                result.Z = (matrix.M12 - matrix.M21) * s;
            }
            else
            {
                if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
                {
                    var s = (Single)Math.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
                    var invS = 0.5f / s;

                    result.X = 0.5f * s;
                    result.Y = (matrix.M12 + matrix.M21) * invS;
                    result.Z = (matrix.M13 + matrix.M31) * invS;
                    result.W = (matrix.M23 - matrix.M32) * invS;
                }
                else if (matrix.M22 > matrix.M33)
                {
                    var s = (Single)Math.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
                    var invS = 0.5f / s;

                    result.X = (matrix.M21 + matrix.M12) * invS;
                    result.Y = 0.5f * s;
                    result.Z = (matrix.M32 + matrix.M23) * invS;
                    result.W = (matrix.M31 - matrix.M13) * invS;
                }
                else
                {
                    var s = (Single)Math.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
                    var invS = 0.5f / s;

                    result.X = (matrix.M31 + matrix.M13) * invS;
                    result.Y = (matrix.M32 + matrix.M23) * invS;
                    result.Z = 0.5f * s;
                    result.W = (matrix.M12 - matrix.M21) * invS;
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a <see cref="Quaternion"/> from the given rotation matrix.
        /// </summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <param name="result">The <see cref="Quaternion"/> that was created.</param>
        public static void CreateFromRotationMatrix(ref Matrix matrix, out Quaternion result)
        {
            var trace = matrix.M11 + matrix.M22 + matrix.M33;

            if (trace > 0.0f)
            {
                var s = (Single)Math.Sqrt(trace + 1.0f);

                result.W = s * 0.5f;
                s = 0.5f / s;
                result.X = (matrix.M23 - matrix.M32) * s;
                result.Y = (matrix.M31 - matrix.M13) * s;
                result.Z = (matrix.M12 - matrix.M21) * s;
            }
            else
            {
                if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
                {
                    var s = (Single)Math.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
                    var invS = 0.5f / s;

                    result.X = 0.5f * s;
                    result.Y = (matrix.M12 + matrix.M21) * invS;
                    result.Z = (matrix.M13 + matrix.M31) * invS;
                    result.W = (matrix.M23 - matrix.M32) * invS;
                }
                else if (matrix.M22 > matrix.M33)
                {
                    var s = (Single)Math.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
                    var invS = 0.5f / s;

                    result.X = (matrix.M21 + matrix.M12) * invS;
                    result.Y = 0.5f * s;
                    result.Z = (matrix.M32 + matrix.M23) * invS;
                    result.W = (matrix.M31 - matrix.M13) * invS;
                }
                else
                {
                    var s = (Single)Math.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
                    var invS = 0.5f / s;

                    result.X = (matrix.M31 + matrix.M13) * invS;
                    result.Y = (matrix.M32 + matrix.M23) * invS;
                    result.Z = 0.5f * s;
                    result.W = (matrix.M12 - matrix.M21) * invS;
                }
            }
        }

        /// <summary>
        /// Adds two quaternions.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the addition operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion Add(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion result;

            result.X = quaternion1.X + quaternion2.X;
            result.Y = quaternion1.Y + quaternion2.Y;
            result.Z = quaternion1.Z + quaternion2.Z;
            result.W = quaternion1.W + quaternion2.W;

            return result;
        }

        /// <summary>
        /// Adds two quaternions.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the addition operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the addition operator.</param>
        /// <param name="result">The resulting <see cref="Quaternion"/>.</param>
        public static void Add(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
        {
            result.X = quaternion1.X + quaternion2.X;
            result.Y = quaternion1.Y + quaternion2.Y;
            result.Z = quaternion1.Z + quaternion2.Z;
            result.W = quaternion1.W + quaternion2.W;
        }

        /// <summary>
        /// Subtracts one quaternion from another quaternion.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the subtraction operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion Subtract(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion result;

            result.X = quaternion1.X - quaternion2.X;
            result.Y = quaternion1.Y - quaternion2.Y;
            result.Z = quaternion1.Z - quaternion2.Z;
            result.W = quaternion1.W - quaternion2.W;

            return result;
        }

        /// <summary>
        /// Subtracts one quaternion from another quaternion.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the subtraction operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the subtraction operator.</param>
        /// <param name="result">The resulting <see cref="Quaternion"/>.</param>
        public static void Subtract(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
        {
            result.X = quaternion1.X - quaternion2.X;
            result.Y = quaternion1.Y - quaternion2.Y;
            result.Z = quaternion1.Z - quaternion2.Z;
            result.W = quaternion1.W - quaternion2.W;
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the multiplication operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion Multiply(Quaternion quaternion1, Quaternion quaternion2)
        {
            var q1x = quaternion1.X;
            var q1y = quaternion1.Y;
            var q1z = quaternion1.Z;
            var q1w = quaternion1.W;

            var q2x = quaternion2.X;
            var q2y = quaternion2.Y;
            var q2z = quaternion2.Z;
            var q2w = quaternion2.W;

            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            Quaternion result;

            result.X = q1x * q2w + q2x * q1w + cx;
            result.Y = q1y * q2w + q2y * q1w + cy;
            result.Z = q1z * q2w + q2z * q1w + cz;
            result.W = q1w * q2w - dot;

            return result;
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the multiplication operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the multiplication operator.</param>
        /// <param name="result">The resulting <see cref="Quaternion"/>.</param>
        public static void Multiply(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
        {
            var q1x = quaternion1.X;
            var q1y = quaternion1.Y;
            var q1z = quaternion1.Z;
            var q1w = quaternion1.W;

            var q2x = quaternion2.X;
            var q2y = quaternion2.Y;
            var q2z = quaternion2.Z;
            var q2w = quaternion2.W;

            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            result.X = q1x * q2w + q2x * q1w + cx;
            result.Y = q1y * q2w + q2y * q1w + cy;
            result.Z = q1z * q2w + q2z * q1w + cz;
            result.W = q1w * q2w - dot;
        }

        /// <summary>
        /// Multiplies a quaternion by a scaling factor.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the quaternion.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion Multiply(Quaternion quaternion, Single factor)
        {
            Quaternion result;

            result.X = quaternion.X * factor;
            result.Y = quaternion.Y * factor;
            result.Z = quaternion.Z * factor;
            result.W = quaternion.W * factor;

            return result;
        }

        /// <summary>
        /// Multiplies a quaternion by a scaling factor.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the quaternion.</param>
        /// <param name="result">The resulting <see cref="Quaternion"/>.</param>
        public static void Multiply(ref Quaternion quaternion, Single factor, out Quaternion result)
        {
            result.X = quaternion.X * factor;
            result.Y = quaternion.Y * factor;
            result.Z = quaternion.Z * factor;
            result.W = quaternion.W * factor;
        }

        /// <summary>
        /// Divides two quaternions.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the division operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion Divide(Quaternion quaternion1, Quaternion quaternion2)
        {
            var q1x = quaternion1.X;
            var q1y = quaternion1.Y;
            var q1z = quaternion1.Z;
            var q1w = quaternion1.W;

            var lengthSquared =
                quaternion2.X * quaternion2.X +
                quaternion2.Y * quaternion2.Y +
                quaternion2.Z * quaternion2.Z +
                quaternion2.W * quaternion2.W;
            var lengthSquaredInv = 1.0f / lengthSquared;

            var q2x = -quaternion2.X * lengthSquaredInv;
            var q2y = -quaternion2.Y * lengthSquaredInv;
            var q2z = -quaternion2.Z * lengthSquaredInv;
            var q2w = quaternion2.W * lengthSquaredInv;

            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            Quaternion result;

            result.X = q1x * q2w + q2x * q1w + cx;
            result.Y = q1y * q2w + q2y * q1w + cy;
            result.Z = q1z * q2w + q2z * q1w + cz;
            result.W = q1w * q2w - dot;

            return result;
        }

        /// <summary>
        /// Divides two quaternions.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to the left of the division operator.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to the right of the division operator.</param>
        /// <param name="result">The resulting <see cref="Quaternion"/>.</param>
        public static void Divide(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
        {
            var q1x = quaternion1.X;
            var q1y = quaternion1.Y;
            var q1z = quaternion1.Z;
            var q1w = quaternion1.W;

            var lengthSquared =
                quaternion2.X * quaternion2.X +
                quaternion2.Y * quaternion2.Y +
                quaternion2.Z * quaternion2.Z +
                quaternion2.W * quaternion2.W;
            var lengthSquaredInv = 1.0f / lengthSquared;

            var q2x = -quaternion2.X * lengthSquaredInv;
            var q2y = -quaternion2.Y * lengthSquaredInv;
            var q2z = -quaternion2.Z * lengthSquaredInv;
            var q2w = quaternion2.W * lengthSquaredInv;

            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            result.X = q1x * q2w + q2x * q1w + cx;
            result.Y = q1y * q2w + q2y * q1w + cy;
            result.Z = q1z * q2w + q2z * q1w + cz;
            result.W = q1w * q2w - dot;
        }

        /// <summary>
        /// Interpolates between two quaternions using linear interpolation.
        /// </summary>
        /// <param name="source">The source quaternion.</param>
        /// <param name="target">The target quaternion.</param>
        /// <param name="amount">The relative weight of the target quaternion in the interpolation.</param>
        /// <returns>The interpolated quaternion.</returns>
        public static Quaternion Lerp(Quaternion source, Quaternion target, Single amount)
        {
            Quaternion result;

            var t = amount;
            var t1 = 1.0f - amount;

            var dot =
                source.X * target.X +
                source.Y * target.Y +
                source.Z * target.Z +
                source.W * target.W;

            if (dot > 0)
            {
                result.X = t1 * source.X + t * target.X;
                result.Y = t1 * source.Y + t * target.Y;
                result.Z = t1 * source.Z + t * target.Z;
                result.W = t1 * source.W + t * target.W;
            }
            else
            {
                result.X = t1 * source.X - t * target.X;
                result.Y = t1 * source.Y - t * target.Y;
                result.Z = t1 * source.Z - t * target.Z;
                result.W = t1 * source.W - t * target.W;
            }

            var lengthSquared = result.X * result.X + result.Y * result.Y + result.Z * result.Z + result.W * result.W;
            var lengthInv = 1.0f / (Single)Math.Sqrt(lengthSquared);

            result.X *= lengthInv;
            result.Y *= lengthInv;
            result.Z *= lengthInv;
            result.W *= lengthInv;

            return result;
        }

        /// <summary>
        /// Interpolates between two quaternions using linear interpolation.
        /// </summary>
        /// <param name="source">The source quaternion.</param>
        /// <param name="target">The target quaternion.</param>
        /// <param name="amount">The relative weight of the target quaternion in the interpolation.</param>
        /// <param name="result">The interpolated quaternion.</param>
        public static void Lerp(ref Quaternion source, ref Quaternion target, Single amount, out Quaternion result)
        {
            var t = amount;
            var t1 = 1.0f - amount;

            var dot =
                source.X * target.X +
                source.Y * target.Y +
                source.Z * target.Z +
                source.W * target.W;

            if (dot > 0)
            {
                result.X = t1 * source.X + t * target.X;
                result.Y = t1 * source.Y + t * target.Y;
                result.Z = t1 * source.Z + t * target.Z;
                result.W = t1 * source.W + t * target.W;
            }
            else
            {
                result.X = t1 * source.X - t * target.X;
                result.Y = t1 * source.Y - t * target.Y;
                result.Z = t1 * source.Z - t * target.Z;
                result.W = t1 * source.W - t * target.W;
            }

            var lengthSquared = result.X * result.X + result.Y * result.Y + result.Z * result.Z + result.W * result.W;
            var lengthInv = 1.0f / (Single)Math.Sqrt(lengthSquared);

            result.X *= lengthInv;
            result.Y *= lengthInv;
            result.Z *= lengthInv;
            result.W *= lengthInv;
        }

        /// <summary>
        /// Interpolates between two quaternions using spherical linear interpolation.
        /// </summary>
        /// <param name="source">The source quaternion.</param>
        /// <param name="target">The target quaternion.</param>
        /// <param name="amount">The relative weight of the target quaternion in the interpolation.</param>
        /// <returns>The interpolated quaternion.</returns>
        public static Quaternion Slerp(Quaternion source, Quaternion target, Single amount)
        {
            var flip = false;

            var cosOmega =
                source.X * target.X +
                source.Y * target.Y +
                source.Z * target.Z +
                source.W * target.W;

            if (cosOmega < 0)
            {
                flip = true;
                cosOmega = -cosOmega;
            }

            var s1 = default(Single);
            var s2 = default(Single);

            if (MathUtil.IsApproximatelyGreaterThan(cosOmega, 1.0f))
            {
                s1 = 1.0f - amount;
                s2 = flip ? -amount : amount;
            }
            else
            {
                var omega = Math.Acos(cosOmega);
                var invSinOmega = 1f / (Single)Math.Sin(omega);

                s1 = (Single)Math.Sin((1.0f - amount) * omega) * invSinOmega;
                s2 = flip ? -(Single)Math.Sin(amount * omega) * invSinOmega : (Single)Math.Sin(amount * omega) * invSinOmega;
            }

            Quaternion result;

            result.X = s1 * source.X + s2 * target.X;
            result.Y = s1 * source.Y + s2 * target.Y;
            result.Z = s1 * source.Z + s2 * target.Z;
            result.W = s1 * source.W + s2 * target.W;

            return result;
        }

        /// <summary>
        /// Interpolates between two quaternions using spherical linear interpolation.
        /// </summary>
        /// <param name="source">The source quaternion.</param>
        /// <param name="target">The target quaternion.</param>
        /// <param name="amount">The relative weight of the target quaternion in the interpolation.</param>
        /// <param name="result">The interpolated quaternion.</param>
        public static void Slerp(ref Quaternion source, ref Quaternion target, Single amount, out Quaternion result)
        {
            var flip = false;

            var cosOmega =
                source.X * target.X +
                source.Y * target.Y +
                source.Z * target.Z +
                source.W * target.W;

            if (cosOmega < 0)
            {
                flip = true;
                cosOmega = -cosOmega;
            }

            var s1 = default(Single);
            var s2 = default(Single);

            if (MathUtil.IsApproximatelyGreaterThan(cosOmega, 1.0f))
            {
                s1 = 1.0f - amount;
                s2 = flip ? -amount : amount;
            }
            else
            {
                var omega = Math.Acos(cosOmega);
                var invSinOmega = 1f / (Single)Math.Sin(omega);

                s1 = (Single)Math.Sin((1.0f - amount) * omega) * invSinOmega;
                s2 = flip ? -(Single)Math.Sin(amount * omega) * invSinOmega : (Single)Math.Sin(amount * omega) * invSinOmega;
            }

            result.X = s1 * source.X + s2 * target.X;
            result.Y = s1 * source.Y + s2 * target.Y;
            result.Z = s1 * source.Z + s2 * target.Z;
            result.W = s1 * source.W + s2 * target.W;
        }

        /// <summary>
        /// Normalizes a quaternion.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to normalize.</param>
        /// <returns>The normalized <see cref="Quaternion"/>.</returns>
        public static Quaternion Normalize(Quaternion quaternion)
        {
            var magnitude =
                quaternion.X * quaternion.X +
                quaternion.Y * quaternion.Y +
                quaternion.Z * quaternion.Z +
                quaternion.W * quaternion.W;
            var inverseMagnitude = 1f / (Single)Math.Sqrt(magnitude);

            Quaternion result;

            result.X = quaternion.X * inverseMagnitude;
            result.Y = quaternion.Y * inverseMagnitude;
            result.Z = quaternion.Z * inverseMagnitude;
            result.W = quaternion.W * inverseMagnitude;

            return result;
        }

        /// <summary>
        /// Normalizes a quaternion.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to normalize.</param>
        /// <param name="result">The normalized <see cref="Quaternion"/>.</param>
        public static void Normalize(ref Quaternion quaternion, out Quaternion result)
        {
            var magnitude =
                quaternion.X * quaternion.X +
                quaternion.Y * quaternion.Y +
                quaternion.Z * quaternion.Z +
                quaternion.W * quaternion.W;
            var inverseMagnitude = 1f / (Single)Math.Sqrt(magnitude);

            result.X = quaternion.X * inverseMagnitude;
            result.Y = quaternion.Y * inverseMagnitude;
            result.Z = quaternion.Z * inverseMagnitude;
            result.W = quaternion.W * inverseMagnitude;
        }

        /// <summary>
        /// Negates the specified quaternion.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to negate.</param>
        /// <returns>The negated <see cref="Quaternion"/>.</returns>
        public static Quaternion Negate(Quaternion quaternion)
        {
            Quaternion result;

            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = -quaternion.W;

            return result;
        }

        /// <summary>
        /// Negates the specified quaternion.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to negate.</param>
        /// <param name="result">The negated <see cref="Quaternion"/>.</param>
        public static void Negate(ref Quaternion quaternion, out Quaternion result)
        {
            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = -quaternion.W;
        }

        /// <summary>
        /// Inverts the specified quaternion.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to invert.</param>
        /// <returns>The inverted <see cref="Quaternion"/>.</returns>
        public static Quaternion Inverse(Quaternion quaternion)
        {
            var lengthSquared =
                quaternion.X * quaternion.X +
                quaternion.Y * quaternion.Y +
                quaternion.Z * quaternion.Z +
                quaternion.W * quaternion.W;
            var lengthSquaredInv = 1.0f / lengthSquared;

            Quaternion result;

            result.X = -quaternion.X * lengthSquaredInv;
            result.Y = -quaternion.Y * lengthSquaredInv;
            result.Z = -quaternion.Z * lengthSquaredInv;
            result.W = quaternion.W * lengthSquaredInv;

            return result;
        }

        /// <summary>
        /// Inverts the specified quaternion.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> to invert.</param>
        /// <param name="result">The inverted <see cref="Quaternion"/>.</param>
        public static void Inverse(ref Quaternion quaternion, out Quaternion result)
        {
            var lengthSquared =
                quaternion.X * quaternion.X +
                quaternion.Y * quaternion.Y +
                quaternion.Z * quaternion.Z +
                quaternion.W * quaternion.W;
            var lengthSquaredInv = 1.0f / lengthSquared;

            result.X = -quaternion.X * lengthSquaredInv;
            result.Y = -quaternion.Y * lengthSquaredInv;
            result.Z = -quaternion.Z * lengthSquaredInv;
            result.W = quaternion.W * lengthSquaredInv;
        }

        /// <summary>
        /// Calculates the dot product of the specified quaternions.
        /// </summary>
        /// <param name="quaternion1">The first <see cref="Quaternion"/> for which to calculate the dot product.</param>
        /// <param name="quaternion2">The second <see cref="Quaternion"/> for which to calculate the dot product</param>
        /// <returns>The dot product of the specified quaternions.</returns>
        public static Single Dot(Quaternion quaternion1, Quaternion quaternion2)
        {
            return quaternion1.X * quaternion2.X +
                   quaternion1.Y * quaternion2.Y +
                   quaternion1.Z * quaternion2.Z +
                   quaternion1.W * quaternion2.W;
        }

        /// <summary>
        /// Calculates the dot product of the specified quaternions.
        /// </summary>
        /// <param name="quaternion1">The first <see cref="Quaternion"/> for which to calculate the dot product.</param>
        /// <param name="quaternion2">The second <see cref="Quaternion"/> for which to calculate the dot product</param>
        /// <param name="result">The dot product of the specified quaternions.</param>
        public static void Dot(ref Quaternion quaternion1, ref Quaternion quaternion2, out Single result)
        {
            result =
                quaternion1.X * quaternion2.X +
                quaternion1.Y * quaternion2.Y +
                quaternion1.Z * quaternion2.Z +
                quaternion1.W * quaternion2.W;
        }

        /// <summary>
        /// Concatenates two <see cref="Quaternion"/>s; the result represents the <paramref name="quaternion1"/>
        /// rotation followed by the <paramref name="quaternion2"/> rotation.
        /// </summary>
        /// <param name="quaternion1">The first <see cref="Quaternion"/> rotation.</param>
        /// <param name="quaternion2">The second <see cref="Quaternion"/> rotation.</param>
        /// <returns>A <see cref="Quaternion"/> which represents the concatenation of the specified quaternions.</returns>
        public static Quaternion Concatenate(Quaternion quaternion1, Quaternion quaternion2)
        {            
            var q1x = quaternion2.X;
            var q1y = quaternion2.Y;
            var q1z = quaternion2.Z;
            var q1w = quaternion2.W;

            var q2x = quaternion1.X;
            var q2y = quaternion1.Y;
            var q2z = quaternion1.Z;
            var q2w = quaternion1.W;

            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;

            Quaternion result;

            result.X = q1x * q2w + q2x * q1w + cx;
            result.Y = q1y * q2w + q2y * q1w + cy;
            result.Z = q1z * q2w + q2z * q1w + cz;
            result.W = q1w * q2w - dot;

            return result;
        }

        /// <summary>
        /// Concatenates two <see cref="Quaternion"/>s; the result represents the <paramref name="quaternion1"/>
        /// rotation followed by the <paramref name="quaternion2"/> rotation.
        /// </summary>
        /// <param name="quaternion1">The first <see cref="Quaternion"/> rotation.</param>
        /// <param name="quaternion2">The second <see cref="Quaternion"/> rotation.</param>
        /// <param name="result">A <see cref="Quaternion"/> which represents the concatenation of the specified quaternions.</param>
        public static void Concatenate(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
        {
            var q1x = quaternion2.X;
            var q1y = quaternion2.Y;
            var q1z = quaternion2.Z;
            var q1w = quaternion2.W;

            var q2x = quaternion1.X;
            var q2y = quaternion1.Y;
            var q2z = quaternion1.Z;
            var q2w = quaternion1.W;

            var cx = q1y * q2z - q1z * q2y;
            var cy = q1z * q2x - q1x * q2z;
            var cz = q1x * q2y - q1y * q2x;

            var dot = q1x * q2x + q1y * q2y + q1z * q2z;
            
            result.X = q1x * q2w + q2x * q1w + cx;
            result.Y = q1y * q2w + q2y * q1w + cy;
            result.Z = q1z * q2w + q2z * q1w + cz;
            result.W = q1w * q2w - dot;
        }

        /// <summary>
        /// Calculates the conjugate of the specified <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> for which to calculate the conjugate.</param>
        /// <returns>The conjugate of the specifed <see cref="Quaternion"/>.</returns>
        public static Quaternion Conjugate(Quaternion quaternion)
        {
            Quaternion result;

            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = quaternion.W;

            return result;
        }

        /// <summary>
        /// Calculates the conjugate of the specified <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="quaternion">The <see cref="Quaternion"/> for which to calculate the conjugate.</param>
        /// <param name="result">The conjugate of the specifed <see cref="Quaternion"/>.</param>
        public static void Conjugate(ref Quaternion quaternion, out Quaternion result)
        {
            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = quaternion.W;            
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{X:{X} Y:{Y} Z:{Z} W:{W}}}";

        /// <summary>
        /// Calculates the length of the quaternion.
        /// </summary>
        /// <returns>The length of the quaternion.</returns>
        public Single Length()
        {
            return (Single)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        /// <summary>
        /// Calculates the squared length of the quaternion.
        /// </summary>
        /// <returns>The squared length of the quaternion.</returns>
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
        public Quaternion Interpolate(Quaternion target, Single t)
        {
            Slerp(ref this, ref target, t, out Quaternion result);
            return result;
        }

        /// <summary>
        /// Gets a <see cref="Quaternion"/> which represents no rotation.
        /// </summary>
        public static Quaternion Identity { get; } = new Quaternion(0, 0, 0, 1);

        /// <summary>
        /// The x-coordinate of the quaternion's vector component.
        /// </summary>
        public Single X;

        /// <summary>
        /// The y-coordinate of the quaternion's vector component.
        /// </summary>
        public Single Y;

        /// <summary>
        /// The z-coordinate of the quaternion's vector component.
        /// </summary>
        public Single Z;

        /// <summary>
        /// The quaternion's scalar rotation component.
        /// </summary>
        public Single W;
    }
}
