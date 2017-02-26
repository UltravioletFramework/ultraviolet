using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command
        /// does not require a parameter.</param>
        /// <returns><see langword="true"/> if the command can be executed; otherwise, <see langword="false"/>.</returns>
        Boolean CanExecute(Object parameter);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The command parameter, or <see langword="null"/> if the command
        /// does not require a parameter.</param>
        void Execute(Object parameter);

        /// <summary>
        /// Occurs to indicate that the command's ability to execute has changed.
        /// </summary>
        event EventHandler CanExecuteChanged;
    }
}
