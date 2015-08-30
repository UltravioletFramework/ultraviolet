using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents an object which can compile an application's UPF binding expressions into a managed assembly.
    /// </summary>
    public interface IBindingExpressionCompiler
    {
        /// <summary>
        /// Traverses the directory tree rooted in <paramref name="root"/> and builds an assembly containing the compiled binding
        /// expressions of any UPF views which are found therein.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="root">The path to the root directory to search.</param>
        /// <param name="output">The filename of the file to which to save the resulting assembly.</param>
        void Compile(UltravioletContext uv, String root, String output);
    }
}
