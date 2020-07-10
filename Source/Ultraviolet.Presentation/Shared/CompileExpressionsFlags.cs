using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a set of values which can be passed to the <see cref="PresentationFoundation.CompileExpressions(string, CompileExpressionsFlags)"/> method
    /// in order to configure expression compilation.
    /// </summary>
    [Flags]
    public enum CompileExpressionsFlags
    {
        /// <summary>
        /// No special compilation flags.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// The expression compiler will always compile expressions, even if an up-to-date cache file exists.
        /// </summary>
        IgnoreCache = 0x0001,

        /// <summary>
        /// The expression compiler will attempt to correlate errors with their original project files, rather 
        /// than the copies of those files which exist in the application's build directory. This leads to an 
        /// improved debugging experience, since any changes made to the output versions of these files will 
        /// be lost when the project is recompiled.
        /// </summary>
        ResolveContentFiles = 0x00002,

        /// <summary>
        /// The expression compiler will generate its output assembly in-memory, rather than producing
        /// a file on disk.
        /// </summary>
        GenerateInMemory = 0x0004,

        /// <summary>
        /// The expression compiler will perform its compilation in a temporary directory instead
        /// of in the application's working directory.
        /// </summary>
        WorkInTemporaryDirectory = 0x0008,

        /// <summary>
        /// The expression compiler will write its source code files out to the working directory. If
        /// the <see cref="WorkInTemporaryDirectory"/> flag is specified, this flag is ignored.
        /// </summary>
        WriteCompiledFilesToWorkingDirectory = 0x0010,
    }
}
