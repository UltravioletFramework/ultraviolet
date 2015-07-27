using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class UvmlLoader
    {
        /// <summary>
        /// Represents a context within which the view loader instantiates new controls. This context
        /// is used primarily to influence how expressions are bound.
        /// </summary>
        private class InstantiationContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InstantiationContext"/> class.
            /// </summary>
            /// <param name="uv">The Ultraviolet context.</param>
            /// <param name="view">The view which is being loaded.</param>
            /// <param name="viewModelType">The type of view model to which the view is bound.</param>
            /// <param name="templatedParent">The templated parent for the current instantiation context.</param>
            public InstantiationContext(UltravioletContext uv, PresentationFoundationView view, Type viewModelType, DependencyObject templatedParent = null)
            {
                this.uv              = uv;
                this.view            = view;
                this.templatedParent = templatedParent;
                this.viewModelType   = viewModelType;

                FindCompiledBindingExpressions();
            }

            /// <summary>
            /// Gets the property that implements the compiled version of the specified binding expression.
            /// </summary>
            /// <param name="type">The type of the expression for which to retrieve an implementing property.</param>
            /// <param name="expression">The text of the expression for which to retrieve an implementing property.</param>
            /// <returns>A <see cref="PropertyInfo"/> which represents the property that implements the compiled version of the specified binding expression,
            /// or <c>null</c> if the expression has no compiled equivalent.</returns>
            public PropertyInfo GetCompiledBindingExpression(Type type, String expression)
            {
                PropertyInfo property;

                var key = new CompiledBindingExpressionKey(type, expression);
                
                if (compiledBindingExpressions.TryGetValue(key, out property))
                {
                    return property;
                }
                return null;
            }

            /// <summary>
            /// Gets the Ultraviolet context.
            /// </summary>
            public UltravioletContext Ultraviolet
            {
                get { return uv; }
            }

            /// <summary>
            /// Gets or sets the templated parent which will be assigned to elements
            /// created within this instantiation context.
            /// </summary>
            public DependencyObject TemplatedParent
            {
                get { return templatedParent; }
                set { templatedParent = value; }
            }
            
            /// <summary>
            /// Gets the type of view model to which the view is bound.
            /// </summary>
            public Type ViewModelType
            {
                get { return viewModelType; }
            }            

            /// <summary>
            /// Gets the declarative data source for the current instantiation context.
            /// </summary>
            public Object DeclarativeDataSource
            {
                get
                {
                    return view ?? (Object)templatedParent;
                }
            }
            
            /// <summary>
            /// Finds all of the compiled binding expressions on the current view model and adds them to the context's registry.
            /// </summary>
            private void FindCompiledBindingExpressions()
            {
                var wrapperType = viewModelType;
                if (wrapperType == null)
                {
                    var upf = uv.GetUI().GetPresentationFoundation();
                    var wrapperName = PresentationFoundationView.GetDataSourceWrapperNameForComponentTemplate(TemplatedParent.GetType());
                    wrapperType = uv.GetUI().GetPresentationFoundation().GetDataSourceWrapperTypeByName(wrapperName);
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
            }

            // Contains the property that implements each of the view model's compiled binding expressions.
            private readonly Dictionary<CompiledBindingExpressionKey, PropertyInfo> compiledBindingExpressions =
                new Dictionary<CompiledBindingExpressionKey, PropertyInfo>();

            // Property values.
            private readonly UltravioletContext uv;
            private PresentationFoundationView view;
            private DependencyObject templatedParent;
            private Type viewModelType;
        }
    }
}
