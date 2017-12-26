using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains extension methods for the <see cref="IInputElement"/> interface.
    /// </summary>
    internal static class IInputElementExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the specified element is a valid recipient of input events.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element is valid for input; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsValidForInput(this IInputElement element)
        {
            var uiElement = element as UIElement;
            if (uiElement == null)
                return false;

            return uiElement.IsHitTestVisible && uiElement.IsVisible && element.IsEnabled;
        }
    }
}
