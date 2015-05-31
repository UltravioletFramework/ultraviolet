using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the state of the Ultraviolet Presentation Foundation.
    /// </summary>
    public partial class PresentationFoundation : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundation"/> class.
        /// </summary>
        internal PresentationFoundation(UltravioletContext uv)
            : base(uv)
        {
            RuntimeHelpers.RunClassConstructor(typeof(Tweening).TypeHandle);
            RuntimeHelpers.RunClassConstructor(typeof(SimpleClockPool).TypeHandle);
            RuntimeHelpers.RunClassConstructor(typeof(StoryboardClockPool).TypeHandle);

            RegisterCoreTypes();

            this.styleQueue    = new LayoutQueue(InvalidateStyle, false);
            this.measureQueue  = new LayoutQueue(InvalidateMeasure);
            this.arrangeQueue  = new LayoutQueue(InvalidateArrange);
        }

        /// <summary>
        /// Modifies the specified <see cref="UltravioletConfiguration"/> instance so that the Ultraviolet
        /// Presentation Foundation will be registered as the context's view provider.
        /// </summary>
        /// <param name="configuration">The <see cref="UltravioletConfiguration"/> instance to modify.</param>
        public static void Configure(UltravioletConfiguration configuration)
        {
            Contract.Require(configuration, "configuration");

            configuration.ViewProviderAssembly = typeof(PresentationFoundation).Assembly.FullName;
        }

        /// <summary>
        /// Updates the state of the Presentation Foundation.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            PerformanceStats.BeginFrame();

            ProcessStyleQueue();
            ProcessMeasureQueue();
            ProcessArrangeQueue();
        }

        /// <summary>
        /// Attempts to create an instance of the element with the specified name.
        /// </summary>
        /// <typeparam name="TViewModel">The type of view model to which the element will be bound.</typeparam>
        /// <param name="typeName">The name of the element to instantiate.</param>
        /// <param name="name">The ID with which to create the element.</param>
        /// <param name="bindingContext">The binding context to apply to the element which is instantiated.</param>
        /// <returns>The element that was created, or <c>null</c> if the element could not be created.</returns>
        public UIElement InstantiateElementByName<TViewModel>(String typeName, String name, String bindingContext = null)
        {
            return InstantiateElementByName(typeName, name, typeof(TViewModel), bindingContext);
        }

        /// <summary>
        /// Attempts to create an instance of the element with the specified name.
        /// </summary>
        /// <param name="typeName">The name of the element to instantiate.</param>
        /// <param name="name">The ID with which to create the element.</param>
        /// <param name="viewModelType">The type of view model to which the element will be bound.</param>
        /// <param name="bindingContext">The binding context to apply to the element which is instantiated.</param>
        /// <returns>The element that was created, or <c>null</c> if the element could not be created.</returns>
        public UIElement InstantiateElementByName(String typeName, String name, Type viewModelType, String bindingContext = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (bindingContext != null && !BindingExpressions.IsBindingExpression(bindingContext))
                throw new ArgumentException("bindingContext");

            KnownElement registration;
            if (!GetKnownElementRegistration(typeName, out registration))
                return null;

            var isFrameworkElement = typeof(FrameworkElement).IsAssignableFrom(registration.Type);

            var ctor = isFrameworkElement ?                
                registration.Type.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) }) :
                registration.Type.GetConstructor(new[] { typeof(UltravioletContext) });

            if (ctor == null)
                throw new InvalidOperationException(UltravioletStrings.NoValidConstructor.Format(registration.Type));

            var instance = (UIElement)ctor.Invoke(isFrameworkElement ? 
                new Object[] { Ultraviolet, name } : 
                new Object[] { Ultraviolet });

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

            KnownElement registration;
            if (!GetKnownElementRegistration(name, out registration))
                return false;

            return registration.Layout != null;
        }

        /// <summary>
        /// Gets the known type with the specified name.
        /// </summary>
        /// <param name="name">The name of the known type to retrieve.</param>
        /// <param name="type">The type associated with the specified name.</param>
        /// <returns><c>true</c> if the specified known type was retrieved; otherwise, <c>false</c>.</returns>
        public Boolean GetKnownType(String name, out Type type)
        {
            return GetKnownType(name, true, out type);
        }

        /// <summary>
        /// Gets the known type with the specified name.
        /// </summary>
        /// <param name="name">The name of the known type to retrieve.</param>
        /// <param name="isCaseSensitive">A value indicating whether the resolution of the type name is case-sensitive.</param>
        /// <param name="type">The type associated with the specified name.</param>
        /// <returns><c>true</c> if the specified known type was retrieved; otherwise, <c>false</c>.</returns>
        public Boolean GetKnownType(String name, Boolean isCaseSensitive, out Type type)
        {
            Contract.RequireNotEmpty(name, "name");
            Contract.EnsureNotDisposed(this, Disposed);

            type = null;

            KnownType registration;
            if (!GetKnownTypeRegistration(name, out registration))
                return false;

            if (isCaseSensitive && !String.Equals(name, registration.Name, StringComparison.Ordinal))
                return false;

            type = registration.Type;
            return true;
        }

        /// <summary>
        /// Gets the type associated with the specified element name.
        /// </summary>
        /// <param name="name">The name of the element for which to retrieve the associated type.</param>
        /// <param name="type">The type associated with the specified element.</param>
        /// <returns><c>true</c> if the specified element type was retrieved; otherwise, <c>false</c>.</returns>
        public Boolean GetKnownElement(String name, out Type type)
        {
            return GetKnownElement(name, true, out type);
        }

        /// <summary>
        /// Gets the type associated with the specified element name.
        /// </summary>
        /// <param name="name">The name of the element for which to retrieve the associated type.</param>
        /// <param name="isCaseSensitive">A value indicating whether the resolution of the element name is case-sensitive.</param>
        /// <param name="type">The type associated with the specified element.</param>
        /// <returns><c>true</c> if the specified element type was retrieved; otherwise, <c>false</c>.</returns>
        public Boolean GetKnownElement(String name, Boolean isCaseSensitive, out Type type)
        {
            Contract.RequireNotEmpty(name, "name");
            Contract.EnsureNotDisposed(this, Disposed);

            type = null;

            KnownElement registration;
            if (!GetKnownElementRegistration(name, out registration))
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

            KnownElement registration;
            if (!GetKnownElementRegistration(name, out registration))
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

            KnownElement registration;
            if (!GetKnownElementRegistration(type, out registration))
                return false;

            property = registration.DefaultProperty;
            return true;
        }

        /// <summary>
        /// Registers a custom element type with the Presentation Foundation.
        /// </summary>
        /// <param name="type">The type that implements the custom element.</param>
        public void RegisterElement(Type type)
        {
            Contract.Require(type, "type");
            Contract.EnsureNotDisposed(this, Disposed);

            RegisterElementInternal(registeredTypes, type, null);
        }

        /// <summary>
        /// Registers a custom element type with the Presentation Foundation.
        /// </summary>
        /// <param name="layout">The XML document that defines the custom element's layout.</param>
        public void RegisterElement(XDocument layout)
        {
            Contract.Require(layout, "layout");
            Contract.EnsureNotDisposed(this, Disposed);

            var type = ExtractElementTypeFromLayout(layout);

            RegisterElementInternal(registeredTypes, type, layout);
        }

        /// <summary>
        /// Unregisters a custom element.
        /// </summary>
        /// <param name="type">The type that implements the custom element.</param>
        /// <returns><c>true</c> if the custom element was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterKnownType(Type type)
        {
            Contract.Require(type, "type");
            Contract.EnsureNotDisposed(this, Disposed);

            KnownType registration;
            if (!GetKnownTypeRegistration(type, out registration))
                return false;

            return registeredTypes.Remove(registration.Name);
        }

        /// <summary>
        /// Unregisters a custom element.
        /// </summary>
        /// <param name="type">The element type to unregister.</param>
        /// <returns><c>true</c> if the custom element was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterKnownElement(Type type)
        {
            Contract.Require(type, "type");
            Contract.EnsureNotDisposed(this, Disposed);

            KnownElement registration;
            if (!GetKnownElementRegistration(type, out registration))
                return false;

            return registeredTypes.Remove(registration.Name);
        }

        /// <summary>
        /// Unregisters a custom element.
        /// </summary>
        /// <param name="layout">The XML document that defines the custom element's layout.</param>
        /// <returns><c>true</c> if the custom element was unregistered; otherwise, <c>false</c>.</returns>
        public Boolean UnregisterKnownElement(XDocument layout)
        {
            Contract.Require(layout, "layout");
            Contract.EnsureNotDisposed(this, Disposed);

            var type = ExtractElementTypeFromLayout(layout);

            KnownElement registration;
            if (!GetKnownElementRegistration(type, out registration))
                return false;

            return registeredTypes.Remove(registration.Name);
        }

        /// <summary>
        /// Sets the global style sheet used by all Presentation Foundation views.
        /// </summary>
        /// <param name="styleSheet">The global style sheet to set.</param>
        public void SetGlobalStyleSheet(UvssDocument styleSheet)
        {
            this.globalStyleSheet = styleSheet;
            OnGlobalStyleSheetChanged();
        }

        /// <summary>
        /// Gets the performance statistics which have been collected by the Ultraviolet Presentation Foundation.
        /// </summary>
        public PresentationFoundationPerformanceStats PerformanceStats
        {
            get { return performanceStats; }
        }

        /// <summary>
        /// Gets the Presentation Foundation's component template manager.
        /// </summary>
        public ComponentTemplateManager ComponentTemplates
        {
            get { return componentTemplateManager; }
        }

        /// <summary>
        /// Gets the current global style sheet.
        /// </summary>
        public UvssDocument GlobalStyleSheet
        {
            get { return globalStyleSheet; }
        }

        /// <summary>
        /// Occurs when the Presentation Foundation's global style sheet is changed.
        /// </summary>
        public event EventHandler GlobalStyleSheetChanged;

        /// <summary>
        /// Removes the specified UI element from all of the Foundation's processing queues.
        /// </summary>
        /// <param name="element">The element to remove from the queues.</param>
        internal void RemoveFromQueues(UIElement element)
        {
            Contract.Require(element, "element");

            StyleQueue.Remove(element);
            MeasureQueue.Remove(element);
            ArrangeQueue.Remove(element);
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
        /// <param name="attr">The element's <see cref="UvmlKnownTypeAttribute"/> instance.</param>
        /// <returns><c>true</c> if the specified type is a valid element type; otherwise, <c>false</c>.</returns>
        private static Boolean IsValidElementType(Type type, out UvmlKnownTypeAttribute attr)
        {
            attr = null;

            if (!typeof(UIElement).IsAssignableFrom(type))
                return false;

            attr = type.GetCustomAttributes(typeof(UvmlKnownTypeAttribute), false).Cast<UvmlKnownTypeAttribute>().SingleOrDefault();

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
                throw new ArgumentException(PresentationStrings.InvalidUserControlDefinition);

            var attr = layout.Root.Attribute("Type");
            if (attr == null)
                throw new InvalidOperationException(PresentationStrings.UserControlDoesNotDefineType);

            var type = Type.GetType(attr.Value, false);
            if (type == null)
                throw new InvalidOperationException(PresentationStrings.InvalidUserControlType.Format(attr.Value));

            UvmlKnownTypeAttribute uiElementAttr;
            if (!IsValidElementType(type, out uiElementAttr))
                throw new InvalidOperationException(PresentationStrings.InvalidUserControlType.Format(type.Name));

            return type;
        }

        /// <summary>
        /// Registers a custom element type with the Presentation Foundation.
        /// </summary>
        /// <param name="registry">The registry to which to add the element.</param>
        /// <param name="type">The type that implements the custom element.</param>
        /// <param name="layout">The XML document that defines the custom element's layout.</param>
        private void RegisterElementInternal(Dictionary<String, KnownType> registry, Type type, XDocument layout)
        {
            var knownTypeAttr = (UvmlKnownTypeAttribute)type.GetCustomAttributes(typeof(UvmlKnownTypeAttribute), false).SingleOrDefault();
            if (knownTypeAttr == null)
                throw new InvalidOperationException(PresentationStrings.KnownTypeMissingAttribute.Format(type.Name));

            var knownTypeName = knownTypeAttr.Name ?? type.Name;

            KnownType existingRegistration;
            if (GetKnownTypeRegistration(knownTypeName, out existingRegistration))
                throw new InvalidOperationException(PresentationStrings.KnownTypeAlreadyRegistered.Format(knownTypeName));

            RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            KnownType registration;
            if (typeof(UIElement).IsAssignableFrom(type))
            {
                registration = CreateKnownElementRegistration(type, knownTypeAttr);
            }
            else
            {
                registration = CreateKnownTypeRegistration(type, knownTypeAttr);
            }
            
            registry[registration.Name] = registration;
        }

        /// <summary>
        /// Registers any known types which are defined in the Presentation Foundation's core assembly.
        /// </summary>
        private void RegisterCoreTypes()
        {
            var knownTypes = from t in typeof(UIElement).Assembly.GetTypes()
                             let attr = t.GetCustomAttributes(typeof(UvmlKnownTypeAttribute), false).SingleOrDefault()
                             where
                              attr != null
                             select t;

            foreach (var knownType in knownTypes)
            {
                RegisterElementInternal(coreTypes, knownType, null);
            }
        }

        /// <summary>
        /// Creates a known type registration for the specified type.
        /// </summary>
        /// <param name="type">The type for which to create a registration.</param>
        /// <param name="attr">The attribute that marks the type as a known type.</param>
        /// <returns>The <see cref="KnownType"/> registration that was created.</returns>
        private KnownType CreateKnownTypeRegistration(Type type, UvmlKnownTypeAttribute attr)
        {
            var registration = new KnownType(attr.Name ?? type.Name, type);
            return registration;
        }

        /// <summary>
        /// Creates a known element registration for the specified type.
        /// </summary>
        /// <param name="type">The type for which to create a registration.</param>
        /// <param name="attr">The attribute that marks the type as a known type.</param>
        /// <returns>The <see cref="KnownType"/> registration that was created.</returns>
        private KnownType CreateKnownElementRegistration(Type type, UvmlKnownTypeAttribute attr)
        {
            var defaultPropertyAttr  = (DefaultPropertyAttribute)type.GetCustomAttributes(typeof(DefaultPropertyAttribute), true).SingleOrDefault();
            var defaultProperty      = default(String);
            if (defaultPropertyAttr != null)
            {
                defaultProperty = defaultPropertyAttr.Name;
            }

            var ctor = type.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) });
            if (ctor == null)
                throw new InvalidOperationException(PresentationStrings.UIElementInvalidCtor.Format(type.Name));

            RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            var registration = new KnownElement(attr.Name ?? type.Name, type, defaultProperty);
            RegisterDefaultComponentTemplate(type, attr);

            return registration;
        }

        /// <summary>
        /// Gets the registration for the specified known type.
        /// </summary>
        /// <param name="name">The name of the known type for which to retrieve a registration.</param>
        /// <param name="registration">The registration for the known type with the specified name.</param>
        /// <returns><c>true</c> if a known type with the specified name exists; otherwise, <c>false.</c></returns>
        private Boolean GetKnownTypeRegistration(String name, out KnownType registration)
        {
            if (coreTypes.TryGetValue(name, out registration))
                return true;

            if (registeredTypes.TryGetValue(name, out registration))
                return true;

            registration = null;
            return false;
        }

        /// <summary>
        /// Gets the registration for the specified known type.
        /// </summary>
        /// <param name="type">The CLR type of the known type for which to retrieve a registration.</param>
        /// <param name="registration">The registration for the known element associated with the specified CLR type.</param>
        /// <returns><c>true</c> if a known type associated with the specified CLR type exists; otherwise, <c>false.</c></returns>
        private Boolean GetKnownTypeRegistration(Type type, out KnownType registration)
        {
            foreach (var value in coreTypes.Values)
            {
                if (value.Type == type)
                {
                    registration = value;
                    return true;
                }
            }
            foreach (var value in registeredTypes.Values)
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
        /// Gets the registration for the specified known element.
        /// </summary>
        /// <param name="name">The name of the known element for which to retrieve a registration.</param>
        /// <param name="registration">The registration for the known element with the specified name.</param>
        /// <returns><c>true</c> if a known element with the specified name exists; otherwise, <c>false.</c></returns>
        private Boolean GetKnownElementRegistration(String name, out KnownElement registration)
        {
            KnownType typeRegistration;
            if (GetKnownTypeRegistration(name, out typeRegistration) && typeRegistration is KnownElement)
            {
                registration = (KnownElement)typeRegistration;
                return true;
            }
            registration = null;
            return false;
        }

        /// <summary>
        /// Gets the registration for the specified known element.
        /// </summary>
        /// <param name="type">The CLR type of the known element for which to retrieve a registration.</param>
        /// <param name="registration">The registration for the known element associated with the specified CLR type.</param>
        /// <returns><c>true</c> if a known element associated with the specified CLR type exists; otherwise, <c>false.</c></returns>
        private Boolean GetKnownElementRegistration(Type type, out KnownElement registration)
        {
            KnownType typeRegistration;
            if (GetKnownTypeRegistration(type, out typeRegistration) && typeRegistration is KnownElement)
            {
                registration = (KnownElement)typeRegistration;
                return true;
            }
            registration = null;
            return false;
        }

        /// <summary>
        /// Gets the registration for the specified known element.
        /// </summary>
        /// <param name="layout">The layout document of the known element for which to retrieve a registration.</param>
        /// <param name="registration">The registration for the known element with the specified layout.</param>
        /// <returns><c>true</c> if a known element associated with the specified layout exists; otherwise, <c>false</c>.</returns>
        private Boolean GetKnownElementRegistration(XDocument layout, out KnownElement registration)
        {
            var type = ExtractElementTypeFromLayout(layout);
            foreach (var value in coreTypes.Values)
            {
                if (value.Type == type && value is KnownElement)
                {
                    registration = (KnownElement)value;
                    return true;
                }
            }
            foreach (var value in registeredTypes.Values)
            {
                if (value.Type == type && value is KnownElement)
                {
                    registration = (KnownElement)value;
                    return true;
                }
            }
            registration = null;
            return false;
        }

        /// <summary>
        /// Raises the <see cref="GlobalStyleSheetChanged"/> event.
        /// </summary>
        private void OnGlobalStyleSheetChanged()
        {
            var temp = GlobalStyleSheetChanged;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
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

                element.Style(element.View.StyleSheet);
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
        /// Invalidates the specified element's style.
        /// </summary>
        private void InvalidateStyle(UIElement element)
        {
            if (element.IsStyleValid)
            {
                element.InvalidateStyleInternal();
                PerformanceStats.InvalidateStyleCountLastFrame++;
            }
        }

        /// <summary>
        /// Invalidates the specified element's measure.
        /// </summary>
        private void InvalidateMeasure(UIElement element)
        {
            if (element.IsMeasureValid)
            {
                element.InvalidateMeasureInternal();
                PerformanceStats.InvalidateMeasureCountLastFrame++;
            }
        }

        /// <summary>
        /// Invalidates the specified element's arrangement.
        /// </summary>
        private void InvalidateArrange(UIElement element)
        {
            if (element.IsArrangeValid)
            {
                element.InvalidateArrangeInternal();
                PerformanceStats.InvalidateArrangeCountLastFrame++;
            }
        }

        /// <summary>
        /// Registers the specified type's default component template, if it has one.
        /// </summary>
        /// <param name="type">The type that represents the element for which to register a component template.</param>
        /// <param name="uiElementAttr">The <see cref="UvmlKnownTypeAttribute"/> instance which is associated with the element type.</param>
        private void RegisterDefaultComponentTemplate(Type type, UvmlKnownTypeAttribute uiElementAttr)
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

        // Performance stats.
        private readonly PresentationFoundationPerformanceStats performanceStats = 
            new PresentationFoundationPerformanceStats();

        // The component template manager.
        private readonly ComponentTemplateManager componentTemplateManager = 
            new ComponentTemplateManager();

        // The core type registry.
        private readonly Dictionary<String, KnownType> coreTypes = 
            new Dictionary<String, KnownType>(StringComparer.OrdinalIgnoreCase);

        // The custom type registry.
        private readonly Dictionary<String, KnownType> registeredTypes = 
            new Dictionary<String, KnownType>(StringComparer.OrdinalIgnoreCase);

        // The queues of elements with invalid layouts.
        private readonly LayoutQueue styleQueue;
        private readonly LayoutQueue measureQueue;
        private readonly LayoutQueue arrangeQueue;

        // The global style sheet.
        private UvssDocument globalStyleSheet;
    }
}
