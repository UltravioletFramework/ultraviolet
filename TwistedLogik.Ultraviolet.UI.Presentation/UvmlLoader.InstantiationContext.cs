using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            public InstantiationContext(UltravioletContext uv, PresentationFoundationView view, Type viewModelType)
            {
                this.uv            = uv;
                this.view          = view;
                this.viewModelType = viewModelType;

                FindCompiledBindingExpressions();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="InstantiationContext"/> class.
            /// </summary>
            /// <param name="uv">The Ultraviolet context.</param>
            /// <param name="view">The view which is being loaded.</param>
            /// <param name="viewModelType">The type of view model to which the view is bound.</param>
            /// <param name="templatedParent">The template parent to assign to elements instantiated by this context.</param>
            /// <param name="initialBindingContext">The initial binding context.</param>
            public InstantiationContext(UltravioletContext uv, PresentationFoundationView view, Type viewModelType, DependencyObject templatedParent, String initialBindingContext)
            {
                this.uv              = uv;
                this.view            = view;
                this.templatedParent = templatedParent;
                this.viewModelType   = viewModelType;

                if (!String.IsNullOrEmpty(initialBindingContext))
                {
                    PushBindingContext(initialBindingContext);
                }

                FindCompiledBindingExpressions();
            }

            /// <summary>
            /// Gets the property that implements the compiled version of the specified binding expression.
            /// </summary>
            /// <param name="expression">The text of the expression for which to retrieve an implementing property.</param>
            /// <returns>A <see cref="PropertyInfo"/> which represents the property that implements the compiled version of the specified binding expression,
            /// or <c>null</c> if the expression has no compiled equivalent.</returns>
            public PropertyInfo GetCompiledBindingExpression(String expression)
            {
                PropertyInfo property;
                if (compiledBindingExpressions.TryGetValue(expression, out property))
                {
                    return property;
                }
                return null;
            }

            /// <summary>
            /// Pushes a binding context expression onto the binding context stack and
            /// updates the value of the <see cref="BindingContext"/> property.
            /// </summary>
            /// <param name="bindingContext">The binding context expression to push onto the stack.</param>
            public void PushBindingContext(String bindingContext)
            {
                bindingContextStack.Push(bindingContext);
                GenerateBindingContext();
            }

            /// <summary>
            /// Pops a binding context expression off of the binding context stack
            /// and updates the value of the <see cref="BindingContext"/> property.
            /// </summary>
            public void PopBindingContext()
            {
                bindingContextStack.Pop();
                GenerateBindingContext();
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
            /// Gets or sets the current binding context. The binding context is prepended to all binding
            /// expressions within the instantiation context.
            /// </summary>
            public String BindingContext
            {
                get { return bindingContext; }
            }

            /// <summary>
            /// Gets the binding context that was most recently pushed onto the context stack.
            /// </summary>
            public String MostRecentBindingContext
            {
                get 
                {
                    if (bindingContextStack.Count > 0)
                    {
                        return bindingContextStack.Peek();
                    }
                    return null;
                }
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
                get { return view ?? (Object)templatedParent; }
            }

            /// <summary>
            /// Regenerates the value of the <see cref="BindingContext"/> property from the
            /// current binding context stack.
            /// </summary>
            private void GenerateBindingContext()
            {
                var exp = default(String);
                foreach (var context in bindingContextStack)
                {
                    exp = BindingExpressions.Combine(context, exp);
                }
                bindingContext = exp;
            }

            /// <summary>
            /// Finds all of the compiled binding expressions on the current view model and adds them to the context's registry.
            /// </summary>
            private void FindCompiledBindingExpressions()
            {
                if (viewModelType == null)
                    return;

                var properties = viewModelType.GetProperties().Where(x => x.Name.StartsWith("__UPF_Expression")).ToList();
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
                    compiledBindingExpressions.Add(prop.Expression, prop.Property);
                }
            }

            // Contains the property that implements each of the view model's compiled binding expressions.
            private readonly Dictionary<String, PropertyInfo> compiledBindingExpressions =
                new Dictionary<String, PropertyInfo>();

            // Property values.
            private readonly UltravioletContext uv;
            private PresentationFoundationView view;
            private DependencyObject templatedParent;
            private String bindingContext;
            private Type viewModelType;

            // State values.
            private readonly Stack<String> bindingContextStack = new Stack<String>();
        }
    }
}
