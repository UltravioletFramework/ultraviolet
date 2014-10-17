using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

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
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Boolean);

            fixed (Byte* pValData = valData)
            {
                return *((Boolean*)pValData);
            }
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Int32 GetInt32()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Int32);

            fixed (Byte* pValData = valData)
            {
                return *((Int32*)pValData);
            }
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Int32[] GetInt32Array()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Int32Array);

            return (Int32[])refData;
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public UInt32 GetUInt32()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.UInt32);

            fixed (Byte* pValData = valData)
            {
                return *((UInt32*)pValData);
            }
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public UInt32[] GetUInt32Array()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.UInt32Array);

            return (UInt32[])refData;
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Single GetSingle()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Single);

            fixed (Byte* pValData = valData)
            {
                return *((Single*)pValData);
            }
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Single[] GetSingleArray()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.SingleArray);

            return (Single[])refData;
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Double GetDouble()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Double);

            fixed (Byte* pValData = valData)
            {
                return *((Double*)pValData);
            }
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Double[] GetDoubleArray()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.DoubleArray);

            return (Double[])refData;
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector2 GetVector2()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Vector2);

            fixed (Byte* pValData = valData)
            {
                return *((Vector2*)pValData);
            }
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector2[] GetVector2Array()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Vector2Array);

            return (Vector2[])refData;
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector3 GetVector3()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Vector3);

            fixed (Byte* pValData = valData)
            {
                return *((Vector3*)pValData);
            }
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector3[] GetVector3Array()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Vector3Array);

            return (Vector3[])refData;
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector4 GetVector4()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Vector4);

            fixed (Byte* pValData = valData)
            {
                return *((Vector4*)pValData);
            }
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Vector4[] GetVector4Array()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Vector4Array);

            return (Vector4[])refData;
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Color GetColor()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Color);

            fixed (Byte* pValData = valData)
            {
                return *((Color*)pValData);
            }
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Matrix GetMatrix()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Matrix);

            fixed (Byte* pValData = valData)
            {
                return *((Matrix*)pValData);
            }
        }

        /// <summary>
        /// Gets the value that is set into the buffer.
        /// </summary>
        /// <returns>The value that is set into the buffer.</returns>
        public Texture2D GetTexture2D()
        {
            Contract.Ensure<InvalidCastException>(dataType == OpenGLEffectParameterDataType.Texture2D);

            return (Texture2D)refData;
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
