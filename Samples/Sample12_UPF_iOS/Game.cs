using Ultraviolet.BASS;
using Ultraviolet.UI.Presentation.CompiledExpressions;

namespace UltravioletSample.Sample12_UPF
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

