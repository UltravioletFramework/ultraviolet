using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet
{
    /// <summary>
    /// Initializes factory methods for the Framework core.
    /// </summary>
    internal sealed class UltravioletFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The <see cref="UltravioletFactory"/> to initialize.</param>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod(owner.IsRunningInServiceMode ? 
                new SpriteBatchFactory((uv) => null) : 
                new SpriteBatchFactory((uv) => new SpriteBatch(uv)));
        }
    }
}
