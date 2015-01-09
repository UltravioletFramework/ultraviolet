using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents an object which builds instances of <see cref="DataBindingSetter{T}"/>.
    /// </summary>
    internal sealed class DataBindingSetterBuilder : BindingExpressionBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBindingSetterBuilder"/> class.
        /// </summary>
        /// <param name="expressionType">The type of the bound expression.</param>
        /// <param name="dataSourceType">The type of the data source to which the value is being bound.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        public DataBindingSetterBuilder(Type expressionType, Type dataSourceType, String expression)
            : base(dataSourceType)
        {
            this.boundType    = expressionType;
            this.delegateType = typeof(DataBindingSetter<>).MakeGenericType(expressionType);

            CreateReturnTarget();

            var components = BindingExpressions.ParseBindingExpression(expression).ToArray();
            var current    = AddDataSourceReference();
            var value      = AddValueParameter();

            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];

                if (i + 1 < components.Length)
                {
                    current = AddSafeReference(expression, current, component);
                }
                else
                {
                    if (current.Type.IsValueType)
                        return;

                    if (!AddValueAssignment(current, value, component))
                    {
                        return;
                    }
                }
            }

            AddReturn();
            AddReturnLabel();

            var lambdaBody = Expression.Block(variables, expressions);
            var lambda     = Expression.Lambda(delegateType, lambdaBody, parameters);

            lambdaExpression = lambda;
        }

        /// <summary>
        /// Compiles a new instance of the <see cref="DataBindingSetter{T}"/> delegate type.
        /// </summary>
        /// <returns>The <see cref="DataBindingGetter{T}"/> that was compiled.</returns>
        public Delegate Compile()
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

            var variable = Expression.Variable(dataSourceType, "var0");
            variables.Add(variable);

            var assignment = Expression.Assign(variable, Expression.Convert(dataSourceParam, dataSourceType));
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
            var parameter = Expression.Parameter(boundType, "value");
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
        private readonly Type boundType;
        private readonly Type delegateType;
        private readonly LambdaExpression lambdaExpression;
    }
}