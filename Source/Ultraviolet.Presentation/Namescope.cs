using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Controls;

namespace Ultraviolet.Presentation
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
        /// Registers an object with the specified identifying name within the namescope.
        /// </summary>
        /// <param name="name">The identifying name with which to register the object.</param>
        /// <param name="scopedElement">The object to register under the specified name.</param>
        public void RegisterName(String name, Object scopedElement)
        {
            Contract.RequireNotEmpty(name, nameof(name));
            Contract.Require(scopedElement, nameof(scopedElement));

            if (elementsByName.TryGetValue(name, out var existing))
            {
                if (existing != scopedElement)
                    throw new InvalidOperationException(PresentationStrings.DuplicateNamescopeName.Format(name));
            }
            else
            {
                elementsByName[name] = scopedElement;
            }
        }

        /// <summary>
        /// Removes an object from the registry.
        /// </summary>
        /// <param name="name"></param>
        public void UnregisterName(String name)
        {
            if (String.IsNullOrEmpty(name))
                return;

            elementsByName.Remove(name);
        }
        
        /// <summary>
        /// Gets the registered object with the specified identifying name.
        /// </summary>
        /// <param name="name">The identifying name of the object to retrieve.</param>
        /// <returns>The object with the specified identifying name, or <see langword="null"/> if no such object exists within the registry.</returns>
        public Object FindName(String name)
        {
            if (String.IsNullOrEmpty(name))
                return null;

            elementsByName.TryGetValue(name, out var element);
            return element;
        }

        /// <summary>
        /// Examines the specified object's fields and populates them with element references. Elements are matched to
        /// fields which have the same name as the element's identifier.
        /// </summary>
        /// <param name="obj">The object to populate with element references.</param>
        internal void PopulateFieldsFromRegisteredElements(Object obj)
        {
            Contract.Require(obj, nameof(obj));

            if (obj is IDataSourceWrapper)
                obj = ((IDataSourceWrapper)obj).WrappedDataSource;

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
                    if (!fields.TryGetValue(kvp.Key, out var field))
                        continue;

                    if (!field.FieldType.IsAssignableFrom(kvp.Value.GetType()))
                        continue;

                    field.SetValue(obj, kvp.Value);
                }

                type = type.BaseType;
            }
        }

        // The registry of elements for each known context.
        private readonly Dictionary<String, Object> elementsByName = 
            new Dictionary<String, Object>();
    }
}
