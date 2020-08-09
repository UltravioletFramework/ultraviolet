using System;
using Ultraviolet.Graphics;

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
        public OpenGLEffectParameterData()
        {
            valData = new Byte[16 * sizeof(Single)];
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
                    Version++;

                    *(Boolean*)pValData = value;
                }
            }
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
            DataType = OpenGLEffectParameterDataType.Int32Array;
            refData = value;
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
            DataType = OpenGLEffectParameterDataType.UInt32Array;
            refData = value;
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
            DataType = OpenGLEffectParameterDataType.SingleArray;
            refData = value;
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
            DataType = OpenGLEffectParameterDataType.DoubleArray;
            refData = value;
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
            DataType = OpenGLEffectParameterDataType.Vector2Array;
            refData = value;
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
            refData = value;
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
            DataType = OpenGLEffectParameterDataType.Vector4Array;
            refData = value;
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
            DataType = OpenGLEffectParameterDataType.ColorArray;
            refData = value;
            Version++;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Matrix value)
        {
            fixed (Byte* pValData = valData)
            {
                if (DataType != OpenGLEffectParameterDataType.Matrix || *(Matrix*)pValData != value)
                {
                    DataType = OpenGLEffectParameterDataType.Matrix;
                    Version++;

                    *((Matrix*)pValData) = value;
                }
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetRef(ref Matrix value)
        {
            fixed (Byte* pValData = valData)
            {
                if (DataType != OpenGLEffectParameterDataType.Matrix || *(Matrix*)pValData != value)
                {
                    DataType = OpenGLEffectParameterDataType.Matrix;
                    Version++;

                    *((Matrix*)pValData) = value;
                }
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Matrix[] value)
        {
            DataType = OpenGLEffectParameterDataType.MatrixArray;
            refData = value;
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
        /// <returns>The value that is set into the buffer.</returns>
        public Int32[] GetInt32Array()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return null;

            if (DataType == OpenGLEffectParameterDataType.Int32Array)
                return (Int32[])refData;
            
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
        /// <returns>The value that is set into the buffer.</returns>
        public UInt32[] GetUInt32Array()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return null;

            if (DataType == OpenGLEffectParameterDataType.UInt32Array)
                return (UInt32[])refData;

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
        /// <returns>The value that is set into the buffer.</returns>
        public Single[] GetSingleArray()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return null;

            if (DataType == OpenGLEffectParameterDataType.SingleArray)
                return (Single[])refData;

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
        /// <returns>The value that is set into the buffer.</returns>
        public Double[] GetDoubleArray()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return null;

            if (DataType == OpenGLEffectParameterDataType.DoubleArray)
                return (Double[])refData;
            
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
        /// <returns>The value that is set into the buffer.</returns>
        public Vector2[] GetVector2Array()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return null;

            if (DataType == OpenGLEffectParameterDataType.Vector2Array)
                return (Vector2[])refData;
            
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
        /// <returns>The value that is set into the buffer.</returns>
        public Vector3[] GetVector3Array()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return null;

            if (DataType == OpenGLEffectParameterDataType.Vector3Array)
                return (Vector3[])refData;

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
        /// <returns>The value that is set into the buffer.</returns>
        public Vector4[] GetVector4Array()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return null;

            if (DataType == OpenGLEffectParameterDataType.Vector4Array)
                return (Vector4[])refData;

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
        /// <returns>The value that is set into the buffer.</returns>
        public Color[] GetColorArray()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return null;

            if (DataType == OpenGLEffectParameterDataType.ColorArray)
                return (Color[])refData;

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Matrix GetMatrix()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return default(Matrix);

            if (DataType == OpenGLEffectParameterDataType.Matrix)
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
        /// <returns>The value that is set into the buffer.</returns>
        public Matrix[] GetMatrixArray()
        {
            if (DataType == OpenGLEffectParameterDataType.None)
                return null;

            if (DataType == OpenGLEffectParameterDataType.MatrixArray)
                return (Matrix[])refData;

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

        // State values.
        private Byte[] valData;
        private Object refData;
    }
}
