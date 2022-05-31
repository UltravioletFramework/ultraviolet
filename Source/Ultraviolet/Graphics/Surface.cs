namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the base class for all surfaces.
    /// </summary>
    public abstract class Surface : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Surface"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public Surface(UltravioletContext uv)
            : base(uv)
        {

        }
    }
}
