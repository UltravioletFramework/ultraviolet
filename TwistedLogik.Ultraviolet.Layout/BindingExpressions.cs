using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Layout.Elements;

namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Represents a method which is used to retrieve the value of a data bound dependency property.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="model">The model object for the current binding context.</param>
    /// <returns>The current value of the bound property.</returns>
    internal delegate T DataBindingGetter<T>(Object model);

    /// <summary>
    /// Represents a method which is used to set the value of a data bound dependency property.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="model">The model object for the current binding context.</param>
    /// <param name="value">The value to set on the bound property.</param>
    internal delegate void DataBindingSetter<T>(Object model, T value);

    /// <summary>
    /// Contains methods for creating and manipulating binding expressions.
    /// </summary>
    internal partial class BindingExpressions
    {
        /// <summary>
        /// Gets a value indicating whether the specified string represents a binding expression.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns><c>true</c> if the specified string represents a binding expression; otherwise, <c>false</c>.</returns>
        public static Boolean IsBindingExpression(String expression, Boolean braces = true)
        {
            if (String.IsNullOrEmpty(expression))
                return false;

            return !braces || expression.StartsWith("{{") && expression.EndsWith("}}");
        }

        /// <summary>
        /// Creates a bound event delegate.
        /// </summary>
        /// <param name="uiElement">The element to which an event is being bound.</param>
        /// <param name="viewModelType">The type of view model to which the event is being bound.</param>
        /// <param name="delegateType">The type of delegate that handles the event being bound.</param>
        /// <param name="expression">The expression which represents the path to the event handler.</param>
        /// <returns>The bound event delegate that was created.</returns>
        public static Delegate CreateBoundEventDelegate(UIElement uiElement, Type viewModelType, Type delegateType, String expression)
        {
            Contract.Require(uiElement, "element");
            Contract.Require(viewModelType, "viewModelType");
            Contract.Require(delegateType, "delegateType");
            Contract.RequireNotEmpty(expression, "expression");

            var builder = new BoundEventBuilder(uiElement, delegateType, viewModelType, expression);
            return builder.Compile();
        }

        /// <summary>
        /// Creates a getter for the specified binding expression.
        /// </summary>
        /// <param name="viewModelType">The type of view model to which the value is being bound.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        /// <returns>A <see cref="DataBindingGetter{T}"/> that represents the specified model and expression.</returns>
        public static DataBindingGetter<T> CreateBindingGetter<T>(Type viewModelType, String expression)
        {
            Contract.Require(viewModelType, "viewModelType");
            Contract.RequireNotEmpty(expression, "expression");

            var builder = new DataBindingGetterBuilder<T>(viewModelType, expression);
            return builder.Compile();
        }

        /// <summary>
        /// Creates a setter for the specified binding expression.
        /// </summary>
        /// <param name="viewModelType">The type of view model to which the value is being bound.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        /// <returns>A <see cref="DataBindingSetter{T}"/> that represents the specified model and expression.</returns>
        public static DataBindingSetter<T> CreateBindingSetter<T>(Type viewModelType, String expression)
        {
            Contract.Require(viewModelType, "viewModelType");
            Contract.RequireNotEmpty(expression, "expression");

            var builder = new DataBindingSetterBuilder<T>(viewModelType, expression);
            return builder.Compile();
        }

        /// <summary>
        /// Parses the specified binding expression into its constituent components.
        /// </summary>
        /// <param name="expression">The binding expression to parse.</param>
        /// <param name="braces">A value indicating whether the expression's containing braces are included.</param>
        /// <returns>The specified binding expression's constituent components.</returns>
        public static IEnumerable<String> ParseBindingExpression(String expression, Boolean braces = true)
        {
            if (!IsBindingExpression(expression, braces))
                throw new ArgumentException(LayoutStrings.InvalidBindingExpression.Format(expression));

            var code       = braces ? expression.Substring("{{".Length, expression.Length - "{{}}".Length) : expression;
            var components = code.Split('.');

            return components;
        }
    }
}
