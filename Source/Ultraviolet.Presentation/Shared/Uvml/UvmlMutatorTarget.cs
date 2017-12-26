namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents the types of properties and events which can be mutated by a UVML mutator.
    /// </summary>
    internal enum UvmlMutatorTarget
    {
        /// <summary>
        /// Represents a dependency property.
        /// </summary>
        DependencyProperty,

        /// <summary>
        /// Represents a dependency property which is being data bound.
        /// </summary>
        DependencyPropertyBinding,

        /// <summary>
        /// Represents a routed event.
        /// </summary>
        RoutedEvent,

        /// <summary>
        /// Represents a standard .NET property.
        /// </summary>
        StandardProperty,

        /// <summary>
        /// Represents a standard .NET event.
        /// </summary>
        StandardEvent,
    }
}
