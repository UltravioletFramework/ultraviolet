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
        /// <param name="dataSource">The data source to which the event is being bound.</param>
        /// <param name="dataSourceType">The type of the data source to which the expression is being bound.</param>
        /// <param name="delegateType">The type of delegate that will be created to bind to the event.</param>
        /// <param name="expression">The binding expression that represents the method to bind to the event.</param>
        public BoundEventBuilder(Object dataSource, Type dataSourceType, Type delegateType, String expression)
            : base(dataSourceType)
        {
            CreateParameters(delegateType);
            CreateReturnTarget();

            var components = BindingExpressions.ParseBindingExpression(expression, false).ToArray();
            var current    = AddDataSourceReference(expression, dataSource, dataSourceType);

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
        /// <param name="dataSource">The data source to which the event is being bound.</param>
        /// <param name="dataSourceType">The type of data source to which the event is being bound.</param>
        /// <returns>The current expression in the chain.</returns>
        private Expression AddDataSourceReference(String expression, Object dataSource, Type dataSourceType)
        {
            var dataSourceVariable = Expression.Variable(dataSource.GetType(), "dataSource");
            variables.Add(dataSourceVariable);

            var dataSourceAssignment = Expression.Assign(dataSourceVariable, Expression.Constant(dataSource));
            expressions.Add(dataSourceAssignment);

            AddNullCheck(dataSourceVariable);

            var view = dataSource as PresentationFoundationView;
            if (view != null)
            {
                var viewModelVariable = Expression.Variable(dataSourceType, "viewModel");
                variables.Add(viewModelVariable);

                var viewModelAssignment = Expression.Assign(viewModelVariable, 
                    Expression.Convert(Expression.Property(dataSourceVariable, "ViewModel"), dataSourceType));
                expressions.Add(viewModelAssignment);

                AddNullCheck(viewModelVariable);

                return viewModelVariable;
            }

            return dataSourceVariable;
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
