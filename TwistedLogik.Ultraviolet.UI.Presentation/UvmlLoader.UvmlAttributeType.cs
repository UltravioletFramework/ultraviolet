
namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class UvmlLoader
    {
        /// <summary>
        /// Represents the type of value represented by an instance of the <see cref="UvmlAttribute"/> class.
        /// </summary>
        private enum UvmlAttributeType
        {
            /// <summary>
            /// The value represents a UPF dependency property.
            /// </summary>
            DependencyProperty,

            /// <summary>
            /// The value represents a UPF routed event.
            /// </summary>
            RoutedEvent,

            /// <summary>
            /// The value represents a .NET Framework property.
            /// </summary>
            FrameworkProperty,

            /// <summary>
            /// The value represents a .NET Framework event.
            /// </summary>
            FrameworkEvent,
        }
    }
}
