using System.IO;
using System.Reflection;
using System;
using Ultraviolet.Audio;
using Ultraviolet.Core;
using Ultraviolet.FMOD.Audio;
using System.Linq;

namespace Ultraviolet.FMOD
{
    /// <summary>
    /// Represents an Ultraviolet plugin which registers FMOD as the audio subsystem implementation.
    /// </summary>
    public class FMODAudioPlugin : UltravioletPlugin
    {
        /// <inheritdoc/>
        public override void Configure(UltravioletContext uv, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<UltravioletAudioFactory>((uv, configuration) => new FMODUltravioletAudio(uv, configuration));

            {
                factory.SetFactoryMethod<SongPlayerFactory>((uv) => new FMODSongPlayer(uv));
                factory.SetFactoryMethod<SoundEffectPlayerFactory>((uv) => new FMODSoundEffectPlayer(uv));

                try
                {
                    if (UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Android)
                    {
                        var shim = Assembly.Load("Ultraviolet.Shims.Android.FMOD.dll");
                        var type = shim.GetTypes().Where(x => x.IsClass && !x.IsAbstract && typeof(FMODPlatformSpecificImplementationDetails).IsAssignableFrom(x)).SingleOrDefault();
                        if (type == null)
                            throw new InvalidOperationException(FMODStrings.CannotFindPlatformShimClass);

                        factory.SetFactoryMethod<FMODPlatformSpecificImplementationDetailsFactory>(
                            (uv) => (FMODPlatformSpecificImplementationDetails)Activator.CreateInstance(type));
                    }
                }
                catch (FileNotFoundException e)
                {
                    throw new InvalidCompatibilityShimException(UltravioletStrings.MissingCompatibilityShim.Format(e.FileName));
                }
            }
            base.Configure(uv, factory);
        }

        /// <inheritdoc/>
        public override void Initialize(UltravioletContext uv, UltravioletFactory factory)
        {
            var content = uv.GetContent();
            {
                content.Importers.RegisterImporter<FMODMediaImporter>(FMODMediaImporter.SupportedExtensions);

                content.Processors.RegisterProcessor<FMODSongProcessor>();
                content.Processors.RegisterProcessor<FMODSoundEffectProcessor>();
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
