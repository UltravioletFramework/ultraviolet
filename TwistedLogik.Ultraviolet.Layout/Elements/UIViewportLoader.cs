﻿using System;
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
    /// Contains methods for loading UI viewports.
    /// </summary>
    internal static partial class UIViewportLoader
    {
        /// <summary>
        /// Initializes the <see cref="UIViewportLoader"/> type.
        /// </summary>
        static UIViewportLoader()
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
                    throw new InvalidOperationException("TODO");
                }

                var metadata = new UIElementMetadata(
                    uiElementType.ElementAttribute.Name,
                    uiElementType.ElementType,
                    constructor, defaultProperty);

                uiElementMetadata[uiElementType.ElementAttribute.Name] = metadata;
            }
        }

        /// <summary>
        /// Loads an instance of the <see cref="UIViewport"/> from an XML node.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="xml">The <see cref="XElement"/> from which to load the viewport.</param>
        /// <param name="screenArea">The viewport's initial area on the screen.</param>
        /// <returns>The <see cref="UIViewport"/> that was loaded from the specified XML element.</returns>
        public static UIViewport Load(UltravioletContext uv, XElement xml, Rectangle screenArea)
        {
            var modelType = default(Type);
            var modelTypeAttr = xml.Attribute("ModelType");
            if (modelTypeAttr != null)
            {
                modelType = Type.GetType(modelTypeAttr.Value, false);
                if (modelType == null)
                {
                    throw new InvalidOperationException("TODO");
                }
            }

            var viewport = new UIViewport(modelType, uv, screenArea);

            PopulateElementProperties(uv, viewport.Canvas, xml, modelType);
            PopulateElementChildren(uv, viewport.Canvas, xml, modelType);

            return viewport;
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
                throw new InvalidOperationException("TODO"); // TODO: UvmlException

            var id       = xmlElement.AttributeValueString("ID");
            var instance = (UIElement)metadata.Constructor.Invoke(new Object[] { uv, id });

            return instance;
        }

        /// <summary>
        /// Populates the specified element's dependency property values.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose dependency property values will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="modelType">The viewport's associated model type.</param>
        private static void PopulateElementProperties(UltravioletContext uv, UIElement uiElement, XElement xmlElement, Type modelType)
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
                    var type = Type.GetTypeFromHandle(dprop.PropertyType);

                    if (IsBindingExpression(xmlElement.Value))
                    {
                        if (modelType == null)
                            throw new InvalidOperationException("TODO");

                        var expression = xmlElement.Value;
                        miBindValue.MakeGenericMethod(type).Invoke(uiElement, new Object[] { dprop, modelType, expression });
                    }
                    else
                    {
                        var value = ObjectResolver.FromString(attr.Value, type);
                        miSetLocalValue.MakeGenericMethod(type).Invoke(uiElement, new Object[] { dprop, value });
                    }
                }
            }

            PopulateElementDefaultProperty(uv, uiElement, xmlElement, modelType);
        }

        /// <summary>
        /// Populates the specified element's default property value.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uiElement">The element whose default property value will be populated.</param>
        /// <param name="xmlElement">The XML element that represents the UI element.</param>
        /// <param name="modelType">The viewport's associated model type.</param>
        private static void PopulateElementDefaultProperty(UltravioletContext uv, UIElement uiElement, XElement xmlElement, Type modelType)
        {
            UIElementMetadata metadata;
            if (!uiElementMetadata.TryGetValue(uiElement.Name, out metadata))
                return;

            if (metadata.DefaultProperty != null && !String.IsNullOrEmpty(xmlElement.Value))
            {
                var dprop = DependencyProperty.FindByName(metadata.DefaultProperty, uiElement.GetType());
                var type  = Type.GetTypeFromHandle(dprop.PropertyType);

                if (IsBindingExpression(xmlElement.Value))
                {
                    if (modelType == null)
                        throw new InvalidOperationException("TODO");

                    var expression = xmlElement.Value;
                    miBindValue.MakeGenericMethod(type).Invoke(uiElement, new Object[] { dprop, modelType, expression });
                }
                else
                {
                    var value = ObjectResolver.FromString(xmlElement.Value, type);
                    miSetLocalValue.MakeGenericMethod(type).Invoke(uiElement, new Object[] { dprop, value });
                }
            }
        }

        /// <summary>
        /// Populates the specified container with children.
        /// </summary>
        /// <param name="uv">The Ultraviolet container.</param>
        /// <param name="uiContainer">The container to populate with children.</param>
        /// <param name="xmlElement">The XML element that represents the UI container.</param>
        /// <param name="modelType">The viewport's associated model type.</param>
        private static void PopulateElementChildren(UltravioletContext uv, UIContainer uiContainer, XElement xmlElement, Type modelType)
        {
            foreach (var child in xmlElement.Elements())
            {
                var uiElement = InstantiateElement(uv, child);
                uiContainer.Children.Add(uiElement);

                PopulateElementProperties(uv, uiElement, child, modelType);

                var uiChildContainer = uiElement as UIContainer;
                if (uiChildContainer != null)
                {
                    PopulateElementChildren(uv, uiChildContainer, child, modelType);
                }
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
