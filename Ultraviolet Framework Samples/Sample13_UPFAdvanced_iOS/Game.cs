using TwistedLogik.Ultraviolet.BASS;
using TwistedLogik.Ultraviolet.UI.Presentation.CompiledExpressions;

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

