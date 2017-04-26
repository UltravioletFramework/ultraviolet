using Ultraviolet.BASS;
using Ultraviolet.Presentation.CompiledExpressions;

namespace UltravioletSample.Sample13_UPFAdvanced
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<BASSUltravioletAudio>();
            EnsureAssemblyIsLinked<CompilerMetadata>();
        }
    }
}

