using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class UIElementRegistry
    {
        /// <summary>
        /// Clears the element registry.
        /// </summary>
        public void Clear()
        {
            elementsByID.Clear();
        }

        /// <summary>
        /// Adds an element to the registry.
        /// </summary>
        /// <param name="element">The element to add to the registry.</param>
        public void RegisterElement(UIElement element)
        {
            if (String.IsNullOrEmpty(element.ID))
                return;

            UIElement existing;
            if (elementsByID.TryGetValue(element.ID, out existing))
            {
                if (existing == element)
                {
                    return;
                }
                throw new InvalidOperationException(UltravioletStrings.ElementWithIDAlreadyExists.Format(element.ID));
            }
            elementsByID[element.ID] = element;
        }

        /// <summary>
        /// Removes an element from the registry.
        /// </summary>
        /// <param name="element">The element to remove from the registry.</param>
        public void UnregisterElement(UIElement element)
        {
            if (String.IsNullOrEmpty(element.ID))
                return;

            elementsByID.Remove(element.ID);
        }

        /// <summary>
        /// Gets the registered element with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the element to retrieve.</param>
        /// <returns>The element with the specified identifier, or <c>null</c> if no such element exists within the registry.</returns>
        public UIElement GetElementByID(String id)
        {
            UIElement element;
            elementsByID.TryGetValue(id, out element);
            return element;
        }

        /// <summary>
        /// Examines the specified object's fields and populates them with element references. Elements are matched to
        /// fields which have the same name as the element's identifier.
        /// </summary>
        /// <param name="obj">The object to populate with element references.</param>
        public void PopulateFieldsFromRegisteredElements(Object obj)
        {
            Contract.Require(obj, "obj");

            var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToDictionary(x => x.Name);

            foreach (var kvp in elementsByID)
            {
                FieldInfo field;
                if (!fields.TryGetValue(kvp.Key, out field))
                    continue;

                if (!field.FieldType.IsAssignableFrom(kvp.Value.GetType()))
                    continue;

                field.SetValue(obj, kvp.Value);
            }
        }

        // The registry of elements for each known context.
        private readonly Dictionary<String, UIElement> elementsByID = 
            new Dictionary<String, UIElement>();
    }
}
