using System;

namespace UvDebugSandbox
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<Ultraviolet.BASS.BASSUltravioletAudio>();
            EnsureAssemblyIsLinked<Ultraviolet.UI.Presentation.CompiledExpressions.CompilerMetadata>();
        }
    }
}

