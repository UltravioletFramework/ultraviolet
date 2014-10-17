
namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the effect used to render sprites.
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
