using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Documents;

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
            var dobjMethods = typeof(DependencyObject).GetMethods();

            miBindValue     = dobjMethods.Where(x => x.Name == "BindValue").Single();
            miSetLocalValue = dobjMethods.Where(x => x.Name == "SetLocalValue").Single();
            miGetValue      = dobjMethods.Where(x => x.Name == "GetValue" && x.IsGenericMethod).Single();
        }

        /// <summary>
        /// Loads an instance of the <see cref="PresentationFoundationView"/> from an XML node.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiPanel">The <see cref="UIPanel"/> that is creating the panel.</param>
        /// <param name="uiPanelDefinition">The <see cref="UIPanelDefinition"/> that defines the view.</param>
        /// <returns>The <see cref="PresentationFoundationView"/> that was loaded from the specified XML element.</returns>
        public static PresentationFoundationView Load(UltravioletContext uv, UIPanel uiPanel, UIPanelDefinition uiPanelDefinition)
        {
            Contract.Require(uv, "uv");
            Contract.Require(uiPanelDefinition, "uiPanelDefinition");

            var xml = uiPanelDefinition.ViewElement;

            var viewModelType      = default(Type);
            var viewModelTypeAttr  = xml.Attribute("ViewModelType");
            if (viewModelTypeAttr != null)
            {
                viewModelType = Type.GetType(viewModelTypeAttr.Value, false);

                if (viewModelType == null)
                    throw new InvalidOperationException(PresentationStrings.ViewModelTypeNotFound.Format(viewModelTypeAttr.Value));

                var viewModelWrapperAttr = viewModelType.GetCustomAttributes(typeof(ViewModelWrapperAttribute), false).Cast<ViewModelWrapperAttribute>().SingleOrDefault();
                if (viewModelWrapperAttr != null)
                {
                    viewModelType = viewModelWrapperAttr.WrapperType;
                }
                else
                {
                    var viewModelWrapperName = PresentationFoundationView.GetDataSourceWrapperNameForView(uiPanelDefinition.AssetFilePath);
                    var viewModelWrapperType = uv.GetUI().GetPresentationFoundation().GetDataSourceWrapperTypeByName(viewModelWrapperName) ?? viewModelType;
                    viewModelType = viewModelWrapperType;
                }
            }

            var view    = new PresentationFoundationView(uv, uiPanel, viewModelType);
            var context = new InstantiationContext(uv, view, viewModelType);
            
            var root = view.LayoutRoot;
            root.BeginInit();

            var rootAdornerDecorator = new AdornerDecorator(uv, null);
            root.Child = rootAdornerDecorator;
            rootAdornerDecorator.BeginInit();

            var rootGrid = (Grid)InstantiateElement(uv, null, xml, context, "Grid");
            rootAdornerDecorator.Child = rootGrid;
            
            var objectTree = BuildObjectTree(uv, xml, rootGrid, context);
            PopulateObjectTree(uv, objectTree, context);
            root.EndInit();

            return view;
        }

        /// <summary>
        /// Initializes an instance of <see cref="UserControl"/> from the specified layout definition.
        /// </summary>
        /// <typeparam name="TViewModelType">The type of view model to which the user control will be bound.</typeparam>
        /// <param name="userControl">The instance of <see cref="UserControl"/> to initialize.</param>
        /// <param name="layout">The XML document that specifies the control's layout.</param>
        public static void LoadUserControl<TViewModelType>(UserControl userControl, XDocument layout)
        {
            LoadUserControl(userControl, layout, typeof(TViewModelType));
        }

        /// <summary>
        /// Initializes an instance of <see cref="UserControl"/> from the specified layout definition.
        /// </summary>
        /// <param name="userControl">The instance of <see cref="UserControl"/> to initialize.</param>
        /// <param name="layout">The XML document that specifies the control's layout.</param>
        /// <param name="viewModelType">The type of view model to which the user control will be bound.</param>
        public static void LoadUserControl(UserControl userControl, XDocument layout, Type viewModelType)
        {
            var viewElement = layout.Root.Element("View");
            if (viewElement == null)
                return;

            var contentElement = viewElement.Elements().SingleOrDefault();
            if (contentElement == null)
                return;

            var uv = userControl.Ultraviolet;
            var context = new InstantiationContext(uv, null, viewModelType, userControl);

            userControl.BeginInit();
            userControl.ComponentTemplateNamescope.Clear();

            PopulateElementPropertiesAndEvents(uv, userControl, viewElement, context);

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
            var viewElement = template.Root.Element("View");
            if (viewElement == null)
                return;

            var uv      = control.Ultraviolet;
            var context = new InstantiationContext(uv, null, null, control);            
            
            control.ComponentTemplateNamescope.Clear();
            PopulateElementPropertiesAndEvents(uv, control, viewElement, context);

            var rootElement = viewElement.Elements().SingleOrDefault();
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
        /// <param name="typeNameOverride">A type name to use in place of the XML element's name.</param>
        /// <returns>The interface element that was instantiated.</returns>
        private static UIElement InstantiateElement(UltravioletContext uv, UIElement parent, XElement xmlElement, InstantiationContext context, String typeNameOverride = null)
        {
            var instance  = default(UIElement);            
            var name      = xmlElement.AttributeValueString("Name");
            var classes   = xmlElement.AttributeValueString("Class");
            var classList = (classes == null) ? Enumerable.Empty<String>() : classes.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var typeName = typeNameOverride ?? xmlElement.Name.LocalName;
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
                instance = uv.GetUI().GetPresentationFoundation().InstantiateElementByName(typeName, name, context.ViewModelType);
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

            PopulateParentDefaultPropertyWithChildIfApplicable(uv, parent, instance, context);

            var fe = instance as FrameworkElement;
            if (fe != null)
                fe.BeginInit();

            instance.DeclarativeDataSource = context.DeclarativeDataSource;

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
                if (String.Equals(attachedContainer, uiElement.UvmlName, StringComparison.InvariantCulture))
                {
                    return FindDependencyPropertyByName(attachedProperty, uiElement.GetType(), out isAttachedEvent);
                }
                else
                {
                    Type attachedContainerType;
                    if (!uv.GetUI().GetPresentationFoundation().GetKnownType(attachedContainer, out attachedContainerType))
                        throw new InvalidOperationException(PresentationStrings.UnrecognizedType.Format(attachedContainer));

                    var dprop = FindDependencyPropertyByName(attachedProperty, attachedContainerType, out isAttachedEvent);
                    if (dprop.IsAttached)
                    {
                        return dprop;
                    }

                    throw new InvalidOperationException(PresentationStrings.AttachablePropertyNotFound.Format(attachedProperty, attachedContainer));
                }
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
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="type">The type of object to load.</param>
        /// <param name="value">The XML element to load as an object.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns>The object that was loaded.</returns>
        private static Object LoadObjectWithSerializer(UltravioletContext uv, Type type, XElement value, InstantiationContext context)
        {
            var instance = default(Object);

            if (type.IsAbstract)
            {
                var child = value.Elements().SingleOrDefault();
                if (child != null)
                {
                    Type childType;
                    if (!uv.GetUI().GetPresentationFoundation().GetKnownType(child.Name.LocalName, out childType))
                        throw new InvalidOperationException();

                    instance = LoadObjectWithSerializer(uv, childType, child, context);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                instance = ObjectLoader.LoadObject((loaderObj, loaderName, loaderValue, attribute) =>
                {
                    if (attribute)
                    {
                        return DependencyPropertyResolutionHandler(loaderObj, loaderName, loaderValue, context);
                    }
                    return true;
                }, type, value);

                LoadObjectDefaultProperty(uv, instance, value, context);
            }

            var dobj = instance as DependencyObject;
            if (dobj != null)
            {
                dobj.DeclarativeDataSource = context.DeclarativeDataSource;
            }

            return instance;
        }

        /// <summary>
        /// Loads the default property of the specified object which is being loaded by the Nucleus serializer.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="instance">The object instance that is being loaded.</param>
        /// <param name="value">The XML element to load as an object.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void LoadObjectDefaultProperty(UltravioletContext uv, Object instance, XElement value, InstantiationContext context)
        {
            var defaultPropAttr = (DefaultPropertyAttribute)instance.GetType().GetCustomAttributes(
                typeof(DefaultPropertyAttribute), true).SingleOrDefault();

            if (defaultPropAttr == null)
                return;

            var defaultProp = instance.GetType().GetProperty(defaultPropAttr.Name);
            if (defaultProp == null)
                return;

            var itemType = defaultProp.PropertyType.GetGenericListElementType();
            if (itemType != null)
            {
                var listInstance  = Activator.CreateInstance(defaultProp.PropertyType);
                var listAddMethod = defaultProp.PropertyType.GetMethod("Add", 
                    BindingFlags.Public | BindingFlags.Instance, null, new[] { itemType }, null);

                var children = value.Elements();
                foreach (var child in children)
                {
                    Type childType;

                    if (!uv.GetUI().GetPresentationFoundation().GetKnownType(child.Name.LocalName, out childType))
                        throw new InvalidOperationException(PresentationStrings.UnrecognizedType.Format(child.Name.LocalName));

                    if (!itemType.IsAssignableFrom(childType))
                        throw new InvalidOperationException(PresentationStrings.IncompatibleType.Format(itemType, childType));

                    var childInstance = LoadObjectWithSerializer(uv, childType, child, context);
                    listAddMethod.Invoke(listInstance, new[] { childInstance });
                }

                defaultProp.SetValue(instance, listInstance, null);
            }
            else
            {
                var child = value.Elements().SingleOrDefault();
                if (child != null)
                {
                    var childInstance = LoadObjectWithSerializer(uv, defaultProp.PropertyType, child, context);
                    defaultProp.SetValue(instance, childInstance, null);
                }
            }
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

            var templateWrapperName = PresentationFoundationView.GetDataSourceWrapperNameForComponentTemplate(templateType);
            var templateWrapperType = uv.GetUI().GetPresentationFoundation().GetDataSourceWrapperTypeByName(templateWrapperName);

            var dpAliasedContent = DependencyProperty.FindByName(alias, templateType);
            if (dpAliasedContent != null)
            {
                contentPresenter.BindValue(ContentPresenter.ContentProperty, templateWrapperType, 
                    "{{" + dpAliasedContent.Name + "}}");
            }

            if (!contentPresenter.HasDefinedValue(ContentPresenter.ContentStringFormatProperty))
            {
                var dpAliasedContentStringFormat = DependencyProperty.FindByName(alias + "StringFormat", templateType);
                if (dpAliasedContentStringFormat != null)
                {
                    contentPresenter.BindValue(ContentPresenter.ContentStringFormatProperty, templateWrapperType,
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
                    if (propType.IsAssignableFrom(typeof(UIElement)) && element.Elements().Count() == 1)
                    {
                        var childXml     = element.Elements().Single();
                        var childElement = InstantiateElement(uv, null, childXml, context);
                        var childTree    = BuildObjectTree(uv, childXml, childElement, context);
                        PopulateObjectTree(uv, childTree, context);

                        SetPropertyValue(uv, uiElement, propName, childElement);
                    }
                    else
                    {
                        PopulateElementPropertyWithSerializer(uiElement, propName, element, context);
                    }
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
                var propValue = LoadObjectWithSerializer(context.Ultraviolet, dprop.PropertyType, value, context);
                SetDependencyProperty(uiElement, dprop, propValue);
            }
            else
            {
                var propInfo = FindElementStandardProperty(uiElement, name);
                if (!propInfo.CanWrite)
                    throw new InvalidOperationException(PresentationStrings.PropertyHasNoSetter.Format(name));

                var propValue = LoadObjectWithSerializer(context.Ultraviolet, propInfo.PropertyType, value, context);
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
                var item = LoadObjectWithSerializer(context.Ultraviolet, itemType, itemElement, context);
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
                var value = String.Join(String.Empty, xmlElement.Nodes().Where(x => x is XText).Select(x => ((XText)x).Value.Trim()));
                if (!String.IsNullOrEmpty(value))
                {
                    PopulateElementProperty(uiElement, defaultProperty, value, context);
                }
            }
        }

        /// <summary>
        /// Populates the specified element's default property with the specified child element, if applicable.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="parent">The parent element.</param>
        /// <param name="child">The child element.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateParentDefaultPropertyWithChildIfApplicable(UltravioletContext uv, UIElement parent, UIElement child, InstantiationContext context)
        {
            if (parent == null || child == null)
                return;

            String defaultProperty;
            if (!uv.GetUI().GetPresentationFoundation().GetElementDefaultProperty(parent.GetType(), out defaultProperty))
                return;

            if (String.IsNullOrEmpty(defaultProperty))
                return;

            var type = GetPropertyType(uv, parent, defaultProperty);
            if (type.IsAssignableFrom(child.GetType()))
            {
                var value = GetPropertyValue(uv, parent, defaultProperty);
                if (value != null)
                {
                    throw new InvalidOperationException(PresentationStrings.InvalidChildElements.Format(parent.UvmlName));
                }

                SetPropertyValue(uv, parent, defaultProperty, child);
            }
            else
            {
                var ienum = (from iface in type.GetInterfaces()
                             where
                                 iface.IsGenericType &&
                                 iface.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                                 iface.GetGenericArguments()[0].IsAssignableFrom(child.GetType())
                             select iface).SingleOrDefault();

                if (ienum != null)
                {
                    var value = GetPropertyValue(uv, parent, defaultProperty);
                    if (value == null)
                    {
                        var ctor = type.GetConstructor(Type.EmptyTypes);
                        if (ctor != null)
                            throw new InvalidOperationException(UltravioletStrings.NoValidConstructor.Format(type.Name));

                        value = ctor.Invoke(null);
                        SetPropertyValue(uv, parent, defaultProperty, value);
                    }

                    var addMethod = (from method in value.GetType().GetMethods()
                                     let methodName = method.Name
                                     let methodParams = method.GetParameters()
                                     where
                                        methodName == "Add" &&
                                        methodParams.Length == 1 &&
                                        methodParams[0].ParameterType.IsAssignableFrom(child.GetType())
                                     select method).FirstOrDefault();
                    if (addMethod != null)
                    {
                        addMethod.Invoke(value, new[] { child });
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
                
                var compiledImpl = context.GetCompiledBindingExpression(dprop.PropertyType, expression);
                if (compiledImpl == null)
                    throw new InvalidOperationException(PresentationStrings.CompiledExpressionNotFound.Format(expression));

                expression = "{{" + compiledImpl.Name + "}}";

                miBindValue.Invoke(dobj, new Object[] { dprop, context.ViewModelType, expression });
            }
            else
            {
                var compiledImpl = context.GetCompiledBindingExpression(dprop.PropertyType, expression);
                if (compiledImpl == null)
                    throw new InvalidOperationException(PresentationStrings.CompiledExpressionNotFound.Format(expression));

                expression = "{{" + compiledImpl.Name + "}}";

                var dataSource = PresentationFoundation.GetDataSourceWrapper(context.TemplatedParent);
                miBindValue.Invoke(dobj, new Object[] { dprop, dataSource.GetType(), expression });
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
        /// Gets the type of the specified property.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The UI element from which to retrieve the property value.</param>
        /// <param name="name">The name of the property to retrieve.</param>
        /// <returns>The type of the specified property.</returns>
        private static Type GetPropertyType(UltravioletContext uv, UIElement uiElement, String name)
        {
            var dprop = FindElementDependencyProperty(uv, uiElement, name);
            if (dprop != null)
            {
                return dprop.PropertyType;
            }
            else
            {
                var propInfo = FindElementStandardProperty(uiElement, name);
                if (!propInfo.CanRead)
                    throw new InvalidOperationException(PresentationStrings.PropertyHasNoGetter.Format(name));

                return propInfo.PropertyType;
            }
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
            var dataSource = (Object)uiElement.View;
            var dataSourceType = context.ViewModelType;

            var templatedParent = context.TemplatedParent as UIElement;
            if (templatedParent != null)
            {
                dataSource = templatedParent;
                PresentationFoundation.Instance.ComponentTemplates.Get(templatedParent, out dataSourceType);
            }

            return BindingExpressions.CreateBoundEventDelegate(dataSource, dataSourceType, handlerType, handlerName);
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
            var debugName = root.Instance.GetType().Name;
            var childrenXml = root.Xml.Elements().Where(x => !ElementNameRepresentsProperty(x));

            foreach (var childXml in childrenXml)
            {
                var child = InstantiateElement(uv, root.Instance, childXml, context);
                var childObject = new UvmlObject(childXml, child);

                root.Children.Add(childObject);

                BuildObjectTree(uv, childObject, context);
            }

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
            PopulateElementPropertiesAndEvents(uv, root.Instance, root.Xml, context);
            foreach (var child in root.Children)
            {
                PopulateObjectTree(uv, child, context);
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
