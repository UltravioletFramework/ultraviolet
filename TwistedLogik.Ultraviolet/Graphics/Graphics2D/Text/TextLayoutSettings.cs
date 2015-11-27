using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the settings used to specify the behavior of a <see cref="TextLayoutEngine"/>.
    /// </summary>
    public struct TextLayoutSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSettings"/> structure.
        /// </summary>
        /// <param name="font">The default font.</param>
        /// <param name="width">The width of the layout area.</param>
        /// <param name="height">The height of the layout area.</param>
        /// <param name="flags">A set of flags that specify how to render and align the text.</param>
        /// <param name="options">A set of options which can be used to modify the behavior of the layout engine.</param>
        public TextLayoutSettings(SpriteFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options)
            : this(font, width, height, flags, SpriteFontStyle.Regular, options)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSettings"/> structure.
        /// </summary>
        /// <param name="font">The default font.</param>
        /// <param name="width">The width of the layout area.</param>
        /// <param name="height">The height of the layout area.</param>
        /// <param name="flags">A set of flags that specify how to render and align the text.</param>
        /// <param name="style">The initial font style.</param>
        /// <param name="options">A set of options which can be used to modify the behavior of the layout engine.</param>
        public TextLayoutSettings(SpriteFont font, Int32? width, Int32? height, TextFlags flags, SpriteFontStyle style = SpriteFontStyle.Regular, TextLayoutOptions options = TextLayoutOptions.None)
        {
            this.font    = font;
            this.width   = width;
            this.height  = height;
            this.flags   = (flags == 0) ? TextFlags.Standard : flags;
            this.style   = style;
            this.options = options;
        }
        
        /// <summary>
        /// Gets the default font.
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }

        /// <summary>
        /// Gets the width of the layout area.
        /// </summary>
        public Int32? Width
        {
            get { return width; }
        }

        /// <summary>
        /// Gets the height of the layout area.
        /// </summary>
        public Int32? Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets the set of flags used to specify how to render and align the text.
        /// </summary>
        public TextFlags Flags
        {
            get { return flags; }
        }

        /// <summary>
        /// Gets the initial font style.
        /// </summary>
        public SpriteFontStyle Style
        {
            get { return style; }
        }

        /// <summary>
        /// Gets the layout options.
        /// </summary>
        public TextLayoutOptions Options
        {
            get { return options; }
        }

        // Property values.
        private readonly SpriteFont font;
        private readonly Int32? width;
        private readonly Int32? height;
        private readonly TextFlags flags;
        private readonly SpriteFontStyle style;
        private readonly TextLayoutOptions options;
    }
}
