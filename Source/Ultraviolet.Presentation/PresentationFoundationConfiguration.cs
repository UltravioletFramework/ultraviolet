using System;
using System.Reflection;

namespace Ultraviolet.Presentation
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
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            BindingExpressionCompilerAssembly = String.Format("Ultraviolet.Presentation.Compiler, Version={0}, Culture=neutral, PublicKeyToken=78da2f4877323311, processorArchitecture=MSIL", version);
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
