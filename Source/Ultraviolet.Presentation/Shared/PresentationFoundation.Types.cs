using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    partial class PresentationFoundation
    {
        /// <summary>
        /// Gets the collection of types which are known to the Presentation Foundation.
        /// </summary>
        /// <returns>A dictionary containing the types which are known to the Presentation Foundation, using
        /// the UVML names of the types as the dictionary key.</returns>
        public IDictionary<String, Type> GetKnownTypes()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var types = new Dictionary<String, Type>();

            foreach (var kvp in coreTypes)
                types[kvp.Key] = kvp.Value.Type;

            foreach (var kvp in registeredTypes)
                types[kvp.Key] = kvp.Value.Type;

            return types;
        }

        /// <summary>
        /// Registers any UVML-known types in the specified assembly.
        /// </summary>
        /// <param name="asm">The assembly for which to register known types.</param>
        public void RegisterKnownTypes(Assembly asm)
        {
            Contract.Require(asm, nameof(asm));

            var knownTypes = from t in asm.GetTypes()
                             let attr = t.GetCustomAttributes(typeof(UvmlKnownTypeAttribute), false).SingleOrDefault()
                             where
                              attr != null
                             select t;

            foreach (var knownType in knownTypes)
            {
                RegisterElementInternal(registeredTypes, knownType, null);
            }
        }

        /// <summary>
        /// Registers a custom element type with the Presentation Foundation.
        /// </summary>
        /// <param name="type">The type that implements the custom element.</param>
        public void RegisterElement(Type type)
        {
            Contract.Require(type, nameof(type));
            Contract.EnsureNotDisposed(this, Disposed);

            RegisterElementInternal(registeredTypes, type, null);
        }

        /// <summary>
        /// Unregisters a custom element.
        /// </summary>
        /// <param name="type">The type that implements the custom element.</param>
        /// <returns><see langword="true"/> if the custom element was unregistered; otherwise, <see langword="false"/>.</returns>
        public Boolean UnregisterKnownType(Type type)
        {
            Contract.Require(type, nameof(type));
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
        /// <returns><see langword="true"/> if the custom element was unregistered; otherwise, <see langword="false"/>.</returns>
        public Boolean UnregisterKnownElement(Type type)
        {
            Contract.Require(type, nameof(type));
            Contract.EnsureNotDisposed(this, Disposed);

            KnownElement registration;
            if (!GetKnownElementRegistration(type, out registration))
                return false;

            return registeredTypes.Remove(registration.Name);
        }

        /// <summary>
        /// Gets the known type with the specified name.
        /// </summary>
        /// <param name="name">The name of the known type to retrieve.</param>
        /// <param name="type">The type associated with the specified name.</param>
        /// <returns><see langword="true"/> if the specified known type was retrieved; otherwise, <see langword="false"/>.</returns>
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
        /// <returns><see langword="true"/> if the specified known type was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetKnownType(String name, Boolean isCaseSensitive, out Type type)
        {
            Contract.RequireNotEmpty(name, nameof(name));
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
        /// <returns><see langword="true"/> if the specified element type was retrieved; otherwise, <see langword="false"/>.</returns>
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
        /// <returns><see langword="true"/> if the specified element type was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetKnownElement(String name, Boolean isCaseSensitive, out Type type)
        {
            Contract.RequireNotEmpty(name, nameof(name));
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
        /// <returns><see langword="true"/> if the specified element's default property was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetElementDefaultProperty(String name, out String property)
        {
            Contract.RequireNotEmpty(name, nameof(name));
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
        /// <returns><see langword="true"/> if the specified element's default property was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetElementDefaultProperty(Type type, out String property)
        {
            Contract.Require(type, nameof(type));
            Contract.EnsureNotDisposed(this, Disposed);

            property = null;

            KnownElement registration;
            if (!GetKnownElementRegistration(type, out registration))
                return false;

            property = registration.DefaultProperty;
            return true;
        }
        
        /// <summary>
        /// Attempts to create an instance of the element with the specified name.
        /// </summary>
        /// <typeparam name="TViewModel">The type of view model to which the element will be bound.</typeparam>
        /// <param name="typeName">The name of the element to instantiate.</param>
        /// <param name="name">The ID with which to create the element.</param>
        /// <returns>The element that was created, or <see langword="null"/> if the element could not be created.</returns>
        public UIElement InstantiateElementByName<TViewModel>(String typeName, String name)
        {
            return InstantiateElementByName(typeName, name, typeof(TViewModel));
        }

        /// <summary>
        /// Attempts to create an instance of the element with the specified name.
        /// </summary>
        /// <param name="typeName">The name of the element to instantiate.</param>
        /// <param name="name">The ID with which to create the element.</param>
        /// <param name="viewModelType">The type of view model to which the element will be bound.</param>
        /// <returns>The element that was created, or <see langword="null"/> if the element could not be created.</returns>
        public UIElement InstantiateElementByName(String typeName, String name, Type viewModelType)
        {
            Contract.EnsureNotDisposed(this, Disposed);

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

            return instance;
        }

        /// <summary>
        /// Gets the Presentation Foundation's component template manager.
        /// </summary>
        public ComponentTemplateManager ComponentTemplates
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return componentTemplateManager;
            }
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

                var template = XDocument.Load(stream, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
                ComponentTemplates.SetDefault(type, template);
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
            var defaultPropertyAttr = (UvmlDefaultPropertyAttribute)type.GetCustomAttributes(typeof(UvmlDefaultPropertyAttribute), true).SingleOrDefault();
            var defaultProperty = default(String);
            if (defaultPropertyAttr != null)
            {
                defaultProperty = defaultPropertyAttr.Name;
            }

            var ctor = type.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) });
            if (ctor == null && !type.IsAbstract)
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
        /// <returns><see langword="true"/> if a known type with the specified name exists; otherwise, <c>false.</c></returns>
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
        /// <returns><see langword="true"/> if a known type associated with the specified CLR type exists; otherwise, <c>false.</c></returns>
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
        /// <returns><see langword="true"/> if a known element with the specified name exists; otherwise, <c>false.</c></returns>
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
        /// <returns><see langword="true"/> if a known element associated with the specified CLR type exists; otherwise, <c>false.</c></returns>
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

        // The component template manager.
        private readonly ComponentTemplateManager componentTemplateManager =
            new ComponentTemplateManager();

        // The registry of known types.
        private readonly Dictionary<String, KnownType> coreTypes =
            new Dictionary<String, KnownType>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<String, KnownType> registeredTypes =
            new Dictionary<String, KnownType>(StringComparer.OrdinalIgnoreCase);
    }
}
