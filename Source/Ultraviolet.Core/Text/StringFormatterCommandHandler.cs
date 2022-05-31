using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a custom command handler for the <see cref="StringFormatter"/> class.
    /// </summary>
    public abstract class StringFormatterCommandHandler
    {
        /// <summary>
        /// Gets a value indicating whether this command handler can handle
        /// a command with the specified name.
        /// </summary>
        /// <param name="name">The name of the command to evaluate.</param>
        /// <returns><see langword="true"/> if the handler can handle the specified command; 
        /// otherwise, <see langword="false"/>.</returns>
        public abstract Boolean CanHandleCommand(StringSegment name);

        /// <summary>
        /// Handles a command which does not have an associated formatter argument value (that is, a command
        /// in the format of {foo} rather than {0:foo}).
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        public virtual void HandleCommand(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidWithoutArgument.Format(command)); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="Boolean"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        public virtual void HandleCommandBoolean(StringFormatter formatter, StringBuilder output, 
            StringSegment command, StringFormatterCommandArguments arguments, Boolean value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(Boolean))); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="Byte"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        public virtual void HandleCommandByte(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments, Byte value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(Byte))); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="Char"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        public virtual void HandleCommandChar(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments, Char value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(Char))); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="Int16"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        public virtual void HandleCommandInt16(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments, Int16 value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(Int16))); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="Int32"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        public virtual void HandleCommandInt32(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments, Int32 value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(Int32))); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="Int64"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        public virtual void HandleCommandInt64(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments, Int64 value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(Int64))); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="UInt16"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        [CLSCompliant(false)]
        public virtual void HandleCommandUInt16(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments, UInt16 value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(UInt16))); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="UInt32"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        [CLSCompliant(false)]
        public virtual void HandleCommandUInt32(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments, UInt32 value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(UInt32))); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="UInt64"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        [CLSCompliant(false)]
        public virtual void HandleCommandUInt64(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments, UInt64 value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(UInt64))); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="Single"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        [CLSCompliant(false)]
        public virtual void HandleCommandSingle(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments, Single value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(Single))); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="Double"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        [CLSCompliant(false)]
        public virtual void HandleCommandDouble(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments, Double value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(Double))); }

        /// <summary>
        /// Handles a command which has an associated formatter argument value of <see cref="LocalizedString"/> type.
        /// </summary>
        /// <param name="formatter">The formatter which is parsing the command.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="command">The name of the command being handled.</param>
        /// <param name="arguments">The arguments for the command being handled.</param>
        /// <param name="value">The command's associated value.</param>
        [CLSCompliant(false)]
        public virtual void HandleCommandLocalizedString(StringFormatter formatter, StringBuilder output,
            StringSegment command, StringFormatterCommandArguments arguments, LocalizedString value)
        { throw new NotSupportedException(CoreStrings.FmtCmdInvalidForArgument.Format(command, typeof(LocalizedString))); }
    }
}
