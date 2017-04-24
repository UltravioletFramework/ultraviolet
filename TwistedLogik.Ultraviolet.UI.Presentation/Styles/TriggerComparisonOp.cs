
namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents the types of comparisons that can be performed by triggers.
    /// </summary>
    public enum TriggerComparisonOp
    {
        /// <summary>
        /// Checks the values for equality.
        /// </summary>
        Equals,
        
        /// <summary>
        /// Checks the values for inequality.
        /// </summary>
        NotEquals,

        /// <summary>
        /// Checks to determine if the specified value is greater than the reference value.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Checks to determine if the specified value is greater than or equal to the reference value.
        /// </summary>
        GreaterThanOrEqualTo,

        /// <summary>
        /// Checks to determine if the specified value is less than the reference value.
        /// </summary>
        LessThan,

        /// <summary>
        /// Checks to determine if the specified value is less than or equal to the reference value.
        /// </summary>
        LessThanOrEqualTo,
    }
}
