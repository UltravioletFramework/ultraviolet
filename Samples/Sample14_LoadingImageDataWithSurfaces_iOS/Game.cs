using TwistedLogik.Ultraviolet.BASS;

namespace UltravioletSample.Sample14_LoadingImageDataWithSurfaces
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

