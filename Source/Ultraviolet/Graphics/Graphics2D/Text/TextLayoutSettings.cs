using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
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
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags)
            : this(font, width, height, flags, TextLayoutOptions.None, TextDirection.LeftToRight, TextScript.Unknown, UltravioletFontStyle.Regular, null)
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
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags, UltravioletFontStyle fontStyle)
            : this(font, width, height, flags, TextLayoutOptions.None, TextDirection.LeftToRight, TextScript.Unknown, fontStyle, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSettings"/> structure.
        /// </summary>
        /// <param name="font">The default font.</param>
        /// <param name="width">The width of the layout area.</param>
        /// <param name="height">The height of the layout area.</param>
        /// <param name="flags">A set of flags that specify how to render and align the text.</param>
        /// <param name="initialLayoutStyle">The name of the initial layout style, or <see langword="null"/> to use no initial layout style.</param>
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags, String initialLayoutStyle)
            : this(font, width, height, flags, TextLayoutOptions.None, TextDirection.LeftToRight, TextScript.Unknown, UltravioletFontStyle.Regular, initialLayoutStyle)
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
        /// <param name="initialLayoutStyle">The name of the initial layout style, or <see langword="null"/> to use no initial layout style.</param>
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags, UltravioletFontStyle fontStyle, String initialLayoutStyle)
            : this(font, width, height, flags, TextLayoutOptions.None, TextDirection.LeftToRight, TextScript.Unknown, fontStyle, initialLayoutStyle)
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
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options)
            : this(font, width, height, flags, options, TextDirection.LeftToRight, TextScript.Unknown, UltravioletFontStyle.Regular, null)
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
        /// <param name="direction">A value indicating the direction in which the text should be laid out.</param>
        /// <param name="script">A value specifying which script is used to draw the text.</param>
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options, TextDirection direction, TextScript script)
            : this(font, width, height, flags, options, direction, script, UltravioletFontStyle.Regular, null)
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
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options, UltravioletFontStyle fontStyle)
            : this(font, width, height, flags, options, TextDirection.LeftToRight, TextScript.Unknown, fontStyle, null)
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
        /// <param name="direction">A value indicating the direction in which the text should be laid out.</param>
        /// <param name="script">A value specifying which script is used to draw the text.</param>
        /// <param name="fontStyle">The initial font style.</param>
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options, TextDirection direction, TextScript script, UltravioletFontStyle fontStyle)
            : this(font, width, height, flags, options, direction, script, fontStyle, null)
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
        /// <param name="initialLayoutStyle">The name of the initial layout style, or <see langword="null"/> to use no initial layout style.</param>
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options, String initialLayoutStyle)
            : this(font, width, height, flags, options, TextDirection.LeftToRight, TextScript.Unknown, UltravioletFontStyle.Regular, initialLayoutStyle)
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
        /// <param name="direction">A value indicating the direction in which the text should be laid out.</param>
        /// <param name="script">A value specifying which script is used to draw the text.</param>
        /// <param name="initialLayoutStyle">The name of the initial layout style, or <see langword="null"/> to use no initial layout style.</param>
        /// <param name="language">The ISO 639 name of the text language.</param>
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options, TextDirection direction, TextScript script, String initialLayoutStyle, String language = "en")
            : this(font, width, height, flags, options, direction, script, UltravioletFontStyle.Regular, initialLayoutStyle, language)
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
        /// <param name="initialLayoutStyle">The name of the initial layout style, or <see langword="null"/> to use no initial layout style.</param>
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options, UltravioletFontStyle fontStyle, String initialLayoutStyle)
            : this(font, width, height, flags, options, TextDirection.LeftToRight, TextScript.Unknown, fontStyle, initialLayoutStyle)
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
        /// <param name="direction">A value indicating the direction in which the text should be laid out.</param>
        /// <param name="script">A value specifying which script is used to draw the text.</param>
        /// <param name="fontStyle">The initial font style.</param>
        /// <param name="initialLayoutStyle">The name of the initial layout style, or <see langword="null"/> to use no initial layout style.</param>
        /// <param name="language">The ISO 639 name of the text language.</param>
        public TextLayoutSettings(UltravioletFont font, Int32? width, Int32? height, TextFlags flags, TextLayoutOptions options, TextDirection direction, TextScript script, UltravioletFontStyle fontStyle, String initialLayoutStyle, String language = "en")
        {
            if (direction == TextDirection.TopToBottom || direction == TextDirection.BottomToTop)
                throw new NotSupportedException(UltravioletStrings.UnsupportedTextDirection);

            this.Font = font;
            this.Width = width;
            this.Height = height;
            this.Flags = (flags == 0) ? TextFlags.Standard : flags;
            this.Style = fontStyle;
            this.Direction = direction;
            this.Script = script;
            this.Options = options;
            this.InitialLayoutStyle = initialLayoutStyle;
            this.Language = language;
        }

        /// <summary>
        /// Gets the default font.
        /// </summary>
        public UltravioletFont Font { get; }

        /// <summary>
        /// Gets the width of the layout area.
        /// </summary>
        public Int32? Width { get; }

        /// <summary>
        /// Gets the height of the layout area.
        /// </summary>
        public Int32? Height { get; }

        /// <summary>
        /// Gets the set of flags used to specify how to render and align the text.
        /// </summary>
        public TextFlags Flags { get; }

        /// <summary>
        /// Gets the initial font style.
        /// </summary>
        public UltravioletFontStyle Style { get; }

        /// <summary>
        /// Gets the layout options.
        /// </summary>
        public TextLayoutOptions Options { get; }

        /// <summary>
        /// Gets the direction in which text is laid out.
        /// </summary>
        public TextDirection Direction { get; }

        /// <summary>
        /// Gets the text script.
        /// </summary>
        public TextScript Script { get; }

        /// <summary>
        /// Gets the name of the text's initial layout style.
        /// </summary>
        public String InitialLayoutStyle { get; }

        /// <summary>
        /// Gets the ISO 639 name of the text language.
        /// </summary>
        public String Language { get; }
    }
}
