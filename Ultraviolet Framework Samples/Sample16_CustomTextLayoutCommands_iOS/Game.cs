using TwistedLogik.Ultraviolet.BASS;

namespace UltravioletSample.Sample16_CustomTextLayoutCommands
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

