
namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the routing strategy for a routed event.
    /// </summary>
    public enum RoutingStrategy
    {
        /// <summary>
        /// The routed event bubbles upwards through the tree of elements, starting at the event source.
        /// </summary>
        Bubble,

        /// <summary>
        /// The routed event does not route through the element tree.
        /// </summary>
        Direct,

        /// <summary>
        /// The routed event tunnels downwards through the tree of elements, starting at the root element.
        /// </summary>
        Tunnel,
    }
}
