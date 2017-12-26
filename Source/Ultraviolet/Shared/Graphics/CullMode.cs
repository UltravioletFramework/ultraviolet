
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the cull mode, which instructs the graphics device as to which 
    /// winding orders may be used for back face culling.
    /// </summary>
    public enum CullMode
    {
        /// <summary>
        /// Do not cull back faces.
        /// </summary>
        None,

        /// <summary>
        /// Cull back faces with clockwise vertices.
        /// </summary>
        CullClockwiseFace,

        /// <summary>
        /// Cull back faces with counter-clockwise vertices.
        /// </summary>
        CullCounterClockwiseFace,
    }
}
