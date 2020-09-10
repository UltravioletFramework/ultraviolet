
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the supported formats for vertex data.
    /// </summary>
    public enum VertexElementFormat
    {
        /// <summary>
        /// 8-bit unsigned byte with four components.
        /// </summary>
        Color,

        /// <summary>
        /// 8-bit signed integer value.
        /// </summary>
        SByte,

        /// <summary>
        /// Packed vector containing two 8-bit signed integer values.
        /// </summary>
        SByte2,

        /// <summary>
        /// Packed vector containing three 8-bit signed integer values.
        /// </summary>
        SByte3,

        /// <summary>
        /// Packed vector containing four 8-bit signed integer values.
        /// </summary>
        SByte4,

        /// <summary>
        /// 8-bit signed normalized integer value ranging from -1 to +1.
        /// </summary>
        NormalizedSByte,

        /// <summary>
        /// Packed vector containing two 8-bit signed normalized integer values ranging from -1 to +1.
        /// </summary>
        NormalizedSByte2,

        /// <summary>
        /// Packed vector containing three 8-bit signed normalized integer values ranging from -1 to +1.
        /// </summary>
        NormalizedSByte3,

        /// <summary>
        /// Packed vector containing four 8-bit signed normalized integer values ranging from -1 to +1.
        /// </summary>
        NormalizedSByte4,

        /// <summary>
        /// 8-bit unsigned integer value.
        /// </summary>
        Byte,

        /// <summary>
        /// Packed vector containing two 8-bit unsigned integer values.
        /// </summary>
        Byte2,

        /// <summary>
        /// Packed vector containing three 8-bit unsigned integer values.
        /// </summary>
        Byte3,

        /// <summary>
        /// Packed vector containing four 8-bit unsigned integer values.
        /// </summary>
        Byte4,

        /// <summary>
        /// 8-bit unsigned normalized integer value ranging from 0 to +1.
        /// </summary>
        NormalizedByte,

        /// <summary>
        /// Packed vector containing two 8-bit unsigned normalized integer values ranging from 0 to +1.
        /// </summary>
        NormalizedByte2,

        /// <summary>
        /// Packed vector containing three 8-bit unsigned normalized integer values ranging from 0 to +1.
        /// </summary>
        NormalizedByte3,

        /// <summary>
        /// Packed vector containing four 8-bit unsigned normalized integer values ranging from 0 to +1.
        /// </summary>
        NormalizedByte4,

        /// <summary>
        /// 16-bit signed integer value.
        /// </summary>
        Short,

        /// <summary>
        /// Packed vector containing two 16-bit signed integer values.
        /// </summary>
        Short2,

        /// <summary>
        /// Packed vector containing three 16-bit signed integer values.
        /// </summary>
        Short3,

        /// <summary>
        /// Packed vector containing two 16-bit signed integer values.
        /// </summary>
        Short4,

        /// <summary>
        /// 16-bit signed normalized integer value ranging from -1 to +1.
        /// </summary>
        NormalizedShort,

        /// <summary>
        /// Packed vector containing two 16-bit signed normalized integer values ranging from -1 to +1.
        /// </summary>
        NormalizedShort2,

        /// <summary>
        /// Packed vector containing three 16-bit signed normalized integer values ranging from -1 to +1.
        /// </summary>
        NormalizedShort3,

        /// <summary>
        /// Packed vector containing four 16-bit signed normalized integer values ranging from -1 to +1.
        /// </summary>
        NormalizedShort4,

        /// <summary>
        /// 16-bit unsigned integer value.
        /// </summary>
        UnsignedShort,

        /// <summary>
        /// Packed vector containing two 16-bit unsigned integer values.
        /// </summary>
        UnsignedShort2,

        /// <summary>
        /// Packed vector containing three 16-bit unsigned integer values.
        /// </summary>
        UnsignedShort3,

        /// <summary>
        /// Packed vector containing four 16-bit unsigned integer values.
        /// </summary>
        UnsignedShort4,

        /// <summary>
        /// 16-bit unsigned normalized integer value ranging from 0 to +1.
        /// </summary>
        NormalizedUnsignedShort,

        /// <summary>
        /// Packed vector containing two 16-bit unsigned normalized integer values ranging from 0 to +1.
        /// </summary>
        NormalizedUnsignedShort2,

        /// <summary>
        /// Packed vector containing three 16-bit unsigned normalized integer values ranging from 0 to +1.
        /// </summary>
        NormalizedUnsignedShort3,

        /// <summary>
        /// Packed vector containing four 16-bit unsigned normalized integer values ranging from 0 to +1.
        /// </summary>
        NormalizedUnsignedShort4,

        /// <summary>
        /// 32-bit signed integer value.
        /// </summary>
        Int,

        /// <summary>
        /// Packed vector containing two 32-bit signed integer values.
        /// </summary>
        Int2,

        /// <summary>
        /// Packed vector containing three 32-bit signed integer values.
        /// </summary>
        Int3,

        /// <summary>
        /// Packed vector containing two 32-bit signed integer values.
        /// </summary>
        Int4,

        /// <summary>
        /// 32-bit normalizeed signed integer value.
        /// </summary>
        NormalizedInt,

        /// <summary>
        /// Packed vector containing two 32-bit normalizeed signed integer values.
        /// </summary>
        NormalizedInt2,

        /// <summary>
        /// Packed vector containing three 32-bit normalizeed signed integer values.
        /// </summary>
        NormalizedInt3,

        /// <summary>
        /// Packed vector containing two 32-bit normalizeed signed integer values.
        /// </summary>
        NormalizedInt4,

        /// <summary>
        /// 32-bit unsigned integer value.
        /// </summary>
        UnsignedInt,

        /// <summary>
        /// Packed vector containing two 32-bit unsigned integer values.
        /// </summary>
        UnsignedInt2,

        /// <summary>
        /// Packed vector containing three 32-bit unsigned integer values.
        /// </summary>
        UnsignedInt3,

        /// <summary>
        /// Packed vector containing four 32-bit unsigned integer values.
        /// </summary>
        UnsignedInt4,

        /// <summary>
        /// 32-bit normalized unsigned integer value.
        /// </summary>
        NormalizedUnsignedInt,

        /// <summary>
        /// Packed vector containing two 32-bit normalized unsigned integer values.
        /// </summary>
        NormalizedUnsignedInt2,

        /// <summary>
        /// Packed vector containing three 32-bit normalized unsigned integer values.
        /// </summary>
        NormalizedUnsignedInt3,

        /// <summary>
        /// Packed vector containing four 32-bit normalized unsigned integer values.
        /// </summary>
        NormalizedUnsignedInt4,

        /// <summary>
        /// 32-bit floating point with one component.
        /// </summary>
        Single,

        /// <summary>
        /// 32-bit floating point with two components.
        /// </summary>
        Vector2,

        /// <summary>
        /// 32-bit floating point with three components.
        /// </summary>
        Vector3,

        /// <summary>
        /// 32-bit floating point with three components.
        /// </summary>
        Vector4,
    }
}
