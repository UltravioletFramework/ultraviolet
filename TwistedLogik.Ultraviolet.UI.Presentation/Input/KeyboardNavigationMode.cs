namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a set of values which determine how keyboard controls navigate through a particular element.
    /// </summary>
    public enum KeyboardNavigationMode
    {
        /// <summary>
        /// Each navigation stop receives focus, and navigation leaves the element upon reaching
        /// the last contained element.
        /// </summary>
        Continue,

        /// <summary>
        /// The entire element and all of its descendants receive focus exactly once.
        /// </summary>
        Once,

        /// <summary>
        /// Upon reaching the last contained element, navigation moves to the first contained
        /// element (or vice-versa if moving in the other direction).
        /// </summary>
        Cycle,

        /// <summary>
        /// Keyboard navigation is not allowed.
        /// </summary>
        None,

        /// <summary>
        /// Each navigation stop receives focus, but navigation will not move beyond the
        /// first or last element in the container, depending on the direction of navigation.
        /// </summary>
        Contained,

        /// <summary>
        /// Only tab indexes within the local subtree are considered for navigation; after
        /// those navigation stops are exhausted, this element behaves as with <see cref="Continue"/>.
        /// </summary>
        Local,
    }
}
