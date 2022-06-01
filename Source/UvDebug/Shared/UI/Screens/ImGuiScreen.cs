#if IMGUI
using System;
using Ultraviolet;
using Ultraviolet.Content;
using Ultraviolet.ImGuiViewProvider;
using Ultraviolet.ImGuiViewProvider.Bindings;
using Ultraviolet.UI;

namespace UvDebug.UI.Screens
{
    /// <summary>
    /// Represents a screen for testing ImGui.
    /// </summary>
    public class ImGuiScreen : GameScreenBase, IImGuiPanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImGuiScreen"/> class.
        /// </summary>
        /// <param name="globalContent">The content manager with which to load globally-available assets.</param>
        /// <param name="uiScreenService">The screen service which created this screen.</param>
        public ImGuiScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/ImGuiScreen", "ImGuiScreen", globalContent, uiScreenService)
        {

        }

        /// <inheritdoc/>
        void IImGuiPanel.ImGuiRegisterResources(ImGuiView view)
        {
            view.Fonts.RegisterDefault();
            myFont = view.Fonts.RegisterFromAssetTTF(GlobalContent, "Fonts/Inconsolata-Regular", 16f);
            testTextureID = view.Textures.Register(GlobalContent, "Textures/Logo");
        }

        private ImFontPtr myFont;
        private Int32 testTextureID;

        /// <inheritdoc/>
        void IImGuiPanel.ImGuiUpdate(UltravioletTime time)
        {
            ImGui.PushFont(myFont);

            if (ImGui.Begin("Hello, world!", ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGui.Text("This is some text.");
                if (ImGui.Button("Click Me!"))
                {
                    Console.WriteLine("!!!");
                }
                ImGui.Checkbox("Some checkbox", ref checkbox);
                ImGui.InputText("Some text", buf, (UInt32)buf.Length);
                ImGui.Image(new IntPtr(testTextureID), new System.Numerics.Vector2(256, 256), new Vector2(0, 0), new Vector2(1, 1));
            }
            ImGui.End();

            if (ImGui.Begin("Goodbye, world!", ImGuiWindowFlags.AlwaysAutoResize))
            {
                var v = 0;
                ImGui.SliderInt("Slider", ref v, 0, 100, "what");
            }
            ImGui.End();

            ImGui.PopFont();
        }
        private Byte[] buf = new byte[256];
        private Boolean checkbox;

        /// <inheritdoc/>
        void IImGuiPanel.ImGuiDraw(UltravioletTime time)
        {

        }

        /// <inheritdoc/>
        protected override Object CreateViewModel(UIView view) => new ImGuiViewModel(this);
    }
}
#endif