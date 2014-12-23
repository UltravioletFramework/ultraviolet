using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="PresentationFrameworkManager"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="PresentationFrameworkManager"/> that was created.</returns>
    public delegate PresentationFrameworkManager PresentationFrameworkManagerFactory(UltravioletContext uv);

    /// <summary>
    /// Represents the state of the Ultraviolet Presentation Framework.
    /// </summary>
    public partial class PresentationFrameworkManager : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFrameworkManager"/> class.
        /// </summary>
        internal PresentationFrameworkManager(UltravioletContext uv)
            : base(uv)
        {
            RegisterCoreElements();
        }

        /// <summary>
        /// Attempts to create an instance of the element with the specified name.
        /// </summary>
        /// <typeparam name="TViewModel">The type of view model to which the element will be bound.</typeparam>
        /// <param name="name">The name of the element to instantiate.</param>
        /// <param name="id">The ID with which to create the element.</param>
        /// <param name="bindingContext">The binding context to apply to the element which is instantiated.</param>
        /// <returns>The element that was created, or <c>null</c> if the element could not be created.</returns>
        public UIElement InstantiateElementByName<TViewModel>(String name, String id, String bindingContext = null)
        {
            return InstantiateElementByName(name, id, typeof(TViewModel), bindingContext);
        }

        /// <summary>
        /// Attempts to create an instance of the element with the specified name.
        /// </summary>
        /// <param name="name">The name of the element to instantiate.</param>
        /// <param name="id">The ID with which to create the element.</param>
        /// <param name="viewModelType">The type of view model to which the element will be bound.</param>
        /// <param name="bindingContext">The binding context to apply to the element which is instantiated.</param>
        /// <returns>The element that was created, or <c>null</c> if the element could not be created.</returns>
        public UIElement InstantiateElementByName(String name, String id, Type viewModelType, String bindingContext = null)
        {
            if (bindingContext != null && !BindingExpressions.IsBindingExpression(bindingContext))
                throw new ArgumentException("bindingContext");

            RegisteredElement registration;
            if (!IsElementRegistered(name, out registration))
                return null;

            var instance = (UIElement)Activator.CreateInstance(registration.Type, Ultraviolet, id);
            if (registration.Layout != null)
            {
                UIViewLoader.LoadUserControl((UserControl)instance, registration.Layout, viewModelType, bindingContext);
            }

            return instance;
        }

        /// <summary>
        /// Gets a value indicating whether the element with the specified name is a user control.
        /// </summary>
        /// <param name="name">The name of the element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is a user control; otherwise, <c>false</c>.</returns>
        public Boolean IsUserControl(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            RegisteredElement registration;
            if (!IsElementRegistered(name, out registration))
                return false;

            return registration.Layout != null;
        }

        /// <summary>
        /// Gets the type associated with the specified element name.
        /// </summary>
        /// <param name="name">The name of the element for which to retrieve the associated type.</param>
        /// <param name="type">The type associated with the specified element.</param>
        /// <returns><c>true</c> if the specified element type was retrieved; otherwise, <c>false</c>.</returns>
        public Boolean GetElementType(String name, out Type type)
        {
            return GetElementType(name, true, out type);
        }

        /// <summary>
        /// Gets the type associated with the specified element name.
        /// </summary>
        /// <param name="name">The name of the element for which to retrieve the associated type.</param>
        /// <param name="isCaseSensitive">A value indicating whether the resolution of the element name is case-sensitive.</param>
        /// <param name="type">The type associated with the specified element.</param>
        /// <returns><c>true</c> if the specified element type was retrieved; otherwise, <c>false</c>.</returns>
        public Boolean GetElementType(String name, Boolean isCaseSensitive, out Type type)
        {
            Contract.RequireNotEmpty(name, "name");

            type = null;

            RegisteredElement registration;
            if (!IsElementRegistered(name, out registration))
                return false;

            if (isCaseSensitive && !String.Equals(name, registration.Name, StringComparison.Ordinal))
                return false;

            type = registration.Type;
            return true;
        }

        /// <summary>
        /// Gets the name of the specified element's default property.
        /// </summary>
        /// <param name="name">The name of the element to evaluate.</param>
        /// <param name="property">The name of the element's default property.</param>
        /// <returns><c>true</c> if the specified element's default property was retrieved; otherwise, <c>false</c>.</returns>
        public Boolean GetElementDefaultProperty(String name, out String property)
        {
            Contract.RequireNotEmpty(name, "name");

            property = null;

            RegisteredElement registration;
            if (!IsElementRegistered(name, out registration))
                return false;

            property = registration.DefaultProperty;
            return true;
        }

        /// <summary>
        /// Gets the name of the specified element's default property.
        /// </summary>
        /// <param name="type">The type of the element to evaluate.</param>
        /// <param name="property">The name of the element's default property.</param>
        /// <returns><c>true</c> if the specified element's default property was retrieved; otherwise, <c>false</c>.</returns>
        public Boolean GetElementDefaultProperty(Type type, out String property)
        {
            Contract.Require(type, "type");

            property = null;

            RegisteredElement registration;
            if (!IsElementRegistered(type, out registration))
                return false;

            property = registration.DefaultProperty;
            return true;
        }

        /// <summary>
        /// Registers a custom element type with the Presentation Framework.
        /// </summary>
        /// <param name="type">The type that implements the custom element.</param>
        public void RegisterElement(Type type)
        {
            Contract.Require(type, "type");

            RegisterElementInternal(type, null);
        }

        /// <summary>
        /// Registers a custom element type with the Presentation Framework.
        /// </summary>
        /// <param name="layout">The XML document that defines the custom element's layout.</param>
        public void RegisterElement(XDocument layout)
        {
            Contract.Require(layout, "layout");

            var type = ExtractElementTypeFromLayout(layout);

            RegisterElementInternal(type, layout);
        }

        /// <summary>
        /// Unregisters a custom element.
        /// </summary>
        /// <param name="type">The type that implements the custom element.</param>
        /// <returns><c>true</c> if the custom element was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterElement(Type type)
        {
            Contract.Require(type, "type");

            RegisteredElement registration;
            if (!IsElementRegistered(type, out registration))
                return false;

            return registeredElements.Remove(registration.Name);
        }

        /// <summary>
        /// Unregisters a custom element.
        /// </summary>
        /// <param name="layout">The XML document that defines the custom element's layout.</param>
        /// <returns><c>true</c> if the custom element was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterElement(XDocument layout)
        {
            Contract.Require(layout, "layout");

            var type = ExtractElementTypeFromLayout(layout);

            RegisteredElement registration;
            if (!IsElementRegistered(type, out registration))
                return false;

            return registeredElements.Remove(registration.Name);
        }

        /// <summary>
        /// Gets a value indicating whether the specified XML document is a valid element layout.
        /// </summary>
        /// <param name="layout">The XML document that defines the custom element's layout.</param>
        /// <returns><c>true</c> if the specified XML document is a valid element layout; otherwise, <c>false</c>.</returns>
        private static Boolean IsValidElementLayout(XDocument layout)
        {
            return layout.Root.Name.LocalName == "UserControl";
        }

        /// <summary>
        /// Gets a value indicating whether the specified type is a valid element type.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <param name="name">The element's name.</param>
        /// <returns><c>true</c> if the specified type is a valid element type; otherwise, <c>false</c>.</returns>
        private static Boolean IsValidElementType(Type type, out String name)
        {
            if (!typeof(UIElement).IsAssignableFrom(type))
            {
                name = null;
                return false;
            }

            var attr = type.GetCustomAttributes(typeof(UIElementAttribute), false).Cast<UIElementAttribute>().SingleOrDefault();
            if (attr == null)
            {
                name = null;
                return false;
            }

            name = attr.Name;
            return true;
        }

        /// <summary>
        /// Extracts the element type associated with the specified layout.
        /// </summary>
        /// <param name="layout">The XML document that defines the custom element's layout.</param>
        /// <returns>The element type associated with the specified layout.</returns>
        private static Type ExtractElementTypeFromLayout(XDocument layout)
        {
            if (!IsValidElementLayout(layout))
                throw new ArgumentException(UltravioletStrings.InvalidUserControlDefinition);

            var attr = layout.Root.Attribute("Type");
            if (attr == null)
                throw new InvalidOperationException(UltravioletStrings.UserControlDoesNotDefineType);

            var type = Type.GetType(attr.Value, false);
            if (type == null)
                throw new InvalidOperationException(UltravioletStrings.InvalidUserControlType.Format(attr.Value));

            String name;
            if (!IsValidElementType(type, out name))
                throw new InvalidOperationException(UltravioletStrings.InvalidUIElementType.Format(type.Name));

            return type;
        }

        /// <summary>
        /// Registers the Presentation Framework's core elements.
        /// </summary>
        private void RegisterCoreElements()
        {
            var elementTypes = from t in typeof(UIElement).Assembly.GetTypes()
                               let attr = t.GetCustomAttributes(typeof(UIElementAttribute), false).SingleOrDefault()
                               where
                                attr != null
                               select new { ElementType = t, ElementAttribute = (UIElementAttribute)attr };

            foreach (var elementType in elementTypes)
            {
                var defaultPropertyAttr  = (DefaultPropertyAttribute)elementType.ElementType.GetCustomAttributes(typeof(DefaultPropertyAttribute), true).SingleOrDefault();
                var defaultProperty      = default(String);
                if (defaultPropertyAttr != null)
                {
                    defaultProperty = defaultPropertyAttr.Name;
                }

                var constructor = elementType.ElementType.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) });
                if (constructor == null)
                {
                    throw new InvalidOperationException(UltravioletStrings.UIElementInvalidCtor.Format(elementType));
                }

                var registration = new RegisteredElement(
                    elementType.ElementAttribute.Name,
                    elementType.ElementType,
                    constructor, defaultProperty);

                coreElements[elementType.ElementAttribute.Name] = registration;
            }
        }

        /// <summary>
        /// Registers a custom element type with the Presentation Framework.
        /// </summary>
        /// <param name="type">The type that implements the custom element.</param>
        /// <param name="layout">The XML document that defines the custom element's layout.</param>
        private void RegisterElementInternal(Type type, XDocument layout)
        {
            String name;
            if (!IsValidElementType(type, out name))
                throw new InvalidOperationException(UltravioletStrings.InvalidUIElementType.Format(type.Name));

            RegisteredElement existingRegistration;
            if (IsElementRegistered(name, out existingRegistration))
                throw new InvalidOperationException(UltravioletStrings.UnrecognizedUIElement.Format(name));

            var defaultProperty = default(String);
            var defaultPropertyAttr = type.GetCustomAttributes(typeof(DefaultPropertyAttribute), true).Cast<DefaultPropertyAttribute>().SingleOrDefault();
            if (defaultPropertyAttr != null)
            {
                defaultProperty = defaultPropertyAttr.Name;
            }

            var ctor = type.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) });
            if (ctor == null)
                throw new InvalidOperationException(UltravioletStrings.UIElementInvalidCtor.Format(type.Name));

            registeredElements[name] = new RegisteredElement(name, type, ctor, defaultProperty, layout);
        }

        /// <summary>
        /// Gets a value indicating whether the specified custom element is registered.
        /// </summary>
        /// <param name="name">The name of the custom element.</param>
        /// <param name="registration">The element registration.</param>
        /// <returns><c>true</c> if the specified element is registered; otherwise, <c>false</c>.</returns>
        private Boolean IsElementRegistered(String name, out RegisteredElement registration)
        {
            if (coreElements.TryGetValue(name, out registration))
                return true;

            if (registeredElements.TryGetValue(name, out registration))
                return true;

            registration = null;
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified custom element is registered.
        /// </summary>
        /// <param name="type">The type that implements the custom element.</param>
        /// <param name="registration">The element registration.</param>
        /// <returns><c>true</c> if the specified element is registered; otherwise, <c>false</c>.</returns>
        private Boolean IsElementRegistered(Type type, out RegisteredElement registration)
        {
            foreach (var value in coreElements.Values)
            {
                if (value.Type == type)
                {
                    registration = value;
                    return true;
                }
            }
            foreach (var value in registeredElements.Values)
            {
                if (value.Type == type)
                {
                    registration = value;
                    return true;
                }
            }
            registration = null;
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified custom element is registered.
        /// </summary>
        /// <param name="layout">The XML document that defines the custom element's layout.</param>
        /// <param name="registration">The element registration.</param>
        /// <returns><c>true</c> if the specified element is registered; otherwise, <c>false</c>.</returns>
        private Boolean IsElementRegistered(XDocument layout, out RegisteredElement registration)
        {
            var type = ExtractElementTypeFromLayout(layout);
            foreach (var value in coreElements.Values)
            {
                if (value.Type == type)
                {
                    registration = value;
                    return true;
                }
            }
            foreach (var value in registeredElements.Values)
            {
                if (value.Type == type)
                {
                    registration = value;
                    return true;
                }
            }
            registration = null;
            return false;
        }

        // The core element registry.
        private readonly Dictionary<String, RegisteredElement> coreElements = 
            new Dictionary<String, RegisteredElement>(StringComparer.OrdinalIgnoreCase);

        // The custom element registry.
        private readonly Dictionary<String, RegisteredElement> registeredElements = 
            new Dictionary<String, RegisteredElement>(StringComparer.OrdinalIgnoreCase);
    }
}
