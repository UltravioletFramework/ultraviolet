using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Xml;

namespace TwistedLogik.Ultraviolet.Layout.Elements
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

            var uiElementTypes = from a in AppDomain.CurrentDomain.GetAssemblies()
                                 from t in a.GetTypes()
                                 let attr = t.GetCustomAttributes(typeof(UIElementAttribute), false).SingleOrDefault()
                                 where
                                  attr != null
                                 select new { ElementType = t, ElementAttribute = (UIElementAttribute)attr };

            foreach (var uiElementType in uiElementTypes)
            {
                var defaultPropertyAttr  = (DefaultPropertyAttribute)uiElementType.ElementType.GetCustomAttributes(typeof(DefaultPropertyAttribute), true).SingleOrDefault();
                var defaultProperty      = default(String);
                if (defaultPropertyAttr != null)
                {
                    defaultProperty = defaultPropertyAttr.Name;
                }

                var constructor = uiElementType.ElementType.GetConstructor(new[] { typeof(UltravioletContext), typeof(String) });
                if (constructor == null)
                {
                    throw new InvalidOperationException(LayoutStrings.UIElementInvalidCtor.Format(uiElementType));
                }

                var metadata = new UIElementMetadata(
                    uiElementType.ElementAttribute.Name,
                    uiElementType.ElementType,
                    constructor, defaultProperty);

                uiElementMetadata[uiElementType.ElementAttribute.Name] = metadata;
            }
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
                    throw new InvalidOperationException(LayoutStrings.ViewModelTypeNotFound.Format(viewModelTypeAttr.Value));
                }
            }

            var view = new UIView(viewModelType, uv);

            PopulateElementProperties(uv, view.Canvas, xml, viewModelType);
            PopulateElementChildren(uv, view.Canvas, xml, viewModelType);

            return view;
        }

        /// <summary>
        /// Instantiates a new element.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <returns>The new instance of <see cref="UIElement"/> that was instantiated.</returns>
        private static UIElement InstantiateElement(UltravioletContext uv, XElement xmlElement)
        {
            UIElementMetadata metadata;
            if (!uiElementMetadata.TryGetValue(xmlElement.Name.LocalName, out metadata))
                throw new UvmlException(LayoutStrings.UnrecognizedUIElement.Format(xmlElement.Name.LocalName));

            var id        = xmlElement.AttributeValueString("ID");
            var classes   = xmlElement.AttributeValueString("Class");
            var classList = (classes == null) ? Enumerable.Empty<String>() : classes.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var instance = (UIElement)metadata.Constructor.Invoke(new Object[] { uv, id });

            foreach (var className in classList)
            {
                instance.Classes.Add(className);
            }

            return instance;
        }

        /// <summary>
        /// Populates the specified element's dependency property values.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose dependency property values will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="viewModelType">The view's associated view model type.</param>
        private static void PopulateElementProperties(UltravioletContext uv, UIElement uiElement, XElement xmlElement, Type viewModelType)
        {
            var dprop = default(DependencyProperty);

            foreach (var attr in xmlElement.Attributes())
            {
                var attrName          = attr.Name.LocalName;
                var attachedContainer = String.Empty;
                var attachedProperty  = String.Empty;
                if (IsAttachedProperty(attrName, out attachedContainer, out attachedProperty))
                {
                    if (String.Equals(uiElement.Container.Name, attachedContainer, StringComparison.InvariantCulture))
                    {
                        dprop = DependencyProperty.FindByName(attachedProperty, uiElement.Container.GetType());
                    }
                }
                else
                {
                    dprop = DependencyProperty.FindByName(attrName, uiElement.GetType());
                }

                if (dprop != null)
                {
                    BindOrSetProperty(uiElement, dprop, attr.Value, viewModelType);
                }
            }

            PopulateElementDefaultProperty(uv, uiElement, xmlElement, viewModelType);
        }

        /// <summary>
        /// Populates the specified element's default property value.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose default property value will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="viewModelType">The view's associated view model type.</param>
        private static void PopulateElementDefaultProperty(UltravioletContext uv, UIElement uiElement, XElement xmlElement, Type viewModelType)
        {
            UIElementMetadata metadata;
            if (!uiElementMetadata.TryGetValue(uiElement.Name, out metadata))
                return;

            if (metadata.DefaultProperty != null && !String.IsNullOrEmpty(xmlElement.Value))
            {
                var dprop = DependencyProperty.FindByName(metadata.DefaultProperty, uiElement.GetType());
                if (dprop == null)
                    throw new InvalidOperationException(LayoutStrings.InvalidDefaultProperty.Format(metadata.DefaultProperty, uiElement.GetType()));

                BindOrSetProperty(uiElement, dprop, xmlElement.Value, viewModelType);
            }
        }

        /// <summary>
        /// Populates the specified container with children.
        /// </summary>
        /// <param name="uv">The Ultraviolet container.</param>
        /// <param name="uiContainer">The container to populate with children.</param>
        /// <param name="xmlElement">The XML element that represents the UI container.</param>
        /// <param name="viewModelType">The view's associated view model type.</param>
        private static void PopulateElementChildren(UltravioletContext uv, UIContainer uiContainer, XElement xmlElement, Type viewModelType)
        {
            foreach (var child in xmlElement.Elements())
            {
                var uiElement = InstantiateElement(uv, child);
                uiContainer.Children.Add(uiElement);

                PopulateElementProperties(uv, uiElement, child, viewModelType);

                var uiChildContainer = uiElement as UIContainer;
                if (uiChildContainer != null)
                {
                    PopulateElementChildren(uv, uiChildContainer, child, viewModelType);
                }
            }
        }

        /// <summary>
        /// Binds or sets the specified dependency property, depending on whether the given value is a binding expression.
        /// </summary>
        /// <param name="uiElement">The UI element to modify.</param>
        /// <param name="dprop">The dependency property to bind or set.</param>
        /// <param name="value">The binding expression or value to set on the property.</param>
        /// <param name="viewModelType">The view's associated view model type.</param>
        private static void BindOrSetProperty(UIElement uiElement, DependencyProperty dprop, String value, Type viewModelType)
        {
            if (IsBindingExpression(value))
            {
                BindProperty(uiElement, dprop, value, viewModelType);
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
            var type          = Type.GetTypeFromHandle(dprop.PropertyType);
            var resolvedValue = ObjectResolver.FromString(value, type);

            miSetLocalValue.MakeGenericMethod(type).Invoke(uiElement, new Object[] { dprop, resolvedValue });
        }

        /// <summary>
        /// Binds the specified dependency property.
        /// </summary>
        /// <param name="uiElement">The UI element to modify.</param>
        /// <param name="dprop">The dependency property to bind or set.</param>
        /// <param name="value">The binding expression to set on the property.</param>
        /// <param name="viewModelType">The view's associated view model type.</param>
        private static void BindProperty(UIElement uiElement, DependencyProperty dprop, String expression, Type viewModelType)
        {
            if (viewModelType == null)
                throw new InvalidOperationException(LayoutStrings.NoViewModel);

            var type = Type.GetTypeFromHandle(dprop.PropertyType);

            miBindValue.MakeGenericMethod(type).Invoke(uiElement, new Object[] { dprop, viewModelType, expression });
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
            return value.StartsWith("{{") && value.EndsWith("}}");
        }

        // Reflection information.
        private static readonly MethodInfo miBindValue;
        private static readonly MethodInfo miSetLocalValue;

        // UI element metadata for registered types.
        private static readonly Dictionary<String, UIElementMetadata> uiElementMetadata = 
            new Dictionary<String, UIElementMetadata>(StringComparer.InvariantCulture);
    }
}
