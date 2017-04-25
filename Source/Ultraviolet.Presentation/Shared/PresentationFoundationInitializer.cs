using System;
using Ultraviolet.UI;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains methods for initializing the Presentation Foundation.
    /// </summary>
    internal sealed class PresentationFoundationInitializer : UIViewProviderInitializer
    {
        /// <inheritdoc/>
        public override void Initialize(UltravioletContext uv, Object configuration)
        {
            var config = (PresentationFoundationConfiguration)configuration ?? new PresentationFoundationConfiguration();

            var upf = uv.GetUI().GetPresentationFoundation();
            upf.BindingExpressionCompilerAssemblyName = config.BindingExpressionCompilerAssembly;
        }
    }
}
