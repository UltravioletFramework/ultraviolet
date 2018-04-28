using System;
using System.Linq.Expressions;
using System.Reflection;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an object which builds instances of <see cref="DataBindingSetter{T}"/>.
    /// </summary>
    internal sealed class DataBindingSetterBuilder : ExpressionBuilder
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
            this.boundType = expressionType;
            this.delegateType = typeof(DataBindingSetter<>).MakeGenericType(expressionType);

            if (UltravioletPlatformInfo.IsRuntimeCodeGenerationSupported())
            {
                CreateReturnTarget();

                var path = BindingExpressions.GetBindingMemberPathPart(expression);
                var pathParts = path.Contains(".") ? path.Split('.') : null;
                var pathFinalPart = (pathParts == null) ? path : pathParts[pathParts.Length - 1];

                var current = AddDataSourceReference();
                var value = AddValueParameter();

                if (current.Type.IsValueType)
                    return;

                if (pathParts != null)
                {
                    for (int i = 0; i < pathParts.Length - 1; i++)
                        current = AddSafeReference(expression, current, pathParts[i]);
                }

                if (!AddValueAssignment(current, value, pathFinalPart))
                    return;

                AddReturn();
                AddReturnLabel();

                var lambdaBody = Expression.Block(variables, expressions);
                var lambda = Expression.Lambda(delegateType, lambdaBody, parameters);

                lambdaExpression = lambda;
            }
            else
            {
                var expParamDataSource = Expression.Parameter(typeof(Object), "dataSource");
                var expParamValue = Expression.Parameter(boundType, "value");

                var implMethod = typeof(DataBindingSetterBuilder).GetMethod(nameof(ReflectionBasedImplementation),
                    BindingFlags.NonPublic | BindingFlags.Static);

                var path = BindingExpressions.GetBindingMemberPathPart(expression);
                var pathParts = path.Contains(".") ? path.Split('.') : null;
                var pathFinalPart = (pathParts == null) ? path : pathParts[pathParts.Length - 1];

                var expDataSource = (Expression)Expression.Convert(expParamDataSource, dataSourceType);
                var expDataSourceType = dataSourceType;

                var current = dataSourceType;
                if (pathParts != null)
                {
                    for (int i = 0; i < pathParts.Length - 1; i++)
                    {
                        expDataSourceType = BindingExpressions.GetMemberType(BindingExpressions.FindPropertyOrField(expDataSourceType, pathParts[i]));
                        expDataSource = Expression.PropertyOrField(expDataSource, pathParts[i]);
                    }
                }

                var member = BindingExpressions.FindPropertyOrField(expDataSourceType, pathFinalPart);
                if (!BindingExpressions.CanWriteMember(member))
                    return;

                var expImplMethodCall = Expression.Call(implMethod,
                    Expression.Constant(member),
                    Expression.Convert(expDataSource, typeof(Object)),
                    Expression.Convert(expParamValue, typeof(Object)));

                lambdaExpression = Expression.Lambda(delegateType, expImplMethodCall, expParamDataSource, expParamValue);
            }
        }

        /// <summary>
        /// Compiles a new instance of the <see cref="DataBindingSetter{T}"/> delegate type.
        /// </summary>
        /// <returns>The <see cref="DataBindingSetter{T}"/> that was compiled.</returns>
        public Delegate Compile()
        {
            return lambdaExpression?.Compile();
        }

        /// <summary>
        /// Represents a reflection-based implementation of a binding expression setter which is
        /// used on platforms that don't support runtime code generation.
        /// </summary>
        private static void ReflectionBasedImplementation(PropertyInfo property, Object dataSource, Object value)
        {
            if (dataSource == null)
                return;

            var convertedValue = Convert.ChangeType(value, property.PropertyType);
            property.SetValue(dataSource, convertedValue, null);
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
        /// <returns><see langword="true"/> if the assignment was added; otherwise, <see langword="false"/>.</returns>
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