using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ultraviolet.Presentation.Controls;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents the instantiation context for a UVML template.
    /// </summary>
    public sealed class UvmlInstantiationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlInstantiationContext"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="templatedParent">The templated parent for the instantiated object.</param>
        /// <param name="dataSource">The data source for the instantiated object.</param>
        /// <param name="dataSourceType">The data source type for the instantiated object.</param>
        /// <param name="namescope">The context's namescope, or <see langword="null"/> to create a new namescope.</param>
        internal UvmlInstantiationContext(UltravioletContext uv, Object templatedParent, Object dataSource, Type dataSourceType, Namescope namescope = null)
        {
            this.Ultraviolet = uv;
            this.Namescope = namescope ?? new Namescope();
            this.TemplatedParent = templatedParent;
            this.DataSource = dataSource;
            this.DataSourceType = dataSourceType;
            
            FindCompiledBindingExpressions();
        }

        /// <summary>
        /// Creates an instantiation context for the specified view.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="view">The view for which to create an instantiation context.</param>
        /// <returns>The instantiation context which was created.</returns>
        internal static UvmlInstantiationContext ForView(UltravioletContext uv, PresentationFoundationView view)
        {
            return new UvmlInstantiationContext(uv, null, view, view.ViewModelType, view.Namescope);
        }

        /// <summary>
        /// Creates an instantiation context for the specified view.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="control">The control for which to create an instantiation context.</param>
        /// <returns>The instantiation context which was created.</returns>
        internal static UvmlInstantiationContext ForControl(UltravioletContext uv, Control control)
        {
            var wrapper = PresentationFoundation.GetDataSourceWrapper(control);
            return new UvmlInstantiationContext(uv, control, control, wrapper.GetType(), control.ComponentTemplateNamescope);
        }

        /// <summary>
        /// Gets the property that implements specified compiled binding expression, if it exists in the current context.
        /// </summary>
        /// <param name="type">The type of the expression for which to retrieve an implementing property.</param>
        /// <param name="expression">The text of the expression for which to retrieve an implementing property.</param>
        /// <returns>A <see cref="PropertyInfo"/> which represents the property that implements the compiled version of the 
        /// specified binding expression, or <see langword="null"/> if the expression has no compiled equivalent.</returns>
        public PropertyInfo GetCompiledBindingExpression(Type type, String expression)
        {
            PropertyInfo property;

            var specificKey = new CompiledBindingExpressionKey(type, expression);
            if (compiledBindingExpressions.TryGetValue(specificKey, out property))
                return property;

            var fallbackKey = new CompiledBindingExpressionKey(null, expression);
            if (compiledBindingExpressions.TryGetValue(fallbackKey, out property))
                return property;

            return null;
        }
        
        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet { get; }

        /// <summary>
        /// Gets the namescope for the current template.
        /// </summary>
        public Namescope Namescope { get; }

        /// <summary>
        /// Gets the current templated parent.
        /// </summary>
        public Object TemplatedParent { get; }

        /// <summary>
        /// Gets the current data source.
        /// </summary>
        public Object DataSource { get; }

        /// <summary>
        /// Gets the current data source's type.
        /// </summary>
        public Type DataSourceType { get; }

        /// <summary>
        /// Finds all of the compiled binding expressions on the current data
        /// source and adds them to the context's registry.
        /// </summary>
        private void FindCompiledBindingExpressions()
        {
            var wrapperType = DataSource is PresentationFoundationView || TemplatedParent == null ? DataSourceType : null;
            if (wrapperType == null)
            {
                for (var templateType = TemplatedParent.GetType(); templateType != null; templateType = templateType.BaseType)
                {
                    wrapperType = Ultraviolet.GetUI().GetPresentationFoundation().GetDataSourceWrapperType(templateType);

                    if (wrapperType != null)
                        break;
                }

                if (wrapperType == null)
                    throw new UvmlException(PresentationStrings.CannotFindViewModelWrapper.Format(TemplatedParent.GetType().Name));
            }

            var properties = wrapperType.GetProperties().Where(x => x.Name.StartsWith("__UPF_Expression")).ToList();
            var propertiesWithExpressions = from prop in properties
                                            let attr = (CompiledBindingExpressionAttribute)prop.GetCustomAttributes(typeof(CompiledBindingExpressionAttribute), false).Single()
                                            let expr = attr.Expression
                                            select new
                                            {
                                                Property = prop,
                                                Expression = expr,
                                            };

            foreach (var prop in propertiesWithExpressions)
            {
                var key = new CompiledBindingExpressionKey(prop.Property.PropertyType, prop.Expression);
                compiledBindingExpressions.Add(key, prop.Property);
            }

            var uniques = compiledBindingExpressions.GroupBy(x => x.Key).Where(x => x.Count() == 1).ToList();
            foreach (var unique in uniques)
            {
                var key = new CompiledBindingExpressionKey(null, unique.Key.Expression);
                compiledBindingExpressions[key] = unique.Single().Value;
            }
        }
        
        // Associates expression implementations with their keys.
        private readonly Dictionary<CompiledBindingExpressionKey, PropertyInfo> compiledBindingExpressions =
            new Dictionary<CompiledBindingExpressionKey, PropertyInfo>();
    }
}
