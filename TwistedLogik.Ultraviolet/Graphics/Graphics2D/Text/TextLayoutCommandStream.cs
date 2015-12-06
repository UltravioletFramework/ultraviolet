using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
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
        /// Gets a <see cref="LineInfo"/> structure which describes the specified line of formatted text.
        /// </summary>
        /// <param name="index">The index of the line for which to retrieve metadata.</param>
        /// <returns>A <see cref="LineInfo"/> structure which describes the specified line of formatted text.</returns>
        public LineInfo GetLineInfo(Int32 index)
        {
            Contract.EnsureRange(index >= 0 && index < LineCount, "index");

            var acquiredPointers = !HasAcquiredPointers;
            if (acquiredPointers)
                AcquirePointers();

            Seek(0);
            var blockOffset = ((TextLayoutBlockInfoCommand*)Data)->Offset;

            SeekNextCommand();
            var lineInfo = (TextLayoutLineInfoCommand*)Data;
            var linePosition = 0;
            for (int i = 0; i < index; i++)
            {
                linePosition += lineInfo->LineHeight;
                Seek(1 + StreamPositionInObjects + lineInfo->LengthInCommands);
                lineInfo = (TextLayoutLineInfoCommand*)Data;
            }

            var result = new LineInfo(lineInfo->Offset, blockOffset + linePosition, 
                lineInfo->LineWidth, lineInfo->LineHeight, lineInfo->LengthInCommands, lineInfo->LengthInGlyphs);

            if (acquiredPointers)
                ReleasePointers();

            return result;
        }

        /// <summary>
        /// Moves the stream to the command with the specified index.
        /// </summary>
        /// <param name="index">The index of the command to which the stream will seek.</param>
        /// <returns>A <see cref="TextLayoutCommandType"/> that represents the type of command at the stream's new position.</returns>
        public TextLayoutCommandType Seek(Int32 index)
        {
            return *(TextLayoutCommandType*)stream.RawSeekObject(index);
        }

        /// <summary>
        /// Moves the stream to the first command in the specified line of text.
        /// </summary>
        /// <param name="index">The index of the line to which to seek.</param>
        /// <returns>A <see cref="TextLayoutCommandType"/> that represents the type of command at the stream's new position.</returns>
        public TextLayoutCommandType SeekLine(Int32 index)
        {
            Contract.EnsureRange(index >= 0 && index < LineCount, "index");

            var position = 1;

            for (int i = 0; i <= index; i++)
            {
                stream.RawSeekObject(position);
                position += ((TextLayoutLineInfoCommand*)stream.Data)->LengthInCommands;
            }

            return *(TextLayoutCommandType*)stream.Data;
        }

        /// <summary>
        /// Moves the stream to the next command.
        /// </summary>
        /// <returns><c>true</c> if the stream was able to seek to the next command; otherwise, <c>false</c>.</returns>
        public Boolean SeekNextCommand()
        {
            if (stream.PositionInObjects < stream.LengthInObjects)
            {
                stream.RawSeekForward();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the stream to the first command in the next line of text, if there is one.
        /// </summary>
        /// <returns><c>true</c> if the stream was able to seek to another line of text; otherwise, <c>false</c>.</returns>
        public Boolean SeekNextLine()
        {
            var currentCommandType = *(TextLayoutCommandType*)stream.Data;
            if (currentCommandType == TextLayoutCommandType.LineInfo)
            {
                Seek(stream.PositionInObjects + ((TextLayoutLineInfoCommand*)stream.Data)->LengthInCommands + 1);
            }
            else
            {
                while (*(TextLayoutCommandType*)stream.Data != TextLayoutCommandType.LineInfo && SeekNextCommand()) { }
            }
            return stream.PositionInObjects + 1 < Count;
        }

        /// <summary>
        /// Registers a style with the command stream.
        /// </summary>
        /// <param name="name">The name of the style to register.</param>
        /// <param name="style">The style to register under the specified name.</param>
        /// <returns>The index of the specified style within the command stream's internal registry.</returns>
        public Int16 RegisterStyle(StringSegment name, TextStyle style)
        {
            Contract.Require(style, "style");

            return RegisterResource(name, style, styles, stylesByName);
        }

        /// <summary>
        /// Registers an icon with the command stream.
        /// </summary>
        /// <param name="name">The name of the icon to register.</param>
        /// <param name="icon">The icon to register under the specified name.</param>
        /// <returns>The index of the specified icon within the command stream's internal registry.</returns>
        public Int16 RegisterIcon(StringSegment name, TextIconInfo icon)
        {
            return RegisterResource(name, icon, icons, iconsByName);
        }

        /// <summary>
        /// Registers a font with the command stream.
        /// </summary>
        /// <param name="name">The name of the font to register.</param>
        /// <param name="font">The font to register under the specified name.</param>
        /// <returns>The index of the specified font within the command stream's internal registry.</returns>
        public Int16 RegisterFont(StringSegment name, SpriteFont font)
        {
            Contract.Require(font, "font");

            return RegisterResource(name, font, fonts, fontsByName);
        }

        /// <summary>
        /// Registers a glyph shader with the command stream.
        /// </summary>
        /// <param name="name">The name of the glyph shader to register.</param>
        /// <param name="glyphShader">The glyph shader to register under the specified name.</param>
        /// <returns>The index of the specified glyph shader within the command stream's internal registry.</returns>
        public Int16 RegisterGlyphShader(StringSegment name, GlyphShader glyphShader)
        {
            Contract.Require(glyphShader, "glyphShader");

            return RegisterResource(name, glyphShader, glyphShaders, glyphShadersByName);
        }

        /// <summary>
        /// Registers a source string with the command stream.
        /// </summary>
        /// <param name="source">The source string to register.</param>
        /// <returns>The index of the specified source string within the command stream's internal registry.</returns>
        public Int16 RegisterSourceString(String source)
        {
            Contract.Require(source, "source");

            return RegisterSource(source);
        }

        /// <summary>
        /// Registers a source string builder with the command stream.
        /// </summary>
        /// <param name="source">The source string builder to register.</param>
        /// <returns>The index of the specified source string builder within the command stream's internal registry.</returns>
        public Int16 RegisterSourceStringBuilder(StringBuilder source)
        {
            Contract.Require(source, "source");

            return RegisterSource(source);
        }

        /// <summary>
        /// Retrieves the registered style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style to retrieve.</param>
        /// <returns>The registered style with the specified name.</returns>
        public TextStyle GetStyle(StringSegment name)
        {
            Int16 index;
            if (!stylesByName.TryGetValue(name, out index))
                return null;

            return styles[index];
        }

        /// <summary>
        /// Retrieves the registered style at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered style to retrieve.</param>
        /// <returns>The registered style at the specified index within the command stream's internal registry.</returns>
        public TextStyle GetStyle(Int16 index)
        {
            return styles[index];
        }

        /// <summary>
        /// Retrieves the registered icon with the specified name.
        /// </summary>
        /// <param name="name">The name of the icon to retrieve.</param>
        /// <returns>The registered icon with the specified name.</returns>
        public TextIconInfo? GetIcon(StringSegment name)
        {
            Int16 index;
            if (!iconsByName.TryGetValue(name, out index))
                return null;

            return icons[index];
        }

        /// <summary>
        /// Retrieves the registered icon at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered icon to retrieve.</param>
        /// <returns>The registered icon at the specified index within the command stream's internal registry.</returns>
        public TextIconInfo GetIcon(Int16 index)
        {
            return icons[index];
        }

        /// <summary>
        /// Retrieves the registered font with the specified name.
        /// </summary>
        /// <param name="name">The name of the font to retrieve.</param>
        /// <returns>The registered font with the specified name.</returns>
        public SpriteFont GetFont(StringSegment name)
        {
            Int16 index;
            if (!fontsByName.TryGetValue(name, out index))
                return null;

            return fonts[index];
        }

        /// <summary>
        /// Retrieves the registered font at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered font to retrieve.</param>
        /// <returns>The registered font at the specified index within the command stream's internal registry.</returns>
        public SpriteFont GetFont(Int16 index)
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
            Int16 index;
            if (!glyphShadersByName.TryGetValue(name, out index))
                return null;

            return glyphShaders[index];
        }

        /// <summary>
        /// Retrieves the registered glyph shader at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered glyph shader to retrieve.</param>
        /// <returns>The registered glyph shader at the specified index within the command stream's internal registry.</returns>
        public GlyphShader GetGlyphShader(Int16 index)
        {
            return glyphShaders[index];
        }

        /// <summary>
        /// Retrieves the registered source string at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered source string to retrieve.</param>
        /// <returns>The registered source string at the specified index within the command stream's internal registry.</returns>
        public String GetSourceString(Int16 index)
        {
            return (String)sources[index];
        }

        /// <summary>
        /// Retrieves the registered source string builder at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered source string builder to retrieve.</param>
        /// <returns>The registered source string builder at the specified index within the command stream's internal registry.</returns>
        public StringBuilder GetSourceStringBuilder(Int16 index)
        {
            return (StringBuilder)sources[index];
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

            styles.Clear();
            stylesByName.Clear();

            icons.Clear();
            iconsByName.Clear();

            fonts.Clear();
            fontsByName.Clear();

            glyphShaders.Clear();
            glyphShadersByName.Clear();

            sources.Clear();
            sourcesByReference.Clear();

            Settings = default(TextLayoutSettings);
            SourceText = StringSegment.Empty;
            Bounds = default(Rectangle);
            ActualWidth = 0;
            ActualHeight = 0;
            TotalLength = 0;
            LineCount = 0;

            hasMultipleFontStyles = false;
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.BlockInfo"/> command to the current position in the stream.
        /// </summary>
        public void WriteBlockInfo()
        {
            stream.Reserve(sizeof(TextLayoutBlockInfoCommand));
            *(TextLayoutBlockInfoCommand*)stream.Data = new TextLayoutBlockInfoCommand();
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.BlockInfo;
            stream.FinalizeObject(sizeof(TextLayoutBlockInfoCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.LineInfo"/> command to the current position in the stream.
        /// </summary>
        public void WriteLineInfo()
        {
            stream.Reserve(sizeof(TextLayoutLineInfoCommand));
            *(TextLayoutLineInfoCommand*)stream.Data = new TextLayoutLineInfoCommand();
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.LineInfo;
            stream.FinalizeObject(sizeof(TextLayoutLineInfoCommand));
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
            hasMultipleFontStyles = true;

            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.ToggleBold;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.ToggleItalic"/> command to the current position in the stream.
        /// </summary>
        public void WriteToggleItalic()
        {
            hasMultipleFontStyles = true;

            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.ToggleItalic;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PushStyle"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePushStyle(TextLayoutStyleCommand command)
        {
            hasMultipleFontStyles = true;

            stream.Reserve(sizeof(TextLayoutStyleCommand));
            *(TextLayoutStyleCommand*)stream.Data = command;
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.PushStyle;
            stream.FinalizeObject(sizeof(TextLayoutStyleCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PushFont"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePushFont(TextLayoutFontCommand command)
        {
            hasMultipleFontStyles = true;

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
        /// Writes a <see cref="TextLayoutCommandType.PopStyle"/> command to the current position in the stream.
        /// </summary>
        public void WritePopStyle()
        {
            hasMultipleFontStyles = true;

            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.PopStyle;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PopFont"/> command to the current position in the stream.
        /// </summary>
        public void WritePopFont()
        {
            hasMultipleFontStyles = true;

            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.PopFont;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PopColor"/> command to the current position in the stream.
        /// </summary>
        public void WritePopColor()
        {
            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.PopColor;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PopGlyphShader"/> command to the current position in the stream.
        /// </summary>
        public void WritePopGlyphShader()
        {
            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.PopGlyphShader;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.ChangeSourceString"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteChangeSourceString(TextLayoutSourceStringCommand command)
        {
            stream.Reserve(sizeof(TextLayoutSourceStringCommand));
            *(TextLayoutSourceStringCommand*)stream.Data = command;
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.ChangeSourceString;
            stream.FinalizeObject(sizeof(TextLayoutSourceStringCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.ChangeSourceStringBuilder"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteChangeSourceStringBuilder(TextLayoutSourceStringBuilderCommand command)
        {
            stream.Reserve(sizeof(TextLayoutSourceStringBuilderCommand));
            *(TextLayoutSourceStringBuilderCommand*)stream.Data = command;
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.ChangeSourceStringBuilder;
            stream.FinalizeObject(sizeof(TextLayoutSourceStringBuilderCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.Hyphen"/> command to the current position in the stream.
        /// </summary>
        public void WriteHyphen()
        {
            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.Hyphen;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.LineBreak"/> command to the current position in the stream.
        /// </summary>
        public void WriteLineBreak()
        {
            stream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)stream.Data = TextLayoutCommandType.LineBreak;
            stream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.BlockInfo"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutBlockInfoCommand ReadBlockInfoCommand()
        {
            var command = *(TextLayoutBlockInfoCommand*)stream.Data;
            stream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.LineInfo"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutLineInfoCommand ReadLineInfoCommand()
        {
            var command = *(TextLayoutLineInfoCommand*)stream.Data;
            stream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.Text"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutTextCommand ReadTextCommand()
        {
            var command = *(TextLayoutTextCommand*)stream.Data;
            stream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.Icon"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutIconCommand ReadIconCommand()
        {
            var command = *(TextLayoutIconCommand*)stream.Data;
            stream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.ToggleBold"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadToggleBoldCommand()
        {
            stream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.ToggleItalic"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadToggleItalicCommand()
        {
            stream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PushStyle"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutStyleCommand ReadPushStyleCommand()
        {
            var command = *(TextLayoutStyleCommand*)stream.Data;
            stream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PushFont"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutFontCommand ReadPushFontCommand()
        {
            var command = *(TextLayoutFontCommand*)stream.Data;
            stream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PushColor"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutColorCommand ReadPushColorCommand()
        {
            var command = *(TextLayoutColorCommand*)stream.Data;
            stream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PushGlyphShader"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutGlyphShaderCommand ReadPushGlyphShaderCommand()
        {
            var command = *(TextLayoutGlyphShaderCommand*)stream.Data;
            stream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PopStyle"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadPopStyleCommand()
        {
            stream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PopFont"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadPopFontCommand()
        {
            stream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PopColor"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadPopColorCommand()
        {
            stream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PopGlyphShader"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadPopGlyphShaderCommand()
        {
            stream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.ChangeSourceString"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutSourceStringCommand ReadChangeSourceStringCommand()
        {
            var command = *(TextLayoutSourceStringCommand*)stream.Data;
            stream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.ChangeSourceStringBuilder"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutSourceStringBuilderCommand ReadChangeSourceStringBuilderCommand()
        {
            var command = *(TextLayoutSourceStringBuilderCommand*)stream.Data;
            stream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.Hyphen"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadHyphenCommand()
        {
            stream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.LineBreak"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadLineBreakCommand()
        {
            stream.RawSeekForward();
        }

        /// <summary>
        /// Gets the layout settings which were used to produce the command stream.
        /// </summary>
        public TextLayoutSettings Settings
        {
            get;
            internal set;
        }
        
        /// <summary>
        /// Gets the parser options which were used to produce the command stream.
        /// </summary>
        public TextParserOptions ParserOptions
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the text that was processed by the layout engine.
        /// </summary>
        public StringSegment SourceText
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the bounds of the text after layout has been performed, relative to the layout area.
        /// </summary>
        public Rectangle Bounds
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the total width, in pixels, of the text after layout has been performed.
        /// </summary>
        public Int32 ActualWidth
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the total height, in pixels, of the text after layout has been performed.
        /// </summary>
        public Int32 ActualHeight
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the total length of the text which was laid out.
        /// </summary>
        public Int32 TotalLength
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the position of the command stream within its object index.
        /// </summary>
        public Int32 StreamPositionInObjects
        {
            get { return stream.PositionInObjects; }
        }

        /// <summary>
        /// Gets the position of the command stream within its data buffer.
        /// </summary>
        public Int32 StreamPositionInBytes
        {
            get { return stream.PositionInBytes; }
        }

        /// <summary>
        /// Gets the number of commands in the stream.
        /// </summary>
        public Int32 Count
        {
            get { return stream.LengthInObjects; }
        }

        /// <summary>
        /// Gets the number of lines of text in the stream.
        /// </summary>
        public Int32 LineCount
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a pointer to the stream's current position within its internal data buffer.
        /// </summary>
        public IntPtr Data
        {
            get
            {
                return stream.Data;
            }
        }

        /// <summary>
        /// Gets a pointer to the beginning of the stream's internal data buffer.
        /// </summary>
        public IntPtr Data0
        {
            get
            {
                return stream.Data0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the stream contains commands which change the style of the rendered font 
        /// </summary>
        /// <remarks>This value will be <c>true</c> if the stream contains any 
        /// <see cref="TextLayoutCommandType.ToggleBold"/>,
        /// <see cref="TextLayoutCommandType.ToggleItalic"/>,
        /// <see cref="TextLayoutCommandType.PushFont"/>, 
        /// <see cref="TextLayoutCommandType.PushStyle"/>, 
        /// <see cref="TextLayoutCommandType.PopFont"/>, or 
        /// <see cref="TextLayoutCommandType.PopStyle"/> commands.
        /// </remarks>
        public Boolean HasMultipleFontStyles
        {
            get { return hasMultipleFontStyles; }
        }

        /// <summary>
        /// Gets a value indicating whether the command stream has acquired pointers to its underlying buffers.
        /// </summary>
        public Boolean HasAcquiredPointers
        {
            get { return stream.HasAcquiredPointers; }
        }
        
        /// <summary>
        /// Gets the <see cref="UnsafeObjectStream"/> which provides the command stream's storage.
        /// </summary>
        internal UnsafeObjectStream InternalObjectStream
        {
            get { return stream; }
        }
        
        /// <summary>
        /// Registers a resource with the command stream.
        /// </summary>
        private Int16 RegisterResource<TResource>(StringSegment name, TResource resource, List<TResource> resourcesList, Dictionary<StringSegment, Int16> resourcesByName)
        {
            Int16 index;
            if (resourcesByName.TryGetValue(name, out index))
                return index;

            index = (Int16)resourcesList.Count;

            if (index > Int16.MaxValue)
                throw new InvalidOperationException("TODO");

            resourcesList.Add(resource);
            resourcesByName[name] = index;

            return index;
        }

        /// <summary>
        /// Registers a source string or string builder with the command stream.
        /// </summary>
        private Int16 RegisterSource(Object source)
        {
            var index = default(Int16);
            if (sourcesByReference.TryGetValue(source, out index))
                return index;

            index = (Int16)sources.Count;

            if (index > Int16.MaxValue)
                throw new InvalidOperationException("TODO");

            sources.Add(source);
            sourcesByReference[source] = index;

            return (Int16)index;
        }

        // The underlying data stream containing our commands.
        private readonly UnsafeObjectStream stream = new UnsafeObjectStream(32, 256);

        // The stream's object registries.
        private readonly Dictionary<StringSegment, Int16> stylesByName = new Dictionary<StringSegment, Int16>();
        private readonly Dictionary<StringSegment, Int16> iconsByName = new Dictionary<StringSegment, Int16>();
        private readonly Dictionary<StringSegment, Int16> fontsByName = new Dictionary<StringSegment, Int16>();
        private readonly Dictionary<StringSegment, Int16> glyphShadersByName = new Dictionary<StringSegment, Int16>();
        private readonly Dictionary<Object, Int16> sourcesByReference = new Dictionary<Object, Int16>();
        private readonly List<TextStyle> styles = new List<TextStyle>();
        private readonly List<TextIconInfo> icons = new List<TextIconInfo>();
        private readonly List<SpriteFont> fonts = new List<SpriteFont>();
        private readonly List<GlyphShader> glyphShaders = new List<GlyphShader>();
        private readonly List<Object> sources = new List<Object>();

        // Property values.
        private Boolean hasMultipleFontStyles;
    }
}
