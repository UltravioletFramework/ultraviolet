using System;
using System.Collections.Generic;
using System.Text;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Contains methods for formatting the contents of a <see cref="StringBuilder"/> object without
    /// generating any allocations to the managed heap.
    /// </summary>
    public unsafe partial class StringFormatter
    {
        /// <summary>
        /// Represents a method that is used to handle a particular argument type.
        /// </summary>
        private delegate void StringFormatterArgumentHandler(StringFormatter formatter,
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmdInfo);

        /// <summary>
        /// Represents an argument that has been added to the formatter.
        /// </summary>
        private struct StringFormatterArgument
        {
            public StringFormatterArgument(Object obj)
            {
                Reference = obj;
                Value = 0;
                Type = (obj == null) ? 0 : obj.GetType().TypeHandle.Value.ToInt64();
            }
            public StringFormatterArgument(Type type, ulong value)
            {
                Reference = null;
                Value = value;
                Type = type.TypeHandle.Value.ToInt64();
            }
            public readonly object Reference;
            public readonly ulong Value;
            public readonly long Type;
        }

        /// <summary>
        /// Initializes the <see cref="StringFormatter"/> type.
        /// </summary>
        static StringFormatter()
        {
            AddArgumentHandler<Boolean>(ArgumentHandler_Boolean);
            AddArgumentHandler<Byte>(ArgumentHandler_Byte);
            AddArgumentHandler<Char>(ArgumentHandler_Char);
            AddArgumentHandler<Int16>(ArgumentHandler_Int16);
            AddArgumentHandler<Int32>(ArgumentHandler_Int32);
            AddArgumentHandler<Int64>(ArgumentHandler_Int64);
            AddArgumentHandler<UInt16>(ArgumentHandler_UInt16);
            AddArgumentHandler<UInt32>(ArgumentHandler_UInt32);
            AddArgumentHandler<UInt64>(ArgumentHandler_UInt64);
            AddArgumentHandler<Single>(ArgumentHandler_Single);
            AddArgumentHandler<Double>(ArgumentHandler_Double);
            AddArgumentHandler<LocalizedString>(ArgumentHandler_LocalizedString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringFormatter"/> class.
        /// </summary>
        public StringFormatter()
        {
            RegisterCommandHandler(stdCmd_Pad);
            RegisterCommandHandler(stdCmd_Decimals);
            RegisterCommandHandler(stdCmd_Hex);
            RegisterCommandHandler(stdCmd_Variant);
            RegisterCommandHandler(stdCmd_Match);
        }

        /// <summary>
        /// Resets the string formatter's state.
        /// </summary>
        public void Reset()
        {
            arguments.Clear();
        }

        /// <summary>
        /// Registers the specified command handler with the formatter.
        /// </summary>
        /// <param name="handler">The command handler to register.</param>
        public void RegisterCommandHandler(StringFormatterCommandHandler handler)
        {
            Contract.Require(handler, nameof(handler));

            if (commandHandlers.Contains(handler))
                throw new InvalidOperationException(CoreStrings.FmtCmdHandlerAlreadyRegistered);

            commandHandlers.Add(handler);
        }

        /// <summary>
        /// Unregisters the specified command handler from the formatter.
        /// </summary>
        /// <param name="handler">The command handler to unregister.</param>
        public void UnregisterCommandHandler(StringFormatterCommandHandler handler)
        {
            Contract.Require(handler, nameof(handler));

            commandHandlers.Remove(handler);
        }

        /// <summary>
        /// Unregisters all of the formatter's command handlers.
        /// </summary>
        public void UnregisterAllCommandHandlers()
        {
            commandHandlers.Clear();
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(Boolean value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Boolean), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(Byte value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Byte), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(Char value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Char), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(Int16 value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Int16), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(Int32 value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Int32), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(Int64 value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Int64), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [CLSCompliant(false)]
        public void AddArgument(UInt16 value)
        {
            arguments.Add(new StringFormatterArgument(typeof(UInt16), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [CLSCompliant(false)]
        public void AddArgument(UInt32 value)
        {
            arguments.Add(new StringFormatterArgument(typeof(UInt32), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [CLSCompliant(false)]
        public void AddArgument(UInt64 value)
        {
            arguments.Add(new StringFormatterArgument(typeof(UInt64), value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(Single value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Single), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(Double value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Double), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(String value)
        {
            arguments.Add(new StringFormatterArgument(value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(LocalizedString value)
        {
            arguments.Add(new StringFormatterArgument(value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(LocalizedStringVariant value)
        {
            arguments.Add(new StringFormatterArgument(value));
        }

        /// <summary>
        /// Formats the content of the specified <see cref="StringBuilder"/> using the formatter's current list of arguments.
        /// </summary>
        /// <param name="input">A string specifying how to format the formatter's arguments.</param>
        /// <param name="output">The <see cref="StringBuilder"/> to which to write the formatted string.</param>
        public void Format(String input, StringBuilder output)
        {
            // Validate arguments.
            if (output == null)
                throw new ArgumentNullException("output");
            if (input == null)
                throw new ArgumentNullException("input");
            if (input.Length == 0)
                return;
            output.Length = 0;

            var position = 0;
            var length = 0;
            while (true)
            {
                if (position >= input.Length)
                    break;

                if (IsFormatSpecifier(input, position, out length))
                {
                    ProcessFormatSpecifier(input, output, position, length);
                    position += length;
                }
                else
                {
                    if (IsEscapedFormatSpecifier(input, position, out length))
                    {
                        output.Append(input[position]);
                        position += length;
                    }
                    else
                    {
                        output.Append(input[position]);
                        position += 1;
                    }
                }
            }
        }
        
        /// <summary>
        /// Gets the specified formatter argument.
        /// </summary>
        /// <param name="ix">The index of the formatter argument to retrieve.</param>
        /// <returns>The formatter argument with the specified index..</returns>
        private StringFormatterArgument GetArgument(Int32 ix)
        {
            if (ix < 0 || ix >= arguments.Count)
                throw new FormatException(CoreStrings.FmtArgumentOutOfRange);
            return arguments[ix];
        }

        /// <summary>
        /// Gets a value indicating whether the character at the specified index within the string 
        /// is the beginning of a format specifier.
        /// </summary>
        /// <param name="str">The string to evaluate.</param>
        /// <param name="ix">The index to evaluate.</param>
        /// <param name="length">The format specifier's length.</param>
        /// <returns><see langword="true"/> if the specified index is the beginning of a format specifier; otherwise, <see langword="false"/>.</returns>
        private Boolean IsFormatSpecifier(String str, Int32 ix, out Int32 length)
        {
            // If this doesn't start with a format specifier delimiter, then obviously it isn't a format specifier!
            if (str[ix] != '{' || IsEscapedFormatSpecifier(str, ix, out length))
            {
                length = 0;
                return false;
            }
            length = 0;

            // Read to the end of the specifier.
            var delimiters = 0;
            var end = ix;
            for (int pos = ix + 1; pos < str.Length; pos++)
            {
                if (str[pos] == '{') delimiters++;
                if (str[pos] == '}')
                {
                    if (delimiters > 0) delimiters--;
                    else
                    {
                        end = pos;
                        break;
                    }
                }
            }
            length = 1 + (end - ix);
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the character at the specified index within the string 
        /// is an escaped format specifier character ({ or }).
        /// </summary>
        /// <param name="str">The string to evaluate.</param>
        /// <param name="ix">The index to evaluate.</param>
        /// <param name="length">The escape sequence's length.</param>
        /// <returns><see langword="true"/> if the specified index is an escaped format specifier; otherwise, <see langword="false"/>.</returns>
        private Boolean IsEscapedFormatSpecifier(String str, Int32 ix, out Int32 length)
        {
            length = 0;
            if (ix + 1 >= str.Length)
                return false;

            if (str[ix] == '{')
            {
                length = 2;
                return str[ix + 1] == '{';
            }
            if (str[ix] == '}')
            {
                length = 2;
                return str[ix + 1] == '}';
            }
            return false;
        }

        /// <summary>
        /// Attempts to process a format specifier.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="ix">The index at which the specifier begins.</param>
        /// <param name="length">The length of the specifier.</param>
        private void ProcessFormatSpecifier(String input, StringBuilder output, Int32 ix, Int32 length)
        {
            // Parse the command string to pull out its name and arguments.
            var argument = default(StringFormatterArgument);
            var cmdStart = ix + 1;
            var cmdLength = 0;

            for (int i = 1; i < length; i++)
            {
                var c = input[ix + i];
                if (c == '}')
                {
                    break;
                }
                cmdLength++;
            }

            var cmdName = default(StringSegment);
            var cmdArgs = new StringFormatterCommandArguments(input, cmdStart, cmdLength);
            
            var arg0 = cmdArgs.GetArgument(0);
            var arg0Text = arg0.Text;
            var arg0Value = 0;
            var arg0ValueIsInteger = arg0.TryGetValueAsInt32(out arg0Value);

            // If the command starts with a reference to an argument, parse 
            // that out so we can get at the command name.
            var referencesGenerated = arg0Text == "?";
            var referencesArgument = referencesGenerated || arg0ValueIsInteger;
            if (referencesArgument)
            {
                if (cmdArgs.Count > 1)
                {
                    cmdName = cmdArgs.GetArgument(1).Text;
                    cmdArgs = cmdArgs.Discard(2);
                }
            }
            else
            {
                cmdName = arg0Text;
                cmdArgs = cmdArgs.Discard(1);
            }
            
            // Determine if we have a custom handler for this command.
            var matchingCommandHandler = default(StringFormatterCommandHandler);
            if (!cmdName.IsEmpty)
            {
                for (int i = commandHandlers.Count - 1; i >= 0; i--)
                {
                    var commandHandler = commandHandlers[i];
                    if (commandHandler.CanHandleCommand(cmdName))
                    {
                        matchingCommandHandler = commandHandler;
                        break;
                    }
                }

                if (matchingCommandHandler == null)
                    throw new FormatException(CoreStrings.FmtCmdUnrecognized.Format(cmdName));
            }

            // If we can't convert the first argument to an integer, just write out the whole command string.
            if (!referencesArgument)
            {
                if (matchingCommandHandler != null)
                {
                    matchingCommandHandler.HandleCommand(this, output, cmdName, cmdArgs);
                    return;
                }

                output.AppendSubstring(input, ix, length);
                return;
            }

            // Append the selected argument to the buffer.
            var cmdInfo = new StringFormatterCommandInfo(cmdName, cmdArgs, matchingCommandHandler);
            if (referencesGenerated)
            {
                argument = new StringFormatterArgument();
                ArgumentHandler_LocalizedString(this, 
                    input, output, arg0Text.Start + arg0Text.Length, ref argument, ref cmdInfo);
            }
            else
            {
                argument = GetArgument(arg0Value);
                AppendArgument(input, output, arg0Text.Start + arg0Text.Length, ref argument, ref cmdInfo);
            }
        }

        /// <summary>
        /// Appends a formatter argument to the specified output buffer.
        /// </summary>
        private void AppendArgument(String input, StringBuilder output, Int32 position,
            ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmdInfo)
        {
            StringFormatterArgumentHandler handler;
            if (argumentHandlers.TryGetValue(argument.Type, out handler))
            {
                handler(this, input, output, position, ref argument, ref cmdInfo);
            }
            else
            {
                if (argument.Reference != null)
                {
                    output.Append(argument.Reference.ToString());
                }
                else
                {
                    output.Append("???");
                }
            }
        }
        
        /// <summary>
        /// Adds an argument handler to the argument handler registry.
        /// </summary>
        private static void AddArgumentHandler<T>(StringFormatterArgumentHandler handler)
        {
            argumentHandlers[typeof(T).TypeHandle.Value.ToInt64()] = handler;
        }

        /// <summary>
        /// Handles <see cref="Boolean"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_Boolean(StringFormatter formatter,
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var raw = argument.Value;
            var value = *(bool*)&raw;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandBoolean(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, value);
            }
            else
            {
                output.Append(value ? Boolean.TrueString : Boolean.FalseString);
            }
        }

        /// <summary>
        /// Handles <see cref="Byte"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_Byte(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var raw = argument.Value;
            var value = *(byte*)&raw;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandByte(formatter, 
                    output, cmd.CommandName, cmd.CommandArguments, value);
            }
            else
            {
                output.Concat(value);
            }
        }

        /// <summary>
        /// Handles <see cref="Char"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_Char(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var raw = argument.Value;
            var value = *(char*)&raw;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandChar(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, value);
            }
            else
            {
                output.Append(value);
            }
        }

        /// <summary>
        /// Handles <see cref="Int16"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_Int16(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var raw = argument.Value;
            var value = *(Int16*)&raw;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandInt16(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, value);
            }
            else
            {
                output.Concat(value);
            }
        }

        /// <summary>
        /// Handles <see cref="Int32"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_Int32(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var raw = argument.Value;
            var value = *(Int32*)&raw;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandInt32(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, value);
            }
            else
            {
                output.Concat(value);
            }
        }

        /// <summary>
        /// Handles <see cref="Int64"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_Int64(StringFormatter formatter,
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var raw = argument.Value;
            var value = *(Int64*)&raw;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandInt64(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, value);
            }
            else
            {
                output.Concat(value);
            }
        }

        /// <summary>
        /// Handles <see cref="UInt16"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_UInt16(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var raw = argument.Value;
            var value = *(UInt16*)&raw;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandUInt16(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, value);
            }
            else
            {
                output.Concat(value);
            }
        }

        /// <summary>
        /// Handles <see cref="UInt32"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_UInt32(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var raw = argument.Value;
            var value = *(UInt32*)&raw;
            
            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandUInt32(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, value);
            }
            else
            {
                output.Concat(value);
            }
        }

        /// <summary>
        /// Handles <see cref="UInt64"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_UInt64(StringFormatter formatter,
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var raw = argument.Value;
            var value = *(UInt64*)&raw;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandUInt64(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, value);
            }
            else
            {
                output.Concat(value);
            }
        }

        /// <summary>
        /// Handles <see cref="Single"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_Single(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var raw = argument.Value;
            var value = *(Single*)&raw;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandSingle(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, value);
            }
            else
            {
                output.Concat(value);
            }
        }

        /// <summary>
        /// Handles <see cref="Double"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_Double(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var raw = argument.Value;
            var value = *(Double*)&raw;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandDouble(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, value);
            }
            else
            {
                output.Concat((Single)value);
            }
        }
        
        /// <summary>
        /// Handles <see cref="LocalizedString"/> formatter arguments.
        /// </summary>
        private static void ArgumentHandler_LocalizedString(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var localizedString = (LocalizedString)argument.Reference;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandLocalizedString(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, localizedString);
            }
            else
            {
                output.Append((String)localizedString ?? "???");
            }
        }

        // Registered argument handlers.
        private static readonly Dictionary<Int64, StringFormatterArgumentHandler> argumentHandlers = 
            new Dictionary<Int64, StringFormatterArgumentHandler>();

        // Formatter state.
        private readonly List<StringFormatterArgument> arguments =
            new List<StringFormatterArgument>();
        private readonly List<StringFormatterCommandHandler> commandHandlers =
            new List<StringFormatterCommandHandler>();

        // Standard command handlers.
        private static readonly StringFormatterCommandHandler stdCmd_Pad = new PadCommandHandler();
        private static readonly StringFormatterCommandHandler stdCmd_Decimals = new DecimalsCommandHandler();
        private static readonly StringFormatterCommandHandler stdCmd_Hex = new HexCommandHandler();
        private static readonly StringFormatterCommandHandler stdCmd_Variant = new VariantCommandHandler();
        private static readonly StringFormatterCommandHandler stdCmd_Match = new MatchCommandHandler();
    }
}
