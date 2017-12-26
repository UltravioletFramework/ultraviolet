using System;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Contains helper methods for working with instances of the <see cref="IInputElement"/> interface.
    /// </summary>
    internal static class IInputElementHelper
    {
        /// <summary>
        /// Adds an event handler for a routed event to the specified input element.
        /// </summary>
        /// <param name="dobj">The input element to which to add the event handler.</param>
        /// <param name="routedEvent">A <see cref="RoutedEvent"/> that identifies the routed event for which to add a handler.</param>
        /// <param name="handler">A delegate that represents the handler to add to the element for the specified routed event.</param>
        public static void AddHandler(DependencyObject dobj, RoutedEvent routedEvent, Delegate handler)
        {
            var element = dobj as IInputElement;
            if (element != null)
            {
                element.AddHandler(routedEvent, handler);
                return;
            }

            throw new ArgumentException(PresentationStrings.NotAnInputElement.Format("dobj"));
        }

        /// <summary>
        /// Removes a handler for a routed event from the specified input element.
        /// </summary>
        /// <param name="dobj">The input element from which to remove the event handler.</param>
        /// <param name="routedEvent">A <see cref="RoutedEvent"/> that identifies the routed event for which to remove a handler.</param>
        /// <param name="handler">A delegate that represents the handler to remove from the element for the specified routed event.</param>
        public static void RemoveHandler(DependencyObject dobj, RoutedEvent routedEvent, Delegate handler)
        {
            var element = dobj as IInputElement;
            if (element != null)
            {
                element.RemoveHandler(routedEvent, handler);
                return;
            }

            throw new ArgumentException(PresentationStrings.NotAnInputElement.Format("dobj"));
        }
    }
}
