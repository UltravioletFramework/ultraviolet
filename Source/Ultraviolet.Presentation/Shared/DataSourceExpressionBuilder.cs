using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an object which is used to build expression trees associated with a particular data source.
    /// </summary>
    internal abstract class ExpressionBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionBuilder"/> class.
        /// </summary>
        /// <param name="dataSourceType">The type of the data source to which the value is being bound.</param>
        protected ExpressionBuilder(Type dataSourceType)
        {
            this.dataSourceType = dataSourceType;
        }

        /// <summary>
        /// Adds a safe reference to the specified component. If accessing the component would result
        /// in a <see cref="NullReferenceException"/>, the getter will return a default value.
        /// </summary>
        /// <param name="bindingExpression">The binding expression being converted into a delegate.</param>
        /// <param name="current">The current expression in the chain.</param>
        /// <param name="component">The component to which to add a reference.</param>
        /// <param name="conversion">The type to which to convert the reference, if any.</param>
        /// <returns>The variable expression that contains the safe reference.</returns>
        protected Expression AddSafeReference(String bindingExpression, Expression current, String component, Type conversion = null)
        {
            Expression reference;
            try
            {
                reference = Expression.PropertyOrField(current, component);
            }
            catch (ArgumentException e)
            {
                throw new InvalidOperationException(PresentationStrings.CannotResolveBindingExpression.Format(bindingExpression), e);
            }

            if (conversion != null)
            {
                reference = Expression.Convert(reference, conversion);
            }

            var variable = Expression.Variable(reference.Type, "var" + variables.Count);
            variables.Add(variable);

            var assignment = Expression.Assign(variable, reference);
            expressions.Add(assignment);

            if (reference.Type.IsClass)
            {
                AddNullCheck(variable);
            }

            return variable;
        }

        /// <summary>
        /// Adds a null check on the specified variable. If the variable is null, the lambda
        /// will immediately return a default value.
        /// </summary>
        /// <param name="variable">The variable to check for nullity.</param>
        protected void AddNullCheck(ParameterExpression variable)
        {
            var nullCondition = Expression.Equal(variable, Expression.Constant(null));
            var nullCheck = Expression.IfThen(nullCondition, defaultReturnExpression);

            expressions.Add(nullCheck);
        }

        /// <summary>
        /// Creates the lambda's return target label with void type.
        /// </summary>
        protected void CreateReturnTarget()
        {
            exitTarget = Expression.Label("exit");
            exitLabel = Expression.Label(exitTarget);
            defaultReturnExpression = Expression.Return(exitTarget);
        }

        /// <summary>
        /// Creates the lambda's return target label with the specified type.
        /// </summary>
        /// <param name="type">The target's type.</param>
        protected void CreateReturnTarget(Type type)
        {
            exitTarget = Expression.Label(type, "exit");
            exitLabel = Expression.Label(exitTarget, Expression.Default(type));
            defaultReturnExpression = Expression.Return(exitTarget, Expression.Default(type));
        }

        /// <summary>
        /// Adds a return label with void type to the lambda.
        /// </summary>
        protected void AddReturnLabel()
        {
            expressions.Add(exitLabel);
        }

        /// <summary>
        /// Adds a return expression with no value.
        /// </summary>
        protected void AddReturn()
        {
            expressions.Add(Expression.Return(exitTarget));
        }

        /// <summary>
        /// Adds a return expression with the specified value.
        /// </summary>
        /// <param name="value">The expression that represents the return value.</param>
        protected void AddReturn(Expression value)
        {
            expressions.Add(Expression.Return(exitTarget, value));
        }

        // State values.
        protected readonly Type dataSourceType;
        protected readonly List<Expression> expressions = new List<Expression>();
        protected readonly List<ParameterExpression> variables = new List<ParameterExpression>();
        protected readonly List<ParameterExpression> parameters = new List<ParameterExpression>();
        private LabelTarget exitTarget;
        private LabelExpression exitLabel;
        private Expression defaultReturnExpression;
    }
}
