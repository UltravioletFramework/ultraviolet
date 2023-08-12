using Ultraviolet.Core;
using Ultraviolet.UI;

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

            ultravioletConfig.ViewProviderConfiguration = null;
        }

        /// <inheritdoc/>
        public override void Configure(UltravioletContext uv, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<UIViewFactory>((uv, uiPanel, uiPanelDefinition, vmfactory) => ImGuiView.Create(uv, uiPanel, uiPanelDefinition, vmfactory));
            base.Configure(uv, factory);
        }

        /// <inheritdoc/>
        public override void Initialize(UltravioletContext uv, UltravioletFactory factory)
        {
            base.Initialize(uv, factory);
        }
    }
}
