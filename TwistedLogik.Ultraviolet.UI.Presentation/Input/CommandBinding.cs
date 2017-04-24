using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a binding between a command and the event handlers which implement the command.
    /// </summary>
    [UvmlKnownType]
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
        public CommandBinding(ICommand command, UpfExecutedRoutedEventHandler executed)
            : this (command, executed, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBinding"/> class.
        /// </summary>
        /// <param name="command">The command that is being bound.</param>
        /// <param name="executed">The handler for the command's <see cref="Executed"/> event.</param>
        /// <param name="canExecute">The handler for the command's <see cref="CanExecute"/> event.</param>
        public CommandBinding(ICommand command, UpfExecutedRoutedEventHandler executed, UpfCanExecuteRoutedEventHandler canExecute)
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
        public event UpfExecutedRoutedEventHandler PreviewExecuted;

        /// <summary>
        /// Occurs when the command is executed.
        /// </summary>
        public event UpfExecutedRoutedEventHandler Executed;

        /// <summary>
        /// Occurs before query to determine if the command can execute.
        /// </summary>
        public event UpfCanExecuteRoutedEventHandler PreviewCanExecute;

        /// <summary>
        /// Occurs when querying to determine if the command can execute.
        /// </summary>
        public event UpfCanExecuteRoutedEventHandler CanExecute;

        /// <summary>
        /// Called when the command binding is executed or queried to determine whether it can execute.
        /// </summary>
        internal void HandleExecutedOrCanExecute(DependencyObject element, ICommand command, Object parameter, RoutedEventData data, Boolean preview)
        {
            if (data is CanExecuteRoutedEventData)
            {
                HandleCanExecute(element, command, parameter, (CanExecuteRoutedEventData)data, preview);
            }
            else
            {
                HandleExecuted(element, command, parameter, (ExecutedRoutedEventData)data, preview);
            }
        }

        /// <summary>
        /// Called when the command binding is executed.
        /// </summary>
        internal void HandleExecuted(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data, Boolean preview)
        {
            if (data.Handled)
                return;

            var handler = preview ? PreviewExecuted : Executed;
            if (handler != null)
            {
                var canExecuteData = CanExecuteRoutedEventData.Retrieve(data.Source as DependencyObject, autorelease: false);
                try
                {
                    HandleCanExecute(element, command, parameter, canExecuteData, false);
                    if (canExecuteData.CanExecute)
                    {
                        handler(element, command, parameter, data);
                        data.Handled = true;
                    }
                }
                finally
                {
                    canExecuteData.Release();
                }
            }
        }

        /// <summary>
        /// Called when the command binding is being queried to determine whether it can execute.
        /// </summary>
        internal void HandleCanExecute(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data, Boolean preview)
        {
            if (data.Handled)
                return;

            if (preview)
            {
                if (PreviewCanExecute != null)
                {
                    PreviewCanExecute(element, command, parameter, data);

                    if (data.CanExecute)
                        data.Handled = true;
                }
            }
            else
            {
                if (CanExecute != null)
                {
                    CanExecute(element, command, parameter, data);

                    if (data.CanExecute)
                        data.Handled = true;
                }
                else
                {
                    if (!data.CanExecute && Executed != null)
                    {
                        data.CanExecute = true;
                        data.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="PreviewExecuted"/> event.
        /// </summary>
        private void RaisePreviewExecuted(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data) =>
            PreviewExecuted?.Invoke(element, command, parameter, data);

        /// <summary>
        /// Raises the <see cref="Executed"/> event.
        /// </summary>
        private void RaiseExecuted(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data) =>
            Executed?.Invoke(element, command, parameter, data);

        /// <summary>
        /// Raises the <see cref="PreviewCanExecute"/> event.
        /// </summary>
        private void RaisePreviewCanExecute(DependencyObject element, ICommand command, Object parameters, CanExecuteRoutedEventData data) =>
            PreviewCanExecute?.Invoke(element, command, parameters, data);

        /// <summary>
        /// Raises the <see cref="CanExecute"/> event.
        /// </summary>
        private void RaiseCanExecute(DependencyObject element, ICommand command, Object parameters, CanExecuteRoutedEventData data) =>
            CanExecute?.Invoke(element, command, parameters, data);

        // Property values.
        private ICommand command;
    }
}
