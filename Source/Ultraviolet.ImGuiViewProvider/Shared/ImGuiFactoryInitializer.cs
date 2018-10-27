using Ultraviolet.UI;

namespace Ultraviolet.ImGuiViewProvider
{
    /// <summary>
    /// Initializes factory methods for the Ultraviolet ImGui View Provider Plugin.
    /// </summary>
    public class ImGuiFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<UIViewFactory>((uv, uiPanel, uiPanelDefinition, vmfactory) => ImGuiView.Create(uv, uiPanel, uiPanelDefinition, vmfactory));
        }
    }
}
