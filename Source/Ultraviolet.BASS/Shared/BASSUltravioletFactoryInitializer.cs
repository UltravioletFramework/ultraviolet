using Ultraviolet.Audio;
using Ultraviolet.BASS.Audio;
using Ultraviolet.Core;

namespace Ultraviolet.BASS
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
