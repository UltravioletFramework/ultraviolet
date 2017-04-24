using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Stores and manages the collection of component templates that are
    /// currently registered with the Ultraviolet Presentation Foundation.
    /// </summary>
    public sealed partial class ComponentTemplateManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentTemplateManager"/> class.
        /// </summary>
        internal ComponentTemplateManager()
        {

        }

        /// <summary>
        /// Gets the component template for the specified interface element.
        /// </summary>
        /// <param name="element">The element for which to retrieve a component template.</param>
        /// <returns>The component template for the specified interface element, or <see langword="null"/> if no such template is registered.</returns>
        public XDocument Get(UIElement element)
        {
            Type type;
            return Get(element, out type);
        }

        /// <summary>
        /// Gets the component template for the specified interface element.
        /// </summary>
        /// <param name="element">The element for which to retrieve a component template.</param>
        /// <param name="actualType">The actual type for which a template was retrieved, which may be an 
        /// ancestor of the runtime type of <paramref name="element"/>.</param>
        /// <returns>The component template for the specified interface element, or <see langword="null"/> if no such template is registered.</returns>
        public XDocument Get(UIElement element, out Type actualType)
        {
            Contract.Require(element, nameof(element));

            return Get(element.GetType(), out actualType);
        }

        /// <summary>
        /// Gets the component template for the specified element type, if one has been registered.
        /// </summary>
        /// <typeparam name="T">The type of element for which to retrieve a component template.</typeparam>
        /// <returns>The component template for the specified type, or <see langword="null"/> if no such template exists.</returns>
        public XDocument Get<T>() where T : UIElement
        {
            Type actualType;
            return Get(typeof(T), out actualType);
        }

        /// <summary>
        /// Gets the component template for the specified element type, if one has been registered.
        /// </summary>
        /// <param name="type">The type of element for which to retrieve a component template.</param>
        /// <returns>The component template for the specified type, or <see langword="null"/> if no such template exists.</returns>
        public XDocument Get(Type type)
        {
            Type actualType;
            return Get(type, out actualType);
        }

        /// <summary>
        /// Gets the component template for the specified element type, if one has been registered.
        /// </summary>
        /// <param name="type">The type of element for which to retrieve a component template.</param>
        /// <param name="actualType">The actual type for which a template was retrieved, which may be an ancestor of <paramref name="type"/>.</param>
        /// <returns>The component template for the specified type, or <see langword="null"/> if no such template exists.</returns>
        public XDocument Get(Type type, out Type actualType)
        {
            Contract.Require(type, nameof(type));

            var template = default(XDocument);
            actualType = null;

            lock (sync)
            {
                for (var current = type; current != null; current = current.BaseType)
                {
                    if (templates.TryGetValue(current, out template))
                    {
                        actualType = current;
                        break;
                    }

                    if (defaults.TryGetValue(current, out template))
                    {
                        actualType = current;
                        break;
                    }
                }
            }

            return template;
        }

        /// <summary>
        /// Gets the default component template for the specified element type, if one has been registered.
        /// </summary>
        /// <typeparam name="T">The type of element for which to retrieve the default component template.</typeparam>
        /// <returns>The default component template for the specified type, or <see langword="null"/> if no such template exists.</returns>
        public XDocument GetDefault<T>() where T : UIElement
        {
            return GetDefault(typeof(T));
        }
        
        /// <summary>
        /// Gets the default component template for the specified element type, if one has been registered.
        /// </summary>
        /// <param name="type">The type of element for which to retrieve the default component template.</param>
        /// <returns>The default component template for the specified type, or <see langword="null"/> if no such template exists.</returns>
        public XDocument GetDefault(Type type)
        {
            Type actualType;
            return GetDefault(type, out actualType);
        }

        /// <summary>
        /// Gets the default component template for the specified element type, if one has been registered.
        /// </summary>
        /// <param name="type">The type of element for which to retrieve the default component template.</param>
        /// <param name="actualType">The actual type for which a template was retrieved, which may be an ancestor of <paramref name="type"/>.</param>
        /// <returns>The default component template for the specified type, or <see langword="null"/> if no such template exists.</returns>
        public XDocument GetDefault(Type type, out Type actualType)
        {
            Contract.Require(type, nameof(type));

            var template = default(XDocument);
            actualType = null;

            lock (sync)
            {
                for (var current = type; type != null; type = type.BaseType)
                {
                    if (defaults.TryGetValue(current, out template))
                    {
                        actualType = current;
                        break;
                    }
                }
            }

            return template;
        }

        /// <summary>
        /// Sets the component template for the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of element for which to register a component template.</typeparam>
        /// <param name="template">The template to register for the specified type.</param>
        public void Set<T>(XDocument template) where T : UIElement
        {
            Set(typeof(T), template);
        }

        /// <summary>
        /// Sets the component template for the specified element type.
        /// </summary>
        /// <param name="type">The type of element for which to register a component template.</param>
        /// <param name="template">The template to register for the specified type.</param>
        public void Set(Type type, XDocument template)
        {
            Contract.Require(type, nameof(type));
            Contract.Require(template, nameof(template));

            lock (sync)
            {
                templates[type] = template;
            }
        }

        /// <summary>
        /// Sets the default component template for the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of element for which to register the default component template.</typeparam>
        /// <param name="template">The template to register as the default for the specified type.</param>
        public void SetDefault<T>(XDocument template) where T : UIElement
        {
            SetDefault(typeof(T), template);
        }

        /// <summary>
        /// Sets the default component template for the specified element type.
        /// </summary>
        /// <param name="type">The type of element for which to register the default component template.</param>
        /// <param name="template">The template to register as the default for the specified type.</param>
        public void SetDefault(Type type, XDocument template)
        {
            Contract.Require(type, nameof(type));
            Contract.Require(template, nameof(template));

            lock (sync)
            {
                defaults[type] = template;
            }
        }

        /// <summary>
        /// Removes the component template for the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of element for which to remove the component template.</typeparam>
        public void Remove<T>() where T : UIElement
        {
            Remove(typeof(T));
        }

        /// <summary>
        /// Removes the component template for the specified element type.
        /// </summary>
        /// <param name="type">The type of element for which to remove the component template.</param>
        public void Remove(Type type)
        {
            Contract.Require(type, nameof(type));

            lock (sync)
            {
                templates.Remove(type);
            }
        }

        /// <summary>
        /// Removes the default component template for the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of element for which to remove the default component template.</typeparam>
        public void RemoveDefault<T>() where T : UIElement
        {
            RemoveDefault(typeof(T));
        }

        /// <summary>
        /// Removes the default component template for the specified element type.
        /// </summary>
        /// <param name="type">The type of element for which to remove the default component template.</param>
        public void RemoveDefault(Type type)
        {
            lock (sync)
            {
                defaults.Remove(type);
            }
        }

        /// <summary>
        /// Clears the component templates stored in this manager instance.
        /// </summary>
        public void Clear()
        {
            lock (sync)
            {
                templates.Clear();
            }
        }

        /// <summary>
        /// Clears the default component templates stored in this manager instance.
        /// </summary>
        public void ClearDefaults()
        {
            lock (sync)
            {
                defaults.Clear();
            }
        }
                
        // State values.
        private readonly Object sync = new Object();
        private readonly Dictionary<Type, XDocument> defaults = new Dictionary<Type, XDocument>();
        private readonly Dictionary<Type, XDocument> templates = new Dictionary<Type, XDocument>();
    }
}
