using TwistedLogik.Ultraviolet.Audio;
using TwistedLogik.Ultraviolet.BASS.Audio;

namespace TwistedLogik.Ultraviolet.BASS
{
    /// <summary>
    /// Initializes factory methods for the BASS implementation of the audio subsystem.
    /// </summary>
    public sealed class BASSUltravioletFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The factory to initialize.</param>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SongPlayerFactory>((uv) => new BASSSongPlayer(uv));
            factory.SetFactoryMethod<SoundEffectPlayerFactory>((uv) => new BASSSoundEffectPlayer(uv));
        }
    }
}
