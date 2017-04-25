using Ultraviolet.BASS;

namespace UltravioletSample.Sample6_RenderingText
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

