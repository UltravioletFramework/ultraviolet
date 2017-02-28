using TwistedLogik.Nucleus;
using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents a binding between a command and the event handlers which implement the command.
    /// </summary>
    public class CommandBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBinding"/> class.
        /// </summary>
        public CommandBinding() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBinding"/> class.
        /// </summary>
        /// <param name="command">The command that is being bound.</param>
        public CommandBinding(ICommand command)
            : this(command, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBinding"/> class.
        /// </summary>
        /// <param name="command">The command that is being bound.</param>
        /// <param name="executed">The handler for the command's <see cref="Executed"/> event.</param>
        public CommandBinding(ICommand command, ExecutedRoutedEventHandler executed)
            : this (command, executed, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBinding"/> class.
        /// </summary>
        /// <param name="command">The command that is being bound.</param>
        /// <param name="executed">The handler for the command's <see cref="Executed"/> event.</param>
        /// <param name="canExecute">The handler for the command's <see cref="CanExecute"/> event.</param>
        public CommandBinding(ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            Contract.Require(command, nameof(command));

            this.command = command;

            if (executed != null)
                Executed += executed;

            if (canExecute != null)
                CanExecute += canExecute;
        }

        /// <summary>
        /// Gets or sets the binding's associated command.
        /// </summary>
        public ICommand Command
        {
            get { return command; }
            set
            {
                Contract.Require(value, nameof(value));

                command = value;
            }
        }

        /// <summary>
        /// Occurs before the command is executed.
        /// </summary>
        public event ExecutedRoutedEventHandler PreviewExecuted;

        /// <summary>
        /// Occurs when the command is executed.
        /// </summary>
        public event ExecutedRoutedEventHandler Executed;

        /// <summary>
        /// Occurs before query to determine if the command can execute.
        /// </summary>
        public event CanExecuteRoutedEventHandler PreviewCanExecute;

        /// <summary>
        /// Occurs when querying to determine if the command can execute.
        /// </summary>
        public event CanExecuteRoutedEventHandler CanExecute;

        /// <summary>
        /// Raises the <see cref="PreviewExecuted"/> event.
        /// </summary>
        private void RaisePreviewExecuted(Object sender, ICommand command, Object parameter) =>
            PreviewExecuted?.Invoke(sender, command, parameter);

        /// <summary>
        /// Raises the <see cref="Executed"/> event.
        /// </summary>
        private void RaiseExecuted(Object sender, ICommand command, Object parameter) =>
            Executed?.Invoke(sender, command, parameter);

        /// <summary>
        /// Raises the <see cref="PreviewCanExecute"/> event.
        /// </summary>
        private void RaisePreviewCanExecute(Object sender, ICommand command, Object parameters, CanExecuteEventArgs args) =>
            PreviewCanExecute?.Invoke(sender, command, parameters, args);

        /// <summary>
        /// Raises the <see cref="CanExecute"/> event.
        /// </summary>
        private void RaiseCanExecute(Object sender, ICommand command, Object parameters, CanExecuteEventArgs args) =>
            CanExecute?.Invoke(sender, command, parameters, args);

        // Property values.
        private ICommand command;
    }
}
