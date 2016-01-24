using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
	partial class UvmlLoader
    {
        /// <summary>
        /// Represents a context within which the view loader instantiates new controls.
        /// </summary>
        /// <remarks>The instantiation context tracks the current state of the UVML loader. It maintains a namescope,
        /// determines how expressions are bound, and tracks scoped data like the current templated parent.</remarks>
        private class InstantiationContext
        {
			/// <summary>
			/// Initializes a new instance of the <see cref="InstantiationContext"/> class.
			/// </summary>
			/// <param name="uv">The Ultraviolet context.</param>
			/// <param name="dataSource">The data source for the current scope.</param>
			/// <param name="dataSourceType">The type of the data source for the current scope.</param>
			/// <param name="culture">The culture used to parse UVML values.</param>
			private InstantiationContext(UltravioletContext uv, Object dataSource, Type dataSourceType, CultureInfo culture)
            {
                this.Ultraviolet = uv;
                this.Namescope = new Namescope();
                this.DataSource = dataSource;
                this.DataSourceType = dataSourceType;
				this.Culture = culture;

                FindCompiledBindingExpressions();
            }

			/// <summary>
			/// Creates an instantiation context for a view.
			/// </summary>
			/// <param name="uv">The Ultraviolet context.</param>
			/// <param name="view">The view which is being loaded.</param>
			/// <param name="viewModelType">The view model type for the view which is being loaded.</param>
			/// <param name="culture">The culture used to parse UVML values.</param>
			/// <returns>The instantiation context for the specified view.</returns>
			public static InstantiationContext FromView(UltravioletContext uv,
				PresentationFoundationView view, Type viewModelType, CultureInfo culture)
			{
				Contract.Require(culture, nameof(culture));

				return new InstantiationContext(uv, view, viewModelType, culture);
            }

            /// <summary>
            /// Creates an instantiation context for a control.
            /// </summary>
            /// <param name="uv">The Ultraviolet context.</param>
            /// <param name="control">The control which is being loaded.</param>
			/// <param name="culture">The culture used to parse UVML values.</param>
            /// <returns>The instantiation context for the specified control.</returns>
            public static InstantiationContext FromControl(UltravioletContext uv, 
				Control control, CultureInfo culture)
			{
				Contract.Require(culture, nameof(culture));

				var wrapper = PresentationFoundation.GetDataSourceWrapper(control);
                return new InstantiationContext(uv, control, wrapper.GetType(), culture);
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
                    return property;

                return null;
            }

            /// <summary>
            /// Gets the Ultraviolet context.
            /// </summary>
            public UltravioletContext Ultraviolet
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets or sets the current namescope.
            /// </summary>
            public Namescope Namescope
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets or sets the templated parent which will be assigned to elements
            /// created within this instantiation context.
            /// </summary>
            public Object TemplatedParent
            {
                get { return DataSource is PresentationFoundationView ? null : DataSource; }
            }

            /// <summary>
            /// Gets the declarative data source for the current instantiation context.
            /// </summary>
            public Object DataSource
            {
                get;
                private set;
            }
            
            /// <summary>
            /// Gets the type of view model to which the view is bound.
            /// </summary>
            public Type DataSourceType
            {
                get;
                private set;
            }

			/// <summary>
			/// Gets the culture which is used to parse UVML values.
			/// </summary>
			public CultureInfo Culture
			{
				get;
				private set;
			}

            /// <summary>
            /// Finds all of the compiled binding expressions on the current view model and adds them to the context's registry.
            /// </summary>
            private void FindCompiledBindingExpressions()
            {
                var wrapperName = default(String);
                var wrapperType = DataSource is PresentationFoundationView ? DataSourceType : null;
                if (wrapperType == null)
                {
                    for (var templateType = TemplatedParent.GetType(); templateType != null; templateType = templateType.BaseType)
                    {
                        wrapperName = PresentationFoundationView.GetDataSourceWrapperNameForComponentTemplate(templateType);
                        wrapperType = Ultraviolet.GetUI().GetPresentationFoundation().GetDataSourceWrapperTypeByName(wrapperName);

                        if (wrapperType != null)
                            break;
                    }

                    if (wrapperType == null)
                        throw new InvalidOperationException(PresentationStrings.CannotFindViewModelWrapper.Format(wrapperName));
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

        }
    }
}
