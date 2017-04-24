
namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the comparison functions used by alpha, stencil, and depth buffer tests.
    /// </summary>
    public enum CompareFunction
    {
        /// <summary>
        /// Always pass the test.
        /// </summary>
        Always,

        /// <summary>
        /// Never pass the test.
        /// </summary>
        Never,

        /// <summary>
        /// Pass the test if the new value is equal to the current value.
        /// </summary>
        Equal,

        /// <summary>
        /// Pass the test if the new value is not equal to the current value.
        /// </summary>
        NotEqual,

        /// <summary>
        /// Pass the test if the new value is greater than the current value.
        /// </summary>
        Greater,

        /// <summary>
        /// Pass the test if the new value is greater than or equal to the current value.
        /// </summary>
        GreaterEqual,

        /// <summary>
        /// Pass the test if the new value is less than the current value.
        /// </summary>
        Less,

        /// <summary>
        /// Pass the test if the new value is less than or equal to the current value.
        /// </summary>
        LessEqual,
    }
}
