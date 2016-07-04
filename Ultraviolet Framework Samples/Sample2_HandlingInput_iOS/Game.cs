using TwistedLogik.Ultraviolet.BASS;

namespace UltravioletSample.Sample2_HandlingInput
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

