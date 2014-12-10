using System;
using System.Linq.Expressions;

namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Represents an object which builds instances of <see cref="DataBindingGetter{T}"/>.
    /// </summary>
    /// <typeparam name="T"
    internal sealed class DataBindingGetterBuilder<T> : BindingExpressionBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBindingGetterBuilder{T}"/> class.
        /// </summary>
        /// <param name="viewModelType">The type of view model to which the value is being bound.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        public DataBindingGetterBuilder(Type viewModelType, String expression)
            : base(viewModelType)
        {
            CreateReturnTarget<T>();

            var components = BindingExpressions.ParseBindingExpression(expression);
            var current    = AddDataSourceReference();

            foreach (var component in components)
            {
                current = AddSafeReference(current, component);
            }

            var result = Expression.Convert(current, typeof(T));

            AddReturn(result);
            AddReturnLabel<T>();

            var lambdaBody = Expression.Block(variables, expressions);
            var lambda     = Expression.Lambda<DataBindingGetter<T>>(lambdaBody, parameters);

            lambdaExpression = lambda;
        }

        /// <summary>
        /// Compiles a new instance of the <see cref="DataBindingGetter{T}"/> delegate type.
        /// </summary>
        /// <returns>The <see cref="DataBindingGetter{T}"/> that was compiled.</returns>
        public DataBindingGetter<T> Compile()
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

        // State values.
        private readonly Expression<DataBindingGetter<T>> lambdaExpression;
    }
}
