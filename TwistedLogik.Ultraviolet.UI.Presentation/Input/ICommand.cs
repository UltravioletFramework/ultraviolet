using System;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command does not require a parameter.</param>
        /// <returns><see langword="true"/> if the command can be executed; otherwise, <see langword="false"/>.</returns>
        Boolean CanExecute(PresentationFoundationView view, Object parameter);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="view">The view within which the command is being executed.</param>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command does not require a parameter.</param>
        void Execute(PresentationFoundationView view, Object parameter);

        /// <summary>
        /// Occurs to indicate that the command's ability to execute has changed.
        /// </summary>
        event EventHandler CanExecuteChanged;
    }
}
