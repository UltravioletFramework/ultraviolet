using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Represents an object which builds instances of <see cref="DataBindingSetter{T}"/>.
    /// </summary>
    internal sealed class DataBindingSetterBuilder<T> : BindingExpressionBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBindingSetterBuilder{T}"/> class.
        /// </summary>
        /// <param name="viewModelType">The type of view model to which the value is being bound.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        public DataBindingSetterBuilder(Type viewModelType, String expression)
            : base(viewModelType)
        {
            CreateReturnTarget();

            var components = BindingExpressions.ParseBindingExpression(expression).ToArray();
            var current    = AddDataSourceReference();
            var value      = AddValueParameter();

            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];

                if (i + 1 < components.Length)
                {
                    current = AddSafeReference(current, component);
                }
                else
                {
                    if (current.Type.IsValueType)
                        throw new InvalidOperationException(LayoutStrings.BindingAssignmentToValueType.Format(expression));

                    if (!AddValueAssignment(current, value, component))
                    {
                        return;
                    }
                }
            }

            AddReturn();
            AddReturnLabel();

            var lambdaBody = Expression.Block(variables, expressions);
            var lambda     = Expression.Lambda<DataBindingSetter<T>>(lambdaBody, parameters);

            lambdaExpression = lambda;
        }

        /// <summary>
        /// Compiles a new instance of the <see cref="DataBindingSetter{T}"/> delegate type.
        /// </summary>
        /// <returns>The <see cref="DataBindingGetter{T}"/> that was compiled.</returns>
        public DataBindingSetter<T> Compile()
        {
            return (lambdaExpression == null) ? null : lambdaExpression.Compile();
        }

        /// <summary>
        /// Adds a reference to the data source. If accessing the data source would
        /// result in a <see cref="NullReferenceException"/>, the getter will return a default value.
        /// </summary>
        /// <returns>The current expression in the chain.</returns>
        private Expression AddDataSourceReference()
        {
            var dataSourceParam = Expression.Parameter(typeof(Object), "dataSource");
            parameters.Add(dataSourceParam);

            var variable = Expression.Variable(viewModelType, "var0");
            variables.Add(variable);

            var assignment = Expression.Assign(variable, Expression.Convert(dataSourceParam, viewModelType));
            expressions.Add(assignment);

            AddNullCheck(dataSourceParam);

            return variable;
        }

        /// <summary>
        /// Adds the parameter through which the value to set is passed.
        /// </summary>
        /// <returns>The parameter expression that was added.</returns>
        private Expression AddValueParameter()
        {
            var parameter = Expression.Parameter(typeof(T), "value");
            parameters.Add(parameter);

            return parameter;
        }

        /// <summary>
        /// Adds the expression which assigns the value to the bound property.
        /// </summary>
        /// <param name="current">The current expression in the chain.</param>
        /// <param name="value">The value parameter expression.</param>
        /// <param name="component">The next component in the chain.</param>
        /// <returns><c>true</c> if the assignment was added; otherwise, <c>false</c>.</returns>
        private Boolean AddValueAssignment(Expression current, Expression value, String component)
        {
            var memberExpression = Expression.PropertyOrField(current, component);
            if (memberExpression.Member.MemberType == MemberTypes.Property)
            {
                var memberProperty = (PropertyInfo)memberExpression.Member;
                if (!memberProperty.CanWrite)
                {
                    return false;
                }
            }

            expressions.Add(Expression.Assign(memberExpression, Expression.Convert(value, memberExpression.Type)));
            return true;
        }

        // State values.
        private readonly Expression<DataBindingSetter<T>> lambdaExpression;
    }
}