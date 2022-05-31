
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an effect which exposes a combined view-projection matrix.
    /// </summary>
    public interface IEffectViewProj
    {
        /// <summary>
        /// Gets or sets the effect's view-projection matrix.
        /// </summary>
        Matrix ViewProj
        {
            get;
            set;
        }
    }
}
