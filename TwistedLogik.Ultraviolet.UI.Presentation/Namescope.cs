using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the namescope for an interface element. A namescope represents a region in UVML within
    /// which element names must be unique; this usually means a view layout or a control definition.
    /// </summary>
    public sealed class Namescope
    {
        /// <summary>
        /// Clears the element registry.
        /// </summary>
        public void Clear()
        {
            elementsByName.Clear();
        }

        /// <summary>
        /// Adds an element to the registry.
        /// </summary>
        /// <param name="element">The element to add to the registry.</param>
        public void RegisterElement(FrameworkElement element)
        {
            if (String.IsNullOrEmpty(element.Name))
                return;

            FrameworkElement existing;
            if (elementsByName.TryGetValue(element.Name, out existing))
            {
                if (existing == element)
                {
                    return;
                }
                throw new InvalidOperationException(PresentationStrings.ElementWithNameAlreadyExists.Format(element.Name));
            }
            elementsByName[element.Name] = element;
        }

        /// <summary>
        /// Removes an element from the registry.
        /// </summary>
        /// <param name="element">The element to remove from the registry.</param>
        public void UnregisterElement(FrameworkElement element)
        {
            if (String.IsNullOrEmpty(element.Name))
                return;

            elementsByName.Remove(element.Name);
        }

        /// <summary>
        /// Gets the registered element with the specified identifying name.
        /// </summary>
        /// <param name="name">The identifying name of the element to retrieve.</param>
        /// <returns>The element with the specified identifying name, or <c>null</c> if no such element exists within the registry.</returns>
        public FrameworkElement GetElementByName(String name)
        {
            FrameworkElement element;
            elementsByName.TryGetValue(name, out element);
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

            var type = obj.GetType();

            while (type != null)
            {
                if (type == typeof(UIElement) || 
                    type == typeof(FrameworkElement) ||
                    type == typeof(Control))
                {
                    break;
                }

                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToDictionary(x => x.Name);

                foreach (var kvp in elementsByName)
                {
                    FieldInfo field;
                    if (!fields.TryGetValue(kvp.Key, out field))
                        continue;

                    if (!field.FieldType.IsAssignableFrom(kvp.Value.GetType()))
                        continue;

                    field.SetValue(obj, kvp.Value);
                }

                type = type.BaseType;
            }
        }

        // The registry of elements for each known context.
        private readonly Dictionary<String, FrameworkElement> elementsByName = 
            new Dictionary<String, FrameworkElement>();
    }
}
