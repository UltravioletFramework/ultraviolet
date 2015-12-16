using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the configuration settings for the Ultraviolet Presentation Foundation.
    /// </summary>
    public sealed class PresentationFoundationConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundationConfiguration"/> class.
        /// </summary>
        public PresentationFoundationConfiguration()
        {
#if SIGNED
            BindingExpressionCompilerAssembly = "TwistedLogik.Ultraviolet.UI.Presentation.Compiler, Version=1.2.0.0, Culture=neutral, PublicKeyToken=78da2f4877323311, processorArchitecture=MSIL";
#else
            BindingExpressionCompilerAssembly = "TwistedLogik.Ultraviolet.UI.Presentation.Compiler, Version=1.2.0.0, Culture=neutral, processorArchitecture=MSIL";
#endif
        }

        /// <summary>
        /// Gets or sets the name of the assembly that implements the binding expression compiler.
        /// </summary>
        public String BindingExpressionCompilerAssembly
        {
            get;
            set;
        }
    }
}
