using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections.Specialized;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a stream of commands produced by the text layout engine.
    /// </summary>
    [SecuritySafeCritical]
    public unsafe class TextLayoutCommandStream
    {
        /// <summary>
        /// Moves the stream to the command with the specified index.
        /// </summary>
        /// <param name="index">The index of the command to which the stream will seek.</param>
        /// <returns>A <see cref="TextLayoutCommandType"/> that represents the type of command at the stream's new position.</returns>
        public TextLayoutCommandType Seek(Int32 index)
        {
            Contract.EnsureRange(index >= 0 && index < Count, "index");

            var data = stream.SeekObject(index);
            return *(TextLayoutCommandType*)data;
        }

        /// <summary>
        /// Registers an icon with the command stream.
        /// </summary>
        /// <param name="name">The name of the icon to register.</param>
        /// <param name="icon">The icon to register under the specified name.</param>
        /// <returns>The index of the specified icon within the command stream's internal registry.</returns>
        public Int32 RegisterIcon(StringSegment name, SpriteAnimation icon)
        {
            Contract.Require(icon, "icon");

            Int32 index;
            if (iconsByName.TryGetValue(name, out index))
                return index;

            index = icons.Count;

            icons.Add(icon);
            iconsByName[name] = index;

            return index;
        }

        /// <summary>
        /// Registers a font with the command stream.
        /// </summary>
        /// <param name="name">The name of the font to register.</param>
        /// <param name="font">The font to register under the specified name.</param>
        /// <returns>The index of the specified font within the command stream's internal registry.</returns>
        public Int32 RegisterFont(StringSegment name, SpriteFontFace font)
        {
            Contract.Require(font, "font");

            Int32 index;
            if (fontsByName.TryGetValue(name, out index))
                return index;

            index = fonts.Count;

            fonts.Add(font);
            fontsByName[name] = index;

            return index;
        }

        /// <summary>
        /// Registers a glyph shader with the command stream.
        /// </summary>
        /// <param name="name">The name of the glyph shader to register.</param>
        /// <param name="glyphShader">The glyph shader to register under the specified name.</param>
        /// <returns>The index of the specified glyph shader within the command stream's internal registry.</returns>
        public Int32 RegisterGlyphShader(StringSegment name, GlyphShader glyphShader)
        {
            Contract.Require(glyphShader, "glyphShader");

            Int32 index;
            if (glyphShadersByName.TryGetValue(name, out index))
                return index;

            index = glyphShaders.Count;

            glyphShaders.Add(glyphShader);
            glyphShadersByName[name] = index;

            return index;
        }

        /// <summary>
        /// Retrieves the registered icon with the specified name.
        /// </summary>
        /// <param name="name">The name of the icon to retrieve.</param>
        /// <returns>The registered icon with the specified name.</returns>
        public SpriteAnimation GetIcon(StringSegment name)
        {
            Int32 index;
            if (!iconsByName.TryGetValue(name, out index))
                return null;

            return icons[index];
        }

        /// <summary>
        /// Retrieves the registered icon at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered icon to retrieve.</param>
        /// <returns>The registered icon at the specified index within the command stream's internal registry.</returns>
        public SpriteAnimation GetIcon(Int32 index)
        {
            return icons[index];
        }

        /// <summary>
        /// Retrieves the registered font with the specified name.
        /// </summary>
        /// <param name="name">The name of the font to retrieve.</param>
        /// <returns>The registered font with the specified name.</returns>
        public SpriteFontFace GetFont(StringSegment name)
        {
            Int32 index;
            if (!fontsByName.TryGetValue(name, out index))
                return null;

            return fonts[index];
        }

        /// <summary>
        /// Retrieves the registered font at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered font to retrieve.</param>
        /// <returns>The registered font at the specified index within the command stream's internal registry.</returns>
        public SpriteFontFace GetFont(Int32 index)
        {
            return fonts[index];
        }

        /// <summary>
        /// Retrieves the registered glyph shader with the specified name.
        /// </summary>
        /// <param name="name">The name of the glyph shader to retrieve.</param>
        /// <returns>The registered glyph shader with the specified name.</returns>
        public GlyphShader GetGlyphShader(StringSegment name)
        {
            Int32 index;
            if (!glyphShadersByName.TryGetValue(name, out index))
                return null;

            return glyphShaders[index];
        }

        /// <summary>
        /// Retrieves the registered glyph shader at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered glyph shader to retrieve.</param>
        /// <returns>The registered glyph shader at the specified index within the command stream's internal registry.</returns>
        public GlyphShader GetGlyphShader(Int32 index)
        {
            return glyphShaders[index];
        }

        /// <summary>
        /// Prepares the stream for reading or writing by acquiring pointers to its underlying buffers.
        /// While pointers are acquired, these buffers will be pinned in memory.
        /// </summary>
        public void AcquirePointers()
        {
            stream.AcquirePointers();
        }

        /// <summary>
        /// Releases the pointers which were acquired by <see cref="AcquirePointers()"/> and unpins
        /// the stream's underlying buffers.
        /// </summary>
        public void ReleasePointers()
        {
            stream.ReleasePointers();
        }

        /// <summary>
        /// Removes all commands from the stream.
        /// </summary>
        public void Clear()
        {
            stream.Clear();
            
            icons.Clear();
            iconsByName.Clear();

            fonts.Clear();
            fontsByName.Clear();

            glyphShaders.Clear();
            glyphShadersByName.Clear();
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.Text"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteText(TextLayoutTextCommand command)
        {
            stream.Reserve(sizeof(TextLayoutTextCommand));
            *(TextLayoutTextCommand*)stream.Data = command;
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.Text;
            stream.FinalizeObject(sizeof(TextLayoutTextCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.Icon"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteIcon(TextLayoutIconCommand command)
        {
            stream.Reserve(sizeof(TextLayoutIconCommand));
            *(TextLayoutIconCommand*)stream.Data = command;
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.Icon;
            stream.FinalizeObject(sizeof(TextLayoutIconCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.ToggleBold"/> command to the current position in the stream.
        /// </summary>
        public void WriteToggleBold()
        {
            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.ToggleBold;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.ToggleItalic"/> command to the current position in the stream.
        /// </summary>
        public void WriteToggleItalic()
        {
            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.ToggleItalic;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PushFont"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePushFont(TextLayoutFontCommand command)
        {
            stream.Reserve(sizeof(TextLayoutFontCommand));
            *(TextLayoutFontCommand*)stream.Data = command;
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.PushFont;
            stream.FinalizeObject(sizeof(TextLayoutFontCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PushColor"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePushColor(TextLayoutColorCommand command)
        {
            stream.Reserve(sizeof(TextLayoutColorCommand));
            *(TextLayoutColorCommand*)stream.Data = command;
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.PushColor;
            stream.FinalizeObject(sizeof(TextLayoutColorCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PushGlyphShader"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePushGlyphShader(TextLayoutGlyphShaderCommand command)
        {
            stream.Reserve(sizeof(TextLayoutGlyphShaderCommand));
            *(TextLayoutGlyphShaderCommand*)stream.Data = command;
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.PushGlyphShader;
            stream.FinalizeObject(sizeof(TextLayoutGlyphShaderCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PopFont"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePopFont()
        {
            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.PopFont;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PopColor"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePopColor()
        {
            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.PopColor;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PopGlyphShader"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePopGlyphShader()
        {
            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.PopGlyphShader;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.Text"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutTextCommand ReadTextCommand()
        {
            var command = *(TextLayoutTextCommand*)stream.Data;
            stream.Seek(sizeof(TextLayoutTextCommand), SeekOrigin.Current);
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.Icon"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutIconCommand ReadIconCommand()
        {
            var command = *(TextLayoutIconCommand*)stream.Data;
            stream.Seek(sizeof(TextLayoutIconCommand), SeekOrigin.Current);
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.ToggleBold"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadToggleBoldCommand()
        {
            stream.Seek(sizeof(TextLayoutCommandType), SeekOrigin.Current);
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.ToggleItalic"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadToggleItalicCommand()
        {
            stream.Seek(sizeof(TextLayoutCommandType), SeekOrigin.Current);
        }

        /// <summary>
        /// Gets the number of commands in the stream.
        /// </summary>
        public Int32 Count
        {
            get { return stream.LengthInObjects; }
        }

        // The underlying data stream containing our commands.
        private readonly UnsafeObjectStream stream = new UnsafeObjectStream(32, 256);

        // The stream's object registries.
        private readonly Dictionary<StringSegment, Int32> iconsByName = new Dictionary<StringSegment, Int32>();
        private readonly Dictionary<StringSegment, Int32> fontsByName = new Dictionary<StringSegment, Int32>();
        private readonly Dictionary<StringSegment, Int32> glyphShadersByName = new Dictionary<StringSegment, Int32>();
        private readonly List<SpriteAnimation> icons = new List<SpriteAnimation>();
        private readonly List<SpriteFontFace> fonts = new List<SpriteFontFace>();
        private readonly List<GlyphShader> glyphShaders = new List<GlyphShader>();
    }
}
