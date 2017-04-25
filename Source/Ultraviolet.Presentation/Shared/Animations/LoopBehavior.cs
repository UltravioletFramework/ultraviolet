
namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Specifies how a storyboard loops.
    /// </summary>
    public enum LoopBehavior
    {
        /// <summary>
        /// Upon reaching the end of its timeline, the storyboard animations hold their current values until stopped.
        /// </summary>
        None,

        /// <summary>
        /// Upon reaching the end of its timeline, the storyboard loops back to its beginning.
        /// </summary>
        Loop,

        /// <summary>
        /// Upon reaching the end of its timeline, the storyboard plays in reverse.
        /// </summary>
        Reverse,
    }
}
