using Ultraviolet.BASS;

namespace UltravioletSample.Sample4_ContentManagement
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

