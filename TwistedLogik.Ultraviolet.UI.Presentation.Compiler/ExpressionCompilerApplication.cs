using TwistedLogik.Ultraviolet.OpenGL;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Compiler
{
    public class ExpressionCompilerApplication : UltravioletApplication
    {
        public ExpressionCompilerApplication() 
            : base ("TwistedLogik", "Expression Compiler")
        {
            PreserveApplicationSettings = false;
        }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new OpenGLUltravioletConfiguration() { EnableServiceMode = true, LoadCompatibilityShim = false };
            return new OpenGLUltravioletContext(this, configuration);
        }
    }
}
