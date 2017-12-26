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

        public void Load(ContentManager content)
        {
            Contract.Require(content, nameof(content));

            this.SpriteBatch = SpriteBatch.Create();
            this.SpriteFont = content.Load<SpriteFont>(FontPath ?? "Fonts/Garamond");

            this.TextIcons = content.Load<Sprite>(IconPath ?? "Sprites/InterfaceIcons");

            this.TextParserResult = new TextParserTokenStream();
            this.TextParser = new TextParser();
            this.TextParser.Parse(this.Text, this.TextParserResult, this.TextParserOptions);

            this.TextLayoutResult = new TextLayoutCommandStream();
            this.TextLayoutEngine = new TextLayoutEngine();
            this.TextLayoutEngine.RegisterIcon("test", this.TextIcons["test"]);

            this.TextRenderer = new TextRenderer();
            this.TextRenderer.RegisterIcon("test", this.TextIcons["test"]);

            this.BlankTexture = Texture2D.Create(1, 1);
            this.BlankTexture.SetData(new[] { Color.White });
        }

        public SpriteBatch SpriteBatch { get; private set; }
        public SpriteFont SpriteFont { get; private set; }
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
