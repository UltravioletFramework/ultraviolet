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
        public TextLayoutSettings(SpriteFont font, Int32? width, Int32? height, TextFlags flags)
            : this(font, width, height, flags, TextLayoutOptions.None, SpriteFontStyle.Regular)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSettings"/> structure.
        /// </summary>
        /// <param name="font">The default font.</param>
        /// <param name="width">The width of the layout area.</param>
        /// <param name="height">The height of the layout area.</param>
        /// <param name="flags">A set of flags that specify how to render and align the text.</param>
        /// <param name="fontStyle">The initial font style.</param>
        public TextLayoutSettings(SpriteFont font, Int32? width, Int32? height, TextFlags flags, SpriteFontStyle fontStyle)
            : this(font, width, height, flags, TextLayoutOptions.None, fontStyle, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSettings"/> structure.
        /// </summary>
        /// <param name="font">The default font.</param>
        /// <param name="width">The width of the layout area.</param>
        /// <param name="height">The height of the layout area.</param>
        /// <param name="flags">A set of flags that specify how to render and align the text.</param>
        /// <param name="initialLayoutStyle">The name of the initial layout style, or <c>null</c> to use no initial layout style.</param>
        public TextLayoutSettings(SpriteFont font, Int32? width, Int32? height, TextFlags flags, String initialLayoutStyle)
            : this(font, width, height, flags, TextLayoutOptions.None, SpriteFontStyle.Regular, initialLayoutStyle)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSettings"/> structure.
        /// </summary>
        /// <param name="font">The default font.</param>
        /// <param name="width">The width of the layout area.</param>
        /// <param name="height">The height of the layout area.</param>
        /// <param name="flags">A set of flags that specify how to render and align the text.</param>
        /// <param name="fontStyle">The initial font style.</param>
        /// <param name="initialLayoutStyle">The name of the initial layout style, or <c>null</c> to use no initial layout style.</param>
        public TextLayoutSettings(SpriteFont font, Int32? width, Int32? height, TextFlags flags, SpriteFontStyle fontStyle, String initialLayoutStyle)
            : this(font, width, height, flags, TextLayoutOptions.None, fontStyle, initialLayoutStyle)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSettings"/> structure.
        /// </summary>
        /// <param name="font">The default font.</param>
        /// <param name="width">The width of the layout area.</param>
        /// <param name="height">The height of the layout area.</param>
        /// <param name="flags">A set of flags that specify how to render and align the text.</param>
        /// <param name="options">A set of options which can be used to modify the behavior of the layout engine.</param>
        public TextLayoutSettings(SpriteFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options)
            : this(font, width, height, flags, options, SpriteFontStyle.Regular)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSettings"/> structure.
        /// </summary>
        /// <param name="font">The default font.</param>
        /// <param name="width">The width of the layout area.</param>
        /// <param name="height">The height of the layout area.</param>
        /// <param name="flags">A set of flags that specify how to render and align the text.</param>
        /// <param name="options">A set of options which can be used to modify the behavior of the layout engine.</param>
        /// <param name="fontStyle">The initial font style.</param>
        public TextLayoutSettings(SpriteFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options, SpriteFontStyle fontStyle)
            : this(font, width, height, flags, options, fontStyle, null)
        {

        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSettings"/> structure.
        /// </summary>
        /// <param name="font">The default font.</param>
        /// <param name="width">The width of the layout area.</param>
        /// <param name="height">The height of the layout area.</param>
        /// <param name="flags">A set of flags that specify how to render and align the text.</param>
        /// <param name="options">A set of options which can be used to modify the behavior of the layout engine.</param>
        /// <param name="initialLayoutStyle">The name of the initial layout style, or <c>null</c> to use no initial layout style.</param>
        public TextLayoutSettings(SpriteFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options, String initialLayoutStyle)
            : this(font, width, height, flags, options, SpriteFontStyle.Regular, initialLayoutStyle)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSettings"/> structure.
        /// </summary>
        /// <param name="font">The default font.</param>
        /// <param name="width">The width of the layout area.</param>
        /// <param name="height">The height of the layout area.</param>
        /// <param name="flags">A set of flags that specify how to render and align the text.</param>
        /// <param name="options">A set of options which can be used to modify the behavior of the layout engine.</param>
        /// <param name="fontStyle">The initial font style.</param>
        /// <param name="initialLayoutStyle">The name of the initial layout style, or <c>null</c> to use no initial layout style.</param>
        public TextLayoutSettings(SpriteFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options, SpriteFontStyle fontStyle, String initialLayoutStyle)
        {
            this.font = font;
            this.width = width;
            this.height = height;
            this.flags = (flags == 0) ? TextFlags.Standard : flags;
            this.style = fontStyle;
            this.options = options;
            this.initialLayoutStyle = initialLayoutStyle;
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

        /// <summary>
        /// Gets the name of the text's initial layout style.
        /// </summary>
        public String InitialLayoutStyle
        {
            get { return initialLayoutStyle; }
        }

        // Property values.
        private readonly SpriteFont font;
        private readonly Int32? width;
        private readonly Int32? height;
        private readonly TextFlags flags;
        private readonly TextLayoutOptions options;
        private readonly SpriteFontStyle style;
        private readonly String initialLayoutStyle;
    }
}
