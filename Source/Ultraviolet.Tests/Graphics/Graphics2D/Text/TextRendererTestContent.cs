using System;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.Tests.Graphics.Graphics2D.Text
{
    public class TextRendererTestContent
    {
        public TextRendererTestContent(String text, TextParserOptions parserOptions = TextParserOptions.None)
        {
            Contract.RequireNotEmpty(text, nameof(text));

            this.Text = text;
            this.TextParserOptions = parserOptions;
        }

        public void Load(ContentManager content, FontKind kind)
        {
            switch (kind)
            {
                case FontKind.SpriteFont:
                    Load(content);
                    break;

                case FontKind.FreeType2:
                    LoadFreeType(content);
                    break;

                default:
                    throw new ArgumentException(nameof(kind));
            }
        }

        public void Load(ContentManager content)
        {
            Contract.Require(content, nameof(content));

            this.SpriteBatch = SpriteBatch.Create();
            this.Font = content.Load<UltravioletFont>(FontPath ?? "Fonts/Garamond");
            this.FontAlternate = content.Load<UltravioletFont>(FontPath ?? "Fonts/SegoeUI");

            this.TextIcons = content.Load<Sprite>(IconPath ?? "Sprites/InterfaceIcons");

            this.TextParserResult = new TextParserTokenStream();
            this.TextParser = new TextParser();
            this.TextParser.Parse(this.Text, this.TextParserResult, this.TextParserOptions);

            this.TextLayoutResult = new TextLayoutCommandStream();
            this.TextLayoutEngine = new TextLayoutEngine();
            this.TextLayoutEngine.RegisterIcon("test", this.TextIcons["test"]);

            this.TextRenderer = new TextRenderer();
            this.TextRenderer.LayoutEngine.RegisterIcon("test", this.TextIcons["test"]);
            this.TextRenderer.LayoutEngine.RegisterFont("alt", FontAlternate);

            this.BlankTexture = Texture2D.CreateTexture(1, 1);
            this.BlankTexture.SetData(new[] { Color.White });
        }

        public void LoadFreeType(ContentManager content)
        {
            Contract.Require(content, nameof(content));

            this.SpriteBatch = SpriteBatch.Create();
            this.Font = content.Load<UltravioletFont>(FontPath ?? "Fonts/FiraSans");
            this.FontAlternate = content.Load<UltravioletFont>(FontPath ?? "Fonts/SegoeUI");

            this.TextIcons = content.Load<Sprite>(IconPath ?? "Sprites/InterfaceIcons");

            this.TextParserResult = new TextParserTokenStream();
            this.TextParser = new TextParser();
            this.TextParser.Parse(this.Text, this.TextParserResult, this.TextParserOptions);

            this.TextLayoutResult = new TextLayoutCommandStream();
            this.TextLayoutEngine = new TextLayoutEngine();
            this.TextLayoutEngine.RegisterTextShaper(TextShaper.Create());
            this.TextLayoutEngine.RegisterIcon("test", this.TextIcons["test"], descender: -3);

            this.TextRenderer = new TextRenderer();
            this.TextRenderer.LayoutEngine.RegisterTextShaper(TextShaper.Create());
            this.TextRenderer.LayoutEngine.RegisterIcon("test", this.TextIcons["test"]);
            this.TextRenderer.LayoutEngine.RegisterFont("alt", FontAlternate);

            this.BlankTexture = Texture2D.CreateTexture(1, 1);
            this.BlankTexture.SetData(new[] { Color.White });
        }

        public SpriteBatch SpriteBatch { get; private set; }
        public UltravioletFont Font { get; private set; }
        public UltravioletFont FontAlternate { get; private set; }
        public String FontPath { get; set; }
        public String IconPath { get; set; }
        public String Text { get; private set; }
        public Sprite TextIcons { get; private set; }
        public TextParserTokenStream TextParserResult { get; private set; }
        public TextParser TextParser { get; private set; }
        public TextParserOptions TextParserOptions { get; private set; }
        public TextLayoutCommandStream TextLayoutResult { get; private set; }
        public TextLayoutEngine TextLayoutEngine { get; private set; }
        public TextRenderer TextRenderer { get; private set; }
        public Texture2D BlankTexture { get; private set; }
    }
}
