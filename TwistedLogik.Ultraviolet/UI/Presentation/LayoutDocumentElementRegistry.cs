using System;
using System.Collections.Generic;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a registry which tracks element identifiers for a particular layout document context.
    /// </summary>
    internal partial class LayoutDocumentElementRegistry : IEnumerable<KeyValuePair<String, UIElement>>
    {
        /// <summary>
        /// Gets the element with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the element to retrieve.</param>
        /// <returns>The element with the specified identifier, or <c>null</c> if no such element exists.</returns>
        public UIElement GetElementByID(String id)
        {
            UIElement element;
            elementsByID.TryGetValue(id, out element);
            return element;
        }

        /// <summary>
        /// Registers an element identifier with the document.
        /// </summary>
        /// <param name="element">The element to register.</param>
        public void RegisterElementID(UIElement element)
        {
            if (String.IsNullOrEmpty(element.ID))
                return;

            UIElement existing;
            if (elementsByID.TryGetValue(element.ID, out existing))
                throw new InvalidOperationException(UltravioletStrings.ElementWithIDAlreadyExists.Format(element.ID));

            elementsByID[element.ID] = element;
        }

        /// <summary>
        /// Unregisters an element identifier from the document.
        /// </summary>
        /// <param name="element">The element to unregister.</param>
        public void UnregisterElementID(UIElement element)
        {
            if (String.IsNullOrEmpty(element.ID))
                return;

            UIElement existing;
            elementsByID.TryGetValue(element.ID, out existing);

            if (existing == element)
                elementsByID.Remove(element.ID);
        }

        // A dictionary which associates element IDs with elements.
        private readonly Dictionary<String, UIElement> elementsByID = 
            new Dictionary<String, UIElement>(StringComparer.InvariantCultureIgnoreCase);
    }
}
