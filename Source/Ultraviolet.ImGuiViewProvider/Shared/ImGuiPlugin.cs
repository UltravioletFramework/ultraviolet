using Ultraviolet.Core;

namespace Ultraviolet.ImGuiViewProvider
{
    /// <summary>
    /// Represents a plugin for the Ultraviolet Framework which provides user interface views using Dear ImGui.
    /// </summary>
    public sealed class ImGuiPlugin : UltravioletPlugin
    {
        /// <inheritdoc/>
        public override void Register(UltravioletConfiguration ultravioletConfig)
        {
            Contract.Require(ultravioletConfig, nameof(ultravioletConfig));

            ultravioletConfig.ViewProviderAssembly = typeof(ImGuiPlugin).Assembly.FullName;
            ultravioletConfig.ViewProviderConfiguration = null;
        }
    }
}
