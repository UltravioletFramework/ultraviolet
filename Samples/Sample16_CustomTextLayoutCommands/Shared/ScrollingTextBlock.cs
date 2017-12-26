using System;
using Ultraviolet.Core;
using Ultraviolet;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace UltravioletSample.Sample16_CustomTextLayoutCommands
{
    /// <summary>
    /// Represents a block of text which is revealed character-by-character as time passes.
    /// </summary>
    /// <remarks>The behavior of a scrolling text block can be controlled with two custom layout commands:
    /// <list type="bullet">
    ///     <item>
    ///         <term>|delay:n|</term>
    ///         <description>Causes the text to stop scrolling for n milliseconds, or 100 milliseconds if n is not specified.</description>
    ///     </item>
    ///     <item>
    ///         <term>|speed:n|</term>
    ///         <description>Changes the scroll speed to n characters per second, or to the default if n is not specified.</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public sealed class ScrollingTextBlock
    {
        /// <summary>
        /// Initializes the <see cref="ScrollingTextBlock"/> type.
        /// </summary>
        static ScrollingTextBlock()
        {
            DelayCommandID = TextParser.RegisterCustomCommand("delay");
            SpeedCommandID = TextParser.RegisterCustomCommand("speed");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollingTextBlock"/> class.
        /// </summary>
        /// <param name="textRenderer">The text renderer which will be used to draw the scrolling text block's text.</param>
        /// <param name="font">The sprite font which will be used to draw the scrolling text block's text.</param>
        /// <param name="text">The string which is displayed by the scrolling text block.</param>
        /// <param name="width">The width of the scrolling text block's layout area in pixels,
        /// or <see langword="null"/> to give the layout area an unconstrained width.</param>
        /// <param name="height">The height of the scrolling text block's layout area in pixels,
        /// or <see langword="null"/> to give the layout area an unconstrained height.</param>
        public ScrollingTextBlock(TextRenderer textRenderer, SpriteFont font, String text, Int32? width, Int32? height)
        {
            Contract.Require(textRenderer, nameof(textRenderer));
            Contract.Require(font, nameof(font));
            Contract.Require(text, nameof(text));

            this.TextRenderer = textRenderer;
            this.Font = font;

            this.Width = width;
            this.Height = height;

            this.textParserTokens = new TextParserTokenStream();
            this.textLayoutCommands = new TextLayoutCommandStream();

            this.Text = text;
            this.TextRenderer.Parse(text, textParserTokens);
            this.TextRenderer.CalculateLayout(Text, textLayoutCommands, 
                new TextLayoutSettings(Font, width, height, TextFlags.Standard));

            Reset();
        }

        /// <summary>
        /// Changes the text block's text.
        /// </summary>
        /// <param name="text">The string which is displayed by the scrolling text block.</param>
        public void ChangeText(String text)
        {
            Contract.Require(text, nameof(text));

            this.Text = text;
            this.TextRenderer.Parse(text, textParserTokens);
            this.TextRenderer.CalculateLayout(Text, textLayoutCommands,
                new TextLayoutSettings(Font, Width, Height, TextFlags.Standard));

            Reset();
        }

        /// <summary>
        /// Resizes the scrolling text block's layout area.
        /// </summary>
        /// <param name="width">The width of the scrolling text block's layout area in pixels,
        /// or <see langword="null"/> to give the layout area an unconstrained width.</param>
        /// <param name="height">The height of the scrolling text block's layout area in pixels,
        /// or <see langword="null"/> to give the layout area an unconstrained height.</param>
        public void ChangeSize(Int32? width, Int32? height)
        {
            if (Width == width && Height == height)
                return;

            this.Width = width;
            this.Height = height;

            this.TextRenderer.CalculateLayout(Text, textLayoutCommands, 
                new TextLayoutSettings(Font, width, height, TextFlags.Standard));
        }

        /// <summary>
        /// Resets the scrolling text block to its initial state.
        /// </summary>
        public void Reset()
        {
            charsVisible = 0.0;
            charsPerSecond = DefaultSpeed;
        }

        /// <summary>
        /// Updates the scrolling text block's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            if (ProcessDelay(time))
                return;

            var fullCharsVisible = (Int32)charsVisible;

            charsVisible = Math.Min(Text.Length, charsVisible + (time.ElapsedTime.TotalSeconds * charsPerSecond));

            var fullCharsRevealed = (Int32)charsVisible - fullCharsVisible;
            if (fullCharsRevealed == 0)
                return;

            textLayoutCommands.GetCustomCommands(fullCharsVisible, fullCharsRevealed, this, (state, position, command) =>
            {
                if (command.ID == DelayCommandID)
                    return ((ScrollingTextBlock)state).HandleDelayCommand(command);

                if (command.ID == SpeedCommandID)
                    return ((ScrollingTextBlock)state).HandleSpeedCommand(command);

                return true;
            });
        }

        /// <summary>
        /// Draws the scrolling text block.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the text block.</param>
        /// <param name="position">The position at which to draw the text block.</param>
        /// <param name="defaultColor">The default color with which to draw the text block.</param>
        public void Draw(UltravioletTime time, SpriteBatch spriteBatch, Vector2 position, Color defaultColor)
        {
            TextRenderer.Draw(spriteBatch, textLayoutCommands, position, defaultColor, 0, (Int32)charsVisible);
        }

        /// <summary>
        /// Gets or sets the text renderer which is used to render the scrolling text block's text.
        /// </summary>
        public TextRenderer TextRenderer { get; }

        /// <summary>
        /// Gets or sets the sprite font which is used to render the scrolling text block's text.
        /// </summary>
        public SpriteFont Font { get; private set; }

        /// <summary>
        /// Gets or sets the string which is displayed by the scrolling text block.
        /// </summary>
        public String Text { get; private set; }

        /// <summary>
        /// Gets or sets the width of the text block's layout area in pixels.
        /// </summary>
        public Int32? Width { get; private set; }

        /// <summary>
        /// Gets or sets the height of the text block's layout area in pixels.
        /// </summary>
        public Int32? Height { get; private set; }

        /// <summary>
        /// Updates the current delay, if there is one, and returns a value indicating whether
        /// the block should be prevented from scrolling.
        /// </summary>
        private Boolean ProcessDelay(UltravioletTime time)
        {
            if (delay > 0)
                delay = Math.Max(0, delay - time.ElapsedTime.TotalMilliseconds);

            return delay > 0;
        }

        /// <summary>
        /// Handles a |delay| command.
        /// </summary>
        private Boolean HandleDelayCommand(TextLayoutCustomCommand cmd)
        {
            delay = (cmd.Value == 0) ? DefaultDelay : cmd.Value;
            charsVisible = (Int32)charsVisible;

            return false;
        }

        /// <summary>
        /// Handles a |speed| command.
        /// </summary>
        private Boolean HandleSpeedCommand(TextLayoutCustomCommand cmd)
        {
            charsPerSecond = (cmd.Value == 0) ? DefaultSpeed : cmd.Value;
            charsVisible = (Int32)charsVisible;

            return false;
        }

        // Custom layout command identifiers.
        private static readonly Byte DelayCommandID;
        private static readonly Byte SpeedCommandID;

        // Default values for custom commands.
        const Double DefaultDelay = 100.0;
        const Double DefaultSpeed = 25.0;

        // Text block state values.
        private Double delay;
        private Double charsVisible;
        private Double charsPerSecond;
        private TextParserTokenStream textParserTokens;
        private TextLayoutCommandStream textLayoutCommands;
    }
}
