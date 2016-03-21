using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace TwistedLogik.Nucleus.Text
{
    /// <summary>
    /// Contains methods for formatting the contents of a <see cref="StringBuilder"/> object without
    /// generating any allocations to the managed heap.
    /// </summary>
    public unsafe class StringFormatter
    {
        /// <summary>
        /// Represents a method that is used to handle a particular argument type.
        /// </summary>
        private delegate void StringFormatterArgumentHandler(StringFormatter formatter,
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmdInfo);

        /// <summary>
        /// Represents an argument that has been added to the formatter.
        /// </summary>
        [SecuritySafeCritical]
        private unsafe struct StringFormatterArgument
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
            AddArgumentHandler<UInt16>(ArgumentHandler_UInt16);
            AddArgumentHandler<UInt32>(ArgumentHandler_UInt32);
            AddArgumentHandler<Single>(ArgumentHandler_Single);
            AddArgumentHandler<Double>(ArgumentHandler_Double);
            AddArgumentHandler<LocalizedString>(ArgumentHandler_LocalizedString);
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
                throw new InvalidOperationException(NucleusStrings.FmtCmdHandlerAlreadyRegistered);

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
        [SecuritySafeCritical]
        public void AddArgument(Boolean value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Boolean), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Byte value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Byte), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Char value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Char), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Int16 value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Int16), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Int32 value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Int32), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [CLSCompliant(false)]
        [SecuritySafeCritical]
        public void AddArgument(UInt16 value)
        {
            arguments.Add(new StringFormatterArgument(typeof(UInt16), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [CLSCompliant(false)]
        [SecuritySafeCritical]
        public void AddArgument(UInt32 value)
        {
            arguments.Add(new StringFormatterArgument(typeof(UInt32), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Single value)
        {
            arguments.Add(new StringFormatterArgument(typeof(Single), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
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
        /// Converts a character that represents a decimal digit into an integer value.
        /// </summary>
        /// <param name="digit">The digit to convert.</param>
        /// <param name="value">The value that the digit represents.</param>
        /// <returns><c>true</c> if the digit was successfully converted; otherwise, <c>false</c>.</returns>
        private static bool ConvertDecimalDigit(Char digit, out Int32 value)
        {
            switch (digit)
            {
                case '0': value = 0; return true;
                case '1': value = 1; return true;
                case '2': value = 2; return true;
                case '3': value = 3; return true;
                case '4': value = 4; return true;
                case '5': value = 5; return true;
                case '6': value = 6; return true;
                case '7': value = 7; return true;
                case '8': value = 8; return true;
                case '9': value = 9; return true;
            }
            value = 0;
            return false;
        }

        /// <summary>
        /// Converts the specified substring into a numeric value.
        /// </summary>
        /// <param name="input">The string that contains the substring to convert.</param>
        /// <param name="ix">The index of the first character in the substring.</param>
        /// <param name="length">The total number of characters in the substring.</param>
        /// <param name="value">The value that was converted from the substring.</param>
        /// <returns><c>true</c> if the substring was successfully converted; otherwise, <c>false</c>.</returns>
        private static bool ConvertInteger(String input, Int32 ix, Int32 length, out Int32 value)
        {
            value = 0;
            var magnitude = (int)Math.Pow(10, length - 1);
            var digit = 0;
            var end = ix + length;
            for (int j = ix; j < end; j++)
            {
                if (!ConvertDecimalDigit(input[j], out digit))
                    return false;
                value += (magnitude * digit);
                magnitude /= 10;
            }
            return true;
        }

        /// <summary>
        /// Gets the specified formatter argument.
        /// </summary>
        /// <param name="ix">The index of the formatter argument to retrieve.</param>
        /// <returns>The formatter argument with the specified index..</returns>
        private StringFormatterArgument GetArgument(Int32 ix)
        {
            if (ix < 0 || ix >= arguments.Count)
                throw new FormatException(NucleusStrings.FmtArgumentOutOfRange);
            return arguments[ix];
        }

        /// <summary>
        /// Gets a value indicating whether the character at the specified index within the string 
        /// is the beginning of a format specifier.
        /// </summary>
        /// <param name="str">The string to evaluate.</param>
        /// <param name="ix">The index to evaluate.</param>
        /// <param name="length">The format specifier's length.</param>
        /// <returns><c>true</c> if the specified index is the beginning of a format specifier; otherwise, <c>false</c>.</returns>
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
        /// <returns><c>true</c> if the specified index is an escaped format specifier; otherwise, <c>false</c>.</returns>
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

            // If the command starts with a reference to an argument, parse 
            // that out so we can get at the command name.
            var referencesArgument = ConvertInteger(arg0Text.SourceString, arg0Text.Start, arg0Text.Length, out arg0Value);
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

            // The ? operator is a special case: it's assuming that whatever we output is going
            // to be generated by a match rule, so treat it like a non-existent localized string.
            if (cmdName == "?")
            {
                var matchCommandInfo = 
                    new StringFormatterCommandInfo(cmdName, cmdArgs, null);

                argument = new StringFormatterArgument();
                ArgumentHandler_LocalizedString(this, input, output, arg0Text.Start + arg0Text.Length, ref argument, ref matchCommandInfo);
                return;
            }

            // Determine if we have a custom handler for this command.
            var matchingCommandHandler = default(StringFormatterCommandHandler);
            for (int i = commandHandlers.Count - 1; i >= 0; i--)
            {
                var commandHandler = commandHandlers[i];
                if (commandHandler.CanHandleCommand(cmdName))
                {
                    matchingCommandHandler = commandHandler;
                    break;
                }
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
            argument = GetArgument(arg0Value);
            AppendArgument(input, output, arg0Text.Start + arg0Text.Length, ref argument, ref cmdInfo);
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
        /// Reads a command integer starting at the specified position in the input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="position">The current position in the input string.</param>
        /// <param name="value">The value that was read.</param>
        /// <returns><c>true</c> if a value was read; otherwise, <c>false</c>.</returns>
        private static bool ReadCommandInteger(String input, ref Int32 position, out Int32 value)
        {
            value = 0;
            if (input[position] != ':')
                return false;
            position++;

            var start = position;
            while (input[position] != ':' && input[position] != '}')
                position++;

            return ConvertInteger(input, start, position - start, out value);
        }

        /// <summary>
        /// Reads a command string starting at the specified position in the input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="position">The current position in the input string.</param>
        /// <param name="value">The value that was read.</param>
        /// <returns><c>true</c> if a value was read; otherwise, <c>false</c>.</returns>
        private static bool ReadCommandString(String input, ref Int32 position, out StringSegment value)
        {
            if (input[position] != ':')
            {
                value = default(StringSegment);
                return false;
            }
            position++;

            var start = position;
            while (input[position] != ':' && input[position] != '}')
                position++;

            value = new StringSegment(input, start, position - start);
            return true;
        }

        /// <summary>
        /// Adds an argument handler to the argument handler registry.
        /// </summary>
        [SecuritySafeCritical]
        private static void AddArgumentHandler<T>(StringFormatterArgumentHandler handler)
        {
            argumentHandlers[typeof(T).TypeHandle.Value.ToInt64()] = handler;
        }

        /// <summary>
        /// Handles <see cref="Boolean"/> formatter arguments.
        /// </summary>
        [SecuritySafeCritical]
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
        [SecuritySafeCritical]
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
        [SecuritySafeCritical]
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
        [SecuritySafeCritical]
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
                ArgumentHandler_SignedInteger<Int16>(formatter, input, output, position, value);
            }
        }

        /// <summary>
        /// Handles <see cref="Int32"/> formatter arguments.
        /// </summary>
        [SecuritySafeCritical]
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
                ArgumentHandler_SignedInteger<Int32>(formatter, input, output, position, value);
            }
        }

        /// <summary>
        /// Handles <see cref="Int16"/> and <see cref="Int32"/> formatter arguments.
        /// </summary>
        [SecuritySafeCritical]
        private static void ArgumentHandler_SignedInteger<T>(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, Int32 value)
        {
            Int32 commands = 0;
            StringSegment command;
            while (ReadCommandString(input, ref position, out command))
            {
                commands++;
                if (command.Equals("pad"))
                {
                    var pad = 0;
                    if (ReadCommandInteger(input, ref position, out pad))
                    {
                        output.Concat(value, (uint)pad);
                    }
                    else throw new FormatException(NucleusStrings.FmtCmdParseFailure.Format("pad"));
                }
                else throw new FormatException(NucleusStrings.FmtCmdInvalidForArgument.Format(command, typeof(T)));
            }
            if (commands == 0)
                output.Concat(value);
        }

        /// <summary>
        /// Handles <see cref="UInt16"/> formatter arguments.
        /// </summary>
        [SecuritySafeCritical]
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
                ArgumentHandler_UnsignedInteger<UInt16>(formatter, input, output, position, value);
            }
        }

        /// <summary>
        /// Handles <see cref="UInt32"/> formatter arguments.
        /// </summary>
        [SecuritySafeCritical]
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
                ArgumentHandler_UnsignedInteger<UInt32>(formatter, input, output, position, value);
            }
        }

        /// <summary>
        /// Handles <see cref="UInt16"/> and <see cref="UInt32"/> formatter arguments.
        /// </summary>
        [SecuritySafeCritical]
        private static void ArgumentHandler_UnsignedInteger<T>(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, UInt32 value)
        {
            Int32 commands = 0;
            StringSegment command;
            while (ReadCommandString(input, ref position, out command))
            {
                commands++;
                if (command.Equals("pad"))
                {
                    var pad = 0;
                    if (ReadCommandInteger(input, ref position, out pad))
                    {
                        output.Concat(value, (uint)pad);
                    }
                    else throw new FormatException(NucleusStrings.FmtCmdParseFailure.Format("pad"));
                }
                else throw new FormatException(NucleusStrings.FmtCmdInvalidForArgument.Format(command, typeof(T)));
            }
            if (commands == 0)
                output.Concat(value);
        }

        /// <summary>
        /// Handles <see cref="Single"/> formatter arguments.
        /// </summary>
        [SecuritySafeCritical]
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
                ArgumentHandler_FloatingPoint<Single>(formatter, input, output, position, value);
            }
        }

        /// <summary>
        /// Handles <see cref="Double"/> formatter arguments.
        /// </summary>
        [SecuritySafeCritical]
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
                ArgumentHandler_FloatingPoint<Single>(formatter, input, output, position, (float)value);
            }
        }

        /// <summary>
        /// Handles <see cref="Single"/> and <see cref="Double"/> formatter arguments.
        /// </summary>
        [SecuritySafeCritical]
        private static void ArgumentHandler_FloatingPoint<T>(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, Single value)
        {
            Int32 commands = 0;
            StringSegment command;
            while (ReadCommandString(input, ref position, out command))
            {
                commands++;
                if (command.Equals("decimals"))
                {
                    var decimals = 0;
                    if (ReadCommandInteger(input, ref position, out decimals))
                    {
                        output.Concat(value, (uint)decimals);
                    }
                    else throw new FormatException(NucleusStrings.FmtCmdParseFailure.Format("decimals"));
                }
                else throw new FormatException(NucleusStrings.FmtCmdInvalidForArgument.Format(command, typeof(T)));
            }
            if (commands == 0)
                output.Concat(value);
        }

        /// <summary>
        /// Handles <see cref="LocalizedString"/> formatter arguments.
        /// </summary>
        [SecuritySafeCritical]
        private static void ArgumentHandler_LocalizedString(StringFormatter formatter, 
            String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument, ref StringFormatterCommandInfo cmd)
        {
            var localizedString = (LocalizedString)argument.Reference;

            if (cmd.CommandHandler != null)
            {
                cmd.CommandHandler.HandleCommandLocalizedString(formatter,
                    output, cmd.CommandName, cmd.CommandArguments, localizedString);
                return;
            }

            Int32 commands = 0;
            StringSegment command;
            while (ReadCommandString(input, ref position, out command))
            {
                commands++;
                if (command.Equals("variant"))
                {
                    // Make sure this isn't a generated string.
                    if (localizedString == null)
                        throw new FormatException(NucleusStrings.FmtCmdInvalidForGeneratedStrings.Format("variant"));
                    
                    // Output the specified variant.
                    StringSegment variant;
                    if (ReadCommandString(input, ref position, out variant))
                    {
                        output.Append(localizedString.GetVariant(ref variant));
                        return;
                    }
                    else throw new FormatException(NucleusStrings.FmtCmdParseFailure.Format("variant"));
                }
                else if (command.Equals("match"))
                {
                    Int32 targetArgumentIndex;
                    StringSegment targetMatchRule;
                    if (ReadCommandInteger(input, ref position, out targetArgumentIndex) && ReadCommandString(input, ref position, out targetMatchRule))
                    {
                        // Make sure our target is a localized string variant.
                        var target = formatter.GetArgument(targetArgumentIndex);
                        if (target.Reference == null || !(target.Reference is LocalizedStringVariant))
                            throw new FormatException(NucleusStrings.FmtCmdInvalidForArgument.Format("match", target.Reference.GetType()));

                        // Run the specified match evaluator.
                        var match = Localization.MatchVariant(localizedString, (LocalizedStringVariant)target.Reference, targetMatchRule);
                        if (match != null)
                            output.Append(match);
                        return;
                    }
                    else throw new FormatException(NucleusStrings.FmtCmdParseFailure.Format("match"));
                }
                else throw new FormatException(NucleusStrings.FmtCmdInvalidForArgument.Format(command, typeof(LocalizedString)));
            }
            if (commands == 0)
                output.Append((string)localizedString ?? "???");
        }

        // Registered argument handlers.
        private static readonly Dictionary<Int64, StringFormatterArgumentHandler> argumentHandlers = 
            new Dictionary<Int64, StringFormatterArgumentHandler>();

        // Formatter state.
        private readonly List<StringFormatterArgument> arguments =
            new List<StringFormatterArgument>();
        private readonly List<StringFormatterCommandHandler> commandHandlers =
            new List<StringFormatterCommandHandler>();
    }
}
