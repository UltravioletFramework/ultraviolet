using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents an <see cref="Effect"/> used to render sprites.
    /// </summary>
    public interface ISpriteBatchEffect
    {
        /// <summary>
        /// Gets or sets the effect's transformation matrix.
        /// </summary>
        Matrix MatrixTransform
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the effect should transform
        /// colors from the sRGB space to the linear space in the vertex shader.
        /// </summary>
        Boolean SrgbColor
        {
            get;
            set;
        }
    }
}
