using System;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
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
        /// Gets a value indicating whether this instance contains the same data as the specified instance.
        /// </summary>
        /// <param name="other">The instance to compare to this instance.</param>
        /// <returns><see langword="true"/> if both instances contain the same data; otherwise, <see langword="false"/>.</returns>
        public Boolean ContainsSameData(OpenGLEffectParameterData other)
        {
            Contract.Require(other, nameof(other));

            if (other.DataType != this.DataType)
                return false;

            switch (DataType)
            {
                case OpenGLEffectParameterDataType.None:
                    return true;

                case OpenGLEffectParameterDataType.Boolean:
                    return this.valData[0] == other.valData[0];

                case OpenGLEffectParameterDataType.Int32:
                case OpenGLEffectParameterDataType.UInt32:
                case OpenGLEffectParameterDataType.Single:
                case OpenGLEffectParameterDataType.Color:
                    return
                        this.valData[0] == other.valData[0] &&
                        this.valData[1] == other.valData[1] &&
                        this.valData[2] == other.valData[2] &&
                        this.valData[3] == other.valData[3];

                case OpenGLEffectParameterDataType.Double:
                case OpenGLEffectParameterDataType.Vector2:
                    for (int i = 0; i < sizeof(Vector2); i++)
                    {
                        if (this.valData[i] != other.valData[i])
                            return false;
                    }
                    return true;

                case OpenGLEffectParameterDataType.Vector3:
                    for (int i = 0; i < sizeof(Vector3); i++)
                    {
                        if (this.valData[i] != other.valData[i])
                            return false;
                    }
                    return true;

                case OpenGLEffectParameterDataType.Vector4:
                    for (int i = 0; i < sizeof(Vector4); i++)
                    {
                        if (this.valData[i] != other.valData[i])
                            return false;
                    }
                    return true;

                case OpenGLEffectParameterDataType.Matrix:
                    for (int i = 0; i < sizeof(Matrix); i++)
                    {
                        if (this.valData[i] != other.valData[i])
                            return false;
                    }
                    return true;

                case OpenGLEffectParameterDataType.Int32Array:
                case OpenGLEffectParameterDataType.UInt32Array:
                case OpenGLEffectParameterDataType.SingleArray:
                case OpenGLEffectParameterDataType.DoubleArray:
                case OpenGLEffectParameterDataType.Vector2Array:
                case OpenGLEffectParameterDataType.Vector3Array:
                case OpenGLEffectParameterDataType.Vector4Array:
                case OpenGLEffectParameterDataType.ColorArray:
                case OpenGLEffectParameterDataType.MatrixArray:
                case OpenGLEffectParameterDataType.Texture2D:
                    return this.refData == other.refData;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Clears the data buffer.
        /// </summary>
        public void Clear()
        {
            dataType = OpenGLEffectParameterDataType.None;
            refData = null;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Boolean value)
        {
            dataType = OpenGLEffectParameterDataType.Boolean;
            fixed (Byte* pValData = valData)
            {
                *((Boolean*)pValData) = value;
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Int32 value)
        {
            dataType = OpenGLEffectParameterDataType.Int32;
            fixed (Byte* pValData = valData)
            {
                *((Int32*)pValData) = value;
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Int32[] value)
        {
            dataType = OpenGLEffectParameterDataType.Int32Array;
            refData = value;
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(UInt32 value)
        {
            dataType = OpenGLEffectParameterDataType.UInt32;
            fixed (Byte* pValData = valData)
            {
                *((UInt32*)pValData) = value;
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(UInt32[] value)
        {
            dataType = OpenGLEffectParameterDataType.UInt32Array;
            refData = value;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Single value)
        {
            dataType = OpenGLEffectParameterDataType.Single;
            fixed (Byte* pValData = valData)
            {
                *((Single*)pValData) = value;
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Single[] value)
        {
            dataType = OpenGLEffectParameterDataType.SingleArray;
            refData = value;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Double value)
        {
            dataType = OpenGLEffectParameterDataType.Double;
            fixed (Byte* pValData = valData)
            {
                *((Double*)pValData) = value;
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Double[] value)
        {
            dataType = OpenGLEffectParameterDataType.DoubleArray;
            refData = value;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector2 value)
        {
            dataType = OpenGLEffectParameterDataType.Vector2;
            fixed (Byte* pValData = valData)
            {
                *((Vector2*)pValData) = value;
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector2[] value)
        {
            dataType = OpenGLEffectParameterDataType.Vector2Array;
            refData = value;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector3 value)
        {
            dataType = OpenGLEffectParameterDataType.Vector3;
            fixed (Byte* pValData = valData)
            {
                *((Vector3*)pValData) = value;
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector3[] value)
        {
            dataType = OpenGLEffectParameterDataType.Vector3Array;
            refData = value;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector4 value)
        {
            dataType = OpenGLEffectParameterDataType.Vector4;
            fixed (Byte* pValData = valData)
            {
                *((Vector4*)pValData) = value;
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Vector4[] value)
        {
            dataType = OpenGLEffectParameterDataType.Vector4Array;
            refData = value;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Color value)
        {
            dataType = OpenGLEffectParameterDataType.Color;
            fixed (Byte* pValData = valData)
            {
                *((Color*)pValData) = value;
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Color[] value)
        {
            dataType = OpenGLEffectParameterDataType.ColorArray;
            refData = value;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Matrix value)
        {
            dataType = OpenGLEffectParameterDataType.Matrix;
            fixed (Byte* pValData = valData)
            {
                *((Matrix*)pValData) = value;
            }
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Matrix[] value)
        {
            dataType = OpenGLEffectParameterDataType.MatrixArray;
            refData = value;
        }

        /// <summary>
        /// Sets a value into the buffer.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(Texture2D value)
        {
            dataType = OpenGLEffectParameterDataType.Texture2D;
            refData = value;
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Boolean GetBoolean()
        {
            if (dataType == OpenGLEffectParameterDataType.None)
                return default(Boolean);

            if (dataType == OpenGLEffectParameterDataType.Boolean)
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
            if (dataType == OpenGLEffectParameterDataType.None)
                return default(Int32);

            if (dataType == OpenGLEffectParameterDataType.Int32)
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
            if (dataType == OpenGLEffectParameterDataType.None)
                return null;

            if (dataType == OpenGLEffectParameterDataType.Int32Array)
                return (Int32[])refData;
            
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public UInt32 GetUInt32()
        {
            if (dataType == OpenGLEffectParameterDataType.None)
                return default(UInt32);

            if (dataType == OpenGLEffectParameterDataType.UInt32)
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
            if (dataType == OpenGLEffectParameterDataType.None)
                return null;

            if (dataType == OpenGLEffectParameterDataType.UInt32Array)
                return (UInt32[])refData;

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Single GetSingle()
        {
            if (dataType == OpenGLEffectParameterDataType.None)
                return default(Single);

            if (dataType == OpenGLEffectParameterDataType.Single)
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
            if (dataType == OpenGLEffectParameterDataType.None)
                return null;

            if (dataType == OpenGLEffectParameterDataType.SingleArray)
                return (Single[])refData;

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Double GetDouble()
        {
            if (dataType == OpenGLEffectParameterDataType.None)
                return default(Double);

            if (dataType == OpenGLEffectParameterDataType.Double)
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
            if (dataType == OpenGLEffectParameterDataType.None)
                return null;

            if (dataType == OpenGLEffectParameterDataType.DoubleArray)
                return (Double[])refData;
            
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector2 GetVector2()
        {
            if (dataType == OpenGLEffectParameterDataType.None)
                return default(Vector2);

            if (dataType == OpenGLEffectParameterDataType.Vector2)
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
            if (dataType == OpenGLEffectParameterDataType.None)
                return null;

            if (dataType == OpenGLEffectParameterDataType.Vector2Array)
                return (Vector2[])refData;
            
            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector3 GetVector3()
        {
            if (dataType == OpenGLEffectParameterDataType.None)
                return default(Vector3);

            if (dataType == OpenGLEffectParameterDataType.Vector3)
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
            if (dataType == OpenGLEffectParameterDataType.None)
                return null;

            if (dataType == OpenGLEffectParameterDataType.Vector3Array)
                return (Vector3[])refData;

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector4 GetVector4()
        {
            if (dataType == OpenGLEffectParameterDataType.None)
                return default(Vector4);

            if (dataType == OpenGLEffectParameterDataType.Vector4)
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
            if (dataType == OpenGLEffectParameterDataType.None)
                return null;

            if (dataType == OpenGLEffectParameterDataType.Vector4Array)
                return (Vector4[])refData;

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Color GetColor()
        {
            if (dataType == OpenGLEffectParameterDataType.None)
                return default(Color);

            if (dataType == OpenGLEffectParameterDataType.Color)
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
            if (dataType == OpenGLEffectParameterDataType.None)
                return null;

            if (dataType == OpenGLEffectParameterDataType.ColorArray)
                return (Color[])refData;

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Matrix GetMatrix()
        {
            if (dataType == OpenGLEffectParameterDataType.None)
                return default(Matrix);

            if (dataType == OpenGLEffectParameterDataType.Matrix)
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
            if (dataType == OpenGLEffectParameterDataType.None)
                return null;

            if (dataType == OpenGLEffectParameterDataType.MatrixArray)
                return (Matrix[])refData;

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Texture2D GetTexture2D()
        {
            if (dataType == OpenGLEffectParameterDataType.None)
                return null;

            if (dataType == OpenGLEffectParameterDataType.Texture2D)
                return (Texture2D)refData;

            throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the type of data currently stored in this buffer.
        /// </summary>
        public OpenGLEffectParameterDataType DataType
        {
            get { return dataType; }
        }

        // Property values.
        private OpenGLEffectParameterDataType dataType;

        // State values.
        private Byte[] valData;
        private Object refData;
    }
}
