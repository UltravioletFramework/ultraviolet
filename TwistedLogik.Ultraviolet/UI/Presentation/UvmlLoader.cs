using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains methods for loading UI elements from UVML.
    /// </summary>
    internal static partial class UvmlLoader
    {
        /// <summary>
        /// Initializes the <see cref="UvmlLoader"/> type.
        /// </summary>
        static UvmlLoader()
        {
            miBindValue     = typeof(DependencyObject).GetMethod("BindValue");
            miSetLocalValue = typeof(DependencyObject).GetMethod("SetLocalValue");
            miGetValue      = typeof(DependencyObject).GetMethod("GetValue");
        }

        /// <summary>
        /// Loads an instance of the <see cref="UIView"/> from an XML node.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="xml">The <see cref="XElement"/> from which to load the view.</param>
        /// <returns>The <see cref="UIView"/> that was loaded from the specified XML element.</returns>
        public static UIView Load(UltravioletContext uv, XElement xml)
        {
            var viewModelType      = default(Type);
            var viewModelTypeAttr  = xml.Attribute("ViewModelType");
            if (viewModelTypeAttr != null)
            {
                viewModelType = Type.GetType(viewModelTypeAttr.Value, false);
                if (viewModelType == null)
                {
                    throw new InvalidOperationException(UltravioletStrings.ViewModelTypeNotFound.Format(viewModelTypeAttr.Value));
                }
            }

            var view    = new UIView(uv, viewModelType);
            var context = new InstantiationContext(viewModelType);

            PopulateElement(uv, view.LayoutRoot, xml, context);

            return view;
        }

        /// <summary>
        /// Initializes an instance of <see cref="UserControl"/> from the specified layout definition.
        /// </summary>
        /// <typeparam name="TViewModelType">The type of view model to which the user control will be bound.</typeparam>
        /// <param name="userControl">The instance of <see cref="UserControl"/> to initialize.</param>
        /// <param name="layout">The XML document that specifies the control's layout.</param>
        /// <param name="bindingContext">The binding context for the user control, if any.</param>
        public static void LoadUserControl<TViewModelType>(UserControl userControl, XDocument layout, String bindingContext = null)
        {
            LoadUserControl(userControl, layout, typeof(TViewModelType), bindingContext);
        }

        /// <summary>
        /// Initializes an instance of <see cref="UserControl"/> from the specified layout definition.
        /// </summary>
        /// <param name="userControl">The instance of <see cref="UserControl"/> to initialize.</param>
        /// <param name="layout">The XML document that specifies the control's layout.</param>
        /// <param name="viewModelType">The type of view model to which the user control will be bound.</param>
        /// <param name="bindingContext">The binding context for the user control, if any.</param>
        public static void LoadUserControl(UserControl userControl, XDocument layout, Type viewModelType, String bindingContext = null)
        {
            if (bindingContext != null && !BindingExpressions.IsBindingExpression(bindingContext))
                throw new ArgumentException(UltravioletStrings.InvalidBindingContext.Format(bindingContext));

            var contentElement = layout.Root.Elements().SingleOrDefault();
            if (contentElement == null)
                return;

            var uv      = userControl.Ultraviolet;
            var context = new InstantiationContext(viewModelType, userControl, bindingContext);
            var content = InstantiateAndPopulateElement(uv, null, contentElement, context);

            userControl.ComponentRoot = content;
            userControl.PopulateFieldsFromRegisteredElements();
        }

        /// <summary>
        /// Loads the component root of the specified control.
        /// </summary>
        /// <param name="control">The instance of <see cref="Control"/> for which to load a component root.</param>
        /// <param name="template">The component template that specified the control's component layout.</param>
        /// <param name="viewModelType">The type of view model to which the user control will be bound.</param>
        /// <param name="bindingContext">The binding context for the user control, if any.</param>
        public static void LoadComponentRoot(Control control, XDocument template, Type viewModelType, String bindingContext = null)
        {
            if (bindingContext != null && !BindingExpressions.IsBindingExpression(bindingContext))
                throw new ArgumentException(UltravioletStrings.InvalidBindingContext.Format(bindingContext));

            var rootElement = template.Root.Elements().SingleOrDefault();
            if (rootElement == null)
                return;

            var uv      = control.Ultraviolet;
            var context = new InstantiationContext(viewModelType, control, bindingContext);
            var root    = (Container)InstantiateAndPopulateElement(uv, null, rootElement, context);

            control.ComponentRoot = root;
            control.PopulateFieldsFromRegisteredElements();
            control.ContentElement = control.ComponentRoot.FindContentPanel();
        }

        /// <summary>
        /// Instantiates a new interface element.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="parent">The element which is the element's parent.</param>
        /// <param name="xmlElement">The XML element that defines the element to instantiate.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns>The interface element that was instantiated.</returns>
        private static UIElement InstantiateElement(UltravioletContext uv, UIElement parent, XElement xmlElement, InstantiationContext context)
        {
            var id        = xmlElement.AttributeValueString("ID");
            var classes   = xmlElement.AttributeValueString("Class");
            var classList = (classes == null) ? Enumerable.Empty<String>() : classes.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var instance = uv.GetUI().PresentationFramework.InstantiateElementByName(xmlElement.Name.LocalName, id, context.ViewModelType, context.BindingContext);
            if (instance == null)
                throw new UvmlException(UltravioletStrings.UnrecognizedUIElement.Format(xmlElement.Name.LocalName));

            foreach (var className in classList)
            {
                instance.Classes.Add(className);
            }

            var container = parent as Container;
            if (container != null)
                container.Children.Add(instance);

            var contentControl = parent as ContentControl;
            if (contentControl != null)
                contentControl.Content = instance;

            return instance;
        }

        /// <summary>
        /// Instantiates a new interface element.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="parent">The element which is the element's parent.</param>
        /// <param name="xmlElement">The XML element that defines the element to instantiate.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns>The interface element that was instantiated.</returns>
        private static UIElement InstantiateAndPopulateElement(UltravioletContext uv, UIElement parent, XElement xmlElement, InstantiationContext context)
        {
            var bindingContext        = xmlElement.AttributeValueString("BindingContext");
            var bindingContextDefined = (bindingContext != null);
            if (bindingContextDefined)
            {
                if (!BindingExpressions.IsBindingExpression(bindingContext))
                    throw new InvalidOperationException(UltravioletStrings.InvalidBindingContext.Format(bindingContext));

                context.PushBindingContext(bindingContext);
            }

            var element = InstantiateElement(uv, parent, xmlElement, context);
            PopulateElement(uv, element, xmlElement, context);

            if (bindingContextDefined)
            {
                context.PopBindingContext();
            }

            return element;
        }

        /// <summary>
        /// Finds the identifier of the specified dependency property on the specified UI element.
        /// </summary>
        /// <param name="uiElement">The UI element to search for the dependency property.</param>
        /// <param name="name">The name of the dependency property for which to search.</param>
        /// <returns>The identifier of the specified dependency property, or <c>null</c> if no such property was found.</returns>
        private static DependencyProperty FindElementDependencyProperty(UIElement uiElement, String name)
        {
            var attachedContainer = String.Empty;
            var attachedProperty  = String.Empty;
            if (IsAttachedProperty(name, out attachedContainer, out attachedProperty))
            {
                if (uiElement.Parent != null && String.Equals(uiElement.Parent.Name, attachedContainer, StringComparison.InvariantCulture))
                {
                    return DependencyProperty.FindByName(attachedProperty, uiElement.Parent.GetType());
                }
            }
            else
            {
                return DependencyProperty.FindByName(name, uiElement.GetType());
            }

            return null;
        }

        /// <summary>
        /// Finds the property information for the specified standard property on the specified UI element.
        /// </summary>
        /// <param name="uiElement">The UI element to search for the standard property.</param>
        /// <param name="name">The name of the standard property for which to search.</param>
        /// <returns>The <see cref="PropertyInfo"/> for the specified property.</returns>
        private static PropertyInfo FindElementStandardProperty(UIElement uiElement, String name)
        {
            return FindElementStandardProperty(uiElement, name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// Finds the property information for the specified standard property on the specified UI element.
        /// </summary>
        /// <param name="uiElement">The UI element to search for the standard property.</param>
        /// <param name="name">The name of the standard property for which to search.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>The <see cref="PropertyInfo"/> for the specified property.</returns>
        private static PropertyInfo FindElementStandardProperty(UIElement uiElement, String name, BindingFlags bindingAttr)
        {
            var standardProperty = uiElement.GetType().GetProperty(name,  bindingAttr);
            if (standardProperty == null)
                throw new InvalidOperationException(UltravioletStrings.PropertyDoesNotExist.Format(name, uiElement.GetType()));

            return standardProperty;
        }

        /// <summary>
        /// Gets the type of the specified property.
        /// </summary>
        /// <param name="uiElement">The UI element to search for the specified property.</param>
        /// <param name="name">The name of the property for which to search.</param>
        /// <returns>The type of the specified property.</returns>
        private static Type FindPropertyType(UIElement uiElement, String name)
        {
            var dprop = FindElementDependencyProperty(uiElement, name);
            if (dprop != null)
            {
                return dprop.PropertyType;
            }

            var sprop = FindElementStandardProperty(uiElement, name);
            return sprop.PropertyType;
        }

        /// <summary>
        /// Gets a value indicating whether the specified XML element's name represents
        /// a property on a UI element.
        /// </summary>
        /// <param name="element">The XML element to evaluate.</param>
        /// <returns><c>true</c> if the specified XML element's name represents a property on
        /// a UI element; otherwise, <c>false</c>.</returns>
        private static Boolean ElementNameRepresentsProperty(XElement element)
        {
            return element.Name.LocalName.Contains('.');
        }

        /// <summary>
        /// Gets a value indicating whether the specified name corresponds to an event on the specified object.
        /// </summary>
        /// <param name="dobj">The dependency object to evaluate.</param>
        /// <param name="name">The name to evaluate.</param>
        /// <returns><c>true</c> if the specified name corresponds to an event; otherwise, <c>false</c>.</returns>
        private static Boolean IsEvent(DependencyObject dobj, String name)
        {
            return (dobj.GetType().GetEvent(name) != null);
        }

        /// <summary>
        /// Gets a value indicating whether the specified attribute name is reserved by the UVML loader.
        /// </summary>
        /// <param name="name">The name to evaluate.</param>
        /// <returns><c>true</c> if the specified attribute name is reserved; otherwise, <c>false</c>.</returns>
        private static Boolean IsReservedAttribute(String name)
        {
            switch (name)
            {
                case "ID":
                case "Class":
                case "ViewModelType":
                case "BindingContext":
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified type represents an enumerable collection.
        /// </summary>
        /// <param name="enumType">The type to evaluate.</param>
        /// <param name="itemType">The type of item contained by the collection.</param>
        /// <returns><c>true</c> if the specified type represents an enumerable collection; otherwise, <c>false</c>.</returns>
        private static Boolean IsTypeAnEnumerableCollection(Type enumType, out Type itemType)
        {
            var ifaces = enumType.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (ifaces.Count() > 1 || !ifaces.Any())
            {
                itemType = null;
                return false;
            }

            var iface = ifaces.Single();
            itemType = iface.GetGenericArguments()[0];
            return true;
        }
        
        /// <summary>
        /// Resolves dependency properties when loading objects through the Nucleus object serializer.
        /// </summary>
        /// <param name="obj">The object which is being populated.</param>
        /// <param name="name">The name of the property which is being populated.</param>
        /// <param name="value">The value which is being resolved.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns><c>true</c> if the property was resolved; otherwise, <c>false</c>.</returns>
        private static Boolean DependencyPropertyResolutionHandler(Object obj, String name, String value, InstantiationContext context)
        {
            var dobj = obj as DependencyObject;
            if (dobj == null)
                return false;

            var dprop = DependencyProperty.FindByName(name, dobj.GetType());
            if (dprop == null)
                return false;

            BindOrSetDependencyProperty(dobj, dprop, value, context);

            return true;
        }

        /// <summary>
        /// Loads an object using the Nucleus object serializer.
        /// </summary>
        /// <param name="type">The type of object to load.</param>
        /// <param name="value">The XML element to load as an object.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns>The object that was loaded.</returns>
        private static Object LoadObjectWithSerializer(Type type, XElement value, InstantiationContext context)
        {
            return ObjectLoader.LoadObject((loaderObj, loaderName, loaderValue) =>
            {
                return DependencyPropertyResolutionHandler(loaderObj, loaderName, loaderValue, context);
            }, type, value);
        }

        /// <summary>
        /// Populates a UI element's events, properties, and children.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose dependency property values will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElement(UltravioletContext uv, UIElement uiElement, XElement xmlElement, InstantiationContext context)
        {
            PopulateElementProperties(uv, uiElement, xmlElement, context);
            PopulateElementEvents(uv, uiElement, xmlElement, context);
            PopulateElementChildren(uv, uiElement, xmlElement, context);
        }

        /// <summary>
        /// Populates the specified element's events.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose dependency property values will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElementEvents(UltravioletContext uv, UIElement uiElement, XElement xmlElement, InstantiationContext context)
        {
            foreach (var attr in xmlElement.Attributes())
            {
                var attrName = attr.Name.LocalName;
                if (attrName == "BindingContext")
                    continue;

                var attrEvent = uiElement.GetType().GetEvent(attrName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (attrEvent == null || String.IsNullOrEmpty(attr.Value))
                    continue;

                Delegate lambda;
                if (context.ComponentOwner != null)
                {
                    lambda = BindingExpressions.CreateElementBoundEventDelegate(context.ComponentOwner, context.ViewModelType, 
                        attrEvent.EventHandlerType, attr.Value);
                }
                else
                {
                    lambda = BindingExpressions.CreateViewModelBoundEventDelegate(uiElement, context.ViewModelType, 
                        attrEvent.EventHandlerType, attr.Value);
                }
                attrEvent.AddEventHandler(uiElement, lambda);
            }
        }

        /// <summary>
        /// Populates the specified element's property values.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose property values will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElementProperties(UltravioletContext uv, UIElement uiElement, XElement xmlElement, InstantiationContext context)
        {
            PopulateElementPropertiesFromAttributes(uv, uiElement, xmlElement, context);
            PopulateElementPropertiesFromElements(uv, uiElement, xmlElement, context);
            PopulateElementDefaultProperty(uv, uiElement, xmlElement, context);
        }

        /// <summary>
        /// Populates the specified element's property values using the values of the specified
        /// XML element's attributes.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose property values will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElementPropertiesFromAttributes(UltravioletContext uv, UIElement uiElement, XElement xmlElement, InstantiationContext context)
        {
            foreach (var attr in xmlElement.Attributes())
            {
                var attrName = attr.Name.LocalName;
                if (IsReservedAttribute(attrName))
                    continue;

                PopulateElementProperty(uiElement, attrName, attr.Value, context);
            }
        }

        /// <summary>
        /// Populates the specified element's property values using the values of the specified
        /// XML element's child elements.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose property values will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElementPropertiesFromElements(UltravioletContext uv, UIElement uiElement, XElement xmlElement, InstantiationContext context)
        {            
            foreach (var element in xmlElement.Elements())
            {
                var elementName = element.Name.LocalName;
                if (IsReservedAttribute(elementName) || !ElementNameRepresentsProperty(element))
                    continue;

                var propDelimIndex = elementName.IndexOf('.');
                var propOwnerName  = elementName.Substring(0, propDelimIndex);
                var propName       = elementName.Substring(propDelimIndex + 1);
                var isAttached     = !String.Equals(propOwnerName, uiElement.Name, StringComparison.InvariantCulture);
                if (isAttached)
                    propName = elementName;

                // Special case handling for implementations of IEnumerable<T>: we create the
                // collection's items from this XML element's children.
                var propType = FindPropertyType(uiElement, propName);
                var itemType = default(Type);
                if (IsTypeAnEnumerableCollection(propType, out itemType))
                {
                    PopulateEnumerableCollection(uiElement, propName, element, propType, itemType, context);
                    continue;
                }

                // If the element has child elements, then this must be a complex object;
                // therefore we need to use Nucleus' XML serializer to load it.
                // If the element is empty (i.e. has no value), then all we can do is create
                // the object using a default constructor. Nucleus will also handle this case.
                if (element.Elements().Any() || element.IsEmpty)
                {
                    PopulateElementPropertyWithSerializer(uiElement, propName, element, context);
                    continue;
                }

                PopulateElementProperty(uiElement, propName, element.Value, context);
            }
        }

        /// <summary>
        /// Populates the specified property by deserializing an object from the specified XML element.
        /// </summary>
        /// <param name="uiElement">The element whose property value will be populated.</param>
        /// <param name="name">The name of the property being populated.</param>
        /// <param name="value">The XML element containing the value to deserialize.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElementPropertyWithSerializer(UIElement uiElement, String name, XElement value, InstantiationContext context)
        {
            var dprop = FindElementDependencyProperty(uiElement, name);
            if (dprop != null)
            {
                var propValue = LoadObjectWithSerializer(dprop.PropertyType, value, context);
                SetDependencyProperty(uiElement, dprop, propValue);
            }
            else
            {
                var propInfo = FindElementStandardProperty(uiElement, name);
                if (!propInfo.CanWrite)
                    throw new InvalidOperationException(UltravioletStrings.PropertyHasNoSetter.Format(name));

                var propValue = LoadObjectWithSerializer(propInfo.PropertyType, value, context);
                propInfo.SetValue(uiElement, propValue, null);
            }
        }

        /// <summary>
        /// Populates the specified properties of a UI element with the specified value.
        /// </summary>
        /// <param name="uiElement">The element whose property value will be populated.</param>
        /// <param name="name">The name of the property to populate.</param>
        /// <param name="value">The value to set in the specified property.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElementProperty(UIElement uiElement, String name, String value, InstantiationContext context)
        {
            var dprop = FindElementDependencyProperty(uiElement, name);
            if (dprop != null)
            {
                BindOrSetDependencyProperty(uiElement, dprop, value, context);
            }
            else
            {
                if (IsEvent(uiElement, name))
                    return;

                var propInfo = FindElementStandardProperty(uiElement, name);
                if (!propInfo.CanWrite)
                    throw new InvalidOperationException(UltravioletStrings.PropertyHasNoSetter.Format(name));
                
                var propValue = ObjectResolver.FromString(value, propInfo.PropertyType);
                propInfo.SetValue(uiElement, propValue, null);
            }
        }

        /// <summary>
        /// Populates a property on the specified UI element which is an enumerable collection.
        /// </summary>
        /// <param name="uiElement">The element whose property value will be populated.</param>
        /// <param name="name">The name of the property to populate.</param>
        /// <param name="value">The XML element that represents the value of the property.</param>
        /// <param name="enumType">The type of enumerable collection to instantiate.</param>
        /// <param name="itemType">The type of item contained by the enumerable collection.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateEnumerableCollection(UIElement uiElement, String name, XElement value, Type enumType, Type itemType, InstantiationContext context)
        {
            // Deserialize the collection's items from XML.
            var itemElements = value.Elements(itemType.Name).ToList();
            if (itemElements.Count != value.Elements().Count())
                throw new InvalidOperationException(UltravioletStrings.CollectionContainsInvalidElements.Format(name));

            var items = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
            foreach (var itemElement in itemElements)
            {
                var item = LoadObjectWithSerializer(itemType, itemElement, context);
                items.Add(item);
            }

            // Check if an instance already exists, and if so, try to clear it, then use it
            // as our instance instead of creating a new one.
            var existingValue = (IEnumerable)GetPropertyValue(uiElement, name);
            if (existingValue != null)
            {
                var clearMethod = existingValue.GetType().GetMethod("Clear", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, 
                    null, Type.EmptyTypes, null);

                if (clearMethod == null)
                    throw new InvalidOperationException(UltravioletStrings.CollectionCannotBeCleared.Format(name));

                clearMethod.Invoke(existingValue, null);
            }

            // If an instance doesn't already exist, attempt to instantiate one.
            var enumInstance = existingValue;
            if (enumInstance == null)
            {
                var ctor = 
                    enumType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(IEnumerable<>).MakeGenericType(itemType) }, null) ??
                    enumType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);

                if (ctor == null)
                    throw new InvalidOperationException(UltravioletStrings.NoValidConstructor.Format(enumType.Name));

                if (ctor.GetParameters().Length == 0)
                {
                    enumInstance = (IEnumerable)ctor.Invoke(null);
                    PopulateEnumerableCollectionItems(enumInstance, items, enumType, itemType);
                }
                else
                {
                    enumInstance = (IEnumerable)ctor.Invoke(new Object[] { items });
                }

                SetPropertyValue(uiElement, name, enumInstance);
            }
            else
            {
                PopulateEnumerableCollectionItems(enumInstance, items, enumType, itemType);
            }
        }

        /// <summary>
        /// Adds the specified set of items to a collection.
        /// </summary>
        /// <param name="collection">The collection to populate with items.</param>
        /// <param name="items">The set of items to add to the collection.</param>
        /// <param name="enumType">The type of enumerable collection.</param>
        /// <param name="itemType">The type of item contained by the enumerable collection.</param>
        private static void PopulateEnumerableCollectionItems(IEnumerable collection, IEnumerable items, Type enumType, Type itemType)
        {
            var addMethod = 
                enumType.GetMethod("Add", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { itemType }, null) ?? 
                enumType.GetMethod("Add", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(Object) }, null);

            if (addMethod == null)
                throw new InvalidOperationException(UltravioletStrings.CollectionHasNoAddMethod.Format(enumType.Name));

            foreach (var item in items)
                addMethod.Invoke(collection, new Object[] { item });
        }

        /// <summary>
        /// Populates the specified element's default property value.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose default property value will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElementDefaultProperty(UltravioletContext uv, UIElement uiElement, XElement xmlElement, InstantiationContext context)
        {
            String defaultProperty;
            if (!uv.GetUI().PresentationFramework.GetElementDefaultProperty(uiElement.GetType(), out defaultProperty))
                return;

            if (defaultProperty != null && !String.IsNullOrEmpty(xmlElement.Value))
            {
                var dprop = DependencyProperty.FindByName(defaultProperty, uiElement.GetType());
                if (dprop == null)
                    throw new InvalidOperationException(UltravioletStrings.InvalidDefaultProperty.Format(defaultProperty, uiElement.GetType()));

                var value = String.Join(String.Empty, xmlElement.Nodes().Where(x => x is XText).Select(x => ((XText)x).Value.Trim()));
                if (!String.IsNullOrEmpty(value))
                {
                    BindOrSetDependencyProperty(uiElement, dprop, value, context);
                }
            }
        }

        /// <summary>
        /// Populates the specified container with children.
        /// </summary>
        /// <param name="uv">The Ultraviolet container.</param>
        /// <param name="uiElement">The element to populate with children.</param>
        /// <param name="xmlElement">The XML element that represents the UI container.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElementChildren(UltravioletContext uv, UIElement uiElement, XElement xmlElement, InstantiationContext context)
        {
            var xmlChildren = xmlElement.Elements().Where(x => !ElementNameRepresentsProperty(x)).ToList();
            
            var container = uiElement as Container;
            if (container != null)
            {
                foreach (var child in xmlChildren)
                {                    
                    InstantiateAndPopulateElement(uv, uiElement, child, context);
                }
            }
            else
            {
                var contentControl = uiElement as ContentControl;
                if (contentControl != null && !(uiElement is UserControl))
                {
                    if (xmlChildren.Count > 1)
                    {
                        var id = uiElement.ID ?? uiElement.GetType().Name;
                        throw new InvalidOperationException(UltravioletStrings.InvalidChildElements.Format(id));
                    }

                    var contentElement = xmlChildren.SingleOrDefault();
                    if (contentElement != null)
                    {
                        InstantiateAndPopulateElement(uv, uiElement, contentElement, context);
                    }
                }
                else
                {
                    if (xmlChildren.Any())
                    {
                        var id = uiElement.ID ?? uiElement.GetType().Name;
                        throw new InvalidOperationException(UltravioletStrings.InvalidChildElements.Format(id));
                    }
                }
            }
        }

        /// <summary>
        /// Binds or sets the specified dependency property, depending on whether the given value is a binding expression.
        /// </summary>
        /// <param name="dobj">The dependency object to modify.</param>
        /// <param name="dprop">The dependency property to bind or set.</param>
        /// <param name="value">The binding expression or value to set on the property.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void BindOrSetDependencyProperty(DependencyObject dobj, DependencyProperty dprop, String value, InstantiationContext context)
        {
            if (IsBindingExpression(value))
            {
                BindDependencyProperty(dobj, dprop, value, context);
            }
            else
            {
                SetDependencyProperty(dobj, dprop, value);
            }
        }

        /// <summary>
        /// Sets the local value of the specified dependency property.
        /// </summary>
        /// <param name="dobj">The dependency object to modify.</param>
        /// <param name="dprop">The dependency property to bind or set.</param>
        /// <param name="value">The value to set on the property.</param>
        private static void SetDependencyProperty(DependencyObject dobj, DependencyProperty dprop, String value)
        {
            try
            {
                var type          = dprop.PropertyType;
                var resolvedValue = ObjectResolver.FromString(value, type);

                miSetLocalValue.MakeGenericMethod(type).Invoke(dobj, new Object[] { dprop, resolvedValue });
            }
            catch (FormatException e)
            {
                throw new InvalidOperationException(UltravioletStrings.InvalidElementPropertyValue.Format(value, dprop.Name), e);
            }
        }

        /// <summary>
        /// Sets the local value of the specified dependency property.
        /// </summary>
        /// <param name="dobj">The dependency object to modify.</param>
        /// <param name="dprop">The dependency property to bind or set.</param>
        /// <param name="value">The value to set on the property.</param>
        private static void SetDependencyProperty(DependencyObject dobj, DependencyProperty dprop, Object value)
        {
            miSetLocalValue.MakeGenericMethod(dprop.PropertyType).Invoke(dobj, new Object[] { dprop, value });
        }

        /// <summary>
        /// Binds the specified dependency property.
        /// </summary>
        /// <param name="dobj">The dependency object to modify.</param>
        /// <param name="dprop">The dependency property to bind or set.</param>
        /// <param name="expression">The binding expression to set on the property.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void BindDependencyProperty(DependencyObject dobj, DependencyProperty dprop, String expression, InstantiationContext context)
        {
            if (context.ViewModelType == null)
                throw new InvalidOperationException(UltravioletStrings.NoViewModel);

            var expressionType = dprop.PropertyType;
            var expressionFull = BindingExpressions.Combine(context.BindingContext, expression);

            miBindValue.Invoke(dobj, new Object[] { dprop, context.ViewModelType, expressionFull });
        }

        /// <summary>
        /// Sets the value of the specified property.
        /// </summary>
        /// <param name="uiElement">The UI element on which to set the property value.</param>
        /// <param name="name">The name of the property to set.</param>
        /// <param name="value">The value to set for the specified property.</param>
        private static void SetPropertyValue(UIElement uiElement, String name, Object value)
        {
            var dprop = FindElementDependencyProperty(uiElement, name);
            if (dprop != null)
            {
                SetDependencyProperty(uiElement, dprop, value);
            }
            else
            {
                var propInfo = FindElementStandardProperty(uiElement, name);
                if (!propInfo.CanWrite)
                    throw new InvalidOperationException(UltravioletStrings.PropertyHasNoSetter.Format(name));

                propInfo.SetValue(uiElement, value, null);
            }
        }

        /// <summary>
        /// Gets the value of the specified property.
        /// </summary>
        /// <param name="uiElement">The UI element from which to retrieve the property value.</param>
        /// <param name="name">The name of the property to retrieve.</param>
        /// <returns>The value of the specified property.</returns>
        private static Object GetPropertyValue(UIElement uiElement, String name)
        {
            var dprop = FindElementDependencyProperty(uiElement, name);
            if (dprop != null)
            {
                return miGetValue.MakeGenericMethod(dprop.PropertyType).Invoke(uiElement, new Object[] { dprop });
            }
            else
            {
                var propInfo = FindElementStandardProperty(uiElement, name);
                if (!propInfo.CanRead)
                    throw new InvalidOperationException(UltravioletStrings.PropertyHasNoGetter.Format(name));
                
                return propInfo.GetValue(uiElement, null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified attribute name represents an attached property.
        /// </summary>
        /// <param name="name">The attribute name to evaluate.</param>
        /// <param name="container">The attached property's container type.</param>
        /// <param name="property">The attached property's property name.</param>
        /// <returns><c>true</c> if the specified attribtue name represents an attached property; otherwise, <c>false</c>.</returns>
        private static Boolean IsAttachedProperty(String name, out String container, out String property)
        {
            container = null;
            property  = null;

            var delimiterIx = name.IndexOf('.');
            if (delimiterIx >= 0)
            {
                container = name.Substring(0, delimiterIx);
                property  = name.Substring(delimiterIx + 1);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified string is a binding expression.
        /// </summary>
        /// <param name="value">The string to evaluate.</param>
        /// <returns><c>true</c> if the specified string is a binding expression; otherwise, <c>false</c>.</returns>
        private static Boolean IsBindingExpression(String value)
        {
            return BindingExpressions.IsBindingExpression(value);
        }

        // Reflection information.
        private static readonly MethodInfo miBindValue;
        private static readonly MethodInfo miSetLocalValue;
        private static readonly MethodInfo miGetValue;
    }
}
