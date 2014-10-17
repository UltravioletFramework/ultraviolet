
namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the types of data that can be set on an effect.
    /// </summary>
    public enum OpenGLEffectParameterDataType
    {
        /// <summary>
        /// No data is set on the effect.
        /// </summary>
        None,

        /// <summary>
        /// A Boolean value is set on the effect.
        /// </summary>
        Boolean,

        /// <summary>
        /// An Int32 value is set on the effect.
        /// </summary>
        Int32,

        /// <summary>
        /// An array of Int32 values is set on the effect.
        /// </summary>
        Int32Array,

        /// <summary>
        /// A UInt32 value is set on the effect.
        /// </summary>
        UInt32,

        /// <summary>
        /// An array of UInt32 values is set on the effect.
        /// </summary>
        UInt32Array,

        /// <summary>
        /// A Single value is set on the effect.
        /// </summary>
        Single,

        /// <summary>
        /// An array of Single values is set on the effect.
        /// </summary>
        SingleArray,

        /// <summary>
        /// A Double value is set on the effect.
        /// </summary>
        Double,

        /// <summary>
        /// An array of Double values is set on the effect.
        /// </summary>
        DoubleArray,

        /// <summary>
        /// A Vector2 value is set on the effect.
        /// </summary>
        Vector2,

        /// <summary>
        /// An array of Vector2 values is set on the effect.
        /// </summary>
        Vector2Array,

        /// <summary>
        /// A Vector3 value is set on the effect.
        /// </summary>
        Vector3,

        /// <summary>
        /// An array of Vector3 values is set on the effect.
        /// </summary>
        Vector3Array,

        /// <summary>
        /// A Vector4 value is set on the effect.
        /// </summary>
        Vector4,

        /// <summary>
        /// An array of Vector3 values is set on the effect.
        /// </summary>
        Vector4Array,

        /// <summary>
        /// A Color value is set on the effect.
        /// </summary>
        Color,

        /// <summary>
        /// A Matrix value is set on the effect.
        /// </summary>
        Matrix,

        /// <summary>
        /// A Texture2D value is set on the effect.
        /// </summary>
        Texture2D,
    }
}
