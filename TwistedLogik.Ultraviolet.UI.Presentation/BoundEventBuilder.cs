using System;
using System.Linq;
using System.Linq.Expressions;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents an object which builds event delegates for bound events.
    /// </summary>
    internal sealed class BoundEventBuilder : BindingExpressionBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundEventBuilder"/> type.
        /// </summary>
        /// <param name="uiElement">The interface element which provides the event's data source, if any.</param>
        /// <param name="delegateType">The type of delegate that will be created to bind to the event.</param>
        /// <param name="dataSourceType">The type of the data source to which the expression is being bound.</param>
        /// <param name="expression">The binding expression that represents the method to bind to the event.</param>
        /// <param name="bindToElement">A value indicating whether to bind events to the element object, rather than the view model.</param>
        public BoundEventBuilder(UIElement uiElement, Type dataSourceType, Type delegateType, String expression, Boolean bindToElement)
            : base(dataSourceType)
        {
            CreateParameters(delegateType);
            CreateReturnTarget();

            var components = BindingExpressions.ParseBindingExpression(expression, false).ToArray();
            var current    = AddDataSourceReference(expression, uiElement, bindToElement);

            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];

                if (i + 1 < components.Length)
                {
                    current = AddSafeReference(expression, current, component);
                }
                else
                {
                    current = AddMethodInvocation(expression, current, component);
                }
            }

            AddReturn();
            AddReturnLabel();

            var lambdaBody = Expression.Block(variables, expressions);
            var lambda     = Expression.Lambda(delegateType, lambdaBody, parameters);

            lambdaExpression = lambda;
        }

        /// <summary>
        /// Compiles an event delegate.
        /// </summary>
        /// <returns>The event delegate that was compiled.</returns>
        public Delegate Compile()
        {
            return (lambdaExpression == null) ? null : lambdaExpression.Compile();
        }

        /// <summary>
        /// Creates the event's parameter expressions.
        /// </summary>
        /// <param name="delegateType">The event's delegate type.</param>
        private void CreateParameters(Type delegateType)
        {
            var invoke                 = delegateType.GetMethod("Invoke");
            var invokeParams           = invoke.GetParameters();
            var invokeParamExpressions = invokeParams.Select(x => Expression.Parameter(x.ParameterType, x.Name));

            foreach (var invokeParamExpression in invokeParamExpressions)
            {
                parameters.Add(invokeParamExpression);
            }
        }

        /// <summary>
        /// Adds a reference to the data source. If accessing the data source would
        /// result in a <see cref="NullReferenceException"/>, the getter will return a default value.
        /// </summary>
        /// <param name="expression">The binding expression which is being evaluated.</param>
        /// <param name="uiElement">The element to which the event is being bound.</param>
        /// <param name="bindToElement">A value indicating whether to bind to the element, rather than the view.</param>
        /// <returns>The current expression in the chain.</returns>
        private Expression AddDataSourceReference(String expression, UIElement uiElement, Boolean bindToElement)
        {
            if (bindToElement)
            {
                return AddElementReference(uiElement);
            }
            return AddViewModelReference(expression, uiElement);
        }

        /// <summary>
        /// Adds a reference to the view model which contains the bound event method.
        /// </summary>
        /// <param name="expression">The binding expression which is being evaluated.</param>
        /// <param name="uiElement">The element to which an event is being bound.</param>
        /// <returns>The current expression in the chain.</returns>
        private Expression AddViewModelReference(String expression, UIElement uiElement)
        {
            var elementVariable = Expression.Variable(typeof(UIElement), "uiElement");
            variables.Add(elementVariable);

            var elementAssignment = Expression.Assign(elementVariable, Expression.Constant(uiElement));
            expressions.Add(elementAssignment);

            AddNullCheck(elementVariable);

            var refView      = AddSafeReference(expression, elementVariable, "View");
            var refViewModel = AddSafeReference(expression, refView, "ViewModel", dataSourceType);

            return refViewModel;
        }

        /// <summary>
        /// Adds a reference to the element which contains the bound event method.
        /// </summary>
        /// <param name="uiElement">The element to which an event is being bound.</param>
        /// <returns>The current expression in the chain.</returns>
        private Expression AddElementReference(UIElement uiElement)
        {
            var userControlVariable = Expression.Variable(uiElement.GetType(), "uiElement");
            variables.Add(userControlVariable);

            var userControlAssignment = Expression.Assign(userControlVariable, Expression.Constant(uiElement));
            expressions.Add(userControlAssignment);

            AddNullCheck(userControlVariable);

            return userControlVariable;
        }

        /// <summary>
        /// Adds the bound method invocation to the lambda.
        /// </summary>
        /// <param name="expression">The binding expression which is being evaluated.</param>
        /// <param name="current">The current expression in the chain.</param>
        /// <param name="component">The next component in the chain.</param>
        /// <returns>The method invocation expression.</returns>
        private Expression AddMethodInvocation(String expression, Expression current, String component)
        {
            Expression invocation;
            try
            {
                invocation = Expression.Call(current, component, Type.EmptyTypes, parameters.ToArray());
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(PresentationStrings.CannotResolveBindingExpression.Format(expression), e);
            }

            expressions.Add(invocation);
            return invocation;
        }

        // State values.
        private readonly LambdaExpression lambdaExpression;
    }
}
