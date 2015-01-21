using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        /// Updates the state of the presentation framework.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            ProcessStyleQueue();
            ProcessMeasureQueue();
            ProcessArrangeQueue();
            ProcessPositionQueue();
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
            Contract.EnsureNotDisposed(this, Disposed);

            if (bindingContext != null && !BindingExpressions.IsBindingExpression(bindingContext))
                throw new ArgumentException("bindingContext");

            RegisteredElement registration;
            if (!IsElementRegistered(name, out registration))
                return null;

            var ctor = registration.Type.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) });
            if (ctor == null)
                throw new InvalidOperationException(UltravioletStrings.NoValidConstructor.Format(registration.Type));

            var instance = (UIElement)ctor.Invoke(new Object[] { Ultraviolet, id });

            if (registration.Layout != null)
            {
                UvmlLoader.LoadUserControl((UserControl)instance, registration.Layout, viewModelType, bindingContext);
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
            Contract.EnsureNotDisposed(this, Disposed);

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
            Contract.EnsureNotDisposed(this, Disposed);

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
            Contract.EnsureNotDisposed(this, Disposed);

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
            Contract.EnsureNotDisposed(this, Disposed);

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
            Contract.EnsureNotDisposed(this, Disposed);

            RegisterElementInternal(type, null);
        }

        /// <summary>
        /// Registers a custom element type with the Presentation Framework.
        /// </summary>
        /// <param name="layout">The XML document that defines the custom element's layout.</param>
        public void RegisterElement(XDocument layout)
        {
            Contract.Require(layout, "layout");
            Contract.EnsureNotDisposed(this, Disposed);

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
            Contract.EnsureNotDisposed(this, Disposed);

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
            Contract.EnsureNotDisposed(this, Disposed);

            var type = ExtractElementTypeFromLayout(layout);

            RegisteredElement registration;
            if (!IsElementRegistered(type, out registration))
                return false;

            return registeredElements.Remove(registration.Name);
        }

        /// <summary>
        /// Gets the Presentation Framework's component template manager.
        /// </summary>
        public ComponentTemplateManager ComponentTemplates
        {
            get { return componentTemplateManager; }
        }

        /// <summary>
        /// Gets the queue of elements with invalid styling states.
        /// </summary>
        internal LayoutQueue StyleQueue
        {
            get { return styleQueue; }
        }

        /// <summary>
        /// Gets the queue of elements with invalid measurement states.
        /// </summary>
        internal LayoutQueue MeasureQueue
        {
            get { return measureQueue; }
        }

        /// <summary>
        /// Gets the queue of elements with invalid arrangement states.
        /// </summary>
        internal LayoutQueue ArrangeQueue
        {
            get { return arrangeQueue; }
        }

        /// <summary>
        /// Gets the queue of elements with invalid position states.
        /// </summary>
        internal LayoutQueue PositionQueue
        {
            get { return positionQueue; }
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
        /// <param name="attr">The element's <see cref="UIElementAttribute"/> instance.</param>
        /// <returns><c>true</c> if the specified type is a valid element type; otherwise, <c>false</c>.</returns>
        private static Boolean IsValidElementType(Type type, out UIElementAttribute attr)
        {
            attr = null;

            if (!typeof(UIElement).IsAssignableFrom(type))
                return false;

            attr = type.GetCustomAttributes(typeof(UIElementAttribute), false).Cast<UIElementAttribute>().SingleOrDefault();

            return attr != null;
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

            UIElementAttribute uiElementAttr;
            if (!IsValidElementType(type, out uiElementAttr))
                throw new InvalidOperationException(UltravioletStrings.InvalidUserControlType.Format(type.Name));

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

                var type = elementType.ElementType;
                var ctor = type.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) });
                
                if (ctor == null)
                    throw new InvalidOperationException(UltravioletStrings.UIElementInvalidCtor.Format(elementType));

                RuntimeHelpers.RunClassConstructor(type.TypeHandle);

                var registration = new RegisteredElement(
                    elementType.ElementAttribute.Name,
                    elementType.ElementType,
                    defaultProperty);

                RegisterDefaultComponentTemplate(type, elementType.ElementAttribute);

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
            UIElementAttribute uiElementAttr;
            if (!IsValidElementType(type, out uiElementAttr))
                throw new InvalidOperationException(UltravioletStrings.InvalidUIElementType.Format(type.Name));

            RegisteredElement existingRegistration;
            if (IsElementRegistered(uiElementAttr.Name, out existingRegistration))
                throw new InvalidOperationException(UltravioletStrings.UnrecognizedUIElement.Format(uiElementAttr.Name));

            var defaultProperty = default(String);
            var defaultPropertyAttr = type.GetCustomAttributes(typeof(DefaultPropertyAttribute), true).Cast<DefaultPropertyAttribute>().SingleOrDefault();
            if (defaultPropertyAttr != null)
            {
                defaultProperty = defaultPropertyAttr.Name;
            }

            var ctor = type.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) });
            if (ctor == null)
                throw new InvalidOperationException(UltravioletStrings.UIElementInvalidCtor.Format(type.Name));

            RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            RegisterDefaultComponentTemplate(type, uiElementAttr);

            registeredElements[uiElementAttr.Name] = new RegisteredElement(uiElementAttr.Name, type, defaultProperty, layout);
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

        /// <summary>
        /// Processes the queue of elements with invalid styling states.
        /// </summary>
        private void ProcessStyleQueue()
        {
            while (styleQueue.Count > 0)
            {
                var element = styleQueue.Dequeue();
                if (element.IsStyleValid)
                    continue;

                element.Style(element.MostRecentStylesheet);
                element.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Processes the queue of elements with invalid measurement states.
        /// </summary>
        private void ProcessMeasureQueue()
        {
            while (measureQueue.Count > 0)
            {
                var element = measureQueue.Dequeue();
                if (element.IsMeasureValid)
                    continue;

                element.Measure(element.MostRecentAvailableSize);
                element.InvalidateArrange();
            }
        }

        /// <summary>
        /// Processes the queue of elements with invalid arrangement states.
        /// </summary>
        private void ProcessArrangeQueue()
        {
            while (arrangeQueue.Count > 0)
            {
                var element = arrangeQueue.Dequeue();
                if (element.IsArrangeValid)
                    continue;

                element.Arrange(element.MostRecentFinalRect, element.MostRecentArrangeOptions);
            }
        }

        /// <summary>
        /// Processes the queue of elements with invalid position states.
        /// </summary>
        private void ProcessPositionQueue()
        {
            while (positionQueue.Count > 0)
            {
                var element = positionQueue.Dequeue();
                if (element.IsPositionValid)
                    continue;

                element.Position(element.MostRecentPosition);
            }
        }

        /// <summary>
        /// Registers the specified type's default component template, if it has one.
        /// </summary>
        /// <param name="type">The type that represents the element for which to register a component template.</param>
        /// <param name="uiElementAttr">The <see cref="UIElementAttribute"/> instance which is associated with the element type.</param>
        private void RegisterDefaultComponentTemplate(Type type, UIElementAttribute uiElementAttr)
        {
            if (String.IsNullOrEmpty(uiElementAttr.ComponentTemplate))
                return;

            var asm = type.Assembly;

            using (var stream = asm.GetManifestResourceStream(uiElementAttr.ComponentTemplate))
            {
                if (stream == null)
                    return;

                var template = XDocument.Load(stream);
                ComponentTemplates.SetDefault(type, template);
            }
        }

        // The component template manager.
        private readonly ComponentTemplateManager componentTemplateManager = 
            new ComponentTemplateManager();

        // The core element registry.
        private readonly Dictionary<String, RegisteredElement> coreElements = 
            new Dictionary<String, RegisteredElement>(StringComparer.OrdinalIgnoreCase);

        // The custom element registry.
        private readonly Dictionary<String, RegisteredElement> registeredElements = 
            new Dictionary<String, RegisteredElement>(StringComparer.OrdinalIgnoreCase);

        // The queues of elements with invalid layouts.
        private readonly LayoutQueue styleQueue = new LayoutQueue();
        private readonly LayoutQueue measureQueue = new LayoutQueue();
        private readonly LayoutQueue arrangeQueue = new LayoutQueue();
        private readonly LayoutQueue positionQueue = new LayoutQueue();
    }
}
