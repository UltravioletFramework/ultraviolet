using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Documents;
using TwistedLogik.Ultraviolet.UI.Presentation.Uvml;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains methods for loading UI elements from UVML.
    /// </summary>
    internal static partial class UvmlLoader
    {
        /// <summary>
        /// Loads an instance of the <see cref="PresentationFoundationView"/> from an XML node.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiPanel">The <see cref="UIPanel"/> that is creating the panel.</param>
        /// <param name="uiPanelDefinition">The <see cref="UIPanelDefinition"/> that defines the view.</param>
        /// <param name="vmfactory">A view model factory which is used to create the view's initial view model, or <c>null</c> to skip view model creation.</param>
        /// <returns>The <see cref="PresentationFoundationView"/> that was loaded from the specified XML element.</returns>
        public static PresentationFoundationView Load(UltravioletContext uv, UIPanel uiPanel, UIPanelDefinition uiPanelDefinition, UIViewModelFactory vmfactory)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(uiPanel, nameof(uiPanel));
            Contract.Require(uiPanelDefinition, nameof(uiPanelDefinition));

            var viewElement = uiPanelDefinition.ViewElement;
            if (viewElement == null)
                return null;

            // Determine which culture to use when parsing values.
            var cultureRequested = (String)uiPanelDefinition.RootElement.Attribute("Culture");
            var cultureInfo = CultureInfo.GetCultureInfo(cultureRequested ?? String.Empty);

            // Determine the type of view model used by this view.
            var viewModelType = default(Type);
            var viewModelTypeName = (String)viewElement.Attribute("ViewModelType");
            if (viewModelTypeName != null)
            {
                viewModelType = Type.GetType(viewModelTypeName, false);

                if (viewModelType == null)
                    throw new UvmlException(PresentationStrings.ViewModelTypeNotFound.Format(viewModelTypeName));

                var viewModelWrapperAttr = viewModelType.GetCustomAttributes(typeof(ViewModelWrapperAttribute), false)
                    .Cast<ViewModelWrapperAttribute>().SingleOrDefault();
                if (viewModelWrapperAttr != null)
                {
                    viewModelType = viewModelWrapperAttr.WrapperType;
                }
                else
                {
                    var viewModelWrapperName = PresentationFoundationView.GetDataSourceWrapperNameForView(uiPanelDefinition.AssetFilePath);
                    var viewModelWrapperType = uv.GetUI().GetPresentationFoundation().GetDataSourceWrapperTypeByName(viewModelWrapperName);
                    viewModelType = viewModelWrapperType;
                }
            }

            // Create a UVML template which will instantiate the view.
            var viewTemplate = new UvmlTemplate(viewElement, typeof(PresentationFoundationView), (puv, pname) =>
            {
                var view = new PresentationFoundationView(puv, uiPanel, viewModelType);
                var viewModel = vmfactory == null ? null : vmfactory(view);
                if (viewModel != null)
                {
                    view.SetViewModel(viewModel);
                }

                var root = view.LayoutRoot;
                root.BeginInit();

                var rootAdornerDecorator = new AdornerDecorator(puv, null);
                rootAdornerDecorator.BeginInit();
                root.Child = rootAdornerDecorator;
                
                var rootGridTemplate = CreateTemplateFromXml(puv, viewElement, null, typeof(Grid), cultureInfo);
                var rootGridContext = UvmlInstantiationContext.ForView(puv, view);
                var rootGrid = (Grid)rootGridTemplate.Instantiate(puv, rootGridContext);

                rootAdornerDecorator.Child = rootGrid;

                if (viewModel != null)
                    rootGridContext.Namescope.PopulateFieldsFromRegisteredElements(viewModel);

                rootAdornerDecorator.EndInit();
                root.EndInit();
                root.CacheLayoutParameters();

                return view;
            });
            
            // Instantiate the view template.
            return (PresentationFoundationView)viewTemplate.Instantiate(uv, null);
        }

        /// <summary>
        /// Loads the component template of the specified control.
        /// </summary>
        /// <param name="control">The instance of <see cref="Control"/> for which to load a component root.</param>
        /// <param name="template">The component template that specifies the control's component layout.</param>
        /// <returns>The <see cref="UIElement"/> which serves as the specified control's component template.</returns>
        public static UIElement LoadComponentTemplate(Control control, XDocument template)
        {
            Contract.Require(control, nameof(control));

            if (template == null)
                return null;

            var componentContext = UvmlInstantiationContext.ForControl(control.Ultraviolet, control);
            var componentTemplate = default(UvmlTemplate);

            if (!componentTemplateCache.TryGetValue(template, out componentTemplate))
            {
                var componentElement = template.Root.Element("View")?.Elements().SingleOrDefault();
                if (componentElement == null)
                    return null;

                var componentType = default(Type);
                if (!control.Ultraviolet.GetUI().GetPresentationFoundation().GetKnownType(componentElement.Name.LocalName, out componentType))
                    throw new UvmlException(PresentationStrings.UnrecognizedType.Format(componentElement.Name.LocalName));

                var cultureRequested = (String)template.Root.Attribute("Culture");
                var cultureInfo = CultureInfo.GetCultureInfo(cultureRequested ?? String.Empty);

                componentTemplate = CreateTemplateFromXml(control.Ultraviolet, componentElement, control.GetType(), componentType, cultureInfo);
                componentTemplateCache[template] = componentTemplate;
            }

            var component = (UIElement)componentTemplate.Instantiate(control.Ultraviolet, componentContext);
            componentContext.Namescope.PopulateFieldsFromRegisteredElements(control);

            return component;
        }
        
        /// <summary>
        /// Creates a new <see cref="UvmlTemplate"/> from the specified XML element.
        /// </summary>
        private static UvmlTemplate CreateTemplateFromXml(UltravioletContext uv,
            XElement xml, Type templatedParentType, Type templatedObjectType, CultureInfo cultureInfo)
        {
            var mutators = new List<UvmlMutator>();

            templatedObjectType = ResolveTypeFromName(uv, templatedParentType, templatedObjectType, xml.Name.LocalName);

            // Create mutators from attributes
            var attrs = xml.Attributes().ToList();
            foreach (var attr in attrs)
            {
                switch (attr.Name.LocalName)
                {
                    case "Name":
                    case "Class":
                    case "ViewModelType":
                    case "BindingContext":
                        continue;
                }

                var mutator = CreateMutatorForXmlAttribute(uv, xml, attr, templatedParentType, templatedObjectType, cultureInfo);
                mutators.Add(mutator);
            }

            // Sort elements into "properties/events" and "children"
            var elems = xml.Elements().ToList();
            var elemsRepresentingProperties = elems.Where(x => x.Name.LocalName.Contains('.')).ToList();
            var elemsRepresentingChildren = elems.Except(elemsRepresentingProperties).ToList();

            // Create mutators from elements representing properties
            if (elemsRepresentingProperties.Any())
            {
                foreach (var elem in elemsRepresentingProperties)
                {
                    var mutator = CreateMutatorForXmlElement(uv, xml, elem, templatedParentType, templatedObjectType, cultureInfo);
                    mutators.Add(mutator);
                }
            }

            // Create mutators from child elements
            if (elemsRepresentingChildren.Any())
            {
                var mutator = CreateMutatorForXmlChildren(uv, xml, elemsRepresentingChildren, templatedParentType, templatedObjectType, cultureInfo);
                mutators.Add(mutator);
            }
            else
            {
                // Create mutator for simple default property
                var mutator = CreateMutatorForLiteralDefaultProperty(uv, xml, templatedParentType, templatedObjectType, cultureInfo);
                if (mutator != null)
                {
                    mutators.Add(mutator);
                }
            }

            var template = new UvmlTemplate(xml, templatedObjectType, null, mutators);
            template.IsItemsPanelForTemplatedParent = typeof(UIElement).IsAssignableFrom(templatedObjectType) &&
                String.Equals(xml.Name.LocalName, "ItemsPanel", StringComparison.Ordinal);

            return template;
        }

        /// <summary>
        /// Determines whether the specified target is a dependency property, routed event, or standard property/event.
        /// </summary>
        private static UvmlMutatorTarget GetMutatorTarget(UltravioletContext uv, 
            String name, String value, Type type, out Object target, out Type targetType)
        {
            var upf = uv.GetUI().GetPresentationFoundation();
            
            // If this is an attached property/event, find the owner type.
            var depname = new DependencyName(name);
            if (depname.IsAttached)
            {
                var attachedOwnerType = default(Type);
                if (!upf.GetKnownType(depname.Owner, out attachedOwnerType))
                    throw new UvmlException(PresentationStrings.UnrecognizedType.Format(depname.Owner));

                type = attachedOwnerType;
            }

            // Is it a dependency property?
            var dprop = DependencyProperty.FindByName(depname.Name, type);
            if (dprop != null)
            {
                target = dprop;
                if (value != null && BindingExpressions.IsBindingExpression(value))
                {
                    targetType = typeof(String);
                    return UvmlMutatorTarget.DependencyPropertyBinding;
                }
                targetType = dprop.PropertyType;
                return UvmlMutatorTarget.DependencyProperty;
            }

            // Is it a routed event?
            var revt = EventManager.FindByName(depname.Name, type);
            if (revt != null)
            {
                target = revt;
                targetType = typeof(String);
                return UvmlMutatorTarget.RoutedEvent;
            }

            // Is it a standard property?
            var clrprop = type.GetProperty(depname.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (clrprop != null)
            {
                target = clrprop;
                targetType = clrprop.PropertyType;
                return UvmlMutatorTarget.StandardProperty;
            }

            // Is it a standard event?
            var clrevt = type.GetEvent(depname.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (clrevt != null)
            {
                target = clrevt;
                targetType = typeof(String);
                return UvmlMutatorTarget.StandardEvent;
            }

            throw new UvmlException(PresentationStrings.EventOrPropertyDoesNotExist.Format(depname.Name, type.Name));
        }
        
        /// <summary>
        /// Creates a new mutator from the specified target information.
        /// </summary>
        private static UvmlMutator CreateMutatorForTarget(
            UvmlMutatorTarget targetKind, Type templatedParentType, Type templatedObjectType, Object target, UvmlNode value)
        {
            switch (targetKind)
            {
                case UvmlMutatorTarget.DependencyProperty:
                    return new UvmlDependencyPropertyValueMutator((DependencyProperty)target, value);

                case UvmlMutatorTarget.DependencyPropertyBinding:
                    return new UvmlDependencyPropertyBindingMutator((DependencyProperty)target, value);

                case UvmlMutatorTarget.RoutedEvent:
                    return new UvmlRoutedEventHandlerMutator((RoutedEvent)target, value);

                case UvmlMutatorTarget.StandardProperty:
                    return new UvmlStandardPropertyValueMutator((PropertyInfo)target, value);

                case UvmlMutatorTarget.StandardEvent:
                    return new UvmlStandardEventHandlerMutator((EventInfo)target, value);
            }

            throw new NotSupportedException();
        }
        
        /// <summary>
        /// Creates a new mutator which populates a collection's items.
        /// </summary>
        private static UvmlMutator CreateMutatorForCollection(UltravioletContext uv,
            UvmlMutatorTarget targetKind, Object target, IEnumerable<UvmlNode> items)
        {
            if (targetKind == UvmlMutatorTarget.StandardProperty)
            {
                return new UvmlStandardPropertyCollectionItemMutator((PropertyInfo)target, items);
            }
            else
            {
                return new UvmlDependencyPropertyCollectionItemMutator((DependencyProperty)target, items);
            }
        }

        /// <summary>
        /// Creates a new mutator from the specified XML attribute.
        /// </summary>
        private static UvmlMutator CreateMutatorForXmlAttribute(UltravioletContext uv, 
            XElement parent, XAttribute attribute, Type templatedParentType, Type templatedObjectType, CultureInfo cultureInfo)
        {
            var xmlName = attribute.Name.LocalName;
            var xmlValue = attribute.Value;

            var target = default(Object);
            var targetType = default(Type);
            var targetKind = GetMutatorTarget(uv, xmlName, xmlValue, templatedObjectType, out target, out targetType);

            return CreateMutatorForTarget(targetKind, templatedParentType, templatedObjectType, target, 
                new UvmlLiteral(xmlValue, targetType, cultureInfo));
        }

        /// <summary>
        /// Creates a new mutator from the specified XML element.
        /// </summary>
        private static UvmlMutator CreateMutatorForXmlElement(UltravioletContext uv, 
            XElement parent, XElement element, Type templatedParentType, Type templatedObjectType, CultureInfo cultureInfo)
        {
            var name = element.Name.LocalName;
            var value = default(UvmlNode);

            var target = default(Object);
            var targetType = default(Type);
            var targetKind = default(UvmlMutatorTarget);

            var elementChildren = element.Elements().ToList();
            if (elementChildren.Any() || element.IsEmpty)
            {
                targetKind = GetMutatorTarget(uv, 
                    element.Name.LocalName, null, templatedObjectType, out target, out targetType);

                var itemType = default(Type);
                if (UvmlCollectionItemMutatorBase.IsSupportedCollectionType(targetType, out itemType))
                {
                    var items = elementChildren.Select(x => CreateTemplateFromXml(uv, x, templatedParentType, itemType, cultureInfo)).ToList();
                    return CreateMutatorForCollection(uv, targetKind, target, items);
                }
                else
                {
                    if (elementChildren.Count() > 1)
                        throw new UvmlException(PresentationStrings.InvalidChildElements.Format(name));

                    value = CreateTemplateFromXml(uv, elementChildren.Single(), templatedParentType, targetType, cultureInfo);
                }
            }
            else
            {
                targetKind = GetMutatorTarget(uv, 
                    element.Name.LocalName, element.Value, templatedObjectType, out target, out targetType);

                value = new UvmlLiteral(element.Value, targetType, cultureInfo);
            }
            
            return CreateMutatorForTarget(targetKind, templatedParentType, templatedObjectType, target, value);
        }

        /// <summary>
        /// Creates a new mutator from the specified collection of XML child elements.
        /// </summary>
        private static UvmlMutator CreateMutatorForXmlChildren(UltravioletContext uv, 
            XElement parent, IEnumerable<XElement> children, Type templatedParentType, Type templatedObjectType, CultureInfo cultureInfo)
        {
            var itemType = default(Type);

            // Does the object have a default property?
            var defaultPropertyAttr = GetDefaultPropertyAttribute(templatedObjectType);
            if (defaultPropertyAttr != null)
            {
                var target = default(Object);
                var targetType = default(Type);
                var targetKind = GetMutatorTarget(uv, 
                    defaultPropertyAttr.Name, null, templatedObjectType, out target, out targetType);

                // Is the default property a supported collection?
                if (UvmlCollectionItemMutatorBase.IsSupportedCollectionType(targetType, out itemType))
                {
                    var items = children.Select(x => CreateTemplateFromXml(uv, x, templatedParentType, itemType, cultureInfo));
                    return CreateMutatorForCollection(uv, targetKind, target, items);
                }
                else
                {
                    if (children.Count() > 1)
                        throw new UvmlException(PresentationStrings.InvalidChildElements.Format(parent.Name.LocalName));

                    var value = CreateTemplateFromXml(uv, children.Single(), templatedParentType, targetType, cultureInfo);
                    return CreateMutatorForTarget(targetKind, templatedParentType, templatedObjectType, target, value);
                }
            }

            // Is the object itself a supported collection?
            if (UvmlCollectionItemMutatorBase.IsSupportedCollectionType(templatedObjectType, out itemType))
            {
                var items = children.Select(x => CreateTemplateFromXml(uv, x, templatedParentType, itemType, cultureInfo));
                return new UvmlCollectionItemMutator(items);
            }
            
            throw new UvmlException(PresentationStrings.InvalidChildElements.Format(parent.Name.LocalName));
        }

        /// <summary>
        /// Creates a new mutator for the specified type's default property.
        /// </summary>
        private static UvmlMutator CreateMutatorForLiteralDefaultProperty(UltravioletContext uv,
            XElement parent, Type templatedParentType, Type templatedObjectType, CultureInfo cultureInfo)
        {
            var value = default(String);

            var xtext = parent.Nodes().OfType<XText>();
            if (xtext.Any())
                value = String.Join(String.Empty, xtext.Select(x => x.Value.Trim()));

            if (String.IsNullOrWhiteSpace(value))
                return null;

            var defaultPropertyAttr = GetDefaultPropertyAttribute(templatedObjectType);
            if (defaultPropertyAttr != null)
            {
                var target = default(Object);
                var targetType = default(Type);
                var targetKind = GetMutatorTarget(uv,
                    defaultPropertyAttr.Name, value, templatedObjectType, out target, out targetType);

                return CreateMutatorForTarget(targetKind, templatedParentType, templatedObjectType,
                    target, new UvmlLiteral(value, targetType, cultureInfo));
            }

            return null;
        }

        /// <summary>
        /// Resolves the type that corresponds to the specified UVML name.
        /// </summary>
        private static Type ResolveTypeFromName(UltravioletContext uv, 
            Type templatedParentType, Type templatedObjectType, String name)
        {
            if (String.Equals(name, "View", StringComparison.Ordinal))
                return templatedObjectType;

            var knownType = default(Type);

            var placeholderAttr = templatedParentType?.GetCustomAttributes(typeof(UvmlPlaceholderAttribute), true).Cast<UvmlPlaceholderAttribute>()
                .Where(x => String.Equals(name, x.Placeholder, StringComparison.Ordinal)).SingleOrDefault();
            if (placeholderAttr != null)
            {
                knownType = placeholderAttr.Type;
            }
            else
            {
                if (!uv.GetUI().GetPresentationFoundation().GetKnownType(name, out knownType))
                    throw new UvmlException(PresentationStrings.UnrecognizedType.Format(name));
            }

            if (!templatedObjectType.IsAssignableFrom(knownType))
                throw new UvmlException(PresentationStrings.TemplateTypeMismatch.Format(templatedObjectType.Name, knownType.Name));

            return knownType;
        }

        /// <summary>
        /// Gets the <see cref="DefaultPropertyAttribute"/> which decorates the specified type, if one exists.
        /// </summary>
        private static DefaultPropertyAttribute GetDefaultPropertyAttribute(Type type)
        {
            return type.GetCustomAttributes(typeof(DefaultPropertyAttribute), true)
                .Cast<DefaultPropertyAttribute>().SingleOrDefault();
        }

        // Caches component templates to avoid repeated parsing of UVML documents for controls
        private static readonly Dictionary<XDocument, UvmlTemplate> componentTemplateCache =
            new Dictionary<XDocument, UvmlTemplate>();
    }
}
