using System;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections.Specialized;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a method which is used to evaluate a custom text layout command.
    /// </summary>
    /// <param name="state">An arbitrary state object.</param>
    /// <param name="position">The command's position within the source text.</param>
    /// <param name="command">The command which is being evaluated.</param>
    /// <returns><see langword="true"/> to continue searching for commands; otherwise, <see langword="false"/>.</returns>
    public delegate Boolean TextLayoutCommandEvaluator(Object state, Int32 position, TextLayoutCustomCommand command);

    /// <summary>
    /// Represents a stream of commands produced by the text layout engine.
    /// </summary>
    public unsafe class TextLayoutCommandStream
    {
        /// <summary>
        /// Searches for custom commands in stream and allows them to be evaluated
        /// with the specified evaluator function.
        /// </summary>
        /// <param name="state">A state object to pass to the evaluator.</param>
        /// <param name="evaluator">The function with which to evaluate custom commands.</param>
        public void GetCustomCommands(Object state, TextLayoutCommandEvaluator evaluator)
        {
            GetCustomCommands(0, Int32.MaxValue, state, evaluator);
        }

        /// <summary>
        /// Searches for custom commands in the specified range of glyphs and allows them to be evaluated
        /// with the specified evaluator function.
        /// </summary>
        /// <param name="startGlyph">The index of the first glyph to consider.</param>
        /// <param name="glyphCount">The number of glyphs to consider.</param>
        /// <param name="state">A state object to pass to the evaluator.</param>
        /// <param name="evaluator">The function with which to evaluate custom commands.</param>
        /// <remarks>
        /// This method will return custom commands which fall immediately after the last glyph in the
        /// specified range, but not commands which fall immediately before that range, unless the range
        /// starts at the beginning of the text.
        /// </remarks>
        public void GetCustomCommands(Int32 startGlyph, Int32 glyphCount, Object state, TextLayoutCommandEvaluator evaluator)
        {
            Contract.EnsureRange(startGlyph >= 0, nameof(startGlyph));
            Contract.Require(evaluator, nameof(evaluator));

            var terminateOnNextNonCommandToken = false;

            var glyphsSeen = 0;
            var glyphsMax = (glyphCount == Int32.MaxValue) ? Int32.MaxValue : startGlyph + glyphCount;

            var acquiredPointers = !HasAcquiredPointers;
            if (acquiredPointers)
                AcquirePointers();

            Seek(0);

            while (StreamPositionInObjects < Count)
            {                
                var cmdType = *(TextLayoutCommandType*)Data;
                if (cmdType != TextLayoutCommandType.Custom && terminateOnNextNonCommandToken)
                    break;

                switch (cmdType)
                {
                    case TextLayoutCommandType.Text:
                        {
                            var cmd = (TextLayoutTextCommand*)Data;
                            glyphsSeen += cmd->SourceLength;                            
                        }
                        break;
                        
                    case TextLayoutCommandType.Icon:
                        {
                            glyphsSeen++;
                        }
                        break;

                    case TextLayoutCommandType.LineBreak:
                        {
                            var cmd = (TextLayoutLineBreakCommand*)Data;
                            if (cmd->SourceLength > 0)
                            {
                                glyphsSeen += cmd->SourceLength;
                            }
                        }
                        break;

                    case TextLayoutCommandType.Custom:
                        {
                            if (glyphsSeen > startGlyph || startGlyph == 0)
                            {
                                var cmd = (TextLayoutCustomCommand*)Data;
                                if (!evaluator(state, glyphsSeen, *cmd))
                                    terminateOnNextNonCommandToken = true;
                            }
                        }
                        break;
                }
                
                if (glyphsSeen > glyphsMax)
                    break;

                SeekNextCommand();
            }

            if (acquiredPointers)
                ReleasePointers();
        }

        /// <summary>
        /// Gets a <see cref="LineInfo"/> structure which describes the specified line of formatted text.
        /// </summary>
        /// <param name="index">The index of the line for which to retrieve metadata.</param>
        /// <returns>A <see cref="LineInfo"/> structure which describes the specified line of formatted text.</returns>
        public LineInfo GetLineInfo(Int32 index)
        {
            Contract.EnsureRange(index >= 0 && index < LineCount, nameof(index));

            var acquiredPointers = !HasAcquiredPointers;
            if (acquiredPointers)
                AcquirePointers();

            Seek(0);
            var blockOffset = ((TextLayoutBlockInfoCommand*)Data)->Offset;

            SeekNextCommand();
            var lineInfo = (TextLayoutLineInfoCommand*)Data;
            var linePosition = 0;
            var lineOffsetInSource = 0;
            var lineOffsetInGlyphs = 0;
            for (int i = 0; i < index; i++)
            {
                linePosition += lineInfo->LineHeight;
                lineOffsetInSource += lineInfo->LengthInSource;
                lineOffsetInGlyphs += lineInfo->LengthInGlyphs;
                Seek(1 + StreamPositionInObjects + lineInfo->LengthInCommands);
                lineInfo = (TextLayoutLineInfoCommand*)Data;
            }

            var result = new LineInfo(this, index, StreamPositionInObjects, lineOffsetInSource, lineOffsetInGlyphs, lineInfo->Offset, blockOffset + linePosition, 
                lineInfo->LineWidth, lineInfo->LineHeight, lineInfo->LengthInCommands, lineInfo->LengthInSource, lineInfo->LengthInGlyphs, lineInfo->TerminatingLineBreakSourceLength);

            if (acquiredPointers)
                ReleasePointers();

            return result;
        }

        /// <summary>
        /// Gets a <see cref="LineInfo"/> structure which describes the line that comes immediately
        /// after the specified line.
        /// </summary>
        /// <param name="previous">A <see cref="LineInfo"/> structure which describes the previous line.</param>
        /// <param name="next">A <see cref="LineInfo"/> structure which describes the line that comes after <paramref name="previous"/>.</param>
        /// <returns><see langword="true"/> if a line was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetNextLineInfo(LineInfo previous, out LineInfo next)
        {
            return GetNextLineInfoRef(ref previous, out next);
        }

        /// <summary>
        /// Gets a <see cref="LineInfo"/> structure which describes the line that comes immediately
        /// after the specified line.
        /// </summary>
        /// <param name="previous">A <see cref="LineInfo"/> structure which describes the previous line.</param>
        /// <param name="next">A <see cref="LineInfo"/> structure which describes the line that comes after <paramref name="previous"/>.</param>
        /// <returns><see langword="true"/> if a line was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetNextLineInfoRef(ref LineInfo previous, out LineInfo next)
        {
            if (previous.Source != this)
                throw new ArgumentException(UltravioletStrings.LineInfoIsNotFromSameSource);

            if (previous.LineIndex + 1 == LineCount)
            {
                next = default(LineInfo);
                return false;
            }

            var acquiredPointers = !HasAcquiredPointers;
            if (acquiredPointers)
                AcquirePointers();

            if (StreamPositionInObjects != previous.OffsetInCommands)
                Seek(previous.OffsetInCommands);

            Seek(1 + StreamPositionInObjects + ((TextLayoutLineInfoCommand*)Data)->LengthInCommands);

            var lineInfo = (TextLayoutLineInfoCommand*)Data;
            next = new LineInfo(this, previous.LineIndex + 1, StreamPositionInObjects, 
                previous.OffsetInSource + previous.LengthInSource,
                previous.OffsetInGlyphs + previous.LengthInGlyphs,
                lineInfo->Offset, previous.Y + previous.Height, lineInfo->LineWidth, 
                lineInfo->LineHeight, lineInfo->LengthInCommands, lineInfo->LengthInSource, lineInfo->LengthInGlyphs, lineInfo->TerminatingLineBreakSourceLength);
            
            if (acquiredPointers)
                ReleasePointers();

            return true;
        }

        /// <summary>
        /// Moves the stream to the command with the specified index.
        /// </summary>
        /// <param name="index">The index of the command to which the stream will seek.</param>
        /// <returns>A <see cref="TextLayoutCommandType"/> that represents the type of command at the stream's new position.</returns>
        public TextLayoutCommandType Seek(Int32 index)
        {
            var ptr = InternalObjectStream.RawSeekObject(index);
            return (index < Count) ? *(TextLayoutCommandType*)ptr : TextLayoutCommandType.None;
        }

        /// <summary>
        /// Moves the stream to the first command in the specified line of text.
        /// </summary>
        /// <param name="index">The index of the line to which to seek.</param>
        /// <returns>A <see cref="TextLayoutCommandType"/> that represents the type of command at the stream's new position.</returns>
        public TextLayoutCommandType SeekLine(Int32 index)
        {
            Contract.EnsureRange(index >= 0 && index < LineCount, nameof(index));

            var position = 1;

            for (int i = 0; i <= index; i++)
            {
                InternalObjectStream.RawSeekObject(position);
                position += ((TextLayoutLineInfoCommand*)InternalObjectStream.Data)->LengthInCommands;
            }

            return *(TextLayoutCommandType*)InternalObjectStream.Data;
        }

        /// <summary>
        /// Moves the stream before its first element.
        /// </summary>
        public void SeekBeginning()
        {
            Seek(0);
        }

        /// <summary>
        /// Moves the stream past its last element.
        /// </summary>
        public void SeekEnd()
        {
            Seek(Count);
        }

        /// <summary>
        /// Moves the stream to the previous command.
        /// </summary>
        /// <returns><see langword="true"/> if the stream was able to seek to the previous command; otherwise, <see langword="false"/>.</returns>
        public Boolean SeekPreviousCommand()
        {
            if (InternalObjectStream.PositionInObjects > 0)
            {
                InternalObjectStream.RawSeekBackward();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the stream to the next command.
        /// </summary>
        /// <returns><see langword="true"/> if the stream was able to seek to the next command; otherwise, <see langword="false"/>.</returns>
        public Boolean SeekNextCommand()
        {
            if (InternalObjectStream.PositionInObjects < InternalObjectStream.LengthInObjects)
            {
                InternalObjectStream.RawSeekForward();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the stream to the first command in the next line of text, if there is one.
        /// </summary>
        /// <returns><see langword="true"/> if the stream was able to seek to another line of text; otherwise, <see langword="false"/>.</returns>
        public Boolean SeekNextLine()
        {
            var currentCommandType = *(TextLayoutCommandType*)InternalObjectStream.Data;
            if (currentCommandType == TextLayoutCommandType.LineInfo)
            {
                Seek(InternalObjectStream.PositionInObjects + ((TextLayoutLineInfoCommand*)InternalObjectStream.Data)->LengthInCommands + 1);
            }
            else
            {
                while (*(TextLayoutCommandType*)InternalObjectStream.Data != TextLayoutCommandType.LineInfo && SeekNextCommand()) { }
            }
            return InternalObjectStream.PositionInObjects < Count;
        }

        /// <summary>
        /// Registers a source string with the command stream.
        /// </summary>
        /// <param name="source">The source string to register.</param>
        /// <returns>The index of the specified source string within the command stream's internal registry.</returns>
        public Int16 RegisterSourceString(String source) =>
            (resources = resources ?? new TextLayoutCommandStreamResources()).RegisterSource(source);

        /// <summary>
        /// Registers a source string builder with the command stream.
        /// </summary>
        /// <param name="source">The source string builder to register.</param>
        /// <returns>The index of the specified source string builder within the command stream's internal registry.</returns>
        public Int16 RegisterSourceStringBuilder(StringBuilder source) =>
            (resources = resources ?? new TextLayoutCommandStreamResources()).RegisterSource(source);

        /// <summary>
        /// Registers a source shaped string with the command stream.
        /// </summary>
        /// <param name="source">The source shaped string to register.</param>
        /// <returns>The index of the specified source shaped string within the command stream's internal registry.</returns>
        public Int16 RegisterSourceShapedString(ShapedString source) =>
            (resources = resources ?? new TextLayoutCommandStreamResources()).RegisterSource(source);

        /// <summary>
        /// Registers a source shaped string builder with the command stream.
        /// </summary>
        /// <param name="source">The source shaped string builder to register.</param>
        /// <returns>The index of the specified source shaped string builder within the command stream's internal registry.</returns>
        public Int16 RegisterSourceShapedStringBuilder(ShapedStringBuilder source) =>
            (resources = resources ?? new TextLayoutCommandStreamResources()).RegisterSource(source);

        /// <summary>
        /// Registers a style with the command stream.
        /// </summary>
        /// <param name="name">The name of the style to register.</param>
        /// <param name="style">The style to register under the specified name.</param>
        /// <returns>The index of the specified style within the command stream's internal registry.</returns>
        public Int16 RegisterStyle(StringSegment name, TextStyle style) =>
            (resources = resources ?? new TextLayoutCommandStreamResources()).RegisterStyle(name, style);

        /// <summary>
        /// Registers an icon with the command stream.
        /// </summary>
        /// <param name="name">The name of the icon to register.</param>
        /// <param name="icon">The icon to register under the specified name.</param>
        /// <returns>The index of the specified icon within the command stream's internal registry.</returns>
        public Int16 RegisterIcon(StringSegment name, TextIconInfo icon) =>
            (resources = resources ?? new TextLayoutCommandStreamResources()).RegisterIcon(name, icon);

        /// <summary>
        /// Registers a font with the command stream.
        /// </summary>
        /// <param name="name">The name of the font to register.</param>
        /// <param name="font">The font to register under the specified name.</param>
        /// <returns>The index of the specified font within the command stream's internal registry.</returns>
        public Int16 RegisterFont(StringSegment name, UltravioletFont font) =>
            (resources = resources ?? new TextLayoutCommandStreamResources()).RegisterFont(name, font);

        /// <summary>
        /// Registers a glyph shader with the command stream.
        /// </summary>
        /// <param name="name">The name of the glyph shader to register.</param>
        /// <param name="glyphShader">The glyph shader to register under the specified name.</param>
        /// <returns>The index of the specified glyph shader within the command stream's internal registry.</returns>
        public Int16 RegisterGlyphShader(StringSegment name, GlyphShader glyphShader) =>
            (resources = resources ?? new TextLayoutCommandStreamResources()).RegisterGlyphShader(name, glyphShader);

        /// <summary>
        /// Registers a link target with the command stream.
        /// </summary>
        /// <param name="target">The link target to register.</param>
        /// <returns>The index of the specified link target within the command stream's internal registry.</returns>
        public Int16 RegisterLinkTarget(StringSegment target) =>
            (resources = resources ?? new TextLayoutCommandStreamResources()).RegisterLinkTarget(target.ToString());

        /// <summary>
        /// Retrieves the registered source string at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered source string to retrieve.</param>
        /// <returns>The registered source string at the specified index within the command stream's internal registry.</returns>
        public String GetSourceString(Int16 index)
        {
            if (resources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return resources.GetSourceString(index);
        }

        /// <summary>
        /// Retrieves the registered source string builder at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered source string builder to retrieve.</param>
        /// <returns>The registered source string builder at the specified index within the command stream's internal registry.</returns>
        public StringBuilder GetSourceStringBuilder(Int16 index)
        {
            if (resources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return resources.GetSourceStringBuilder(index);
        }

        /// <summary>
        /// Retrieves the registered source shaped string at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered source shaped string to retrieve.</param>
        /// <returns>The registered source shaped string at the specified index within the command stream's internal registry.</returns>
        public ShapedString GetSourceShapedString(Int16 index)
        {
            if (resources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return resources.GetSourceShapedString(index);
        }

        /// <summary>
        /// Retrieves the registered source shaped string builder at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered source shaped string builder to retrieve.</param>
        /// <returns>The registered source shaped string builder at the specified index within the command stream's internal registry.</returns>
        public ShapedStringBuilder GetSourceShapedStringBuilder(Int16 index)
        {
            if (resources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return resources.GetSourceShapedStringBuilder(index);
        }

        /// <summary>
        /// Retrieves the registered style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style to retrieve.</param>
        /// <returns>The registered style with the specified name.</returns>
        public TextStyle GetStyle(StringSegment name)
        {
            return resources?.GetStyle(name);
        }

        /// <summary>
        /// Retrieves the registered style at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered style to retrieve.</param>
        /// <returns>The registered style at the specified index within the command stream's internal registry.</returns>
        public TextStyle GetStyle(Int16 index)
        {
            if (resources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return resources.GetStyle(index);
        }

        /// <summary>
        /// Retrieves the registered icon with the specified name.
        /// </summary>
        /// <param name="name">The name of the icon to retrieve.</param>
        /// <returns>The registered icon with the specified name.</returns>
        public TextIconInfo? GetIcon(StringSegment name)
        {
            return resources?.GetIcon(name);
        }

        /// <summary>
        /// Retrieves the registered icon at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered icon to retrieve.</param>
        /// <returns>The registered icon at the specified index within the command stream's internal registry.</returns>
        public TextIconInfo GetIcon(Int16 index)
        {
            if (resources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return resources.GetIcon(index);
        }

        /// <summary>
        /// Retrieves the registered font with the specified name.
        /// </summary>
        /// <param name="name">The name of the font to retrieve.</param>
        /// <returns>The registered font with the specified name.</returns>
        public UltravioletFont GetFont(StringSegment name)
        {
            return resources?.GetFont(name);
        }

        /// <summary>
        /// Retrieves the registered font at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered font to retrieve.</param>
        /// <returns>The registered font at the specified index within the command stream's internal registry.</returns>
        public UltravioletFont GetFont(Int16 index)
        {
            if (resources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return resources.GetFont(index);
        }

        /// <summary>
        /// Retrieves the registered glyph shader with the specified name.
        /// </summary>
        /// <param name="name">The name of the glyph shader to retrieve.</param>
        /// <returns>The registered glyph shader with the specified name.</returns>
        public GlyphShader GetGlyphShader(StringSegment name)
        {
            return resources?.GetGlyphShader(name);
        }

        /// <summary>
        /// Retrieves the registered glyph shader at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered glyph shader to retrieve.</param>
        /// <returns>The registered glyph shader at the specified index within the command stream's internal registry.</returns>
        public GlyphShader GetGlyphShader(Int16 index)
        {
            if (resources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return resources.GetGlyphShader(index);
        }

        /// <summary>
        /// Retrieves the registered link target at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered link target to retrieve.</param>
        /// <returns>The registered link target at the specified index within the command stream's internal registry.</returns>
        public String GetLinkTarget(Int16 index)
        {
            if (resources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return resources.GetLinkTarget(index);
        }

        /// <summary>
        /// Updates the position of the cursor relative to this command stream's text.
        /// </summary>
        /// <param name="position">The cursor's position relative to the text's layout area, 
        /// or <see langword="null"/> to indicate that the cursor is not over the text.</param>
        public void UpdateCursor(Point2? position)
        {
            CursorPosition = position;

            if (position == null)
                ActiveLinkIndex = null;
        }

        /// <summary>
        /// Updates the position of the cursor relative to this command stream's text.
        /// </summary>
        /// <param name="x">The x-coordinate of the cursor relative to the text's layout area.</param>
        /// <param name="y">The y-coordinate of the cursor relative to the text's layout area.</param>
        public void UpdateCursor(Int32 x, Int32 y)
        {
            CursorPosition = new Point2(x, y);
        }

        /// <summary>
        /// Activates the link with the specified index within the command stream and deactivates 
        /// any other links in the text.
        /// </summary>
        /// <param name="index">The index of the link to activate, or <see langword="null"/> if no
        /// link should be considered active.</param>
        public void ActivateLink(Int16? index)
        {
            ActiveLinkIndex = index;
        }

        /// <summary>
        /// Prepares the stream for reading or writing by acquiring pointers to its underlying buffers.
        /// While pointers are acquired, these buffers will be pinned in memory.
        /// </summary>
        public void AcquirePointers()
        {
            InternalObjectStream.AcquirePointers();
        }

        /// <summary>
        /// Releases the pointers which were acquired by <see cref="AcquirePointers()"/> and unpins
        /// the stream's underlying buffers.
        /// </summary>
        public void ReleasePointers()
        {
            InternalObjectStream.ReleasePointers();
        }

        /// <summary>
        /// Removes all commands from the stream.
        /// </summary>
        public void Clear()
        {
            InternalObjectStream.Clear();

            if (resources != null)
                resources.Clear();

            Settings = default(TextLayoutSettings);
            SourceText = StringSegment.Empty;
            Bounds = default(Rectangle);
            ActualWidth = 0;
            ActualHeight = 0;
            TotalGlyphLength = 0;
            TotalSourceLength = 0;
            TotalShapedLength = 0;
            LineCount = 0;

            HasMultipleFontStyles = false;
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.BlockInfo"/> command to the current position in the stream.
        /// </summary>
        public void WriteBlockInfo()
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutBlockInfoCommand));
            *(TextLayoutBlockInfoCommand*)InternalObjectStream.Data = new TextLayoutBlockInfoCommand();
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.BlockInfo;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutBlockInfoCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.LineInfo"/> command to the current position in the stream.
        /// </summary>
        public void WriteLineInfo()
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutLineInfoCommand));
            *(TextLayoutLineInfoCommand*)InternalObjectStream.Data = new TextLayoutLineInfoCommand();
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.LineInfo;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutLineInfoCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.Text"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteText(TextLayoutTextCommand command)
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutTextCommand));
            *(TextLayoutTextCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.Text;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutTextCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.Icon"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteIcon(TextLayoutIconCommand command)
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutIconCommand));
            *(TextLayoutIconCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.Icon;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutIconCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.ToggleBold"/> command to the current position in the stream.
        /// </summary>
        public void WriteToggleBold()
        {
            HasMultipleFontStyles = true;

            InternalObjectStream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.ToggleBold;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.ToggleItalic"/> command to the current position in the stream.
        /// </summary>
        public void WriteToggleItalic()
        {
            HasMultipleFontStyles = true;

            InternalObjectStream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.ToggleItalic;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PushStyle"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePushStyle(TextLayoutStyleCommand command)
        {
            HasMultipleFontStyles = true;

            InternalObjectStream.Reserve(sizeof(TextLayoutStyleCommand));
            *(TextLayoutStyleCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.PushStyle;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutStyleCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PushFont"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePushFont(TextLayoutFontCommand command)
        {
            HasMultipleFontStyles = true;

            InternalObjectStream.Reserve(sizeof(TextLayoutFontCommand));
            *(TextLayoutFontCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.PushFont;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutFontCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PushColor"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePushColor(TextLayoutColorCommand command)
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutColorCommand));
            *(TextLayoutColorCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.PushColor;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutColorCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PushGlyphShader"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePushGlyphShader(TextLayoutGlyphShaderCommand command)
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutGlyphShaderCommand));
            *(TextLayoutGlyphShaderCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.PushGlyphShader;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutGlyphShaderCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PopStyle"/> command to the current position in the stream.
        /// </summary>
        public void WritePopStyle()
        {
            HasMultipleFontStyles = true;

            InternalObjectStream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.PopStyle;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PopFont"/> command to the current position in the stream.
        /// </summary>
        public void WritePopFont()
        {
            HasMultipleFontStyles = true;

            InternalObjectStream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.PopFont;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PopColor"/> command to the current position in the stream.
        /// </summary>
        public void WritePopColor()
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.PopColor;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PopGlyphShader"/> command to the current position in the stream.
        /// </summary>
        public void WritePopGlyphShader()
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.PopGlyphShader;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.ChangeSourceString"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteChangeSourceString(TextLayoutSourceStringCommand command)
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutSourceStringCommand));
            *(TextLayoutSourceStringCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.ChangeSourceString;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutSourceStringCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.ChangeSourceStringBuilder"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteChangeSourceStringBuilder(TextLayoutSourceStringBuilderCommand command)
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutSourceStringBuilderCommand));
            *(TextLayoutSourceStringBuilderCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.ChangeSourceStringBuilder;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutSourceStringBuilderCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.ChangeSourceShapedString"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteChangeSourceString(TextLayoutSourceShapedStringCommand command)
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutSourceShapedStringCommand));
            *(TextLayoutSourceShapedStringCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.ChangeSourceShapedString;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutSourceShapedStringCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.ChangeSourceShapedStringBuilder"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteChangeSourceShapedStringBuilder(TextLayoutSourceShapedStringBuilderCommand command)
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutSourceShapedStringBuilderCommand));
            *(TextLayoutSourceShapedStringBuilderCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.ChangeSourceShapedStringBuilder;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutSourceShapedStringBuilderCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.Hyphen"/> command to the current position in the stream.
        /// </summary>
        public void WriteHyphen()
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.Hyphen;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.LineBreak"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteLineBreak(TextLayoutLineBreakCommand command)
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutLineBreakCommand));
            *(TextLayoutLineBreakCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.LineBreak;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutLineBreakCommand));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PushLink"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WritePushLink(TextLayoutLinkCommand command)
        {
            HasMultipleFontStyles = true;

            InternalObjectStream.Reserve(sizeof(TextLayoutLinkCommand));
            *(TextLayoutLinkCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.PushLink;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutLinkCommand));
        }
        
        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.PopLink"/> command to the current position in the stream.
        /// </summary>
        public void WritePopLink()
        {
            HasMultipleFontStyles = true;

            InternalObjectStream.Reserve(sizeof(TextLayoutCommandType));
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.PopLink;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutCommandType));
        }

        /// <summary>
        /// Writes a <see cref="TextLayoutCommandType.Custom"/> command to the current position in the stream.
        /// </summary>
        /// <param name="command">The command to write to the stream.</param>
        public void WriteCustomCommand(TextLayoutCustomCommand command)
        {
            InternalObjectStream.Reserve(sizeof(TextLayoutCustomCommand));
            *(TextLayoutCustomCommand*)InternalObjectStream.Data = command;
            *(TextLayoutCommandType*)InternalObjectStream.Data = TextLayoutCommandType.Custom;
            InternalObjectStream.FinalizeObject(sizeof(TextLayoutCustomCommand));
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.BlockInfo"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutBlockInfoCommand ReadBlockInfoCommand()
        {
            var command = *(TextLayoutBlockInfoCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.LineInfo"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutLineInfoCommand ReadLineInfoCommand()
        {
            var command = *(TextLayoutLineInfoCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.Text"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutTextCommand ReadTextCommand()
        {
            var command = *(TextLayoutTextCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.Icon"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutIconCommand ReadIconCommand()
        {
            var command = *(TextLayoutIconCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.ToggleBold"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadToggleBoldCommand()
        {
            InternalObjectStream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.ToggleItalic"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadToggleItalicCommand()
        {
            InternalObjectStream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PushStyle"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutStyleCommand ReadPushStyleCommand()
        {
            var command = *(TextLayoutStyleCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PushFont"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutFontCommand ReadPushFontCommand()
        {
            var command = *(TextLayoutFontCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PushColor"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutColorCommand ReadPushColorCommand()
        {
            var command = *(TextLayoutColorCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PushGlyphShader"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutGlyphShaderCommand ReadPushGlyphShaderCommand()
        {
            var command = *(TextLayoutGlyphShaderCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PopStyle"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadPopStyleCommand()
        {
            InternalObjectStream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PopFont"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadPopFontCommand()
        {
            InternalObjectStream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PopColor"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadPopColorCommand()
        {
            InternalObjectStream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PopGlyphShader"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadPopGlyphShaderCommand()
        {
            InternalObjectStream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.ChangeSourceString"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutSourceStringCommand ReadChangeSourceStringCommand()
        {
            var command = *(TextLayoutSourceStringCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.ChangeSourceStringBuilder"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutSourceStringBuilderCommand ReadChangeSourceStringBuilderCommand()
        {
            var command = *(TextLayoutSourceStringBuilderCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.Hyphen"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public void ReadHyphenCommand()
        {
            InternalObjectStream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.LineBreak"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutLineBreakCommand ReadLineBreakCommand()
        {
            var command = *(TextLayoutLineBreakCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PushLink"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutLinkCommand ReadPushLinkCommand()
        {
            var command = *(TextLayoutLinkCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.PopLink"/> command from the current position in the command stream.
        /// </summary>
        public void ReadPopLinkCommand()
        {
            InternalObjectStream.RawSeekForward();
        }

        /// <summary>
        /// Reads a <see cref="TextLayoutCommandType.Custom"/> command from the current position in the command stream.
        /// </summary>
        /// <returns>The command that was read.</returns>
        public TextLayoutCustomCommand ReadCustomCommand()
        {
            var command = *(TextLayoutCustomCommand*)InternalObjectStream.Data;
            InternalObjectStream.RawSeekForward();
            return command;
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
        /// Gets the total length of the laid-out text in source characters.
        /// </summary>
        public Int32 TotalSourceLength
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the total length of the laid-out text in shaped characters.
        /// </summary>
        public Int32 TotalShapedLength
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the total length of the laid-out text in glyphs.
        /// </summary>
        public Int32 TotalGlyphLength
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the position of the command stream within its object index.
        /// </summary>
        public Int32 StreamPositionInObjects
        {
            get { return InternalObjectStream.PositionInObjects; }
        }

        /// <summary>
        /// Gets the position of the command stream within its data buffer.
        /// </summary>
        public Int32 StreamPositionInBytes
        {
            get { return InternalObjectStream.PositionInBytes; }
        }

        /// <summary>
        /// Gets the number of commands in the stream.
        /// </summary>
        public Int32 Count
        {
            get { return InternalObjectStream.LengthInObjects; }
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
                return InternalObjectStream.Data;
            }
        }

        /// <summary>
        /// Gets a pointer to the beginning of the stream's internal data buffer.
        /// </summary>
        public IntPtr Data0
        {
            get
            {
                return InternalObjectStream.Data0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the stream contains commands which change the style of the rendered font 
        /// </summary>
        /// <remarks>This value will be <see langword="true"/> if the stream contains any 
        /// <see cref="TextLayoutCommandType.ToggleBold"/>,
        /// <see cref="TextLayoutCommandType.ToggleItalic"/>,
        /// <see cref="TextLayoutCommandType.PushFont"/>, 
        /// <see cref="TextLayoutCommandType.PushStyle"/>, 
        /// <see cref="TextLayoutCommandType.PopFont"/>, or 
        /// <see cref="TextLayoutCommandType.PopStyle"/> commands.
        /// </remarks>
        public Boolean HasMultipleFontStyles { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the command stream has acquired pointers to its underlying buffers.
        /// </summary>
        public Boolean HasAcquiredPointers
        {
            get { return InternalObjectStream.HasAcquiredPointers; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CursorPosition"/> property
        /// currently has a defined value.
        /// </summary>
        public Boolean CursorPositionSpecified
        {
            get { return CursorPosition.HasValue; }
        }

        /// <summary>
        /// Gets a value indicating whether the cursor position (as specified by <see cref="CursorPosition"/>
        /// is currently within the stream's bounds.
        /// </summary>
        public Boolean CursorPositionWithinBounds
        {
            get { return CursorPosition.HasValue && Bounds.Contains(CursorPosition.Value); }
        }
        
        /// <summary>
        /// Gets or sets the position of the mouse cursor relative to the command stream's layout area.
        /// </summary>
        public Point2? CursorPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the index of the stream's currently activated link.
        /// </summary>
        public Int16? ActiveLinkIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a shaped string builder used to assemble shaped tokens for this command stream.
        /// </summary>
        /// <returns>The <see cref="ShapedStringBuilder"/> used to assemble tokens for this command stream.</returns>
        internal ShapedStringBuilder GetShapedStringBuilder() =>
            (shapedStringBuilder = shapedStringBuilder ?? new ShapedStringBuilder());

        /// <summary>
        /// Gets the <see cref="UnsafeObjectStream"/> which provides the command stream's storage.
        /// </summary>
        internal UnsafeObjectStream InternalObjectStream { get; } = new UnsafeObjectStream(32, 256);

        // Registry of content resources used by the stream.
        private TextLayoutCommandStreamResources resources;
        private ShapedStringBuilder shapedStringBuilder;
    }
}
