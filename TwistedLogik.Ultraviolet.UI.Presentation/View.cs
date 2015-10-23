namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains routed events relating to view lifetimes.
    /// </summary>
    [UvmlKnownType]
    public static class View
    {
        /// <summary>
        /// Identifies the Opening routed event.
        /// </summary>
        public static readonly RoutedEvent Opening = EventManager.RegisterRoutedEvent("Opening", 
            RoutingStrategy.Direct, typeof(UpfRoutedEventHandler), typeof(View));

        /// <summary>
        /// Identifies the Opened routed event.
        /// </summary>
        public static readonly RoutedEvent Opened = EventManager.RegisterRoutedEvent("Opened",
            RoutingStrategy.Direct, typeof(UpfRoutedEventHandler), typeof(View));

        /// <summary>
        /// Identifies the Closing routed event.
        /// </summary>
        public static readonly RoutedEvent Closing = EventManager.RegisterRoutedEvent("Closing",
            RoutingStrategy.Direct, typeof(UpfRoutedEventHandler), typeof(View));

        /// <summary>
        /// Identifies the Closed routed event.
        /// </summary>
        public static readonly RoutedEvent Closed = EventManager.RegisterRoutedEvent("Closed",
            RoutingStrategy.Direct, typeof(UpfRoutedEventHandler), typeof(View));
    }
}
