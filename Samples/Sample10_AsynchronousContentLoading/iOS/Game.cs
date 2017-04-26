using Ultraviolet.BASS;

namespace UltravioletSample.Sample10_AsynchronousContentLoading
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
        }
    }
}

