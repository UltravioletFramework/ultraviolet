using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    partial class StringFormatter
    {
        /// <summary>
        /// Implements the built-in "variant" command used by <see cref="StringFormatter"/> to output localized string variants.
        /// </summary>
        private sealed class VariantCommandHandler : StringFormatterCommandHandler
        {
            /// <inheritdoc/>
            public override Boolean CanHandleCommand(StringSegment name)
            {
                return name == "variant";
            }

            /// <inheritdoc/>
            public override void HandleCommandLocalizedString(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, LocalizedString value)
            {
                if (value == null)
                    throw new FormatException(CoreStrings.FmtCmdInvalidForGeneratedStrings.Format("variant"));

                var variantName = arguments.GetArgument(0).Text;
                var variant = value.GetVariant(ref variantName);

                output.Append(variant);
            }            
        }
    }
}
