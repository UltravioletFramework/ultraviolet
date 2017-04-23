using Ultraviolet.Core;
using TwistedLogik.Ultraviolet.Audio;
using TwistedLogik.Ultraviolet.BASS.Audio;

namespace TwistedLogik.Ultraviolet.BASS
{
    /// <summary>
    /// Initializes factory methods for the BASS implementation of the audio subsystem.
    /// </summary>
    [Preserve(AllMembers = true)]
    public sealed class BASSUltravioletFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SongPlayerFactory>((uv) => new BASSSongPlayer(uv));
            factory.SetFactoryMethod<SoundEffectPlayerFactory>((uv) => new BASSSoundEffectPlayer(uv));
        }
    }
}
