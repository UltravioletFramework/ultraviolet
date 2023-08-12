using Ultraviolet.Audio;
using Ultraviolet.BASS.Audio;
using Ultraviolet.Core;

namespace Ultraviolet.BASS
{
    /// <summary>
    /// Represents an Ultraviolet plugin which registers BASS as the audio subsystem implementation.
    /// </summary>
    public class BASSAudioPlugin : UltravioletPlugin
    {
        /// <inheritdoc/>
        public override void Configure(UltravioletContext uv, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<UltravioletAudioFactory>((uv, configuration) => new BASSUltravioletAudio(uv));

            {
                factory.SetFactoryMethod<SongPlayerFactory>((uv) => new BASSSongPlayer(uv));
                factory.SetFactoryMethod<SoundEffectPlayerFactory>((uv) => new BASSSoundEffectPlayer(uv));
            }
            base.Configure(uv, factory);
        }

        /// <inheritdoc/>
        public override void Initialize(UltravioletContext uv, UltravioletFactory factory)
        {
            var content = uv.GetContent();
            {
                content.Importers.RegisterImporter<BASSMediaImporter>(BASSMediaImporter.SupportedExtensions);

                content.Processors.RegisterProcessor<BASSSongProcessor>();
                content.Processors.RegisterProcessor<BASSSoundEffectProcessor>();
            }
            base.Initialize(uv, factory);
        }

        /// <inheritdoc/>
        public override void Register(UltravioletConfiguration configuration)
        {
            Contract.Require(configuration, nameof(configuration));

            base.Register(configuration);
        }
    }
}
