using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the settings used to specify the behavior of the layout anager.
    /// </summary>
    public struct TextLayoutSettings
    {
        /// <summary>
        /// Initializes a new instance of the FormattedTextLayoutSettings structure.
        /// </summary>
        /// <param name="font">The default font.</param>
        /// <param name="width">The width of the layout area.</param>
        /// <param name="height">The height of the layout area.</param>
        /// <param name="flags">A set of flags that specify how to render and align the text.</param>
        public TextLayoutSettings(SpriteFont font, Int32? width, Int32? height, TextFlags flags)
        {
            this.font = font;
            this.width = width;
            this.height = height;
            this.flags = (flags == 0) ? TextFlags.Standard : flags;
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

        // Property values.
        private readonly SpriteFont font;
        private readonly Int32? width;
        private readonly Int32? height;
        private readonly TextFlags flags;
    }
}
