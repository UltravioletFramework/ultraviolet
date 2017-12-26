using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a command which uses delegates to implement the <see cref="ICommand.Execute"/> and <see cref="ICommand.CanExecute"/> methods.
    /// </summary>
    public sealed class DelegateCommand : DelegateCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{TParameter}"/> class.
        /// </summary>
        /// <param name="executeDelegate">The delegate which implements the command's <see cref="ICommand.Execute"/> method.</param>
        public DelegateCommand(Action<PresentationFoundationView> executeDelegate)
            : this(executeDelegate, (view) => true)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{TParameter}"/> class.
        /// </summary>
        /// <param name="executeDelegate">The delegate which implements the command's <see cref="ICommand.Execute"/> method.</param>
        /// <param name="canExecuteDelegate">The delegate which implements the command's <see cref="ICommand.CanExecute"/> method.</param>
        public DelegateCommand(Action<PresentationFoundationView> executeDelegate, Func<PresentationFoundationView, Boolean> canExecuteDelegate)
        {
            Contract.Require(executeDelegate, nameof(executeDelegate));
            Contract.Require(canExecuteDelegate, nameof(canExecuteDelegate));

            this.executeDelegate = executeDelegate;
            this.canExecuteDelegate = canExecuteDelegate;
        }

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <returns><see langword="true"/> if the command can be executed; otherwise, <see langword="false"/>.</returns>
        public void Execute(PresentationFoundationView view) => Execute(view, null);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        public bool CanExecute(PresentationFoundationView view) => CanExecute(view, null);

        /// <inheritdoc/>
        protected override void Execute(PresentationFoundationView view, Object parameter) => executeDelegate(view);

        /// <inheritdoc/>
        protected override bool CanExecute(PresentationFoundationView view, Object parameter) => canExecuteDelegate(view);

        // State values.
        private readonly Action<PresentationFoundationView> executeDelegate;
        private readonly Func<PresentationFoundationView, Boolean> canExecuteDelegate;
    }
}
