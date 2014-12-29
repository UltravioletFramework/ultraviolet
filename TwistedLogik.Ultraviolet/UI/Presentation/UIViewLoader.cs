using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains methods for loading UI views.
    /// </summary>
    internal static partial class UIViewLoader
    {
        /// <summary>
        /// Initializes the <see cref="UIViewLoader"/> type.
        /// </summary>
        static UIViewLoader()
        {
            miBindValue     = typeof(DependencyObject).GetMethod("BindValue");
            miSetLocalValue = typeof(DependencyObject).GetMethod("SetLocalValue");
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

            PopulateElement(uv, view.Canvas, xml, context);

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

            userControl.Content = content;
            userControl.PopulateFieldsFromRegisteredElements();
        }

        /// <summary>
        /// Loads the component root of the specified container.
        /// </summary>
        /// <param name="container">The instance of <see cref="UIContainer"/> for which to load a component root.</param>
        /// <param name="template">The component template that specified the control's component layout.</param>
        /// <param name="viewModelType">The type of view model to which the user control will be bound.</param>
        /// <param name="bindingContext">The binding context for the user control, if any.</param>
        public static void LoadComponentRoot(UIContainer container, XDocument template, Type viewModelType, String bindingContext = null)
        {
            if (bindingContext != null && !BindingExpressions.IsBindingExpression(bindingContext))
                throw new ArgumentException(UltravioletStrings.InvalidBindingContext.Format(bindingContext));

            var rootElement = template.Root.Elements().SingleOrDefault();
            if (rootElement == null)
                return;

            var uv      = container.Ultraviolet;
            var context = new InstantiationContext(viewModelType, container, bindingContext);
            var root    = (UIContainer)InstantiateAndPopulateElement(uv, null, rootElement, context);

            container.ComponentRoot = root;
            container.ContentElement = container.ComponentRoot.FindContentPanel();
            container.PopulateFieldsFromRegisteredElements();
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

            var container = parent as UIContainer;
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
        /// Populates the specified element's dependency property values.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose dependency property values will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void PopulateElementProperties(UltravioletContext uv, UIElement uiElement, XElement xmlElement, InstantiationContext context)
        {
            var dprop = default(DependencyProperty);

            foreach (var attr in xmlElement.Attributes())
            {
                var attrName = attr.Name.LocalName;
                if (attrName == "BindingContext")
                    continue;

                var attachedContainer = String.Empty;
                var attachedProperty  = String.Empty;
                if (IsAttachedProperty(attrName, out attachedContainer, out attachedProperty))
                {
                    if (uiElement.Parent != null && String.Equals(uiElement.Parent.Name, attachedContainer, StringComparison.InvariantCulture))
                    {
                        dprop = DependencyProperty.FindByName(attachedProperty, uiElement.Parent.GetType());
                    }
                }
                else
                {
                    dprop = DependencyProperty.FindByName(attrName, uiElement.GetType());
                }

                if (dprop != null)
                {
                    BindOrSetProperty(uiElement, dprop, attr.Value, context);
                }
            }

            PopulateElementDefaultProperty(uv, uiElement, xmlElement, context);
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

                BindOrSetProperty(uiElement, dprop, xmlElement.Value, context);
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
            var container = uiElement as UIContainer;
            if (container != null)
            {
                foreach (var child in xmlElement.Elements())
                {
                    InstantiateAndPopulateElement(uv, uiElement, child, context);
                }
            }
            else
            {
                var contentControl = uiElement as ContentControl;
                if (contentControl != null && !(uiElement is UserControl))
                {
                    if (xmlElement.Elements().Count() > 1)
                    {
                        var id = uiElement.ID ?? uiElement.GetType().Name;
                        throw new InvalidOperationException(UltravioletStrings.InvalidChildElements.Format(id));
                    }

                    var contentElement = xmlElement.Elements().SingleOrDefault();
                    if (contentElement != null)
                    {
                        InstantiateAndPopulateElement(uv, uiElement, contentElement, context);
                    }
                }
                else
                {
                    if (xmlElement.Elements().Any())
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
        /// <param name="uiElement">The UI element to modify.</param>
        /// <param name="dprop">The dependency property to bind or set.</param>
        /// <param name="value">The binding expression or value to set on the property.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void BindOrSetProperty(UIElement uiElement, DependencyProperty dprop, String value, InstantiationContext context)
        {
            if (IsBindingExpression(value))
            {
                BindProperty(uiElement, dprop, value, context);
            }
            else
            {
                SetProperty(uiElement, dprop, value);
            }
        }

        /// <summary>
        /// Sets the local value of the specified dependency property.
        /// </summary>
        /// <param name="uiElement">The UI element to modify.</param>
        /// <param name="dprop">The dependency property to bind or set.</param>
        /// <param name="value">The value to set on the property.</param>
        private static void SetProperty(UIElement uiElement, DependencyProperty dprop, String value)
        {
            try
            {
                var type          = Type.GetTypeFromHandle(dprop.PropertyType);
                var resolvedValue = ObjectResolver.FromString(value, type);

                miSetLocalValue.MakeGenericMethod(type).Invoke(uiElement, new Object[] { dprop, resolvedValue });
            }
            catch (FormatException e)
            {
                throw new InvalidOperationException(UltravioletStrings.InvalidElementPropertyValue.Format(value, dprop.Name), e);
            }
        }

        /// <summary>
        /// Binds the specified dependency property.
        /// </summary>
        /// <param name="uiElement">The UI element to modify.</param>
        /// <param name="dprop">The dependency property to bind or set.</param>
        /// <param name="expression">The binding expression to set on the property.</param>
        /// <param name="context">The current instantiation context.</param>
        private static void BindProperty(UIElement uiElement, DependencyProperty dprop, String expression, InstantiationContext context)
        {
            if (context.ViewModelType == null)
                throw new InvalidOperationException(UltravioletStrings.NoViewModel);

            var expressionType = Type.GetTypeFromHandle(dprop.PropertyType);
            var expressionFull = BindingExpressions.Combine(context.BindingContext, expression);

            miBindValue.Invoke(uiElement, new Object[] { dprop, context.ViewModelType, expressionFull });
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
    }
}
