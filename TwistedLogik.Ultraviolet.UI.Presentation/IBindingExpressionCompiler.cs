namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an object which can compile an application's UPF binding expressions into a managed assembly.
    /// </summary>
    public interface IBindingExpressionCompiler
    {
        /// <summary>
        /// Traverses the directory tree specified by <paramref name="options"/> and builds an assembly containing the compiled binding
        /// expressions of any UPF views which are found therein.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="options">The compiler options.</param>
        /// <returns>A <see cref="BindingExpressionCompilationResult"/> that represents the result of the compilation.</returns>
        BindingExpressionCompilationResult Compile(UltravioletContext uv, BindingExpressionCompilerOptions options);

        /// <summary>
        /// Compiles the specified view's binding expressions into a view model wrapper and produces a string
        /// containing the resulting C# code.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="options">The compiler options.</param>
        /// <returns>A <see cref="BindingExpressionCompilationResult"/> that represents the result of the compilation.</returns>
        BindingExpressionCompilationResult CompileSingleView(UltravioletContext uv, BindingExpressionCompilerOptions options);
    }
}
