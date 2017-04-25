namespace UvStressTest
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<Ultraviolet.BASS.BASSUltravioletAudio>();
        }
    }
}

