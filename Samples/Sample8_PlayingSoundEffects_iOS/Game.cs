using TwistedLogik.Ultraviolet.BASS;

namespace UltravioletSample.Sample8_PlayingSoundEffects
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

