using System;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an effect which exposes texture width and height parameters.
    /// </summary>
    public interface IEffectTextureSizes
    {
        /// <summary>
        /// Gets or sets the width of the texture being rendered.
        /// </summary>
        Int32 TextureWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of the texture being rendered.
        /// </summary>
        Int32 TextureHeight
        {
            get;
            set;
        }
    }
}
