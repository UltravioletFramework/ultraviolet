namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the set of lists in a Presentation Foundation resource pool that a particular object can be associated with.
    /// </summary>
    internal enum UpfPoolList
    {
        /// <summary>
        /// The object is associated with no pool and was probably allocated on demand after the pool reached its high watermark.
        /// </summary>
        None,

        /// <summary>
        /// The object is part of the available pool.
        /// </summary>
        Available,

        /// <summary>
        /// The object is part of the active pool.
        /// </summary>
        Active,
    }
}
