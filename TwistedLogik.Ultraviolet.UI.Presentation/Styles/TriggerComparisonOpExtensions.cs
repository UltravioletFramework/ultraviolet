using System;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Contains extension methods for the <see cref="TriggerComparisonOp"/> enumeration.
    /// </summary>
    public static class TriggerComparisonOpExtensions
    {
        /// <summary>
        /// Converts a <see cref="TriggerComparisonOp"/> value to the corresponding symbol.
        /// </summary>
        /// <param name="op">The <see cref="TriggerComparisonOp"/> value to convert.</param>
        /// <returns>A symbol that represents the specified operation.</returns>
        public static String ConvertToSymbol(this TriggerComparisonOp op)
        {
            switch (op)
            {
                case TriggerComparisonOp.Equals:
                    return "=";

                case TriggerComparisonOp.NotEquals:
                    return "<>";

                case TriggerComparisonOp.GreaterThan:
                    return ">";

                case TriggerComparisonOp.GreaterThanOrEqualTo:
                    return ">=";

                case TriggerComparisonOp.LessThan:
                    return "<";

                case TriggerComparisonOp.LessThanOrEqualTo:
                    return "<=";
            }
            throw new ArgumentOutOfRangeException("op");
        }
    }
}
