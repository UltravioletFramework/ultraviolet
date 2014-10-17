using System;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an effect parameter.
    /// </summary>
    public abstract class EffectParameter : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the EffectParameter class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public EffectParameter(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Gets the parameter's value as a Boolean.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Boolean GetValueBoolean();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Boolean value);

        /// <summary>
        /// Gets the parameter's value as an Int32.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Int32 GetValueInt32();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Int32 value);

        /// <summary>
        /// Gets the parameter's value as an array of Int32.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Int32[] GetValueInt32Array();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Int32[] value);

        /// <summary>
        /// Gets the parameter's value as a UInt32.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        [CLSCompliant(false)]
        public virtual UInt32 GetValueUInt32()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        [CLSCompliant(false)]
        public virtual void SetValue(UInt32 value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the parameter's value as an array of UInt32.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        [CLSCompliant(false)]
        public virtual UInt32[] GetValueUInt32Array()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        [CLSCompliant(false)]
        public virtual void SetValue(UInt32[] value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the parameter's value as a Single.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Single GetValueSingle();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Single value);

        /// <summary>
        /// Gets the parameter's value as an array of Single.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Single[] GetValueSingleArray();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Single[] value);

        /// <summary>
        /// Gets the parameter's value as a Double.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Double GetValueDouble();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Double value);

        /// <summary>
        /// Gets the parameter's value as an array of Double.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Double[] GetValueDoubleArray();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Double[] value);

        /// <summary>
        /// Gets the parameter's value as a Vector2.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Vector2 GetValueVector2();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Vector2 value);

        /// <summary>
        /// Gets the parameter's value as an array of Vector2.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Vector2[] GetValueVector2Array();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Vector2[] value);

        /// <summary>
        /// Gets the parameter's value as a Vector3.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Vector3 GetValueVector3();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Vector3 value);

        /// <summary>
        /// Gets the parameter's value as an array of Vector3.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Vector3[] GetValueVector3Array();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Vector3[] value);

        /// <summary>
        /// Gets the parameter's value as a Vector4.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Vector4 GetValueVector4();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Vector4 value);

        /// <summary>
        /// Gets the parameter's value as an array of Vector4.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Vector4[] GetValueVector4Array();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Vector4[] value);

        /// <summary>
        /// Gets the parameter's value as a Color.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Color GetValueColor();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Color value);

        /// <summary>
        /// Gets the parameter's value as a Matrix.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Matrix GetValueMatrix();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Matrix value);

        /// <summary>
        /// Gets the parameter's value as a Texture2D.
        /// </summary>
        /// <returns>The parameter's value.</returns>
        public abstract Texture2D GetValueTexture2D();

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void SetValue(Texture2D value);

        /// <summary>
        /// Gets the effect parameter's name.
        /// </summary>
        public abstract String Name
        {
            get;
        }
    }
}
