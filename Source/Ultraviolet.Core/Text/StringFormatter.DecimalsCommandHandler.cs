using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    partial class StringFormatter
    {
        /// <summary>
        /// Implements the built-in "decimals" command used by <see cref="StringFormatter"/> to output floating-point
        /// values with the specified number of decimal places.
        /// </summary>
        private sealed class DecimalsCommandHandler : StringFormatterCommandHandler
        {
            /// <inheritdoc/>
            public override Boolean CanHandleCommand(StringSegment name)
            {
                return name == "decimals";
            }

            /// <inheritdoc/>
            public override void HandleCommandSingle(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Single value)
            {
                var decimals = arguments.GetArgument(0).GetValueAsInt32();
                output.Concat(value, (UInt32)decimals);
            }

            /// <inheritdoc/>
            public override void HandleCommandDouble(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, Double value)
            {
                var decimals = arguments.GetArgument(0).GetValueAsInt32();
                output.Concat((Single)value, (UInt32)decimals);
            }
        }
    }
}
