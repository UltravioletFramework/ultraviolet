namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the type of asset from which an effect source was loaded.
    /// </summary>
    internal enum OpenGLEffectSourceAssetType
    {
        /// <summary>
        /// The effect source was loaded from a JSON definition file.
        /// </summary>
        JObject,

        /// <summary>
        /// The effect source was loaded from a shader source file.
        /// </summary>
        ShaderSource,

        /// <summary>
        /// The effect source was loaded from an XML definition file.
        /// </summary>
        XDocument,
    }
}
