using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Ultraviolet.Audio;
using Ultraviolet.Core;
using Ultraviolet.FMOD.Audio;

namespace Ultraviolet.FMOD
{
    /// <summary>
    /// Initializes factory methods for the FMOD implementation of the audio subsystem.
    /// </summary>
    public sealed class FMODUltravioletFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
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
    }
}
