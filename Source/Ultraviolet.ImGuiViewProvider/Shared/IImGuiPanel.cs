namespace Ultraviolet.ImGuiViewProvider
{
    /// <summary>
    /// Represents a panel which contains an ImGui view.
    /// </summary>
    public interface IImGuiPanel
    {
        /// <summary>
        /// Called when ImGui resources should be registered.
        /// </summary>
        /// <param name="view">The view for which resources should be registered.</param>
        void ImGuiRegisterResources(ImGuiView view);

        /// <summary>
        /// Called when the ImGui interface is updated.
        /// </summary>
        /// <param name="time">Time elapsed since the last update call.</param>
        void ImGuiUpdate(UltravioletTime time);

        /// <summary>
        /// Called when the ImGui interface is drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last draw call.</param>
        void ImGuiDraw(UltravioletTime time);      
    }
}
