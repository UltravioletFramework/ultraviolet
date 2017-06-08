using System;
using System.Linq.Expressions;
using System.Reflection;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an object which builds instances of <see cref="DataBindingGetter{T}"/>.
    /// </summary>
    internal sealed class DataBindingGetterBuilder : ExpressionBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBindingGetterBuilder"/> class.
        /// </summary>
        /// <param name="expressionType">The type of the bound expression.</param>
        /// <param name="dataSourceType">The type of the data source to which the expression is being bound.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        public DataBindingGetterBuilder(Type expressionType, Type dataSourceType, String expression)
            : base(dataSourceType)
        {
            this.delegateType = typeof(DataBindingGetter<>).MakeGenericType(expressionType);

            if (UltravioletPlatformInfo.IsRuntimeCodeGenerationSupported())
            {
                CreateReturnTarget(expressionType);

                var path = BindingExpressions.GetBindingMemberPathPart(expression);
                var current = AddDataSourceReference();

                current = AddSafeReference(expression, current, path);

                var result = Expression.Convert(current, expressionType);

                AddReturn(result);
                AddReturnLabel();

                var lambdaBody = Expression.Block(variables, expressions);
                var lambda = Expression.Lambda(delegateType, lambdaBody, parameters);

                lambdaExpression = lambda;
            }
            else
            {
                var expParamDataSource = Expression.Parameter(typeof(Object), "dataSource");

                var implMethod = typeof(DataBindingGetterBuilder).GetMethod(nameof(ReflectionBasedImplementation),
                    BindingFlags.NonPublic | BindingFlags.Static);

                var path = BindingExpressions.GetBindingMemberPathPart(expression);
                var property = dataSourceType.GetProperty(path);

                if (!property.CanRead)
                    return;

                var expImplMethodCall = Expression.Call(implMethod,
                    Expression.Constant(property),
                    Expression.Convert(expParamDataSource, typeof(Object)));

                lambdaExpression = Expression.Lambda(delegateType,
                    Expression.Convert(
                        Expression.Convert(expImplMethodCall, property.PropertyType),
                    expressionType), expParamDataSource);
            }
        }

        /// <summary>
        /// Compiles a new instance of the <see cref="DataBindingGetter{T}"/> delegate type.
        /// </summary>
        /// <returns>The <see cref="DataBindingGetter{T}"/> that was compiled.</returns>
        public Delegate Compile()
        {
            return lambdaExpression?.Compile();
        }
        
        /// <summary>
        /// Represents a reflection-based implementation of a binding expression setter which is
        /// used on platforms that don't support runtime code generation.
        /// </summary>
        [Preserve]
        private static Object ReflectionBasedImplementation(PropertyInfo property, Object dataSource)
        {
            if (dataSource == null)
                return null;

            return property.GetValue(dataSource, null);
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

        // State values.
        private readonly Type delegateType;
        private readonly LambdaExpression lambdaExpression;
    }
}