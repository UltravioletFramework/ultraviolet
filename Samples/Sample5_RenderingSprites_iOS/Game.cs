using TwistedLogik.Ultraviolet.BASS;

namespace UltravioletSample.Sample5_RenderingSprites
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

