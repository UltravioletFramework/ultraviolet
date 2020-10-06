using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Graphics.Uniforms;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Contains the data for an effect parameter.
    /// </summary>
    public unsafe sealed class OpenGLEffectParameterData
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLEffectParameterData class.
        /// </summary>
        public OpenGLEffectParameterData(UInt32 sizeInBytes)
        {
            var minSizeInBytes = (UInt32)(16 * sizeof(Single));
            if (minSizeInBytes > sizeInBytes)
                sizeInBytes = minSizeInBytes;

            valData = new Byte[sizeInBytes];
        }

        /// <summary>
        /// Clears the data buffer.
        /// </summary>
        public void Clear()
        {
            DataType = OpenGLEffectParameterDataType.None;
            refData = null;
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Boolean value)
        {
            fixed (Byte* pValData = valData)
            {
                if (DataType != OpenGLEffectParameterDataType.Boolean || *(Boolean*)pValData != value)
                {
                    DataType = OpenGLEffectParameterDataType.Boolean;
                    ElementCount = 1;
                    Version++;

                    *(Boolean*)pValData = value;
                }
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Boolean[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.BooleanArray;
            SetArray(value);
            Version++;
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Int32 value)
        {
            fixed (Byte* pValData = valData)
            {
                if (DataType != OpenGLEffectParameterDataType.Int32 || *(Int32*)pValData != value)
                {
                    DataType = OpenGLEffectParameterDataType.Int32;
                    ElementCount = 1;
                    Version++;

                    *(Int32*)pValData = value;
                }
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Int32[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Int32Array;
            SetArray(value);
            Version++;
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(UInt32 value)
        {
            fixed (Byte* pValData = valData)
            {
                if (DataType != OpenGLEffectParameterDataType.UInt32 || *(UInt32*)pValData != value)
                {
                    DataType = OpenGLEffectParameterDataType.UInt32;
                    ElementCount = 1;
                    Version++;

                    *(UInt32*)pValData = value;
                }
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(UInt32[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.UInt32Array;
            SetArray(value);
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Single value)
        {
            fixed (Byte* pValData = valData)
            {
                if (DataType != OpenGLEffectParameterDataType.Single || *(Single*)pValData != value)
                {
                    DataType = OpenGLEffectParameterDataType.Single;
                    ElementCount = 1;
                    Version++;

                    *(Single*)pValData = value;
                }
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Single[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.SingleArray;
            SetArray(value);
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Double value)
        {
            fixed (Byte* pValData = valData)
            {
                if (DataType != OpenGLEffectParameterDataType.Double || *(Double*)pValData != value)
                {
                    DataType = OpenGLEffectParameterDataType.Double;
                    ElementCount = 1;
                    Version++;

                    *(Double*)pValData = value;
                }
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Double[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.DoubleArray;
            SetArray(value);
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector2 value)
        {
            fixed (Byte* pValData = valData)
            {
                if (DataType != OpenGLEffectParameterDataType.Vector2 || *(Vector2*)pValData != value)
                {
                    DataType = OpenGLEffectParameterDataType.Vector2;
                    ElementCount = 1;
                    Version++;

                    *(Vector2*)pValData = value;
                }
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector2[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Vector2Array;
            SetArray(value);
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector3 value)
        {
            fixed (Byte* pValData = valData)
            {
                if (DataType != OpenGLEffectParameterDataType.Vector3 || *(Vector3*)pValData != value)
                {
                    DataType = OpenGLEffectParameterDataType.Vector3;
                    ElementCount = 1;
                    Version++;

                    *(Vector3*)pValData = value;
                }
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector3[] value)
        {
            DataType = OpenGLEffectParameterDataType.Vector3Array;
            SetArray(value);
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector4 value)
        {
            fixed (Byte* pValData = valData)
            {
                if (DataType != OpenGLEffectParameterDataType.Vector4 || *(Vector4*)pValData != value)
                {
                    DataType = OpenGLEffectParameterDataType.Vector4;
                    ElementCount = 1;
                    Version++;

                    *(Vector4*)pValData = value;
                }
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector4[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Vector4Array;
            SetArray(value);
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Color value)
        {
            fixed (Byte* pValData = valData)
            {
                if (DataType != OpenGLEffectParameterDataType.Color || *(Color*)pValData != value)
                {
                    DataType = OpenGLEffectParameterDataType.Color;
                    ElementCount = 1;
                    Version++;

                    *(Color*)pValData = value;
                }
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Color[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.ColorArray;
            SetArray(value);
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat2(in Matrix value)
        {
            fixed (Byte* pValData = valData)
            {
                DataType = OpenGLEffectParameterDataType.Mat2;
                ElementCount = 1;
                Version++;

                *((Mat2*)pValData) = new Mat2(value);
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat2(Matrix[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Mat2Array;
            SetArray(value, x => new Mat2(x));
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat2x3(in Matrix value)
        {
            fixed (Byte* pValData = valData)
            {
                DataType = OpenGLEffectParameterDataType.Mat2x3;
                ElementCount = 1;
                Version++;

                *((Mat2x3*)pValData) = new Mat2x3(value);
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat2x3(Matrix[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Mat2x3Array;
            SetArray(value, x => new Mat2x3(x));
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat2x4(in Matrix value)
        {
            fixed (Byte* pValData = valData)
            {
                DataType = OpenGLEffectParameterDataType.Mat2x4;
                ElementCount = 1;
                Version++;

                *((Mat2x4*)pValData) = new Mat2x4(value);
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat2x4(Matrix[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Mat2x4Array;
            SetArray(value, x => new Mat2x4(x));
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat3(in Matrix value)
        {
            fixed (Byte* pValData = valData)
            {
                DataType = OpenGLEffectParameterDataType.Mat3;
                ElementCount = 1;
                Version++;

                *((Mat3*)pValData) = new Mat3(value);
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat3(Matrix[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Mat3Array;
            SetArray(value, x => new Mat3(x));
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat3x2(in Matrix value)
        {
            fixed (Byte* pValData = valData)
            {
                DataType = OpenGLEffectParameterDataType.Mat3x2;
                ElementCount = 1;
                Version++;

                *((Mat3x2*)pValData) = new Mat3x2(value);
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat3x2(Matrix[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Mat3x2Array;
            SetArray(value, x => new Mat3x2(x));
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat3x4(in Matrix value)
        {
            fixed (Byte* pValData = valData)
            {
                DataType = OpenGLEffectParameterDataType.Mat3x4;
                ElementCount = 1;
                Version++;

                *((Mat3x4*)pValData) = new Mat3x4(value);
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat3x4(Matrix[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Mat3x4Array;
            SetArray(value, x => new Mat3x4(x));
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat4(in Matrix value)
        {
            fixed (Byte* pValData = valData)
            {
                DataType = OpenGLEffectParameterDataType.Mat4;
                ElementCount = 1;
                Version++;

                *((Matrix*)pValData) = value;
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat4(Matrix[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Mat4Array;
            SetArray(value);
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat4x2(in Matrix value)
        {
            fixed (Byte* pValData = valData)
            {
                DataType = OpenGLEffectParameterDataType.Mat4x2;
                ElementCount = 1;
                Version++;

                *((Mat4x2*)pValData) = new Mat4x2(value);
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat4x2(Matrix[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Mat4x2Array;
            SetArray(value, x => new Mat4x2(x));
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat4x3(in Matrix value)
        {
            fixed (Byte* pValData = valData)
            {
                DataType = OpenGLEffectParameterDataType.Mat4x3;
                ElementCount = 1;
                Version++;

                *((Mat4x3*)pValData) = new Mat4x3(value);
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetMat4x3(Matrix[] value)
        {
            Contract.Require(value, nameof(value));

            DataType = OpenGLEffectParameterDataType.Mat4x3Array;
            SetArray(value, x => new Mat4x3(x));
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Texture2D value)
        {
            if (DataType != OpenGLEffectParameterDataType.Texture2D || refData != value)
            {
                DataType = OpenGLEffectParameterDataType.Texture2D;
                ElementCount = 1;
                Version++;

                refData = value;
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Texture3D value)
        {
            if (DataType != OpenGLEffectParameterDataType.Texture3D || refData != value)
            {
                DataType = OpenGLEffectParameterDataType.Texture3D;
                ElementCount = 1;
                Version++;

                refData = value;
            }
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Boolean GetBoolean()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Boolean);

            if (DataType == OpenGLEffectParameterDataType.Boolean)
            {
                fixed (Byte* pValData = valData)
                {
                    return *((Boolean*)pValData);
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetBooleanArray(Boolean[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.BooleanArray)
            {
                GetArray(array, count);
                return;
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Int32 GetInt32()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Int32);

            if (DataType == OpenGLEffectParameterDataType.Int32)
            {
                fixed (Byte* pValData = valData)
                {
                    return *((Int32*)pValData);
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetInt32Array(Int32[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Int32Array)
            {
                GetArray(array, count);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public UInt32 GetUInt32()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(UInt32);

            if (DataType == OpenGLEffectParameterDataType.UInt32)
            {
                fixed (Byte* pValData = valData)
                {
                    return *((UInt32*)pValData);
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetUInt32Array(UInt32[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.UInt32Array)
            {
                GetArray(array, count);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Single GetSingle()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Single);

            if (DataType == OpenGLEffectParameterDataType.Single)
            {
                fixed (Byte* pValData = valData)
                {
                    return *((Single*)pValData);
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetSingleArray(Single[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.SingleArray)
            {
                GetArray(array, count);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Double GetDouble()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Double);

            if (DataType == OpenGLEffectParameterDataType.Double)
            {
                fixed (Byte* pValData = valData)
                {
                    return *((Double*)pValData);
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetDoubleArray(Double[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.DoubleArray)
            {
                GetArray(array, count);
                return;
            }            
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector2 GetVector2()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Vector2);

            if (DataType == OpenGLEffectParameterDataType.Vector2)
            {
                fixed (Byte* pValData = valData)
                {
                    return *((Vector2*)pValData);
                }
            }
            
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetVector2Array(Vector2[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Vector2Array)
            {
                GetArray(array, count);
                return;
            }            
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector3 GetVector3()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Vector3);

            if (DataType == OpenGLEffectParameterDataType.Vector3)
            {
                fixed (Byte* pValData = valData)
                {
                    return *((Vector3*)pValData);
                }
            }
            
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetVector3Array(Vector3[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Vector3Array)
            {
                GetArray(array, count);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector4 GetVector4()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Vector4);

            if (DataType == OpenGLEffectParameterDataType.Vector4)
            {
                fixed (Byte* pValData = valData)
                {
                    return *((Vector4*)pValData);
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetVector4Array(Vector4[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Vector4Array)
            {
                GetArray(array, count);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Color GetColor()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Color);

            if (DataType == OpenGLEffectParameterDataType.Color)
            {
                fixed (Byte* pValData = valData)
                {
                    return *((Color*)pValData);
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetColorArray(Color[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.ColorArray)
            {
                GetArray(array, count);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Mat2 GetMat2()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Mat2);

            if (DataType == OpenGLEffectParameterDataType.Mat2)
            {
                fixed (Byte* pValData = valData)
                {
                    return *(Mat2*)pValData;
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetMat2Array(Matrix[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Mat2Array)
            {
                GetArray<Mat2, Matrix>(array, count, x => (Matrix)x);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Mat2x3 GetMat2x3()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Mat2x3);

            if (DataType == OpenGLEffectParameterDataType.Mat2x3)
            {
                fixed (Byte* pValData = valData)
                {
                    return *(Mat2x3*)pValData;
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetMat2x3Array(Matrix[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Mat2x3Array)
            {
                GetArray<Mat2x3, Matrix>(array, count, x => (Matrix)x);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Mat2x4 GetMat2x4()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Mat2x4);

            if (DataType == OpenGLEffectParameterDataType.Mat2x4)
            {
                fixed (Byte* pValData = valData)
                {
                    return *(Mat2x4*)pValData;
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetMat2x4Array(Matrix[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Mat2x4Array)
            {
                GetArray<Mat2x4, Matrix>(array, count, x => (Matrix)x);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Mat3 GetMat3()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Mat3);

            if (DataType == OpenGLEffectParameterDataType.Mat3)
            {
                fixed (Byte* pValData = valData)
                {
                    return *(Mat3*)pValData;
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetMat3Array(Matrix[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Mat3Array)
            {
                GetArray<Mat3, Matrix>(array, count, x => (Matrix)x);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Mat3x2 GetMat3x2()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Mat3x2);

            if (DataType == OpenGLEffectParameterDataType.Mat3x2)
            {
                fixed (Byte* pValData = valData)
                {
                    return *(Mat3x2*)pValData;
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetMat3x2Array(Matrix[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Mat3x2Array)
            {
                GetArray<Mat3x2, Matrix>(array, count, x => (Matrix)x);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Mat3x4 GetMat3x4()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Mat3x4);

            if (DataType == OpenGLEffectParameterDataType.Mat3x4)
            {
                fixed (Byte* pValData = valData)
                {
                    return *(Mat3x4*)pValData;
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetMat3x4Array(Matrix[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Mat3x4Array)
            {
                GetArray<Mat3x4, Matrix>(array, count, x => (Matrix)x);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Matrix GetMat4()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Matrix);

            if (DataType == OpenGLEffectParameterDataType.Mat4)
            {
                fixed (Byte* pValData = valData)
                {
                    return *((Matrix*)pValData);
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetMat4Array(Matrix[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Mat4Array)
            {
                GetArray(array, count);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Mat4x2 GetMat4x2()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Mat4x2);

            if (DataType == OpenGLEffectParameterDataType.Mat4x2)
            {
                fixed (Byte* pValData = valData)
                {
                    return *(Mat4x2*)pValData;
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetMat4x2Array(Matrix[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Mat4x2Array)
            {
                GetArray<Mat4x2, Matrix>(array, count, x => (Matrix)x);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Mat4x3 GetMat4x3()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Mat4x3);

            if (DataType == OpenGLEffectParameterDataType.Mat4x3)
            {
                fixed (Byte* pValData = valData)
                {
                    return *(Mat4x3*)pValData;
                }
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <param name="array">An array to populate with values.</param>
        /// <param name="count">The maximum number of values to copy into the array.</param>
        public void GetMat4x3Array(Matrix[] array, Int32 count)
        {
            Contract.Require(array, nameof(array));
            Contract.EnsureRange(count >= 0 && count < array.Length, nameof(count));

            if (DataType == OpenGLEffectParameterDataType.None)
            {
                GetDefaultArray(array, count);
                return;
            }

            if (DataType == OpenGLEffectParameterDataType.Mat4x3Array)
            {
                GetArray<Mat4x3, Matrix>(array, count, x => (Matrix)x);
                return;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Texture2D GetTexture2D()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return null;

            if (DataType == OpenGLEffectParameterDataType.Texture2D)
                return (Texture2D)refData;

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Texture3D GetTexture3D()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return null;

            if (DataType == OpenGLEffectParameterDataType.Texture3D)
                return (Texture3D)refData;

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the type of data currently stored in this buffer.
        /// </summary>
        public OpenGLEffectParameterDataType DataType { get; private set; }

        /// <summary>
        /// Gets the version number of this data.
        /// </summary>
        public Int64 Version { get; private set; } = 1;

        /// <summary>
        /// Gets the number of elements in the parameter data. This property will have the value 1 for all non-array types.
        /// </summary>
        public Int32 ElementCount { get; private set; } = 1;

        /// <summary>
        /// Gets the size in bytes of this parameter's data buffer.
        /// </summary>
        public Int32 SizeInBytes => valData.Length;

        /// <summary>
        /// Gets the raw data buffer which is used to contain parameter values.
        /// </summary>
        public Byte[] RawDataBuffer => valData;

        /// <summary>
        /// Copies the value of the specified array into the parameter data.
        /// </summary>
        private void SetArray<TElement>(TElement[] array)
            where TElement : unmanaged
        {
            SetArray(array, x => x);
        }

        /// <summary>
        /// Copies the value of the specified array into the parameter data.
        /// </summary>
        private void SetArray<TSrc, TDst>(TSrc[] array, Func<TSrc, TDst> converter)
            where TSrc : unmanaged
            where TDst : unmanaged
        {
            var countMax = valData.Length / sizeof(TDst);
            var count = array.Length > countMax ? countMax : array.Length;

            fixed (void* pArrData = array)
            fixed (void* pValData = valData)
            {
                var pArrDataInt = (TSrc*)pArrData;
                var pValDataInt = (TDst*)pValData;

                for (var i = 0; i < count; i++)
                {
                    var value = *pArrDataInt++;
                    *pValDataInt++ = converter(value);
                }
            }

            this.ElementCount = count;
        }

        /// <summary>
        /// Copies the value of the parameter data into the specified array. 
        /// </summary>
        private void GetArray<TElement>(TElement[] array, Int32 count)
            where TElement : unmanaged
        {
            GetArray<TElement, TElement>(array, count, x => x);
        }

        /// <summary>
        /// Copies the value of the parameter data into the specified array. 
        /// </summary>
        private void GetArray<TSrc, TDst>(TDst[] array, Int32 count, Func<TSrc, TDst> converter)
            where TSrc : unmanaged
            where TDst : unmanaged
        {
            var countMax = valData.Length / sizeof(TSrc);
            if (countMax < count)
                throw new ArgumentOutOfRangeException(nameof(count));

            fixed (void* pArrData = array)
            fixed (void* pValData = valData)
            {
                var pArrDataInt = (TDst*)pArrData;
                var pValDataInt = (TSrc*)pValData;

                for (var i = 0; i < count; i++)
                {
                    var value = *pValDataInt++;
                    *pArrDataInt++ = converter(value);
                }
            }
        }

        /// <summary>
        /// Copies default values into the specified array.
        /// </summary>
        private void GetDefaultArray<TElement>(TElement[] array, Int32 count)
        {
            for (int i = 0; i < count; i++)
                array[i] = default(TElement);
        }

        // State values.
        private readonly Byte[] valData;
        private Object refData;
    }
}
