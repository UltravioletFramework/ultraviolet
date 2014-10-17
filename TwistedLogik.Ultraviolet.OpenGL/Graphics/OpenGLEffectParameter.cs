using System;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the EffectParameter class.
    /// </summary>
    public unsafe sealed class OpenGLEffectParameter : EffectParameter
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLEffectParameter class.
        /// </summary>
		/// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The effect parameter's name.</param>
        /// <param name="type">The effect parameter's uniform type.</param>
        public OpenGLEffectParameter(UltravioletContext uv, String name, UInt32 type)
            : base(uv)
        {
            Contract.Require(name, "name");

            this.name = name ?? String.Empty;
            this.type = type;
        }

        /// <summary>
        /// Gets the parameter's value as a Boolean.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Boolean GetValueBoolean()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_BOOL);

            return data.GetBoolean();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Boolean value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_BOOL);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as an Int32.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Int32 GetValueInt32()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_INT);

            return data.GetInt32();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Int32 value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_INT);
            
            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as an array of Int32.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Int32[] GetValueInt32Array()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_INT);

            return data.GetInt32Array();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Int32[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_INT);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as a UInt32.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override UInt32 GetValueUInt32()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_UNSIGNED_INT);

            return data.GetUInt32();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(UInt32 value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_UNSIGNED_INT);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as an array of UInt32.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override UInt32[] GetValueUInt32Array()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_UNSIGNED_INT);

            return data.GetUInt32Array();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(UInt32[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_UNSIGNED_INT);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as a Single.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Single GetValueSingle()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT);

            return data.GetSingle();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Single value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as an array of Single.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Single[] GetValueSingleArray()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT);

            return data.GetSingleArray();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Single[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as a Double.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Double GetValueDouble()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_DOUBLE);

            return data.GetDouble();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Double value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_DOUBLE);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as an array of Double.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Double[] GetValueDoubleArray()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_DOUBLE);

            return data.GetDoubleArray();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Double[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_DOUBLE);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as a Vector2.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Vector2 GetValueVector2()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC2);

            return data.GetVector2();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Vector2 value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC2);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as an array of Vector2.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Vector2[] GetValueVector2Array()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC2);

            return data.GetVector2Array();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Vector2[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC2);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as a Vector3.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Vector3 GetValueVector3()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3);

            return data.GetVector3();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Vector3 value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as an array of Vector3.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Vector3[] GetValueVector3Array()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3);

            return data.GetVector3Array();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Vector3[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as a Vector4.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Vector4 GetValueVector4()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC4);

            return data.GetVector4();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Vector4 value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC4);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as an array of Vector4.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Vector4[] GetValueVector4Array()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC4);

            return data.GetVector4Array();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Vector4[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC4);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as a Color.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Color GetValueColor()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3 || type == gl.GL_FLOAT_VEC4);

            return data.GetColor();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Color value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3 || type == gl.GL_FLOAT_VEC4);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as a Matrix.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Matrix GetValueMatrix()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_MAT4);

            return data.GetMatrix();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Matrix value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_MAT4);

            data.Set(value);
        }

        /// <summary>
        /// Gets the parameter's value as a Texture2D.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public override Texture2D GetValueTexture2D()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_SAMPLER_2D);

            return data.GetTexture2D();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public override void SetValue(Texture2D value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_SAMPLER_2D);

            data.Set(value);
        }

        /// <summary>
        /// Gets the effect parameter's name.
        /// </summary>
        public override String Name
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return name; 
            }
        }

        /// <summary>
        /// Gets the parameter's data container.
        /// </summary>
        public OpenGLEffectParameterData Data
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return data;
            }
        }

        // Property values.
        private readonly String name;

        // State values.
        private readonly UInt32 type;
        private readonly OpenGLEffectParameterData data = new OpenGLEffectParameterData();
    }
}
