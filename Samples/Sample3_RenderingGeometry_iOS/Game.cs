using Ultraviolet.BASS;

namespace UltravioletSample.Sample3_RenderingGeometry
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

