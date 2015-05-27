using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

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
        /// Loads an instance of the <see cref="PresentationFoundationView"/> from an XML node.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="xml">The <see cref="XElement"/> from which to load the view.</param>
        /// <returns>The <see cref="PresentationFoundationView"/> that was loaded from the specified XML element.</returns>
        public static PresentationFoundationView Load(UltravioletContext uv, XElement xml)
        {
            var viewModelType      = default(Type);
            var viewModelTypeAttr  = xml.Attribute("ViewModelType");
            if (viewModelTypeAttr != null)
            {
                viewModelType = Type.GetType(viewModelTypeAttr.Value, false);
                if (viewModelType == null)
                {
                    throw new InvalidOperationException(PresentationStrings.ViewModelTypeNotFound.Format(viewModelTypeAttr.Value));
                }
            }

            var view    = new PresentationFoundationView(uv, viewModelType);
            var context = new InstantiationContext(uv, viewModelType);

            var fe = view.LayoutRoot as FrameworkElement;
            if (fe != null)
                fe.BeginInit();

            var objectTree = BuildObjectTree(uv, xml, view.LayoutRoot, context);
            PopulateObjectTree(uv, objectTree, context);

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
                throw new ArgumentException(PresentationStrings.InvalidBindingContext.Format(bindingContext));

            var contentElement = layout.Root.Elements().SingleOrDefault();
            if (contentElement == null)
                return;

            var uv      = userControl.Ultraviolet;
            var context = new InstantiationContext(uv, viewModelType, userControl, bindingContext);

            userControl.BeginInit();
            userControl.ComponentTemplateNamescope.Clear();

            var content           = InstantiateElement(uv, null, contentElement, context);
            var contentObjectTree = BuildObjectTree(uv, contentElement, content, context);

            userControl.ComponentRoot = content;
            userControl.PopulateFieldsFromRegisteredElements();
            
            PopulateObjectTree(uv, contentObjectTree, context);
        }

        /// <summary>
        /// Loads the component template of the specified control.
        /// </summary>
        /// <param name="control">The instance of <see cref="Control"/> for which to load a component root.</param>
        /// <param name="template">The component template that specifies the control's component layout.</param>
        public static void LoadComponentTemplate(Control control, XDocument template)
        {
            var uv      = control.Ultraviolet;
            var context = new InstantiationContext(uv, null, control, null);

            control.ComponentTemplateNamescope.Clear();

            PopulateElementPropertiesAndEvents(uv, control, template.Root, context);

            var rootElement = template.Root.Elements().SingleOrDefault();
            if (rootElement == null)
                return;

            var componentRoot           = InstantiateElement(uv, null, rootElement, context);
            var componentRootObjectTree = BuildObjectTree(uv, rootElement, componentRoot, context);

            control.ComponentRoot = componentRoot;
            control.PopulateFieldsFromRegisteredElements();

            PopulateObjectTree(uv, componentRootObjectTree, context);
        }

        /// <summary>
        /// Categorizes the UVML attributes defined on the specified XML element and returns a list
        /// containing a collection of <see cref="UvmlAttribute"/> objects which represent them.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The UI element for which attributes are being categorized.</param>
        /// <param name="xmlElement">The XML element that defines the element to instantiate.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns>A list containing a collection of <see cref="UvmlAttribute"/> objects which represents the categorized attributes.</returns>
        private static IEnumerable<UvmlAttribute> CategorizeUvmlAttributes(UltravioletContext uv, UIElement uiElement, XElement xmlElement, InstantiationContext context)
        {
            var attributes = new List<UvmlAttribute>();

            foreach (var xmlAttr in xmlElement.Attributes())
            {
                var xmlAttrName = xmlAttr.Name.LocalName;

                if (IsReservedAttribute(xmlAttrName))
                    continue;

                var value          = xmlAttr.Value;
                var name           = default(String);
                var attachment     = default(String);
                var attachmentType = default(Type);
                var examinedType   = uiElement.GetType();

                if (IsAttachedPropertyOrEvent(xmlAttrName, out attachment, out name))
                {
                    if (!uv.GetUI().GetPresentationFoundation().GetKnownType(attachment, out attachmentType))
                        throw new InvalidOperationException(PresentationStrings.UnrecognizedType.Format(attachment));

                    examinedType = attachmentType;
                }
                else
                {
                    name = xmlAttrName;
                }

                // Check for dependency properties.
                var dprop = DependencyProperty.FindByName(name, examinedType);
                if (dprop != null)
                {
                    attributes.Add(new UvmlAttribute(UvmlAttributeType.DependencyProperty, attachment, name, value, dprop));
                    continue;
                }

                // Check for routed events.
                var revt = EventManager.FindByName(name, examinedType);
                if (revt != null)
                {
                    attributes.Add(new UvmlAttribute(UvmlAttributeType.RoutedEvent, attachment, name, value, revt));
                    continue;
                }

                // Check for Framework properties.
                var clrprop = examinedType.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (clrprop != null)
                {
                    attributes.Add(new UvmlAttribute(UvmlAttributeType.FrameworkProperty, attachment, name, value, clrprop));
                    continue;
                }

                // Check for Framework events.
                var clrevt = examinedType.GetEvent(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (clrevt != null)
                {
                    attributes.Add(new UvmlAttribute(UvmlAttributeType.FrameworkEvent, attachment, name, value, clrevt));
                    continue;
                }

                throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(xmlAttrName, examinedType.Name));
            }

            return attributes;
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
            var instance  = default(UIElement);            
            var name      = xmlElement.AttributeValueString("Name");
            var classes   = xmlElement.AttributeValueString("Class");
            var classList = (classes == null) ? Enumerable.Empty<String>() : classes.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var typeName = xmlElement.Name.LocalName;
            if (String.Equals("ItemsPanel", typeName, StringComparison.InvariantCulture))
            {
                var itemPresenterControl = context.TemplatedParent as ItemsControl;
                if (itemPresenterControl == null)
                    throw new UvmlException(PresentationStrings.ItemPresenterNotInItemsControl);

                instance = itemPresenterControl.CreateItemsPanel();
                if (instance == null)
                    throw new InvalidOperationException(PresentationStrings.ItemPresenterNotCreatedCorrectly.Format(context.TemplatedParent.GetType().Name));

                itemPresenterControl.ItemsPanelElement = (Panel)instance;
            }
            else
            {
                instance = uv.GetUI().GetPresentationFoundation().InstantiateElementByName(typeName, name, context.ViewModelType, context.BindingContext);
                if (instance == null)
                    throw new UvmlException(PresentationStrings.UnrecognizedType.Format(xmlElement.Name.LocalName));
            }

            var frameworkElement = instance as FrameworkElement;
            if (frameworkElement != null)
                frameworkElement.TemplatedParent = context.TemplatedParent;

            foreach (var className in classList)
            {
                instance.Classes.Add(className);
            }

            var panel = parent as Panel;
            if (panel != null)
                panel.Children.Add(instance);

            var itemsControl = parent as ItemsControl;
            if (itemsControl != null)
                itemsControl.Items.Add(instance);

            var contentControl = parent as ContentControl;
            if (contentControl != null)
                contentControl.Content = instance;

            var popupControl = parent as Popup;
            if (popupControl != null)
                popupControl.Child = instance;

            var fe = instance as FrameworkElement;
            if (fe != null)
                fe.BeginInit();

            return instance;
        }

        /// <summary>
        /// Finds the identifier of the specified dependency property on the specified UI element.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The UI element to search for the dependency property.</param>
        /// <param name="name">The name of the dependency property for which to search.</param>
        /// <returns>The identifier of the specified dependency property, or <c>null</c> if no such property was found.</returns>
        private static DependencyProperty FindElementDependencyProperty(UltravioletContext uv, UIElement uiElement, String name)
        {
            var isAttachedEvent = false;
            return FindElementDependencyProperty(uv, uiElement, name, out isAttachedEvent);
        }

        /// <summary>
        /// Finds the identifier of the specified dependency property on the specified UI element.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The UI element to search for the dependency property.</param>
        /// <param name="name">The name of the dependency property for which to search.</param>
        /// <param name="isAttachedEvent">A value indicating whether the specified name actually represents an attached event.</param>
        /// <returns>The identifier of the specified dependency property, or <c>null</c> if no such property was found.</returns>
        private static DependencyProperty FindElementDependencyProperty(UltravioletContext uv, UIElement uiElement, String name, out Boolean isAttachedEvent)
        {
            isAttachedEvent = false;

            var attachedContainer = String.Empty;
            var attachedProperty  = String.Empty;
            if (IsAttachedPropertyOrEvent(name, out attachedContainer, out attachedProperty))
            {
                Type attachedContainerType;
                if (!uv.GetUI().GetPresentationFoundation().GetKnownType(attachedContainer, out attachedContainerType))
                    throw new InvalidOperationException(PresentationStrings.UnrecognizedType.Format(attachedContainer));

                return FindDependencyPropertyByName(attachedProperty, attachedContainerType, out isAttachedEvent);
            }
            else
            {
                return FindDependencyPropertyByName(name, uiElement.GetType(), out isAttachedEvent);
            }
        }

        /// <summary>
        /// Searches the specified container type for a dependency property with the specified name.
        /// </summary>
        /// <param name="propertyName">The name of the dependency property to retrieve.</param>
        /// <param name="propertyContainerType">The type of dependency object to search for a property.</param>
        /// <param name="isAttachedEvent">A value indicating whether the specified name actually represents an attached event.</param>
        /// <returns>The identifier of the specified dependency property, or <c>null</c> if no such property was found.</returns>
        private static DependencyProperty FindDependencyPropertyByName(String propertyName, Type propertyContainerType, out Boolean isAttachedEvent)
        {
            isAttachedEvent = false;

            var dprop = DependencyProperty.FindByName(propertyName, propertyContainerType);
            if (dprop != null)
            {
                return dprop;
            }

            var attachedEvent = EventManager.FindByName(propertyName, propertyContainerType);
            if (attachedEvent != null)
            {
                isAttachedEvent = true;
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
                throw new InvalidOperationException(PresentationStrings.EventOrPropertyDoesNotExist.Format(name, uiElement.GetType()));

            return standardProperty;
        }

        /// <summary>
        /// Gets the type of the specified property.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The UI element to search for the specified property.</param>
        /// <param name="name">The name of the property for which to search.</param>
        /// <returns>The type of the specified property.</returns>
        private static Type FindPropertyType(UltravioletContext uv, UIElement uiElement, String name)
        {
            var dprop = FindElementDependencyProperty(uv, uiElement, name);
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
        /// <param name="attributeName">The attribute name to evaluate.</param>
        /// <returns><c>true</c> if the specified attribute name is reserved; otherwise, <c>false</c>.</returns>
        private static Boolean IsReservedAttribute(String attributeName)
        {
            switch (attributeName)
            {
                case "Name":
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
        /// Populates a UI element's events and properties.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose properties and events will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElementPropertiesAndEvents(UltravioletContext uv, UIElement uiElement, XElement xmlElement, InstantiationContext context)
        {
            uiElement.InitializeDependencyProperties(false);

            var attrs = CategorizeUvmlAttributes(uv, uiElement, xmlElement, context);

            foreach (var attr in attrs)
            {
                PopulateElementPropertyOrEventFromAttribute(uiElement, attr, context);
            }

            PopulateElementPropertiesFromElements(uv, uiElement, xmlElement, context);
            PopulateElementDefaultProperty(uv, uiElement, xmlElement, context);

            var contentPresenter = uiElement as ContentPresenter;
            if (contentPresenter != null)
            {
                PopulateAliasedContentPresenterProperties(uv, contentPresenter);
            }
        }

        /// <summary>
        /// When creating an instance of <see cref="ContentPresenter"/>, this method is responsible for binding its aliased
        /// properties (i.e. <see cref="ContentPresenter.Content"/> and <see cref="ContentPresenter.ContentStringFormat"/>) to
        /// the presenter's templated parent.
        /// </summary>
        private static void PopulateAliasedContentPresenterProperties(UltravioletContext uv, ContentPresenter contentPresenter)
        {
            if (contentPresenter.HasDefinedValue(ContentPresenter.ContentProperty) || contentPresenter.TemplatedParent == null)
                return;

            var alias = contentPresenter.ContentSource ?? "Content";
            if (alias == String.Empty)
                return;

            var templateType = contentPresenter.TemplatedParent.GetType();

            var dpAliasedContent = DependencyProperty.FindByName(alias, templateType);
            if (dpAliasedContent != null)
            {
                contentPresenter.BindValue(ContentPresenter.ContentProperty, templateType, 
                    "{{" + dpAliasedContent.Name + "}}");
            }

            if (!contentPresenter.HasDefinedValue(ContentPresenter.ContentStringFormatProperty))
            {
                var dpAliasedContentStringFormat = DependencyProperty.FindByName(alias + "StringFormat", templateType);
                if (dpAliasedContentStringFormat != null)
                {
                    contentPresenter.BindValue(ContentPresenter.ContentStringFormatProperty, templateType,
                        "{{" + dpAliasedContentStringFormat.Name + "}}");
                }
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
                var isAttached     = !String.Equals(propOwnerName, uiElement.UvmlName, StringComparison.InvariantCulture);
                if (isAttached)
                    propName = elementName;

                // Special case handling for implementations of IEnumerable<T>: we create the
                // collection's items from this XML element's children.
                var propType = FindPropertyType(uv, uiElement, propName);
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
            var dprop = FindElementDependencyProperty(context.Ultraviolet, uiElement, name);
            if (dprop != null)
            {
                var propValue = LoadObjectWithSerializer(dprop.PropertyType, value, context);
                SetDependencyProperty(uiElement, dprop, propValue);
            }
            else
            {
                var propInfo = FindElementStandardProperty(uiElement, name);
                if (!propInfo.CanWrite)
                    throw new InvalidOperationException(PresentationStrings.PropertyHasNoSetter.Format(name));

                var propValue = LoadObjectWithSerializer(propInfo.PropertyType, value, context);
                propInfo.SetValue(uiElement, propValue, null);
            }
        }

        /// <summary>
        /// Populates a property or event from the specified UVML attribute.
        /// </summary>
        /// <param name="uiElement">The element which will be populated by the attribute.</param>
        /// <param name="attr">The attribute with which to populate the element.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElementPropertyOrEventFromAttribute(UIElement uiElement, UvmlAttribute attr, InstantiationContext context)
        {
            switch (attr.AttributeType)
            {
                case UvmlAttributeType.DependencyProperty:
                    {
                        var dprop = (DependencyProperty)attr.Identifier;
                        BindOrSetDependencyProperty(uiElement, dprop, attr.Value, context);
                    }
                    break;
                
                case UvmlAttributeType.RoutedEvent:
                    {
                        var eventID      = (RoutedEvent)attr.Identifier;
                        var eventHandler = CreateEventDelegate(uiElement, eventID.DelegateType, attr.Value, context);
                        uiElement.AddHandler(eventID, eventHandler);
                    }
                    break;

                case UvmlAttributeType.FrameworkProperty:
                    {
                        var propInfo  = (PropertyInfo)attr.Identifier;
                        var propValue = ResolveValue(uiElement, attr.Value, propInfo.PropertyType);
                        propInfo.SetValue(uiElement, propValue, null);
                    }
                    break;

                case UvmlAttributeType.FrameworkEvent:
                    {
                        var eventInfo    = (EventInfo)attr.Identifier;
                        var eventHandler = CreateEventDelegate(uiElement, eventInfo.EventHandlerType, attr.Value, context);
                        eventInfo.AddEventHandler(uiElement, eventHandler);
                    }
                    break;
            }
        }

        /// <summary>
        /// Populates the specified properties of a UI element with the specified value.
        /// </summary>
        /// <param name="uiElement">The element whose property value will be populated.</param>
        /// <param name="name">The name of the property to populate.</param>
        /// <param name="value">The value to set in the specified property.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <param name="deferred">A value indicating whether to populate deferred properties.</param>
        private static void PopulateElementProperty(UIElement uiElement, String name, String value, InstantiationContext context, Boolean deferred = false)
        {
            var isAttachedEvent = false;

            var dprop = FindElementDependencyProperty(context.Ultraviolet, uiElement, name, out isAttachedEvent);
            if (dprop != null)
            {
                BindOrSetDependencyProperty(uiElement, dprop, value, context);
            }
            else
            {
                if (IsEvent(uiElement, name) || isAttachedEvent)
                    return;

                var propInfo = FindElementStandardProperty(uiElement, name);
                if (!propInfo.CanWrite)
                    throw new InvalidOperationException(PresentationStrings.PropertyHasNoSetter.Format(name));

                var propValue = ResolveValue(uiElement, value, propInfo.PropertyType);
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
                throw new InvalidOperationException(PresentationStrings.CollectionContainsInvalidElements.Format(name));

            var items = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
            foreach (var itemElement in itemElements)
            {
                var item = LoadObjectWithSerializer(itemType, itemElement, context);
                items.Add(item);
            }

            // Check if an instance already exists, and if so, try to clear it, then use it
            // as our instance instead of creating a new one.
            var existingValue = (IEnumerable)GetPropertyValue(context.Ultraviolet, uiElement, name);
            if (existingValue != null)
            {
                var clearMethod = existingValue.GetType().GetMethod("Clear", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, 
                    null, Type.EmptyTypes, null);

                if (clearMethod == null)
                    throw new InvalidOperationException(PresentationStrings.CollectionCannotBeCleared.Format(name));

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

                SetPropertyValue(context.Ultraviolet, uiElement, name, enumInstance);
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
                throw new InvalidOperationException(PresentationStrings.CollectionHasNoAddMethod.Format(enumType.Name));

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
            if (!uv.GetUI().GetPresentationFoundation().GetElementDefaultProperty(uiElement.GetType(), out defaultProperty))
                return;

            if (defaultProperty != null && !String.IsNullOrEmpty(xmlElement.Value))
            {
                var dprop = DependencyProperty.FindByName(defaultProperty, uiElement.GetType());
                if (dprop == null)
                    throw new InvalidOperationException(PresentationStrings.InvalidDefaultProperty.Format(defaultProperty, uiElement.GetType()));

                var value = String.Join(String.Empty, xmlElement.Nodes().Where(x => x is XText).Select(x => ((XText)x).Value.Trim()));
                if (!String.IsNullOrEmpty(value))
                {
                    BindOrSetDependencyProperty(uiElement, dprop, value, context);
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
            if (!dprop.IsAttached && !dprop.IsOwner(dobj.GetType()))
                throw new InvalidOperationException(PresentationStrings.LocalPropertyCannotBeAppliedToType.Format(dprop.Name, dprop.OwnerType.Name));

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
                var resolvedValue = ResolveValue(dobj as UIElement, value, type);
                
                miSetLocalValue.MakeGenericMethod(type).Invoke(dobj, new Object[] { dprop, resolvedValue });
            }
            catch (FormatException e)
            {
                throw new InvalidOperationException(PresentationStrings.InvalidElementPropertyValue.Format(value, dprop.Name), e);
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
            if (context.TemplatedParent == null)
            {
                if (context.ViewModelType == null)
                    throw new InvalidOperationException(PresentationStrings.NoViewModel);

                var expressionType = dprop.PropertyType;
                var expressionFull = BindingExpressions.Combine(context.BindingContext, expression);

                miBindValue.Invoke(dobj, new Object[] { dprop, context.ViewModelType, expressionFull });
            }
            else
            {
                miBindValue.Invoke(dobj, new Object[] { dprop, context.TemplatedParent.GetType(), expression });
            }
        }

        /// <summary>
        /// Sets the value of the specified property.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The UI element on which to set the property value.</param>
        /// <param name="name">The name of the property to set.</param>
        /// <param name="value">The value to set for the specified property.</param>
        private static void SetPropertyValue(UltravioletContext uv, UIElement uiElement, String name, Object value)
        {
            var dprop = FindElementDependencyProperty(uv, uiElement, name);
            if (dprop != null)
            {
                SetDependencyProperty(uiElement, dprop, value);
            }
            else
            {
                var propInfo = FindElementStandardProperty(uiElement, name);
                if (!propInfo.CanWrite)
                    throw new InvalidOperationException(PresentationStrings.PropertyHasNoSetter.Format(name));

                propInfo.SetValue(uiElement, value, null);
            }
        }

        /// <summary>
        /// Gets the value of the specified property.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The UI element from which to retrieve the property value.</param>
        /// <param name="name">The name of the property to retrieve.</param>
        /// <returns>The value of the specified property.</returns>
        private static Object GetPropertyValue(UltravioletContext uv, UIElement uiElement, String name)
        {
            var dprop = FindElementDependencyProperty(uv, uiElement, name);
            if (dprop != null)
            {
                return miGetValue.MakeGenericMethod(dprop.PropertyType).Invoke(uiElement, new Object[] { dprop });
            }
            else
            {
                var propInfo = FindElementStandardProperty(uiElement, name);
                if (!propInfo.CanRead)
                    throw new InvalidOperationException(PresentationStrings.PropertyHasNoGetter.Format(name));
                
                return propInfo.GetValue(uiElement, null);
            }
        }

        /// <summary>
        /// Resolves a string to a value.
        /// </summary>
        /// <param name="uiElement">The <see cref="UIElement"/> for which the value is being resolved.</param>
        /// <param name="value">The string to resolve.</param>
        /// <param name="type">The type of value to resolve.</param>
        /// <returns>The resolved value.</returns>
        private static Object ResolveValue(UIElement uiElement, String value, Type type)
        {
            if (value == "{{null}}")
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }

            var frameworkElement = uiElement as FrameworkElement;
            if (frameworkElement != null && typeof(UIElement).IsAssignableFrom(type))
            {
                var templatedParentControl = frameworkElement.TemplatedParent as Control;

                var namescope = (templatedParentControl == null) ? frameworkElement.View.Namescope : templatedParentControl.ComponentTemplateNamescope;
                if (namescope == null)
                    return null;

                return namescope.GetElementByName(value);
            }

            return ObjectResolver.FromString(value, type);
        }

        /// <summary>
        /// Gets a value indicating whether the specified attribute name represents an attached property.
        /// </summary>
        /// <param name="name">The attribute name to evaluate.</param>
        /// <param name="container">The attached property's container type.</param>
        /// <param name="property">The attached property's property name.</param>
        /// <returns><c>true</c> if the specified attribtue name represents an attached property; otherwise, <c>false</c>.</returns>
        private static Boolean IsAttachedPropertyOrEvent(String name, out String container, out String property)
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

        /// <summary>
        /// Creates a delegate for binding to an event.
        /// </summary>
        /// <param name="uiElement">The element to which the event is being bound.</param>
        /// <param name="handlerType">The event handler's type.</param>
        /// <param name="handlerName">The event handler's name within the view model or control.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns>The specified delegate.</returns>
        private static Delegate CreateEventDelegate(UIElement uiElement, Type handlerType, String handlerName, InstantiationContext context)
        {
            var templateParentElement = context.TemplatedParent as UIElement;
            if (templateParentElement != null)
            {
                return BindingExpressions.CreateElementBoundEventDelegate(templateParentElement, handlerType, handlerName);
            }
            else
            {
                return BindingExpressions.CreateViewModelBoundEventDelegate(uiElement, context.ViewModelType, handlerType, handlerName);
            }
        }

        /// <summary>
        /// Builds an object tree from the specified root.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="xml">The xml that defines the root object.</param>
        /// <param name="instance">The element instance that corresponds to the root object.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns>The object tree's root object.</returns>
        private static UvmlObject BuildObjectTree(UltravioletContext uv, XElement xml, UIElement instance, InstantiationContext context)
        {
            var root = new UvmlObject(xml, instance);
            BuildObjectTree(uv, root, context);
            return root;
        }

        /// <summary>
        /// Builds an object subtree from the specified root.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="root">The object subtree's root object.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns>The object tree's root object.</returns>
        private static UvmlObject BuildObjectTree(UltravioletContext uv, UvmlObject root, InstantiationContext context)
        {
            var debugName   = root.Instance.GetType().Name;
            var childrenXml = root.Xml.Elements().Where(x => !ElementNameRepresentsProperty(x));

            // Single child
            if (root.Instance is ContentControl || root.Instance is Popup)
            {
                var fe = root.Instance as FrameworkElement;
                if (fe != null)
                    debugName = fe.Name;

                if (childrenXml.Count() > 1)
                    throw new InvalidOperationException(PresentationStrings.InvalidChildElements.Format(debugName));

                var childXml = childrenXml.SingleOrDefault();
                if (childXml != null)
                {
                    var child       = InstantiateElement(uv, root.Instance, childXml, context);
                    var childObject = new UvmlObject(childXml, child);

                    root.Children.Add(childObject);

                    BuildObjectTree(uv, childObject, context);
                }
                return root;
            }

            // Multiple children
            if (root.Instance is Panel || root.Instance is ItemsControl)
            {
                foreach (var childXml in childrenXml)
                {
                    var child       = InstantiateElement(uv, root.Instance, childXml, context);
                    var childObject = new UvmlObject(childXml, child);

                    root.Children.Add(childObject);

                    BuildObjectTree(uv, childObject, context);
                }
                return root;
            }

            // No children
            if (childrenXml.Any())
                throw new InvalidOperationException(debugName);

            return root;
        }

        /// <summary>
        /// Populates the properties and events of the elements in the specified object tree.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="root">The root object of the object tree to populate.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateObjectTree(UltravioletContext uv, UvmlObject root, InstantiationContext context)
        {
            var bindingContext        = root.Xml.AttributeValueString("BindingContext");
            var bindingContextDefined = (bindingContext != null);
            if (bindingContextDefined)
            {
                if (!BindingExpressions.IsBindingExpression(bindingContext))
                    throw new InvalidOperationException(PresentationStrings.InvalidBindingContext.Format(bindingContext));

                context.PushBindingContext(bindingContext);
            }

            PopulateElementPropertiesAndEvents(uv, root.Instance, root.Xml, context);
            foreach (var child in root.Children)
            {
                PopulateObjectTree(uv, child, context);
            }

            if (bindingContextDefined)
            {
                context.PopBindingContext();
            }

            var fe = root.Instance as FrameworkElement;
            if (fe != null)
                fe.EndInit();
        }

        // Reflection information.
        private static readonly MethodInfo miBindValue;
        private static readonly MethodInfo miSetLocalValue;
        private static readonly MethodInfo miGetValue;
    }
}
