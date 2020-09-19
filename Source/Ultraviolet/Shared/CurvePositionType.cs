namespace Ultraviolet
{
    /// <summary>
    /// Represents a set of values which categorize a position relative to the keyframes in a <see cref="Curve{TValue, TKey}"/> instance.
    /// </summary>
    public enum CurvePositionType
    {
        /// <summary>
        /// The position falls before the start of the curve.
        /// </summary>
        BeforeCurve,

        /// <summary>
        /// The position falls within the curve.
        /// </summary>
        WithinCurve,

        /// <summary>
        /// The position falls after the end of the curve.
        /// </summary>
        AfterCurve,
    }
}
