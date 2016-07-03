using System;

namespace UvDebugSandbox
{
    partial class Game
    {
        partial void PlatformSpecificInitialization()
        {
            EnsureAssemblyIsLinked<TwistedLogik.Ultraviolet.BASS.BASSUltravioletAudio>();
            EnsureAssemblyIsLinked<TwistedLogik.Ultraviolet.UI.Presentation.CompiledExpressions.CompilerMetadata>();
        }
    }
}

