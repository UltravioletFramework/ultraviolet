using TwistedLogik.Ultraviolet.BASS;

namespace UltravioletSample.Sample1_CreatingAnApplication
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

