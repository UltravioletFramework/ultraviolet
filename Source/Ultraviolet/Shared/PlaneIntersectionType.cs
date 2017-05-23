namespace Ultraviolet
{
    /// <summary>
    /// Represents the type of intersection between a plane and a bounding volume.
    /// </summary>
    public enum PlaneIntersectionType
    {
        /// <summary>
        /// There is no intersection, and the bounding volume is in the negative half-space of the <see cref="Plane"/>.
        /// </summary>
        Front,

        /// <summary>
        /// There is no intersection, and the bounding volume is in the positive half-space of the <see cref="Plane"/>.
        /// </summary>
        Back,

        /// <summary>
        /// The <see cref="Plane"/> is intersected.
        /// </summary>
        Intersecting,
    }
}
