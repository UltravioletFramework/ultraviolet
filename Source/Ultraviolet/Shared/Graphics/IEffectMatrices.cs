
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an effect which exposes world, view, and projection matrix parameters.
    /// </summary>
    public interface IEffectMatrices
    {
        /// <summary>
        /// Gets or sets the effect's world matrix.
        /// </summary>
        Matrix World
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the effect's view matrix.
        /// </summary>
        Matrix View
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the effect's projection matrix.
        /// </summary>
        Matrix Projection
        {
            get;
            set;
        }
    }
}
