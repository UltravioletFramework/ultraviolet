using System;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.ImGuiViewProvider.Bindings;
using Ultraviolet.Input;
using Ultraviolet.UI;

namespace Ultraviolet.ImGuiViewProvider
{
    /// <summary>
    /// Represents a container for Dear ImGui UI elements.
    /// </summary>
    public partial class ImGuiView : UIView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImGuiView"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="panel">The panel that is creating the view.</param>
        /// <param name="viewModelType">The view's associated model type.</param>
        public ImGuiView(UltravioletContext uv, UIPanel panel, Type viewModelType) 
            : base(uv, panel, viewModelType)
        {
            imGuiContext = ImGui.CreateContext();
            imGuiGeometry = new ImGuiGeometryBuffer(uv, this);

            ImGui.SetCurrentContext(imGuiContext);

            this.Textures = new ImGuiTextureRegistry();
            this.Fonts = new ImGuiFontRegistry(imGuiContext);

            (panel as IImGuiPanel)?.ImGuiRegisterResources(this);

            this.Fonts.Lock();

            InitInput();
            InitFontTextureAtlas();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ImGuiView"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiPanel">The <see cref="UIPanel"/> that is creating the view.</param>
        /// <param name="uiPanelDefinition">The <see cref="UIPanelDefinition"/> that defines the view's containing panel.</param>
        /// <param name="vmfactory">A view model factory which is used to create the view's initial view model, or <see langword="null"/> to skip view model creation.</param>
        /// <returns>The <see cref="ImGuiView"/> that was created.</returns>
        public static ImGuiView Create(UltravioletContext uv, UIPanel uiPanel, UIPanelDefinition uiPanelDefinition, UIViewModelFactory vmfactory)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(uiPanel, nameof(uiPanel));
            Contract.Require(uiPanelDefinition, nameof(uiPanelDefinition));

            var viewElement = uiPanelDefinition.ViewElement;
            if (viewElement == null)
                return null;

            var viewModelType = default(Type);
            var viewModelTypeName = (String)viewElement.Attribute("ViewModelType");
            if (viewModelTypeName != null)
            {
                viewModelType = Type.GetType(viewModelTypeName, false) ?? 
                    throw new InvalidOperationException(ImGuiStrings.ViewModelTypeNotFound.Format(viewModelTypeName));
            }

            return new ImGuiView(uv, uiPanel, viewModelType);
        }

        /// <inheritdoc/>
        public override void Draw(UltravioletTime time, SpriteBatch spriteBatch, Single opacity = 1)
        {
            if (imGuiContext != IntPtr.Zero)
            {
                var spriteBatchState = spriteBatch.GetCurrentState();
                spriteBatch.End();

                ImGui.SetCurrentContext(imGuiContext);
                ImGui.Render();

                var drawDataPtr = ImGui.GetDrawData();
                imGuiGeometry.Draw(ref drawDataPtr);

                (Panel as IImGuiPanel)?.ImGuiDraw(time);

                spriteBatch.Begin(spriteBatchState);
            }
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            if (imGuiContext != IntPtr.Zero)
            {
                ImGui.SetCurrentContext(imGuiContext);

                var imGuiIO = ImGui.GetIO();
                imGuiIO.DeltaTime = (Single)time.ElapsedTime.TotalSeconds;
                imGuiIO.DisplaySize = new System.Numerics.Vector2(Width, Height);
                imGuiIO.DisplayFramebufferScale = new System.Numerics.Vector2(1, 1);

                UpdateInput();

                ImGui.NewFrame();

                (Panel as IImGuiPanel)?.ImGuiUpdate(time);

                ImGui.EndFrame();
            }
        }

        /// <summary>
        /// Gets the view's font registry.
        /// </summary>
        public ImGuiFontRegistry Fonts { get; }

        /// <summary>
        /// Gets the view's texture registry.
        /// </summary>
        public ImGuiTextureRegistry Textures { get; }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (imGuiContext != IntPtr.Zero)
            {
                ImGui.DestroyContext(imGuiContext);
                imGuiContext = IntPtr.Zero;
            }

            imGuiGeometry?.Dispose();
            imGuiGeometry = null;

            Textures.Unregister(fontAtlasID);
            fontAtlas?.Dispose();
            fontAtlas = null;

