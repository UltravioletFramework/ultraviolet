using System;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a command which uses delegates to implement the <see cref="ICommand.Execute"/> and <see cref="ICommand.CanExecute"/> methods.
    /// </summary>
    public abstract class DelegateCommandBase : ICommand
    {
        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        public void RaiseCanExecuteChanged() => OnCanExecuteChanged();
        
        /// <inheritdoc/>
        void ICommand.Execute(PresentationFoundationView view, Object parameter) => Execute(view, parameter);

        /// <inheritdoc/>
        bool ICommand.CanExecute(PresentationFoundationView view, Object parameter) => CanExecute(view, parameter);

        /// <summary>
        /// Occurs to indicate that the command's ability to execute has changed.
        /// </summary>
        public virtual event EventHandler CanExecuteChanged;
        
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command does not require a parameter.</param>
        protected abstract void Execute(PresentationFoundationView view, Object parameter);

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command does not require a parameter.</param>
        /// <returns><see langword="true"/> if the command can be executed; otherwise, <see langword="false"/>.</returns>
        protected abstract bool CanExecute(PresentationFoundationView view, Object parameter);

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        protected virtual void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
