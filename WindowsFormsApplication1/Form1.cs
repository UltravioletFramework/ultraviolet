using System;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.OpenGL;
using TwistedLogik.Ultraviolet.WindowsForms;
using WindowsFormsApplication1.Assets;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.Graphics;

namespace WindowsFormsApplication1
{
    public partial class Form1 : UltravioletForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = testObject;
        }

        protected override UltravioletContext OnCreatingUltravioletContext(out UltravioletTickMode tickmode)
        {
            tickmode = UltravioletTickMode.Idle;

            var configuration = new OpenGLUltravioletConfiguration() { Headless = true };
            return new OpenGLUltravioletContext(this, configuration);
        }

        protected override void OnLoadingContent()
        {
            content = ContentManager.Create("Content");

            var contentManifestFiles = this.content.GetAssetFilePathsInDirectory("Manifests");
            Ultraviolet.GetContent().Manifests.Load(contentManifestFiles);

            Ultraviolet.GetContent().Manifests["Global"]["Fonts"].PopulateAssetLibrary(typeof(GlobalFontID));

            spriteBatch = SpriteBatch.Create();

            textRenderer = new TextRenderer();

            base.OnLoadingContent();
        }

        protected override void OnDrawing()
        {

            base.OnDrawing();
        }

        private ContentManager content;

        private TestObject testObject = new TestObject();

        private SpriteBatch spriteBatch;
        private TextRenderer textRenderer;

        private void ultravioletPanel1_Drawing(object sender, EventArgs e)
        {
            spriteBatch.Begin();

            if (testObject.Font.IsValid)
            {
                var font = content.Load<SpriteFont>(testObject.Font);
                var window = Ultraviolet.GetPlatform().Windows.GetCurrent();
                var settings = new TextLayoutSettings(font, window.ClientSize.Width, window.ClientSize.Height, TextFlags.AlignCenter | TextFlags.AlignMiddle);
                textRenderer.Draw(spriteBatch, "Hello, world!", Vector2.Zero, Color.White, settings);
            }

            spriteBatch.End();
        }
    }
}
