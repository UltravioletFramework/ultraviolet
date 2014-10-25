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
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument being handled.</param>
        private delegate void StringFormatterArgumentHandler(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument);

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
        public StringFormatter()
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
            Arguments.Clear();
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Boolean value)
        {
            Arguments.Add(new StringFormatterArgument(typeof(Boolean), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Byte value)
        {
            Arguments.Add(new StringFormatterArgument(typeof(Byte), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Char value)
        {
            Arguments.Add(new StringFormatterArgument(typeof(Char), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Int16 value)
        {
            Arguments.Add(new StringFormatterArgument(typeof(Int16), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Int32 value)
        {
            Arguments.Add(new StringFormatterArgument(typeof(Int32), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [CLSCompliant(false)]
        [SecuritySafeCritical]
        public void AddArgument(UInt16 value)
        {
            Arguments.Add(new StringFormatterArgument(typeof(UInt16), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [CLSCompliant(false)]
        [SecuritySafeCritical]
        public void AddArgument(UInt32 value)
        {
            Arguments.Add(new StringFormatterArgument(typeof(UInt32), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Single value)
        {
            Arguments.Add(new StringFormatterArgument(typeof(Single), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        [SecuritySafeCritical]
        public void AddArgument(Double value)
        {
            Arguments.Add(new StringFormatterArgument(typeof(Double), *(ulong*)&value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(String value)
        {
            Arguments.Add(new StringFormatterArgument(value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(LocalizedString value)
        {
            Arguments.Add(new StringFormatterArgument(value));
        }

        /// <summary>
        /// Adds an argument to the string formatter.
        /// </summary>
        /// <param name="value">The value to add as an argument.</param>
        public void AddArgument(LocalizedStringVariant value)
        {
            Arguments.Add(new StringFormatterArgument(value));
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
            if (ix < 0 || ix >= Arguments.Count)
                throw new FormatException(NucleusStrings.FmtArgumentOutOfRange);
            return Arguments[ix];
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
            // Find the referenced argument index.
            StringFormatterArgument argument;
            var argumentIndex = -1;
            var argumentStart = 0;
            var argumentLength = 0;
            for (int i = 1; i < length; i++)
            {
                var c = input[ix + i];
                if (c == ':' || c == '}')
                {
                    argumentStart = ix + 1;
                    argumentLength = (ix + i) - argumentStart;
                    break;
                }
            }
            if (argumentLength == 1 && input[argumentStart] == '?')
            {
                // The ? operator is a special case: it's assuming that whatever we output is going
                // to be generated by a match rule, so treat it like a non-existent localized string.
                argument = new StringFormatterArgument();
                ArgumentHandler_LocalizedString(input, output, argumentStart + argumentLength, ref argument);
                return;
            }
            else
            {
                if (!ConvertInteger(input, argumentStart, argumentLength, out argumentIndex))
                {
                    output.AppendSubstring(input, ix, length);
                    return;
                }
            }
            
            // Append the selected argument to the buffer.
            argument = GetArgument(argumentIndex);
            AppendArgument(input, output, argumentStart + argumentLength, ref argument);
        }

        /// <summary>
        /// Appends a formatter argument to the specified output buffer.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument to append to the output buffer.</param>
        private void AppendArgument(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument)
        {
            StringFormatterArgumentHandler handler;
            if (ArgumentHandlers.TryGetValue(argument.Type, out handler))
            {
                handler(input, output, position, ref argument);
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
        /// <param name="handler">The handler to add to the registry.</param>
        [SecuritySafeCritical]
        private void AddArgumentHandler<T>(StringFormatterArgumentHandler handler)
        {
            ArgumentHandlers[typeof(T).TypeHandle.Value.ToInt64()] = handler;
        }

        /// <summary>
        /// Handles Boolean formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument being handled.</param>
        [SecuritySafeCritical]
        private void ArgumentHandler_Boolean(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument)
        {
            var raw = argument.Value;
            var value = *(bool*)&raw;
            output.Append(value ? Boolean.TrueString : Boolean.FalseString);
        }

        /// <summary>
        /// Handles Byte formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument being handled.</param>
        [SecuritySafeCritical]
        private void ArgumentHandler_Byte(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument)
        {
            var raw = argument.Value;
            var value = *(byte*)&raw;
            output.Concat((int)value);
        }

        /// <summary>
        /// Handles Char formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument being handled.</param>
        [SecuritySafeCritical]
        private void ArgumentHandler_Char(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument)
        {
            var raw = argument.Value;
            var value = *(char*)&raw;
            output.Append(value);
        }

        /// <summary>
        /// Handles Int16 formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument being handled.</param>
        [SecuritySafeCritical]
        private void ArgumentHandler_Int16(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument)
        {
            var raw = argument.Value;
            var value = *(Int16*)&raw;
            ArgumentHandler_SignedInteger<Int16>(input, output, position, value);
        }

        /// <summary>
        /// Handles Int32 formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument being handled.</param>
        [SecuritySafeCritical]
        private void ArgumentHandler_Int32(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument)
        {
            var raw = argument.Value;
            var value = *(Int32*)&raw;
            ArgumentHandler_SignedInteger<Int32>(input, output, position, value);
        }

        /// <summary>
        /// Handles Int16 and Int32 formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="value">The value of the argument being handled.</param>
        [SecuritySafeCritical]
        private void ArgumentHandler_SignedInteger<T>(String input, StringBuilder output, Int32 position, Int32 value)
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
        /// Handles UInt16 formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument being handled.</param>
        [SecuritySafeCritical]
        private void ArgumentHandler_UInt16(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument)
        {
            var raw = argument.Value;
            var value = *(UInt16*)&raw;
            ArgumentHandler_UnsignedInteger<UInt16>(input, output, position, value);
        }

        /// <summary>
        /// Handles UInt32 formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument being handled.</param>
        [SecuritySafeCritical]
        private void ArgumentHandler_UInt32(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument)
        {
            var raw = argument.Value;
            var value = *(UInt32*)&raw;
            ArgumentHandler_UnsignedInteger<UInt32>(input, output, position, value);
        }

        /// <summary>
        /// Handles UInt16 and UInt32 formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="value">The value of the argument being handled.</param>
        [SecuritySafeCritical]
        private void ArgumentHandler_UnsignedInteger<T>(String input, StringBuilder output, Int32 position, UInt32 value)
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
        /// Handles Single formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument being handled.</param>
        [SecuritySafeCritical]
        private void ArgumentHandler_Single(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument)
        {
            var raw = argument.Value;
            var value = *(Single*)&raw;
            ArgumentHandler_FloatingPoint<Single>(input, output, position, value);
        }

        /// <summary>
        /// Handles Double formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument being handled.</param>
        [SecuritySafeCritical]
        private void ArgumentHandler_Double(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument)
        {
            var raw = argument.Value;
            var value = *(Double*)&raw;
            ArgumentHandler_FloatingPoint<Double>(input, output, position, (float)value);
        }

        /// <summary>
        /// Handles Single and Double formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="value">The value of the argument being handled.</param>
        private void ArgumentHandler_FloatingPoint<T>(String input, StringBuilder output, Int32 position, Single value)
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
        /// Handles LocalizedString formatter arguments.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="output">The output buffer.</param>
        /// <param name="position">The current position within the input string.</param>
        /// <param name="argument">The argument being handled.</param>
        private void ArgumentHandler_LocalizedString(String input, StringBuilder output, Int32 position, ref StringFormatterArgument argument)
        {
            var localizedString = (LocalizedString)argument.Reference;

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
                        var target = GetArgument(targetArgumentIndex);
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

        // Formatter state.
        private readonly Dictionary<Int64, StringFormatterArgumentHandler> ArgumentHandlers = new Dictionary<Int64, StringFormatterArgumentHandler>();
        private readonly List<StringFormatterArgument> Arguments = new List<StringFormatterArgument>();
    }
}
