using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a command which uses delegates to implement the <see cref="ICommand.Execute"/> and <see cref="ICommand.CanExecute"/> methods.
    /// </summary>
    /// <typeparam name="TParameter">The command's parameter type.</typeparam>
    public sealed class DelegateCommand<TParameter> : DelegateCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{TParameter}"/> class.
        /// </summary>
        /// <param name="executeDelegate">The delegate which implements the command's <see cref="ICommand.Execute"/> method.</param>
        public DelegateCommand(Action<PresentationFoundationView, TParameter> executeDelegate)
            : this(executeDelegate, (view, parameter) => true)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{TParameter}"/> class.
        /// </summary>
        /// <param name="executeDelegate">The delegate which implements the command's <see cref="ICommand.Execute"/> method.</param>
        /// <param name="canExecuteDelegate">The delegate which implements the command's <see cref="ICommand.CanExecute"/> method.</param>
        public DelegateCommand(Action<PresentationFoundationView, TParameter> executeDelegate, Func<PresentationFoundationView, TParameter, Boolean> canExecuteDelegate)
        {
            Contract.Require(executeDelegate, nameof(executeDelegate));
            Contract.Require(canExecuteDelegate, nameof(canExecuteDelegate));

            if (!IsParameterReferenceTypeOrNullable())
                throw new InvalidCastException(PresentationStrings.DelegateCommandParamTypeMismatch);

            this.executeDelegate = executeDelegate;
            this.canExecuteDelegate = canExecuteDelegate;
        }
        
        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command does not require a parameter.</param>
        /// <returns><see langword="true"/> if the command can be executed; otherwise, <see langword="false"/>.</returns>
        public void Execute(PresentationFoundationView view, TParameter parameter)
        {
            executeDelegate(view, parameter);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command does not require a parameter.</param>
        public bool CanExecute(PresentationFoundationView view, TParameter parameter)
        {
            return canExecuteDelegate(view, parameter);
        }
        
        /// <inheritdoc/>
        protected override void Execute(PresentationFoundationView view, Object parameter) => executeDelegate(view, (TParameter)parameter);

        /// <inheritdoc/>
        protected override bool CanExecute(PresentationFoundationView view, Object parameter) => canExecuteDelegate(view, (TParameter)parameter);

        /// <summary>
        /// Gets a value indicating whether <typeparamref name="TParameter"/> meets the requirement of being either
        /// a reference type or Nullable{T}.
        /// </summary>
        private static Boolean IsParameterReferenceTypeOrNullable()
        {
            var type = typeof(TParameter);
            if (type.IsValueType)
            {
                if (type.IsGenericType && typeof(Nullable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        // State values.
        private readonly Action<PresentationFoundationView, TParameter> executeDelegate;
        private readonly Func<PresentationFoundationView, TParameter, Boolean> canExecuteDelegate;
    }
}
