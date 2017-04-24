using Microsoft.CSharp;
using Ultraviolet.Presentation.Compiler;

namespace Ultraviolet.Presentation.RoslynCompiler
{
    /// <summary>
    /// Represents a binding expression compiler which is backed by the .NET Compiler Platform ("Roslyn").
    /// </summary>
    public class RoslynExpressionCompiler : ExpressionCompiler
    {
        /// <inheritdoc/>
        protected override CSharpCodeProvider CreateCodeProvider()
        {
            return new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
        }
    }
}
