using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a dependency object whose properties can be styled using an Ultraviolet Stylesheet (UVSS) document.
    /// </summary>
    public abstract class StyledDependencyObject : DependencyObject
    {
        /// <summary>
        /// Represents a method which sets the value of a styled property on a dependency object.
        /// </summary>
        /// <param name="dobj">The dependency object on which to set the style.</param>
        /// <param name="value">The string representation of the value to set for the style.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        protected delegate void StyleSetter(StyledDependencyObject dobj, String value, IFormatProvider provider);

        /// <summary>
        /// Initializes the <see cref="StyledDependencyObject"/> class.
        /// </summary>
        static StyledDependencyObject()
        {
            miFromString     = typeof(ObjectResolver).GetMethod("FromString", new Type[] { typeof(String), typeof(Type), typeof(IFormatProvider) });
            miSetStyledValue = typeof(DependencyObject).GetMethod("SetStyledValue");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StyledDependencyObject"/> class.
        /// </summary>
        protected StyledDependencyObject()
        {
            CreateStyleSetters();
        }

        /// <summary>
        /// Finds a styled dependency property according to its styling name.
        /// </summary>
        /// <param name="name">The styling name of the dependency property to retrieve.</param>
        /// <param name="type">The type to search for a dependency property.</param>
        /// <returns>The <see cref="DependencyProperty"/> instance which matches the specified styling name, or <c>null</c> if no
        /// such dependency property exists on this object.</returns>
        internal static DependencyProperty FindStyledDependencyProperty(String name, Type type)
        {
            Contract.RequireNotEmpty("name", name);
            Contract.Require(type, "type");

            while (type != null)
            {
                Dictionary<String, DependencyProperty> styledPropertiesForCurrentType;
                if (styledProperties.TryGetValue(type, out styledPropertiesForCurrentType))
                {
                    DependencyProperty dp;
                    if (styledPropertiesForCurrentType.TryGetValue(name, out dp))
                    {
                        return dp;
                    }
                }

                type = type.BaseType;
            }
            return null;
        }

        /// <summary>
        /// Applies the styles from the specified stylesheet to this object.
        /// </summary>
        /// <param name="document">The stylesheet to apply to this object.</param>
        protected internal virtual void ApplyStyles(UvssDocument document)
        {

        }

        /// <summary>
        /// Applies a style to this object.
        /// </summary>
        /// <param name="style">The style which is being applied.</param>
        /// <param name="selector">The selector which caused the style to be applied.</param>
        /// <param name="attached">A value indicating whether thie style represents an attached property.</param>
        protected internal virtual void ApplyStyle(UvssStyle style, UvssSelector selector, Boolean attached)
        {
            Contract.Require(style, "style");
            Contract.Require(selector, "selector");

            if (attached)
                return;

            var name  = style.Name;
            var value = style.Value.Trim();

            var setter = GetStyleSetter(name, selector.PseudoClass);
            if (setter == null)
                return;

            setter(this, value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the style setter for the style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style for which to retrieve a setter.</param>
        /// <returns>A function to set the value of the specified style.</returns>
        protected StyleSetter GetStyleSetter(String name)
        {
            return GetStyleSetter(name, null);
        }

        /// <summary>
        /// Gets the style setter for the style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style for which to retrieve a setter.</param>
        /// <param name="pseudoClass">The pseudo-class of the style for which to retrieve a setter.</param>
        /// <returns>A function to set the value of the specified style.</returns>
        protected StyleSetter GetStyleSetter(String name, String pseudoClass)
        {
            var currentType = GetType();

            lock (styleSetters)
            {
                while (currentType != null && typeof(StyledDependencyObject).IsAssignableFrom(currentType))
                {
                    Dictionary<UvssStyleKey, StyleSetter> styleSettersForCurrentType;
                    if (styleSetters.TryGetValue(currentType, out styleSettersForCurrentType))
                    {
                        StyleSetter setter;
                        if (styleSettersForCurrentType.TryGetValue(new UvssStyleKey(name, pseudoClass), out setter))
                        {
                            return setter;
                        }
                    }

                    currentType = currentType.BaseType;
                }
            }

            return null;
        }

        /// <summary>
        /// Dynamically compiles a collection of lambda methods which can be used to apply styles
        /// to the object's properties.
        /// </summary>
        private void CreateStyleSetters()
        {
            var currentType = GetType();

            while (currentType != null && typeof(StyledDependencyObject).IsAssignableFrom(currentType))
            {
                Dictionary<UvssStyleKey, StyleSetter> styleSettersForCurrentType;
                Dictionary<String, DependencyProperty> styledPropertiesForCurrentType;
                if (!styleSetters.TryGetValue(currentType, out styleSettersForCurrentType))
                {
                    styleSettersForCurrentType     = new Dictionary<UvssStyleKey, StyleSetter>();
                    styledPropertiesForCurrentType = new Dictionary<String, DependencyProperty>();

                    var styledDependencyProperties = 
                        from field in currentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                        let attr = field.GetCustomAttributes(typeof(StyledAttribute), false).SingleOrDefault()
                        let type = field.FieldType
                        let name = field.Name
                        where
                            attr != null &&
                            type == typeof(DependencyProperty)
                        select new { Attribute = (StyledAttribute)attr, FieldInfo = field };

                    foreach (var prop in styledDependencyProperties)
                    {
                        var dp                  = (DependencyProperty)prop.FieldInfo.GetValue(null);
                        var dpType              = dp.PropertyType;

                        var setStyledValue      = miSetStyledValue.MakeGenericMethod(dpType);

                        var expParameterDObj    = Expression.Parameter(typeof(StyledDependencyObject), "dobj");
                        var expParameterValue   = Expression.Parameter(typeof(String), "value");
                        var expParameterFmtProv = Expression.Parameter(typeof(IFormatProvider), "provider");
                        var expResolveValue     = Expression.Convert(Expression.Call(miFromString, expParameterValue, Expression.Constant(dpType), expParameterFmtProv), dpType);
                        var expCallMethod       = Expression.Call(expParameterDObj, setStyledValue, Expression.Constant(dp), expResolveValue);

                        var lambda = Expression.Lambda<StyleSetter>(expCallMethod, expParameterDObj, expParameterValue, expParameterFmtProv).Compile();

                        var styleKey = new UvssStyleKey(prop.Attribute.Name, prop.Attribute.PseudoClass);
                        styleSettersForCurrentType[styleKey] = lambda;
                        styledPropertiesForCurrentType[prop.Attribute.Name] = dp;
                    }

                    styleSetters[currentType]     = styleSettersForCurrentType;
                    styledProperties[currentType] = styledPropertiesForCurrentType;
                }

                currentType = currentType.BaseType;
            }
        }

        // Functions for setting styles on known element types.
        private static readonly MethodInfo miFromString;
        private static readonly MethodInfo miSetStyledValue;
        private static readonly Dictionary<Type, Dictionary<String, DependencyProperty>> styledProperties = 
            new Dictionary<Type, Dictionary<String, DependencyProperty>>();
        private static readonly Dictionary<Type, Dictionary<UvssStyleKey, StyleSetter>> styleSetters = 
            new Dictionary<Type, Dictionary<UvssStyleKey, StyleSetter>>();
    }
}
