namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the categories of vertex attribute types (single-precision floating point, double-precision
    /// floating point, or integer).
    /// </summary>
    public enum OpenGLAttribCategory
    {
        /// <summary>
        /// The attribute is a single-precision floating point value and should be set using glVertexAttribPointer.
        /// </summary>
        Single,

        /// <summary>
        /// The attribute is a double-precision floating point value and should be set using glVertexAttribLPointer.
        /// </summary>
        Double,

        /// <summary>
        /// The attribute is an integer value and should be set using glVertexAttribIPointer.
        /// </summary>
        Integer,
    }
}
