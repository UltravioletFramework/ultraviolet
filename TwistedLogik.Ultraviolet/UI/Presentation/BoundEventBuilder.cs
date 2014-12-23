using System;
using System.Linq;
using System.Linq.Expressions;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;

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
        /// <param name="uiElement">The interface element which provides the event's data source.</param>
        /// <param name="viewModelType">The type of view model that is referenced by the current view.</param>
        /// <param name="delegateType">The type of delegate that will be created to bind to the event.</param>
        /// <param name="expression">The binding expression that represents the method to bind to the event.</param>
        public BoundEventBuilder(UIElement uiElement, Type viewModelType, Type delegateType, String expression)
            : this(uiElement, null, delegateType, viewModelType, expression)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundEventBuilder"/> type.
        /// </summary>
        /// <param name="userControl">The user control which provides the event's data source.</param>
        /// <param name="viewModelType">The type of view model that is referenced by the current view.</param>
        /// <param name="delegateType">The type of delegate that will be created to bind to the event.</param>
        /// <param name="expression">The binding expression that represents the method to bind to the event.</param>
        public BoundEventBuilder(UserControl userControl, Type viewModelType, Type delegateType, String expression)
            : this(null, userControl, delegateType, viewModelType, expression)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundEventBuilder"/> type.
        /// </summary>
        /// <param name="uiElement">The interface element which provides the event's data source, if any.</param>
        /// <param name="userControl">The user control which provides the event's data source, if any.</param>
        /// <param name="viewModelType">The type of view model that is referenced by the current view.</param>
        /// <param name="delegateType">The type of delegate that will be created to bind to the event.</param>
        /// <param name="expression">The binding expression that represents the method to bind to the event.</param>
        private BoundEventBuilder(UIElement uiElement, UserControl userControl, Type delegateType, Type viewModelType, String expression)
            : base(viewModelType)
        {
            CreateParameters(delegateType);
            CreateReturnTarget();

            var components = BindingExpressions.ParseBindingExpression(expression, false).ToArray();
            var current    = AddDataSourceReference(uiElement, userControl);

            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];

                if (i + 1 < components.Length)
                {
                    current = AddSafeReference(current, component);
                }
                else
                {
                    current = AddMethodInvocation(current, component);
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
        /// <param name="uiElement">The element to which the event is being bound, if any.</param>
        /// <param name="userControl">The user control to which the event is being bound, if any.</param>
        /// <returns>The current expression in the chain.</returns>
        private Expression AddDataSourceReference(UIElement uiElement, UserControl userControl)
        {
            if (userControl != null)
            {
                return AddUserControlReference(userControl);
            }
            return AddViewModelReference(uiElement);
        }

        /// <summary>
        /// Adds a reference to the view model which contains the bound event method.
        /// </summary>
        /// <param name="uiElement">The element to which an event is being bound.</param>
        /// <returns>The current expression in the chain.</returns>
        private Expression AddViewModelReference(UIElement uiElement)
        {
            var elementVariable = Expression.Variable(typeof(UIElement), "uiElement");
            variables.Add(elementVariable);

            var elementAssignment = Expression.Assign(elementVariable, Expression.Constant(uiElement));
            expressions.Add(elementAssignment);

            AddNullCheck(elementVariable);

            var refView      = AddSafeReference(elementVariable, "View");
            var refViewModel = AddSafeReference(refView, "ViewModel", viewModelType);

            return refViewModel;
        }

        /// <summary>
        /// Adds a reference to the user control which contains the bound event method.
        /// </summary>
        /// <param name="userControl">The user control to which an event is being bound.</param>
        /// <returns>The current expression in the chain.</returns>
        private Expression AddUserControlReference(UserControl userControl)
        {
            var userControlVariable = Expression.Variable(userControl.GetType(), "userControl");
            variables.Add(userControlVariable);

            var userControlAssignment = Expression.Assign(userControlVariable, Expression.Constant(userControl));
            expressions.Add(userControlAssignment);

            AddNullCheck(userControlVariable);

            return userControlVariable;
        }

        /// <summary>
        /// Adds the bound method invocation to the lambda.
        /// </summary>
        /// <param name="current">The current expression in the chain.</param>
        /// <param name="component">The next component in the chain.</param>
        /// <returns>The method invocation expression.</returns>
        private Expression AddMethodInvocation(Expression current, String component)
        {
            var invocation = Expression.Call(current, component, Type.EmptyTypes, parameters.ToArray());
            expressions.Add(invocation);

            return invocation;
        }

        // State values.
        private readonly LambdaExpression lambdaExpression;
    }
}
