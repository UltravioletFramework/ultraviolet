
namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
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
    }
}
