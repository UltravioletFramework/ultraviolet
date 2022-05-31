namespace Ultraviolet.Presentation
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
        public static readonly RoutedEvent OpeningEvent = EventManager.RegisterRoutedEvent("Opening", 
            RoutingStrategy.Direct, typeof(UpfRoutedEventHandler), typeof(View));

        /// <summary>
        /// Identifies the Opened routed event.
        /// </summary>
        public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent("Opened",
            RoutingStrategy.Direct, typeof(UpfRoutedEventHandler), typeof(View));

        /// <summary>
        /// Identifies the Closing routed event.
        /// </summary>
        public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent("Closing",
            RoutingStrategy.Direct, typeof(UpfRoutedEventHandler), typeof(View));

        /// <summary>
        /// Identifies the Closed routed event.
        /// </summary>
        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed",
            RoutingStrategy.Direct, typeof(UpfRoutedEventHandler), typeof(View));

        /// <summary>
        /// Identifies the ViewModelChanged routed event.
        /// </summary>
        public static readonly RoutedEvent ViewModelChangedEvent = EventManager.RegisterRoutedEvent("ViewModelChanged",
            RoutingStrategy.Direct, typeof(UpfRoutedEventHandler), typeof(View));
    }
}
