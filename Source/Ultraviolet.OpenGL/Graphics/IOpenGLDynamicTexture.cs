namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents a dynamic texture which should be flushed prior to being boound for reading.
    /// </summary>
    public interface IOpenGLDynamicTexture
    {
        /// <summary>
        /// Flushes any pending changes and uploads the result to video memory.
        /// </summary>
        void Flush();
    }
}
