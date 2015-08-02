using Microsoft.CSharp;
using TwistedLogik.Ultraviolet.UI.Presentation.Compiler;

namespace TwistedLogik.Ultraviolet.UI.Presentation.RoslynCompiler
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
