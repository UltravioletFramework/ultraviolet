using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    partial class StringFormatter
    {
        /// <summary>
        /// Implements the built-in "hex" command used by <see cref="StringFormatter"/> to output hexadecimal values
        /// values with the specified number of digits.
        /// </summary>
        private sealed class HexCommandHandler : StringFormatterCommandHandler
        {
            private const String HexDigitsU = "0123456789ABCDEF";
            private const String HexDigitsL = "0123456789abcdef";

            /// <inheritdoc/>
            public override Boolean CanHandleCommand(StringSegment name)
            {
                return name == "hex";
            }

            /// <inheritdoc/>
            public override void HandleCommandBoolean(StringFormatter formatter, 
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Boolean value)
            {
                ConcatHexString(output, ref arguments, value ? 1UL : 0UL, sizeof(Boolean));
            }

            /// <inheritdoc/>
            public override void HandleCommandChar(StringFormatter formatter, 
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Char value)
            {
                ConcatHexString(output, ref arguments, value, sizeof(Char));
            }

            /// <inheritdoc/>
            public override void HandleCommandByte(StringFormatter formatter, 
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Byte value)
            {
                ConcatHexString(output, ref arguments, value, sizeof(Byte));
            }

            /// <inheritdoc/>
            public override void HandleCommandInt16(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Int16 value)
            {
                ConcatHexString(output, ref arguments, (UInt64)value, sizeof(Int16));
            }

            /// <inheritdoc/>
            public override void HandleCommandInt32(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Int32 value)
            {
                ConcatHexString(output, ref arguments, (UInt64)value, sizeof(Int32));
            }

            /// <inheritdoc/>
            public override void HandleCommandInt64(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Int64 value)
            {
                ConcatHexString(output, ref arguments, (UInt64)value, sizeof(Int64));
            }
            
            /// <inheritdoc/>
            public override void HandleCommandUInt16(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, UInt16 value)
            {
                ConcatHexString(output, ref arguments, value, sizeof(UInt16));
            }

            /// <inheritdoc/>
            public override void HandleCommandUInt32(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, UInt32 value)
            {
                ConcatHexString(output, ref arguments, value, sizeof(UInt32));
            }

            /// <inheritdoc/>
            public override void HandleCommandUInt64(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, UInt64 value)
            {
                ConcatHexString(output, ref arguments, value, sizeof(UInt64));
            }

            /// <inheritdoc/>
            public override void HandleCommandSingle(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Single value)
            {
                unsafe
                {
                    ConcatHexString(output, ref arguments, *(UInt32*)&value, sizeof(Single));
                }
            }

            /// <inheritdoc/>
            public override void HandleCommandDouble(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Double value)
            {
                unsafe
                {
                    ConcatHexString(output, ref arguments, *(UInt64*)&value, sizeof(Double));
                }
            }

            /// <summary>
            /// Concatenates the specified value to the output buffer as a hexadecimal string with the specified minimum number of digits.
            /// </summary>
            private static void ConcatHexString(StringBuilder output, ref StringFormatterCommandArguments arguments, UInt64 value, Int32 sizeInBytes)
            {
                var upper = true;
                if (arguments.Count > 0)
                {
                    var arg = arguments.GetArgument(0);
                    if (arg.Text.Equals("L") || arg.Text.Equals("l"))
                    {
                        upper = false;
                    }
                }

                var digits = upper ? HexDigitsU : HexDigitsL;
                var digitsCount = sizeInBytes * 2;
                var digitsIndex = output.Length;
                output.Length += digitsCount;

                for (var i = digitsCount - 1; i >= 0; i--, value >>= 4)
                    output[digitsIndex + i] = digits[(Int32)(value & 0xF)];
            }
        }
    }
}
