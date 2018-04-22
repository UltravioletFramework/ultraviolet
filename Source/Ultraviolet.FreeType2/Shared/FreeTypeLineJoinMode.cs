namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents the types of line caps which the FreeType2 library can use to join stroke segments.
    /// </summary>
    public enum FreeTypeLineJoinMode
    {
        /// <summary>
        /// Lines are joined smoothly by circular arcs.
        /// </summary>
        Round,

        /// <summary>
        /// Lines are joined by enclosing the triangular region of the corner 
        /// with a straight line between the outer corners of each stroke.
        /// </summary>
        Bevel,

        /// <summary>
        /// Lines are joined with miters, with variable bevels if the miter limit
        /// is exceeded. The intersection of the strokes is clipped at a line perpendicular 
        /// to the bisector of the angle between the strokes, at the distance from the 
        /// intersection of the segments equal to the product of the miter limit value 
        /// and the border radius.
        /// </summary>
        MiterVariable,

        /// <summary>
        /// Lines are joined with miters, with variable bevels if the miter limit
        /// is exceeded. The intersection of the strokes is clipped at a line perpendicular 
        /// to the bisector of the angle between the strokes, at the distance from the 
        /// intersection of the segments equal to the product of the miter limit value 
        /// and the border radius.
        /// </summary>
        Miter = MiterVariable,

        /// <summary>
        /// Lines are joined with miters, with fixed bevels if the miter limit
        /// is exceeded. The outer edges of the strokes for the two segments are extended 
        /// until they meet at an angle. If the segments meet at too sharp an angle (such that the 
        /// miter would extend from the intersection of the segments a distance greater than 
        /// the product of the miter limit value and the border radius), then a bevel join 
        /// is used instead.
        /// </summary>
        MiterFixed,
    }
}
