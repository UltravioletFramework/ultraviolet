using System.Text;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Contains common resources used by UI elements.
    /// </summary>
    public static class UIElementResources
    {
        /// <summary>
        /// Gets the string formatter used by UI elements.
        /// </summary>
        public static StringFormatter StringFormatter
        {
            get { return stringFormatter; }
        }

        /// <summary>
        /// Gets the string buffer used by UI elements.
        /// </summary>
        public static StringBuilder StringBuffer
        {
            get { return stringBuffer; }
        }

        /// <summary>
        /// Gets the text renderer used by UI elements.
        /// </summary>
        public static TextRenderer TextRenderer
        {
            get { return textRenderer; }
        }

        /// <summary>
        /// Gets the blank texture used by UI elements.
        /// </summary>
        public static Texture2D BlankTexture
        {
            get { return blankTexture.Value; }
        }

        // Property values.
        private static readonly StringFormatter stringFormatter = new StringFormatter();
        private static readonly StringBuilder stringBuffer = new StringBuilder();
        private static readonly TextRenderer textRenderer = new TextRenderer();
        private static readonly UltravioletSingleton<Texture2D> blankTexture = new UltravioletSingleton<Texture2D>((uv) =>
        {
            var texture = Texture2D.Create(1, 1);
            texture.SetData(new[] { Color.White });
            return texture;
        });
    }
}
