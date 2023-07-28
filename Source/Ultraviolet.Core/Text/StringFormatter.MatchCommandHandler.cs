using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    partial class StringFormatter
    {
        /// <summary>
        /// Implements the built-in "match" command used by <see cref="StringFormatter"/> to match a localized
        /// string to another localized string.
        /// </summary>
        private sealed class MatchCommandHandler : StringFormatterCommandHandler
        {
            /// <inheritdoc/>
            public override Boolean CanHandleCommand(StringSegment name)
            {
                return name == "match";
            }

            /// <inheritdoc/>
            public override void HandleCommandLocalizedString(StringFormatter formatter,
                StringBuilder output, StringSegment command, StringFormatterCommandArguments arguments, LocalizedString value)
            {
                var targetArgumentArg = arguments.GetArgument(0);
                var targetArgumentIndex = targetArgumentArg.GetValueAsInt32();
                var targetMatchRuleArg = arguments.GetNextArgument(ref targetArgumentArg);
                var targetMatchRule = targetMatchRuleArg.Text;

                // Make sure our target is a localized string variant.
                var target = formatter.GetArgument(targetArgumentIndex);
                if (target.Reference == null || !(target.Reference is LocalizedStringVariant))
                    throw new FormatException(CoreStrings.FmtCmdInvalidForArgument.Format("match", target.Reference?.GetType()));

                // Run the specified match evaluator.
                var match = Localization.MatchVariant(value, (LocalizedStringVariant)target.Reference, targetMatchRule);
                output.Append(match ?? "???");
            }
        }
    }
}
