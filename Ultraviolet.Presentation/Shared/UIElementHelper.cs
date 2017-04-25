using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains helper methods for working with instances of <see cref="UIElement"/>.
    /// </summary>
    internal static class UIElementHelper
    {
        /// <summary>
        /// Adds a routed event handler to the specified element.
        /// </summary>
        public static void AddHandler(DependencyObject dobj, RoutedEvent routedEvent, Delegate handler)
        {
            var element = dobj as UIElement;
            if (element == null)
                throw new ArgumentException(PresentationStrings.NotUIElement);

            element.AddHandler(routedEvent, handler);
        }

        /// <summary>
        /// Removes a routed event handler from the specified element.
        /// </summary>
        public static void RemoveHandler(DependencyObject dobj, RoutedEvent routedEvent, Delegate handler)
        {
            var element = dobj as UIElement;
            if (element == null)
                throw new ArgumentNullException(PresentationStrings.NotUIElement);

            element.RemoveHandler(routedEvent, handler);
        }
    }
}
