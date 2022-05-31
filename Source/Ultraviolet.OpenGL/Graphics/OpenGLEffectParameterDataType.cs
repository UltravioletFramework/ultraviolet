
namespace Ultraviolet.OpenGL.Graphics
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
        /// A <see cref="System.Boolean"/> value is set on the effect.
        /// </summary>
        Boolean,

        /// <summary>
        /// An array of <see cref="System.Boolean"/> values is set on the effect.
        /// </summary>
        BooleanArray,

        /// <summary>
        /// An <see cref="System.Int32"/> value is set on the effect.
        /// </summary>
        Int32,

        /// <summary>
        /// An array of <see cref="System.Int32"/> values is set on the effect.
        /// </summary>
        Int32Array,

        /// <summary>
        /// A <see cref="System.UInt32"/> value is set on the effect.
        /// </summary>
        UInt32,

        /// <summary>
        /// An array of <see cref="System.UInt32"/> values is set on the effect.
        /// </summary>
        UInt32Array,

        /// <summary>
        /// A <see cref="System.Single"/> value is set on the effect.
        /// </summary>
        Single,

        /// <summary>
        /// An array of <see cref="System.Single"/> values is set on the effect.
        /// </summary>
        SingleArray,

        /// <summary>
        /// A <see cref="System.Double"/> value is set on the effect.
        /// </summary>
        Double,

        /// <summary>
        /// An array of <see cref="System.Double"/> values is set on the effect.
        /// </summary>
        DoubleArray,

        /// <summary>
        /// A <see cref="Ultraviolet.Vector2"/> value is set on the effect.
        /// </summary>
        Vector2,

        /// <summary>
        /// An array of <see cref="Ultraviolet.Vector2"/> values is set on the effect.
        /// </summary>
        Vector2Array,

        /// <summary>
        /// A <see cref="Ultraviolet.Vector3"/> value is set on the effect.
        /// </summary>
        Vector3,

        /// <summary>
        /// An array of <see cref="Ultraviolet.Vector3"/> values is set on the effect.
        /// </summary>
        Vector3Array,

        /// <summary>
        /// A <see cref="Ultraviolet.Vector4"/> value is set on the effect.
        /// </summary>
        Vector4,

        /// <summary>
        /// An array of <see cref="Ultraviolet.Vector4"/> values is set on the effect.
        /// </summary>
        Vector4Array,

        /// <summary>
        /// A <see cref="Ultraviolet.Color"/> value is set on the effect.
        /// </summary>
        Color,

        /// <summary>
        /// An array of <see cref="Ultraviolet.Color"/> values is set on the effect.
        /// </summary>
        ColorArray,

        /// <summary>
        /// A <see cref="Uniforms.Mat2"/> value is set on the effect.
        /// </summary>
        Mat2,

        /// <summary>
        /// An array of <see cref="Uniforms.Mat2"/> values is set on the effect.
        /// </summary>
        Mat2Array,

        /// <summary>
        /// A <see cref="Uniforms.Mat2x3"/> value is set on the effect.
        /// </summary>
        Mat2x3,

        /// <summary>
        /// An array of <see cref="Uniforms.Mat2x3"/> values is set on the effect.
        /// </summary>
        Mat2x3Array,

        /// <summary>
        /// A <see cref="Uniforms.Mat2x4"/> value is set on the effect.
        /// </summary>
        Mat2x4,

        /// <summary>
        /// An array of <see cref="Uniforms.Mat2x4"/> values is set on the effect.
        /// </summary>
        Mat2x4Array,

        /// <summary>
        /// A <see cref="Uniforms.Mat3"/> value is set on the effect.
        /// </summary>
        Mat3,

        /// <summary>
        /// An array of <see cref="Uniforms.Mat3"/> values is set on the effect.
        /// </summary>
        Mat3Array,

        /// <summary>
        /// A <see cref="Uniforms.Mat3x2"/> value is set on the effect.
        /// </summary>
        Mat3x2,

        /// <summary>
        /// An array of <see cref="Uniforms.Mat3x2"/> values is set on the effect.
        /// </summary>
        Mat3x2Array,

        /// <summary>
        /// A <see cref="Uniforms.Mat3x4"/> value is set on the effect.
        /// </summary>
        Mat3x4,

        /// <summary>
        /// An array of <see cref="Uniforms.Mat3x4"/> values is set on the effect.
        /// </summary>
        Mat3x4Array,

        /// <summary>
        /// A <see cref="Ultraviolet.Matrix"/> value is set on the effect.
        /// </summary>
        Mat4,

        /// <summary>
        /// An array of <see cref="Ultraviolet.Matrix"/> values is set on the effect.
        /// </summary>
        Mat4Array,

        /// <summary>
        /// A <see cref="Uniforms.Mat4x2"/> value is set on the effect.
        /// </summary>
        Mat4x2,

        /// <summary>
        /// An array of <see cref="Uniforms.Mat4x2"/> values is set on the effect.
        /// </summary>
        Mat4x2Array,

        /// <summary>
        /// A <see cref="Uniforms.Mat4x3"/> value is set on the effect.
        /// </summary>
        Mat4x3,

        /// <summary>
        /// An array of <see cref="Uniforms.Mat4x3"/> values is set on the effect.
        /// </summary>
        Mat4x3Array,

        /// <summary>
        /// A <see cref="Ultraviolet.Graphics.Texture2D"/> value is set on the effect.
        /// </summary>
        Texture2D,

        /// <summary>
        /// A <see cref="Ultraviolet.Graphics.Texture3D"/> value is set on the effect.
        /// </summary>
        Texture3D,
    }
}
