using Ultraviolet.BASS;

namespace UltravioletSample.Sample9_ManagingStateWithUIScreens
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

