namespace Ultraviolet
{
    /// <summary>
    /// Represents a value which describes the relationship between two bounding volumes.
    /// </summary>
    public enum ContainmentType
    {
        /// <summary>
        /// There is no relationship between the bounding volumes.
        /// </summary>
        Disjoint,

        /// <summary>
        /// One bounding volume completely contains the other.
        /// </summary>
        Contains,

        /// <summary>
        /// The bounding volumes partially overlap.
        /// </summary>
        Intersects,
    }
}
