namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents the types of line caps which the FreeType2 library can use to terminate stroke segments.
    /// </summary>
    public enum FreeTypeLineCapMode
    {
        /// <summary>
        /// The end of lines are rendered as a full stop on the last point.
        /// </summary>
        Butt,

        /// <summary>
        /// The end of lines are rendered as a half circle on the last point.
        /// </summary>
        Round,

        /// <summary>
        /// The end of lines are rendered as a square around the last point.
        /// </summary>
        Square,
    }
}
