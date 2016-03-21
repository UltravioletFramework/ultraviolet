using System;
using System.Text;

namespace TwistedLogik.Nucleus.Text
{
    partial class StringFormatter
    {
        /// <summary>
        /// Implements the built-in "pad" command used by <see cref="StringFormatter"/> to output padded integers.
        /// </summary>
        private sealed class PadCommandHandler : StringFormatterCommandHandler
        {
            /// <inheritdoc/>
            public override Boolean CanHandleCommand(StringSegment name)
            {
                return name == "pad";
            }

            /// <inheritdoc/>
            public override void HandleCommandUInt16(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, UInt16 value)
            {
                var padding = arguments.GetArgument(0).GetValueAsInt32();
                output.Concat(value, (UInt32)padding);
            }

            /// <inheritdoc/>
            public override void HandleCommandInt32(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Int32 value)
            {
                var padding = arguments.GetArgument(0).GetValueAsInt32();
                output.Concat(value, (UInt32)padding);
            }

            /// <inheritdoc/>
            public override void HandleCommandInt16(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Int16 value)
            {
                var padding = arguments.GetArgument(0).GetValueAsInt32();
                output.Concat(value, (UInt32)padding);
            }

            /// <inheritdoc/>
            public override void HandleCommandUInt32(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, UInt32 value)
            {
                var padding = arguments.GetArgument(0).GetValueAsInt32();
                output.Concat(value, (UInt32)padding);
            }
        }
    }
}
