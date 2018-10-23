using ImGuiNET;

namespace Ultraviolet.ImGui
{
    /// <summary>
    /// Represents a plugin for the Ultraviolet Framework which provides user interface views using ImGui.NET.
    /// </summary>
    public sealed class ImGuiPlugin : UltravioletPlugin
    {
        /// <inheritdoc/>
        public override void Configure(UltravioletConfiguration configuration)
        {
            configuration.ViewProviderAssembly = typeof(ImGuiPlugin).Assembly.FullName;
            configuration.ViewProviderConfiguration = null;

            base.Configure(configuration);
        }
    }
}