            base.Dispose(disposing);
        }

        /// <inheritdoc/>
        protected override void OnOpening()
        {

        }

        /// <inheritdoc/>
        protected override void OnOpened()
        {

        }

        /// <inheritdoc/>
        protected override void OnClosing()
        {

        }

        /// <inheritdoc/>
        protected override void OnClosed()
        {

        }

        /// <summary>
        /// Gets a value indicating whether the Alt key is down.
        /// </summary>
        private static Boolean IsAltDown(KeyboardDevice device) => device.IsButtonDown(Scancode.LeftAlt) || device.IsButtonDown(Scancode.RightAlt);

        /// <summary>
        /// Gets a value indicating whether the Ctrl key is down.
        /// </summary>
        private static Boolean IsCtrlDown(KeyboardDevice device) => device.IsButtonDown(Scancode.LeftControl) || device.IsButtonDown(Scancode.RightControl);

        /// <summary>
        /// Gets a value indicating whether the Shift key is down.
        /// </summary>
        private static Boolean IsShiftDown(KeyboardDevice device) => device.IsButtonDown(Scancode.LeftShift) || device.IsButtonDown(Scancode.RightShift);

        /// <summary>
        /// Gets a value indicating whether the Gui key is down.
        /// </summary>
        private static Boolean IsSuperDown(KeyboardDevice device) => device.IsButtonDown(Scancode.LeftGui) || device.IsButtonDown(Scancode.RightGui);

        /// <summary>
        /// Handles the <see cref="KeyboardDevice.TextInput"/> event.
        /// </summary>
        private void Keyboard_TextInput(Platform.IUltravioletWindow window, KeyboardDevice device)
        {
            device.GetTextInput(textBuffer, true);
        }

        /// <summary>
        /// Initializes input for the current ImGui context.
        /// </summary>
        private void InitInput()
        {
            var io = ImGui.GetIO();
            for (int i = 0; i < (Int32)ImGuiKey.COUNT; i++)
            {
                io.KeyMap[i] = (Int32)ScancodeMap[i];
            }

            var input = Ultraviolet.GetInput();
            var keyboard = input.GetKeyboard();
            if (keyboard != null)
            {
                io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
                keyboard.TextInput += this.Keyboard_TextInput;
            }
        }

        /// <summary>
        /// Releases resources associated with the input devices.
        /// </summary>
        private void QuitInput()
        {
            var input = Ultraviolet.GetInput();
            var keyboard = input.GetKeyboard();
            if (keyboard != null)
            {
                keyboard.TextInput -= this.Keyboard_TextInput;
            }
        }

        /// <summary>
        /// Initializes the font texture atlas for the current ImGui context.
        /// </summary>
        private void InitFontTextureAtlas()
        {
            unsafe
            {
                var io = ImGui.GetIO();

                io.Fonts.GetTexDataAsRGBA32(out var textureData,
                    out var textureWidth, out var textureHeight, out var textureBytesPerPixel);

                var srgb = Ultraviolet.GetGraphics().Capabilities.SrgbEncodingEnabled;

                var surfaceOptions = srgb ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;
                var surface = Surface2D.Create(textureWidth, textureHeight, surfaceOptions);
                surface.SetRawData((IntPtr)textureData, 0, 0, textureWidth * textureHeight);
                surface.Flip(SurfaceFlipDirection.Vertical);

                var textureOptions = TextureOptions.ImmutableStorage | (srgb ? TextureOptions.SrgbColor : TextureOptions.LinearColor);
                var texture = Texture2D.CreateTexture(textureWidth, textureHeight, textureOptions);
                texture.SetData(surface);

                fontAtlasID = Textures.Register(texture);

                io.Fonts.SetTexID(new IntPtr(fontAtlasID));
                io.Fonts.ClearTexData();
            }
        }

        /// <summary>
        /// Updates user input states for the current context.
        /// </summary>
        private void UpdateInput()
        {
            var io = ImGui.GetIO();

            var keyboard = Ultraviolet.GetInput().GetKeyboard();
            if (keyboard != null)
            {
                io.KeyAlt = new Bool8(IsAltDown(keyboard));
                io.KeyCtrl = new Bool8(IsCtrlDown(keyboard));
                io.KeyShift = new Bool8(IsShiftDown(keyboard));
                io.KeySuper = new Bool8(IsSuperDown(keyboard));

                for (int i = 0; i < (Int32)ImGuiKey.COUNT; i++)
                    io.KeysDown[(Int32)ScancodeMap[i]] = new Bool8(keyboard.IsButtonDown(ScancodeMap[i]));

                for (int i = 0; i < textBuffer.Length; i++)
                    io.AddInputCharacter(textBuffer[i]);

                textBuffer.Clear();
            }

            var mouse = Ultraviolet.GetInput().GetMouse();
            if (mouse != null)
            {
                io.MousePos = (Vector2)mouse.Position;
                io.MouseDown[0] = new Bool8(mouse.IsButtonDown(MouseButton.Left));
                io.MouseDown[1] = new Bool8(mouse.IsButtonDown(MouseButton.Right));
                io.MouseDown[2] = new Bool8(mouse.IsButtonDown(MouseButton.Middle));
            }
        }

        // State values.
        private IntPtr imGuiContext;
        private ImGuiGeometryBuffer imGuiGeometry;

        // Font atlas.
        private Texture2D fontAtlas;
        private Int32 fontAtlasID;

        // Text input buffer.
        private readonly StringBuilder textBuffer = new StringBuilder(16);
    }
}
