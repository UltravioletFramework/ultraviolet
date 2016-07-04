using TwistedLogik.Ultraviolet.BASS;

namespace UltravioletSample.Sample7_PlayingMusic
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

