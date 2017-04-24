using TwistedLogik.Ultraviolet.BASS;

namespace UltravioletSample.Sample11_GamePads
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

