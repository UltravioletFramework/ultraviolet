namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an effect which exposes a texture parameter.
    /// </summary>
    public interface IEffectTexture
    {
        /// <summary>
        /// Gets or sets the texture being rendered.
        /// </summary>
        Texture2D Texture
        {
            get;
            set;
        }
    }
}
