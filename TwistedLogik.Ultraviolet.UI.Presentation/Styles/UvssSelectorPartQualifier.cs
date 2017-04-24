namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents the special type of a <see cref="UvssSelectorPart"/>.
    /// </summary>
    public enum UvssSelectorPartQualifier
    {
        /// <summary>
        /// No special type.
        /// </summary>
        None,

        /// <summary>
        /// The selector part must be a visual child of the previous part.
        /// </summary>
        VisualChild,

        /// <summary>
        /// The selector part must be a logical child of the previous part.
        /// </summary>
        LogicalChild,

        /// <summary>
        /// The selector part must be a templated child of the previous part.
        /// </summary>
        TemplatedChild,
    }
}
